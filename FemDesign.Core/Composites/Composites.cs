// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Composites
{
    /// <summary>
    /// Represents composites.
    /// </summary>
    [System.Serializable]
    public partial class Composites
    {
        /// <summary>
        /// Gets or sets the composite section.
        /// </summary>
        [XmlElement("composite_section", Order = 1)]
        public List<CompositeSection> CompositeSection = new List<CompositeSection>();

        /// <summary>
        /// Gets or sets the complex composite.
        /// </summary>
        [XmlElement("complex_composite", Order = 2)]
        public List<ComplexComposite> ComplexComposite = new List<ComplexComposite>();
    }
}
