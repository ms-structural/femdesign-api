using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FemDesign.Calculate;

namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "Shells, Stresses" result
    /// </summary>
    [Result(typeof(ShellStress), ListProc.ShellStressesBottomLoadCase, ListProc.ShellStressesBottomLoadCombination, ListProc.ShellStressesMembraneLoadCase, ListProc.ShellStressesMembraneLoadCombination, ListProc.ShellStressesTopLoadCase, ListProc.ShellStressesTopLoadCombination /*ListProc.ShellStressesBottomExtractLoadCase, ListProc.ShellStressesBottomExtractLoadCombination, ListProc.ShellStressesMembraneExtractLoadCase, ListProc.ShellStressesMembraneExtractLoadCombination, ListProc.ShellStressesTopExtractLoadCase, ListProc.ShellStressesTopExtractLoadCombination*/)]
    public partial class ShellStress : IResult
    {
        /// <summary>
        /// Shell name identifier
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Finite element id
        /// </summary>
        public int ElementId { get; }

        /// <summary>
        /// Finite element node id
        /// </summary>
        public int? NodeId { get; }

        /// <summary>
        /// Normal Stress in the local X direction
        /// </summary>
        public double SigmaX { get; }

        /// <summary>
        /// Normal Stress in in the local Y direction
        /// </summary>
        public double SigmaY { get; }

        /// <summary>
        /// Tangential Stress in XY plane
        /// </summary>
        public double TauXY { get; }

        /// <summary>
        /// Tangential Stress in XZ plane
        /// </summary>
        public double TauXZ { get; }

        /// <summary>
        /// Tangential Stress in YZ plane
        /// </summary>
        public double TauYZ { get; }

        /// <summary>
        /// VonMises Stress [N/mm2]
        /// </summary>
        public double SigmaVM { get; }

        /// <summary>
        /// Principal Stress Value - First direction
        /// </summary>
        public double Sigma1 { get; }

        /// <summary>
        /// Principal Stress Value - Second direction
        /// </summary>
        public double Sigma2 { get; }

        /// <summary>
        /// Angle between the local X axis and the direction
        /// of the principle stress Sigma1 [rad]
        /// </summary>
        public double Alpha { get; }

        /// <summary>
        /// Positional Result for Shell Stress
        /// Top, Membrane, Bottom
        /// </summary>
        public string Side { get; }

        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal ShellStress(string id, int elementId, int? nodeId, double sigmaX, double sigmaY, double tauXY, double tauXZ, double tauYZ, double sigmaVM, double sigma1, double sigma2, double alpha, string side, string caseIdentifier)
        {
            this.Id = id;
            this.ElementId = elementId;
            this.NodeId = nodeId;
            this.SigmaX = sigmaX;
            this.SigmaY = sigmaY;
            this.TauXY = tauXY;
            this.TauXZ = tauXZ;
            this.TauYZ = tauYZ;
            this.SigmaVM = sigmaVM;
            this.Sigma1 = sigma1;
            this.Sigma2 = sigma2;
            this.Alpha = alpha;
            this.Side = side;
            this.CaseIdentifier = caseIdentifier;
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
                return new Regex(@"^(?'type'Shells, Stresses), (?'side'top|bottom|membrane) ?(?'extract'\(Extract\))?, ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Shells, Stresses), (?'side'top|bottom|membrane) ?(?'extract'\(Extract\))?, ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^Shell\t(Max\.\t)?Elem\tNode\t\u03c3 x'\t\u03c3 y'\t\u03c4 x'y'\t\u03c4 x'z'\t\u03c4 y'z'\t\u03c3 vm\t\u03c3 1\t\u03c3 2\t\u03b1\t(Case|Comb\.)|^\[.*\]");
            }
        }

        internal static ShellStress Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            if (row.Count() == 14) // Extract
            {
                string id = row[0];
                int elementId = Int32.Parse(row[2], CultureInfo.InvariantCulture);
                // Depending on the output option, some values are not specified as integer but as [-].
                int? nodeId = int.Parse(row[3] == "-" ? "-1" : row[3], CultureInfo.InvariantCulture);
                double sigmaX = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double sigmaY = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double tauXY = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double tauXZ = Double.Parse(row[7], CultureInfo.InvariantCulture);
                double tauYZ = Double.Parse(row[8], CultureInfo.InvariantCulture);
                double sigmaVM = Double.Parse(row[9], CultureInfo.InvariantCulture);
                double sigma1 = Double.Parse(row[10], CultureInfo.InvariantCulture);
                double sigma2 = Double.Parse(row[11], CultureInfo.InvariantCulture);
                double alpha = Double.Parse(row[12], CultureInfo.InvariantCulture);
                string side = HeaderData["side"];
                string caseIdentifier = HeaderData["casename"];
                return new ShellStress(id, elementId, nodeId, sigmaX, sigmaY, tauXY, tauXZ, tauYZ, sigmaVM, sigma1, sigma2, alpha, side, caseIdentifier);
            }
            else
            {
                string id = row[0];
                int elementId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
                // Depending on the output option, some values are not specified as integer but as [-].
                int? nodeId = int.Parse(row[2] == "-" ? "-1" : row[2], CultureInfo.InvariantCulture);
                double sigmaX = Double.Parse(row[3], CultureInfo.InvariantCulture);
                double sigmaY = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double tauXY = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double tauXZ = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double tauYZ = Double.Parse(row[7], CultureInfo.InvariantCulture);
                double sigmaVM = Double.Parse(row[8], CultureInfo.InvariantCulture);
                double sigma1 = Double.Parse(row[9], CultureInfo.InvariantCulture);
                double sigma2 = Double.Parse(row[10], CultureInfo.InvariantCulture);
                double alpha = Double.Parse(row[11], CultureInfo.InvariantCulture);
                string side = HeaderData["side"];
                string caseIdentifier = HeaderData["casename"];
                return new ShellStress(id, elementId, nodeId, sigmaX, sigmaY, tauXY, tauXZ, tauYZ, sigmaVM, sigma1, sigma2, alpha, side, caseIdentifier);
            }
        }


        /// <summary>
        /// The method has been created for returning the value for Grasshopper and Dynamo.
        /// The method can still be use for C# users.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="Result">the result.</param>
        /// <param name="LoadCase">the load case.</param>
        public static Dictionary<string, object> DeconstructShellStress(List<FemDesign.Results.ShellStress> Result, string LoadCase)
        {
            var shellStress = Result.Cast<FemDesign.Results.ShellStress>();

            // Return the unique load case - load combination
            var uniqueLoadCases = shellStress.Select(n => n.CaseIdentifier).Distinct().ToList();

            // Select the Nodal Displacement for the selected Load Case - Load Combination
            if (uniqueLoadCases.Contains(LoadCase, StringComparer.OrdinalIgnoreCase))
            {
                shellStress = shellStress.Where(n => String.Equals(n.CaseIdentifier, LoadCase, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                var warning = $"Load Case '{LoadCase}' does not exist";
                throw new ArgumentException(warning);
            }

            // Parse Results from the object
            var identifier = new List<string>();
            var elementId = new List<int>();
            var nodeId = new List<int?>();

            var sigmaX = new List<double>();
            var sigmaY = new List<double>();
            var tauXY = new List<double>();
            var tauXZ = new List<double>();
            var tauYZ = new List<double>();
            var sigmaVM = new List<double>();
            var sigma1 = new List<double>();
            var sigma2 = new List<double>();
            var alpha = new List<double>();
            var side = new List<string>();


            var uniqueLoadCase = shellStress.Select(n => n.CaseIdentifier).Distinct().ToList();

            foreach (var shellResult in shellStress)
            {
                // FemDesign Return a value in the center of the shell
                // The output is not necessary in this case as the user can compute the value
                // doing an average.
                if (shellResult.NodeId != null)
                {
                    identifier.Add(shellResult.Id);
                    elementId.Add(shellResult.ElementId);
                    nodeId.Add(shellResult.NodeId);

                    sigmaX.Add(shellResult.SigmaX);
                    sigmaY.Add(shellResult.SigmaY);
                    tauXY.Add(shellResult.TauXY);
                    tauXZ.Add(shellResult.TauXZ);
                    tauYZ.Add(shellResult.TauYZ);
                    sigmaVM.Add(shellResult.SigmaVM);
                    sigma1.Add(shellResult.Sigma1);
                    sigma2.Add(shellResult.Sigma2);
                    alpha.Add(shellResult.Alpha);
                    side.Add(shellResult.Side);
                }
            }


            return new Dictionary<string, dynamic>
            {
                {"CaseIdentifier", uniqueLoadCase},
                {"Identifier", identifier},
                {"ElementId", elementId},
                {"NodeId", nodeId},
                {"SigmaX",sigmaX},
                {"SigmaY",sigmaY},
                {"TauXY",tauXY},
                {"TauXZ",tauXZ},
                {"TauYZ",tauYZ},
                {"SigmaVM",sigmaVM},
                {"Sigma1",sigma1},
                {"Sigma2",sigma2},
                {"Alpha",alpha},
                {"Side", side}
            };
        }
    }
}
