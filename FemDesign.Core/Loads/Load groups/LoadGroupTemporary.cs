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
        /// <summary>
        /// Gets or sets the safety factor.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the psi0.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the psi1.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the psi2.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the leading cases.
        /// </summary>
        [XmlAttribute("leading_cases")]
        public bool LeadingCases { get; set; }
        /// <summary>
        /// Gets or sets the ignore sls.
        /// </summary>
        [XmlAttribute("ignore_sls")]
        public bool IgnoreSls { get; set; } = false;

        /// <summary>
        /// Gets or sets the custom table.
        /// </summary>
        [XmlElement(ElementName = "custom_table", Order = 1)]
        public TemporaryGroupRecord CustomTable { get; set; }

        /// <summary>
        /// Gets or sets the model load case.
        /// </summary>
        [XmlElement("load_case", Order = 2)]
        public List<ModelLoadCaseInGroup> ModelLoadCase { get; set; }

        /// <summary>
        /// Gets or sets the temporary effect.
        /// </summary>
        [XmlAttribute("temporary_effect")]
        public TemporaryEffect TemporaryEffect { get; set; } = TemporaryEffect.General;

        /// <summary>
        /// ONLY FOR DESERIALIZATION
        /// NEEDS TO BE FIXED
        /// </summary>
        [XmlElement("load_case", Order = 2)]
        public List<Reference_type> Load_case { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("load_cases_of_moving_load", Order = 7)]
        public System.Collections.Generic.List<Temporary_load_groupLoad_cases_of_moving_load> Load_cases_of_moving_load { get; set; }

        /// parameterless constructor for serialization///
        public LoadGroupTemporary() { }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="safetyFactor">the safety factor.</param>
        /// <param name="psi0">the psi0.</param>
        /// <param name="psi1">the psi1.</param>
        /// <param name="psi2">the psi2.</param>
        /// <param name="potentiallyLeadingAction">the potentially leading action.</param>
        /// <param name="loadCases">the load cases.</param>
        /// <param name="relationsship">the relationsship.</param>
        /// <param name="name">the name.</param>
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
        /// <param name="loadCase">the load case.</param>
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

    /// <summary>
    /// Represents a Temporary Group Record.
    /// </summary>
    [System.Serializable]
    public class TemporaryGroupRecord
    {
        /// <summary>
        /// Gets or sets the record.
        /// </summary>
        [XmlElement("record")]
        public List<StruSoft.Interop.StruXml.Data.Temporary_load_groupRecord> Record { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryGroupRecord"/> class.
        /// </summary>
        public TemporaryGroupRecord()
        {
        }
    }

    /// <summary>
    /// Defines the Temporary Effect enumeration.
    /// </summary>
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