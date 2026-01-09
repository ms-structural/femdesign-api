// https://strusoft.com/

using System;
using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    /// <summary>
    /// Represents a Face.
    /// </summary>
    [System.Serializable]
    public partial class Face
    {
        /// <summary>
        /// Gets or sets the node1.
        /// </summary>
        [XmlIgnore]
        public int? Node1 { get; }
        /// <summary>
        /// Gets or sets the node2.
        /// </summary>
        [XmlIgnore]
        public int? Node2 { get; }
        /// <summary>
        /// Gets or sets the node3.
        /// </summary>
        [XmlIgnore]
        public int? Node3 { get; }
        /// <summary>
        /// Gets or sets the node4.
        /// </summary>
        [XmlIgnore]
        public int? Node4 { get; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Face()
        {

        }

        /// <summary>
        /// Construct Face from index node1 node2 node3 node4.
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <param name="node3"></param>
        /// <param name="node4"></param>
        public Face(int node1, int node2, int node3, int node4)
        {
            this.Node1 = node1;
            this.Node2 = node2;
            this.Node3 = node3;
            this.Node4 = node4;
        }

        /// <summary>
        /// Construct Face from index node1 node2 node3 node4.
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <param name="node3"></param>
        public Face(int node1, int node2, int node3)
        {
            this.Node1 = node1;
            this.Node2 = node2;
            this.Node3 = node3;
        }

        /// <summary>
        /// Determines whether triangle.
        /// </summary>
        /// <returns>The result.</returns>
        public bool IsTriangle()
        {
            bool isTriangle = Node4 == null ? true : false;
            return isTriangle;
        }

        /// <summary>
        /// Determines whether quad.
        /// </summary>
        /// <returns>The result.</returns>
        public bool IsQuad()
        {
            bool isQuad = Node4 != null ? true : false;
            return isQuad;
        }

        /// <summary>
        /// Defines an operator overload.
        /// </summary>
        /// <param name="feaShell">the fea shell.</param>
        /// <returns>The result.</returns>
        public static implicit operator Face(FemDesign.Results.FemShell feaShell)
        {
            var face = new Geometry.Face(feaShell.Node1, feaShell.Node2, feaShell.Node3, feaShell.Node4);
            return face;
        }

    }
}