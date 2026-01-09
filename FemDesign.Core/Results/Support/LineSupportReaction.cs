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
    /// FemDesign "Line support group, Reactions" result
    /// </summary>
    [Result(typeof(LineSupportReaction), ListProc.LineSupportReactionsLoadCase, ListProc.LineSupportReactionsLoadCombination)]
    public partial class LineSupportReaction : IResult
    {
        /// <summary>
        /// Support name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Finite element identifier
        /// </summary>
        public int ElementId { get; }
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
        internal LineSupportReaction(string id, int elementId, int nodeId, double fx, double fy, double fz, double mx, double my, double mz, double fr, double mr, string resultCase)
        {
            Id = id;
            ElementId = elementId;
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
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Line support group), (?'result'Reactions), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Line support group), (?'result'Reactions), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^ID\tElem\tNode\tFx'\tFy'\tFz'\tMx'\tMy'\tMz'\tFr\tMr\t(Case|Comb\.)|^\[.+\]");
            }
        }

        internal static LineSupportReaction Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string supportname = row[0];
            int elementId = int.Parse(row[1], CultureInfo.InvariantCulture);
            int nodeId = int.Parse(row[2], CultureInfo.InvariantCulture);
            double fx = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double fy = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double fz = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double mx = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double my = Double.Parse(row[7], CultureInfo.InvariantCulture);
            double mz = Double.Parse(row[8], CultureInfo.InvariantCulture);
            double fr = Double.Parse(row[9], CultureInfo.InvariantCulture);
            double mr = Double.Parse(row[10], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new LineSupportReaction(supportname, elementId, nodeId, fx, fy, fz, mx, my, mz, fr, mr, lc);
        }

        /// <summary>
        /// The method has been created for returning the value for Grasshopper and Dynamo.
        /// The method can still be use for C# users.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="Result">the result.</param>
        /// <param name="LoadCase">the load case.</param>
        public static Dictionary<string, object> DeconstructLineSupportReaction(List<FemDesign.Results.LineSupportReaction> Result, string LoadCase)
        {
            var lineSupportReactions = Result.Cast<FemDesign.Results.LineSupportReaction>();

            // Return the unique load case - load combination
            var uniqueLoadCases = lineSupportReactions.Select(n => n.CaseIdentifier).Distinct().ToList();

            // Select the Nodal Reactions for the selected Load Case - Load Combination
            if (uniqueLoadCases.Contains(LoadCase, StringComparer.OrdinalIgnoreCase))
            {
                lineSupportReactions = lineSupportReactions.Where(n => String.Equals(n.CaseIdentifier, LoadCase, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                var warning = $"Load Case '{LoadCase}' does not exist";
                throw new ArgumentException(warning);
            }

            // Parse Results from the object
            var identifier = lineSupportReactions.Select(n => n.Id).ToList();
            var elementId = lineSupportReactions.Select(n =>n.ElementId).ToList();
            var nodeId = lineSupportReactions.Select(n => n.NodeId).ToList();
            var loadCases = lineSupportReactions.Select(n => n.CaseIdentifier).Distinct().ToList();
            var forceResultant = lineSupportReactions.Select(n => n.Fr).ToList();
            var momentResultant = lineSupportReactions.Select(n => n.Mr).ToList();

            // Create a Fd Vector/Point for Visualising the Reaction Forces
            var reactionForceVector = new List<FemDesign.Geometry.Vector3d>();
            var reactionMomentVector = new List<FemDesign.Geometry.Vector3d>();


            foreach (var reaction in lineSupportReactions)
            {
                var forceVector = new FemDesign.Geometry.Vector3d(reaction.Fx, reaction.Fy, reaction.Fz);
                var momentVector = new FemDesign.Geometry.Vector3d(reaction.Mx, reaction.My, reaction.Mz);

                reactionForceVector.Add(forceVector);
                reactionMomentVector.Add(momentVector);
            }


            return new Dictionary<string, dynamic>
            {
                {"CaseIdentifier", loadCases},
                {"Identifier", identifier},
                {"ElementId", elementId},
                {"NodeId", nodeId},
                {"ReactionForce", reactionForceVector},
                {"ReactionMoment", reactionMomentVector},
                {"ForceResultant", forceResultant},
                {"MomentResultant", momentResultant}
            };
        }
    }
}
