using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using FemDesign.GenericClasses;

namespace FemDesign
{
    /// <summary>
    /// Defines the PTC Load Case enumeration.
    /// </summary>
    public enum PTCLoadCase
    {
        /// <summary>
        /// PTC T0
        /// </summary>
        T0,
        /// <summary>
        /// PTC T8
        /// </summary>
        T8,
    }

    /// <summary>
    /// Represents a Activated Load Case.
    /// </summary>
    [System.Serializable]
    public partial class ActivatedLoadCase
    {

        /// <summary>
        /// Gets or sets the case.
        /// </summary>
        [XmlAttribute("case")]
        public string _case;

        /// <summary>
        /// Gets a value indicating whether load case.
        /// </summary>
        public bool IsLoadCase => Guid.TryParse(_case, out Guid _);
        /// <summary>
        /// Gets or sets the load case guid.
        /// </summary>
        public Guid LoadCaseGuid => IsLoadCase ? new Guid(_case) : throw new Exception($"Case \"{_case}\" is not a guid of a load case.");

        private static readonly Regex IndexedGuidPattern = new Regex("(?<guid>[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12})#(?<index>[1-9][0-9]{0,4})");
        /// <summary>
        /// Gets a value indicating whether moving load.
        /// </summary>
        public bool IsMovingLoad => IndexedGuidPattern.IsMatch(_case);
        /// <summary>
        /// Gets or sets the moving load index.
        /// </summary>
        public int MovingLoadIndex => IsMovingLoad
            ? int.Parse(IndexedGuidPattern.Match(_case).Groups["index"].Value)
            : throw new Exception($"Case \"{_case}\" is not an indexed guid of a moving load case.");

        private static readonly Regex PTCLoadCasePattern = new Regex("ptc_t(0|8)");
        /// <summary>
        /// Gets a value indicating whether ptc load case.
        /// </summary>
        public bool IsPTCLoadCase => PTCLoadCasePattern.IsMatch(_case);
        /// <summary>
        /// Gets or sets the ptc load case.
        /// </summary>
        public PTCLoadCase PTCLoadCase => IsPTCLoadCase ? (_case.EndsWith("0") ? PTCLoadCase.T0 : PTCLoadCase.T8) : throw new Exception($"Case \"{_case}\" is not a PTC load case.");

        /// <summary>
        /// Gets or sets the load case display name.
        /// </summary>
        [XmlIgnore]
        public string LoadCaseDisplayName { get; internal set; }

        /// <summary>
        /// Gets or sets the factor.
        /// </summary>
        [XmlAttribute("factor")]
        public double _factor { get; set; }

        [XmlIgnore]
        public double Factor
        {
            get { return this._factor; }
            set { this._factor = RestrictedDouble.NonNegMax_1e20(value); }
        }

        /// <summary>
        /// Gets or sets the s factors.
        /// </summary>
        [XmlElement("s_factors")]
        public SFactorType SFactors { get; set; }


        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute("partitioning")]
        public ActivationType Type { get; set; }

        /// <summary>
        /// Private construction for serialization
        /// </summary>
        private ActivatedLoadCase()
        {

        }

        /// <summary>
        /// Create a (construction stage) activated load case.
        /// </summary>
        /// <param name="loadCase">the load case.</param>
        /// <param name="factor">Load case factor.</param>
        /// <param name="type">ActivationType.</param>
        /// <param name="sfactors">SFactorType.</param>
        public ActivatedLoadCase(Loads.LoadCase loadCase, double factor, ActivationType type, SFactorType sfactors = null)
        {
            Initialize(factor, type, sfactors);
            this.LoadCaseDisplayName = loadCase.Name;
            this._case = loadCase.Guid.ToString();
        }

        /// <summary>
        /// Create a (construction stage) activated load case of a PTC load case.
        /// </summary>
        /// <param name="sfactors">the sfactors.</param>
        /// <param name="loadCase">the load case.</param>
        /// <param name="factor">Load case factor.</param>
        /// <param name="type">Activation type.</param>
        public ActivatedLoadCase(PTCLoadCase loadCase, double factor, ActivationType type, SFactorType sfactors = null)
        {
            Initialize(factor, type, sfactors);
            this.LoadCaseDisplayName =
                loadCase == PTCLoadCase.T0 ? "PTC T0" :
                loadCase == PTCLoadCase.T8 ? "PTC T8" :
                throw new Exception("Not a valid PTCLoadCase value");
            this._case =
                loadCase == PTCLoadCase.T0 ? "ptc_t0" :
                loadCase == PTCLoadCase.T8 ? "ptc_t8" :
                throw new Exception("Not a valid PTCLoadCase value");
        }

        /// <summary>
        /// Create a (construction stage) activated load case of a Moving load case.
        /// </summary>
        /// <param name="sfactors">the sfactors.</param>
        /// <param name="movingLoad">the moving load.</param>
        /// <param name="index">the index.</param>
        /// <param name="factor">Load case factor.</param>
        /// <param name="type">Activation type.</param>
        public ActivatedLoadCase(StruSoft.Interop.StruXml.Data.Moving_load_type movingLoad, int index, double factor, ActivationType type, SFactorType sfactors = null)
        {
            Initialize(factor, type, sfactors);
            this.LoadCaseDisplayName = movingLoad.Name + "-" + index; // E.g "MVL-42"
            this._case = movingLoad.Guid.ToString() + "#" + index;
        }

        private void Initialize(double factor, ActivationType type, SFactorType sfactors = null)
        {
            this.Factor = factor;
            this.Type = type;
            this.SFactors = sfactors;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"ActivatedLoadCase {this.LoadCaseDisplayName} {this.Factor} {this.SFactors.Sc} {this.SFactors.Sf} {this.SFactors.Sq} {this.Type}";
        }
    }

    /// <summary>
    /// Defines the Activation Type enumeration.
    /// </summary>
    public enum ActivationType
    {
        /// <summary>
        /// Only in this stage
        /// </summary>
        [Parseable("only_in_this_stage", "0", "OnlyInThisStage")]
        [XmlEnum("only_in_this_stage")]
        OnlyInThisStage,
        /// <summary>
        /// From this stage on
        /// </summary>
        [Parseable("from_this_stage_on", "1", "FromThisStageOn")]
        [XmlEnum("from_this_stage_on")]
        FromThisStageOn,
        /// <summary>
        /// Shifted from first stage
        /// </summary>
        [Parseable("shifted_from_first_stage", "2", "ShiftedFromFirstStage")]
        [XmlEnum("shifted_from_first_stage")]
        ShiftedFromFirstStage,
        /// <summary>
        /// Only stage activated elements
        /// </summary>
        [Parseable("only_stage_activated_elem", "3", "OnlyStageActivatedElements")]
        [XmlEnum("only_stage_activated_elem")]
        OnlyStageActivatedElements
    }
}
