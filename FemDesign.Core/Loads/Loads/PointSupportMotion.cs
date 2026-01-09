// https://strusoft.com/

using System.Xml.Serialization;
using FemDesign.Geometry;
using FemDesign.Utils;
using System;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Point Support Motion.
    /// </summary>
    [System.Serializable]
    public partial class PointSupportMotion : SupportMotionBase
    {
        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        [XmlElement("direction")]
        public Geometry.Vector3d Direction { get; set; } // point_type_3d
        /// <summary>
        /// Gets or sets the displacement.
        /// </summary>
        [XmlElement("displacement")]
        public LoadLocationValue Displacement { get; set; } // location_value

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private PointSupportMotion()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointSupportMotion"/> class.
        /// </summary>
        /// <param name="point">the point.</param>
        /// <param name="disp">the disp.</param>
        /// <param name="loadCase">the load case.</param>
        /// <param name="comment">the comment.</param>
        /// <param name="type">the type.</param>
        public PointSupportMotion(Geometry.Point3d point, Geometry.Vector3d disp, LoadCase loadCase, string comment, SupportMotionType type)
        {
            this.EntityCreated();
            this.LoadCase = loadCase;
            this.Comment = comment;
            this.SupportMotionType = type;
            this.Direction = disp.Normalize();
            this.Displacement = new LoadLocationValue(point, disp.Length());
        }


        /// <summary>
        /// Motion.
        /// </summary>
        /// <param name="point">the point.</param>
        /// <param name="disp">the disp.</param>
        /// <param name="loadCase">the load case.</param>
        /// <param name="comment">the comment.</param>
        /// <returns>The result.</returns>
        public static PointSupportMotion Motion(Geometry.Point3d point, Geometry.Vector3d disp, LoadCase loadCase, string comment = "")
        {
            return new PointSupportMotion(point, disp, loadCase, comment, SupportMotionType.Motion);
        }

        /// <summary>
        /// Rotation.
        /// </summary>
        /// <param name="point">the point.</param>
        /// <param name="disp">the disp.</param>
        /// <param name="loadCase">the load case.</param>
        /// <param name="comment">the comment.</param>
        /// <returns>The result.</returns>
        public static PointSupportMotion Rotation(Geometry.Point3d point, Geometry.Vector3d disp, LoadCase loadCase, string comment = "")
        {
            return new PointSupportMotion(point, disp, loadCase, comment, SupportMotionType.Rotation);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            var units = this.SupportMotionType == SupportMotionType.Motion ? "m" : "rad";
            var text = $"{this.GetType().Name} Pos: ({this.Displacement.X.ToString("0.00")}, {this.Displacement.Y.ToString("0.00")}, {this.Displacement.Z.ToString("0.00")}), {this.SupportMotionType}: {this.Direction * this.Displacement.Value} {units}";
            if (LoadCase != null)
                return text + $", LoadCase: {this.LoadCase.Name}";
            else
                return text;
        }
    }
}