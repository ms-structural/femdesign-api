// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Supports
{
    /// <summary>
    /// Represents supports.
    /// </summary>
    [System.Serializable]
    public partial class Supports
    {
        /// <summary>
        /// Gets or sets the point support.
        /// </summary>
        [XmlElement("point_support", Order = 1)]
        public List<PointSupport> PointSupport = new List<PointSupport>(); // point_support_type
        /// <summary>
        /// Gets or sets the line support.
        /// </summary>
        [XmlElement("line_support", Order = 2)]
        public List<LineSupport> LineSupport = new List<LineSupport>(); // line_support_type
        /// <summary>
        /// Gets or sets the surface support.
        /// </summary>
        [XmlElement("surface_support", Order = 3)] 
        public List<SurfaceSupport> SurfaceSupport = new List<SurfaceSupport>(); // surface_support
        /// <summary>
        /// Gets or sets the stiffness point.
        /// </summary>
        [XmlElement("stiffness_point", Order = 4)]
        public List<StiffnessPoint> StiffnessPoint = new List<StiffnessPoint>(); // surface_support

        /// <summary>
        /// Gets the supports.
        /// </summary>
        /// <returns>The result.</returns>
        public List<GenericClasses.ISupportElement> GetSupports()
        {
            var objs = new List<GenericClasses.ISupportElement>();
            objs.AddRange(this.PointSupport);
            objs.AddRange(this.LineSupport);
            objs.AddRange(this.SurfaceSupport);
            return objs;
        }

    }
}