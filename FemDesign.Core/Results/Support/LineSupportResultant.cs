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
    /// FemDesign "Line support group, Resultants" result
    /// </summary>
    [Result(typeof(LineSupportResultant), ListProc.LineSupportResultantsLoadCase, ListProc.LineSupportResultantsLoadCombination)]
    public partial class LineSupportResultant : IResult
    {
        /// <summary>
        /// Support name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// l/2 length of support
        /// </summary>
        public double HalfLength { get; }
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
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal LineSupportResultant(string id, double halfLength, double fx, double fy, double fz, double mx, double my, double mz, string resultCase)
        {
            Id = id;
            HalfLength = halfLength;
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
                return new Regex(@"^(?'type'Line support group), (?'result'Resultants), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Line support group), (?'result'Resultants), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^ID\tl/2\tFx'\tFy'\tFz'\tMx'\tMy'\tMz'\t(Case|Comb\.)|^\t\[.+\]");
            }
        }

        internal static LineSupportResultant Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string supportname = row[0];
            double halfLength = Double.Parse(row[1], CultureInfo.InvariantCulture);
            double fx = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double fy = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double fz = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double mx = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double my = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double mz = Double.Parse(row[7], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new LineSupportResultant(supportname, halfLength, fx, fy, fz, mx, my, mz, lc);
        }
    }
}
