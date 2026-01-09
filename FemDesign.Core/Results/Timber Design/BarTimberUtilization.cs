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
    [Result(typeof(BarTimberUtilization), ListProc.TimberBarUtilizationLoadCombination)]
    public partial class BarTimberUtilization : IResult
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Applied profile
        /// </summary>
        public string Section { get; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; }
        /// <summary>
        /// Maximum utilisation from all the verifications
        /// </summary>
        public double Max { get; }
        /// <summary>
        /// Combined bending and axial tension - 6.2.3
        /// </summary>
        public double T { get; }
        /// <summary>
        /// Combined bending and compression tension - 6.1.4, 6.2.4
        /// </summary>
        public double C { get; }

        /// <summary>
        /// Combined shear and torsion - 6.1.7, 6.1.8
        /// </summary>
        public double S { get; }
        /// <summary>
        /// Flexural buckling around axis 1 - 6.3.2
        /// </summary>
        public double FB1 { get; }
        /// <summary>
        /// Flexural buckling around axis 2 - 6.3.2
        /// </summary>
        public double FB2 { get; }
        /// <summary>
        /// Lateral torsional buckling - 6.3.3
        /// </summary>
        public double Ltb { get; }

        /// <summary>
        /// Gets or sets the case identifier.
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal BarTimberUtilization(string id, string section, string status, double max, double t, double c, double s, double fb1, double fb2, double ltb, string caseIdentifier)
        {
            this.Id = id;
            this.Section = section;
            this.Status = status;
            this.Max = max;
            this.T = t;
            this.C = c;
            this.S = s;
            this.FB1 = fb1;
            this.FB2 = fb2;
            this.Ltb = ltb;
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
                return new Regex(@"^(?'type'Timber bar), (?'result'Utilization), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Timber bar), (?'result'Utilization), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^Member\tSection\tStatus\tMaximum\tT\tC\tS\tFB1\tFB2\tLTB|^\[.+\]");
            }
        }

        internal static BarTimberUtilization Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string id = row[0];
            string section = row[1];
            string status = row[2];
            double max = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double t = row[4] == "-" ? 0 : Double.Parse(row[4], CultureInfo.InvariantCulture);
            double c = row[5] == "-" ? 0 : Double.Parse(row[5], CultureInfo.InvariantCulture);
            double s = row[6] == "-" ? 0 : Double.Parse(row[6], CultureInfo.InvariantCulture);
            double fb1 = row[7] == "-" ? 0 : Double.Parse(row[7], CultureInfo.InvariantCulture);
            double fb2 = row[8] == "-" ? 0 : Double.Parse(row[8], CultureInfo.InvariantCulture);
            double ltb = row[9] == "-" ? 0 : Double.Parse(row[9], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new BarTimberUtilization(id, section, status, max, t, c, s, fb1, fb2, ltb, lc);
        }
    }
}