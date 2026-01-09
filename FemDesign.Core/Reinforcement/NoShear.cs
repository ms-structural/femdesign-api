using System;
using System.Xml.Serialization;


namespace FemDesign.Reinforcement
{
    /// <summary>
    /// Represents a No Shear Auto Type.
    /// </summary>
    [System.Serializable]
    public partial class NoShearAutoType
    {
        /// <summary>
        /// Gets or sets the connected structure.
        /// </summary>
        [XmlAttribute("connected_structure")]
        public Guid ConnectedStructure { get; set; }

        /// <summary>
        /// Gets or sets the factor.
        /// </summary>
        [XmlAttribute("factor")]
        public double Factor { get; set; }

        /// <summary>
        /// Gets or sets the inactive.
        /// </summary>
        [XmlAttribute("inactive")]
        public bool Inactive { get; set; }
    }

    /// <summary>
    /// Represents a No Shear Region Type.
    /// </summary>
    [System.Serializable]
    public partial class NoShearRegionType: EntityBase
    {
        /// <summary>
        /// Gets or sets the automatic.
        /// </summary>
        [XmlElement("automatic", Order = 1)]
        public NoShearAutoType Automatic { get; set; }

        /// <summary>
        /// Gets or sets the contour.
        /// </summary>
        [XmlElement("contour", Order = 2)]
        public Geometry.Contour Contour { get; set; }

        /// <summary>
        /// Gets or sets the base plate.
        /// </summary>
        [XmlAttribute("base_plate")]
        public Guid BasePlate { get; set; }
    }
}