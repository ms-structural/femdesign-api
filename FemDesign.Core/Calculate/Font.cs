// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// FONT
    /// </summary>
    public partial class Font
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlElement("name")]
        public string Name = "Tahoma"; // SZID
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlElement("type")]
        public string Type = "ANSI_CHARSET"; // FONTTYPE
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        [XmlElement("size")]
        public string Size = "0.003"; // REAL_PLUS
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        [XmlElement("width")]
        public string Width = "1"; // REAL_PLUS
        /// <summary>
        /// Gets or sets the slant.
        /// </summary>
        [XmlElement("slant")]
        public string Slant = "0"; // FONTSLANT
        /// <summary>
        /// Initializes a new instance of the <see cref="Font"/> class.
        /// </summary>
        public Font()
        {

        }
    }
}