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
    /// FemDesign "RC design: Shell, utilization" result
    /// </summary>
    [Result(typeof(RCShellUtilization), ListProc.RCDesignShellUtilizationLoadCombination)]
    public class RCShellUtilization : IResult
    {
        /// <summary>
        /// Shell name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Max utilization of bottom/top and x'/y' reinforcement and crack width on top/bottom face.
        /// </summary>
        public double Max { get
            {
                return (new double[] { RBX, RBY, RTX, RTY, CWB, CWT }).Max();
            }
        }
        /// <summary>
        /// Utilization of bottom x'/r reinforcement
        /// </summary>
        public double RBX { get; }
        /// <summary>
        /// Utilization of bottom y'/t reinforcement
        /// </summary>
        public double RBY { get; }
        /// <summary>
        /// Utilization of top x'/r reinforcement
        /// </summary>
        public double RTX { get; }
        /// <summary>
        /// Utilization of top y'/t reinforcement
        /// </summary>
        public double RTY { get; }
        /// <summary>
        /// Utilization for shell buckling
        /// </summary>
        public double BU { get; }
        /// <summary>
        /// Utilization for crack width on the bottom face
        /// </summary>
        public double CWB { get; }
        /// <summary>
        /// Utilization for crack width on the top face
        /// </summary>
        public double CWT { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal RCShellUtilization(string id, double rbx, double rby, double rtx, double rty, double bu, double cwb, double cwt, string resultCase)
        {
            Id = id;
            RBX = rbx;
            RBY = rby;
            RTX = rtx;
            RTY = rty;
            BU = bu;
            CWB = cwb;
            CWT = cwt;
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
                return new Regex(@"^(?'max'Max\. of load combinations, Shell, Utilization)(?: - selected objects)?$|^(?'type'Shell, Utilization), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'max'Max\. of load combinations, Shell, Utilization)(?: - selected objects)?$|^(?'type'Shell, Utilization), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^Shell\tMax\.\t(Combination\t)?RBX\tRBY\tRTX\tRTY\tBU\tCWB\tCWT|^\[.*\]");
            }
        }

        internal static RCShellUtilization Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            if (HeaderData.ContainsKey("casename"))
            {
                string id = row[0];
                double rbx = double.Parse(row[2], CultureInfo.InvariantCulture);
                double rby = double.Parse(row[3], CultureInfo.InvariantCulture);
                double rtx = double.Parse(row[4], CultureInfo.InvariantCulture);
                double rty = double.Parse(row[5], CultureInfo.InvariantCulture);
                double bu = double.Parse(row[6], CultureInfo.InvariantCulture);
                double cwb = double.Parse(row[7], CultureInfo.InvariantCulture);
                double cwt = double.Parse(row[8], CultureInfo.InvariantCulture);
                string lc = HeaderData["casename"];
                return new RCShellUtilization(id, rbx, rby, rtx, rty, bu, cwb, cwt, lc);
            }
            else
            {
                string id = row[0];
                double rbx = double.Parse(row[3], CultureInfo.InvariantCulture);
                double rby = double.Parse(row[4], CultureInfo.InvariantCulture);
                double rtx = double.Parse(row[5], CultureInfo.InvariantCulture);
                double rty = double.Parse(row[6], CultureInfo.InvariantCulture);
                double bu = double.Parse(row[7], CultureInfo.InvariantCulture);
                double cwb = double.Parse(row[8], CultureInfo.InvariantCulture);
                double cwt = double.Parse(row[9], CultureInfo.InvariantCulture);
                string lc = row[2];
                return new RCShellUtilization(id, rbx, rby, rtx, rty, bu, cwb, cwt, lc);
            }

        }
    }
}
