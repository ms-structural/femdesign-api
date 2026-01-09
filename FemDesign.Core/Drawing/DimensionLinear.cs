using System;
using System.Collections.Generic;
using System.Linq;
using FemDesign.Geometry;
using FemDesign.GenericClasses;
using Struxml = StruSoft.Interop.StruXml.Data;

namespace FemDesign.Drawing
{
    /// <summary>
    /// Represents a Dimension Linear.
    /// </summary>
    public class DimensionLinear : EntityBase, IStructureElement
    {
        /// <value>
        /// Points to measure. Will measure between the points projection in the plane of the dimension line.
        /// </value>
        public List<FemDesign.Geometry.Point3d> ReferencePoints;

        /// <value>
        /// Plane of dimension. Dimension line will be placed at origin of plane.
        /// </value>
        public Plane Plane;

        /// <value>
        /// Dimension text font
        /// </value>
        public Struxml.Font_type Font;


        /// <value>
        /// Dimension line arrow
        /// </value>
        public Struxml.Arrow_type Arrow;

        /// <value>
        /// Number of decimals of measurement
        /// </value>
        public int Decimals = new Struxml.Dimtext_type().Decimals;

        /// <value>
        /// Measurement unit
        /// </value>
        public Struxml.Lengthunit_type LengthUnit = Struxml.Lengthunit_type.M;

        /// <value>
        /// Show unit after measurement
        /// </value>
        public bool ShowUnit = false;

        /// <value>
        /// Returns the distances between the reference points measured along the plane x-axis.
        /// </value>
        public List<double> Distances
        {
            get
            {
                var dims = new List<double>();
                for (int idx = 0; idx < ReferencePoints.Count; idx++)
                {
                    if (idx != 0)
                    {
                        FemDesign.Geometry.Point3d p1 = ReferencePoints[idx - 1];
                        FemDesign.Geometry.Point3d p2 = ReferencePoints[idx];
                        Vector3d v = p2 - p1;
                        dims.Add(v.Dot(Plane.LocalX));
                    }
                }
                return dims;
            }
        }

        /// <value>
        /// Returns the dimtext type for the first measurement in the chain, will be used for a following measures as well.
        /// Will be used to interpret the dimtext options such as decimals and length unit.
        /// </value>
        public Struxml.Dimtext_type DimtextType
        {
            get
            {
                var dimTextType = new Struxml.Dimtext_type
                {
                    Value = 0.0, // if set to 0.0 FD will interpret this as a request to calculate the measurement.
                    Decimals = this.Decimals,
                    Length_unit = this.LengthUnit,
                    Measurement_unit = this.ShowUnit,
                    Position = new Struxml.Point_type_3d(), // empty point 3d type, fd will disregard if
                    Plane_x = new Struxml.Point_type_3d(), // empty point 3d type, fd will disregard if empty.
                    Plane_y = new Struxml.Point_type_3d() // empty point 3d type, fd will disregard if empty.
                };
                return dimTextType;
            }
        }

        /// <value>
        /// Get action as Modification_type
        /// </value>
        public Struxml.Modification_type StruxmlAction
        {
            get
            {
                Struxml.Modification_type res;
                System.Enum.TryParse<Struxml.Modification_type>(Action, out res);
                return res;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"DimensionLinear: O: {Plane.Origin}, X: {Plane.LocalX}, Measures: {string.Join(" ", Distances.Select(x => Math.Round(x, Decimals)))}";
        }

        /// <summary>
        /// Initialise.
        /// </summary>
        public void Initialise()
        {
            EntityCreated();
            Font = new Struxml.Font_type();
            Arrow = new Struxml.Arrow_type();
        }

        /// <summary>
        /// Construct a new linear dimension from reference points and the plane of the dimension.
        /// </summary>   
        /// <param name="referencePoints">the reference points.</param>
        /// <param name="dimPlane">the dim plane.</param>
        public DimensionLinear(List<FemDesign.Geometry.Point3d> referencePoints, Plane dimPlane)
        {
            Initialise();
            ReferencePoints = referencePoints;
            Plane = dimPlane;
        }

        public static implicit operator Struxml.Dimline_type(DimensionLinear d) => new Struxml.Dimline_type
        {
            Guid = d.Guid.ToString(),
            Action = d.StruxmlAction,
            Last_change = d.LastChange,
            Point = d.ReferencePoints.Select(x => (Struxml.Point_type_3d)x).ToList(),
            Position = d.Plane.Origin,
            Plane_x = d.Plane.LocalX,
            Plane_y = d.Plane.LocalY,
            Dimension_line = new Struxml.Dimdimline_type { },
            Extension_line = new Struxml.Extline_type
            {
                Extension_a = 0.005,
                Extension_b = 0.005,
                Offset_c = 0.005
            },
            Arrow = d.Arrow,
            Font = d.Font,
            Text = new List<Struxml.Dimtext_type>{d.DimtextType}
        };
    }
}
