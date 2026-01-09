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
    [Result(typeof(RCShellShearCapacity), ListProc.RCDesignShellShearCapacityLoadCombination)]
    public class RCShellShearCapacity : IResult
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
        /// Gets or sets the ved.
        /// </summary>
        public double Ved { get; }
        /// <summary>
        /// Gets or sets the ved red.
        /// </summary>
        public double VedRed { get; }
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
        /// Finite element node id
        /// </summary>
        public double AsMissing { get; }
        /// <summary>
        /// vEd,Red	vRd,Max	vRd,c	vRd,s	as,missing
        /// </summary>
        public string Max { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal RCShellShearCapacity(string id, int element, int nodeId, double ved, double vedred, double vrdmax, double vrdc, double vrds, double asMissing, string resultCase)
        {
            Id = id;
            ElementId = element;
            NodeId = nodeId;
            Ved = ved;
            VedRed = vedred;
            VrdMax = vrdmax;
            VrdC = vrdc;
            VrdS = vrds;
            AsMissing = asMissing;
            CaseIdentifier = resultCase;
        }

        [JsonConstructor]
        internal RCShellShearCapacity(string id, int element, int nodeId, string max, double ved, double vedred, double vrdmax, double vrdc, double vrds, double asMissing, string resultCase)
        {
            Id = id;
            ElementId = element;
            NodeId = nodeId;
            Max = max;
            Ved = ved;
            VedRed = vedred;
            VrdMax = vrdmax;
            VrdC = vrdc;
            VrdS = vrds;
            AsMissing = asMissing;
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
                return new Regex(@"^(?'Cmax'Max\. of load combinations, Shell, Shear capacity)(?: - selected objects)?$|^(?'Gmax'Max\. of load groups, Shell, Shear capacity)(?: - selected objects)?$|^(?'type'Shell, Shear capacity), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'Cmax'Max\. of load combinations, Shell, Shear capacity)(?: - selected objects)?$|^(?'Gmax'Max\. of load groups, Shell, Shear capacity)(?: - selected objects)?$|^(?'type'Shell, Shear capacity), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^ID\tElem\tNode\t(Max\t)?vEd\tvEd,Red\tvRd,Max\tvRd,c\tvRd,s\tas,missing(\tComb)?|^\[.*\]");
            }
        }

        internal static RCShellShearCapacity Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            if (HeaderData.ContainsKey("casename"))
            {
                string id = row[0];
                int elementId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
                int nodeId = Int32.Parse(row[2], CultureInfo.InvariantCulture);
                double ved = Double.Parse(row[3], CultureInfo.InvariantCulture);
                double vedred = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double vrdmax = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double vrdc = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double vrds = Double.Parse(row[7], CultureInfo.InvariantCulture);
                double asMissing = Double.Parse(row[8], CultureInfo.InvariantCulture);
                string resultCase = HeaderData["casename"];
                return new RCShellShearCapacity(id, elementId, nodeId, ved, vedred, vrdmax, vrdc, vrds, asMissing, resultCase);
            }
            else // it is max of load groups or max of load combinations
            {
                string id = row[0];
                int elementId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
                int nodeId = Int32.Parse(row[2], CultureInfo.InvariantCulture);
                string max = row[3];
                double ved = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double vedred = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double vrdmax = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double vrdc = Double.Parse(row[7], CultureInfo.InvariantCulture);
                double vrds = Double.Parse(row[8], CultureInfo.InvariantCulture);
                double asMissing = Double.Parse(row[9], CultureInfo.InvariantCulture);
                string resultCase = row[10];
                return new RCShellShearCapacity(id, elementId, nodeId, max, ved, vedred, vrdmax, vrdc, vrds, asMissing, resultCase);
            }
        }
    }
}