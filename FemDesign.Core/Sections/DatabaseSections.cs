// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Sections
{
    /// <summary>
    /// Sections container used in database.
    /// </summary>
    [System.Serializable]
    public partial class DatabaseSections
    {
        /// <summary>
        /// Gets or sets the section.
        /// </summary>
        [XmlElement("section")]
        public List<Section> Section = new List<Section>();
    }
}