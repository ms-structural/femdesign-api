using System.Xml.Serialization;


namespace FemDesign.Materials
{
    /// <summary>
    /// Represents a Timber Panel Strength.
    /// </summary>
    [System.Serializable]
    public partial class TimberPanelStrength
    {
        /// <summary>
        /// Gets or sets the fm k0.
        /// </summary>
        [XmlAttribute("fm_k0")]
        public double _fm_k0;
        [XmlIgnore]
        public double Fm_k0
        {
            get
            {
                return this._fm_k0;
            }
            set
            {
                this._fm_k0 = RestrictedDouble.NonNegMax_1e5(value);
            }
        }

        /// <summary>
        /// Gets or sets the fm k90.
        /// </summary>
        [XmlAttribute("fm_k90")]
        public double _fm_k90;
        [XmlIgnore]
        public double Fm_k90
        {
            get
            {
                return this._fm_k90;
            }
            set
            {
                this._fm_k90 = RestrictedDouble.NonNegMax_1e5(value);
            }
        }

        /// <summary>
        /// Gets or sets the ft k0.
        /// </summary>
        [XmlAttribute("ft_k0")]
        public double _ft_k0;
        [XmlIgnore]
        public double Ft_k0
        {
            get
            {
                return this._ft_k0;
            }
            set
            {
                this._ft_k0 = RestrictedDouble.NonNegMax_1e5(value);
            }
        }

        /// <summary>
        /// Gets or sets the ft k90.
        /// </summary>
        [XmlAttribute("ft_k90")]
        public double _ft_k90;
        [XmlIgnore]
        public double Ft_k90
        {
            get
            {
                return this._ft_k90;
            }
            set
            {
                this._ft_k90 = RestrictedDouble.NonNegMax_1e5(value);
            }
        }

        /// <summary>
        /// Gets or sets the fc k0.
        /// </summary>
        [XmlAttribute("fc_k0")]
        public double _fc_k0;
        [XmlIgnore]
        public double Fc_k0
        {
            get
            {
                return this._fc_k0;
            }
            set
            {
                this._fc_k0 = RestrictedDouble.NonNegMax_1e5(value);
            }
        }

        /// <summary>
        /// Gets or sets the fc k90.
        /// </summary>
        [XmlAttribute("fc_k90")]
        public double _fc_k90;
        [XmlIgnore]
        public double Fc_k90
        {
            get
            {
                return this._fc_k90;
            }
            set
            {
                this._fc_k90 = RestrictedDouble.NonNegMax_1e5(value);
            }
        }

        /// <summary>
        /// Gets or sets the fr k0.
        /// </summary>
        [XmlAttribute("fr_k0")]
        public double _fr_k0;
        [XmlIgnore]
        public double Fr_k0
        {
            get
            {
                return this._fr_k0;
            }
            set
            {
                this._fr_k0 = RestrictedDouble.NonNegMax_1e5(value);
            }
        }

        /// <summary>
        /// Gets or sets the fr k90.
        /// </summary>
        [XmlAttribute("fr_k90")]
        public double _fr_k90;
        [XmlIgnore]
        public double Fr_k90
        {
            get
            {
                return this._fr_k90;
            }
            set
            {
                this._fr_k90 = RestrictedDouble.NonNegMax_1e5(value);
            }
        }
        
        /// <summary>
        /// Gets or sets the fv k.
        /// </summary>
        [XmlAttribute("fv_k")]
        public double _fv_k;
        [XmlIgnore]
        public double Fv_k
        {
            get
            {
                return this._fv_k;
            }
            set
            {
                this._fv_k = RestrictedDouble.NonNegMax_1e5(value);
            }
        }
    }
}