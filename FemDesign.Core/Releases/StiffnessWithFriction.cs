// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Releases
{
    /// <summary>
    /// Represents a Stiffness With Friction.
    /// </summary>
    [System.Serializable]
    public partial class StiffnessWithFriction: SimpleStiffnessType
    {
        /// <summary>
        /// Gets or sets the friction.
        /// </summary>
        [XmlAttributeAttribute("friction")]
        public double _friction;
        [XmlIgnore]
        public double Friction
        {
            get
            {
                return this._friction;
            }
            set
            {
                this._friction = RestrictedDouble.NonNegMax_1(value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StiffnessWithFriction"/> class.
        /// </summary>
        public StiffnessWithFriction()
        {

        }
    }
}
