using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    /// <summary>
    /// Represents a Circle Edge.
    /// </summary>
    [XmlRoot("database", Namespace = "urn:strusoft")]
    [System.Serializable]
    public partial class CircleEdge : Edge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CircleEdge"/> class.
        /// </summary>
        public CircleEdge()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircleEdge"/> class.
        /// </summary>
        /// <param name="radius">the radius.</param>
        /// <param name="centerPoint">the center point.</param>
        /// <param name="plane">the plane.</param>
        public CircleEdge(double radius, Point3d centerPoint, Plane plane) : base(radius, centerPoint, plane)
        {
        }

    }
}
