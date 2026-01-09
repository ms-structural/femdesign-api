// https://strusoft.com/
using FemDesign.GenericClasses;


namespace FemDesign.Shells
{
    /// <summary>
    /// Represents a Shell Eccentricity.
    /// </summary>
    public partial class ShellEccentricity
    {
        /// <summary>
        /// Gets or sets the alignment.
        /// </summary>
        public VerticalAlignment Alignment { get; set; }
        private double _eccentricity; // align_offset // abs_max_1e20
        public double Eccentricity
        {
            get {return this._eccentricity;}
            set {this._eccentricity = RestrictedDouble.AbsMax_1e20(value);}
        }
        /// <summary>
        /// Gets or sets the eccentricity calculation.
        /// </summary>
        public bool EccentricityCalculation { get; set; } // bool
        /// <summary>
        /// Gets or sets the eccentricity by cracking.
        /// </summary>
        public bool EccentricityByCracking { get; set; } // bool

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ShellEccentricity()
        {

        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellEccentricity"/> class.
        /// </summary>
        /// <param name="alignment">the alignment.</param>
        /// <param name="eccentricity">the eccentricity.</param>
        /// <param name="eccentricityCalculation">the eccentricity calculation.</param>
        /// <param name="eccentricityByCracking">the eccentricity by cracking.</param>
        public ShellEccentricity(VerticalAlignment alignment, double eccentricity, bool eccentricityCalculation, bool eccentricityByCracking)
        {
            this.Alignment = alignment;
            this.Eccentricity = eccentricity;
            this.EccentricityCalculation = eccentricityCalculation;
            this.EccentricityByCracking = eccentricityByCracking;
        }

        /// <summary>
        /// Create a default ShellEccentricity.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        public static ShellEccentricity Default => new ShellEccentricity(VerticalAlignment.Center, 0, false, false);

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{this.GetType().Name} {this.Eccentricity} m";
        }
    }
}