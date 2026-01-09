using System;
using System.Xml.Serialization;


namespace FemDesign.Shells
{
    /// <summary>
    /// Represents a Shell Buckling Type.
    /// </summary>
    [System.Serializable]
    public partial class ShellBucklingType: EntityBase
    {
        /// <summary>
        /// Gets or sets the local x.
        /// </summary>
        [XmlElement("direction", Order = 1)]
        public Geometry.Vector3d LocalX { get; set; }

        /// <summary>
        /// Gets or sets the contour.
        /// </summary>
        [XmlElement("contour", Order = 2)]
        public Geometry.Contour Contour { get; set; }

        /// <summary>
        /// Gets or sets the base shell.
        /// </summary>
        [XmlAttribute("base_shell")]
        public Guid BaseShell { get; set; }

        /// <summary>
        /// Gets or sets the beta.
        /// </summary>
        [XmlAttribute("beta")]
        public double Beta { get; set; }
    }
}