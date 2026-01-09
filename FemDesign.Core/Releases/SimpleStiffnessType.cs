// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Releases
{
    /// <summary>
    /// Represents a Simple Stiffness Type.
    /// </summary>
    [System.Serializable]
    public partial class SimpleStiffnessType
    {
        /// <summary>
        /// Gets or sets the mov x.
        /// </summary>
        [XmlElement("mov_x", Order = 1)]
        public StiffBaseType MovX { get; set; }

        /// <summary>
        /// Gets or sets the rot x.
        /// </summary>
        [XmlElement("rot_x", Order = 2)]
        public StiffBaseType RotX { get; set; }

        /// <summary>
        /// Gets or sets the mov y.
        /// </summary>
        [XmlElement("mov_y", Order = 3)]
        public StiffBaseType MovY { get; set; }

        /// <summary>
        /// Gets or sets the rot y.
        /// </summary>
        [XmlElement("rot_y", Order = 4)]
        public StiffBaseType RotY { get; set; }

        /// <summary>
        /// Gets or sets the mov z.
        /// </summary>
        [XmlElement("mov_z", Order = 5)]
        
        public StiffBaseType MovZ { get; set; }

        /// <summary>
        /// Gets or sets the rot z.
        /// </summary>
        [XmlElement("rot_z", Order = 6)]
        public StiffBaseType RotZ { get; set; }

        /// <summary>
        /// Gets or sets the detach.
        /// </summary>
        [XmlAttribute("detach")]
        public DetachType Detach { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public SimpleStiffnessType()
        {

        }
    }
}