// https://strusoft.com/

using System.Globalization;
using System.Xml.Serialization;


namespace FemDesign.Releases
{
    /// <summary>
    /// Represents a Rigidity Data Type3.
    /// </summary>
    [System.Serializable]
    public partial class RigidityDataType3: RigidityDataType2
    {
        /// <summary>
        /// Type string in order to make field nullable. When null FEM-Design will load default value.
        /// </summary>
        [XmlAttribute("friction")]
        public string _friction; // reduction_factor_type. Default = 0.3
        [XmlIgnore]
        public double Friction 
        {
            get
            {
                if (this._friction == null)
                {
                    throw new System.ArgumentException("_friction is null");
                }
                else
                {    
                    return System.Convert.ToDouble(this._friction, CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this._friction = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private RigidityDataType3()
        {
        }

        /// <summary>
        /// Construct RigidityDataType3 with default friction
        /// </summary>
        /// <param name="motions">the motions.</param>
        /// <param name="rotations">the rotations.</param>
        public RigidityDataType3(Motions motions, Rotations rotations) : base(motions, rotations)
        {
        }

        /// <summary>
        /// Construct RigidityDataType3 with default friction
        /// </summary>
        /// <param name="motions">the motions.</param>
        /// <param name="motionsPlasticLimits">the motions plastic limits.</param>
        /// <param name="rotations">the rotations.</param>
        /// <param name="rotationsPlasticLimits">the rotations plastic limits.</param>
        public RigidityDataType3(Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits) : base(motions, motionsPlasticLimits, rotations, rotationsPlasticLimits)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RigidityDataType3"/> class.
        /// </summary>
        /// <param name="motions">the motions.</param>
        /// <param name="motionsPlasticLimits">the motions plastic limits.</param>
        /// <param name="rotations">the rotations.</param>
        /// <param name="rotationsPlasticLimits">the rotations plastic limits.</param>
        /// <param name="friction">the friction.</param>
        /// <param name="detachType">the detach type.</param>
        public RigidityDataType3(Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits, double friction, DetachType detachType) : base(motions, motionsPlasticLimits, rotations, rotationsPlasticLimits)
        {
            this.Friction = friction;
            this.DetachType = detachType;
        }

        /// <summary>
        /// Construct RigidityDataType3 with defined friction
        /// </summary>
        /// <param name="motions">the motions.</param>
        /// <param name="rotations">the rotations.</param>
        /// <param name="friction">the friction.</param>
        public RigidityDataType3(Motions motions, Rotations rotations, double friction) : base(motions, rotations)
        {
            this.Friction = friction;
        }

        /// <summary>
        /// Construct simple hinged line RidigityDataType3.
        /// </summary>
        /// <returns>The result.</returns>
        public static RigidityDataType3 HingedLine()
        {
            return new RigidityDataType3(Motions.RigidLine(), Rotations.Free());
        }

        /// <summary>
        /// Construct simple rigid line RigidityDataType3.
        /// </summary>
        /// <returns>The result.</returns>
        public static RigidityDataType3 RigidLine()
        {
            return new RigidityDataType3(Motions.RigidLine(), Rotations.RigidLine());
        }


    }
}