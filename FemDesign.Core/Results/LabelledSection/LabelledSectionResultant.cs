using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;
using FemDesign.Calculate;
using Newtonsoft.Json;
namespace FemDesign.Results
{
    /// <summary>
    /// Represents a Labelled Section Resultant.
    /// </summary>
    [Result(typeof(LabelledSectionResultant), ListProc.LabelledSectionsResultantsLoadCase, ListProc.LabelledSectionsResultantsLoadCombination)]
    public partial class LabelledSectionResultant : IResult
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Identifier
        /// </summary>
        public double BasePointFromSP { get; }
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
        internal LabelledSectionResultant(string id, double basePointFromSP, double fx, double fy, double fz, double mx, double my, double mz, string resultCase)
        {
            this.Id = id;
            this.BasePointFromSP = basePointFromSP;
            this.Fx = fx;
            this.Fy = fy;
            this.Fz = fz;
            this.Mx = mx;
            this.My = my;
            this.Mz = mz;
            this.CaseIdentifier = resultCase;
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
                return new Regex(@"^(?'type'Labelled sections), Resultants, ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Labelled sections), Resultants, ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^ID\tBase point from SP\.\tFx'\tFy'\tFz'\tMx'\tMy'\tMz'\t(Case|Comb\.)|^\t\[.*\]");
            }
        }

        internal static LabelledSectionResultant Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string id = row[0];
            double basePoint = Double.Parse(row[1], CultureInfo.InvariantCulture);
            double fx = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double fy = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double fz = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double mx = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double my = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double mz = Double.Parse(row[7], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new LabelledSectionResultant(id, basePoint, fx, fy, fz, mx, my, mz, lc);
        }
    }
}
