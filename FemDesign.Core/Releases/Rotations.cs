// https://strusoft.com/


namespace FemDesign.Releases
{
    /// <summary>
    /// Represents rotations.
    /// </summary>
    [System.Serializable]
    public partial class Rotations: StiffnessType
    {
        /// <summary>
        /// Gets or sets the value rigid point.
        /// </summary>
        public static double ValueRigidPoint = 10000000000;
        /// <summary>
        /// Gets or sets the value rigid line.
        /// </summary>
        public static double ValueRigidLine = 10000000;
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Rotations()
        {

        }

        /// <summary>
        /// Rotations constructor [kNm/rad or kNm/m/rad].
        /// </summary>
        /// <param name="xNeg">Cx' compression. </param>
        /// <param name="xPos">Cx' tension.</param>
        /// <param name="yNeg">Cy' compression.</param>
        /// <param name="yPos">Cy' tension.</param>
        /// <param name="zNeg">Cz' compression.</param>
        /// <param name="zPos">Cz' tension.</param>
        public Rotations(double xNeg = 0, double xPos = 0, double yNeg = 0, double yPos = 0, double zNeg = 0, double zPos = 0)
        {
            this.XNeg = xNeg;
            this.XPos = xPos;
            this.YNeg = yNeg;
            this.YPos = yPos;
            this.ZNeg = zNeg;
            this.ZPos = zPos; 
        }
        
        /// <summary>
        /// Define a new rotations release [kNm/rad or kNm/m/rad].
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="xNeg">Cx' compression [kNm/rad or kNm/m/rad].</param>
        /// <param name="xPos">Cx' tension [kNm/rad or kNm/m/rad].</param>
        /// <param name="yNeg">Cy' compression [kNm/rad or kNm/m/rad].</param>
        /// <param name="yPos">Cy' tension [kNm/rad or kNm/m/rad].</param>
        /// <param name="zNeg">Cz' compression [kNm/rad or kNm/m/rad].</param>
        /// <param name="zPos">Cz' tension [kNm/rad or kNm/m/rad].</param>
        /// <returns>The result.</returns>
        public static Rotations Define(double xNeg = 0, double xPos = 0, double yNeg = 0, double yPos = 0, double zNeg = 0, double zPos = 0)
        {
            return new Rotations(xNeg, xPos, yNeg, yPos, zNeg, zPos);
        }
        /// <summary>
        /// Define a rigid rotations release for a point-type release (1e+10 kNm/rad).
        /// </summary>
        /// <returns>The result.</returns>
        public static Rotations RigidPoint()
        {
            double val = Rotations.ValueRigidPoint;
            return new Rotations(val, val, val, val, val, val);
        }
        /// <summary>
        /// Define a rigid rotations release for a line-type release (1e+07 kNm/m/rad).
        /// </summary>
        /// <returns>The result.</returns>
        public static Rotations RigidLine()
        {
            double val = Rotations.ValueRigidLine;
            return new Rotations(val, val, val, val,  val, val);
        }
        /// <summary>
        /// Define a free rotations release.
        /// </summary>
        /// <returns>The result.</returns>
        public static Rotations Free()
        {
            return new Rotations(0,0,0,0,0,0);
        }   
    }
}