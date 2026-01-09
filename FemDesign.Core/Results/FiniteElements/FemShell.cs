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
    [Result(typeof(FemShell), ListProc.FemShell)]
    public partial class FemShell : IResult
    {
        /// <summary>
        /// Shell name identifier
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Shell Element Index
        /// </summary>
        public int ElementId { get; }

        /// <summary>
        /// Shell Connectivity
        /// Node i-1
        /// </summary>
        public int Node1 { get; }

        /// <summary>
        /// Shell Connectivity
        /// Node i-2
        /// </summary>
        public int Node2 { get; }

        /// <summary>
        /// Shell Connectivity
        /// Node i-3
        /// </summary>
        public int Node3 { get; }

        /// <summary>
        /// Shell Connectivity
        /// Node i-4
        /// </summary>
        public int Node4 { get; }

        internal FemShell(string id, int elementId, int node1, int node2, int node3, int node4)
        {
            this.Id = id;
            this.ElementId = elementId;
            this.Node1 = node1;
            this.Node2 = node2;
            this.Node3 = node3;
            this.Node4 = node4;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, {ElementId}, {Node1}, {Node2}, {Node3}, {Node4}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Surface elements)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Surface elements)(?: - selected objects)?$|^Shell\tElem\tNode 1\tNode 2\tNode 3\tNode 4");
            }
        }

        internal static FemShell Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string id = row[0];
            int elementId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
            int node1 = Int32.Parse(row[2], CultureInfo.InvariantCulture);
            int node2 = Int32.Parse(row[3], CultureInfo.InvariantCulture);
            int node3 = Int32.Parse(row[4], CultureInfo.InvariantCulture);
            int node4 = Int32.Parse(row[5], CultureInfo.InvariantCulture);

            return new FemShell(id, elementId, node1, node2, node3, node4);
        }

        /// <summary>
        /// The method has been created for returning the value for Grasshopper and Dynamo.
        /// The method can still be use for C# users.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="Result">the result.</param>
        public static Dictionary<string, object> DeconstructFeaShell(List<FemDesign.Results.FemShell> Result)
        {
            FemDesign.Geometry.Face face;
            var feaShells = Result.Cast<FemDesign.Results.FemShell>();

            // Parse Results from the object
            var id = feaShells.Select(n => n.Id).ToList();
            var elementId = feaShells.Select(n => n.ElementId).ToList();

            // Create a Fd Face for Visualising the Mesh Geometry
            var feaShellFaces = new List<FemDesign.Geometry.Face>();
            
            foreach (var obj in feaShells)
            {
                // Triangular Shells have node4 specified as 0
                if(obj.Node4 == 0)
                {
                    // Create a Triangular Face
                    face = new FemDesign.Geometry.Face(obj.Node1, obj.Node2, obj.Node3);
                }
                else
                {
                    // Create a Quadrangular Face
                    face = new FemDesign.Geometry.Face(obj.Node1, obj.Node2, obj.Node3, obj.Node4);
                }
                feaShellFaces.Add(face);
            }

            return new Dictionary<string, dynamic>
            {
                {"Identifier", id},
                {"ElementId", elementId},
                {"Face", feaShellFaces}
            };
        }
    }
}