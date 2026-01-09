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
    [Result(typeof(RCShellCrackWidth), ListProc.RCDesignShellCrackWidthLoadCombination)]
    public class RCShellCrackWidth : IResult
    {
        /// <summary>
        /// Shell name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Finite element id
        /// </summary>
        public int ElementId { get; }
        /// <summary>
        /// Face of the shell
        /// </summary>
        public Face Face { get; }
        /// <summary>
        /// Creck width in primary direction
        /// </summary>
        public double Width1 { get; }
        /// <summary>
        /// Primary direction angle
        /// </summary>
        public double Direction1 { get; }
        /// <summary>
        /// Creck width in secondary direction
        /// </summary>
        public double Width2 { get; }
        /// <summary>
        /// Secondary direction angle
        /// </summary>
        public double Direction2 { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal RCShellCrackWidth(string id, int elementId, Face face, double w1, double d1, double w2, double d2, string resultCase)
        {
            Id = id;
            ElementId = elementId;
            Face = face;
            Width1 = w1;
            Direction1 = d1;
            Width2 = w2;
            Direction2 = d2;
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
                return new Regex(@"^(?'max'Max\. of load combinations, Shell, Crack width)(?: - selected objects)?$|^(?'type'Shell, Crack width), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'max'Max\. of load combinations, Shell, Crack width)(?: - selected objects)?$|^(?'type'Shell, Crack width), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^ID\tElem\tFace\tWidth 1\tDirection 1\tWidth 2\tDirection 2(\tComb)?|^\[.*\]");
            }
        }

        internal static RCShellCrackWidth Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            // TODO: Read line by line instead of saving to "HeaderData"
            // This is due to a bug where femdesign don't fill in all table values
            // This is a workaround

            if (string.IsNullOrEmpty(row[0]))
                row[0] = HeaderData["id"];
            else
                HeaderData["id"] = row[0];

            if (string.IsNullOrEmpty(row[1]))
                row[1] = HeaderData["elementId"];
            else
                HeaderData["elementId"] = row[1];

            string id = row[0];
            int elementId = int.Parse(row[1], CultureInfo.InvariantCulture);
            Face face = (Face)Enum.Parse(typeof(Face), row[2], true);
            double w1 = double.Parse(row[3], CultureInfo.InvariantCulture);
            double d1 = double.Parse(row[4], CultureInfo.InvariantCulture);
            double w2 = double.Parse(row[5], CultureInfo.InvariantCulture);
            double d2 = double.Parse(row[6], CultureInfo.InvariantCulture);

            string lc = (row.Length > 7) ? row[7] : HeaderData["casename"];

            return new RCShellCrackWidth(id, elementId, face, w1, d1, w2, d2, lc);
        }
    }
}
