// https://strusoft.com/

using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_case (child of load_group_table)
    /// </summary>
    [System.Serializable]
    public partial class ModelGeneralLoadGroup
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        [XmlAttribute("guid")]
        public System.Guid Guid { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Gets or sets the consider in gmax.
        /// </summary>
        [XmlAttribute("consider_in_gmax")]
        [DefaultValue(true)]
        public bool ConsiderInGmax { get; set; } = true;
        /// <summary>
        /// Gets or sets the accidental load group.
        /// </summary>
        [XmlElement("accidental", Order = 1)]
        public StruSoft.Interop.StruXml.Data.Accidental_load_group AccidentalLoadGroup { get; set; }

        /// <summary>
        /// Gets or sets the model load group permanent.
        /// </summary>
        [XmlElement("permanent", Order = 2)]
        public LoadGroupPermanent ModelLoadGroupPermanent { get; set; }
        /// <summary>
        /// Gets or sets the seismic load group.
        /// </summary>
        [XmlElement("seismic", Order = 3)]
        public StruSoft.Interop.StruXml.Data.Seismic_load_group SeismicLoadGroup { get; set; }
        /// <summary>
        /// Gets or sets the stress load group.
        /// </summary>
        [XmlElement("stress", Order = 4)]
        public StruSoft.Interop.StruXml.Data.Stress_load_group StressLoadGroup { get; set; }
        /// <summary>
        /// Gets or sets the model load group temporary.
        /// </summary>
        [XmlElement("temporary", Order = 5)]
        public LoadGroupTemporary ModelLoadGroupTemporary { get; set; }

        /// <summary>
        /// parameterless constructor for serialization
        /// </summary>
        public ModelGeneralLoadGroup()
        {
        }

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="LoadGroup">Specific load group object</param>
        public ModelGeneralLoadGroup(LoadGroupBase LoadGroup)
        {
            AddSpecificLoadGroup(LoadGroup, false);
            Name = LoadGroup.Name;
        }

        /// <summary>
        /// Gets the load cases of the specific load group that the general load group owns
        /// </summary>
        /// <returns>List of load cases</returns>
        public List<LoadCase> GetLoadCases()
        {
            List<LoadCase> loadCases =  new List<LoadCase>();
            if (ModelLoadGroupPermanent != null)
                loadCases = ModelLoadGroupPermanent.LoadCase;
            else if (ModelLoadGroupTemporary != null)
                loadCases = ModelLoadGroupTemporary.LoadCase;

            return loadCases;
        }

        /// <summary>
        /// Get LoadCase guids of LoadCases in LoadCombination.
        /// </summary>
        /// <returns>List of load case guids</returns>
        public List<string> GetLoadCaseGuidsAsString()
        {
            var loadCaseGuids = new List<string>();
            if (ModelLoadGroupPermanent != null)
                foreach (ModelLoadCaseInGroup item in ModelLoadGroupPermanent.ModelLoadCase)
                    loadCaseGuids.Add(item.Guid.ToString());
            else if (ModelLoadGroupTemporary != null)
                foreach (ModelLoadCaseInGroup item in ModelLoadGroupTemporary.ModelLoadCase)
                    loadCaseGuids.Add(item.Guid.ToString());
            return loadCaseGuids;
        }

        /// <summary>
        /// Gets the load group type of the specific load group that the general load group owns
        /// </summary>
        /// <returns>The type pf the load group</returns>
        public ELoadGroupType GetLoadGroupType()
        {
            LoadGroupBase specificLoadGroup = GetSpecificLoadGroup();
            ELoadGroupType type = ELoadGroupType.Permanent;
            if (specificLoadGroup is LoadGroupPermanent)
                type = ELoadGroupType.Permanent;
            else if (specificLoadGroup is LoadGroupTemporary)
                type = ELoadGroupType.Temporary;
            return type;
        }

        /// <summary>
        /// Assignes the load group to the correct field depending on its type
        /// </summary>
        /// <param name="loadGroup"><see cref="LoadGroupBase">A specific load group instance, derived from LoadGroupBase</see></param>
        /// <param name="replaceExisting">True if the general load group already contains a specific load group</param>
        public void AddSpecificLoadGroup(LoadGroupBase loadGroup, bool replaceExisting)
        {
            if (loadGroup is LoadGroupPermanent)
                if ((GetSpecificLoadGroup() == null) || replaceExisting)
                    this.ModelLoadGroupPermanent = (LoadGroupPermanent)loadGroup;
                else
                    throw new System.ArgumentException("There already exists a specific load group in the general load group");
            else if (loadGroup is LoadGroupTemporary)
                if ((GetSpecificLoadGroup() == null) || replaceExisting)
                    this.ModelLoadGroupTemporary = (LoadGroupTemporary)loadGroup;
            else
                throw new System.ArgumentException("Load group type not yet implemented");
        }

        /// <summary>
        /// Gets the specific load group from one of the general load groups fields 
        /// </summary>
        /// <returns></returns>
        public LoadGroupBase GetSpecificLoadGroup()
        {
            LoadGroupBase specificLoadGroup = null;

            if (ModelLoadGroupPermanent != null)
                specificLoadGroup = ModelLoadGroupPermanent;
            else if (ModelLoadGroupTemporary != null)
                specificLoadGroup = ModelLoadGroupTemporary;

            return specificLoadGroup;
        }
    }
}