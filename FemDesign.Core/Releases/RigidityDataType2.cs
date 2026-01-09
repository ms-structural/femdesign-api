// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Releases
{
    /// <summary>
    /// Represents a Rigidity Data Type2.
    /// </summary>
    [System.Serializable]
    public partial class RigidityDataType2: RigidityDataType1
    {
        /// <summary>
        /// Gets or sets the rotations.
        /// </summary>
        [XmlElement("rotations", Order=3)]
        public Releases.Rotations Rotations { get; set; }
        /// <summary>
        /// Gets or sets the plastic limit moments.
        /// </summary>
        [XmlElement("plastic_limit_moments", Order=4)]
        public Releases.RotationsPlasticLimits PlasticLimitMoments { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RigidityDataType2"/> class.
        /// </summary>
        public RigidityDataType2()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RigidityDataType2"/> class.
        /// </summary>
        /// <param name="motions">the motions.</param>
        /// <param name="rotations">the rotations.</param>
        public RigidityDataType2(Motions motions, Rotations rotations) : base(motions)
        {
            Rotations = rotations;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RigidityDataType2"/> class.
        /// </summary>
        /// <param name="motions">the motions.</param>
        /// <param name="motionsPlasticLimits">the motions plastic limits.</param>
        /// <param name="rotations">the rotations.</param>
        /// <param name="rotationsPlasticLimits">the rotations plastic limits.</param>
        public RigidityDataType2(Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits) : base(motions, motionsPlasticLimits)
        {
            Rotations = rotations;
            PlasticLimitMoments = rotationsPlasticLimits;
        }
    }
}