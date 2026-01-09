// https://strusoft.com/


namespace FemDesign.Releases
{
    /// <summary>
    /// Represents motions.
    /// </summary>
    [System.Serializable]
    public partial class Motions : StiffnessType
    {
        /// <summary>
        /// Rigid translation/rotation stiffness value for point support. [kN/m or kNm/rad]
        /// </summary>
        public static double ValueRigidPoint = 1e10;
        /// <summary>
        /// Rigid translation/rotation stiffness value for line support. [kN/m/m or kNm/m/rad]
        /// </summary>
        public static double ValueRigidLine = 1e7;
        /// <summary>
        /// Rigid translation stiffness value for surface support. [kN/m2/m]
        /// </summary>
        public static double ValueRigidSurface = 1e5;
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Motions()
        {

        }

        /// <summary>
        /// Motions constructor [kN/m, kN/m/m or kN/m2/m].
        /// </summary>
        /// <param name="xNeg">Kx' compression.</param>
        /// <param name="xPos">Kx' tension.</param>
        /// <param name="yNeg">Ky' compression.</param>
        /// <param name="yPos">Ky' tension.</param>
        /// <param name="zNeg">Kz' compression.</param>
        /// <param name="zPos">Kz' tension.</param>
        public Motions(double xNeg = 0, double xPos = 0, double yNeg = 0, double yPos = 0, double zNeg = 0, double zPos = 0)
        {
            this.XNeg = xNeg;
            this.XPos = xPos;
            this.YNeg = yNeg;
            this.YPos = yPos;
            this.ZNeg = zNeg;
            this.ZPos = zPos; 
        }

        /// <summary>
        /// Define a new translations release [kN/m, kN/m/m or kN/m2/m].
        /// </summary>
        /// <param name="xNeg">Kx' compression [kN/m, kN/m/m or kN/m2/m].</param>
        /// <param name="xPos">Kx' tension [kN/m, kN/m/m or kN/m2/m].</param>
        /// <param name="yNeg">Ky' compression [kN/m, kN/m/m or kN/m2/m].</param>
        /// <param name="yPos">Ky' tension [kN/m, kN/m/m or kN/m2/m].</param>
        /// <param name="zNeg">Kz' compression [kN/m, kN/m/m or kN/m2/m].</param>
        /// <param name="zPos">Kz' tension [kN/m, kN/m/m or kN/m2/m].</param>
        /// <returns>The result.</returns>
        public static Motions Define(double xNeg = 0, double xPos = 0, double yNeg = 0, double yPos = 0, double zNeg = 0, double zPos = 0)
        {
            return new Motions(xNeg, xPos, yNeg, yPos, zNeg, zPos);
        }
        /// <summary>
        /// Define a rigid translation release for a point-type release (1.000e+10 kN/m)
        /// </summary>
        /// <returns>The result.</returns>
        public static Motions RigidPoint()
        {
            double val = Motions.ValueRigidPoint;
            return new Motions(val, val, val, val, val, val);
        }
        /// <summary>
        /// Define a rigid translation release for a line-type release (1.000e+7 kN/m/m)
        /// </summary>
        /// <returns>The result.</returns>
        public static Motions RigidLine()
        {
            double val = Motions.ValueRigidLine;
            return new Motions(val, val, val, val, val, val);
        }
        /// <summary>
        /// Define a rigid translation release for a surface-type release (1.000e+5 kN/m2/m)
        /// </summary>
        /// <returns>The result.</returns>
        public static Motions RigidSurface()
        {
            double val = Motions.ValueRigidSurface;
            return new Motions(val, val, val, val, val, val);
        }
        /// <summary>
        /// Define a free translation release.
        /// </summary>
        /// <returns>The result.</returns>
        public static Motions Free()
        {
            return new Motions(0, 0, 0, 0, 0, 0);
        }
    }
}