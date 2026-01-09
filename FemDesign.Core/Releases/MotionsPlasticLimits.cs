// https://strusoft.com/


namespace FemDesign.Releases
{
    /// <summary>
    /// Represents a Motions Plastic Limits.
    /// </summary>
    [System.Serializable]
    public partial class MotionsPlasticLimits : PlasticityType3d
    {
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MotionsPlasticLimits()
        {

        }

        /// <summary>
        /// Define translational stiffness values for plastic limits [kN, kN/m or kN/m2].
        /// </summary>
        /// <param name="xNeg">Kx' compression. [kN, kN/m or kN/m2]. Default input null means no plastic limit.</param>
        /// <param name="xPos">Kx' tension. [kN, kN/m or kN/m2]. Default input null means no plastic limit.</param>
        /// <param name="yNeg">Ky' compression. [kN, kN/m or kN/m2]. Default input null means no plastic limit.</param>
        /// <param name="yPos">Ky' tension. [kN, kN/m or kN/m2]. Default input null means no plastic limit.</param>
        /// <param name="zNeg">Kz' compression. [kN, kN/m or kN/m2]. Default input null means no plastic limit.</param>
        /// <param name="zPos">Kz' tension. [kN, kN/m or kN/m2]. Default input null means no plastic limit.</param>
        public MotionsPlasticLimits(double? xNeg = null, double? xPos = null, double? yNeg = null, double? yPos = null, double? zNeg = null, double? zPos = null)
        {
            this.XNeg = xNeg;
            this.XPos = xPos;
            this.YNeg = yNeg;
            this.YPos = yPos;
            this.ZNeg = zNeg;
            this.ZPos = zPos; 
        }
    }
}