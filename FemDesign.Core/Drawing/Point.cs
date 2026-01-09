using FemDesign.Geometry;
using StruSoft.Interop.StruXml.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Drawing
{
    /// <summary>
    /// Represents a Point3d.
    /// </summary>
    public class Point3d : IDrawing
    {
        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        [XmlIgnore]
        public double X { get; set; }
        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        [XmlIgnore]
        public double Y { get; set; }
        /// <summary>
        /// Gets or sets the z.
        /// </summary>
        [XmlIgnore]
        public double Z { get; set; }


        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        [XmlElement("style")]
        public Style_type Style { get; set; }

        private Point3d()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point3d"/> class.
        /// </summary>
        /// <param name="x">value for <paramref name="x"/>.</param>
        /// <param name="y">value for <paramref name="y"/>.</param>
        /// <param name="z">value for <paramref name="z"/>.</param>
        /// <param name="layer">the layer.</param>
        /// <param name="style">the style.</param>
        public Point3d(double x, double y, double z, Layer_type layer, Style_type style)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;


            this.Style = style;
            this.Style.LayerObj = layer;
        }


        public static implicit operator StruSoft.Interop.StruXml.Data.Point_type(Point3d point)
        {
            return new StruSoft.Interop.StruXml.Data.Point_type
            {
                Location = new StruSoft.Interop.StruXml.Data.Point_type_3d
                {
                    X = point.X,
                    Y = point.Y,
                    Z = point.Z,
                },
                Style = point.Style,
            };
        }

    }
}
