using System;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using FemDesign.Calculate;
using Newtonsoft.Json;
namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "Bars, Stresses" result
    /// </summary>
    [Result(typeof(BarStress), ListProc.BarsStressesLoadCase, ListProc.BarsStressesLoadCombination)]
    public partial class BarStress : IResult
    {
        /// <summary>
        /// Bar name identifier
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Position Result
        /// The value is the relative distance from the
        /// starting point of the bar
        /// </summary>
        public double Pos { get; }

        /// <summary>
        /// Maximum Normal Tension
        /// </summary>
        public double SigmaXiMax { get; }

        /// <summary>
        /// Minimum Normal Tension
        /// </summary>
        public double SigmaXiMin { get; }

        /// <summary>
        /// Von Mises Stress
        /// </summary>
        public double SigmaVM { get; }

        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal BarStress(string id, double pos, double sigmaXiMax, double sigmaXiMin, double sigmaVM, string resultCase)
        {
            this.Id = id;
            this.Pos = pos;
            this.SigmaXiMax = sigmaXiMax;
            this.SigmaXiMin = sigmaXiMin;
            this.SigmaVM = sigmaVM;
            this.CaseIdentifier = resultCase;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Bars), (?'result'Stresses), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Bars), (?'result'Stresses), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^ID\tx\t\u03c3 x'\(max\)\t\u03c3 x'\(min\)\t\u03c3 vm\t(Case|Comb\.)|^\[.+\]");
            }
        }

        internal static BarStress Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string barname = row[0];
            double pos = Double.Parse(row[1], CultureInfo.InvariantCulture);
            double sigmaXiMax = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double sigmaXiMin = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double sigmaVM = Double.Parse(row[4], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new BarStress(barname, pos, sigmaXiMax, sigmaXiMin, sigmaVM, lc);
        }

        /// <summary>
        /// The method has been created for returning the value for Grasshopper and Dynamo.
        /// The method can still be use for C# users.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="Result">the result.</param>
        /// <param name="LoadCase">the load case.</param>
        public static Dictionary<string, object> DeconstructBarStress(List<FemDesign.Results.BarStress> Result, string LoadCase)
        {
            var barStress = Result.Cast<FemDesign.Results.BarStress>();

            // Return the unique load case - load combination
            var uniqueLoadCases = barStress.Select(n => n.CaseIdentifier).Distinct().ToList();

            // Select the Nodal Displacement for the selected Load Case - Load Combination
            if (uniqueLoadCases.Contains(LoadCase, StringComparer.OrdinalIgnoreCase))
            {
                barStress = barStress.Where(n => String.Equals(n.CaseIdentifier, LoadCase, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                var warning = $"Load Case '{LoadCase}' does not exist";
                throw new ArgumentException(warning);
            }

            // Parse Results from the object
            var loadCases = new List<string>();
            var elementId = new List<string>();
            var positionResult = new List<double>();

            var sigmaXiMax = new List<double>();
            var sigmaXiMin = new List<double>();
            var sigmaVM = new List<double>();


            foreach (var resultBar in barStress)
            {
                loadCases.Add(resultBar.CaseIdentifier);
                elementId.Add(resultBar.Id);
                positionResult.Add(resultBar.Pos);

                sigmaXiMax.Add(resultBar.SigmaXiMax);
                sigmaXiMin.Add(resultBar.SigmaXiMin);
                sigmaVM.Add(resultBar.SigmaVM);
            }

            return new Dictionary<string, dynamic>
            {
                {"CaseIdentifier", loadCases},
                {"ElementId", elementId},
                {"PositionResult",positionResult},
                {"SigmaXiMax", sigmaXiMax},
                {"SigmaXiMin", sigmaXiMin},
                {"SigmaVM", sigmaVM},
            };
        }
    }
}