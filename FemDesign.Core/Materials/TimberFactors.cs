using System.Xml.Serialization;


namespace FemDesign.Materials
{
    /// <summary>
    /// Represents a Timber Factors.
    /// </summary>
    [System.Serializable]
    public partial class TimberFactors
    {
        /// <summary>
        /// Gets or sets the gamma mu.
        /// </summary>
        [XmlAttribute("gamma_m_u")]
        public double _gammaMU;
        [XmlIgnore]
        public double GammaMU
        {
            get
            {
                return this._gammaMU;
            }
            set
            {
                this._gammaMU = RestrictedDouble.NonNegMax_1e10(value);
            }
        }

        /// <summary>
        /// Gets or sets the gamma m as.
        /// </summary>
        [XmlAttribute("gamma_m_as")]
        public double _gammaMAs;
        [XmlIgnore]
        public double GammaMAs
        {
            get
            {
                return this._gammaMAs;
            }
            set
            {
                this._gammaMAs = RestrictedDouble.NonNegMax_1e10(value);
            }
        }

        /// <summary>
        /// kdef_U
        /// </summary>
        [XmlAttribute("kdef_U")]
        public double _kdefU;
        [XmlIgnore]
        public double KdefU
        {
            get
            {
                return this._kdefU;
            }
            set
            {
                this._kdefU = RestrictedDouble.NonNegMax_1e10(value);
            }
        }

        /// <summary>
        /// kdef_Sq
        /// </summary>
        [XmlAttribute("kdef_Sq")]
        public double _kdefSq;
        [XmlIgnore]
        public double KdefSq
        {
            get
            {
                return this._kdefSq;
            }
            set
            {
                this._kdefSq = RestrictedDouble.NonNegMax_1e10(value);
            }
        }

        /// <summary>
        /// kdef_Sf
        /// </summary>
        [XmlAttribute("kdef_Sf")]
        public double _kdefSf;
        [XmlIgnore]
        public double KdefSf
        {
            get
            {
                return this._kdefSf;
            }
            set
            {
                this._kdefSf = RestrictedDouble.NonNegMax_1e10(value);
            }
        }

        /// <summary>
        /// kdef_Sc
        /// </summary>
        [XmlAttribute("kdef_Sc")]
        public double _kdefSc;
        [XmlIgnore]
        public double KdefSc
        {
            get
            {
                return this._kdefSc;
            }
            set
            {
                this._kdefSc = RestrictedDouble.NonNegMax_1e10(value);
            }
        }

        /// <summary>
        /// Gets or sets the service class.
        /// </summary>
        [XmlAttribute("service_class")]
        public TimberServiceClassEnum ServiceClass { get; set; }

        /// <summary>
        /// Gets or sets the system factor.
        /// </summary>
        [XmlAttribute("system_factor")]
        public double _systemFactor;
        [XmlIgnore]
        public double SystemFactor
        {
            get
            {
                return this._systemFactor;
            }
            set
            {
                this._systemFactor = RestrictedDouble.NonNegMax_1e10(value);
            }
        }

        private TimberFactors()
        {

        }

        /// <summary>
        /// Factors for Orthotropic shell and CLT timber materials.
        /// </summary>
        /// <param name="gammaMU">Gamma M (U)</param>
        /// <param name="gammaMAs">Gamma M (Ua, Us)</param>
        /// <param name="kdefU">kdef (U, Ua, Us)</param>
        /// <param name="kdefSq">kdef (Sq)</param>
        /// <param name="kdefSf">kdef (Sf)</param>
        /// <param name="kdefSc">kdef (Sc)</param>
        /// <param name="serviceClass">Service Class</param>
        /// <param name="systemFactor">System Factor</param>
        public TimberFactors(double gammaMU, double gammaMAs, double kdefU, double kdefSq, double kdefSf, double kdefSc, TimberServiceClassEnum serviceClass, double systemFactor)
        {
            GammaMU = gammaMU;
            GammaMAs = gammaMAs;
            KdefU = kdefU;
            KdefSq = kdefSq;
            KdefSf = kdefSf;
            KdefSc = kdefSc;
            ServiceClass = serviceClass;
            SystemFactor = systemFactor;
        }
    }
}
