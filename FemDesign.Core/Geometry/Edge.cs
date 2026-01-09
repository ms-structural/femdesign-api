// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;

namespace FemDesign.Geometry
{
    /// <summary>
    /// Represents a Edge.
    /// 
    /// Curves in FEM-Design are expressed as edges. This extended edge also contains a LCS to keep track of directions.
    /// </summary>
    [XmlInclude(typeof(LineEdge))]
    [XmlInclude(typeof(ArcEdge))]
    [XmlInclude(typeof(CircleEdge))]
    [XmlRoot("database", Namespace = "urn:strusoft")]
    [System.Serializable]
    public partial class Edge
    {
        [XmlIgnore]
        private Plane _plane;
        /// <summary>
        /// Get/Set LCS
        /// 
        /// If no LCS exists on Edge (i.e. when an Edge was deserialized from path) an LCS will be reconstructed.
        /// </summary>
        [XmlIgnore]
        public Plane Plane
        {
            get
            {
                if (this._plane != null)
                {
                    return this._plane;
                }
                else
                {
                    Point3d origin;
                    Vector3d localX, localY, localZ;

                    // arc1
                    if (this.Type == "arc" && this.Points.Count == 1)
                    {
                        // sweep angle
                        double sweepAngle = this.EndAngle - this.StartAngle;

                        // find p0, p1 and p2
                        Point3d p0, p1, p2;
                        Vector3d v = this.XAxis.Scale(this.Radius);
                        p0 = this.Points[0].Translate(v);
                        p1 = this.Points[0].Translate(v.RotateAroundAxis(sweepAngle / 2, this.Normal));
                        p2 = this.Points[0].Translate(v.RotateAroundAxis(sweepAngle, this.Normal));

                        origin = p1;
                        localX = new Vector3d(p0, p2).Normalize();
                        localZ = this.Normal;
                        localY = localZ.Cross(localX);
                        return new Plane(origin, localX, localY);
                    }

                    // arc2
                    else if (this.Type == "arc" && this.Points.Count == 3)
                    {
                        origin = this.Points[1];
                        localX = new Vector3d(this.Points[0], this.Points[2]).Normalize();
                        Vector3d v = new Vector3d(this.Points[0], this.Points[1]).Normalize();
                        localZ = v.Cross(localX);
                        localY = localZ.Cross(localX);
                        return new Plane(origin, localX, localY);
                    }

                    // line
                    else if (this.Type == "line" && this.Points.Count == 2)
                    {
                        Vector3d v = new Vector3d(this.Points[0], this.Points[1]);
                        origin = this.Points[0].Translate(v.Scale(0.5));
                        localX = v.Normalize();
                        localY = this.Normal;
                        localZ = localX.Cross(localY);
                        return new Plane(origin, localX, localY);
                    }

                    // else
                    else
                    {
                        throw new System.ArgumentException($"Could not reconstruct FdCoordinateSystem from Edge of type: {this.Type}");
                    }
                }
            }
            set { this._plane = value; }
        }
        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        [XmlElement("point", Order = 1)]
        public List<Geometry.Point3d> Points = new List<Geometry.Point3d>(); // sequence: point_type_3d // ordered internal points, or the center of the circle/arc
        /// <summary>
        /// Gets or sets the normal.
        /// </summary>
        [XmlElement("normal", Order = 2)]
        public Vector3d Normal { get; set; } // point_type_3d // normal of the curve; it must be used if the curve is arc or circle.
        /// <summary>
        /// Gets or sets the x axis.
        /// </summary>
        [XmlElement("x_axis", Order = 3)]
        public Geometry.Vector3d XAxis { get; set; } // point_type_3d // axis of base line (the value default is the x axis {1, 0, 0}) angles are measured from this direction.
        /// <summary>
        /// Gets or sets the edge connection.
        /// </summary>
        [XmlElement("edge_connection", Order = 4)]
        public Shells.EdgeConnection EdgeConnection { get; set; } // optional. ec_type.
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute("type")]
        public string _type; // edgetype
        [XmlIgnore]
        public string Type
        {
            get { return this._type; }
            set { this._type = RestrictedString.EdgeType(value); }
        }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        [XmlAttribute("radius")]
        public double _radius;

        [XmlIgnore]
        public double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        /// <summary>
        /// Should Serialize radius.
        /// </summary>
        /// <returns>The result.</returns>
        public bool ShouldSerialize_radius()
        {
            return _radius > 0.0;
        }


        /// <summary>
        /// Gets or sets the start angle.
        /// </summary>
        [XmlAttribute("start_angle")]
        public double _startAngle;

