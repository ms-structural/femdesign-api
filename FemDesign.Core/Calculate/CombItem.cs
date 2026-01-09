// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// ANALCOMBITEM
    /// </summary>
    [System.Serializable]
    public partial class CombItem
    {
        /// <summary>
        /// Gets or sets the calc.
        /// </summary>
        [XmlAttribute("Calc")]
        public int _calc { get; set; } = 1;

        /// <summary>
        /// Calculate load combination (linear analysis).
        /// </summary>
        [XmlIgnore]
        public bool Calc
        {
            get 
            { 
                return Convert.ToBoolean(_calc); 
            }
            set 
            { 
                _calc = Convert.ToInt32(value);
                if(!value)
                {
                    NLE = PL = NLS = Cr = f2nd = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the nle.
        /// </summary>
        [XmlAttribute("NLE")]
        public int _nle { get; set; }

        /// <summary>
        /// Consider elastic nonlinear behaviour of structural elements. If false, 'NLE' must be false.
        /// </summary>
        [XmlIgnore]
        public bool NLE
        {
            get 
            { 
                return Convert.ToBoolean(_nle); 
            }
            set 
            { 
                _nle = Convert.ToInt32(value);
                if(!value)
                    PL = false;
            }
        }

        /// <summary>
        /// Gets or sets the pl.
        /// </summary>
        [XmlAttribute("PL")]
        public int _pl { get; set; }

        /// <summary>
        /// Consider plastic behaviour of structural elements. If true, 'NLE' must be true.
        /// </summary>
        [XmlIgnore]
        public bool PL
        {
            get 
            { 
                return Convert.ToBoolean(_pl); 
            }
            set 
            { 
                _pl = Convert.ToInt32(value);
                if (value) NLE = true; 
            }
        }

        /// <summary>
        /// Gets or sets the nls.
        /// </summary>
        [XmlAttribute("NLS")]
        public int _nls { get; set; }

        /// <summary>
        /// Consider nonlinear behaviour of soil.
        /// </summary>
        [XmlIgnore]
        public bool NLS
        {
            get { return Convert.ToBoolean(_nls); }
            set { _nls = Convert.ToInt32(value); }
        }

        /// <summary>
        /// Gets or sets the cr.
        /// </summary>
        [XmlAttribute("Cr")]
        public int _cr { get; set; }

        /// <summary>
        /// Cracked section analysis. If true, 'PL' must be false. Note that Cr only executes properly in RCDesign with DesignCheck set to true. 
        /// </summary>
        [XmlIgnore]
        public bool Cr
        {
            get 
            { 
                return Convert.ToBoolean(_cr); 
            }
            set 
            { 
                _cr = Convert.ToInt32(value);
                if (value) PL = false;
            }
        }

        /// <summary>
        /// Gets or sets the f2nd.
        /// </summary>
        [XmlAttribute("f2nd")]
        public int _f2nd { get; set; }

        /// <summary>
        /// 2nd order analysis. If true, 'PL' must be false.
        /// </summary>
        [XmlIgnore]
        public bool f2nd
        {
            get 
            { 
                return Convert.ToBoolean(_f2nd); 
            }
            set 
            { 
                _f2nd = Convert.ToInt32(value);
                if (value) PL = false;
            }
        }

        /// <summary>
        /// Gets or sets the im.
        /// </summary>
        [XmlAttribute("Im")]
        public int Im { get; set; }

        /// <summary>
        /// Gets or sets the waterlevel.
        /// </summary>
        [XmlAttribute("Waterlevel")]
        public int Waterlevel { get; set; }

        /// <summary>
        /// Gets or sets the impf rqd.
        /// </summary>
        [XmlAttribute("ImpfRqd")]
        public int ImpfRqd { get; set; }

        /// <summary>
        /// Gets or sets the stab rqd.
        /// </summary>
        [XmlAttribute("StabRqd")]
        public int StabRqd { get; set; }

        /// <summary>
        /// Gets or sets the amplitude.
        /// </summary>
        [XmlAttribute("Amplitudo")]
        public double Amplitude { get; set; }

        /// <summary>
        /// Gets or sets the comb name.
        /// </summary>
        [XmlIgnore]
        public string CombName { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CombItem()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CombItem"/> class.
        /// </summary>
        /// <param name="impfRqd">the impf rqd.</param>
        /// <param name="stabReq">the stab req.</param>
        /// <param name="NLE">the nle.</param>
        /// <param name="PL">the pl.</param>
        /// <param name="NLS">the nls.</param>
        /// <param name="Cr">the cr.</param>
        /// <param name="f2nd">the f2nd.</param>
        /// <param name="Im">the im.</param>
        /// <param name="amplitude">the amplitude.</param>
        /// <param name="waterlevel">the waterlevel.</param>
        public CombItem(int impfRqd = 0, int stabReq = 0, bool NLE = true, bool PL = true, bool NLS = false, bool Cr = false, bool f2nd = false, int Im = 0, double amplitude = 0.0, int waterlevel = 0)
        {
            this.NLE = NLE;
            this.PL = PL;
            this.NLS = NLS;
            this.Cr = Cr;
            this.f2nd = f2nd;
            this.Im = Im;
            this.Waterlevel = waterlevel;
            this.ImpfRqd = impfRqd;
            this.Amplitude = amplitude;
            this.StabRqd = stabReq;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CombItem"/> class.
        /// </summary>
        /// <param name="combName">the comb name.</param>
        /// <param name="impfRqd">the impf rqd.</param>
        /// <param name="stabReq">the stab req.</param>
        /// <param name="NLE">the nle.</param>
        /// <param name="PL">the pl.</param>
        /// <param name="NLS">the nls.</param>
        /// <param name="Cr">the cr.</param>
        /// <param name="f2nd">the f2nd.</param>
        /// <param name="Im">the im.</param>
        /// <param name="amplitude">the amplitude.</param>
        /// <param name="waterlevel">the waterlevel.</param>
        public CombItem(string combName, int impfRqd = 0, int stabReq = 0, bool NLE = true, bool PL = true, bool NLS = false, bool Cr = false, bool f2nd = false, int Im = 0, double amplitude = 0.0, int waterlevel = 0)
        {
            this.CombName = combName;
            this.NLE = NLE;
            this.PL = PL;
            this.NLS = NLS;
            this.Cr = Cr;
            this.f2nd = f2nd;
            this.Im = Im;
            this.Waterlevel = waterlevel;
            this.ImpfRqd = impfRqd;
            this.Amplitude = amplitude;
            this.StabRqd = stabReq;
        }

        /// <summary>
        /// Stability.
        /// </summary>
        /// <param name="stabReq">the stab req.</param>
        /// <returns>The result.</returns>
        public static CombItem Stability(int stabReq)
        {
            var combItem = new CombItem(stabReq: stabReq);
            return combItem;
        }

        /// <summary>
        /// Imperfection.
        /// </summary>
        /// <param name="impfRqd">the impf rqd.</param>
        /// <returns>The result.</returns>
        public static CombItem Imperfection(int impfRqd)
        {
            var combItem = new CombItem(impfRqd: impfRqd);
            return combItem;
        }

        /// <summary>
        /// Non Linear.
        /// </summary>
        /// <param name="plastic">the plastic.</param>
        /// <returns>The result.</returns>
        public static CombItem NonLinear(bool plastic = true)
        {
            var combItem = new CombItem(NLE: true, PL: plastic);
            return combItem;
        }

        /// <summary>
        /// Default.
        /// </summary>
        /// <returns>The result.</returns>
        public static CombItem Default()
        {
            int impfRqd = 0;
            int stabRqd = 0;
            bool NLE = true;
            bool PL = true;
            bool NLS = false;
            bool Cr = false;
            bool f2nd = false;
            int im = 0;
            double amplitude = 0.0;
            int waterlevel = 0;

            var combItem = new CombItem(impfRqd, stabRqd, NLE, PL, NLS, Cr, f2nd, im, amplitude, waterlevel);
            return combItem;
        }
    }
}
