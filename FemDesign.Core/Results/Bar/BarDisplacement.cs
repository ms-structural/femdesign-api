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
    /// FemDesign "Bars, Displacements" result
    /// </summary>
    [Result(typeof(BarDisplacement), ListProc.BarsDisplacementsLoadCase, ListProc.BarsDisplacementsLoadCombination)]
    public partial class BarDisplacement : IResult
    {
        /// <summary>
        /// Bar name identifier
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Position Result
        /// </summary>
        public double Pos { get; }

        /// <summary>
        /// Displacement in Global or Local x
        /// </summary>
        public double Ex { get; }

        /// <summary>
        /// Displacement in Global or Local y
        /// </summary>
        public double Ey { get; }

        /// <summary>
        /// Displacement in Global or Local z
        /// </summary>
        public double Ez { get; }

        /// <summary>
        /// Rotation around Global or Local x
        /// </summary>
        public double Fix { get; }

        /// <summary>
        /// Rotation around Global or Local y
        /// </summary>
        public double Fiy { get; }

        /// <summary>
        /// Rotation around Global or Local z
        /// </summary>
        public double Fiz { get; }

        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal BarDisplacement(string id, double pos, double ex, double ey, double ez, double fix, double fiy, double fiz, string resultCase)
        {
            this.Id = id;
            this.Pos = pos;
            this.Ex = ex;
            this.Ey = ey;
            this.Ez = ez;
            this.Fix = fix;
            this.Fiy = fiy;
            this.Fiz = fiz;
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
                return new Regex(@"^(?'type'Bars), (?'result'Displacements), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Bars), (?'result'Displacements), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^Bar\tx\tex'\tey'\tez'\t\u03c6 x'\t\u03c6 y'\t\u03c6 z'\t(Case|Comb\.)|^\[.+\]");
            }
        }

        internal static BarDisplacement Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string barname = row[0];
            double pos = Double.Parse(row[1], CultureInfo.InvariantCulture);
            double ex = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double ey = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double ez = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double fix = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double fiy = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double fiz = Double.Parse(row[7], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new BarDisplacement(barname, pos, ex, ey, ez, fix, fiy, fiz, lc);
        }


        /// <summary>
        /// The method has been created for returning the value for Grasshopper and Dynamo.
        /// The method can still be use for C# users.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="Result">the result.</param>
        /// <param name="LoadCase">the load case.</param>
        public static Dictionary<string, object> DeconstructBarDisplacements(List<FemDesign.Results.BarDisplacement> Result, string LoadCase)
        {
            var barDisplacements = Result.Cast<FemDesign.Results.BarDisplacement>();

            // Return the unique load case - load combination
            var uniqueLoadCases = barDisplacements.Select(n => n.CaseIdentifier).Distinct().ToList();

            // Select the Nodal Displacement for the selected Load Case - Load Combination
            if (uniqueLoadCases.Contains(LoadCase, StringComparer.OrdinalIgnoreCase))
            {
                barDisplacements = barDisplacements.Where(n => String.Equals(n.CaseIdentifier, LoadCase, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                var warning = $"Load Case '{LoadCase}' does not exist";
                throw new ArgumentException(warning);
            }

            // Parse Results from the object
            var elementId = barDisplacements.Select(n => n.Id).ToList();
            var loadCases = barDisplacements.Select(n => n.CaseIdentifier).Distinct().ToList();
            var localPosition = barDisplacements.Select(n => n.Pos).ToList();

            // Create a Rhino Vector for Displacement and Rotation
            var translation = new List<FemDesign.Geometry.Vector3d>();
            var rotation = new List<FemDesign.Geometry.Vector3d>();

            foreach (var nodeDisp in barDisplacements)
            {
                var transVector = new FemDesign.Geometry.Vector3d(nodeDisp.Ex, nodeDisp.Ey, nodeDisp.Ez);
                translation.Add(transVector);

                var rotVector = new FemDesign.Geometry.Vector3d(nodeDisp.Fix, nodeDisp.Fiy, nodeDisp.Fiz);
                rotation.Add(rotVector);
            }

            var CaseIdentifier = loadCases;
            var ElementId = elementId;
            var PositionResult = localPosition;
            var Translation = translation;
            var Rotation = rotation;

            return new Dictionary<string, dynamic>
            {
                {"CaseIdentifier", CaseIdentifier},
                {"ElementId", ElementId},
                {"PositionResult", PositionResult},
                {"Translation", Translation},
                {"Rotation", Rotation},
            };
        }
    }
}