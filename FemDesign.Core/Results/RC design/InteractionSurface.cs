using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace FemDesign.Results
{
    /// <summary>
    /// Represents a Interaction Surface.
    /// </summary>
    public partial class InteractionSurface
    {
        /// <summary>
        /// Gets or sets the vertices.
        /// </summary>
        public Dictionary<int, FemDesign.Geometry.Point3d> Vertices { get; set; }
        /// <summary>
        /// Gets or sets the faces.
        /// </summary>
        public Dictionary<int, FemDesign.Geometry.Face> Faces { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionSurface"/> class.
        /// </summary>
        /// <param name="vertices">the vertices.</param>
        /// <param name="faces">the faces.</param>
        public InteractionSurface(Dictionary<int, FemDesign.Geometry.Point3d> vertices, Dictionary<int, FemDesign.Geometry.Face> faces)
        {
            Vertices = vertices;
            Faces = faces;
        }

        internal static InteractionSurface ReadFromFile(string filepath)
        {
            var vertices = new Dictionary<int, Geometry.Point3d>();
            var faces = new Dictionary<int, Geometry.Face>();

            bool isFace = false;

            using (System.IO.StreamReader reader = new StreamReader(filepath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("#node") || line.StartsWith("id"))
                    {
                        continue;
                    }

                    if (line.StartsWith("#face"))
                    {
                        isFace = true;
                        continue;
                    }

                    if (line.StartsWith("t:"))
                    {
                        break;
                    }

                    string[] values = line.Split();
                    int key = int.Parse( values[0] ) - 1;

                    if (isFace)
                    {
                        int node_i = int.Parse( values[1], CultureInfo.InvariantCulture) - 1;
                        int node_j = int.Parse( values[2], CultureInfo.InvariantCulture) - 1;
                        int node_k = int.Parse( values[3], CultureInfo.InvariantCulture) - 1;
                        faces[key] = new Geometry.Face(node_i, node_j, node_k);
                    }
                    else
                    {
                        var x = Double.Parse( values[1], CultureInfo.InvariantCulture);
                        var y = Double.Parse( values[2], CultureInfo.InvariantCulture);
                        var z = Double.Parse( values[3], CultureInfo.InvariantCulture) * -1;
                        vertices[key] = new Geometry.Point3d(x, y, z);
                    }
                }
            }

            return new InteractionSurface(vertices, faces);
        }
    }
}