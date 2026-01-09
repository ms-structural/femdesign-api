// https://strusoft.com/

using System;
using System.Globalization;
using System.Xml.Serialization;

namespace FemDesign.Releases
{
    /// <summary>
    /// Represents a Stiffness Type.
    /// </summary>
    [Serializable]
    public partial class StiffnessType
    {
        /// <summary>
        /// Gets or sets the x neg.
        /// </summary>
        [XmlAttribute("x_neg")]
        public string _xNeg; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double XNeg
        {
            get { return double.Parse(this._xNeg); }
            set { this._xNeg = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); }
        }
        /// <summary>
        /// Gets or sets the x pos.
        /// </summary>
        [XmlAttribute("x_pos")]
        
        public string _xPos; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double XPos
        {
            get { return double.Parse(this._xPos); }
            set { this._xPos = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); }
        }
        /// <summary>
        /// Gets or sets the y neg.
        /// </summary>
        [XmlAttribute("y_neg")]
        
        public string _yNeg; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double YNeg
        {
            get { return double.Parse(this._yNeg); }
            set { this._yNeg = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); }
        }
        /// <summary>
        /// Gets or sets the y pos.
        /// </summary>
        [XmlAttribute("y_pos")]
        
        public string _yPos; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double YPos
        {
            get { return double.Parse(this._yPos); }
            set { this._yPos = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); }
        }
        /// <summary>
        /// Gets or sets the z neg.
        /// </summary>
        [XmlAttribute("z_neg")]
        
        public string _zNeg; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double ZNeg
        {
            get { return double.Parse(this._zNeg); }
            set { this._zNeg = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); }
        }
        /// <summary>
        /// Gets or sets the z pos.
        /// </summary>
        [XmlAttribute("z_pos")]
        public string _zPos; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double ZPos
        {
            get { return double.Parse(this._zPos); }
            set { this._zPos = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public StiffnessType()
        {

        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{this.GetType().Name} x: (-{this.XNeg.ToString("0.000e0")}/{this.XPos.ToString("0.000e0")}), y: (-{this.YNeg.ToString("0.000e0")}/{this.YPos.ToString("0.000e0")}), z: (-{this.ZNeg.ToString("0.000e0")}/{this.ZPos.ToString("0.000e0")})";
        }
    }
}