using FemDesign.Calculate;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FemDesign.Results
{
    /// <summary>
    /// Represents a Equilibrium.
    /// </summary>
    [Result(typeof(Equilibrium), ListProc.EquilibriumLoadCase, ListProc.EquilibriumLoadCombination)]
    public class Equilibrium : IResult
    {
        /// <summary>
        /// Gets or sets the case identifier.
        /// </summary>
        public string CaseIdentifier { get; }
        /// <summary>
        /// Gets or sets the component.
        /// </summary>
        public string Component { get; }
        /// <summary>
        /// Gets or sets the loads.
        /// </summary>
        public double Loads { get; }
        /// <summary>
        /// Gets or sets the reactions.
        /// </summary>
        public double Reactions { get; }
        /// <summary>
        /// Error express in percentage.
        /// </summary>
        public double Error { get; }


        [JsonConstructor]
        internal Equilibrium(string caseIdentifier, string component, double loads, double reactions, double error)
        {
            this.CaseIdentifier = caseIdentifier;
            this.Component = component;
            this.Loads = loads;
            this.Reactions = reactions;
            this.Error = error;
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
                return new Regex(@"(?'type'Equilibrium)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'type'Equilibrium)|(Load comb\tComp.\tLoads\tReactions\tError|Case\tComponent\tLoads\tReactions\tError)|\[.*\]");
            }
        }

        internal static Equilibrium Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string caseIdentifier = row[0];
            string component = row[1];
            double loads = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double reactions = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double error = row[4] == "-" ? 0 : Double.Parse(row[4], CultureInfo.InvariantCulture);
            return new Equilibrium(caseIdentifier, component, loads, reactions, error);
        }

    }
}
