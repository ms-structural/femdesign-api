using System.Xml.Serialization;


namespace FemDesign.Materials
{
    /// <summary>
    /// Represents a Limit Stresses.
    /// </summary>
    [System.Serializable()]
    public partial class LimitStresses: MechProps
    {
        /// <summary>
        /// Gets or sets the fm0k.
        /// </summary>
        [XmlAttribute("fm0k")]
        public double fm0k { get; set; }
        /// <summary>
        /// Gets or sets the fm90k.
        /// </summary>
        [XmlAttribute("fm90k")]
        public double fm90k { get; set; }
        /// <summary>
        /// Gets or sets the ft0k.
        /// </summary>
        [XmlAttribute("ft0k")]
        public double ft0k { get; set; }
        /// <summary>
        /// Gets or sets the ft90k.
        /// </summary>
        [XmlAttribute("ft90k")]
        public double ft90k { get; set; }
        /// <summary>
        /// Gets or sets the fc0k.
        /// </summary>
        [XmlAttribute("fc0k")]
        public double fc0k { get; set; }
        /// <summary>
        /// Gets or sets the fc90k.
        /// </summary>
        [XmlAttribute("fc90k")]
        public double fc90k { get; set; }
        /// <summary>
        /// Gets or sets the fxyk.
        /// </summary>
        [XmlAttribute("fxyk")]
        public double fxyk { get; set; }
        /// <summary>
        /// Gets or sets the fvk.
        /// </summary>
        [XmlAttribute("fvk")]
        public double fvk { get; set; }
        /// <summary>
        /// Gets or sets the f rk.
        /// </summary>
        [XmlAttribute("fRk")]
        public double fRk { get; set; }
        /// <summary>
        /// Gets or sets the f tork.
        /// </summary>
        [XmlAttribute("fTork")]
        public double fTork { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitStresses"/> class.
        /// </summary>
        public LimitStresses()
        {
            
        }
    }
}