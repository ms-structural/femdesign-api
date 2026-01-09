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
    /// FemDesign "Bars, End forces" result
    /// </summary>
    [Result(typeof(BarEndForce), ListProc.BarsEndForcesLoadCombination)]
    public partial class BarEndForce : IResult
    {
        /// <summary>
        /// Bar name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Start or end node of bar. End is true.
        /// </summary>
        public bool End { get; }
        /// <summary>
        /// Axial force
        /// </summary>
        public double Fx { get; }
        /// <summary>
        /// Shear force along major axis
        /// </summary>
        public double Fy { get; }
        /// <summary>
        /// Shear force along minor axis
        /// </summary>
        public double Fz { get; }
        /// <summary>
        /// Moment around axis
        /// </summary>
        public double Mx { get; }
        /// <summary>
        /// Moment around major bending axis
        /// </summary>
        public double My { get; }
        /// <summary>
        /// Moment around minor bending axis
        /// </summary>
        public double Mz { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal BarEndForce(string id, bool end, double fx, double fy, double fz, double mx, double my, double mz, string resultCase)
        {
            Id = id;
            End = end;
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
                return new Regex(@"^(?'type'Bars), (?'result'End forces), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[\w\ ]+?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Bars), (?'result'End forces), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[\w\ ]+?)(?: - selected objects)?$|^ID\tEnd\tN\tTy'\tTz'\tMt\tMy'\tMz'\t(Case|Comb\.)|^\[.*\]");
            }
        }

        internal static BarEndForce Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string barname = row[0];
            bool end = row[1] == "End";
            double fx = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double fy = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double fz = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double mx = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double my = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double mz = Double.Parse(row[7], CultureInfo.InvariantCulture);
            string lc = row[8];
            return new BarEndForce(barname, end, fx, fy, fz, mx, my, mz, lc);
        }
    }
}
