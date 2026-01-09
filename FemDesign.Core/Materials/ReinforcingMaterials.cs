// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Materials
{
    /// <summary>
    /// reinforcing_materials.
    /// </summary>
    [System.Serializable]
    public partial class ReinforcingMaterials
    {
        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        [XmlElement("material", Order = 1)]
        public List<Material> Material = new List<Material>(); // sequence: rfmaterial_type
    }
}