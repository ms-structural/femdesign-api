using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    /// <summary>
    /// Represents a Line Edge.
    /// </summary>
    [XmlRoot("database", Namespace = "urn:strusoft")]
    [System.Serializable]
    public partial class LineEdge : Edge
    {
        /// <summary>
        /// Gets or sets the start point.
        /// </summary>
        [XmlIgnore]
        public Point3d StartPoint => base.Points[0];

        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        [XmlIgnore]
        public Point3d EndPoint => base.Points[1];

        /// <summary>
        /// Initializes a new instance of the <see cref="LineEdge"/> class.
        /// </summary>
        public LineEdge()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineEdge"/> class.
        /// </summary>
        /// <param name="startPoint">the start point.</param>
        /// <param name="endPoint">the end point.</param>
        /// <param name="plane">the plane.</param>
        public LineEdge(Point3d startPoint, Point3d endPoint, Plane plane) : base(startPoint, endPoint, plane)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineEdge"/> class.
        /// </summary>
        /// <param name="startPoint">the start point.</param>
        /// <param name="endPoint">the end point.</param>
        /// <param name="localY">the local y.</param>
        public LineEdge(Point3d startPoint, Point3d endPoint, Vector3d localY = null) : base(startPoint, endPoint, localY)
        {
        }

        /// <summary>
        /// Evaluate a curve at a certain factor along its length.
        /// </summary>
        /// <param name="lengthFactor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Point3d PointAtLength(double lengthFactor)
		{
            var curveLength = this.Length;

            if (lengthFactor > curveLength) throw new ArgumentException("Length factor cannot be larger than the length of the curve.");
            if (lengthFactor < 0.00) throw new ArgumentException("Length factor cannot be smaller than zero.");

            var percentage = lengthFactor / curveLength;

            var domainX = this.EndPoint.X - this.StartPoint.X;
            var domainY = this.EndPoint.Y - this.StartPoint.Y;
            var domainZ = this.EndPoint.Z - this.StartPoint.Z;

            return new Point3d(domainX * percentage + this.StartPoint.X, domainY * percentage + this.StartPoint.Y, domainZ * percentage + this.StartPoint.Z);
        }

    }
}
