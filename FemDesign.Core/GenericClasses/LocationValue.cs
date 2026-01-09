// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign
{
    /// <summary>
    /// Represents a Location Value.
    /// </summary>
    [System.Serializable]
    public partial class LocationValue: Geometry.Point3d
    {
        /// <summary>
        /// Value.
        /// </summary>
        [XmlAttribute("val")]
        public double _value;
        [XmlIgnore]
        public double Value
        {
            get { return this._value; }
            set { this._value = RestrictedDouble.AbsMax_1e20(value); }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal LocationValue()
        {

        }

        /// <summary>
        /// Convert coordinates of LocationValue to FdPoint.
        /// </summary>
        /// <returns>The result.</returns>
        public Geometry.Point3d GetFdPoint()
        {
            return new Geometry.Point3d(this.X, this.Y, this.Z);
        }
    }
}