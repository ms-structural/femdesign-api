// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Materials
{
    /// <summary>
    /// Represents materials.
    /// </summary>
    [System.Serializable]
    public partial class Materials
    {
        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        [XmlElement("material", Order = 1)]
        public List<Material> Material = new List<Material>(); // sequence: material_type

    }
}