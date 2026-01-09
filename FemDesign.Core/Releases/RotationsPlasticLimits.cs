// https://strusoft.com/


namespace FemDesign.Releases
{
    /// <summary>
    /// Represents a Rotations Plastic Limits.
    /// </summary>
    [System.Serializable]
    public partial class RotationsPlasticLimits : PlasticityType3d
    {
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private RotationsPlasticLimits()
        {

        }

        /// <summary>
        /// Define rotational stiffness values for plastic limits [kN/m or kN/m/m].
        /// </summary>
        /// <param name="xNeg">Kx' compression. [kN/m or kN/m/m]. Default input null means no plastic limit.</param>
        /// <param name="xPos">Kx' tension. [kN/m or kN/m/m]. Default input null means no plastic limit.</param>
        /// <param name="yNeg">Ky' compression. [kN/m or kN/m/m]. Default input null means no plastic limit.</param>
        /// <param name="yPos">Ky' tension. [kN/m or kN/m/m]. Default input null means no plastic limit.</param>
        /// <param name="zNeg">Kz' compression. [kN/m or kN/m/m]. Default input null means no plastic limit.</param>
        /// <param name="zPos">Kz' tension. [kN/m or kN/m/m]. Default input null means no plastic limit.</param>
        public RotationsPlasticLimits(double? xNeg = null, double? xPos = null, double? yNeg = null, double? yPos = null, double? zNeg = null, double? zPos = null)
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