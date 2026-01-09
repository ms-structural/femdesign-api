using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using FemDesign.GenericClasses;
using Newtonsoft.Json;
using FemDesign.Calculate;

namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "Nodal displacements" result
    /// </summary>
    [Result(typeof(NodalVibration), ListProc.NodalVibrationShape)]
    public partial class NodalVibration : IResult
    {
        /// <summary>
        /// Support name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Finite element node id
        /// </summary>
        public int NodeId { get; }
        /// <summary>
        /// Displacement in global x
        /// </summary>
        public double Ex { get; }
        /// <summary>
        /// Displacement in global y
        /// </summary>
        public double Ey { get; }
        /// <summary>
        /// Displacement in global z
        /// </summary>
        public double Ez { get; }
        /// <summary>
        /// Rotation around global x
        /// </summary>
        public double Fix { get; }
        /// <summary>
        /// Rotation around global y
        /// </summary>
        public double Fiy { get; }
        /// <summary>
        /// Rotation around global z
        /// </summary>
        public double Fiz { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public int ShapeId { get; }

        [JsonConstructor]
        internal NodalVibration(string id, int nodeId, double ex, double ey, double ez, double fix, double fiy, double fiz, int shapeId)
        {
            this.Id = id;
            this.NodeId = nodeId;
            this.Ex = ex;
            this.Ey = ey;
            this.Ez = ez;
            this.Fix = fix;
            this.Fiy = fiy;
            this.Fiz = fiz;
            this.ShapeId = shapeId;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        //Nodal vibration shapes, 3
        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Nodal vibration) shapes,\s*(?'shapeid'\d*?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Nodal vibration) shapes,\s*(?'shapeid'\d*?)(?: - selected objects)?$|^ID\tNode\tex\tey\tez\t\u03c6 x\t\u03c6 y\t\u03c6 z|^\[.*\]");
            }
        }

        internal static NodalVibration Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string identifier = row[0];
            int nodeId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
            double ex = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double ey = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double ez = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double fix = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double fiy = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double fiz = Double.Parse(row[7], CultureInfo.InvariantCulture);
            int shapeId = Int32.Parse(HeaderData["shapeid"]);
            return new NodalVibration(identifier, nodeId, ex, ey, ez, fix, fiy, fiz, shapeId);
        }

        /// <summary>
        /// The method has been created for returning the value for Grasshopper and Dynamo.
        /// The method can still be use for C# users.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="Result">the result.</param>
        /// <param name="ModeShapeId">the mode shape id.</param>
        public static Dictionary<string, object> DeconstructNodalVibration(List<FemDesign.Results.NodalVibration> Result, string ModeShapeId)
        {
            var nodalDisplacements = Result.Cast<FemDesign.Results.NodalVibration>();

            // Return the unique load cases - load combinations
            var uniqueShapeId = nodalDisplacements.Select(n => n.ShapeId.ToString()).Distinct().ToList();

            // Select the Nodal Displacement for the selected Load Case - Load Combination
            if (uniqueShapeId.Contains(ModeShapeId, StringComparer.OrdinalIgnoreCase))
            {
                nodalDisplacements = nodalDisplacements.Where(n => String.Equals(n.ShapeId.ToString(), ModeShapeId, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                var warning = $"Shape Mode '{ModeShapeId}' does not exist";
                throw new ArgumentException(warning);
            }

            // Parse Results from the object
            var identifier = nodalDisplacements.Select(n => n.Id).ToList();
            var nodeId = nodalDisplacements.Select(n => n.NodeId).ToList();
            var shapeIds = nodalDisplacements.Select(n => n.ShapeId).Distinct().ToList();

            // Create a Rhino Vector for Displacement and Rotation
            var translation = new List<FemDesign.Geometry.Vector3d>();
            var rotation = new List<FemDesign.Geometry.Vector3d>();

            foreach (var nodeDisp in nodalDisplacements)
            {
                var transVector = new FemDesign.Geometry.Vector3d(nodeDisp.Ex, nodeDisp.Ey, nodeDisp.Ez);
                translation.Add(transVector);

                var rotVector = new FemDesign.Geometry.Vector3d(nodeDisp.Fix, nodeDisp.Fiy, nodeDisp.Fiz);
                rotation.Add(rotVector);
            }


            return new Dictionary<string, dynamic>
            {
                {"ShapeId", shapeIds},
                {"Id", identifier},
                {"NodeId", nodeId},
                {"Translation", translation},
                {"Rotation", rotation},
            };
        }
    }
}