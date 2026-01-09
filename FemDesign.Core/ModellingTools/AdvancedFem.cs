// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.ModellingTools
{
    /// <summary>
    /// Connections and virtual objects
    /// </summary>
    [System.Serializable]
    public partial class AdvancedFem
    {
        /// <summary>
        /// Gets or sets the connected points.
        /// </summary>
        [XmlElement("connected_points", Order = 1)]
        public List<ConnectedPoints> ConnectedPoints { get; set; } = new List<ConnectedPoints>();

        /// <summary>
        /// Gets or sets the connected lines.
        /// </summary>
        [XmlElement("connected_lines", Order = 2)]
        public List<ConnectedLines> ConnectedLines { get; set; } = new List<ConnectedLines>();

        /// <summary>
        /// Gets or sets the surface connections.
        /// </summary>
        [XmlElement("surface_connection", Order = 3)]
        public List<SurfaceConnection> SurfaceConnections { get; set; } = new List<SurfaceConnection>();

        /// <summary>
        /// Gets or sets the fictitious bars.
        /// </summary>
        [XmlElement("virtual_bar", Order = 4)]
        public List<FictitiousBar> FictitiousBars = new List<FictitiousBar>();

        /// <summary>
        /// Gets or sets the fictitious shells.
        /// </summary>
        [XmlElement("virtual_shell", Order = 5)]
        public List<FictitiousShell> FictitiousShells = new List<FictitiousShell>();

        /// <summary>
        /// Gets or sets the diaphragms.
        /// </summary>
        [XmlElement("diaphragm", Order = 6)]
        public List<Diaphragm> Diaphragms { get; set; } = new List<Diaphragm>();


        /// <summary>
        /// Gets or sets the steel joint type.
        /// </summary>
        [XmlElement("steel_joint", Order = 7)]
        public List<StruSoft.Interop.StruXml.Data.Steel_joint_type> SteelJointType { get; set; }

        /// <summary>
        /// List of Cover (cover_type)
        /// </summary>
        [XmlElement("cover", Order = 8)]
        public List<Cover> Covers = new List<Cover>();

    }
}