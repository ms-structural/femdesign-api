// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Releases
{
    /// <summary>
    /// Represents a Rigidity Data Type0.
    /// </summary>
    [System.Serializable]
    public partial class RigidityDataType0
    {
        /// <summary>
        /// Gets or sets the motions.
        /// </summary>
        [XmlElement("motions", Order = 1)]
        public Releases.Motions Motions { get; set; }
        /// <summary>
        /// Gets or sets the plastic limit forces.
        /// </summary>
        [XmlElement("plastic_limit_forces", Order = 2)]
        public Releases.MotionsPlasticLimits PlasticLimitForces { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        public RigidityDataType0()
        {

        }

        /// <summary>
        /// Construct RigidityDataType1 with motions only
        /// </summary>
        /// <param name="motions">the motions.</param>
        public RigidityDataType0(Motions motions)
        {
            this.Motions = motions;
        }

        /// <summary>
        /// Construct RigidityDataType1 with motions and plastic limits forces only
        /// </summary>
        /// <param name="motions">the motions.</param>
        /// <param name="motionsPlasticLimits">the motions plastic limits.</param>
        public RigidityDataType0(Motions motions, MotionsPlasticLimits motionsPlasticLimits)
        {
            this.Motions = motions;
            this.PlasticLimitForces = motionsPlasticLimits;
        }
    }
}