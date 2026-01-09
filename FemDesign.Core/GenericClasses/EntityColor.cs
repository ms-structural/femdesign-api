// https://strusoft.com/
using System;
using System.Drawing;
using System.Globalization;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using Newtonsoft.Json.Linq;

namespace FemDesign
{
    /// <summary>
    /// Represents a Entity Color.
    /// </summary>
    [Serializable]
    public partial class EntityColor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityColor"/> class.
        /// </summary>
        public EntityColor()
        {
        }

        /// <summary>
        /// Gets or sets the tone.
        /// </summary>
        [XmlAttribute("tone")]
        public string _tone;

        [XmlIgnore]
        public Color Tone
        {
            get
            {
                Color col = System.Drawing.ColorTranslator.FromHtml("#" + this._tone);
                return col;
            }
            set
            {
                this._tone = ColorTranslator.ToHtml((Color)value).Substring(1);
            }
        }

        /// <summary>
        /// Gets or sets the pen width.
        /// </summary>
        [XmlAttribute("penwidth")]
        public double PenWidth { get; set; }

    }
}

