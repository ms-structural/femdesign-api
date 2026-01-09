// https://strusoft.com/

using System.Xml.Serialization;
using FemDesign.Geometry;
using FemDesign.Utils;
using System;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Point Load.
    /// </summary>
    [System.Serializable]
    public partial class PointLoad: ForceLoadBase
    {
        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        [XmlElement("direction")]
        public Geometry.Vector3d Direction { get; set; } // point_type_3d
        /// <summary>
        /// Gets or sets the load.
        /// </summary>
        [XmlElement("load")]
        public LoadLocationValue Load { get; set; } // location_value

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private PointLoad()
        {

        }

        /// <summary>
        /// Internal constructor accessed by static methods.
        /// </summary>
        /// <param name="point">the point.</param>
        /// <param name="force">the force.</param>
        /// <param name="loadCase">the load case.</param>
        /// <param name="comment">the comment.</param>
        /// <param name="type">the type.</param>
        public PointLoad(Geometry.Point3d point, Geometry.Vector3d force, LoadCase loadCase, string comment, ForceLoadType type)
        {
            this.EntityCreated();
            this.LoadCase = loadCase;
            this.Comment = comment;
            this.LoadType = type;
            this.Direction = force.Normalize();
            this.Load = new LoadLocationValue(point, force.Length());
        }


        /// <summary>
        /// Force.
        /// </summary>
        /// <param name="point">the point.</param>
        /// <param name="force">the force.</param>
        /// <param name="loadCase">the load case.</param>
        /// <param name="comment">the comment.</param>
        /// <returns>The result.</returns>
        public static PointLoad Force(Geometry.Point3d point, Geometry.Vector3d force, LoadCase loadCase, string comment = "")
		{
            return new PointLoad(point, force, loadCase, comment, ForceLoadType.Force);
        }

        /// <summary>
        /// Moment.
        /// </summary>
        /// <param name="point">the point.</param>
        /// <param name="force">the force.</param>
        /// <param name="loadCase">the load case.</param>
        /// <param name="comment">the comment.</param>
        /// <returns>The result.</returns>
        public static PointLoad Moment(Geometry.Point3d point, Geometry.Vector3d force, LoadCase loadCase, string comment = "")
        {
            return new PointLoad(point, force, loadCase, comment, ForceLoadType.Moment);
        }

        /// <summary>
        /// Caseless Point Load.
        /// </summary>
        /// <param name="point">the point.</param>
        /// <param name="force">the force.</param>
        /// <returns>The result.</returns>
        public static PointLoad CaselessPointLoad(Geometry.Point3d point, Geometry.Vector3d force)
        {
            var caseless = new PointLoad();
            caseless.EntityCreated();
            caseless.LoadType = ForceLoadType.Force;
            caseless.Direction = force.Normalize();
            caseless.Load = new LoadLocationValue(point, force.Length());
            return caseless;
        }

        /// <summary>
        /// Defines an operator overload.
        /// </summary>
        /// <param name="obj">the obj.</param>
        /// <returns>The result.</returns>
        public static explicit operator PointLoad( StruSoft.Interop.StruXml.Data.Caseless_point_load_type obj)
        {
            var pointLoad = new PointLoad();


            pointLoad.Guid = new System.Guid( obj.Guid );
            pointLoad.Action = obj.Action.ToString().ToLower();
            pointLoad.LastChange = obj.Last_change;
            pointLoad.LoadType = (ForceLoadType)Enum.Parse(typeof(ForceLoadType), obj.Load_type.ToString());

            var pos = new Point3d(obj.Load.X, obj.Load.Y, obj.Load.Z);
            var val = obj.Load.Val;
            pointLoad.Load = new LoadLocationValue(pos, val);

            pointLoad.Direction = new Vector3d( obj.Direction.X, obj.Direction.Y, obj.Direction.Z);

            return pointLoad;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            var units = this.LoadType == ForceLoadType.Force ? "kN" : "kNm";
            var text = $"{this.GetType().Name} Pos: ({this.Load.X.ToString("0.00")}, {this.Load.Y.ToString("0.00")}, {this.Load.Z.ToString("0.00")}), {this.LoadType}: {this.Direction * this.Load.Value} {units}";
            if (LoadCase != null)
                return text + $", LoadCase: {this.LoadCase.Name}";
            else
                return text;
        }
    }
}