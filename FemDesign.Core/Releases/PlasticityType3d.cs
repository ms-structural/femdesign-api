// https://strusoft.com/

using System;
using System.Globalization;
using System.Xml.Serialization;

namespace FemDesign.Releases
{
    /// <summary>
    /// Represents a Plasticity Type3d.
    /// </summary>
    [Serializable]
    public partial class PlasticityType3d : StiffnessType
    {
        [XmlIgnore]
        public new double? XNeg
        {
            get { if (string.IsNullOrEmpty(this._xNeg)) return null; else return double.Parse(this._xNeg); }
            set { if (value != null) this._xNeg = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); else this._xNeg = null; }
        }
        [XmlIgnore]
        public new double? XPos
        {
            get { if (string.IsNullOrEmpty(this._xPos)) return null; else return double.Parse(this._xPos); }
            set { if (value != null) this._xPos = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); else this._xPos = null; }
        }
        [XmlIgnore]
        public new double? YNeg
        {
            get { if (string.IsNullOrEmpty(this._yNeg)) return null; else return double.Parse(this._yNeg); }
            set { if (value != null) this._yNeg = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); else this._yNeg = null; }
        }
        [XmlIgnore]
        public new double? YPos
        {
            get { if (string.IsNullOrEmpty(this._yPos)) return null; else return double.Parse(this._yPos); }
            set { if (value != null) this._yPos = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); else this._yPos = null; }
        }
        [XmlIgnore]
        public new double? ZNeg
        {
            get { if (string.IsNullOrEmpty(this._zNeg)) return null; else return double.Parse(this._zNeg); }
            set { if (value != null) this._zNeg = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); else this._zNeg = null; }
        }
        [XmlIgnore]
        public new double? ZPos
        {
            get { if (string.IsNullOrEmpty(this._zPos)) return null; else return double.Parse(this._zPos); }
            set { if (value != null) this._zPos = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); else this._zPos = null; }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public PlasticityType3d()
        {

        }
    }
}