        [XmlIgnore]
        public double StartAngle
        {
            get { return _startAngle; }
            set { _startAngle = value; }
        }

        /// <summary>
        /// Gets or sets the end angle.
        /// </summary>
        [XmlAttribute("end_angle")]
        public double _endAngle;

        [XmlIgnore]
        public double EndAngle
        {
            get { return _endAngle; }
            set { _endAngle = value; }
        }


        [XmlIgnore]
        public double Length
        {
            get
            {
                if (this.Type == "line")
                {
                    return (new Vector3d(this.Points[0], this.Points[1]).Length());
                }
                else if (this.Type == "arc")
                {
                    if (this.Points.Count == 3)
                    {
                        // https://github.com/strusoft/femdesign-api/issues/123#issuecomment-1205322334
                        var v1 = Points[0] - Points[1];
                        var v2 = Points[2] - Points[1];
                        var normal = v1.Cross(v2);
                        var m1 = Points[1] + 0.5 * v1;
                        var m2 = Points[1] + 0.5 * v2;
                        var N1 = normal.Cross(v1);
                        var N2 = normal.Cross(v2);

                        var (t1, t2) = Proximity.LineLineProximity(m1, N1, m2, N2);

                        var centre = m1 + (t1 * N1);

                        var radius = (Points[0] - centre).Length();
                        var r1 = (Points[0] - centre).Normalize();
                        var r2 = (Points[2] - centre).Normalize();
                        var angle = Math.Acos(r1.Dot(r2));
                        return angle * radius;
                    }
                    else
                    {
                        return (this.EndAngle - this.StartAngle) * this.Radius;
                    }
                }
                else if (this.Type == "circle")
                {
                    return 2 * Math.PI * this.Radius;
                }
                else
                {
                    throw new System.ArgumentException($"Edge type {this.Type} is not supported for length evaluation.");
                }
            }
        }
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal Edge()
        {

        }

        /// <summary>
        /// Construct Edge of arc1 type.
        /// </summary>
        /// <param name="radius">the radius.</param>
        /// <param name="startAngle">the start angle.</param>
        /// <param name="endAngle">the end angle.</param>
        /// <param name="centerPoint">the center point.</param>
        /// <param name="xAxis">value for <paramref name="xAxis"/>.</param>
        /// <param name="coordinateSystem">the coordinate system.</param>
        public Edge(double radius, double startAngle, double endAngle, Geometry.Point3d centerPoint, Geometry.Vector3d xAxis, Geometry.Plane coordinateSystem)
        {
            this.Type = "arc";
            this.Radius = radius;
            this.StartAngle = startAngle;
            this.EndAngle = endAngle;
            this.Points.Add(centerPoint);
            this.Normal = coordinateSystem.LocalZ;
            this.XAxis = xAxis;
            this.Plane = coordinateSystem;
        }

        /// <summary>
        /// Construct Edge of arc2 type.
        /// </summary>
        /// <param name="start">the start.</param>
        /// <param name="mid">the mid.</param>
        /// <param name="end">the end.</param>
        /// <param name="plane">the plane.</param>
        public Edge(Geometry.Point3d start, Geometry.Point3d mid, Geometry.Point3d end, Geometry.Plane plane)
        {
            this.Type = "arc";
            this.Points.Add(start);
            this.Points.Add(mid);
            this.Points.Add(end);
            this.Plane = plane;
        }

        /// <summary>
        /// Construct Edge of circle type.
        /// </summary>
        /// <param name="radius">the radius.</param>
        /// <param name="center">the center.</param>
        /// <param name="plane">the plane.</param>
        public Edge(double radius, Geometry.Point3d center, Geometry.Plane plane)
        {
            this.Type = "circle";
            this.Radius = radius;
            this.Points.Add(center);
            this.Normal = plane.LocalZ;
            this.Plane = plane;
        }

        /// <summary>
        /// Construct Edge of line type by points and coordinate system.
        /// </summary>
        /// <param name="start">the start.</param>
        /// <param name="end">the end.</param>
        /// <param name="plane">the plane.</param>
        public Edge(Geometry.Point3d start, Geometry.Point3d end, Geometry.Plane plane)
        {
            this.Type = "line";
            this.Points.Add(start);
            this.Points.Add(end);
            this.Normal = plane.LocalY;
            this.XAxis = plane.LocalX;
            this.Plane = plane;
        }

