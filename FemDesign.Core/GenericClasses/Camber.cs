// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign
{
    /// <summary>
    /// Represents a Camber.
    /// </summary>
    [System.Serializable]
    public partial class Camber
    {
        /// <summary>
        /// Gets or sets the force.
        /// </summary>
        [XmlAttribute("force")]
        public double _force;

        [XmlIgnore]
        public double Force
        {
            get
            {
                return this._force;
            }
            set
            {
                this._force = RestrictedDouble.NonNegMax_1e10(value);
            }
        }

        /// <summary>
        /// Gets or sets the eccentricity.
        /// </summary>
        [XmlAttribute("e")]
        public double _eccentricity;
        [XmlIgnore]
        public double Eccentricity
        {
            get
            {
                return this._eccentricity;
            }
            set
            {
                this._eccentricity = RestrictedDouble.NonNegMax_10000(value);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private Camber()
        {

        }

        /// <summary>
        /// Construct a new Camber instance
        /// </summary>
        /// <param name="force">Force.</param>
        /// <param name="eccentricity">Eccentricity.</param>
        public Camber(double force, double eccentricity)
        {
            this.Force = force;
            this.Eccentricity = eccentricity;
        }
    }
}