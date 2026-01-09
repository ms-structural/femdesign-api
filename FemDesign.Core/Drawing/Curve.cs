using FemDesign.GenericClasses;
using FemDesign.Geometry;
using Newtonsoft.Json.Linq;
using StruSoft.Interop.StruXml.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Drawing
{
    /// <summary>
    /// Represents a Curve.
    /// </summary>
    public class Curve : Edge, IDrawing
    {
        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        [XmlElement("style")]
        public Style_type Style { get; set; }

        private Curve()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Curve"/> class.
        /// </summary>
        /// <param name="startPoint">the start point.</param>
        /// <param name="endPoint">the end point.</param>
        /// <param name="layer">the layer.</param>
        /// <param name="style">the style.</param>
        /// <param name="localY">the local y.</param>
        public Curve(FemDesign.Geometry.Point3d startPoint, FemDesign.Geometry.Point3d endPoint, Layer_type layer, Style_type style, Vector3d localY = null) : base(startPoint, endPoint, localY = null)
        {
            this.Style = style;
            this.Style.LayerObj = layer;
        }

        public static implicit operator StruSoft.Interop.StruXml.Data.Curve_type(Curve curve)
        {
            return new StruSoft.Interop.StruXml.Data.Curve_type
            {
                Point = curve.Points.Select(x => (StruSoft.Interop.StruXml.Data.Point_type_3d)x).ToList(),
                Style = curve.Style,
            };
        }
    }
}