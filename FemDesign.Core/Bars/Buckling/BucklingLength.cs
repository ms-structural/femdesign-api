// https://strusoft.com/
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign.Bars.Buckling
{
    /// <summary>
    /// Represents a Buckling Length.
    /// </summary>
    [System.Serializable]
    public partial class BucklingLength
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute("type")]
        public BucklingType Type { get; set; } // bar_buckling_type

        /// <summary>
        /// Gets or sets the beta.
        /// </summary>
        [XmlAttribute("beta")]
        public string _beta; // non_neg_max_100

        [XmlIgnore]
        public double Beta
        {
            get
            {
                return double.Parse(this._beta, CultureInfo.InvariantCulture);
            }
            set
            {
                this._beta = RestrictedDouble.NonNegMax_100(value).ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets or sets the sway.
        /// </summary>
        [XmlAttribute("sway")]
        public bool Sway;

        /// <summary>
        /// Gets or sets the load position.
        /// </summary>
        [XmlAttribute("load_position")]
        public VerticalAlignment LoadPosition { get; set; }

        /// <summary>
        /// Gets or sets the continously restrained.
        /// </summary>
        [XmlAttribute("continously_restrained")]
        public bool ContinouslyRestrained;

        /// <summary>
        /// Gets a value indicating whether ntilever.
        /// </summary>
        [XmlAttribute("cantilever")]
        public bool Cantilever;

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        [XmlElement("position")]
        public Position Position { get; set; } // segmentposition_type

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal BucklingLength()
        {
            
        }

        /// <summary>
        /// Constructor for flexural buckling length.
        /// </summary>
        internal BucklingLength(Position position, BucklingType type, double beta = 1, bool sway = false)
        {
            this.Position = position;
            this.Type = type;
            this.Beta = beta;
            if (sway)
            {
                this.Sway = sway;
            }   
        }

        /// <summary>
        /// Constructor for pressured flange buckling length.
        /// </summary>
        internal BucklingLength(Position position, BucklingType type, double beta, VerticalAlignment loadPosition, bool continouslyRestrained)
        {
            this.Position = position;
            this.Type = type;
            this.Beta = beta;
            this.LoadPosition = loadPosition;
            if (continouslyRestrained)
            {
                this.ContinouslyRestrained = continouslyRestrained;
            }
        }

        /// <summary>
        /// Constructor for lateral torsional buckling length.
        /// </summary>
        internal BucklingLength(Position position, BucklingType type, VerticalAlignment loadPosition, bool continouslyRestrained, bool cantilever)
        {
            this.Position = position;
            this.Type = type;
            this.LoadPosition = loadPosition;
            if (continouslyRestrained)
            {
                this.ContinouslyRestrained = continouslyRestrained;
                }
            if (cantilever)
            {
                this.Cantilever = cantilever;
            }
        }

        /// <summary>
        /// Define BucklingLength in Flexural Stiff direction.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="beta">Beta factor.</param>
        /// <param name="sway">Sway. True/false.</param>
        /// <returns></returns>
        public static BucklingLength FlexuralStiff(double beta = 1, bool sway = false)
        {
            BucklingType _type = BucklingType.FlexuralStiff;
            return new BucklingLength(Position.AlongBar(), _type, beta, sway);
        }
        /// <summary>
        /// Define BucklingLength in Flexural Weak direction.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="beta">Beta factor.</param>
        /// <param name="sway">Sway. True/false.</param>
        /// <returns></returns>
        public static BucklingLength FlexuralWeak(double beta = 1, bool sway = false)
        {
            BucklingType _type = BucklingType.FlexuralWeak;
            return new BucklingLength(Position.AlongBar(), _type, beta, sway);
        }
        /// <summary>
        /// Define BucklingLength for Pressured Top Flange.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="beta">Beta factor.</param>
        /// <param name="loadPosition">"top"/"center"/"bottom"</param>
        /// <param name="continuouslyRestrained">Continuously restrained. True/false.</param>
        /// <returns></returns>
        public static BucklingLength PressuredTopFlange(VerticalAlignment loadPosition, double beta = 1, bool continuouslyRestrained = false)
        {
            BucklingType _type = BucklingType.PressuredTopFlange;
            return new BucklingLength(Position.AlongBar(), _type, beta, loadPosition, continuouslyRestrained);
        }
        /// <summary>
        /// Define BucklingLength for Pressured Bottom Flange.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="beta">Beta factor.</param>
        /// <param name="loadPosition">"top"/"center"/"bottom"</param>
        /// <param name="continuouslyRestrained">Continuously restrained. True/false.</param>
        /// <returns></returns>
        public static BucklingLength PressuredBottomFlange(VerticalAlignment loadPosition, double beta = 1, bool continuouslyRestrained = false)
        {
            BucklingType _type = BucklingType.PressuredBottomFlange;
            return new BucklingLength(Position.AlongBar(), _type, beta, loadPosition, continuouslyRestrained);
        }
        /// <summary>
        /// Define BucklingLength for Lateral Torsional buckling.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="loadPosition">"top"/"center"/"bottom"</param>
        /// <param name="continouslyRestrained">Continously restrained. True/false.</param>
        /// <param name="cantilever">Cantilever. True/false.</param>
        /// <returns></returns>
        public static BucklingLength LateralTorsional(VerticalAlignment loadPosition, bool continouslyRestrained = false, bool cantilever = false)
        {
            BucklingType _type = BucklingType.LateralTorsional;
            return new BucklingLength(Position.AlongBar(), _type, loadPosition, continouslyRestrained, cantilever);
        }
    }
}