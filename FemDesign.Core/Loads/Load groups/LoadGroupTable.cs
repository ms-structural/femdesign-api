using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using StruSoft.Interop.StruXml.Data;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_group_table
    /// </summary>
    [System.Serializable]
    public partial class LoadGroupTable
    {
        [XmlAttribute("last_change")]
        public string _lastChange;
        [XmlIgnore]
        internal DateTime LastChange
        {
            get
            {
                return DateTime.Parse(this._lastChange);
            }
            set
            {
                this._lastChange = value.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
            }
        }
        [XmlAttribute("action")]
        public string Action { get; set; }
        [XmlAttribute("simple_combination_method")]
        public LoadCombinationMethod SimpleCombinationMethod { get; set; } = LoadCombinationMethod.False;

        [XmlElement("custom_table")]
        public StruSoft.Interop.StruXml.Data.Ldgrp_ct_type CustomTable { get; set; }

        [XmlElement("group")]
        public List<ModelGeneralLoadGroup> GeneralLoadGroups { get; set; } = new List<ModelGeneralLoadGroup>(); // sequence: ModelGeneralLoadGroup

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LoadGroupTable() { }

        /// <summary>
        /// Internal constructor. Used for GH components and Dynamo nodes.
        /// </summary>

        public LoadGroupTable(List<ModelGeneralLoadGroup> loadGroups, LoadCombinationMethod combinationMethod = LoadCombinationMethod.False)
        {
            EntityCreated();
            foreach (ModelGeneralLoadGroup loadGroup in loadGroups)
                AddGeneralLoadGroup(loadGroup);
            this.SimpleCombinationMethod = combinationMethod;
        }

        /// <summary>
        /// Add GeneralLoadGroup to LoadGroupTable.
        /// </summary>
        private void AddGeneralLoadGroup(ModelGeneralLoadGroup generalLoadGroup)
        {
            if (this.LoadGroupInLoadGroupTable(generalLoadGroup))
            {
                // pass
            }
            else
            {
                this.GeneralLoadGroups.Add(generalLoadGroup);
            }
        }

        /// <summary>
        /// Check if GeneralLoadGroup is in LoadGroupTable.
        /// </summary>
        private bool LoadGroupInLoadGroupTable(ModelGeneralLoadGroup generalLoadGroup)
        {
            foreach (ModelGeneralLoadGroup elem in this.GeneralLoadGroups)
            {
                if (elem.Guid == generalLoadGroup.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Invoke when an instance is created.
        /// </summary>
        public void EntityCreated()
        {
            LastChange = DateTime.UtcNow;
            Action = "added";
        }

        /// <summary>
        /// Invoke when an instance is modified.
        /// 
        /// Changes timestamp and action.
        /// </summary>
        public void EntityModified()
        {
            this.LastChange = DateTime.UtcNow;
            this.Action = "modified";
        }

        /// <summary>
        /// Apply custom combination table from parsed CSV data.
        /// This method should be called when SimpleCombinationMethod is set to Custom.
        /// </summary>
        /// <param name="customTable">Parsed custom combination table data.</param>
        public void ApplyCustomTable(CustomCombinationTable customTable)
        {
            if (customTable == null)
                throw new ArgumentNullException("customTable");

            if (customTable.MainTableRecords.Count == 0)
                throw new ArgumentException("Custom table has no records.");

            // Set main custom_table
            this.CustomTable = new Ldgrp_ct_type
            {
                Items = customTable.MainTableRecords
            };

            // Apply to each load group
            for (int i = 0; i < this.GeneralLoadGroups.Count; i++)
            {
                var group = this.GeneralLoadGroups[i];
                
                if (!customTable.LoadGroupRecords.ContainsKey(i))
                    throw new ArgumentException($"Custom table does not contain records for load group at index {i}.");

                var records = customTable.LoadGroupRecords[i];

                if (group.ModelLoadGroupPermanent != null)
                {
                    group.ModelLoadGroupPermanent.CustomTable = new PermanentGroupRecord
                    {
                        Record = records.Cast<Permanent_load_groupRecord>().ToList()
                    };
                }
                else if (group.ModelLoadGroupTemporary != null)
                {
                    group.ModelLoadGroupTemporary.CustomTable = new TemporaryGroupRecord
                    {
                        Record = records.Cast<Temporary_load_groupRecord>().ToList()
                    };
                }
                else if (group.AccidentalLoadGroup != null)
                {
                    group.AccidentalLoadGroup.Custom_table = records.Cast<Accidental_load_groupRecord>().ToList();
                }
                else if (group.StressLoadGroup != null)
                {
                    group.StressLoadGroup.Custom_table = records.Cast<Stress_load_groupRecord>().ToList();
                }
            }

            EntityModified();
        }

        /// <summary>
        /// Create a LoadGroupTable with custom combination method from a CSV file.
        /// </summary>
        /// <param name="loadGroups">List of load groups in order.</param>
        /// <param name="csvFilePath">Path to the CSV file containing custom combination data.</param>
        /// <returns>LoadGroupTable with custom combination method applied.</returns>
        public static LoadGroupTable FromCsv(List<ModelGeneralLoadGroup> loadGroups, string csvFilePath)
        {
            var table = new LoadGroupTable(loadGroups, LoadCombinationMethod.Custom);
            var customTable = CustomCombinationTable.FromCsv(csvFilePath, loadGroups);
            table.ApplyCustomTable(customTable);
            return table;
        }
    }

    [System.Serializable]
    public enum LoadCombinationMethod
    {
        [XmlEnum("true")]
        [Parseable("true")]
        True,
        [XmlEnum("false")]
        [Parseable("false")]
        False,
        [XmlEnum("EN 1990 6.4.3(6.10.a, b)")]
        [Parseable("EN 1990 6.4.3(6.10.a, b)")]
        EN_1990_643_610_ab,
        [XmlEnum("EN 1990 6.4.3(6.10)")]
        [Parseable("EN 1990 6.4.3(6.10)")]
        EN_1990_643_610,
        [XmlEnum("custom")]
        [Parseable("custom")]
        Custom
    }
}