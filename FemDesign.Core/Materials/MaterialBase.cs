// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Materials
{
    /// <summary>
    /// Represents a Material Base.
    /// </summary>
    [System.Serializable]
    public partial class MaterialBase
    {
        // material_type_attribs

        /// <summary>
        /// Gets or sets the mass.
        /// </summary>
        [XmlAttribute("mass")]
        public double _mass; // non_neg_max_1e20
        [XmlIgnore]
        public double Mass
        {
            get { return _mass; }
            set { _mass = RestrictedDouble.NonNegMax_1e20(value); } // non_neg_max_1e20
        }

        /// <summary>
        /// Gets or sets the e 0.
        /// </summary>
        [XmlAttribute("E_0")]
        public double E_0  { get; set; }
        /// <summary>
        /// Gets or sets the e 1.
        /// </summary>
        [XmlAttribute("E_1")]
        public double E_1 { get; set; }
        /// <summary>
        /// Gets or sets the e 2.
        /// </summary>
        [XmlAttribute("E_2")]
        public double E_2 { get; set; }
        /// <summary>
        /// Gets or sets the nu 0.
        /// </summary>
        [XmlAttribute("nu_0")]
        public double nu_0 { get; set; }
        /// <summary>
        /// Gets or sets the nu 1.
        /// </summary>
        [XmlAttribute("nu_1")]
        public double nu_1 { get; set; }
        /// <summary>
        /// Gets or sets the nu 2.
        /// </summary>
        [XmlAttribute("nu_2")]
        public double nu_2 { get; set; }
        /// <summary>
        /// Gets or sets the alfa 0.
        /// </summary>
        [XmlAttribute("alfa_0")]
        public double alfa_0 { get; set; }
        /// <summary>
        /// Gets or sets the alfa 1.
        /// </summary>
        [XmlAttribute("alfa_1")]
        public double alfa_1 { get; set; }
        /// <summary>
        /// Gets or sets the alfa 2.
        /// </summary>
        [XmlAttribute("alfa_2")]
        public double alfa_2 { get; set; }
        /// <summary>
        /// Gets or sets the g 0.
        /// </summary>
        [XmlAttribute("G_0")]
        public double G_0 { get; set; }
        /// <summary>
        /// Gets or sets the g 1.
        /// </summary>
        [XmlAttribute("G_1")]
        public double G_1 { get; set; }
        /// <summary>
        /// Gets or sets the g 2.
        /// </summary>
        [XmlAttribute("G_2")]
        public double G_2 { get; set; }
    }
}