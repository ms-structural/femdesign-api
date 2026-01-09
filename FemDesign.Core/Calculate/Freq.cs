// https://strusoft.com/
using FemDesign.GenericClasses;
using System;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// </summary>
    public partial class Freq
    {
        /// <summary>
        /// Gets or sets the num shapes.
        /// </summary>
        [XmlAttribute("Numshapes")]
        public int NumShapes { get; set; } = 3;

        /// <summary>
        /// with the value of 0 disables the "Try to reach ..." option, and an integer greater than 0 activates the option and defines the maximum number of iteration.
        /// </summary>
        [XmlAttribute("AutoIter")]
        public int AutoIter { get; set; } = 0;

        /// <summary>
        /// Gets or sets the shape normalization.
        /// </summary>
        [XmlAttribute("NormUnit")]
        public int _shapeNormalization = 0;

        /// <summary>
        /// a Boolean-type parameter whose True value expresses the `Unit` option and its False value expresses the `Mass matrix` option as the Mode shape normalization method.
        /// </summary>
        [XmlIgnore]
        public ShapeNormalisation ShapeNormalization
        {
            get
            {
                return (ShapeNormalisation)_shapeNormalization;
            }
            set
            {
                _shapeNormalization = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the max sturm.
        /// </summary>
        [XmlAttribute("MaxSturm")]
        public int MaxSturm { get; set; } = 0;

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        [XmlAttribute("X")]
        public bool _x;

        [XmlIgnore]
        public bool X
        {
            get
            {
                return this._x;
            }
            set
            {
                this._x = value;
            }
        }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        [XmlAttribute("Y")]
        public bool _y;

        [XmlIgnore]
        public bool Y
        {
            get
            {
                return this._y;
            }
            set
            {
                this._y = value;
            }
        }

        /// <summary>
        /// Gets or sets the z.
        /// </summary>
        [XmlAttribute("Z")]
        public bool _z;

        [XmlIgnore]
        public bool Z
        {
            get
            {
                return this._z;
            }
            set
            {
                this._z = value;
            }
        }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        [XmlAttribute("top")]
        public double Top { get; set; } = -0.01;

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Freq()
        {
            
        }

        /// <summary>
        /// Define calculation parameters for an eigenfrequency calculation.
        /// </summary>
        /// <param name="numShapes">Number of shapes.</param>
        /// <param name="maxSturm">Max number of Sturm check steps (checking missing eigenvalues).</param>
        /// <param name="x">Consider masses in global x-direction.</param>
        /// <param name="y">Consider masses in global y-direction.</param>
        /// <param name="z">Consider masses in global z-direction.</param>
        /// <param name="top">Top of substructure. Masses on this level and below are not considered in Eigenfrequency calculation.</param>
        public Freq(int numShapes = 3, int maxSturm = 0, bool x = true, bool y = true, bool z = true, double top = -0.01)
        {
            this.NumShapes = numShapes;
            this.MaxSturm = maxSturm;
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Top = top;
        }

        /// <summary>
        /// Define calculation parameters for an eigenfrequency calculation.
        /// </summary>
        /// <param name="numShapes">Number of shapes.</param>
        /// <param name="autoIter">Max. number of iteration.</param>
        /// <param name="normalisation">Mode shape normalisation.</param>
        /// <param name="x">Consider masses in global x-direction.</param>
        /// <param name="y">Consider masses in global y-direction.</param>
        /// <param name="z">Consider masses in global z-direction.</param>
        /// <param name="maxSturm">Max number of Sturm check steps (checking missing eigenvalues).</param>
        /// <param name="top">Top of substructure. Masses on this level and below are not considered in Eigenfrequency calculation.</param>
        public Freq(int numShapes = 3, int autoIter = 0, ShapeNormalisation normalisation = ShapeNormalisation.MassMatrix, bool x = true, bool y = true, bool z = true, int maxSturm = 0, double top = -0.01)
        {
            this.NumShapes = numShapes;
            this.AutoIter = autoIter;
            this.ShapeNormalization = normalisation;
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.MaxSturm = maxSturm;
            this.Top = top;
        }

        /// <summary>
        /// Define default calculation parameters for an eigenfrequency calculation.
        /// </summary>
        /// <returns></returns>
        public static Freq Default()
        {
            return new Freq(3, 0, true, true, true, -0.01);
        }

    }

    /// <summary>
    /// Defines the Shape Normalisation enumeration.
    /// </summary>
    public enum ShapeNormalisation
    {
        [Parseable("MassMatrix")]
        MassMatrix, // 0
        [Parseable("Unit")]
        Unit,       // 1
    }
}