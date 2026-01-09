// https://strusoft.com/

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;


namespace FemDesign.Geometry
{
    /// <summary>
    /// Represents a Line Segment.
    /// </summary>
    [System.Serializable]
    public partial class LineSegment
    {
        /// <summary>
        /// Gets or sets the start point.
        /// </summary>
        [XmlElement("start_point")]
        public Point3d StartPoint;
        /// <summary>
        /// Gets or sets the base point.
        /// </summary>
        [XmlElement("base_point")]
        public Point3d BasePoint;
        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        [XmlElement("end_point")]
        public Point3d EndPoint;

        /// <summary>
        /// Gets or sets the verticies.
        /// </summary>
        [XmlIgnore]
        public List<Point3d> Verticies { 
            get {
                return new List<Point3d>() { StartPoint, EndPoint };
            } 
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public LineSegment()
        {

        }

        /// <summary>
        /// Construct LineSegment from start and endpoint.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public LineSegment(Point3d start, Point3d end)
        {
            this.StartPoint = start;
            this.BasePoint = new Point3d((start.X + end.X) / 2.0, (start.Y + end.Y) / 2.0, (start.Z + end.Z) / 2.0);
            this.EndPoint = end;
        }
    }
}