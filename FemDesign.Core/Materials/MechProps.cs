using System.Xml.Serialization;


namespace FemDesign
{
    /// <summary>
    /// Represents a Mech Props.
    /// </summary>
    [System.Serializable()]
    public partial class MechProps
    {  
        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        [XmlIgnore]
        public string _material;
        
        [XmlAttribute("material")]
        public string Material
        {
            get
            {
                return this._material;
            }
            set
            {
                this._material = RestrictedString.Length(value, 256);
            }
        }

        /// <summary>
        /// Gets or sets the thickness.
        /// </summary>
        [XmlIgnore]
        public double _thickness;

        [XmlAttribute("thickness")]
        public double Thicknesss
        {
            get
            {
                return this._thickness;
            }
            set
            {
                this._thickness = RestrictedDouble.NonNegMax_1(value);
            }
        }

        /// <summary>
        /// Gets or sets the theta.
        /// </summary>
        [XmlAttribute("theta")]
        public double Theta { get; set; }

        /// <summary>
        /// Gets or sets the ex.
        /// </summary>
        [XmlAttribute("Ex")]
        public double Ex { get; set; }

        /// <summary>
        /// Gets or sets the ey.
        /// </summary>
        [XmlAttribute("Ey")]
        public double Ey { get; set; }

        /// <summary>
        /// Gets or sets the nuxy.
        /// </summary>
        [XmlAttribute("nuxy")]
        public double Nuxy { get; set; }

        /// <summary>
        /// Gets or sets the gxy.
        /// </summary>
        [XmlAttribute("Gxy")]
        public double Gxy { get; set; }

        /// <summary>
        /// Gets or sets the gxz.
        /// </summary>
        [XmlAttribute("Gxz")]
        public double Gxz { get; set; }

        /// <summary>
        /// Gets or sets the gyz.
        /// </summary>
        [XmlAttribute("Gyz")]
        public double Gyz { get; set; }

        /// <summary>
        /// Gets or sets the rho.
        /// </summary>
        [XmlAttribute("rho")]
        public double Rho { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MechProps"/> class.
        /// </summary>
        public MechProps()
        {

        }
    }
    
    
}