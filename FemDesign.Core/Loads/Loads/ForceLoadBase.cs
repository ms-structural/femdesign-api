// https://strusoft.com/
using FemDesign.Loads;
using System;
using System.Xml.Serialization;


namespace FemDesign
{
    /// <summary>
    /// Represents a Force Load Base.
    /// </summary>
    [System.Serializable]
    public partial class ForceLoadBase: LoadBase
    {
        /// <summary>
        /// Specifies if the load vector describes a force or a moment
        /// </summary>
        [XmlAttribute("load_type")] // force_load_type
        public Loads.ForceLoadType LoadType { get; set; }

        /// <summary>
        /// ElementPart Guid to assign the load to.
        /// </summary>
        [XmlAttribute("assigned_structure")]
        public Guid AssignedStructure { get; set; }

    }

    /// <summary>
    /// Represents a Support Motion Base.
    /// </summary>
    [System.Serializable]
    public partial class SupportMotionBase : LoadBase
    {
        /// <summary>
        /// Gets or sets the support motion type.
        /// </summary>
        [XmlAttribute("load_type")]
        public SupportMotionType SupportMotionType { get; set; }
    }

}