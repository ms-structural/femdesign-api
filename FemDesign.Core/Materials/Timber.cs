// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Materials
{
    /// <summary>
    /// material_type --> timber
    /// </summary>
    [System.Serializable]
    public partial class Timber: MaterialBase
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute("type")]
        public string Type { get; set; } // integer
        /// <summary>
        /// Gets or sets the quality.
        /// </summary>
        [XmlAttribute("quality")]
        public string Quality { get; set; } // integer
        /// <summary>
        /// Gets or sets the fmk0.
        /// </summary>
        [XmlAttribute("Fmk0")]
        public string Fmk0 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fmk90.
        /// </summary>
        [XmlAttribute("Fmk90")]
        public string Fmk90 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the ft0k.
        /// </summary>
        [XmlAttribute("Ft0k")]
        public string Ft0k { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the ft90k.
        /// </summary>
        [XmlAttribute("Ft90k")]
        public string Ft90k { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fc0k.
        /// </summary>
        [XmlAttribute("Fc0k")]
        public string Fc0k { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fc90k.
        /// </summary>
        [XmlAttribute("Fc90k")]
        public string Fc90k { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fvk.
        /// </summary>
        [XmlAttribute("Fvk")]
        public string Fvk { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the e0mean.
        /// </summary>
        [XmlAttribute("E0mean")]
        public string E0mean { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the e90mean.
        /// </summary>
        [XmlAttribute("E90mean")]
        public string E90mean { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the e0comp.
        /// </summary>
        [XmlAttribute("E0comp")]
        public string E0comp { get; set; } // double
        /// <summary>
        /// Gets or sets the e90comp.
        /// </summary>
        [XmlAttribute("E90comp")]
        public string E90comp { get; set; } // double
        /// <summary>
        /// Gets or sets the gmean.
        /// </summary>
        [XmlAttribute("Gmean")]
        public string Gmean { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the e005.
        /// </summary>
        [XmlAttribute("E005")]
        public string E005 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the g005.
        /// </summary>
        [XmlAttribute("G005")]
        public string G005 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the rhok.
        /// </summary>
        [XmlAttribute("Rhok")]
        public string Rhok { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the rhomean.
        /// </summary>
        [XmlAttribute("Rhomean")]
        public string Rhomean { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the gamma m 0.
        /// </summary>
        [XmlAttribute("gammaM_0")]
        public string GammaM_0 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the gamma m 1.
        /// </summary>
        [XmlAttribute("gammaM_1")]
        public string GammaM_1 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the ksys.
        /// </summary>
        [XmlAttribute("ksys")]
        public double _ksys { get; set; } // system strength factor

        [XmlIgnore]
        public double ksys
        {
            get
            { return _ksys; }
            set
            {
                this._ksys = RestrictedDouble.ValueInClosedInterval(value, 0.00, 1.00);
            }
        }

        /// <summary>
        /// Gets or sets the k cr.
        /// </summary>
        [XmlAttribute("k_cr")]
        public double k_cr { get; set; } // reduction_factor_type. Optional.
        /// <summary>
        /// Gets or sets the service class.
        /// </summary>
        [XmlAttribute("service_class")]
        public int ServiceClass { get; set; } // timber_service_class_type
        /// <summary>
        /// Gets or sets the kdef u.
        /// </summary>
        [XmlAttribute("kdefU")]
        public double kdefU { get; set; } // material_0base_value. Optional.
        /// <summary>
        /// Gets or sets the kdef sq.
        /// </summary>
        [XmlAttribute("kdefSq")]
        public double kdefSq { get; set; } // material_0base_value. Optional.
        /// <summary>
        /// Gets or sets the kdef sf.
        /// </summary>
        [XmlAttribute("kdefSf")]
        public double kdefSf { get; set; } // material_0base_value. Optional.
        /// <summary>
        /// Gets or sets the kdef sc.
        /// </summary>
        [XmlAttribute("kdefSc")]
        public double kdefSc { get; set; } // material_0base_value. Optional.
        /// <summary>
        /// Gets or sets the gamma m fi.
        /// </summary>
        [XmlAttribute("gammaMfi")]
        public string GammaMFi { get; set; } // material_base_value. Optional.

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Timber()
        {

        }

        /// <summary>
        /// Set material parameters to timber material.
        /// </summary>
        /// <param name="_ksys">System strength factor.</param>
        /// <param name="_k_cr">-</param>
        /// <param name="serviceClass">Service class [1, 2, 3]</param>
        /// <param name="_kdefU">kdef U/Ua/Us</param>
        /// <param name="_kdefSq">kdef Sq</param>
        /// <param name="_kdefSf">kdef Sf</param>
        /// <param name="_kdefSc">kdef Sc</param>
        internal void SetMaterialParameters(double _ksys = 1.0, double _k_cr = 0.67, TimberServiceClassEnum serviceClass = TimberServiceClassEnum.ServiceClass1, double _kdefU = 0.0, double _kdefSq = 0.60, double _kdefSf = 0.60, double _kdefSc = 0.60)
        {
            int _serviceClass = (int)serviceClass;

            this.ksys = _ksys;

            if (_k_cr >= 0 & _k_cr <= 1)
            {
                this.k_cr = _k_cr;
            }
            else
            {
                throw new System.ArgumentException("0 <= k_cr <= 1");
            }

            if (_serviceClass == 1 || _serviceClass == 2 || _serviceClass == 3)
            {
                this.ServiceClass = _serviceClass - 1; // struxml service class is [0, 1 ,2] = [1, 2, 3]
            }
            else
            {
                throw new System.ArgumentException("service_class must be 1, 2 or 3");
            }
            this.kdefU = _kdefU;
            this.kdefSq = _kdefSq;
            this.kdefSf = _kdefSf;
            this.kdefSc = _kdefSc;
        }
    }
}