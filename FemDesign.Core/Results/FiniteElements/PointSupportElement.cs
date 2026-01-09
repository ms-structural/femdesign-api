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
    /// Represents a Point Support Element.
    /// </summary>
    [Result(typeof(PointSupportElement), ListProc.FemPointSupport)]
    public partial class PointSupportElement : IResult
    {
        /// <summary>
        /// Point support name identifier
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Point support Element Index
        /// </summary>
        public int ElementId { get; }

        /// <summary>
        /// Point support node
        /// </summary>
        public int Node { get; }

        internal PointSupportElement(string id, int elementId, int node)
        {
            this.Id = id;
            this.ElementId = elementId;
            this.Node = node;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, {Id}, {ElementId}, {Node}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Point support elements)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Point support elements)(?: - selected objects)?$|^Support ID\tElem\tNode");
            }
        }

        internal static PointSupportElement Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string id = row[0];
            int elementId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
            int node = Int32.Parse(row[2], CultureInfo.InvariantCulture);

            return new PointSupportElement(id, elementId, node);
        }
    }
}