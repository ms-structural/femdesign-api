using System.Xml.Serialization;


namespace FemDesign.Materials
{
    /// <summary>
    /// Represents a Service Class Factors.
    /// </summary>
    [System.Serializable]
    public partial class ServiceClassFactors
    {
        /// <summary>
        /// Gets or sets the kdef.
        /// </summary>
        [XmlAttribute("kdef")]
        public double _kdef;
        [XmlIgnore]
        public double Kdef
        {
            get
            {
                return this._kdef;
            }
            set
            {
                this._kdef = RestrictedDouble.NonNegMax_1e5(value);
            }
        }
        /// <summary>
        /// Gets or sets the kmod permanent.
        /// </summary>
        [XmlAttribute("kmod_permanent")]
        public double _kmodPermanent;
        [XmlIgnore]
        public double KmodPermanent
        {
            get
            {
                return this._kmodPermanent;
            }
            set
            {
                this._kmodPermanent = RestrictedDouble.NonNegMax_1e5(value);
            }
        }
        /// <summary>
        /// Gets or sets the kmod long term.
        /// </summary>
        [XmlAttribute("kmod_long_term")]
        public double _kmodLongTerm;
        [XmlIgnore]
        public double KmodLongTerm
        {
            get
            {
                return this._kmodLongTerm;
            }
            set
            {
                this._kmodLongTerm = RestrictedDouble.NonNegMax_1e5(value);
            }
        }
        /// <summary>
        /// Gets or sets the kmod medium term.
        /// </summary>
        [XmlAttribute("kmod_medium_term")]
        public double _kmodMediumTerm;
        [XmlIgnore]
        public double KmodMediumTerm
        {
            get
            {
                return this._kmodMediumTerm;
            }
            set
            {
                this._kmodMediumTerm = RestrictedDouble.NonNegMax_1e5(value);
            }
        }
        /// <summary>
        /// Gets or sets the kmod short term.
        /// </summary>
        [XmlAttribute("kmod_short_term")]
        public double _kmodShortTerm;
        [XmlIgnore]
        public double KmodShortTerm
        {
            get
            {
                return this._kmodShortTerm;
            }
            set
            {
                this._kmodShortTerm = RestrictedDouble.NonNegMax_1e5(value);
            }
        }
        /// <summary>
        /// Gets or sets the kmod instantaneous.
        /// </summary>
        [XmlAttribute("kmod_instantaneous")]
        public double _kmodInstantaneous;
        [XmlIgnore]
        public double KmodInstantaneous
        {
            get
            {
                return this._kmodInstantaneous;
            }
            set
            {
                this._kmodInstantaneous = RestrictedDouble.NonNegMax_1e5(value);
            }
        }
    }
}

