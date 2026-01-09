using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using FemDesign.GenericClasses;
using Newtonsoft.Json;
using FemDesign.Calculate;

namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "Shells, displacements" result
    /// </summary>
    [Result(typeof(ShellDisplacement), ListProc.ShellDisplacementLoadCase, ListProc.ShellDisplacementLoadCombination)]
    public partial class ShellDisplacement : IResult
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
        /// Displacement in global x
        /// </summary>
        public double Ex { get; }
        /// <summary>
        /// Displacement in global y
        /// </summary>
        public double Ey { get; }
        /// <summary>
        /// Displacement in global z
        /// </summary>
        public double Ez { get; }
        /// <summary>
        /// Rotation around global x
        /// </summary>
        public double Fix { get; }
        /// <summary>
        /// Rotation around global y
        /// </summary>
        public double Fiy { get; }
        /// <summary>
        /// Rotation around global z
        /// </summary>
        public double Fiz { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal ShellDisplacement(string id, int elementId, int? nodeId, double ex, double ey, double ez, double fix, double fiy, double fiz, string resultCase)
        {
            Id = id;
            ElementId = elementId;
            NodeId = nodeId;
            Ex = ex;
            Ey = ey;
            Ez = ez;
            Fix = fix;
            Fiy = fiy;
            Fiz = fiz;
            CaseIdentifier = resultCase;
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
                return new Regex(@"^(?'type'Shells, Displacements( \(Extract\))?), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Shells, Displacements( \(Extract\))?), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^Shell\t(Max\.\t)?Elem\tNode\tex'\tey'\tez'\t\u03c6 x'\t\u03c6 y'\t\u03c6 z'\t(e\tfi\t)?(Case|Comb\.)|^\[.*\]");
            }
        }

        internal static ShellDisplacement Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            if (row.Count() == 13) // Extract
            {
                string name = row[0];
                int elementId = int.Parse(row[2], CultureInfo.InvariantCulture);
                // Depending on the output option, some values are not specified as
                // integer but as [-].
                int? nodeId = int.Parse(row[3] == "-" ? "-1" : row[3], CultureInfo.InvariantCulture);
                double ex = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double ey = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double ez = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double fix = Double.Parse(row[7], CultureInfo.InvariantCulture);
                double fiy = Double.Parse(row[8], CultureInfo.InvariantCulture);
                double fiz = Double.Parse(row[9], CultureInfo.InvariantCulture);
                string lc = HeaderData["casename"];
                return new ShellDisplacement(name, elementId, nodeId, ex, ey, ez, fix, fiy, fiz, lc);
            }
            else
            {
                string name = row[0];
                int elementId = int.Parse(row[1], CultureInfo.InvariantCulture);
                // Depending on the output option, some values are not specified as
                // integer but as [-].
                int? nodeId = int.Parse(row[2] == "-" ? "-1" : row[2], CultureInfo.InvariantCulture);
                double ex = Double.Parse(row[3], CultureInfo.InvariantCulture);
                double ey = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double ez = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double fix = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double fiy = Double.Parse(row[7], CultureInfo.InvariantCulture);
                double fiz = Double.Parse(row[8], CultureInfo.InvariantCulture);
                string lc = HeaderData["casename"];
                return new ShellDisplacement(name, elementId, nodeId, ex, ey, ez, fix, fiy, fiz, lc);
            }
        }

        /// <summary>
        /// The method has been created for returning the value for Grasshopper and Dynamo.
        /// The method can still be use for C# users.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="Result">the result.</param>
        /// <param name="LoadCase">the load case.</param>
        public static Dictionary<string, object> DeconstructShellDisplacement(List<FemDesign.Results.ShellDisplacement> Result, string LoadCase)
        {
            var shellDisplacement = Result.Cast<FemDesign.Results.ShellDisplacement>();

            // Return the unique load case - load combination
            var uniqueLoadCases = shellDisplacement.Select(n => n.CaseIdentifier).Distinct().ToList();

            // Select the Nodal Displacement for the selected Load Case - Load Combination
            if (uniqueLoadCases.Contains(LoadCase, StringComparer.OrdinalIgnoreCase))
            {
                shellDisplacement = shellDisplacement.Where(n => String.Equals(n.CaseIdentifier, LoadCase, StringComparison.OrdinalIgnoreCase));
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

            // Create an FD Vector for Displacement and Rotation
            var translation = new List<FemDesign.Geometry.Vector3d>();
            var rotation = new List<FemDesign.Geometry.Vector3d>();


            var uniqueLoadCase = shellDisplacement.Select(n => n.CaseIdentifier).Distinct().ToList();

            foreach (var shellResult in shellDisplacement)
            {
                // FemDesign Return also a value in the center of the shell
                // The output is not necessary in this case as the user can compute the value
                // doing an average.
                if (shellResult.NodeId != null)
                {
                    identifier.Add(shellResult.Id);
                    elementId.Add(shellResult.ElementId);
                    nodeId.Add(shellResult.NodeId);
                    translation.Add(new Geometry.Vector3d(shellResult.Ex, shellResult.Ey, shellResult.Ez));
                    rotation.Add(new Geometry.Vector3d(shellResult.Fix, shellResult.Fiy, shellResult.Fiz));
                }
            }

            return new Dictionary<string, dynamic>
            {
                {"CaseIdentifier", uniqueLoadCase},
                {"ElementId", elementId},
                {"NodeId",nodeId},
                {"Translation", translation},
                {"Rotation", rotation}
            };
        }
    }
}
