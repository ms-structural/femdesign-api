using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.AuxiliaryResults
{
    /// <summary>
    /// Class to contain list in entities. For serialization purposes only. Use LabelledSection to create the object
    /// </summary>
    [System.Serializable]
    public partial class LabelledSectionsGeometry
    {
        /// <summary>
        /// Gets or sets the labelled sections.
        /// </summary>
        [XmlElement("section_geometry", Order = 1)]
        public List<LabelledSection> LabelledSections = new List<LabelledSection>();
    }
}