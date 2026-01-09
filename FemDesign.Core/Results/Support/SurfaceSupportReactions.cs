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
    /// FemDesign "Surface Support, Reactions" result
    /// </summary>
    [Result(typeof(SurfaceSupportReaction), ListProc.SurfaceSupportReactionsLoadCase, ListProc.SurfaceSupportReactionsLoadCombination)]
    public partial class SurfaceSupportReaction : IResult
    {
        /// <summary>
        /// Surface Support name identifier
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Finite element element id
        /// </summary>
        public int ElementId { get; }
        /// <summary>
        /// Finite element node id
        /// </summary>
        public int? NodeId { get; }
        /// <summary>
        /// Local Fx'
        /// </summary>
        public double Fx { get; }
        /// <summary>
        /// Local Fy'
        /// </summary>
        public double Fy { get; }
        /// <summary>
        /// Local Fz'
        /// </summary>
        public double Fz { get; }
        /// <summary>
        /// Local Mx'
        /// </summary>
        public double Fr { get; }
        /// <summary>
        /// Moment resultant
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal SurfaceSupportReaction(string id, int elementId, int? nodeId, double fx, double fy, double fz, double fr, string resultCase)
        {
            Name = id;
            ElementId = elementId;
            NodeId = nodeId;
            Fx = fx;
            Fy = fy;
            Fz = fz;
            Fr = fr;
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
                return new Regex(@"^(?'type'Surface support), (?'result'Reactions),( (?'loadcasetype'[\w\s\-]+) -)? Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Surface support), (?'result'Reactions),( (?'loadcasetype'[\w\s\-]+) -)? Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^No\.\tElem\tNode\tFx'\tFy'\tFz'\tFr\t(Case|Comb\.)|^\[.*\]");
            }
        }

        internal static SurfaceSupportReaction Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string surfaceName = row[0];
            int elementId = int.Parse(row[1], CultureInfo.InvariantCulture);
            int? nodeId = int.Parse(row[2] == "-" ? "-1" : row[2], CultureInfo.InvariantCulture);
            double fx = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double fy = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double fz = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double fr = Double.Parse(row[6], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new SurfaceSupportReaction(surfaceName, elementId, nodeId, fx, fy, fz, fr, lc);
        }
    }
}
