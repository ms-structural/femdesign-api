// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    /// <summary>
    /// Represents a Region.
    /// 
    /// Surfaces in FEM-Design are expressed as regions of contours (outlines). This extended region also contains a LCS to keep track of directions.
    /// </summary>
    [System.Serializable]
    public partial class Region
    {
        /// <summary>
        /// Gets or sets the plane.
        /// </summary>
        [XmlIgnore]
        public Plane Plane { get; set; }

        /// <summary>
        /// Used for panels and sections
        /// </summary>
        [XmlIgnore]
        public Vector3d LocalZ
        {
            get
            {
                return this.Contours[0].LocalZ;
            }
            
            set
            {
                int par = value.IsParallel(this.LocalZ);
                if (par == 1)
                {
                    // pass
                }
                
                else if (par == -1)
                {
                    this.Reverse();
                }

                else
                {
                    Vector3d v = this.LocalZ;
                    throw new System.ArgumentException($"Value: ({value.X}, {value.Y}, {value.Z}) is not parallell to LocalZ ({v.X}, {v.Y}, {v.Z}) ");
                }
            }
        }

        [XmlIgnore]
        public List<Contour> Contours
        {
            get
            {
                if (this._contours == null)
                {
                    var contours = new List<Contour>();
                    return contours;
                }
                return this._contours;
            }
            set { this._contours = value; }
        }

        /// <summary>
        /// Gets or sets the contours.
        /// </summary>
        [XmlElement("contour")]
        public List<Contour> _contours { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal Region()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Region"/> class.
        /// </summary>
        /// <param name="contours">the contours.</param>
        public Region(List<Contour> contours)
        {
            this.Contours = contours;
            this.Plane = contours[0].Edges[0].Plane;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Region"/> class.
        /// </summary>
        /// <param name="contours">the contours.</param>
        /// <param name="plane">the plane.</param>
        public Region(List<Contour> contours, Plane plane)
        {
            this.Contours = contours;
            this.Plane = plane;
        }

        /// <summary>
        /// Construct a region with holes from an outer contour and inner contours.
        /// The direction of the holes contours are assumed to be opposite to the outer.
        /// </summary>
        /// <param name="outerContour"></param>
        /// <param name="holesContours"></param>
        public Region(Contour outerContour, List<Contour> holesContours)
        {
            this.Contours = new List<Contour> { outerContour };
            this.Contours.AddRange(holesContours);
            this.Plane = outerContour.Edges[0].Plane;
        }

        public bool IsPlanar
        {
            get
            {
                var points = new List<Geometry.Point3d>();

                foreach(var contour in this.Contours)
                {
                    foreach(var edge in contour.Edges)
                    {
                        points.AddRange(edge.Points);
                    }
                }

                bool isPlanar = Point3d.ArePointsOnPlane(points);
                return isPlanar;
            }
        }

        /// <summary>
        /// Create region by points and coordinate system.
        /// </summary>
        /// <param name="points">List of sorted points defining the outer perimeter of the region.</param>
        /// <param name="plane">Coordinate system of the region</param>
        public Region(List<Point3d> points, Plane plane)
        {
            // edge normal
            Vector3d edgeLocalY = plane.LocalZ;

            List<Edge> edges = new List<Edge>();
            for (int idx = 0 ; idx < points.Count; idx++)
            {
                // startPoint
                Point3d p0 = p0 = points[idx];

                // endPoint
                Point3d p1;
                if (idx != points.Count - 1)
                {
                    p1 = points[idx + 1];
                }

                else
                {
                    p1 = points[0];
                }

                // create edge
                edges.Add(new Edge(p0, p1, edgeLocalY));
            }
            
            // create contours
            Contour contour = new Contour(edges);

            // set properties
            this.Contours = new List<Contour>{contour};
            this.Plane = plane;
        }

        /// <summary>
        /// Rectangle XZ.
        /// </summary>
        /// <param name="corner">the corner.</param>
        /// <param name="widthX">the width x.</param>
        /// <param name="heightZ">the height z.</param>
        /// <returns>The result.</returns>
        public static Region RectangleXZ(Point3d corner, double widthX, double heightZ)
        {
            var points0 = corner + new Vector3d(0, 0, 0);
            var points1 = corner + new Vector3d(widthX, 0, 0);
            var points2 = corner + new Vector3d(widthX, 0, heightZ);
            var points3 = corner + new Vector3d(0, 0, heightZ);

            var points = new List<Point3d>() { points0, points1, points2, points3 };

            var plane = new Plane(points0, points1, points3);

            // set properties
            var region = new Region(points, plane);

            return region;
        }
        /// <summary>
        /// Rectangle XZ.
        /// </summary>
        /// <param name="width">the width.</param>
        /// <param name="height">the height.</param>
        /// <returns>The result.</returns>
        public static Region RectangleXZ(double width, double height)
        {
            var points0 = new Point3d(0,0,0);
            var points1 = new Point3d(width,0,0);
            var points2 = new Point3d(width,0,height);
            var points3 = new Point3d(0,0,height);

            var points = new List<Point3d>() { points0, points1, points2, points3 };

            var fdCoordinate = new Plane(points0, points1, points3);

            // set properties
            var region = new Region(points, fdCoordinate);

            return region;
        }

        /// <summary>
        /// Rectangle XY.
        /// </summary>
        /// <param name="corner">the corner.</param>
        /// <param name="widthX">the width x.</param>
        /// <param name="widthY">the width y.</param>
        /// <returns>The result.</returns>
        public static Region RectangleXY(Point3d corner, double widthX, double widthY)
        {
            var points0 = corner + new Vector3d(0, 0, 0);
            var points1 = corner + new Vector3d(widthX, 0, 0);
            var points2 = corner + new Vector3d(widthX, widthY, 0);
            var points3 = corner + new Vector3d(0, widthY, 0);

            var points = new List<Point3d>() { points0, points1, points2, points3 };

            var plane = new Plane(points0, points1, points3);

            // set properties
            var region = new Region(points, plane);

            return region;
        }
        /// <summary>
        /// Rectangle XY.
        /// </summary>
        /// <param name="widthX">the width x.</param>
        /// <param name="widthY">the width y.</param>
        /// <returns>The result.</returns>
        public static Region RectangleXY(double widthX, double widthY)
        {
            var points0 = new Point3d(0, 0, 0);
            var points1 = new Point3d(widthX, 0, 0);
            var points2 = new Point3d(widthX, widthY, 0);
            var points3 = new Point3d(0, widthY, 0);

            var points = new List<Point3d>() { points0, points1, points2, points3 };

            var plane = new Plane(points0, points1, points3);

            // set properties
            var region = new Region(points, plane);

            return region;
        }

        /// <summary>
        /// Rectangle YZ.
        /// </summary>
        /// <param name="corner">the corner.</param>
        /// <param name="widthY">the width y.</param>
        /// <param name="heightZ">the height z.</param>
        /// <returns>The result.</returns>
        public static Region RectangleYZ(Point3d corner, double widthY, double heightZ)
        {
            var points0 = corner + new Vector3d(0, 0, 0);
            var points1 = corner + new Vector3d(0, widthY, 0);
            var points2 = corner + new Vector3d(0, widthY, heightZ);
            var points3 = corner + new Vector3d(0, 0, heightZ);

            var points = new List<Point3d>() { points0, points1, points2, points3 };

            var plane = new Plane(points0, points1, points3);

            // set properties
            var region = new Region(points, plane);

            return region;
        }
        /// <summary>
        /// Rectangle YZ.
        /// </summary>
        /// <param name="widthY">the width y.</param>
        /// <param name="heightZ">the height z.</param>
        /// <returns>The result.</returns>
        public static Region RectangleYZ(double widthY, double heightZ)
        {
            var points0 = new Point3d(0, 0, 0);
            var points1 = new Point3d(0, widthY, 0);
            var points2 = new Point3d(0, widthY, heightZ);
            var points3 = new Point3d(0, 0, heightZ);

            var points = new List<Point3d>() { points0, points1, points2, points3 };

            var plane = new Plane(points0, points1, points3);

            // set properties
            var region = new Region(points, plane);

            return region;
        }

        /// <summary>
        /// Reverse the contours in this region
        /// </summary>
        public void Reverse()
        {
            foreach (Contour contour in this.Contours)
            {
                contour.Reverse();
            }
        }
        
        /// <summary>
        /// Get region from a Slab.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="slab">the slab.</param>
        public static Region FromSlab(Shells.Slab slab)
        {
            return slab.SlabPart.Region;
        }

        /// <summary>
        /// Set EdgeConnection on Edge in region by index.
        /// </summary>
        /// <param name="edgeConnection">the edge connection.</param>
        /// <param name="index">the index.</param>
        public void SetEdgeConnection(Shells.EdgeConnection edgeConnection, int index)
        {
            int edgeIdx = 0;
            foreach (Contour contour in this.Contours)
            {
                if (contour.Edges != null)
                {
                    int cInstance = 0;
                    foreach (Edge edge in contour.Edges)
                    {
                        cInstance++;
                        if (index == edgeIdx)
                        {
                            if (!edgeConnection.Release)
                            {
                                edge.EdgeConnection = null;
                            }
                            else
                            {
                                string name = edgeConnection.Name == null ? "CE." + cInstance.ToString() : edgeConnection.Name;

                                Shells.EdgeConnection ec = Shells.EdgeConnection.CopyExisting(edgeConnection, name);
                                edge.EdgeConnection = ec;
                                edge.EdgeConnection.Normal = this.LocalZ;
                            }
                            return;
                        }
                        edgeIdx++;
                    }
                }
                else
                {
                    throw new System.ArgumentException("No edges in contour!");
                }
            }

            // edge not found
            throw new System.ArgumentException("Edge not found.");
        }

        /// <summary>
        /// Set EdgeConnection on all Edges in Region.
        /// </summary>
        /// <param name="edgeConnection">the edge connection.</param>
        public void SetEdgeConnections(Shells.EdgeConnection edgeConnection)
        {
            if (edgeConnection.Release)
            {
                foreach (Contour contour in this.Contours)
                {
                    if (contour.Edges != null)
                    {
                        int cInstance = 0;
                        foreach (Edge edge in contour.Edges)
                        {
                            cInstance++;
                            string name = "CE." + cInstance.ToString();
                            Shells.EdgeConnection ec = Shells.EdgeConnection.CopyExisting(edgeConnection, name);
                            edge.EdgeConnection = ec;
                            edge.EdgeConnection.Normal = this.LocalZ;
                        }
                    }
                    else
                    {
                        throw new System.ArgumentException("No edges in contour!");
                    }
                }
            }
            else
            {
                // don't modify edges if no releases on edgeConnection.
            }
        }

        /// <summary>
        /// Sets the edge connections.
        /// </summary>
        /// <param name="edgeConnections">the edge connections.</param>
        public void SetEdgeConnections(List<Shells.EdgeConnection> edgeConnections)
        {
            if (edgeConnections?.Count == 0)
            {
                // pass; keep edges as before
            }
            else if (edgeConnections.Count == 1)
            {
                this.SetEdgeConnections(edgeConnections[0]);
            }
            else if (edgeConnections.Count == this.GetEdgeConnections().Count)
            {
                for (int i = 0; i < edgeConnections.Count; i++)
                    this.SetEdgeConnection(edgeConnections[i], i);
            }
            else
            {
                throw new ArgumentException($"The number of edge connections must be 1 or equal to the number of surface edges!");
            }
        }

        /// <summary>
        /// Get all EdgeConnection from all Edges in Region.
        /// </summary>
        /// <returns>The result.</returns>
        public List<Shells.EdgeConnection> GetEdgeConnections()
        {
            var edgeConnections = new List<Shells.EdgeConnection>();
            foreach (Contour contour in this.Contours)
            {
                foreach (Edge edge in contour.Edges)
                {
                    var edgeConnection = edge.EdgeConnection;
                    if(edgeConnection != null)
                    {
                        edgeConnection.Edge = edge;
                        edgeConnection.Normal = this.LocalZ;
                    }
                    edgeConnections.Add(edgeConnection);
                }
            }
            return edgeConnections;
        }

        /// <summary>
        /// Get all PredefinedRigidities from all Edges in Region
        /// </summary>
        /// <returns></returns>
        public List<Releases.RigidityDataLibType3> GetPredefinedRigidities()
        {
            List<Releases.RigidityDataLibType3> predefRigidities = new List<Releases.RigidityDataLibType3>();
            foreach (Contour contour in this.Contours)
            {
                foreach(Edge edge in contour.Edges)
                {
                    if (edge.EdgeConnection != null)
                    {
                        if (edge.EdgeConnection.PredefRigidity != null)
                        {
                            predefRigidities.Add(edge.EdgeConnection.PredefRigidity);
                        }
                    }
                    
                }
            }
            return predefRigidities;
        }

        /// <summary>
        /// Set line connection types (i.e predefined line connection type) on region edges
        /// </summary>
        /// <param name="predefinedTypes">the predefined types.</param>
        public void SetPredefinedRigidities(List<Releases.RigidityDataLibType3> predefinedTypes)
        {
            foreach (Geometry.Contour contour in this.Contours)
            {
                foreach (Geometry.Edge edge in contour.Edges)
                {
                    if (edge.EdgeConnection != null)
                    {
                        if (edge.EdgeConnection._predefRigidityRef != null)
                        {
                            foreach (Releases.RigidityDataLibType3 predefinedType in predefinedTypes)
                            {
                                // add predefined type to edge connection if edge connection predef rigidity reference matches guid of predefine type
                                if (edge.EdgeConnection._predefRigidityRef.Guid == predefinedType.Guid)
                                {
                                    edge.EdgeConnection.PredefRigidity = predefinedType;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns a new instance of region, without any EdgeConnections.
        /// </summary>
        /// <returns></returns>
        public Region RemoveEdgeConnections()
        {
            Region newRegion = this.DeepClone();
            foreach (Contour newContour in newRegion.Contours)
            {
                foreach (Edge newEdge in newContour.Edges)
                {
                    if (newEdge.EdgeConnection != null)
                    {
                        newEdge.EdgeConnection = null;
                    }
                }
            }
            return newRegion;
        }


        /// <summary>
        /// Area.
        /// </summary>
        /// <param name="points">the points.</param>
        /// <returns>The result.</returns>
        public static double Area(List<Point3d> points)
        {
            var area = System.Math.Abs(points.Take(points.Count - 1).Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y)).Sum() / 2);
            return area;
        }

        /// <summary>
        /// Defines an operator overload.
        /// </summary>
        /// <param name="obj">the obj.</param>
        /// <returns>The result.</returns>
        public static implicit operator Region(StruSoft.Interop.StruXml.Data.Region_type obj)
        {
            var edges = obj.Contour[0].Edge.Select( x => (Edge)x).ToList();
            var contour = new Contour(edges);
            var contours = new List<Contour>() { contour };

            var region = new Region(contours);

            return region;
        }

        public static implicit operator StruSoft.Interop.StruXml.Data.Region_type(Region obj)
        {
            var region = new StruSoft.Interop.StruXml.Data.Region_type();
            var contours = new List<StruSoft.Interop.StruXml.Data.Contour_type>();

            foreach(var contour in obj.Contours)
                contours.Add(contour);

            region.Contour  = contours;

            return region;
        }
    }
}