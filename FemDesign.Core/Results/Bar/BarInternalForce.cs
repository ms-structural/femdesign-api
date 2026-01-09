using System;
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
    /// FemDesign "Bars, Internal Force" result
    /// </summary>
    [Result(typeof(BarInternalForce), ListProc.BarsInternalForcesLoadCase, ListProc.BarsInternalForcesLoadCombination)]
    public partial class BarInternalForce : IResult
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
        /// Axial force
        /// </summary>
        public double Fx { get; }

        /// <summary>
        /// Shear force along major axis
        /// </summary>
        public double Fy { get; }

        /// <summary>
        /// Shear force along minor axis
        /// </summary>
        public double Fz { get; }

        /// <summary>
        /// Moment around axis
        /// </summary>
        public double Mx { get; }

        /// <summary>
        /// Moment around major bending axis
        /// </summary>
        public double My { get; }

        /// <summary>
        /// Moment around minor bending axis
        /// </summary>
        public double Mz { get; }

        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal BarInternalForce(string id, double pos, double fx, double fy, double fz, double mx, double my, double mz, string resultCase)
        {
            this.Id = id;
            this.Pos = pos;
            this.Fx = fx;
            this.Fy = fy;
            this.Fz = fz;
            this.Mx = mx;
            this.My = my;
            this.Mz = mz;
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
                return new Regex(@"^(?'type'Bars), (?'result'Internal forces), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Bars), (?'result'Internal forces), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^ID\tx\tN\tTy'\tTz'\tMt\tMy'\tMz'\t(Case|Comb\.)|^\[.+\]");
            }
        }

        internal static BarInternalForce Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string barname = row[0];
            double pos = Double.Parse(row[1], CultureInfo.InvariantCulture);
            double fx = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double fy = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double fz = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double mx = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double my = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double mz = Double.Parse(row[7], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new BarInternalForce(barname, pos, fx, fy, fz, mx, my, mz, lc);
        }

        /// <summary>
        /// The method has been created for returning the value for Grasshopper and Dynamo.
        /// The method can still be use for C# users.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="Result">the result.</param>
        /// <param name="LoadCase">the load case.</param>
        public static Dictionary<string, object> DeconstructBarInternalForce(List<FemDesign.Results.BarInternalForce> Result, string LoadCase)
        {
            var barInternalForces = Result.Cast<FemDesign.Results.BarInternalForce>();

            // Return the unique load case - load combination
            var uniqueLoadCases = barInternalForces.Select(n => n.CaseIdentifier).Distinct().ToList();

            // Select the Nodal Displacement for the selected Load Case - Load Combination
            if (uniqueLoadCases.Contains(LoadCase, StringComparer.OrdinalIgnoreCase))
            {
                barInternalForces = barInternalForces.Where(n => String.Equals(n.CaseIdentifier, LoadCase, StringComparison.OrdinalIgnoreCase));
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

            var forceFx = new List<double>();
            var forceFy = new List<double>();
            var forceFz = new List<double>();
            var forceMx = new List<double>();
            var forceMy = new List<double>();
            var forceMz = new List<double>();

            foreach(var resultBar in barInternalForces)
            {
                loadCases.Add(resultBar.CaseIdentifier);
                elementId.Add(resultBar.Id);
                positionResult.Add(resultBar.Pos);

                forceFx.Add(resultBar.Fx);
                forceFy.Add(resultBar.Fy);
                forceFz.Add(resultBar.Fz);
                forceMx.Add(resultBar.Mx);
                forceMy.Add(resultBar.My);
                forceMz.Add(resultBar.Mz);
            }

            return new Dictionary<string, dynamic>
            {
                {"CaseIdentifier", loadCases},
                {"ElementId", elementId},
                {"PositionResult",positionResult},
                {"Fx", forceFx},
                {"Fy", forceFy},
                {"Fz", forceFz},
                {"Mx", forceMx},
                {"My", forceMy},
                {"Mz", forceMz}
            };
        }
    }
}
