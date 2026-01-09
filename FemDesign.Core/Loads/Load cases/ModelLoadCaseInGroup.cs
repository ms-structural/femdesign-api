// https://strusoft.com/

using System.Xml.Serialization;
using System.Collections.Generic;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_case (child of load_group_table_type)
    /// </summary>
    [System.Serializable]
    public partial class ModelLoadCaseInGroup
    {
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        [XmlAttribute("guid")]
        public System.Guid Guid { get; set; } // common_load_case --> guidtype indexed_guid
        /// <summary>
        /// Gets or sets the load group.
        /// </summary>
        [XmlIgnore]
        public LoadGroupBase LoadGroup { get; set; }

        /// parameterless constructor for serialization
        private ModelLoadCaseInGroup() { }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="guid">LoadCase guid reference.</param>
        /// <param name="parentLoadGroup">Parent load group that the load case belongs to.</param>
        public ModelLoadCaseInGroup(System.Guid guid, LoadGroupBase parentLoadGroup)
        {
            this.Guid = guid;
            LoadGroup = parentLoadGroup;
        }     
    }
}