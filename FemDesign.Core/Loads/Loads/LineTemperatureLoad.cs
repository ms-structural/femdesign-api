using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Line Temperature Load.
    /// </summary>
    [System.Serializable]
    public partial class LineTemperatureLoad: LoadBase
    {
        /// <summary>
        /// Edge defining the geometry of the load
        /// </summary>
        [XmlElement("edge", Order=1)]
        public Geometry.Edge Edge { get; set; }


        /// <summary>
        /// Field
        /// </summary>
        [XmlElement("direction", Order = 2)]
        public Geometry.Vector3d _direction;

        /// <summary>
        /// Direction of load.
        /// </summary>
        [XmlIgnore]
        public Geometry.Vector3d Direction
        {
            get
            {
                return this._direction;
            }
            set
            {
                if (value.IsPerpendicular(Edge.Plane.LocalX))
                {
                    this._direction = value;
                }
                else
                {
                    throw new System.ArgumentException("The direction must be perpendicular to the curve.");
                }
            }
        }

        /// <summary>
        /// Optional. Ambiguous what this does.
        /// </summary>
        /// <value></value>

        [XmlElement("normal", Order=3)]
        public Geometry.Vector3d Normal { get; set; }

        /// <summary>
        /// Field
        /// </summary>
        [XmlElement("temperature", Order=4)]
        public List<TopBotLocationValue> _topBotLocVal;

        
        /// <summary>
        /// Top bottom value can be a list of 1 or 2 items. 1 item defines a uniform line load, 2 items defines a variable line load.
        /// </summary>
        [XmlIgnore]
        public List<TopBotLocationValue> TopBotLocVal
        {
            get
            {
                return this._topBotLocVal;
            }
            set
            {
                if (value.Count == 2)
                {
                    this._topBotLocVal = value;
                }
                else
                {
                    throw new System.ArgumentException($"Length of list is: {value.Count}, expected 2");
                }
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private LineTemperatureLoad()
        {

        }

        /// <summary>
        /// Construct a uniform or variable line temperature load
        /// </summary>
        /// <param name="edge">Underlying edge of line load. Line or Arc.</param>
        /// <param name="direction">Directio of load.</param>
        /// <param name="topBotLocVals">1 or 2 top bottom location values</param>
        /// <param name="loadCase"></param>
        /// <param name="comment"></param>
        public LineTemperatureLoad(Geometry.Edge edge, Geometry.Vector3d direction, List<TopBotLocationValue> topBotLocVals, LoadCase loadCase, string comment)
        {
            this.EntityCreated();
            this.Edge = edge;
            this.Direction = direction;
            this.TopBotLocVal = topBotLocVals;
            this.LoadCase = loadCase;
            this.Comment = comment;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{this.GetType().Name} - Start:{this.TopBotLocVal[0]}  End:{this.TopBotLocVal[1]}";
        }

    }
}