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
    /// FemDesign "Shell" result
    /// </summary>
    [Result(typeof(PointConnectionElement), ListProc.FemPointConnection)]
    public partial class PointConnectionElement : IResult
    {
        /// <summary>
        /// Connection name identifier
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Connection Element Index
        /// </summary>
        public int ElementId { get; }

        /// <summary>
        /// Point Connection Connectivity
        /// </summary>
        public int Node1 { get; }

        /// <summary>
        /// Point Connection Connectivity
        /// </summary>
        public int Node1Prime { get; }

        internal PointConnectionElement(string id, int elementId, int node1, int node1Prime)
        {
            this.Id = id;
            this.ElementId = elementId;
            this.Node1 = node1;
            this.Node1Prime = node1Prime;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, {Id}, {ElementId}, {Node1}, {Node1Prime}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Point connection elements)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Point connection elements)(?: - selected objects)?$|^Conn. ID\tElem\tNode 1\tNode 1'");
            }
        }

        internal static PointConnectionElement Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string id = row[0];
            int elementId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
            int node1 = Int32.Parse(row[2], CultureInfo.InvariantCulture);
            int node1Prime = Int32.Parse(row[3], CultureInfo.InvariantCulture);

            return new PointConnectionElement(id, elementId, node1, node1Prime);
        }
    }
}