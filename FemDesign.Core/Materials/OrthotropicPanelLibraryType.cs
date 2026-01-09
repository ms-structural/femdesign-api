// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.Materials;


namespace FemDesign.Materials
{
    /// <summary>
    /// Represents a Orthotropic Panel Types.
    /// </summary>
    [System.Serializable]
    public partial class OrthotropicPanelTypes
    {
        /// <summary>
        /// Gets or sets the orthotropic panel library types.
        /// </summary>
        [XmlElement("predefined_type", Order = 1)]
        public List<OrthotropicPanelLibraryType> OrthotropicPanelLibraryTypes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrthotropicPanelTypes"/> class.
        /// </summary>
        public OrthotropicPanelTypes()
        {
        
        }
    }

    /// <summary>
    /// Represents a Orthotropic Panel Library Type.
    /// </summary>
    [System.Serializable]
    public partial class OrthotropicPanelLibraryType: LibraryBase, IPanelLibraryType
    {
        /// <summary>
        /// Gets or sets the timber panel data.
        /// </summary>
        [XmlElement("timber_panel_data", Order = 1)]
        public OrthotropicPanelData TimberPanelData { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrthotropicPanelLibraryType"/> class.
        /// </summary>
        public OrthotropicPanelLibraryType()
        {

        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"Orthotropic Panel - {this.Name}";
        }
    }
}