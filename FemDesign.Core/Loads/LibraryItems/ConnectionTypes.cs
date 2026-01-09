// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.LibraryItems
{
    /// <summary>
    /// Represents a Point Connection Types.
    /// </summary>
    [System.Serializable]
    public partial class PointConnectionTypes
    {
        /// <summary>
        /// Gets or sets the predefined types.
        /// </summary>
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType2> PredefinedTypes { get; set; }
    }

    /// <summary>
    /// Represents a Point Support Group Types.
    /// </summary>
    [System.Serializable]
    public partial class PointSupportGroupTypes
    {
        /// <summary>
        /// Gets or sets the predefined types.
        /// </summary>
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType2> PredefinedTypes { get; set; }
    }

    /// <summary>
    /// Represents a Line Connection Types.
    /// </summary>
    [System.Serializable]
    public partial class LineConnectionTypes
    {
        /// <summary>
        /// Gets or sets the predefined types.
        /// </summary>
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType3> PredefinedTypes { get; set; }
    }

    /// <summary>
    /// Represents a Line Support Group Types.
    /// </summary>
    [System.Serializable]
    public partial class LineSupportGroupTypes
    {
        /// <summary>
        /// Gets or sets the predefined types.
        /// </summary>
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType2> PredefinedTypes { get; set; }
    }

    /// <summary>
    /// Represents a Surface Connection Types.
    /// </summary>
    [System.Serializable]
    public partial class SurfaceConnectionTypes
    {
        /// <summary>
        /// Gets or sets the predefined types.
        /// </summary>
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType1> PredefinedTypes { get; set; }
    }

    /// <summary>
    /// Represents a Surface Support Types.
    /// </summary>
    [System.Serializable]
    public partial class SurfaceSupportTypes
    {
        /// <summary>
        /// Gets or sets the predefined types.
        /// </summary>
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType1> PredefinedTypes { get; set; }
    }
}
