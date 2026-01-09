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
    /// FemDesign "Bars, End forces" result
    /// </summary>
    [Result(typeof(ShellInternalForce), ListProc.ShellInternalForceLoadCase, ListProc.ShellInternalForceLoadCombination)]
    public partial class ShellInternalForce : IResult
    {
        /// <summary>
        /// Bar name identifier
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Finite element element id
        /// </summary>
        public int ElementId { get; }

        /// <summary>
        /// Finite element node id
        /// </summary>
        public int? NodeId { get; }

        /// <summary>
        /// Mx'
        /// </summary>
        public double Mx { get; }

        /// <summary>
        /// My'
        /// </summary>
        public double My { get; }

        /// <summary>
        /// Mx'y'
        /// </summary>
        public double Mxy { get; }

        /// <summary>
        /// Nx'
        /// </summary>
        public double Nx { get; }

        /// <summary>
        /// Ny'
        /// </summary>
        public double Ny { get; }

        /// <summary>
        /// Nx'y'
        /// </summary>
        public double Nxy { get; }

        /// <summary>
        /// Tx'z'
        /// </summary>
        public double Txz { get; }

        /// <summary>
        /// Ty'z'
        /// </summary>
        public double Tyz { get; }

        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal ShellInternalForce(string id, int elementId, int? nodeId, double mx, double my, double mxy, double nx, double ny, double nxy, double txz, double tyz, string resultCase)
        {
            Id = id;
            ElementId = elementId;
            NodeId = nodeId;
            Mx = mx;
            My = my;
            Mxy = mxy;
            Nx = nx;
            Ny = ny;
            Nxy = nxy;
            Txz = txz;
            Tyz = tyz;
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
                return new Regex(@"^(?'type'Shells), (?'result'Internal forces) ?(?'extract'\(Extract\))?, ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Shells), (?'result'Internal forces) ?(?'extract'\(Extract\))?, ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^ID\t(Max\.\t)?Elem\tNode\tMx'\tMy'\tMx'y'\tNx'\tNy'\tNx'y'\tTx'z'\tTy'z'\t(Case|Comb\.)|^\[.*\]");
            }
        }

        internal static ShellInternalForce Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            if (row.Length == 13) // Extract
            {
                string shellname = row[0];
                int elementId = int.Parse(row[2], CultureInfo.InvariantCulture);
                int? nodeId = int.Parse(row[3] == "-" ? "-1" : row[3], CultureInfo.InvariantCulture);
                double mx = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double my = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double mxy = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double nx = Double.Parse(row[7], CultureInfo.InvariantCulture);
                double ny = Double.Parse(row[8], CultureInfo.InvariantCulture);
                double nxy = Double.Parse(row[9], CultureInfo.InvariantCulture);
                double txz = Double.Parse(row[10], CultureInfo.InvariantCulture);
                double tyz = Double.Parse(row[11], CultureInfo.InvariantCulture);
                string lc = HeaderData["casename"];
                return new ShellInternalForce(shellname, elementId, nodeId, mx, my, mxy, nx, ny, nxy, txz, tyz, lc);
            }
            else
            {
                string shellname = row[0];
                int elementId = int.Parse(row[1], CultureInfo.InvariantCulture);
                int? nodeId = int.Parse(row[2] == "-" ? "-1" : row[2], CultureInfo.InvariantCulture);
                double mx = Double.Parse(row[3], CultureInfo.InvariantCulture);
                double my = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double mxy = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double nx = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double ny = Double.Parse(row[7], CultureInfo.InvariantCulture);
                double nxy = Double.Parse(row[8], CultureInfo.InvariantCulture);
                double txz = Double.Parse(row[9], CultureInfo.InvariantCulture);
                double tyz = Double.Parse(row[10], CultureInfo.InvariantCulture);
                string lc = HeaderData["casename"];
                return new ShellInternalForce(shellname, elementId, nodeId, mx, my, mxy, nx, ny, nxy, txz, tyz, lc);
            }
        }


        /// <summary>
        /// The method has been created for returning the value for Grasshopper and Dynamo.
        /// The method can still be use for C# users.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="Result">the result.</param>
        /// <param name="LoadCase">the load case.</param>
        public static Dictionary<string, object> DeconstructShellInternalForce(List<FemDesign.Results.ShellInternalForce> Result, string LoadCase)
        {
            var shellForces = Result.Cast<FemDesign.Results.ShellInternalForce>();

            // Return the unique load case - load combination
            var uniqueLoadCases = shellForces.Select(n => n.CaseIdentifier).Distinct().ToList();

            // Select the Nodal Displacement for the selected Load Case - Load Combination
            if (uniqueLoadCases.Contains(LoadCase, StringComparer.OrdinalIgnoreCase))
            {
                shellForces = shellForces.Where(n => String.Equals(n.CaseIdentifier, LoadCase, StringComparison.OrdinalIgnoreCase));
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

            var mx = new List<double>();
            var my = new List<double>();
            var mxy = new List<double>();
            var nx = new List<double>();
            var ny = new List<double>();
            var nxy = new List<double>();
            var txz = new List<double>();
            var tyz = new List<double>();


            var uniqueLoadCase = shellForces.Select(n => n.CaseIdentifier).Distinct().ToList();

            foreach (var shellResult in shellForces)
            {
                // FemDesign Return a value in the center of the shell
                // The output is not necessary in this case as the user can compute the value
                // doing an average.
                if (shellResult.NodeId != null)
                {
                    identifier.Add(shellResult.Id);
                    elementId.Add(shellResult.ElementId);
                    nodeId.Add(shellResult.NodeId);

                    mx.Add(shellResult.Mx);
                    my.Add(shellResult.My);
                    mxy.Add(shellResult.Mxy);
                    nx.Add(shellResult.Nx);
                    ny.Add(shellResult.Ny);
                    nxy.Add(shellResult.Nxy);
                    txz.Add(shellResult.Txz);
                    tyz.Add(shellResult.Tyz);
                }
            }


            return new Dictionary<string, dynamic>
            {
                {"CaseIdentifier", uniqueLoadCase},
                {"Identifier", identifier},
                {"ElementId", elementId},
                {"NodeId", nodeId},
                {"Mx",mx},
                {"My",my},
                {"Mxy",mxy},
                {"Nx",nx},
                {"Ny",ny},
                {"Nxy",nxy},
                {"Txz",txz},
                {"Tyz",tyz},
            };
        }
    }
}