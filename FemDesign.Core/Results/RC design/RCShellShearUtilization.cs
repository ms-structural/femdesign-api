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
    [Result(typeof(RCShellShearUtilization), ListProc.RCDesignShellShearUtilizationLoadCombination)]
    public class RCShellShearUtilization : IResult
    {
        /// <summary>
        /// Shell name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Gets or sets the max.
        /// </summary>
        public double Max { get; }
        /// <summary>
        /// Gets or sets the vrd max.
        /// </summary>
        public double VrdMax { get; }
        /// <summary>
        /// Gets or sets the vrd c.
        /// </summary>
        public double VrdC { get; }
        /// <summary>
        /// Gets or sets the vrd s.
        /// </summary>
        public double VrdS { get; }

        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal RCShellShearUtilization(string id, double max, double vrdMax, double vrdC, double vrdS, string resultCase)
        {
            Id = id;
            Max = max;
            VrdMax = vrdMax;
            VrdC = vrdC;
            VrdS = vrdS;
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
                return new Regex(@"^(?'Cmax'Max\. of load combinations, Shell, Shear utilization)(?: - selected objects)?$|^(?'Gmax'Max\. of load groups, Shell, Shear utilization)(?: - selected objects)?$|^(?'type'Shell, Shear utilization), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'Cmax'Max\. of load combinations, Shell, Shear utilization)(?: - selected objects)?$|^(?'Gmax'Max\. of load groups, Shell, Shear utilization)(?: - selected objects)?$|^(?'type'Shell, Shear utilization), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^Shell\tMax\.\t(Combination\t)?vRd,Max\tvRd,c\tvRd,s|^\[.*\]");
            }
        }

        internal static RCShellShearUtilization Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            if (HeaderData.ContainsKey("casename"))
            {
                string id = row[0];
                double max = Double.Parse(row[1], CultureInfo.InvariantCulture);
                double vrdMax = Double.Parse(row[2], CultureInfo.InvariantCulture);
                double vrdC = Double.Parse(row[3], CultureInfo.InvariantCulture);
                double vrdS = Double.Parse(row[4], CultureInfo.InvariantCulture);
                string resultCase = HeaderData["casename"];
                return new RCShellShearUtilization(id, max, vrdMax, vrdC, vrdS, resultCase);
            }
            else
            {
                string id = row[0];
                double max = Double.Parse(row[1], CultureInfo.InvariantCulture);
                string resultCase = row[2];
                double vrdMax = Double.Parse(row[3], CultureInfo.InvariantCulture);
                double vrdC = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double vrdS = Double.Parse(row[5], CultureInfo.InvariantCulture);
                return new RCShellShearUtilization(id, max, vrdMax, vrdC, vrdS, resultCase);
            }
        }
    }
}