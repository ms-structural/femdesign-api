// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Releases
{
    /// <summary>
    /// Represents a Rigidity Data Lib Type1.
    /// </summary>
    [System.Serializable]
    public partial class RigidityDataLibType1: LibraryBase
    {
        // choice rigidity_data_type1
        /// <summary>
        /// Gets or sets the rigidity.
        /// </summary>
        [XmlElement("rigidity", Order = 1)]
        public Releases.RigidityDataType1 Rigidity { get; set; }

        // choice rigidity_group_typ1
        // [XmlElement("rigidity_group", Order = 2)]    
    }
}