// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    /// <summary>
    /// contour_type
    /// </summary>
    [System.Serializable]
    public partial class Contour
    {

        /// <summary>
        /// Get LocalZ of Contour by analysing the direction of the contour.
        /// </summary>
        [XmlIgnore]
        internal Geometry.Vector3d LocalZ
        {
            get
            {
                // contour is circle
                if (this.Edges.Count == 0)
                {
                    throw new System.ArgumentException("Contour contains no edges.");
                }

                else if (this.Edges.Count == 1)
                {
                    // return normal
                    return this.Edges[0].Normal;
                }

                else
                {
                    // any arcs in contour?
                    Edge firstArc = this.Edges.Find(item => item.Type == "arc");
                    if (firstArc != null)
                    {
                        // return normal
                        return firstArc.Normal;
                    }

                    // no arcs in contour?
                    else
                    {
                        // select points
                        var points = this.Edges.Select(x => x.Points.Last());
                        
                        // get ranges
                        double rangeX = points.Max(item => item.X) - points.Min(item => item.X);
                        double rangeY = points.Max(item => item.Y) - points.Min(item => item.Y);
                        double rangeZ = points.Max(item => item.Y) - points.Min(item => item.Z);

                        
                        // get index of edge with outermost point
                        int idx;
                        if (rangeX != 0)
                        {
                            idx = this.Edges.IndexOf(this.Edges.OrderBy(item => item.Points.Last().X).Last());
                        }
                        else if (rangeY != 0)
                        {
                            idx = this.Edges.IndexOf(this.Edges.OrderBy(item => item.Points.Last().Y).Last());
                        }
                        else if (rangeZ != 0)
                        {
                            idx = this.Edges.IndexOf(this.Edges.OrderBy(item => item.Points.Last().Z).Last());
                        }
                        else
                        {
                            throw new System.ArgumentException("Could not get index of edge with outermost point.");
                        }

                        // get edges
                        Edge[] edges = new Edge[2];
                        if (idx == 0)
                        {
                            edges[0] = this.Edges[0];
                            edges[1] = this.Edges[1];
                        }
                        else if (idx == this.Edges.Count - 1)
                        {
                            edges[0] = this.Edges.Last();
                            edges[1] = this.Edges[0];
                        }
                        else
                        {
                            edges[0] = this.Edges[idx];
                            edges[1] = this.Edges[idx + 1];
                        }

                        // construct vector
                        Vector3d v0 = new Vector3d(edges[0].Points[0], edges[0].Points[1]);
                        Vector3d v1 = new Vector3d(edges[1].Points[0], edges[1].Points[1]);

                        // return normal
                        return v0.Cross(v1).Normalize();
                    }   
                }
            }
        }

        [XmlElement("edge")]
        public List<Edge> Edges = new List<Edge>(); // sequence: edge_type

        /// <summary>
        /// Collect all existing points from Edges.
        /// </summary>
        [XmlIgnore]
        public List<Point3d> Points
        {
            get
            {
                if(this.Edges?.Count == 0)
                {
                    return new List<Point3d>();
                }

                List<Point3d> outPts = new List<Point3d> { Edges[0].Points[0] };

                for (int i = 0; i< Edges.Count; i++)    // if Edges.Count == 1 => Contour is a circle
                {
                    var edge = Edges[i];
                    var nextEdge = i + 1 < Edges.Count ? Edges[i + 1] : null;

                    if (edge.Type == "line")
                    {
                        outPts.Add(edge.Points[1]);
                    }
                    else if(edge.Type == "arc")
                    {
                        if (edge.Points.Count == 3)
                        {
                            // Add middle and endpoint for 3-point arc.
                            outPts.Add(edge.Points[1]);
                            outPts.Add(edge.Points[2]);
                        }
                        else if (edge.Points.Count == 1)
                        {
                            if(outPts.Count > 1)   // Add the single point for the arc if it's not the first point.
                            {
                                outPts.Add( edge.Points[0]);
                            }

                            if (nextEdge != null && nextEdge?.Points.Count > 1)     // If there's a next edge, add its starting point unless it's another single-point arc.
                            {
                                outPts.Add(nextEdge.Points[0]);
                            }
                        }
                    }
                }

                return outPts;
            }
        }

        /// <summary>
        /// Collect only the corner points. Centre points of arcs or circles are ignored.
        /// </summary>
        [XmlIgnore]
        public List<Point3d> CornerPoints
        {
            get
            {
                List<Point3d> outPts = new List<Point3d>();
                foreach (Edge edge in this.Edges)    // if Edges.Count == 1 => Contour is a circle
                {
                    if (edge.Type == "line")
                    {
                        if (outPts.Count == 0)
                        {
                            outPts.Add(edge.Points[0]);
                        }

                        outPts.Add(edge.Points[1]);
                    }
                    else if (edge.Type == "arc" && edge.Points.Count == 3)
                    {
                        if (outPts.Count == 0)
                        {
                            outPts.Add(edge.Points[0]);
                        }

                        outPts.Add(edge.Points[2]);
                    }
                }

                return outPts;
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal Contour()
        {

        }

        /// <summary>
        /// Construct Contour from Edges.
        /// Edges should form a closed contour.
        /// </summary>
        public Contour(List<Edge> edges)
        {
            this.Edges = edges;
        }

        /// <summary>
        /// Construct a closed contour from points. 
        /// </summary>
        /// <param name="points"></param>
        public Contour(List<Point3d> points)
        {
            List<Edge> edges = new List<Edge>();

            for (int i = 0; i < points.Count; i++)
            {
                if (i < points.Count - 1)
                {
                    edges.Add(new Edge(points[i], points[i + 1]));
                }
                else
                {
                    edges.Add(new Edge(points[i], points[0]));
                }
            }

            this.Edges = edges;
        }

        /// <summary>
        /// Reverse direction of edges in this contour
        /// </summary>
        public void Reverse()
        {
            // reverse every edge
            foreach (Edge edge in this.Edges)
            {
                edge.Reverse();
            }

            // reverse list of edges
            this.Edges.Reverse();
        }

        public static implicit operator StruSoft.Interop.StruXml.Data.Contour_type(Geometry.Contour obj)
        {
            var contour = new StruSoft.Interop.StruXml.Data.Contour_type();
            var edges = new List<StruSoft.Interop.StruXml.Data.Edge_type>();

            foreach (var edge in obj.Edges)
                edges.Add(edge);

            contour.Edge = edges;

            return contour;
        }

    }
}