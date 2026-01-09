using System.Xml.Serialization;


namespace FemDesign.Materials
{
    /// <summary>
    /// Represents a Orthotropic Panel Data.
    /// </summary>
    [System.Serializable]
    public partial class OrthotropicPanelData
    {
        /// <summary>
        /// Gets or sets the stiffness.
        /// </summary>
        [XmlElement("stiffness", Order=1)]
        public TimberPanelStiffness Stiffness { get; set; }

        /// <summary>
        /// Gets or sets the strength.
        /// </summary>
        [XmlElement("strength", Order=2)]
        public TimberPanelStrength Strength { get; set; }

        /// <summary>
        /// Gets or sets the service class factors0.
        /// </summary>
        [XmlElement("service_class_0_factors", Order=3)]
        public ServiceClassFactors ServiceClassFactors0 { get; set;}

        /// <summary>
        /// Gets or sets the service class factors1.
        /// </summary>
        [XmlElement("service_class_1_factors", Order=4)]
        public ServiceClassFactors ServiceClassFactors1 { get; set;}

        /// <summary>
        /// Gets or sets the service class factors2.
        /// </summary>
        [XmlElement("service_class_2_factors", Order=5)]
        public ServiceClassFactors ServiceClassFactors2 { get; set;}

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("description")]
        public string _name;
        [XmlIgnore]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = RestrictedString.Length(value, 40);
            }
        }
        
        /// <summary>
        /// Gets or sets the thickness.
        /// </summary>
        [XmlAttribute("thickness")]
        public double _thickness;
        [XmlIgnore]
        public double Thickness
        {
            get
            {
                return this._thickness;
            }
            set
            {
                this._thickness = RestrictedDouble.TimberPanelThickness(value);
            }
        }


    }
}