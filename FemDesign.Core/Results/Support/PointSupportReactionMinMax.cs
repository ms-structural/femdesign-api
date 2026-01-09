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
    /// FemDesign "Point support group, Reactions" result
    /// </summary>
    [Result(typeof(PointSupportReactionMinMax), ListProc.PointSupportReactionsMaxOfLoadCombinationMinMax)]
    public partial class PointSupportReactionMinMax : IResult
    {
        /// <summary>
        /// Max or Min
        /// </summary>
        public string Max { get; }
        /// <summary>
        /// Support name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// X-coordinate
        /// </summary>
        public double X { get; }
        /// <summary>
        /// Y-coordinate
        /// </summary>
        public double Y { get; }
        /// <summary>
        /// Z-coordinate
        /// </summary>
        public double Z { get; }
        /// <summary>
        /// Finite element node id
        /// </summary>
        public int NodeId { get; }
        /// <summary>
        /// Local Fx'
        /// </summary>
        public double Fx { get; }
        /// <summary>
        /// Local Fy'
        /// </summary>
        public double Fy { get; }
        /// <summary>
        /// Local Fz'
        /// </summary>
        public double Fz { get; }
        /// <summary>
        /// Local Mx'
        /// </summary>
        public double Mx { get; }
        /// <summary>
        /// Local My'
        /// </summary>
        public double My { get; }
        /// <summary>
        /// Local Mz'
        /// </summary>
        public double Mz { get; }
        /// <summary>
        /// Force resultant
        /// </summary>
        public double Fr { get; }
        /// <summary>
        /// Moment resultant
        /// </summary>
        public double Mr { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal PointSupportReactionMinMax(string max, string id, double x, double y, double z, int nodeId, double fx, double fy, double fz, double mx, double my, double mz, double fr, double mr, string resultCase)
        {
            Max = max;
            Id = id;
            X = x;
            Y = y;
            Z = z;
            NodeId = nodeId;
            Fx = fx;
            Fy = fy;
            Fz = fz;
            Mx = mx;
            My = my;
            Mz = mz;
            Fr = fr;
            Mr = mr;
            CaseIdentifier = resultCase;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{this.GetType().Name}, Max: {this.Max}, Id: {this.Id}, NodeId: {this.NodeId}, Pos: ({this.X}, {this.Y}, {this.Z}), Force: ({this.Fx}, {this.Fy}, {this.Fz}), Moment: ({this.Mx}, {this.My}, {this.Mz}), Fr: {this.Fr}, Mr: {this.Mr}, CaseIdentifier: {this.CaseIdentifier}";

        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^Max\. of load combinations, (?'type'Point support group), (?'result'Reactions), MinMax, (?'casename'(Ultimate|Accidental|Quasi-permanent))(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^Max\. of load combinations, (?'type'Point support group), (?'result'Reactions), MinMax, (?'casename'(Ultimate|Accidental|Quasi-permanent))(?: - selected objects)?$|^Max\.\tID\tx\ty\tz\tNode\tFx'\tFy'\tFz'\tMx'\tMy'\tMz'\tFr\tMr\tComb|^\[.*\]");
            }
        }

        internal static PointSupportReactionMinMax Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string max = row[0];
            string supportname = row[1];
            double x = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double y = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double z = Double.Parse(row[4], CultureInfo.InvariantCulture);
            int nodeId = int.Parse(row[5], CultureInfo.InvariantCulture);
            double fx = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double fy = Double.Parse(row[7], CultureInfo.InvariantCulture);
            double fz = Double.Parse(row[8], CultureInfo.InvariantCulture);
            double mx = Double.Parse(row[9], CultureInfo.InvariantCulture);
            double my = Double.Parse(row[10], CultureInfo.InvariantCulture);
            double mz = Double.Parse(row[11], CultureInfo.InvariantCulture);
            double fr = Double.Parse(row[12], CultureInfo.InvariantCulture);
            double mr = Double.Parse(row[13], CultureInfo.InvariantCulture);
            string lc = row[14];
            return new PointSupportReactionMinMax(max, supportname, x, y, z, nodeId, fx, fy, fz, mx, my, mz, fr, mr, lc);
        }
    }
}
