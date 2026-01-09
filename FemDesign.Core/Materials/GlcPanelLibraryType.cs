using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Materials
{
    /// <summary>
    /// Represents a Glc Panel Types.
    /// </summary>
    [System.Serializable()]
    public partial class GlcPanelTypes
    {
        /// <summary>
        /// Gets or sets the glc panel library types.
        /// </summary>
        [XmlElement("predefined_type", Order = 1)]
        public List<GlcPanelLibraryType> GlcPanelLibraryTypes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlcPanelTypes"/> class.
        /// </summary>
        public GlcPanelTypes()
        {

        }
    }

    /// <summary>
    /// Represents a Glc Panel Library Type.
    /// </summary>
    [System.Serializable()]
    public partial class GlcPanelLibraryType: LibraryBase, IPanelLibraryType
    {
        /// <summary>
        /// Gets or sets the glc panel data.
        /// </summary>
        [XmlElement("glc_panel_data", Order = 1)]
        public GlcDataType GlcPanelData {get; set;}

        /// <summary>
        /// Initializes a new instance of the <see cref="GlcPanelLibraryType"/> class.
        /// </summary>
        public GlcPanelLibraryType()
        {
            
        }
    }
}