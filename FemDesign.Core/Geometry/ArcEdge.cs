using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    /// <summary>
    /// Represents a Arc Edge.
    /// </summary>
    [XmlRoot("database", Namespace = "urn:strusoft")]
    [System.Serializable]
    public partial class ArcEdge : Edge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArcEdge"/> class.
        /// </summary>
        public ArcEdge()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArcEdge"/> class.
        /// </summary>
        /// <param name="start">the start.</param>
        /// <param name="middle">the middle.</param>
        /// <param name="end">the end.</param>
        /// <param name="plane">the plane.</param>
        public ArcEdge(Point3d start, Point3d middle, Point3d end, Plane plane) : base(start, middle, end, plane)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArcEdge"/> class.
        /// </summary>
        /// <param name="radius">the radius.</param>
        /// <param name="startAngle">the start angle.</param>
        /// <param name="endAngle">the end angle.</param>
        /// <param name="center">the center.</param>
        /// <param name="xAxis">value for <paramref name="xAxis"/>.</param>
        /// <param name="plane">the plane.</param>
        public ArcEdge(double radius, double startAngle, double endAngle,  Point3d center, Vector3d xAxis, Plane plane) : base(radius, startAngle, endAngle, center, xAxis, plane)
        {
        }

    }
}
