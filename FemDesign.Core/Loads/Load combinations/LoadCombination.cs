// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Linq;
using System.IO;
using System;
using System.Text.RegularExpressions;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Load Combination.
    /// </summary>
    [System.Serializable]
    [XmlRoot("load_combination", Namespace = "urn:strusoft")]
    public partial class LoadCombination : EntityBase
    {
        private static Regex _combinationNamePattern = new Regex( @"^[ -#%'-;=?A-\uFFFD]{1,159}$" );

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public string _name { get; set; } // name159

        // attributes
        [XmlIgnore]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if(!_combinationNamePattern.IsMatch(value))
                    throw new ArgumentException("'Name' is not valid!");
                this._name = value;
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute("type")]
        public LoadCombType Type { get; set; } // loadcombtype

        // CASES: Xml elements sequence in order
        //
        // load_case (sequence)
        // seismic_max
        // seismic_res_fx_plus_mx
        // seismic_res_fx_minus_mx
        // seismic_res_fy_plus_my
        // seismic_res_fy_minus_my
        // seismic_res_fz
        // ptc_t0
        // ptc_t8
        // ldcase_pile
        // cs_case

        /// <summary>
        /// Gets or sets the model load case.
        /// </summary>
        [XmlElement("load_case")]
        public List<ModelLoadCase> ModelLoadCase { get; set; } = new List<ModelLoadCase>();

        // Special load cases of the combination, from FD18
        /// <summary>
        /// Gets or sets the seismic max.
        /// </summary>
        [XmlElement("seismic_max")]
        public LoadCombinationCaseBase SeismicMax { get; set; }

        /// <summary>
        /// Gets or sets the seismic res fx plus mx.
        /// </summary>
        [XmlElement("seismic_res_fx_plus_mx")]
        public LoadCombinationCaseBase SeismicResFxPlusMx { get; set; }

        /// <summary>
        /// Gets or sets the seismic res fx minus mx.
        /// </summary>
        [XmlElement("seismic_res_fx_minus_mx")]
        public LoadCombinationCaseBase SeismicResFxMinusMx { get; set; }

        /// <summary>
        /// Gets or sets the seismic res fy plus my.
        /// </summary>
        [XmlElement("seismic_res_fy_plus_my")]
        public LoadCombinationCaseBase SeismicResFyPlusMy { get; set; }

        /// <summary>
        /// Gets or sets the seismic res fy minus my.
        /// </summary>
        [XmlElement("seismic_res_fy_minus_my")]
        public LoadCombinationCaseBase SeismicResFyMinusMy { get; set; }

        /// <summary>
        /// Gets or sets the seismic res fz.
        /// </summary>
        [XmlElement("seismic_res_fz")]
        public LoadCombinationCaseBase SeismicResFz { get; set; }

        // Special load cases of the combination, from FD19
        /// <summary>
        /// Gets or sets the ptc t0.
        /// </summary>
        [XmlElement("ptc_t0")]
        public LoadCombinationCaseBase PtcT0 { get; set; }

        /// <summary>
        /// Gets or sets the ptc t8.
        /// </summary>
        [XmlElement("ptc_t8")]
        public LoadCombinationCaseBase PtcT8 { get; set; }

        // Special load cases of the combination, from FD20
        /// <summary>
        /// Gets or sets the pile load case.
        /// </summary>
        [XmlElement("ldcase_pile")]
        public LoadCombinationCaseBase PileLoadCase { get; set; }

        // Special load cases of the combination, from FD21.0003
        /// <summary>
        /// Gets or sets the stage load case.
        /// </summary>
        [XmlElement("cs_case")]
        public StageLoadCase StageLoadCase { get; set; }

        /// <summary>
        /// Gets or sets the comb item.
        /// </summary>
        [XmlIgnore]
        public Calculate.CombItem CombItem { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LoadCombination()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadCombination"/> class.
        /// </summary>
        /// <param name="name">the name.</param>
        /// <param name="type">the type.</param>
        /// <param name="loadCases">the load cases.</param>
        /// <param name="gammas">the gammas.</param>
        /// <param name="combItem">the comb item.</param>
        public LoadCombination(string name, LoadCombType type, List<LoadCase> loadCases, List<double> gammas, Calculate.CombItem combItem = null)
        {
            Initialize(name, type, combItem);

            if (loadCases.Count == gammas.Count)
                for (int i = 0; i < loadCases.Count; i++)
                    this.AddLoadCase(loadCases[i], gammas[i]);
            else
                throw new System.ArgumentException("loadCase and gamma must have equal length");

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadCombination"/> class.
        /// </summary>
        /// <param name="name">the name.</param>
        /// <param name="type">the type.</param>
        /// <param name="values">the values.</param>
        public LoadCombination(string name, LoadCombType type, params (LoadCase lc, double gamma)[] values)
        {
            Initialize(name, type);

            foreach (var (lc, gamma) in values)
                this.AddLoadCase(lc, gamma);
        }

        private void Initialize(string name, LoadCombType type, Calculate.CombItem combItem = null)
        {
            this.EntityCreated();
            this.Name = name;
            this.Type = type;
            this.SetCalculationSettings(combItem);
        }

        /// <summary>
        /// Set the load combination-specific calculation settings. This is known as "Setup by load combinations" in FEM-Design GUI.
        /// </summary>
        /// <param name="combItem">Load combination-specific settings. The default settings will be used if the value is null.</param>
        public void SetCalculationSettings(Calculate.CombItem combItem)
        {
            this.CombItem = combItem;
        }

        /// <summary>
        /// Get LoadCase guids of LoadCases in LoadCombination.
        /// </summary>
        /// <returns>The result.</returns>
        public List<string> GetLoadCaseGuidsAsString()
        {
            var loadCaseGuids = new List<string>();
            foreach (ModelLoadCase item in this.ModelLoadCase)
            {
                loadCaseGuids.Add(item.Guid.ToString());
            }
            return loadCaseGuids;
        }

        /// <summary>
        /// Get gamma values of LoadCases in LoadCombination.
        /// </summary>
        /// <returns>The result.</returns>
        public List<double> GetGammas()
        {
            var gammas = new List<double>();
            foreach (ModelLoadCase item in this.ModelLoadCase)
            {
                gammas.Add(item.Gamma);
            }
            return gammas;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>List of pairs of CaseId and gamma values. CaseId may be LoadCase guid, construction stage index description or special case name.</returns>
        public List<(string CaseId, double Gamma)> GetCaseDescriptionAndGammas()
        {
            var pairs = new List<(string CaseId, double Gamma)>();
            for (int i = 0; i < ModelLoadCase.Count; i++)
            {
                pairs.Add((ModelLoadCase[i].Guid.ToString(), ModelLoadCase[i].Gamma));
            }

            if (SeismicMax != null)
                pairs.Add(("Seismic max.", SeismicMax.Gamma));
            if (SeismicResFxMinusMx != null)
                pairs.Add(("Seismic fx-mx", SeismicResFxMinusMx.Gamma));
            if (SeismicResFxPlusMx != null)
                pairs.Add(("Seismic fx+mx", SeismicResFxPlusMx.Gamma));
            if (SeismicResFyMinusMy != null)
                pairs.Add(("Seismic fy-my", SeismicResFyMinusMy.Gamma));
            if (SeismicResFyPlusMy != null)
                pairs.Add(("Seismic fy+my", SeismicResFyPlusMy.Gamma));
            if (SeismicResFz != null)
                pairs.Add(("Seismic fz", SeismicResFz.Gamma));

            if (PtcT0 != null)
                pairs.Add(("Ptc T0", PtcT0.Gamma));
            if (PtcT8 != null)
                pairs.Add(("Ptc T8", PtcT8.Gamma));

            if (PileLoadCase != null)
                pairs.Add(("pile loadcase", PileLoadCase.Gamma));

            if (StageLoadCase != null)
                pairs.Add((StageLoadCase._stageType, StageLoadCase.Gamma));

            return pairs;
        }



        public List<(LoadCase Case, double Gamma, string CaseType)> GetLoadCasesAndGammas()
        {
            var pairs = new List<(LoadCase Case, double Gamma, string CaseType)>();
            for (int i = 0; i < ModelLoadCase.Count; i++)
            {
                pairs.Add((
                    ModelLoadCase[i].LoadCase,
                    ModelLoadCase[i].Gamma,
                    ModelLoadCase[i].IsMovingLoadLoadCase ? "Moving load case" : "Load case"
                    ));
            }
            return pairs;
        }
        public List<(string Case, double Gamma)> GetSpecialCasesAndGammas()
        {
            var pairs = new List<(string Case, double Gamma)>();

            if (SeismicMax != null)
                pairs.Add(("Seismic max.", SeismicMax.Gamma));
            if (SeismicResFxMinusMx != null)
                pairs.Add(("Seis res, Fx-Mx", SeismicResFxMinusMx.Gamma));
            if (SeismicResFxPlusMx != null)
                pairs.Add(("Seis res, Fx+Mx", SeismicResFxPlusMx.Gamma));
            if (SeismicResFyMinusMy != null)
                pairs.Add(("Seis res, Fy-My", SeismicResFyMinusMy.Gamma));
            if (SeismicResFyPlusMy != null)
                pairs.Add(("Seis res, Fy+My", SeismicResFyPlusMy.Gamma));
            if (SeismicResFz != null)
                pairs.Add(("Seis res, Fz", SeismicResFz.Gamma));

            if (PtcT0 != null)
                pairs.Add(("PTC T0", PtcT0.Gamma));
            if (PtcT8 != null)
                pairs.Add(("PTC T8", PtcT8.Gamma));

            if (PileLoadCase != null)
                pairs.Add(("Neg. Shaft friction", PileLoadCase.Gamma));

            return pairs;
        }

        public List<(object Case, double Gamma, string CaseType)> GetCaseAndGammas()
        {
            var pairs = new List<(object, double, string)>();
            pairs.AddRange(
                GetLoadCasesAndGammas()
                .Select((t) => { return ((object)t.Case, t.Gamma, t.CaseType); })
                .ToList());
            pairs.AddRange(
                GetSpecialCasesAndGammas()
                .Select((t) => { return ((object)t.Case, t.Gamma, "Special load case"); })
                .ToList());

            if (StageLoadCase is null) return pairs;

            if (StageLoadCase.IsFinalStage)
                pairs.Add(("Final construction stage", StageLoadCase.Gamma, "Special load case"));
            else
                pairs.Add((StageLoadCase.Stage, StageLoadCase.Gamma, "Stage"));

            return pairs;
        }

        /// <summary>
        /// Add LoadCase to LoadCombination.
        /// </summary>
        /// <param name="loadCase">the load case.</param>
        /// <param name="gamma">the gamma.</param>
        public void AddLoadCase(LoadCase loadCase, double gamma)
        {
            if (this.LoadCaseInLoadCombination(loadCase))
                return;

            this.ModelLoadCase.Add(new ModelLoadCase(loadCase, gamma));
        }

        /// <summary>
        /// Sets the stage load case.
        /// </summary>
        /// <param name="stage">the stage.</param>
        /// <param name="gamma">the gamma.</param>
        public void SetStageLoadCase(Stage stage, double gamma)
        {
            this.StageLoadCase = new StageLoadCase(stage, gamma);
        }

        /// <summary>
        /// Sets the final stage load case.
        /// </summary>
        /// <param name="gamma">the gamma.</param>
        public void SetFinalStageLoadCase(double gamma)
        {
            this.StageLoadCase = StageLoadCase.FinalStage(gamma);
        }

        /// <summary>
        /// Removes stage load case.
        /// </summary>
        public void RemoveStageLoadCase()
        {
            this.StageLoadCase = null;
        }

        /// <summary>
        /// Check if LoadCase in LoadCombination.
        /// </summary>
        private bool LoadCaseInLoadCombination(LoadCase loadCase)
        {
            return this.ModelLoadCase.Any(elem => elem.Guid == loadCase.Guid);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            const int space = -10;
            const int caseNameSpace = -12;
            const int gammaSpace = -3;
            var repr = "";
            repr += $"{this.Name,space} {this.Type}\n";

            var casesAndGammas = GetCaseAndGammas();

            foreach (var (@case, gamma, type) in casesAndGammas)
            {
                if (@case is null) { return base.ToString(); } // Deserialisation can not get the loadcase name from the object.

                if (@case.GetType() == typeof(LoadCase))
                {
                    var lc = (LoadCase)@case;
                    repr += $"{"",space - 1}{gamma,gammaSpace} {lc.Name,caseNameSpace}\n";
                }
                else
                    repr += $"{"",space - 1}{gamma,gammaSpace} {@case,caseNameSpace}\n";
            }

            return repr;
        }
    }
}
