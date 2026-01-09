// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Bars
{
    /// <summary>
    /// Represents a Eccentricity.
    /// 
    /// This class called Eccentricity as it is the Dynamo facing class.
    /// </summary>
    [System.Serializable]
    public partial class Eccentricity
    {
        /// <summary>
        /// Private field for eccentricity of local-x.
        /// </summary>
        [XmlAttribute("x")]
        public double _x;

        /// <summary>Eccentricity local-x. Sign convention of value as defined in FEM-Design GUI. Note that the value defined here will be negated in the generated .struxml file based on the data-protocol.</summary>
        [XmlIgnore]
        public double X
        {
            get
            {
                // value is negated to correspond to sign convention in FEM-Design GUI. Sign convention in data-protocol and FEM-Design GUI is different.
                double val = this._x * -1;
                return val;
            }
            set
            {
                // value is negated to correspond to sign convention in FEM-Design GUI. Sign convention in data-protocol and FEM-Design GUI is different.
                double val = value * -1;
                this._x = RestrictedDouble.AbsMax_1000(val);
            }
        }
        
        /// <summary>
        /// Private field for eccentricity of local-y
        /// </summary>
        [XmlAttribute("y")]
        public double _y;

        /// <summary>Eccentricity local-y. Sign convention of value as defined in FEM-Design GUI. Note that the value defined here will be negated in the generated .struxml file based on the data-protocol.</summary>
        [XmlIgnore]
        public double Y
        {
            get
            {
                // value is negated to correspond to sign convention in FEM-Design GUI. Sign convention in data-protocol and FEM-Design GUI is different.
                double val = this._y * -1;
                return val;
            }
            set
            {
                // value is negated to correspond to sign convention in FEM-Design GUI. Sign convention in data-protocol and FEM-Design GUI is different.
                double val = value * -1;
                this._y = RestrictedDouble.AbsMax_1000(val);
            }
        }

        /// <summary>Private field for eccentricity of local-z</summary>
        [XmlAttribute("z")]
        public double _z;

        /// <summary>Eccentricity local-z. Sign convention of value as defined in FEM-Design GUI. Note that the value defined here will be negated in the generated .struxml file based on the data-protocol.</summary>
        [XmlIgnore]
        public double Z
        {
            get
            {
                // value is negated to correspond to sign convention in FEM-Design GUI. Sign convention in data-protocol and FEM-Design GUI is different.
                double val = this._z * -1;
                return val;
            }
            set
            {
                // value is negated to correspond to sign convention in FEM-Design GUI. Sign convention in data-protocol and FEM-Design GUI is different.
                double val = value * -1;
                this._z = RestrictedDouble.AbsMax_1000(val);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Eccentricity()
        {
            
        }

        /// <summary>
        /// Constructor for eccentricity object used for bar-eccentricity definition. Sign convention of values as defined in FEM-Design GUI. Note that the value defined here will be negated in the generated .struxml file based on the data-protocol.
        /// </summary>
        /// <param name="y">Eccentricity local-y.</param>
        /// <param name="z">Eccentricity local-z.</param>
        public Eccentricity(double y = 0, double z = 0)
        {
            this.X = 0;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="obj">the obj.</param>
        /// <returns>The result.</returns>
        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Eccentricity p = obj as Eccentricity;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (X == p.X) && (Y == p.Y) && (Z == p.Z);            
        }

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="p">value for <paramref name="p"/>.</param>
        /// <returns>The result.</returns>
        public bool Equals(Eccentricity p)
        {
            if ((object)p == null)
            {
                return false;
            }
            return (X == p.X) && (Y == p.Y) && (Z == p.Z);
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The result.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        /// <summary>
        /// Create a default eccentricity, i.e. no ecceentricity (y=z=0).
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        public static Eccentricity Default => new Eccentricity(0, 0);
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{this.GetType().Name} Local-Y: {this.Y.ToString(FemDesign.TextFormatting.decimalRounding)}, Local-Z: {this.Z.ToString(FemDesign.TextFormatting.decimalRounding)}";
        }
    }
}