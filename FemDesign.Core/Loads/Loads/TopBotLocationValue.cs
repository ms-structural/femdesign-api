// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Top Bot Location Value.
    /// </summary>
    [System.Serializable]
    public partial class TopBotLocationValue: Geometry.Point3d
    {
        /// <summary>
        /// Top value
        /// </summary>
        [XmlAttribute("top_val")]
        public double _topVal;
        [XmlIgnore]
        public double TopVal
        {
            get { return this._topVal; }
            set { this._topVal = RestrictedDouble.AbsMax_1e20(value); }
        }

        /// <summary>
        /// Bottom value
        /// </summary>
        [XmlAttribute("bottom_val")]
        public double _bottomVal;
        [XmlIgnore]
        public double BottomVal
        {
            get { return this._bottomVal; }
            set { this._bottomVal = RestrictedDouble.AbsMax_1e20(value); }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal TopBotLocationValue()
        {

        }

        /// <summary>
        /// Construct top bottom location value by point, top value and bottom value.
        /// </summary>
        /// <param name="point">the point.</param>
        /// <param name="topVal">the top val.</param>
        /// <param name="bottomVal">the bottom val.</param>
        public TopBotLocationValue(Geometry.Point3d point, double topVal, double bottomVal)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.Z = point.Z;
            this.TopVal = topVal;
            this.BottomVal = bottomVal;
        }

        /// <summary>
        /// Convert coordinates to FdPoint.
        /// </summary>
        /// <returns>The result.</returns>
        public Geometry.Point3d GetFdPoint()
        {
            return new Geometry.Point3d(this.X, this.Y, this.Z);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{GetFdPoint()} Top:{this.TopVal}°C Bottom:{this.BottomVal}°C;";
        }
    }
}