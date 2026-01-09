using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Materials
{
    /// <summary>
    /// Represents a Clt Data Type.
    /// </summary>
    [System.Serializable()]
    public partial class CltDataType
    {
        /// <summary>
        /// Gets or sets the default kdef.
        /// </summary>
        [XmlElement("default_kdef", Order = 1)]
        public TimberServiceClassKdfes DefaultKdef { get; set; }

        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        [XmlElement("layer", Order = 2)]
        public List<LimitStresses> Layers { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer.
        /// </summary>
        [XmlAttribute("manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the r33.
        /// </summary>
        [XmlAttribute("r33")]
        public double R33 { get; set; }

        /// <summary>
        /// Gets or sets the r66.
        /// </summary>
        [XmlAttribute("r66")]
        public double R66 { get; set; }

        /// <summary>
        /// Gets or sets the r77.
        /// </summary>
        [XmlAttribute("r77")]
        public double R77 { get; set; }

        /// <summary>
        /// Gets or sets the r88.
        /// </summary>
        [XmlAttribute("r88")]
        public double R88 { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CltDataType"/> class.
        /// </summary>
        public CltDataType()
        {

        }
    }
}