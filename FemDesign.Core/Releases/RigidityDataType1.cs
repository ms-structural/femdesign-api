// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Releases
{
    /// <summary>
    /// Represents a Rigidity Data Type1.
    /// </summary>
    [System.Serializable]
    public partial class RigidityDataType1
    {
        /// <summary>
        /// Gets or sets the motions.
        /// </summary>
        [XmlElement("motions", Order=1)]
        public Releases.Motions Motions { get; set; }
        /// <summary>
        /// Gets or sets the plastic limit forces.
        /// </summary>
        [XmlElement("plastic_limit_forces", Order=2)]
        public Releases.MotionsPlasticLimits PlasticLimitForces { get; set; }

        /// <summary>
        /// Gets or sets the deach type.
        /// </summary>
        [XmlAttribute("detach")]
        public DetachType _deachType = DetachType.None;

        [XmlIgnore]
        public DetachType DetachType
        {
            get
            {
                return this._deachType;
            }
            set
            {
                this._deachType = value;
                if (value == DetachType.X_Compression)
                {
                    this.Motions.XNeg = 0.00;
                }
                else if (value == DetachType.X_Tension)
                {
                    this.Motions.XPos = 0.00;
                }
                else if (value == DetachType.Y_Compression)
                {
                    this.Motions.YNeg = 0.00;
                }
                else if (value == DetachType.Y_Tension)
                {
                    this.Motions.YPos = 0.00;
                }
                else if (value == DetachType.Z_Compression)
                {
                    this.Motions.ZNeg = 0.00;
                }
                else if (value == DetachType.Z_Tension)
                {
                    this.Motions.ZPos = 0.00;
                }

            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        public RigidityDataType1()
        {
            
        }

        /// <summary>
        /// Construct RigidityDataType1 with motions only
        /// </summary>
        /// <param name="motions">the motions.</param>
        public RigidityDataType1(Motions motions)
        {
            this.Motions = motions;
        }

        /// <summary>
        /// Construct RigidityDataType1 with motions and plastic limits forces only
        /// </summary>
        /// <param name="motions">the motions.</param>
        /// <param name="motionsPlasticLimits">the motions plastic limits.</param>
        /// <param name="detachType">the detach type.</param>
        public RigidityDataType1(Motions motions, MotionsPlasticLimits motionsPlasticLimits, DetachType detachType = DetachType.None)
        {
            this.Motions = motions;
            this.PlasticLimitForces = motionsPlasticLimits;
            this.DetachType = detachType;
        }
    }
}