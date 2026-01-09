// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Bars
{
    /// <summary>
    /// Represents a Connectivity.
    /// 
    /// Connectivity / End-condition releases
    /// </summary>
    [System.Serializable]
    public partial class Connectivity
    {
        
        [XmlIgnore]
        private bool _tx;

        // binary-rigid
        /// <summary>Translation local-x axis.</summary>
        [XmlAttribute("m_x")]
        public bool Tx
        {
            get
            {
                return this._tx;
            }
            set
            {
                this._tx = value;
            }
        }
        [XmlIgnore]
        private bool _ty;

        /// <summary>Translation local-y axis. </summary>
        [XmlAttribute("m_y")]
        public bool Ty
        {
            get
            {
                return this._ty;
            }
            set
            {
                this._ty = value;
            }
        }

        [XmlIgnore]
        private bool _tz;

        /// <summary>Translation local-z axis.</summary>
        [XmlAttribute("m_z")]
        public bool Tz
        {
            get
            {
                return this._tz;
            }
            set
            {
                this._tz = value;
            }
        }

        [XmlIgnore]
        private bool _rx;

        /// <summary>Rotation around local-x axis.</summary>
        [XmlAttribute("r_x")]
        public bool Rx
        {
            get
            {
                return this._rx;
            }
            set
            {
                this._rx = value;
            }
        }

        [XmlIgnore]
        private bool _ry;

        /// <summary>Rotation around local-y axis.</summary>
        [XmlAttribute("r_y")]
        public bool Ry
        {
            get
            {
                return this._ry;
            }
            set
            {
                this._ry = value;
            }
        }

        [XmlIgnore]
        private bool _rz;
        
        /// <summary>Rotation around local-z axis.</summary>
        [XmlAttribute("r_z")]
        public bool Rz
        {
            get
            {
                return this._rz;
            }
            set
            {
                this._rz = value;
            }
        }

        // semi-rigid       
        /// <summary>
        /// Gets or sets the tx release.
        /// </summary>
        [XmlAttribute("m_x_release")]
        public double _txRelease; // non_neg_max_1e10. Default = 0. Valid only if m_x is false.
        [XmlIgnore]
        public double TxRelease
        {
            get
            {
                return this._txRelease;
            }
            set
            {
                this._txRelease = RestrictedDouble.NonNegMax_1e10(value);
            }
        }
        /// <summary>
        /// Gets or sets the ty release.
        /// </summary>
        [XmlAttribute("m_y_release")]
        public double _tyRelease; // non_neg_max_1e10. Default = 0. Valid only if m_y is false.
        [XmlIgnore]
        public double TyRelease
        {
            get
            {
                return this._tyRelease;
            }
            set
            {
                this._tyRelease = RestrictedDouble.NonNegMax_1e10(value);
            }
        }
        /// <summary>
        /// Gets or sets the tz release.
        /// </summary>
        [XmlAttribute("m_z_release")]
        public double _tzRelease; // non_neg_max_1e10. Default = 0. Valid only if m_z is false.
        [XmlIgnore]
        public double TzRelease
        {
            get
            {
                return this._tzRelease;
            }
            set
            {
                this._tzRelease = RestrictedDouble.NonNegMax_1e10(value);
            }
        }
        /// <summary>
        /// Gets or sets the rx release.
        /// </summary>
        [XmlAttribute("r_x_release")]
        public double _rxRelease; // non_neg_max_1e10. Default = 0. Valid only if r_x is false.
        [XmlIgnore]
        public double RxRelease
        {
            get
            {
                return this._rxRelease;
            }
            set
            {
                this._rxRelease = RestrictedDouble.NonNegMax_1e10(value);
            }
        }
        /// <summary>
        /// Gets or sets the ry release.
        /// </summary>
        [XmlAttribute("r_y_release")]
        public double _ryRelease; // non_neg_max_1e10. Default = 0. Valid only if r_y is false.
        [XmlIgnore]
        public double RyRelease
        {
            get
            {
                return this._ryRelease;
            }
            set
            {
                this._ryRelease = RestrictedDouble.NonNegMax_1e10(value);
            }
        }
        /// <summary>
        /// Gets or sets the rz release.
        /// </summary>
        [XmlAttribute("r_z_release")]
        public double _rzRelease; // non_neg_max_1e10. Default = 0. Valid only if r_z is false.
        [XmlIgnore]
        public double RzRelease
        {
            get
            {
                return this._rzRelease;
            }
            set
            {
                this._rzRelease = RestrictedDouble.NonNegMax_1e10(value);
            }
        }

        public bool IsRigid
        {
            get
            {
                if(this.Tx && this.Ty && this.Tz && this.Rx && this.Ry && this.Rz)
                    return true;
                else
                    return false;
            }
        }

        public bool IsHinged
        {
            get
            {
                if (this.Tx && this.Ty && this.Tz && this.Rx && this.Ry == false && this.Rz == false)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Connectivity()
        {
        }
        
        /// <summary>
        /// Private constructor for binary-rigid definition.
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <param name="tz"></param>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        /// <param name="rz"></param>
        public Connectivity(bool tx, bool ty, bool tz, bool rx, bool ry, bool rz)
        {
            this.Tx = tx;
            this.Ty = ty;
            this.Tz = tz;
            this.Rx = rx;
            this.Ry = ry;
            this.Rz = rz;
        }

        /// <summary>
        /// Private constructor for semi-rigid definition.
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <param name="tz"></param>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        /// <param name="rz"></param>
        /// <param name="txRelease"></param>
        /// <param name="tyRelease"></param>
        /// <param name="tzRelease"></param>
        /// <param name="rxRelease"></param>
        /// <param name="ryRelease"></param>
        /// <param name="rzRelease"></param>
        public Connectivity(bool tx, bool ty, bool tz, bool rx, bool ry, bool rz, double txRelease, double tyRelease, double tzRelease, double rxRelease, double ryRelease, double rzRelease)
        {
            this.Tx = tx;
            this.Ty = ty;
            this.Tz = tz;
            this.Rx = rx;
            this.Ry = ry;
            this.Rz = rz;
            this.TxRelease = txRelease;
            this.TyRelease = tyRelease;
            this.TzRelease = tzRelease;
            this.RxRelease = rxRelease;
            this.RyRelease = ryRelease;
            this.RzRelease = rzRelease;
        }

        /// <summary>
        /// Define default (rigid) releases for a bar-element.
        /// </summary>
        /// <returns></returns>
        public static Connectivity Default => Connectivity.Rigid;


        /// <summary>
        /// Define hinged releases for a bar-element.
        /// </summary>
        /// <remarks>Create</remarks>
        public static Connectivity Hinged
        {
            get
            {
                Connectivity connectivity = new Connectivity(true, true, true, true, false, false);
                return connectivity;
            }
        }
        /// <summary>
        /// Define rigid releases for a bar-element.
        /// </summary>
        /// <remarks>Create</remarks>
        public static Connectivity Rigid
        {
            get
            {
                    Connectivity connectivity = new Connectivity(true, true, true, true, true, true);
                    return connectivity;

            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            if (IsRigid)
                return $"{this.GetType().Name} Rigid";
            else if (IsHinged)
                return $"{this.GetType().Name} Hinged";
            else
            {
                var tx = this.Tx == true ? "Rigid" : $"{this.TxRelease} kN/m";
                var ty = this.Ty == true ? "Rigid" : $"{this.TyRelease} kN/m";
                var tz = this.Tz == true ? "Rigid" : $"{this.TzRelease} kN/m";

                var rx = this.Rx == true ? "Rigid" : $"{this.RxRelease} kNm/rad";
                var ry = this.Ry == true ? "Rigid" : $"{this.RyRelease} kNm/rad";
                var rz = this.Rz == true ? "Rigid" : $"{this.RzRelease} kNm/rad";

                return $"{this.GetType().Name} Tx: {tx}, Ty: {ty}, Tz: {tz}, Rx: {rx}, Ry: {ry}, Rz: {rz}";
            }
        }
    }
}