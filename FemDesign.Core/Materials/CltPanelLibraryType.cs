using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Materials
{
    /// <summary>
    /// Represents clt panel types.
    /// </summary>
    [System.Serializable()]
    public partial class CltPanelTypes
    {
        /// <summary>
        /// Gets or sets the clt panel library types.
        /// </summary>
        [XmlElement("predefined_type", Order = 1)]
        public List<CltPanelLibraryType> CltPanelLibraryTypes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CltPanelTypes"/> class.
        /// </summary>
        public CltPanelTypes()
        {

        }
    }

    /// <summary>
    /// Represents a Clt Panel Library Type.
    /// </summary>
    [System.Serializable()]
    public partial class CltPanelLibraryType: LibraryBase, IPanelLibraryType
    {
        /// <summary>
        /// Gets or sets the clt panel data.
        /// </summary>
        [XmlElement("clt_panel_data")]
        public CltDataType CltPanelData { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CltPanelLibraryType"/> class.
        /// </summary>
        public CltPanelLibraryType()
        {

        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"CLT Panel - {this.Name}";
        }
    }
}