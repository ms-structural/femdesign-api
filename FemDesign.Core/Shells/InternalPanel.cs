using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign
{
    /// <summary>
    /// internal_panels. List of internal_panel_type, used for struxml heirarchy.
    /// </summary>
    [System.Serializable]
    public partial class InternalPanels
    {
        /// <summary>
        /// Gets or sets the int panels.
        /// </summary>
        [XmlElement("item")]
        public List<InternalPanel> IntPanels { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private InternalPanels()
        {

        }

        /// <summary>
        /// Construct InternalPanels object from a single internal panel.
        /// </summary>
        /// <param name="obj">the obj.</param>
        public InternalPanels(InternalPanel obj)
        {
            this.IntPanels = new List<InternalPanel>{obj};
        }

        /// <summary>
        /// Construct InternalPanels object from List of internal panels.
        /// </summary>
        /// <param name="objs">the objs.</param>
        public InternalPanels(List<InternalPanel> objs)
        {
            this.IntPanels = objs;
        }
    }

    /// <summary>
    /// Represents a Internal Panel.
    /// </summary>
    [System.Serializable]
    public partial class InternalPanel
    {
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        [XmlElement("region")]
        public Geometry.Region Region { get; set; }
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        [XmlAttribute("guid")]
        public System.Guid Guid { get; set; }
        /// <summary>
        /// Gets or sets the mesh size.
        /// </summary>
        [XmlAttribute("mesh_size")]
        public double _meshSize; // non_neg_max_1e20
        [XmlIgnore]
        public double MeshSize
        {
            get
            {
                return this._meshSize;
            }
            set
            {
                this._meshSize = RestrictedDouble.NonNegMax_1e20(value);
            }
        }
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private InternalPanel()
        {

        }

        /// <summary>
        /// Construct InternalPanel
        /// </summary>
        /// <param name="region">Region. Edges can have EdgeConnections.</param>
        public InternalPanel(Geometry.Region region)
        {
            // set guid
            this.Guid = System.Guid.NewGuid();

            // set other properties
            this.Region = region;
        }

        /// <summary>
        /// Construct InternalPanel
        /// </summary>
        /// <param name="region">Region. Edges can have EdgeConnections.</param>
        /// <param name="meshSize"></param>
        public InternalPanel(Geometry.Region region, double meshSize)
        {
            // set guid
            this.Guid = System.Guid.NewGuid();

            // set other properties
            this.Region = region;
            this.MeshSize = meshSize;
        }
    }
}