        /// <summary>
        /// Construct Edge of line type by points and normal (localY).
        /// </summary>
        /// <param name="startPoint">the start point.</param>
        /// <param name="endPoint">the end point.</param>
        /// <param name="localY">the local y.</param>
        public Edge(Point3d startPoint, Point3d endPoint, Vector3d localY = null)
        {
            this.Type = "line";
            this.Points.Add(startPoint);
            this.Points.Add(endPoint);
			if (!this.IsLineVertical())
			{
                this.Normal = localY ?? -(endPoint - startPoint).Cross(Vector3d.UnitZ);
			}
			else
			{
                this.Normal = localY ?? Vector3d.UnitX;
            }
            this.XAxis = this.Plane.LocalX;
        }

        /// <summary>
        /// Check if line
        /// </summary>
        /// <returns>The result.</returns>
        public bool IsLine()
        {
            return (this.Type == "line");
        }

        /// <summary>
        /// Check if line is vertical (i.e. parallel to global Z)
        /// </summary>
        /// <returns>The result.</returns>
        public bool IsLineVertical()
        {
            if (this.IsLine())
            {
                return (this.Points[0].X == this.Points[1].X && this.Points[0].Y == this.Points[1].Y);
            }
            else
            {
                throw new System.ArgumentException($"Edge type: {this.Type}, is not line.");
            }
        }

        /// <summary>
        /// Check if line local x is equal to positive global Z
        /// </summary>
        /// <returns>The result.</returns>
        public bool IsLineTangentEqualToGlobalZ()
        {
            if (this.IsLine())
            {
                return (this.Plane.LocalX.Equals(Geometry.Vector3d.UnitZ, Tolerance.Vector3d));
            }
            else
            {
                throw new System.ArgumentException($"Edge type: {this.Type}, is not line.");
            }

        }

        /// <summary>
        /// Reverse this edge
        /// </summary>
        public void Reverse()
        {
            // reset coordinate system
            this.Plane = null;

            if (this.Type == "line" && this.Points.Count == 2)
            {
                this.Points.Reverse();
            }

            else if (this.Type == "arc" && this.Points.Count == 3)
            {
                this.Points.Reverse();
            }

            else if (this.Type == "arc" && this.Points.Count == 1)
            {
                // get sweep angle
                double sweepAngle = this.EndAngle - this.StartAngle;

                // set new properties
                this.XAxis = this.XAxis.RotateAroundAxis(sweepAngle, this.Normal);
                this.Normal = this.Normal.Reverse();
                this.StartAngle = 0;
                this.EndAngle = sweepAngle;
            }

            else if (this.Type == "circle")
            {
                this.Normal = this.Normal.Reverse();
            }
            else
            {
                throw new System.ArgumentException($"Could not reverse Edge of type: {this.Type}");
            }
        }

        /// <summary>
        /// Gets the intermediate point.
        /// </summary>
        /// <param name="percentage">the percentage.</param>
        /// <returns>The result.</returns>
        public Point3d GetIntermediatePoint(double percentage)
        {
            if (percentage < 0 || percentage > 1)
            {
                throw new ArgumentOutOfRangeException("Percentage must be between 0 and 1.");
            }

            double deltaX = Points[1].X - Points[0].X;
            double deltaY = Points[1].Y - Points[0].Y;
            double deltaZ = Points[1].Z - Points[0].Z;

            double newX = Points[0].X + (deltaX * percentage);
            double newY = Points[0].Y + (deltaY * percentage);
            double newZ = Points[0].Z + (deltaZ * percentage);


            return new Point3d(newX, newY, newZ);
        }

        /// <summary>
        /// Convert an Edge to a LineSegment
        /// </summary>
        /// <param name="edge"></param>
        /// <returns>The result.</returns>
        public static explicit operator LineSegment(Edge edge)
        {
            if (!edge.IsLine())
                throw new ArgumentException("Edge is not a line");

            return new LineSegment(edge.Points[0], edge.Points[1]);
        }

        /// <summary>
        /// Defines an operator overload.
        /// </summary>
        /// <param name="obj">the obj.</param>
        /// <returns>The result.</returns>
        public static implicit operator Edge(StruSoft.Interop.StruXml.Data.Edge_type obj)
        {
            var start = (Point3d)obj.Point[0];
            var end = (Point3d)obj.Point[1];
            var normal = (Vector3d)obj.Normal;
            var edge = new Edge(start, end, normal);
            
            return edge;
        }

        public static implicit operator StruSoft.Interop.StruXml.Data.Edge_type(Edge obj)
        {
            var edge = new StruSoft.Interop.StruXml.Data.Edge_type();

            edge.Point = new List<StruSoft.Interop.StruXml.Data.Point_type_3d> { obj.Points[0] , obj.Points[1] };
            edge.Normal = obj.Normal;

            return edge;
        }

    }
}