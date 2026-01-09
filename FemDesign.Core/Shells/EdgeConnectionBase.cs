// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Shells
{
    /// <summary>
    /// Represents a Edge Connection Base.
    /// </summary>
    [System.Serializable]
    public partial class EdgeConnectionBase: EntityBase
    {
        /// <summary>
        /// Gets or sets the moving local.
        /// </summary>
        [XmlAttribute("moving_local")]
        public bool MovingLocal { get; set; } // bool. Default false according to strusoft.xsd but true in GUI?
        
        /// <summary>
        /// Gets or sets the joined start point.
        /// </summary>
        [XmlAttribute("joined_start_point")]
        public bool JoinedStartPoint { get; set; } // bool. Default false according to strusoft.xsd but true in GUI?
        
        /// <summary>
        /// Gets or sets the joined end point.
        /// </summary>
        [XmlAttribute("joined_end_point")]
        public bool JoinedEndPoint { get; set; } // bool. Default false according to strusoft.xsd but true in GUI?
    }
}