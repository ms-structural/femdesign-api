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
    /// FemDesign "RC design: Shell, Design Forces" result
    /// </summary>
    [Result(typeof(RCShellDesignForces), ListProc.RCDesignShellDesignForcesLoadCombination)]
    public class RCShellDesignForces : IResult
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
        /// mx or mr bottom
        /// </summary>
        public double MxBottom { get; }
        /// <summary>
        /// my or mt bottom
        /// </summary>
        public double MyBottom { get; }
        /// <summary>
        /// mx or mr top
        /// </summary>
        public double MxTop { get; }
        /// <summary>
        /// my or mt top
        /// </summary>
        public double MyTop { get; }
        /// <summary>
        /// nx or nr max
        /// </summary>
        public double NxMax { get; }
        /// <summary>
        /// ny or nt max
        /// </summary>
        public double NyMax { get; }
        /// <summary>
        /// nx or nr min
        /// </summary>
        public double NxMin { get; }
        /// <summary>
        /// ny or nt min
        /// </summary>
        public double NyMin { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal RCShellDesignForces(string id, int elementId, int nodeId, double mxBottom, double myBottom, double mxTop, double myTop, double nxMax, double nyMax, double nxMin, double nyMin, string resultCase)
        {
            Id = id;
            ElementId = elementId;
            NodeId = nodeId;
            MxBottom = mxBottom;
            MyBottom = myBottom;
            MxTop = mxTop;
            MyTop = myTop;
            NxMax = nxMax;
            NyMax = nyMax;
            NxMin = nxMin;
            NyMin = nyMin;
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
                return new Regex(@"^(?'max'Max\. of load combinations, Shell, Design forces)(?: - selected objects)?$|^(?'type'Shell, Design forces), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'max'Max\. of load combinations, Shell, Design forces)(?: - selected objects)?$|^(?'type'Shell, Design forces), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^ID\tElem\tNode\tmx or mr bottom\tmy or mt bottom\tmx or mr top\tmy or mt top\tnx or nr max\.\tny or nt max\.\tnx or nr min\.\tny or nt min\.|^\[.*\]");
            }
        }

        internal static RCShellDesignForces Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            //if (HeaderData.ContainsKey("max"))
            //{
            //    string id = row[0];
            //    double rbx = double.Parse(row[3], CultureInfo.InvariantCulture);
            //    double rby = double.Parse(row[4], CultureInfo.InvariantCulture);
            //    double rtx = double.Parse(row[5], CultureInfo.InvariantCulture);
            //    double rty = double.Parse(row[6], CultureInfo.InvariantCulture);
            //    double bu = double.Parse(row[7], CultureInfo.InvariantCulture);
            //    bool sc = row[8] == "OK";
            //    double cwb = double.Parse(row[9], CultureInfo.InvariantCulture);
            //    double cwt = double.Parse(row[10], CultureInfo.InvariantCulture);
            //    string lc = row[2];
            //    return new RCShellUtilization(id, rbx, rby, rtx, rty, bu, sc, cwb, cwt, lc);
            //}

            {
                string id = row[0];
                int elementId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
                int nodeId = Int32.Parse(row[2] == "-" ? "-1" : row[2], CultureInfo.InvariantCulture);
                double mxBottom = Double.Parse(row[3], CultureInfo.InvariantCulture);
                double myBottom = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double mxTop = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double myTop = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double nxMax = Double.Parse(row[7], CultureInfo.InvariantCulture);
                double nyMax = Double.Parse(row[8], CultureInfo.InvariantCulture);
                double nxMin = Double.Parse(row[9] == "-" ? "0" : row[9], CultureInfo.InvariantCulture);
                double nyMin = Double.Parse(row[10] == "-" ? "0" : row[10], CultureInfo.InvariantCulture);

                string lc = HeaderData["casename"];
                return new RCShellDesignForces(id, elementId, nodeId, mxBottom, myBottom, mxTop, myTop, nxMax, nyMax, nxMin, nyMin, lc);
            }

        }
    }
}
