// https://strusoft.com/

namespace FemDesign.Shells
{
    /// <summary>
    /// Represents a Thickness.
    /// </summary>
    [System.Serializable]
    public partial class Thickness: LocationValue
    {
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal Thickness()
        {

        }

        /// <summary>
        /// Construct Thickness object at point with value.
        /// </summary>
        /// <param name="point">the point.</param>
        /// <param name="val">the val.</param>
        public Thickness(Geometry.Point3d point, double val)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.Z = point.Z;
            this.Value = val;            
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{this.GetType().Name} Pos: {this.GetFdPoint()}, Value: {this.Value} m";
        }
    }
}