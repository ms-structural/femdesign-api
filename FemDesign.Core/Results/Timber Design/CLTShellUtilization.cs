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
    /// FemDesign "CLT Panel: utilization"
    /// </summary>
    [Result(typeof(CLTShellUtilization), ListProc.CLTPanelUtilizationLoadCombination)]
    public class CLTShellUtilization : IResult
    {
        /// <summary>
        /// Panel name identifier
        /// </summary>
        public string Id { get; protected set; }
        /// <summary>
        /// Max utilization [%]
        /// </summary>
        public double Max { get; protected set; }
        /// <summary>
        /// Tension X [%]
        /// </summary>
        public double TensionX { get; protected set; }
        /// <summary>
        /// Tension Y [%]
        /// </summary>
        public double TensionY { get; protected set; }
        /// <summary>
        /// Compression X [%]
        /// </summary>
        public double CompressionX { get; protected set; }
        /// <summary>
        /// Compression Y [%]
        /// </summary>
        public double CompressionY { get; protected set; }
        /// <summary>
        /// Shear XY [%]
        /// </summary>
        public double ShearXY { get; protected set; }
        /// <summary>
        /// Shear X [%]
        /// </summary>
        public double ShearX { get; protected set; }
        /// <summary>
        ///  Shear Y [%]
        /// </summary>
        public double ShearY { get; protected set; }
        /// <summary>
        /// Shear Interaction [%]
        /// </summary>
        public double ShearInteraction { get; protected set; }
        /// <summary>
        /// Tension Shear [%]
        /// </summary>
        public double TensionShear { get; protected set; }
        /// <summary>
        /// Compression Shear [%]
        /// </summary>
        public double CompressionShear { get; protected set; }
        /// <summary>
        /// Buckling [%]
        /// </summary>
        public double Buckling { get; protected set; }
        /// <summary>
        /// Torsion [%]
        /// </summary>
        public double Torsion { get; protected set; }
        /// <summary>
        /// Gets or sets the deflection.
        /// </summary>
        public double Deflection { get; protected set; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CLTShellUtilization"/> class.
        /// </summary>
        /// <param name="id">the id.</param>
        /// <param name="max">the max.</param>
        /// <param name="tensionX">the tension x.</param>
        /// <param name="tensionY">the tension y.</param>
        /// <param name="compressionX">the compression x.</param>
        /// <param name="compressionY">the compression y.</param>
        /// <param name="shearXY">the shear xy.</param>
        /// <param name="shearX">the shear x.</param>
        /// <param name="shearY">the shear y.</param>
        /// <param name="shearInteraction">the shear interaction.</param>
        /// <param name="tensionShear">the tension shear.</param>
        /// <param name="compressionShear">the compression shear.</param>
        /// <param name="buckling">the buckling.</param>
        /// <param name="torsion">the torsion.</param>
        /// <param name="caseIndentifier">the case indentifier.</param>
        [JsonConstructor]
        public CLTShellUtilization(string id, double max, double tensionX, double tensionY, double compressionX, double compressionY, double shearXY, double shearX, double shearY, double shearInteraction, double tensionShear, double compressionShear, double buckling, double torsion, string caseIndentifier)
        {
            Id = id;
            Max = max;
            TensionX = tensionX;
            TensionY = tensionY;
            CompressionX = compressionX;
            CompressionY = compressionY;
            ShearXY = shearXY;
            ShearX = shearX;
            ShearY = shearY;
            ShearInteraction = shearInteraction;
            TensionShear = tensionShear;
            CompressionShear = compressionShear;
            Buckling = buckling;
            Torsion = torsion;
            CaseIdentifier = caseIndentifier;
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
                return new Regex(@"(?'max'Max\. of load combinations, CLT panel, Utilization)(?: - selected objects)?$|(?'type'CLT panel, Utilization), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'max'Max\. of load combinations, CLT panel, Utilization)(?: - selected objects)?$|(?'type'CLT panel, Utilization), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^Panel\tMax\.\t(Combination\t)?Sx\+\tSy\+\tSx-\tSy-\tTxy\tTx\tTy\tSI\tTS\tCS\tBu\tTo|^\[.*\]");
            }
        }

        internal static CLTShellUtilization Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            if (HeaderData.ContainsKey("max"))
            {
                string id = row[0];
                double max = Double.Parse(row[1], CultureInfo.InvariantCulture);
                double tensionX = Double.Parse(row[3] == "-" ? "0.00" : row[3], CultureInfo.InvariantCulture);
                double tensiony = Double.Parse(row[4] == "-" ? "0.00" : row[4], CultureInfo.InvariantCulture);
                double compressionX = Double.Parse(row[5] == "-" ? "0.00" : row[5], CultureInfo.InvariantCulture);
                double compressionY = Double.Parse(row[6] == "-" ? "0.00" : row[6], CultureInfo.InvariantCulture);
                double shearXY = Double.Parse(row[7] == "-" ? "0.00" : row[7], CultureInfo.InvariantCulture);
                double shearX = Double.Parse(row[8] == "-" ? "0.00" : row[8], CultureInfo.InvariantCulture);
                double shearY = Double.Parse(row[9] == "-" ? "0.00" : row[9], CultureInfo.InvariantCulture);
                double shearInteraction = Double.Parse(row[10] == "-" ? "0.00" : row[10], CultureInfo.InvariantCulture);
                double tensionShear = Double.Parse(row[11] == "-" ? "0.00" : row[11], CultureInfo.InvariantCulture);
                double compressionShear = Double.Parse(row[12] == "-" ? "0.00" : row[12], CultureInfo.InvariantCulture);
                double buckling = Double.Parse(row[13] == "-" ? "0.00" : row[13], CultureInfo.InvariantCulture);
                double torsion = Double.Parse(row[14] == "-" ? "0.00" : row[14], CultureInfo.InvariantCulture);
                string caseIndentifier = row[2];

                return new CLTShellUtilization(id, max, tensionX, tensiony, compressionX, compressionY, shearXY, shearX, shearY, shearInteraction, tensionShear, compressionShear, buckling, torsion, caseIndentifier);
            }
            else
            {
                string id = row[0];
                double max = Double.Parse(row[1], CultureInfo.InvariantCulture); ;
                double tensionX = Double.Parse(row[2] == "-" ? "0.00" : row[2], CultureInfo.InvariantCulture);
                double tensiony = Double.Parse(row[3] == "-" ? "0.00" : row[3], CultureInfo.InvariantCulture);
                double compressionX = Double.Parse(row[4] == "-" ? "0.00" : row[4], CultureInfo.InvariantCulture);
                double compressionY = Double.Parse(row[5] == "-" ? "0.00" : row[5], CultureInfo.InvariantCulture);
                double shearXY = Double.Parse(row[6] == "-" ? "0.00" : row[6], CultureInfo.InvariantCulture);
                double shearX = Double.Parse(row[7] == "-" ? "0.00" : row[7], CultureInfo.InvariantCulture);
                double shearY = Double.Parse(row[8] == "-" ? "0.00" : row[8], CultureInfo.InvariantCulture);
                double shearInteraction = Double.Parse(row[9] == "-" ? "0.00" : row[9], CultureInfo.InvariantCulture);
                double tensionShear = Double.Parse(row[10] == "-" ? "0.00" : row[10], CultureInfo.InvariantCulture);
                double compressionShear = Double.Parse(row[11] == "-" ? "0.00" : row[11], CultureInfo.InvariantCulture);
                double buckling = Double.Parse(row[12] == "-" ? "0.00" : row[12], CultureInfo.InvariantCulture);
                double torsion = Double.Parse(row[13] == "-" ? "0.00" : row[13], CultureInfo.InvariantCulture);

                string caseIndentifier = HeaderData["casename"];

                return new CLTShellUtilization(id, max, tensionX, tensiony, compressionX, compressionY, shearXY, shearX, shearY, shearInteraction, tensionShear, compressionShear, buckling, torsion, caseIndentifier);
            }
        }
    }
}