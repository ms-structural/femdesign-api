using System;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    /// <summary>
    /// Represents a Plane.
    /// </summary>
    [System.Serializable]
    public class Plane
    {
        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        [XmlElement("local_pos", Order = 1)]
        public Point3d Origin { get; set; }

        /// <value>
        /// Do not set. Should be private.
        /// </value>
        [XmlElement("local_x", Order = 2)]
        public Vector3d _localX;
        public Vector3d LocalX
        {
            get
            {
                return _localX;
            }
        }

        /// <value>
        /// Do not set. Should be private.
        /// </value>
        [XmlElement("local_y", Order = 3)]
        public Vector3d _localY;
        public Vector3d LocalY
        {
            get
            {
                return _localY;
            }
        }

        /// <value>
        /// Normal vector of the plane with length 1
        /// </value>
        // rename: Normal
        public Vector3d LocalZ
        {
            get
            {
                return LocalX.Cross(LocalY).Normalize();
            }
        }

        /// <value>
        /// XY-plane
        /// </value>
        public static Plane XY
        {
            get
            {
                return new Plane(Point3d.Origin, Vector3d.UnitX, Vector3d.UnitY);
            }
        }

        
        /// <value>
        /// XZ-plane
        /// </value>
        public static Plane XZ
        {
            get
            {
                return new Plane(Point3d.Origin, Vector3d.UnitX, Vector3d.UnitZ);
            }
        }

        
        /// <value>
        /// YZ-plane
        /// </value>
        public static Plane YZ
        {
            get
            {
                return new Plane(Point3d.Origin, Vector3d.UnitY, Vector3d.UnitZ);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> class.
        /// </summary>
        public Plane()
        { }

        private void Initialise(Point3d origin, Vector3d xDir, Vector3d yDir)
        {
            Origin = origin;
            if (xDir.IsPerpendicular(yDir))
            {
                _localX = xDir.Normalize();
                _localY = yDir.Normalize();
            }
            else
            {
                throw new System.ArgumentException($"Can't construct plane. LocalX: {xDir} and LocalY: {yDir} are not perpendicular");
            }
        }

        /// <summary>
        /// Construct a plane by origin, x- and y-direction.
        /// </summary>
        /// <param name="origin">the origin.</param>
        /// <param name="xDir">value for <paramref name="xDir"/>.</param>
        /// <param name="yDir">value for <paramref name="yDir"/>.</param>
        public Plane(Point3d origin, Vector3d xDir, Vector3d yDir)
        {
            this.Initialise(origin, xDir, yDir);
        }

        /// <summary>
        /// Construct a plane by three points: origin, p1 and p2.
        /// </summary>
        /// <param name="origin">the origin.</param>
        /// <param name="p1">the p1.</param>
        /// <param name="p2">the p2.</param>
        public Plane(Point3d origin, Point3d p1, Point3d p2)
        {
            // find normal of plane
            Vector3d v1 = p1 - origin;
            Vector3d v2 = p2 - origin;
            Vector3d n = v1.Cross(v2);
            
            // define perpendicular x/y
            Vector3d xDir = v1;
            Vector3d yDir = n.Cross(v1);

            // initialise
            this.Initialise(origin, xDir, yDir);
        }

        /// <summary>
        /// Set X-dir of plane to input vector by rotating around plane normal.
        /// </summary>
        /// <remarks>Rename: SetXAroundNormal.</remarks>
        /// <param name="v">value for <paramref name="v"/>.</param>
        public void SetXAroundZ(Vector3d v)
        {
            if (v.IsPerpendicular(LocalZ))
            {
                Vector3d y = LocalZ.Cross(v);
                this.Initialise(Origin, v, y);
            }
            else
            {
                throw new System.ArgumentException($"Passed vector: {v} is not perpendicular to the plane normal: {LocalZ}");
            }
        }

        /// <summary>
        /// Set Y-dir of plane to input vector by rotating around plane X-dir.
        /// </summary>
        /// <param name="v">value for <paramref name="v"/>.</param>
        public void SetYAroundX(Vector3d v)
        {
            if (v.IsPerpendicular(LocalX))
            {
                this.Initialise(Origin, LocalX, v);
            }
            else
            {
                throw new System.ArgumentException($"Passed vector: {v} is not perpendicular to the plane XDir: {LocalX}");
            }
        }

        /// <summary>
        /// Set Y-Dir of plane to input vector by rotating around plane normal.
        /// </summary>
        /// <remarks>Rename: SetYAroundNormal.</remarks>
        /// <param name="v">value for <paramref name="v"/>.</param>
        public void SetYAroundZ(Vector3d v)
        {
            if (v.IsPerpendicular(LocalZ))
            {
                Vector3d xDir = v.Cross(LocalZ);
                this.Initialise(Origin, xDir, v);
            }
            else
            {
                throw new System.ArgumentException($"Passed vector: {v} is not perpendicular to the plane normal: {LocalZ}");
            }
        }

        /// <summary>
        /// Align normal of plane to input vector by rotating around plane X-dir.
        /// </summary>
        /// <remarks>Rename: AlignNormalAroundX.</remarks>
        /// <param name="v">value for <paramref name="v"/>.</param>
        public void SetZAroundX(Vector3d v)
        {
            if (v.IsPerpendicular(LocalX))
            {
                Vector3d y = v.Cross(LocalX);
                this.Initialise(Origin, LocalX, y);
            }
            else
            {
                throw new System.ArgumentException($"Passed vector: {v} is not perpendicular to the plane X-dir: {LocalX}");
            }
        }

        /// <summary>
        /// Flip Plane.
        /// </summary>
        public void FlipPlane()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Align plane Y-dir to GCS by rotating around plane X-dir
        /// </summary>
        /// <remarks>Rename: AlignYAroundXToGcs.</remarks>
        public void AlignYAroundXToGcs()
        {
            int par = this.LocalX.IsParallel(Vector3d.UnitZ);
            if (par == 1 || par == -1)
            {
                this.SetYAroundX(Vector3d.UnitY);
            }
            else
            {
                this.SetYAroundX(Vector3d.UnitZ.Cross(this.LocalX));
            }
        }

        /// <summary>
        /// Project point on plane
        /// </summary>
        /// <param name="p">value for <paramref name="p"/>.</param>
        /// <returns>Closest point to the p on the plane.</returns>
        public Point3d ProjectPointOnPlane(Point3d p)
        {
            // get vector from plane origin to p
            Vector3d op = p - Origin;

            // calculate projection of OP on plane normal
            Vector3d v = op.Dot(LocalZ)*LocalZ;

            // move p with v
            Point3d pointOnPlane = p - v;

            // return point on plane
            return pointOnPlane;
        }

        /// <summary>
        /// Implicity convert FdPoint to a FdCoordinateSystem. Local axis are set to Global X and Global Y.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="point">the point.</param>
        public static implicit operator Plane(Point3d point)
        {
            var plane = new Plane(point, Vector3d.UnitX, Vector3d.UnitY);
            return plane;
        }
    }
}
