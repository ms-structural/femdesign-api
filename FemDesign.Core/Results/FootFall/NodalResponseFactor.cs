using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FemDesign.Calculate;

namespace FemDesign.Results
{
    /// <summary>
    /// Represents a Nodal Response Factor.
    /// </summary>
    [Result(typeof(NodalResponseFactor), ListProc.NodalResponseFactor)]
    public partial class NodalResponseFactor : IResult
    {
        /// <summary>
        /// Finite element node id
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Finite element node id
        /// </summary>
        public int NodeId { get; }

        /// <summary>
        /// Nodal response factor in global x
        /// </summary>
        public double RespFactorX { get; }

        /// <summary>
        /// Nodal response factor in global y
        /// </summary>
        public double RespFactorY { get; }

        /// <summary>
        /// Nodal response factor in global z
        /// </summary>
        public double RespFactorZ { get; }

        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal NodalResponseFactor(string id, int nodeId, double respFactorX, double respFactorY, double respFactorZ)
        {
            this.Id = id;
            this.NodeId = nodeId;
            this.RespFactorX = respFactorX;
            this.RespFactorY = respFactorY;
            this.RespFactorZ = respFactorZ;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, {Id}, {RespFactorX}, {RespFactorY}, {RespFactorZ}";
        }
        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Footfall analysis), (?'result'Nodal response factors), (?'casename'[\w\ ]+?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Footfall analysis), (?'result'Nodal response factors), (?'casename'[\w\ ]+?)(?: - selected objects)?$|^ID\tNode\tx\ty\tz|^\[.*\]");
            }
        }

        internal static NodalResponseFactor Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string id = row[0];
            int nodeId = Int16.Parse(row[1], CultureInfo.InvariantCulture);
            double respFactorX = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double respFactorY = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double respFactorZ = Double.Parse(row[4], CultureInfo.InvariantCulture);

            return new NodalResponseFactor(id, nodeId, respFactorX, respFactorY, respFactorZ);
        }
    }
}
