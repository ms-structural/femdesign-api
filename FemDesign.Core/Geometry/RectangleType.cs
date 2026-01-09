// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    /// <summary>
    /// Represents a Rectangle Type.
    /// </summary>
    [System.Serializable]
    public partial class RectangleType
    {
        /// <summary>
        /// Gets or sets the base corner.
        /// </summary>
        [XmlElement("base_corner", Order = 1)]
        public Point3d BaseCorner { get; set; }

        /// <summary>
        /// Gets or sets the local x.
        /// </summary>
        [XmlElement("x_direction", Order = 2)]
        public Vector3d LocalX { get; set; }

        /// <summary>
        /// Gets or sets the local y.
        /// </summary>
        [XmlElement("y_direction", Order = 3)]
        public Vector3d LocalY { get; set; }

        /// <summary>
        /// Gets or sets the dim x.
        /// </summary>
        [XmlAttribute("x_size")]
        public double DimX {get; set; }

        /// <summary>
        /// Gets or sets the dim y.
        /// </summary>
        [XmlAttribute("y_size")]
        public double DimY { get; set; }

        private RectangleType()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleType"/> class.
        /// </summary>
        /// <param name="baseCorner">the base corner.</param>
        /// <param name="xDir">value for <paramref name="xDir"/>.</param>
        /// <param name="yDir">value for <paramref name="yDir"/>.</param>
        /// <param name="xDim">value for <paramref name="xDim"/>.</param>
        /// <param name="yDim">value for <paramref name="yDim"/>.</param>
        public RectangleType(Point3d baseCorner, Vector3d xDir, Vector3d yDir, double xDim, double yDim)
        {
            this.BaseCorner = baseCorner;
            this.LocalX = xDir;
            this.LocalY = yDir;
            this.DimX = xDim;
            this.DimY = yDim;
        }
    }
}
