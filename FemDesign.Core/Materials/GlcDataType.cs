using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Materials
{
    /// <summary>
    /// Represents a Glc Data Type.
    /// </summary>
    [System.Serializable()]
    public partial class GlcDataType
    {
        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        [XmlElement("layer", Order = 1)]
        public List<MechProps> Layers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlcDataType"/> class.
        /// </summary>
        public GlcDataType()
        {

        }
    }
}