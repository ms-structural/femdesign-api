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
    [Result(typeof(RCShellReinforcementRequired), ListProc.RCDesignShellRequiredReinforcementLoadCombination)]
    public class RCShellReinforcementRequired : IResult
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
        /// Gets or sets the x bottom.
        /// </summary>
        public double XBottom { get; }
        /// <summary>
        /// Gets or sets the y bottom.
        /// </summary>
        public double YBottom { get; }
        /// <summary>
        /// Gets or sets the x top.
        /// </summary>
        public double XTop { get; }
        /// <summary>
        /// Gets or sets the y top.
        /// </summary>
        public double YTop { get; }
        /// <summary>
        /// Gets or sets the x mid.
        /// </summary>
        public double? XMid { get; }
        /// <summary>
        /// Gets or sets the y mid.
        /// </summary>
        public double? YMid { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal RCShellReinforcementRequired(string id, int elementId, int nodeId, double xBottom, double yBottom, double xTop, double yTop, double xMid, double yMid, string resultCase)
        {
            Id = id;
            ElementId = elementId;
            NodeId = nodeId;
            XBottom = xBottom;
            YBottom = yBottom;
            XTop = xTop;
            YTop = yTop;
            XMid = xMid;
            YMid = yMid;
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
                return new Regex(@"^(?'max'Max\. of load combinations, Shell, Required reinforcement)(?: - selected objects)?$|^(?'type'Shell, Required reinforcement), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'max'Max\. of load combinations, Shell, Required reinforcement)(?: - selected objects)?$|^(?'type'Shell, Required reinforcement), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^ID\tElem\tNode\tx' or r bottom( Comb)?\ty' or t bottom( Comb)?\tx' or r top( Comb)?\ty' or t top( Comb)?\tx' or r mid( Comb)?\ty' or t mid( Comb)?|^\[.*\]");
            }
        }

        internal static RCShellReinforcementRequired Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            
            {
                string id = row[0];
                int elementId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
                int nodeId = Int32.Parse(row[2] == "-" ? "-1" : row[2], CultureInfo.InvariantCulture);
                double xBottom = Double.Parse(row[3], CultureInfo.InvariantCulture);
                double yBottom = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double xTop = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double yTop = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double xMid = Double.Parse(row[7] == "-" ? "0" : row[7], CultureInfo.InvariantCulture);
                double yMid = Double.Parse(row[8] == "-" ? "0" : row[8], CultureInfo.InvariantCulture);

                string lc = HeaderData["casename"];
                return new RCShellReinforcementRequired(id, elementId, nodeId, xBottom, yBottom, xTop, yTop, xMid, yMid, lc);
            }

        }
    }
}
