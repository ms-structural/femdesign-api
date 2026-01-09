// https://strusoft.com/

using FemDesign.Bars;
using FemDesign.GenericClasses;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    /// <summary>
    /// Represents a Point3d.
    /// </summary>
    [System.Serializable]
    public partial class Point3d
    {
        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        [XmlAttribute("x")]
        public double X;
        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        [XmlAttribute("y")]
        public double Y;
        /// <summary>
        /// Gets or sets the z.
        /// </summary>
        [XmlAttribute("z")]
        public double Z;

        /// <summary>
        /// Construct FdPoint3d in origin
        /// </summary>
        public static Point3d Origin => new Point3d(0, 0, 0);

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Point3d()
        {

        }

        /// <summary>
        /// Construct FdPoint3d from coordinates x, y and z.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Point3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Move a point p along vector v
        /// </summary>
        public static Point3d operator +(Point3d p, Vector3d v) => new Point3d(p.X + v.X, p.Y + v.Y, p.Z + v.Z);
        /// <summary>
        /// Move a point p along vector v
        /// </summary>
        public static Point3d operator +(Vector3d v, Point3d p) => new Point3d(p.X + v.X, p.Y + v.Y, p.Z + v.Z);
        /// <summary>
        /// Move a point p along vector -v
        /// </summary>
        public static Point3d operator -(Point3d p, Vector3d v) => new Point3d(p.X - v.X, p.Y - v.Y, p.Z - v.Z);
        /// <summary>
        /// Create vector from p2 to p1
        /// </summary>
        public static Vector3d operator -(Point3d p1, Point3d p2) => new Vector3d(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        public static Point3d operator +(Point3d p1, Point3d p2) => new Point3d(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        public static Point3d operator /(Point3d p1, double number) => new Point3d(p1.X / number, p1.Y / number, p1.Z / number);
        public static Point3d operator *(Point3d p1, double number) => new Point3d(p1.X * number, p1.Y * number, p1.Z * number);
        public static Point3d operator *(double number, Point3d p1) => new Point3d(p1.X * number, p1.Y * number, p1.Z * number);


        /// <summary>
        /// Translate a point by a vector.
        /// </summary>
        /// <param name="v">Vector.</param>
        /// <returns></returns>
        public Point3d Translate(Vector3d v)
        {
            return new Point3d(this.X + v.X, this.Y + v.Y, this.Z + v.Z);
        }

        /// <summary>
        /// Project point on XY-plane.
        /// </summary>
        /// <returns></returns>
        public Point2d To2d()
        {
            return new Point2d(this.X, this.Y);
        }

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="obj">the obj.</param>
        /// <returns>The result.</returns>
        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Point3d p = obj as Point3d;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (X == p.X) && (Y == p.Y) && (Z == p.Z);            
        }

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="p">value for <paramref name="p"/>.</param>
        /// <returns>The result.</returns>
        public bool Equals(Point3d p)
        {
            if ((object)p == null)
            {
                return false;
            }
            return (X == p.X) && (Y == p.Y) && (Z == p.Z);
        }

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="p">value for <paramref name="p"/>.</param>
        /// <param name="tolerance">the tolerance.</param>
        /// <returns>The result.</returns>
        public bool Equals(Point3d p, double tolerance)
        {
            if ((object)p == null)
            {
                return false;
            }
            return (Math.Abs(X - p.X) < tolerance) && (Math.Abs(Y - p.Y) < tolerance) && (Math.Abs(Z - p.Z) < tolerance);
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The result.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        /// <summary>
        /// Defines an operator overload.
        /// </summary>
        /// <param name="plane">the plane.</param>
        /// <returns>The result.</returns>
        public static implicit operator Point3d(Plane plane)
        {
            var x = plane.Origin.X;
            var y = plane.Origin.Y;
            var z = plane.Origin.Z;
            var point = new Point3d(x, y, z);
            return point;
        }

        /// <summary>
        /// Defines an operator overload.
        /// </summary>
        /// <param name="feaNode">the fea node.</param>
        /// <returns>The result.</returns>
        public static implicit operator Point3d(FemDesign.Results.FemNode feaNode)
        {
            var point = new Geometry.Point3d(feaNode.X, feaNode.Y, feaNode.Z);
            return point;
        }


        private static (double a, double b , double c,  double d) _getPlaneEquation(Point3d p1, Point3d p2, Point3d p3)
        {
            double a1 = p2.X - p1.X;
            double b1 = p2.Y - p1.Y;
            double c1 = p2.Z - p1.Z;

            double a2 = p3.X - p1.X;
            double b2 = p3.Y - p1.Y;
            double c2 = p3.Z - p1.Z;

            double a = b1 * c2 - b2 * c1;
            double b = a2 * c1 - a1 * c2;
            double c = a1 * b2 - b1 * a2;

            double d = (-a * p1.X - b * p1.Y - c * p1.Z);

            return (a, b, c, d);
        }


        /// <summary>
        /// Are Points Collinear.
        /// </summary>
        /// <param name="p1">the p1.</param>
        /// <param name="p2">the p2.</param>
        /// <param name="p3">the p3.</param>
        /// <returns>The result.</returns>
        public static bool ArePointsCollinear(Point3d p1, Point3d p2, Point3d p3)
        {
            double area = 0.5 * ((p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y));
            return (area == 0);
        }


        /// <summary>
        /// Are Points On Plane.
        /// </summary>
        /// <param name="points">the points.</param>
        /// <returns>The result.</returns>
        public static bool ArePointsOnPlane(List<Point3d> points)
        {
            (double a, double b, double c, double d) = _getPlaneEquation(points[0], points[1], points[points.Count-1]);

            for(int j = 0; j < points.Count; j++)
            {
                double IsOnPlane = a * points[j].X + b * points[j].Y + c * points[j].Z + d;

                // check is IsOnPlane is within tolerance
                if (Math.Abs(IsOnPlane) <= Tolerance.LengthComparison)
                    continue;
                else
                    return false;
            }

            return true;
        }


        /// <summary>
        /// Check if a point sit on top of a structural element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool OnStructuralElement(IStructureElement element)
        {
            IBar bar = element as IBar;
            if( bar != null )
            {
                var distance = DistancePointToLine(bar.Edge);

                if (distance <= Tolerance.Point3d)
                    return true;
            }
            else
            {
                IShell shell = element as IShell;
                if (shell != null)
                {
                    var points = new List<Geometry.Point3d>();

                    foreach (var contour in shell.Region.Contours)
                    {
                        foreach (var edge in contour.Edges)
                        {
                            points.AddRange(edge.Points);
                        }
                    }

                    points.Add(this);
                    var planar = ArePointsOnPlane(points);

                    return planar;
                }
                else
                {
                    throw new Exception("It's neither IBar nor IShell");
                }
            }

            return false;
        }

        // Calculate the distance between a point and a line defined by two points in 3D space
        /// <summary>
        /// Distance Point To Line.
        /// </summary>
        /// <param name="edge">the edge.</param>
        /// <returns>The result.</returns>
        public double DistancePointToLine(Edge edge)
        {
            var lineStart = edge.Points[0];
            var lineEnd = edge.Points[1];

            // Vector from the start point of the line to the point
            double lineToPointX = this.X - lineStart.X;
            double lineToPointY = this.Y - lineStart.Y;
            double lineToPointZ = this.Z - lineStart.Z;

            // Vector representing the direction of the line
            double lineDirectionX = lineEnd.X - lineStart.X;
            double lineDirectionY = lineEnd.Y - lineStart.Y;
            double lineDirectionZ = lineEnd.Z - lineStart.Z;

            // Calculate the dot product of lineToPoint and lineDirection
            double dotProduct = lineToPointX * lineDirectionX + lineToPointY * lineDirectionY + lineToPointZ * lineDirectionZ;

            // Calculate the length squared of the line
            double lineLengthSquared = lineDirectionX * lineDirectionX + lineDirectionY * lineDirectionY + lineDirectionZ * lineDirectionZ;

            // Calculate the parameter along the line where the closest point is
            double parameter = dotProduct / lineLengthSquared;

            // Calculate the coordinates of the closest point on the line
            double closestPointX = lineStart.X + parameter * lineDirectionX;
            double closestPointY = lineStart.Y + parameter * lineDirectionY;
            double closestPointZ = lineStart.Z + parameter * lineDirectionZ;

            // Calculate the distance between the point and the closest point on the line
            double distance = Math.Sqrt((this.X - closestPointX) * (this.X - closestPointX) +
                                        (this.Y - closestPointY) * (this.Y - closestPointY) +
                                        (this.Z - closestPointZ) * (this.Z - closestPointZ));

            return distance;
        }


        public static implicit operator StruSoft.Interop.StruXml.Data.Point_type_3d(Point3d p) => new StruSoft.Interop.StruXml.Data.Point_type_3d{
            X = p.X,
            Y = p.Y,
            Z = p.Z
        };

        /// <summary>
        /// Defines an operator overload.
        /// </summary>
        /// <param name="p">value for <paramref name="p"/>.</param>
        /// <returns>The result.</returns>
        public static implicit operator Point3d (StruSoft.Interop.StruXml.Data.Point_type_3d p) => new Point3d
        {
            X = p.X,
            Y = p.Y,
            Z = p.Z
        };


        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"({this.X.ToString(FemDesign.TextFormatting.decimalRounding)}, {this.Y.ToString(FemDesign.TextFormatting.decimalRounding)}, {this.Z.ToString(FemDesign.TextFormatting.decimalRounding)})";
        }
    }
}