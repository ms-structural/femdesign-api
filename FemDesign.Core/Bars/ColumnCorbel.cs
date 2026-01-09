using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Bars
{
    /// <summary>
    /// Represents a Column Corbel.
    /// </summary>
    [System.Serializable]
    public partial class ColumnCorbel: EntityBase
    {
        /// <summary>
        /// Gets or sets the connectable parts.
        /// </summary>
        [XmlElement("connectable_parts", Order = 1)]
        public ThreeGuidListType ConnectableParts { get; set; }

        /// <summary>
        /// Gets or sets the connectivity.
        /// </summary>
        [XmlElement("connectivity", Order = 2)]
        public Connectivity Connectivity { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlIgnore]
        public string Instance
        {
            get
            {
                var found = this.Name.IndexOf(".");
                return this.Name.Substring(found + 1);
            }
        }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Identifier => this.Name.Split('.')[0];

        /// <summary>
        /// Gets or sets the base column.
        /// </summary>
        [XmlAttribute("base_column")]
        public System.Guid BaseColumn { get; set; }

        /// <summary>
        /// Gets or sets the complex material.
        /// </summary>
        [XmlAttribute("complex_material")]
        public System.Guid ComplexMaterial { get; set; }

        /// <summary>
        /// Gets or sets the made.
        /// </summary>
        [XmlAttribute("made")]
        public string Made { get; set; }

        /// <summary>
        /// Gets or sets the complex section.
        /// </summary>
        [XmlAttribute("complex_section")]
        public System.Guid ComplexSection { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        [XmlAttribute("pos")]
        public double Position { get; set; }

        /// <summary>
        /// Gets or sets the alpha.
        /// </summary>
        [XmlAttribute("alpha")]
        public double Alpha { get; set; }

        /// <summary>
        /// Gets or sets the d.
        /// </summary>
        [XmlAttribute("d")]
        public double D { get; set; }

        /// <summary>
        /// Gets or sets the l.
        /// </summary>
        [XmlAttribute("l")]
        public double L { get; set; }

        /// <summary>
        /// Gets or sets the e.
        /// </summary>
        [XmlAttribute("e")]
        public double E { get; set; }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        [XmlAttribute("x")]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        [XmlAttribute("y")]
        public double Y { get; set; }
    }
}