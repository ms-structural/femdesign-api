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
    /// FemDesign "Nodal buckling shapes" result
    /// </summary>
    [Result(typeof(NodalBucklingShape), ListProc.NodalBucklingShape)]
    public partial class NodalBucklingShape : IResult
    {
        /// <summary>
        /// Structural object name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Finite element node id
        /// </summary>
        public int NodeId { get; }
        /// <summary>
        /// Displacement in global x
        /// </summary>
        public double Ex { get; }
        /// <summary>
        /// Displacement in global y
        /// </summary>
        public double Ey { get; }
        /// <summary>
        /// Displacement in global z
        /// </summary>
        public double Ez { get; }
        /// <summary>
        /// Rotation around global x
        /// </summary>
        public double Fix { get; }
        /// <summary>
        /// Rotation around global y
        /// </summary>
        public double Fiy { get; }
        /// <summary>
        /// Rotation around global z
        /// </summary>
        public double Fiz { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }
        /// <summary>
        /// Buckling shape identifier
        /// </summary>
        public int Shape { get; }

        [JsonConstructor]
        internal NodalBucklingShape(string id, int nodeId, double ex, double ey, double ez, double fix, double fiy, double fiz, string resultCase, int shapeID)
        {
            this.Id = id;
            this.NodeId = nodeId;
            this.Ex = ex;
            this.Ey = ey;
            this.Ez = ez;
            this.Fix = fix;
            this.Fiy = fiy;
            this.Fiz = fiz;
            this.CaseIdentifier = resultCase;
            this.Shape = shapeID;
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
                //A load combination name can be up to 159 characters long.
                return new Regex(@"^(?'type'Nodal buckling shapes), (?'casename'[ -#%'-;=?A-\ufffd]{1,159}) / (?'shape'[\d]+?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Nodal buckling shapes), (?'casename'[ -#%'-;=?A-\ufffd]{1,159}) / (?'shape'[\d]+?)(?: - selected objects)?$|^ID\tNode\tex\tey\tez\t\u03c6 x\t\u03c6 y\t\u03c6 z|^\[.*\]");
            }
        }
        
        internal static NodalBucklingShape Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string identifier = row[0];
            int nodeId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
            double ex = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double ey = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double ez = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double fix = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double fiy = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double fiz = Double.Parse(row[7], CultureInfo.InvariantCulture);
            string resultCase = HeaderData["casename"];
            int shapeID = Int32.Parse(HeaderData["shape"]);
            return new NodalBucklingShape(identifier, nodeId, ex, ey, ez, fix, fiy, fiz, resultCase, shapeID);
        }

    }
}