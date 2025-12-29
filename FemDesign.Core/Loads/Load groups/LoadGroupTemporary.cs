// https://strusoft.com/

using System.Xml.Serialization;
using System.Collections.Generic;
using FemDesign.GenericClasses;
using StruSoft.Interop.StruXml.Data;

namespace FemDesign.Loads
{
    /// <summary>
    /// temporary_load_group (child of general_load_group)
    /// </summary>
    [System.Serializable]
    public partial class LoadGroupTemporary : LoadGroupBase
    {
        [XmlAttribute("safety_factor")]
        public double _safetyFactor;
        [XmlIgnore]
        public double SafetyFactor
        {
            get
            {
                return this._safetyFactor;
            }
            set
            {
                this._safetyFactor = RestrictedDouble.Positive(value);
            }
        }
        [XmlAttribute("psi_0")]
        public double _psi0;
        [XmlIgnore]
        public double Psi0
        {
            get
            {
                return this._psi0;
            }
            set
            {
                this._psi0 = RestrictedDouble.NonNegMax_10(value);
            }
        }
        [XmlAttribute("psi_1")]
        public double _psi1;
        [XmlIgnore]
        public double Psi1
        {
            get
            {
                return this._psi1;
            }
            set
            {
                this._psi1 = RestrictedDouble.NonNegMax_10(value);
            }
        }
        [XmlAttribute("psi_2")]
        public double _psi2;
        [XmlIgnore]
        public double Psi2
        {
            get
            {
                return this._psi2;
            }
            set
            {
                this._psi2 = RestrictedDouble.NonNegMax_10(value);
            }
        }
        [XmlAttribute("leading_cases")]
        public bool LeadingCases { get; set; }
        [XmlAttribute("ignore_sls")]
        public bool IgnoreSls { get; set; } = false;

        [XmlElement(ElementName = "custom_table", Order = 1)]
        public TemporaryGroupRecord CustomTable { get; set; }

        [XmlElement("load_case", Order = 2)]
        public new List<ModelLoadCaseInGroup> ModelLoadCase { get; set; }

        [XmlAttribute("temporary_effect")]
        public TemporaryEffect TemporaryEffect { get; set; } = TemporaryEffect.General;

        /// <summary>
        /// ONLY FOR DESERIALIZATION
        /// NEEDS TO BE FIXED
        /// </summary>
        [XmlElement("load_case", Order = 3)]
        public List<Reference_type> Load_case { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("load_cases_of_moving_load", Order = 7)]
        public System.Collections.Generic.List<Temporary_load_groupLoad_cases_of_moving_load> Load_cases_of_moving_load { get; set; }

        /// parameterless constructor for serialization///
        public LoadGroupTemporary() { }

        /// <summary>
        /// Public constructor.
        /// </summary>
        public LoadGroupTemporary(double safetyFactor,
                                       double psi0, double psi1, double psi2,
                                       bool potentiallyLeadingAction, List<LoadCase> loadCases,
                                       ELoadGroupRelationship relationsship, string name)
        {
            this.Name = name;
            this.SafetyFactor = safetyFactor;
            this.Psi0 = psi0;
            this.Psi1 = psi1;
            this.Psi2 = psi2;
            this.LeadingCases = potentiallyLeadingAction;
            this.Relationship = relationsship;
            for (int i = 0; i < loadCases.Count; i++)
                AddLoadCase(loadCases[i]);
        }

        /// <summary>
        /// Add LoadCase to group.
        /// </summary>
        public void AddLoadCase(LoadCase loadCase)
        {
            if (LoadCaseInLoadGroup(loadCase))
            {
                // pass
            }
            else
            {
                if (ModelLoadCase == null)
                    ModelLoadCase = new List<ModelLoadCaseInGroup>();

                ModelLoadCase.Add(new ModelLoadCaseInGroup(loadCase.Guid, this));
                LoadCase.Add(loadCase);
            }
        }
    }

    [System.Serializable]
    public class TemporaryGroupRecord
    {
        [XmlElement("record")]
        public List<StruSoft.Interop.StruXml.Data.Temporary_load_groupRecord> Record { get; set; }

        public TemporaryGroupRecord()
        {
        }
    }

    public enum TemporaryEffect
    {
        [Parseable("General", "general", "0")]
        [XmlEnum("general")]
        General,
        [Parseable("Snow", "snow", "1")]
        [XmlEnum("snow")]
        Snow,
        [Parseable("Wind", "wind", "2")]
        [XmlEnum("wind")]
        Wind,
    }
}