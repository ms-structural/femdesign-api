using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using FemDesign.GenericClasses;

using FemDesign.Calculate;
using Newtonsoft.Json;
namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "Shell, Derived forces" result
    /// </summary>
    [Result(typeof(ShellDerivedForce), ListProc.ShellDerivedForceLoadCase, ListProc.ShellDerivedForceLoadCombination)]
    public partial class ShellDerivedForce : IResult
    {
        /// <summary>
        /// Shell name identifier
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
        /// M1 [kNm/m]
        /// </summary>
        public double M1 { get; }
        /// <summary>
        /// M2 [kNm/m]
        /// </summary>
        public double M2 { get; }
        /// <summary>
        /// Alpha (M) [rad]
        /// </summary>
        public double AlphaM { get; }
        /// <summary>
        /// N1 [kN/m]
        /// </summary>
        public double N1 { get; }
        /// <summary>
        /// N2 [kN/m]
        /// </summary>
        public double N2 { get; }
        /// <summary>
        /// Alpha (N) [rad]
        /// </summary>
        public double AlphaN { get; }

        /// <summary>
        /// Gets or sets the case identifier.
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal ShellDerivedForce(string id, int elementId, int? nodeId, double m1, double m2, double alphaM, double n1, double n2, double alphaN, string resultCase)
        {
            this.Id = id;
            this.ElementId = elementId;
            this.NodeId = nodeId;
            this.M1 = m1;
            this.M2 = m2;
            this.AlphaM = alphaM;
            this.N1 = n1;
            this.N2 = n2;
            this.AlphaN = alphaN;
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
                return new Regex(@"^(?'type'Shells), (?'result'Derived internal forces) ?(?'extract'\(Extract\))?, ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Shells), (?'result'Derived internal forces) ?(?'extract'\(Extract\))?, ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^ID\t(Max\.\t)?Elem\tNode\tM1\tM2\t\u03b1 \(M\)\tN1\tN2\t\u03b1 \(N\)\t(Case|Comb\.)|^\[.*\]");
            }
        }

        internal static ShellDerivedForce Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            if (row.Count() == 11) // Extract
            {
                string shellname = row[0];
                int elementId = int.Parse(row[2], CultureInfo.InvariantCulture);
                int? nodeId = int.Parse(row[3] == "-" ? "-1" : row[3], CultureInfo.InvariantCulture);
                double m1 = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double m2 = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double alphaM = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double n1 = Double.Parse(row[7], CultureInfo.InvariantCulture);
                double n2 = Double.Parse(row[8], CultureInfo.InvariantCulture);
                double alphaN = Double.Parse(row[9], CultureInfo.InvariantCulture);
                string lc = row[10];
                string test = HeaderData["casename"];
                return new ShellDerivedForce(shellname, elementId, nodeId, m1, m2, alphaM, n1, n2, alphaN, lc);
            }
            else
            {
                string shellname = row[0];
                int elementId = int.Parse(row[1], CultureInfo.InvariantCulture);
                int? nodeId = int.Parse(row[2] == "-" ? "-1" : row[2], CultureInfo.InvariantCulture);
                double m1 = Double.Parse(row[3], CultureInfo.InvariantCulture);
                double m2 = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double alphaM = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double n1 = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double n2 = Double.Parse(row[7], CultureInfo.InvariantCulture);
                double alphaN = Double.Parse(row[8], CultureInfo.InvariantCulture);
                string lc = row[9];
                string test = HeaderData["casename"];
                return new ShellDerivedForce(shellname, elementId, nodeId, m1, m2, alphaM, n1, n2, alphaN, lc);
            }
        }
    }
}