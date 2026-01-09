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
    [Result(typeof(SurfaceSupportResultant), ListProc.SurfaceSupportResultantsLoadCase, ListProc.SurfaceSupportResultantsLoadCombination)]
    public partial class SurfaceSupportResultant : IResult
    {
        /// <summary>
        /// Surface Support name identifier
        /// </summary>
        public string Name { get; }
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
        public double Mx { get; }
        /// <summary>
        /// Local My'
        /// </summary>
        public double My { get; }
        /// <summary>
        /// Local Mz'
        /// </summary>
        public double Mz { get; }
        /// <summary>
        /// Moment resultant
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal SurfaceSupportResultant(string id, double fx, double fy, double fz, double mx, double my, double mz, string resultCase)
        {
            Name = id;
            Fx = fx;
            Fy = fy;
            Fz = fz;
            Mx = mx;
            My = my;
            Mz = mz;
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
                return new Regex(@"^(?'type'Surface support), (?'result'Resultants),( (?'loadcasetype'[\w\s\-]+) -)? Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Surface support), (?'result'Resultants),( (?'loadcasetype'[\w\s\-]+) -)? Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^ID\tFx'\tFy'\tFz'\tMx'\tMy'\tMz'\t(Case|Comb\.)|^\t\[.*\]");
            }
        }

        internal static SurfaceSupportResultant Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string surfaceName = row[0];
            double fx = Double.Parse(row[1], CultureInfo.InvariantCulture);
            double fy = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double fz = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double mx = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double my = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double mz = Double.Parse(row[6], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new SurfaceSupportResultant(surfaceName, fx, fy, fz, mx, my, mz, lc);
        }
    }
}
