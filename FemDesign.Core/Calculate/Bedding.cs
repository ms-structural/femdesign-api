// https://strusoft.com/
using FemDesign.GenericClasses;
using System;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// </summary>
    public partial class Bedding
    {
        /// <summary>
        /// Gets or sets the ld comb char.
        /// </summary>
        [XmlAttribute("Ldcomb")]
        public string LdCombChar { get; set; }

        /// <summary>
        /// Gets or sets the mesh prep.
        /// </summary>
        [XmlAttribute("Meshprep")]
        public int _meshPrep = 0;
        [XmlIgnore]
        public MeshPrep MeshPrep
        {
            get
            {
                return (MeshPrep)_meshPrep;
            }
            set
            {
                this._meshPrep = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the stiff x.
        /// </summary>
        [XmlAttribute("Stiff_X")]
        public double StiffX { get; set; } = 0.5;

        /// <summary>
        /// Gets or sets the stiff y.
        /// </summary>
        [XmlAttribute("Stiff_Y")]
        public double StiffY { get; set; } = 0.5;

        private Bedding()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bedding"/> class.
        /// </summary>
        /// <param name="ldCombChar">the ld comb char.</param>
        /// <param name="meshPrep">the mesh prep.</param>
        /// <param name="stiffX">the stiff x.</param>
        /// <param name="stiffY">the stiff y.</param>
        public Bedding(string ldCombChar, MeshPrep meshPrep, double stiffX, double stiffY)
        {
            this.LdCombChar = ldCombChar;
            this.MeshPrep = meshPrep;
            this.StiffX = stiffX;
            this.StiffY = stiffY;
        }

        /// <summary>
        /// Default.
        /// </summary>
        /// <returns>The result.</returns>
        public static Bedding Default()
        {
            return new Bedding
            {
                StiffX = 0.5,
                StiffY = 0.5,
                MeshPrep = MeshPrep.ActualMesh,
            };
        }
    }

    /// <summary>
    /// Defines the Mesh Prep enumeration.
    /// </summary>
    public enum MeshPrep
    {
        [Parseable("ActualMesh")]
        ActualMesh = 0,
        [Parseable("FactoryDefault")]
        FactoryDefault = 1,
    }
}
