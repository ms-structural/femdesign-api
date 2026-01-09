using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using FemDesign.Calculate;
using Newtonsoft.Json;
namespace FemDesign.Results
{
    /// <summary>
    /// Represents a Nodal Acceleration.
    /// </summary>
    [Result(typeof(NodalAcceleration), ListProc.NodalAcceleration)]
    public partial class NodalAcceleration : IResult
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
        /// Nodal acceleration in global x [m/s2]
        /// </summary>
        public double AccX { get; }

        /// <summary>
        /// Nodal acceleration in global y [m/s2]
        /// </summary>
        public double AccY { get; }

        /// <summary>
        /// Nodal acceleration in global z [m/s2]
        /// </summary>
        public double AccZ { get; }

        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal NodalAcceleration(string id, int nodeId, double accX, double accY, double accZ)
        {
            this.Id = id;
            this.NodeId = nodeId;
            this.AccX = accX;
            this.AccY = accY;
            this.AccZ = accZ;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, {Id}, {AccX}, {AccY}, {AccZ}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Footfall analysis), (?'result'Nodal accelerations), (?'casename'[\w\ ]+?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Footfall analysis), (?'result'Nodal accelerations), (?'casename'[\w\ ]+?)(?: - selected objects)?$|^ID\tNode\tax\tay\taz|^\[.*\]");
            }
        }

        internal static NodalAcceleration Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string id = row[0];
            int nodeId = Int16.Parse(row[1], CultureInfo.InvariantCulture);
            double accX = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double accY = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double accZ = Double.Parse(row[4], CultureInfo.InvariantCulture);

            return new NodalAcceleration(id, nodeId, accX, accY, accZ);
        }
    }
}
