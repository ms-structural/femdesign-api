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
    /// FemDesign "Imperfection factors" result
    /// </summary>
    [Result(typeof(ImperfectionFactor), ListProc.ImperfectionFactors)]
    public partial class ImperfectionFactor : IResult
    {
        /// <summary>
        /// Gets or sets the case identifier.
        /// </summary>
        public string CaseIdentifier { get; }
        /// <summary>
        /// Gets or sets the shape.
        /// </summary>
        public int Shape { get; }
        /// <summary>
        /// Gets or sets the critical param.
        /// </summary>
        public double CriticalParam { get; }
        /// <summary>
        /// Gets or sets the amplitude.
        /// </summary>
        public double Amplitude { get; }

        [JsonConstructor]
        internal ImperfectionFactor(string resultCase, int shape, double criticalParam, double amplitude)
        {
            CaseIdentifier = resultCase;
            Shape = shape;
            CriticalParam = criticalParam;
            Amplitude = amplitude;
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
                return new Regex(@"^(?'type'Imperfection factors) \(Only positive cr\.\)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Imperfection factors) \(Only positive cr\.\)|^Comb\tShape\tCritical param\.\tAmplitude|^\t\t\[.*\]");
            }
        }

        internal static ImperfectionFactor Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string caseIdentifier = row[0];
            int shape = Int32.Parse(row[1], CultureInfo.InvariantCulture);
            double criticalParam = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double amplitude = Double.Parse(row[3], CultureInfo.InvariantCulture);
            return new ImperfectionFactor(caseIdentifier, shape, criticalParam, amplitude);
        }
    }
}