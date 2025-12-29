// https://strusoft.com/
using System;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_base_attribs
    /// </summary>
    [System.Serializable]
    public partial class LoadGroupBase
    {
        [XmlIgnore]
        public string Name { get; set; }

        [XmlIgnore]
        public List<ModelLoadCaseInGroup> ModelLoadCase { get; set; }

        [XmlAttribute("relationship")]
        public ELoadGroupRelationship Relationship { get; set; } = ELoadGroupRelationship.Alternative;

        [XmlIgnore]
        public List<LoadCase> LoadCase = new List<LoadCase>(); // List of complete load cases

        [XmlElement("subgroup", Order = 3)]
        public List<StruSoft.Interop.StruXml.Data.Load_subgroup> Subgroups { get; set; }

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
