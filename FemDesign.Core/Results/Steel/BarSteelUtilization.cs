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
    [Result(typeof(BarSteelUtilization), ListProc.SteelDesignBarUtilizationLoadCombination)]
    public partial class BarSteelUtilization : IResult
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
        /// Resistance of cross-section - Part 1-1: 6.2.1
        /// </summary>
        public double RCS { get; }
        /// <summary>
        /// Flexural buckling - Part 1-1: 6.3.1
        /// </summary>
        public double FB { get; }
        /// <summary>
        /// Torsional flexural buckling - Part 1-1: 6.3.1
        /// </summary>
        public double TFB { get; }
        /// <summary>
        /// Lateral torsional buckling, top flange - Part 1-1: 6.3.2.4
        /// </summary>
        public double LTBt { get; }
        /// <summary>
        /// Lateral torsional buckling, bottom flange - Part 1-1: 6.3.2.4
        /// </summary>
        public double LTBb { get; }
        /// <summary>
        /// Shear buckling - Part 1-5: 5
        /// </summary>
        public double SB { get; }
        /// <summary>
        /// Interaction - Part 1-1: 6.3.3
        /// </summary>
        public double IA { get; }
        /// <summary>
        /// Gets or sets the case identifier.
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal BarSteelUtilization(string id, string section, string status, double max, double rcs, double fb, double tfb, double ltbt, double ltbb, double sb, double ia, string caseIdentifier)
        {
            this.Id = id;
            this.Section = section;
            this.Status = status;
            this.Max = max;
            this.RCS = rcs;
            this.FB = fb;
            this.TFB = tfb;
            this.LTBt = ltbt;
            this.LTBb = ltbb;
            this.SB = sb;
            this.IA = ia;
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
                return new Regex(@"^(?'Cmax'Max\. of load combinations, Steel bar, Utilization)(?: - selected objects)?$|^(?'Gmax'Max\. of load groups, Steel bar, Utilization)(?: - selected objects)?$|^(?'type'Steel bar), (?'result'Utilization), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'Cmax'Max\. of load combinations, Steel bar, Utilization)(?: - selected objects)?$|^(?'Gmax'Max\. of load groups, Steel bar, Utilization)(?: - selected objects)?$|^(?'type'Steel bar), (?'result'Utilization), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^Member\tSection\tStatus\tMaximum\t(Combination\t)?RCS\tFB\tTFB\tLTB,t\tLTB,b\tSB\tIA|^\[.+\]");
            }
        }

        internal static BarSteelUtilization Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            if(HeaderData.ContainsKey("casename"))
            {
                string id = row[0];
                string section = row[1];
                string status = row[2];
                double max = Double.Parse(row[3], CultureInfo.InvariantCulture);
                double rcs = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double fb = row[5] == "-" ? 0 : Double.Parse(row[5], CultureInfo.InvariantCulture);
                double tfb = row[6] == "-" ? 0 : Double.Parse(row[6], CultureInfo.InvariantCulture);
                double ltbt = row[7] == "-" ? 0 : Double.Parse(row[7], CultureInfo.InvariantCulture);
                double ltbb = row[8] == "-" ? 0 : Double.Parse(row[8], CultureInfo.InvariantCulture);
                double sb = row[9] == "-" ? 0 : Double.Parse(row[9], CultureInfo.InvariantCulture);
                double ia = row[10] == "-" ? 0 : Double.Parse(row[10], CultureInfo.InvariantCulture);
                string lc = HeaderData["casename"];
                return new BarSteelUtilization(id, section, status, max, rcs, fb, tfb, ltbt, ltbb, sb, ia, lc);
            }
            else
            {
                string id = row[0];
                string section = row[1];
                string status = row[2];
                double max = Double.Parse(row[3], CultureInfo.InvariantCulture);
                double rcs = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double fb = row[6] == "-" ? 0 : Double.Parse(row[5], CultureInfo.InvariantCulture);
                double tfb = row[7] == "-" ? 0 : Double.Parse(row[6], CultureInfo.InvariantCulture);
                double ltbt = row[8] == "-" ? 0 : Double.Parse(row[7], CultureInfo.InvariantCulture);
                double ltbb = row[9] == "-" ? 0 : Double.Parse(row[8], CultureInfo.InvariantCulture);
                double sb = row[10] == "-" ? 0 : Double.Parse(row[9], CultureInfo.InvariantCulture);
                double ia = row[11] == "-" ? 0 : Double.Parse(row[10], CultureInfo.InvariantCulture);
                string lc = row[4];
                return new BarSteelUtilization(id, section, status, max, rcs, fb, tfb, ltbt, ltbb, sb, ia, lc);
            }
        }
    }
}