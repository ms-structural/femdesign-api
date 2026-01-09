using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    /// <summary>
    /// Represents a Horizontal Polygon2d.
    /// </summary>
    [System.Serializable]
    public partial class HorizontalPolygon2d
    {
        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        [XmlElement("point")]
        public List<Point2d> Points { get; set; }
        private HorizontalPolygon2d() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalPolygon2d"/> class.
        /// </summary>
        /// <param name="point2d">the point2d.</param>
        public HorizontalPolygon2d(List<Point2d> point2d)
        {
            this.Points = point2d;
        }
    }
}
