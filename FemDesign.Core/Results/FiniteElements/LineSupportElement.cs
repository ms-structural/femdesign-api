using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using FemDesign.GenericClasses;

using FemDesign.Calculate;

namespace FemDesign.Results
{
    /// <summary>
    /// Represents a Line Support Element.
    /// </summary>
    [Result(typeof(LineSupportElement), ListProc.FemLineSupport)]
    public partial class LineSupportElement : IResult
    {
        /// <summary>
        /// Line support name identifier
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Line support Element Index
        /// </summary>
        public int ElementId { get; }

        /// <summary>
        /// Line support Connectivity
        /// </summary>
        public int Node1 { get; }

        /// <summary>
        /// Line support Connectivity
        /// Node i-2
        /// </summary>
        public int Node2 { get; }

        internal LineSupportElement(string id, int elementId, int node1, int node2)
        {
            this.Id = id;
            this.ElementId = elementId;
            this.Node1 = node1;
            this.Node2 = node2;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, {Id}, {ElementId}, {Node1}, {Node2}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Line support elements)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Line support elements)(?: - selected objects)?$|^Support ID\tElem\tNode 1\tNode 2");
            }
        }

        internal static LineSupportElement Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string id = row[0];
            int elementId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
            int node1 = Int32.Parse(row[2], CultureInfo.InvariantCulture);
            int node2 = Int32.Parse(row[3], CultureInfo.InvariantCulture);

            return new LineSupportElement(id, elementId, node1, node2);
        }
    }
}