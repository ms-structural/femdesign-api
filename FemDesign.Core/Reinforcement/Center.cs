// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Reinforcement
{
    /// <summary>
    /// center (child of shell_rf_params_type)
    /// </summary>
    [System.Serializable]
    public partial class Center
    {
        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        [XmlAttribute("x")]
        public double X { get; set; } // double
        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        [XmlAttribute("y")]
        public double Y { get; set; } // double
        /// <summary>
        /// Gets or sets the z.
        /// </summary>
        [XmlAttribute("z")]
        public double Z { get; set; } // double
        /// <summary>
        /// Gets or sets the polar system.
        /// </summary>
        [XmlAttribute("polar_system")]
        public bool PolarSystem { get; set; } // bool

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Center()
        {

        }

        /// <summary>
        /// Private constructor accessed by static methods.
        /// </summary>
        private Center(double x, double y, double z, bool polarSystem)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.PolarSystem = polarSystem;
        }

        /// <summary>
        /// Create a Center element representing a Straight lay-out.
        /// </summary>
        /// <returns></returns>
        public static Center Straight()
        {
            return new Center(0,0,0, false);
        }
    }
}