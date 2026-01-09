using System.Xml.Serialization;


namespace FemDesign.Materials
{
    /// <summary>
    /// Represents a Timber Panel Stiffness.
    /// </summary>
    [System.Serializable]
    public partial class TimberPanelStiffness
    {
        /// <summary>
        /// Gets or sets the em k0.
        /// </summary>
        [XmlAttribute("Em_k0")]
        public double _em_k0;
        [XmlIgnore]
        public double Em_k0
        {
            get
            {
                return this._em_k0;
            }
            set
            {
                this._em_k0 = RestrictedDouble.NonNegMax_1e5(value);
            }
        }

        /// <summary>
        /// Gets or sets the em k90.
        /// </summary>
        [XmlAttribute("Em_k90")]
        public double _em_k90;
        [XmlIgnore]
        public double Em_k90
        {
            get
            {
                return this._em_k90;
            }
            set
            {
                this._em_k90 = RestrictedDouble.NonNegMax_1e5(value);
            }
        }

        /// <summary>
        /// Gets or sets the et k0.
        /// </summary>
        [XmlAttribute("Et_k0")]
        public double _et_k0;
        [XmlIgnore]
        public double Et_k0
        {
            get
            {
                return this._et_k0;
            }
            set
            {
                this._et_k0 = RestrictedDouble.NonNegMax_1e5(value);
            }
        }

        /// <summary>
        /// Gets or sets the et k90.
        /// </summary>
        [XmlAttribute("Et_k90")]
        public double _et_k90;
        [XmlIgnore]
        public double Et_k90
        {
            get
            {
                return this._et_k90;
            }
            set
            {
                this._et_k90 = RestrictedDouble.NonNegMax_1e5(value);
            }
        }

        /// <summary>
        /// Gets or sets the ec k0.
        /// </summary>
        [XmlAttribute("Ec_k0")]
        public double _ec_k0;
        [XmlIgnore]
        public double Ec_k0
        {
            get
            {
                return this._ec_k0;
            }
            set
            {
                this._ec_k0 = RestrictedDouble.NonNegMax_1e5(value);
            }
        }

        /// <summary>
        /// Gets or sets the ec k90.
        /// </summary>
        [XmlAttribute("Ec_k90")]
        public double _ec_k90;
        [XmlIgnore]
        public double Ec_k90
        {
            get
            {
                return this._ec_k90;
            }
            set
            {
                this._ec_k90 = RestrictedDouble.NonNegMax_1e5(value);
            }
        }

        /// <summary>
        /// Gets or sets the gr k0.
        /// </summary>
        [XmlAttribute("Gr_k0")]
        public double _gr_k0;
        [XmlIgnore]
        public double Gr_k0
        {
            get
            {
                return this._gr_k0;
            }
            set
            {
                this._gr_k0 = RestrictedDouble.NonNegMax_1e5(value);
            }
        }

        /// <summary>
        /// Gets or sets the gr k90.
        /// </summary>
        [XmlAttribute("Gr_k90")]
        public double _gr_k90;
        [XmlIgnore]
        public double Gr_k90
        {
            get
            {
                return this._gr_k90;
            }
            set
            {
                this._gr_k90 = RestrictedDouble.NonNegMax_1e5(value);
            }
        }

        /// <summary>
        /// Gets or sets the gv k.
        /// </summary>
        [XmlAttribute("Gv_k")]
        public double _gv_k;
        [XmlIgnore]
        public double Gv_k
        {
            get
            {
                return this._gv_k;
            }
            set
            {
                this._gv_k = RestrictedDouble.NonNegMax_1e5(value);
            }
        }

        /// <summary>
        /// Gets or sets the rho.
        /// </summary>
        [XmlAttribute("rho")]
        public double _rho;
        [XmlIgnore]
        public double Rho
        {
            get
            {
                return this._rho;
            }
            set
            {
                this._rho = RestrictedDouble.NonNegMax_1e5(value);
            }
        }
    }
}