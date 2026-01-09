// https://strusoft.com/

using System.Xml.Serialization;
using System.ComponentModel;

namespace FemDesign.Reinforcement
{
    /// <summary>
    /// Represents a Wire.
    /// 
    /// Reinforcement wire
    /// </summary>
    [System.Serializable]
    public partial class Wire
    {
        [XmlIgnore]
        private Materials.Material _reinforcingMaterial;
        [XmlIgnore]
        public Materials.Material ReinforcingMaterial
        {
            get
            {
                return this._reinforcingMaterial;
            }
            set
            {
                if (value.ReinforcingSteel == null)
                {
                    throw new System.ArgumentException("Material must be a reinforcing material.");
                }
                else
                {
                    this._reinforcingMaterial = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        [XmlAttribute("diameter")]
        public double _diameter; // rc_diameter_value
        [XmlIgnore]
        public double Diameter
        {
            get {return this._diameter;}
            set {this._diameter = RestrictedDouble.RcDiameterValue(value);}
        }
        /// <summary>
        /// Gets or sets the reinforcing material guid.
        /// </summary>
        [XmlAttribute("reinforcing_material")]
        public System.Guid ReinforcingMaterialGuid { get; set; } // guidtype
        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        [XmlAttribute("profile")]
        [DefaultValue(1)]
        public WireProfileType Profile { get; set; } = WireProfileType.Ribbed;

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Wire()
        {

        }

        /// <summary>
        /// Reinforcement wire.
        /// </summary>
        /// <param name="diameter">the diameter.</param>
        /// <param name="reinforcingMaterial">the reinforcing material.</param>
        /// <param name="profile">the profile.</param>
        public Wire(double diameter, Materials.Material reinforcingMaterial, WireProfileType profile)
        {
            this.ReinforcingMaterial = reinforcingMaterial;
            this.Diameter = diameter;
            this.ReinforcingMaterialGuid = reinforcingMaterial.Guid;
            this.Profile = profile;
        }
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{this.GetType().Name} - {this.ReinforcingMaterial}; D={this.Diameter*1000} mm; {this.Profile}";
        }
    }
}