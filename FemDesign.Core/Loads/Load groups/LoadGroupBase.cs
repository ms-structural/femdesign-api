// https://strusoft.com/
using System;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Load Group Base.
    /// </summary>
    [System.Serializable]
    public partial class LoadGroupBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlIgnore]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the model load case.
        /// </summary>
        [XmlElement("load_case", Order = 2)]
        public List<ModelLoadCaseInGroup> ModelLoadCase { get; set; }

        /// <summary>
        /// Gets or sets the relationship.
        /// </summary>
        [XmlAttribute("relationship")]
        public ELoadGroupRelationship Relationship { get; set; } = ELoadGroupRelationship.Alternative;

        /// <summary>
        /// Gets or sets the load case.
        /// </summary>
        [XmlIgnore]
        public List<LoadCase> LoadCase = new List<LoadCase>(); // List of complete load cases

        /// <summary>
        /// Gets or sets the subgroups.
        /// </summary>
        [XmlElement("subgroup", Order = 3)]
        public List<StruSoft.Interop.StruXml.Data.Load_subgroup> Subgroups { get; set; }

        /// <summary>
        /// Gets or sets the relation table.
        /// </summary>
        [XmlElement("relations", Order = 4)]
        public RelationTable RelationTable { get; set; }

        /// <summary>
        /// Find the corresponding LoadCase instance stored in the load group based on the guid of the modelLoadCaseInGroup instance
        /// </summary>
        /// <param name="modelLoadCaseInGroup">Model load case to find corresponding complete LoadCase instance of</param>
        /// <returns>The LoadCase that has the same guid</returns>
        public LoadCase GetCorrespondingCompleteLoadCase(ModelLoadCaseInGroup modelLoadCaseInGroup)
        {
            LoadCase correspodningLoadCase = LoadCase.Find(i => i.Guid == modelLoadCaseInGroup.Guid);
            return correspodningLoadCase;
        }

        /// <summary>
        /// Check if LoadCase in LoadGroup.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="loadCase">the load case.</param>
        public bool LoadCaseInLoadGroup(LoadCase loadCase)
        {
            if (ModelLoadCase == null)
                return false;

            foreach (ModelLoadCaseInGroup elem in this.ModelLoadCase)
            {
                if (elem.Guid == loadCase.Guid)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
