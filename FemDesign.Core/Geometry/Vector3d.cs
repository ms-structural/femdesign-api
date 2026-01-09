// https://strusoft.com/

using System;
using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    /// <summary>
    /// Represents a Vector3d.
    /// </summary>
    [System.Serializable]
    public partial class Vector3d
    {
        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        [XmlAttribute("x")]
        public double X { get; set; }
        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        [XmlAttribute("y")]
        public double Y { get; set; }
        /// <summary>
        /// Gets or sets the z.
        /// </summary>
        [XmlAttribute("z")]
        public double Z { get; set; }

        /// <summary>
        /// Gets or sets the unit x.
        /// </summary>
        public static Vector3d UnitX => new Vector3d(1, 0, 0);
        /// <summary>
        /// Gets or sets the unit y.
        /// </summary>
        public static Vector3d UnitY => new Vector3d(0, 1, 0);
        /// <summary>
        /// Gets or sets the unit z.
        /// </summary>
        public static Vector3d UnitZ => new Vector3d(0, 0, 1);
        /// <summary>
        /// Gets or sets the zero length.
        /// </summary>
        public static Vector3d ZeroLength => new Vector3d(0, 0, 0);


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Vector3d()
        {
            
        }

        /// <summary>
        /// Construct FdVector3d by x, y and z components.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Construct FdVector3d by start and end points.
        /// </summary>
        /// <param name="p0">Start point</param>
        /// <param name="p1">End point</param>
        public Vector3d(Point3d p0, Point3d p1)
        {
            this.X = p1.X - p0.X;
            this.Y = p1.Y - p0.Y;
            this.Z = p1.Z - p0.Z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3d"/> class.
        /// </summary>
        /// <param name="edge">the edge.</param>
        public Vector3d(LineEdge edge)
        {
            this.X = (edge.EndPoint - edge.StartPoint).X;
            this.Y = (edge.EndPoint - edge.StartPoint).Y;
            this.Z = (edge.EndPoint - edge.StartPoint).Z;
        }

        /// <summary>
        /// Vector addition
        /// </summary>
        public static Vector3d operator +(Vector3d v1, Vector3d v2) => new Vector3d(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);

        /// <summary>
        /// Reverse vector
        /// </summary>
        public static Vector3d operator -(Vector3d v) => v.Reverse();
        
        /// <summary>
        /// Vector difference
        /// </summary>
        public static Vector3d operator -(Vector3d v1, Vector3d v2) => new Vector3d(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);

        /// <summary>
        /// Scale vector
        /// </summary>
        public static Vector3d operator *(Vector3d v, int n) => new Vector3d(n * v.X, n * v.Y, n * v.Z);

        /// <summary>
        /// Scale vector
        /// </summary>
        public static Vector3d operator *(int n, Vector3d v) => new Vector3d(n * v.X, n * v.Y, n * v.Z);

        /// <summary>
        /// Scale vector
        /// </summary>
        public static Vector3d operator *(Vector3d v, double n) => new Vector3d(n * v.X, n * v.Y, n * v.Z);

        /// <summary>
        /// Scale vector
        /// </summary>
        public static Vector3d operator *(double n, Vector3d v) => new Vector3d(n * v.X, n * v.Y, n * v.Z);

        /// <summary>
        /// Implicitly convert Point to Vector
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The result.</returns>
        public static implicit operator Vector3d(Point3d point)
		{
            var x = point.X;
            var y = point.Y;
            var z = point.Z;
            return new Vector3d(x, y, z); ;
        }

        /// <summary>
        /// Implicitly convert LineEedge to Vector
        /// </summary>
        /// <param name="edge"></param>
        /// <returns>The result.</returns>
        public static implicit operator Vector3d(LineEdge edge)
		{
            return edge.EndPoint - edge.StartPoint;
		}

        /// <summary>
        /// Calculate length of FdVector3d.
        /// </summary>
        /// <returns>The result.</returns>
        public double Length()
        {
            double len = Math.Pow((Math.Pow(this.X, 2) + Math.Pow(this.Y, 2) + Math.Pow(this.Z, 2)), (1.0/2.0));
            return len;
        }

        /// <summary>
        /// Normalize FdVector3d (i.e. scale so that length equals 1).
        /// </summary>
        /// <returns>The result.</returns>
        public Vector3d Normalize()
        {
            double l = this.Length();
            Vector3d normal = new Vector3d(this.X / l, this.Y / l, this.Z / l);
            return normal;
        }

        /// <summary>
        /// Calculate cross-product of this FdVector3d and v FdVector3d.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="v">value for <paramref name="v"/>.</param>
        public Vector3d Cross(Vector3d v)
        {
            Vector3d v0 = this;
            Vector3d v1 = v;

            double i, j, k;
            // https://en.wikipedia.org/wiki/Cross_product#Coordinate_notation
            i = v0.Y * v1.Z - v0.Z * v1.Y;
            j = v0.Z * v1.X - v0.X * v1.Z;
            k = v0.X * v1.Y - v0.Y * v1.X;

            Vector3d v2 = new Vector3d(i, j, k);;
            return v2;
        }

        /// <summary>
        /// Calculate dot-product of this FdVector3d and v FdVector3d.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="v">value for <paramref name="v"/>.</param>
        public double Dot(Vector3d v)
        {
            Vector3d v0 = this;
            Vector3d v1 = v;

            // https://en.wikipedia.org/wiki/Dot_product#Algebraic_definition
            double s = v0.X*v1.X + v0.Y*v1.Y + v0.Z*v1.Z;
            return s;

        }

        /// <summary>
        /// Scale this FdVector3d by s.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="s">value for <paramref name="s"/>.</param>
        public Vector3d Scale(double s)
        {   
            return new Vector3d(this.X * s, this.Y * s, this.Z * s);
        }

        /// <summary>
        /// Reverse this by negative scaling.
        /// </summary>
        /// <returns></returns>
        public Vector3d Reverse()
        {
            return this.Scale(-1);
        }

        /// <summary>
        /// Rotate this vector by an angle around an axis
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public Vector3d RotateAroundAxis(double angle, Vector3d axis)
        {
            // normalize vector
            Vector3d _axis = axis.Normalize();

            // https://en.wikipedia.org/wiki/Rotation_matrix#Rotation_matrix_from_axis_and_angle
            double[,] rotationMatrix = new double[3,3];
            rotationMatrix[0,0] = Math.Cos(angle) + Math.Pow(_axis.X, 2)*(1 - Math.Cos(angle));
            rotationMatrix[0,1] = _axis.X*_axis.Y*(1 - Math.Cos(angle)) - _axis.Z*Math.Sin(angle);
            rotationMatrix[0,2] = _axis.X*_axis.Z*(1 - Math.Cos(angle)) + _axis.Y*Math.Sin(angle);
            rotationMatrix[1,0] = _axis.Y*_axis.X*(1 - Math.Cos(angle)) + _axis.Z*Math.Sin(angle);
            rotationMatrix[1,1] = Math.Cos(angle) + Math.Pow(_axis.Y, 2)*(1 - Math.Cos(angle));
            rotationMatrix[1,2] = _axis.Y*_axis.Z*(1 - Math.Cos(angle)) - _axis.X*Math.Sin(angle);
            rotationMatrix[2,0] = _axis.Z*_axis.X*(1 - Math.Cos(angle)) - _axis.Y*Math.Sin(angle);
            rotationMatrix[2,1] = _axis.Z*_axis.Y*(1 - Math.Cos(angle)) + _axis.X*Math.Sin(angle);
            rotationMatrix[2,2] = Math.Cos(angle) + Math.Pow(_axis.Z, 2)*(1 - Math.Cos(angle));

            // matrix multiplication
            double x = this.X*rotationMatrix[0,0] + this.Y*rotationMatrix[0,1] + this.Z*rotationMatrix[0,2];
            double y = this.X*rotationMatrix[1,0] + this.Y*rotationMatrix[1,1] + this.Z*rotationMatrix[1,2];
            double z = this.X*rotationMatrix[2,0] + this.Y*rotationMatrix[2,1] + this.Z*rotationMatrix[2,2];

            // return new vector
            return new Vector3d(x, y, z);
        }

        /// <summary>
        /// Check if this FdVector3d is parallel to v.
        /// Returns 1 if parallel, -1 if antiparallel, 0 if not parallel.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="v">value for <paramref name="v"/>.</param>
        public int IsParallel(Vector3d v)
        {
            Vector3d v0 = this.Normalize();
            Vector3d v1 = v.Normalize();
            if (v0.Equals(v1, Tolerance.Point3d))
            {
                return 1;
            }
            else if (v0.Scale(-1).Equals(v, Tolerance.Point3d))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Check if this FdVector3d is perpendicular to v.
        /// Returns true if perpendicular, false if not perpendicular.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="v">value for <paramref name="v"/>.</param>
        public bool IsPerpendicular(Vector3d v)
        {
            Vector3d v1 = this.Normalize();
            Vector3d v2 = v.Normalize();
            Vector3d v3 = v1.Cross(v2);
            return (Math.Abs(1 - v3.Length()) <= Tolerance.Vector3d);
        }

        /// <summary>
        /// Check if zero vector.
        /// </summary>
        /// <returns></returns>
        public bool IsZero()
        {
            if (this.Length() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            Vector3d v = obj as Vector3d;
            if ((System.Object)v == null)
            {
                return false;
            }
            return (X == v.X) && (Y == v.Y) && (Z == v.Z);            
        }

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="v">value for <paramref name="v"/>.</param>
        /// <returns>The result.</returns>
        public bool Equals(Vector3d v)
        {
            if ((object)v == null)
            {
                return false;
            }
            return (X == v.X) && (Y == v.Y) && (Z == v.Z);
        }

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="v">value for <paramref name="v"/>.</param>
        /// <param name="tol">the tol.</param>
        /// <returns>The result.</returns>
        public bool Equals(Vector3d v, double tol)
        {
            if ((object)v == null)
            {
                return false;
            }
            return (Math.Abs(X - v.X) < tol) && (Math.Abs(Y - v.Y) < tol) && (Math.Abs(Z - v.Z) < tol);
        }

        /// <summary>
        /// Check if z-component is 0 and convert to 2d vector in XY-plane.
        /// </summary>
        /// <returns></returns>
        public Vector2d To2d()
        {
            if (this.Z == 0)
            {
                return new Vector2d(this.X, this.Y);
            }
            else
            {
                throw new System.ArgumentException("Z-component of Vector is not zero. Vector is not in XY plane.");
            }

        }

        public static implicit operator StruSoft.Interop.StruXml.Data.Point_type_3d(Vector3d v) => new StruSoft.Interop.StruXml.Data.Point_type_3d{
            X = v.X,
            Y = v.Y,
            Z = v.Z
        };

        /// <summary>
        /// Defines an operator overload.
        /// </summary>
        /// <param name="p">value for <paramref name="p"/>.</param>
        /// <returns>The result.</returns>
        public static implicit operator Vector3d(StruSoft.Interop.StruXml.Data.Point_type_3d p) => new Vector3d
        {
            X = p.X,
            Y = p.Y,
            Z = p.Z
        };


        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The result.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

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