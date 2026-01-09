using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using FemDesign.Calculate;

namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "CLT Panel: utilization"
    /// </summary>
    [Result(typeof(CLTFireUtilization), ListProc.CLTPanelFireUtilizationLoadCombination)]
    public class CLTFireUtilization : CLTShellUtilization, IResult
    {
        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        public double Duration { get; }
        /// <summary>
        /// Gets or sets the protection.
        /// </summary>
        public string Protection { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CLTFireUtilization"/> class.
        /// </summary>
        /// <param name="id">the id.</param>
        /// <param name="duration">the duration.</param>
        /// <param name="fireProtection">the fire protection.</param>
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
        public CLTFireUtilization(string id, double duration, string fireProtection, double max, double tensionX, double tensionY, double compressionX, double compressionY, double shearXY, double shearX, double shearY, double shearInteraction, double tensionShear, double compressionShear, double buckling, double torsion, string caseIndentifier) : base(id, max, tensionX, tensionY, compressionX, compressionY, shearXY, shearX,  shearY, shearInteraction, tensionShear, compressionShear, buckling, torsion, caseIndentifier)
        {
            Id = id;
            Duration = duration;
            Protection = fireProtection;
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

        internal new static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'max'Max\. of load combinations, CLT panel, Fire design, Utilization)(?: - selected objects)?$|(?'type'CLT panel, Fire design, Utilization), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal new static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'max'Max\. of load combinations, CLT panel, Fire design, Utilization)(?: - selected objects)?$|(?'type'CLT panel, Fire design, Utilization), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^Panel\tDuration\tFire protection\tMax\.\t(Combination\t)?Sx\+\tSy\+\tSx-\tSy-\tTxy\tTx\tTy\tSI\tTS\tCS\tBu\tTo|^\[.*\]");
            }
        }

        internal static CLTFireUtilization Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            if (HeaderData.ContainsKey("max"))
            {
                string id = row[0];
                double duration = Double.Parse(row[1], CultureInfo.InvariantCulture);
                string fireProtection = row[2];
                double max = Double.Parse(row[3], CultureInfo.InvariantCulture); ;
                double tensionX = Double.Parse(row[5] == "-" ? "0.00" : row[5], CultureInfo.InvariantCulture);
                double tensiony = Double.Parse(row[6] == "-" ? "0.00" : row[6], CultureInfo.InvariantCulture);
                double compressionX = Double.Parse(row[7] == "-" ? "0.00" : row[7], CultureInfo.InvariantCulture);
                double compressionY = Double.Parse(row[8] == "-" ? "0.00" : row[8], CultureInfo.InvariantCulture);
                double shearXY = Double.Parse(row[9] == "-" ? "0.00" : row[9], CultureInfo.InvariantCulture);
                double shearX = Double.Parse(row[10] == "-" ? "0.00" : row[10], CultureInfo.InvariantCulture);
                double shearY = Double.Parse(row[11] == "-" ? "0.00" : row[11], CultureInfo.InvariantCulture);
                double shearInteraction = Double.Parse(row[12] == "-" ? "0.00" : row[12], CultureInfo.InvariantCulture);
                double tensionShear = Double.Parse(row[13] == "-" ? "0.00" : row[13], CultureInfo.InvariantCulture);
                double compressionShear = Double.Parse(row[14] == "-" ? "0.00" : row[14], CultureInfo.InvariantCulture);
                double buckling = Double.Parse(row[15] == "-" ? "0.00" : row[15], CultureInfo.InvariantCulture);
                double torsion = Double.Parse(row[16] == "-" ? "0.00" : row[16], CultureInfo.InvariantCulture);

                string caseIndentifier = row[4];

                return new CLTFireUtilization(id, duration, fireProtection, max, tensionX, tensiony, compressionX, compressionY, shearXY, shearX, shearY, shearInteraction, tensionShear, compressionShear, buckling, torsion, caseIndentifier);
            }
            else
            {
                string id = row[0];
                double duration = Double.Parse(row[1], CultureInfo.InvariantCulture);
                string fireProtection = row[2];
                double max = Double.Parse(row[3], CultureInfo.InvariantCulture); ;
                double tensionX = Double.Parse(row[4] == "-" ? "0.00" : row[4], CultureInfo.InvariantCulture);
                double tensiony = Double.Parse(row[5] == "-" ? "0.00" : row[5], CultureInfo.InvariantCulture);
                double compressionX = Double.Parse(row[6] == "-" ? "0.00" : row[6], CultureInfo.InvariantCulture);
                double compressionY = Double.Parse(row[7] == "-" ? "0.00" : row[7], CultureInfo.InvariantCulture);
                double shearXY = Double.Parse(row[8] == "-" ? "0.00" : row[8], CultureInfo.InvariantCulture);
                double shearX = Double.Parse(row[9] == "-" ? "0.00" : row[9], CultureInfo.InvariantCulture);
                double shearY = Double.Parse(row[10] == "-" ? "0.00" : row[10], CultureInfo.InvariantCulture);
                double shearInteraction = Double.Parse(row[11] == "-" ? "0.00" : row[11], CultureInfo.InvariantCulture);
                double tensionShear = Double.Parse(row[12] == "-" ? "0.00" : row[12], CultureInfo.InvariantCulture);
                double compressionShear = Double.Parse(row[13] == "-" ? "0.00" : row[13], CultureInfo.InvariantCulture);
                double buckling = Double.Parse(row[14] == "-" ? "0.00" : row[14], CultureInfo.InvariantCulture);
                double torsion = Double.Parse(row[15] == "-" ? "0.00" : row[15], CultureInfo.InvariantCulture);

                string caseIndentifier = HeaderData["casename"];

                return new CLTFireUtilization(id, duration, fireProtection, max, tensionX, tensiony, compressionX, compressionY, shearXY, shearX, shearY, shearInteraction, tensionShear, compressionShear, buckling, torsion, caseIndentifier);
            }
        }
    }
}