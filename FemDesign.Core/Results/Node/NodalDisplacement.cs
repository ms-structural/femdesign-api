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
    /// FemDesign "Nodal displacements" result
    /// </summary>
    [Result(typeof(NodalDisplacement), ListProc.NodalDisplacementsLoadCase, ListProc.NodalDisplacementsLoadCombination)]
    public partial class NodalDisplacement : IResult
    {
        /// <summary>
        /// Support name identifier
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

        [JsonConstructor]
        internal NodalDisplacement(string id, int nodeId, double ex, double ey, double ez, double fix, double fiy, double fiz, string resultCase)
        {
            Id = id;
            NodeId = nodeId;
            Ex = ex;
            Ey = ey;
            Ez = ez;
            Fix = fix;
            Fiy = fiy;
            Fiz = fiz;
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
                return new Regex(@"^(?'type'Nodal displacements), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Nodal displacements), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^ID\tNode\tex\tey\tez\t\u03c6 x\t\u03c6 y\t\u03c6 z\t(Case|Comb\.)|^\[.*\]");
            }
        }

        internal static NodalDisplacement Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string supportname = row[0];
            int nodeId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
            double ex = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double ey = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double ez = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double fix = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double fiy = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double fiz = Double.Parse(row[7], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new NodalDisplacement(supportname, nodeId, ex, ey, ez, fix, fiy, fiz, lc);
        }

        /// <summary>
        /// The method has been created for returning the value for Grasshopper and Dynamo.
        /// The method can still be use for C# users.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="Result">the result.</param>
        /// <param name="LoadCase">the load case.</param>
        public static Dictionary<string, object> DeconstructNodalDisplacements(List<FemDesign.Results.NodalDisplacement> Result, string LoadCase)
        {
            var nodalDisplacements = Result.Cast<FemDesign.Results.NodalDisplacement>();

            // Return the unique load case - load combination
            var uniqueLoadCases = nodalDisplacements.Select(n => n.CaseIdentifier).Distinct().ToList();


            // Select the Nodal Displacement for the selected Load Case - Load Combination
            if (uniqueLoadCases.Contains(LoadCase, StringComparer.OrdinalIgnoreCase))
            {
                nodalDisplacements = nodalDisplacements.Where(n => String.Equals(n.CaseIdentifier, LoadCase, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                var warning = $"Load Case '{LoadCase}' does not exist";
                throw new ArgumentException(warning);
            }

            // Parse Results from the object
            var nodeId = nodalDisplacements.Select(n => n.NodeId).ToList();
            var loadCases = nodalDisplacements.Select(n => n.CaseIdentifier).Distinct().ToList();

            // Create a Rhino Vector for Displacement and Rotation
            var translation = new List<FemDesign.Geometry.Vector3d>();
            var rotation = new List<FemDesign.Geometry.Vector3d>();

            foreach (var nodeDisp in nodalDisplacements)
            {
                var transVector = new FemDesign.Geometry.Vector3d(nodeDisp.Ex, nodeDisp.Ey, nodeDisp.Ez);
                translation.Add(transVector);

                var rotVector = new FemDesign.Geometry.Vector3d(nodeDisp.Fix, nodeDisp.Fiy, nodeDisp.Fiz);
                rotation.Add(rotVector);
            }

            var CaseIdentifier = loadCases;
            var NodeId = nodeId;
            var Translation = translation;
            var Rotation = rotation;

            return new Dictionary<string, dynamic>
            {
                {"CaseIdentifier", CaseIdentifier},
                {"NodeId", NodeId},
                {"Translation", Translation},
                {"Rotation", Rotation},
            };
        }
    }
}
