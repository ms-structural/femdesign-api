// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// load categories
    /// </summary>
    [System.Serializable]
    public partial class LoadCategories
    {
        /// <summary>
        /// Gets or sets the load category.
        /// </summary>
        [XmlElement("load_category", Order = 1)]
        public List<FemDesign.Loads.LoadCategory> LoadCategory = new List<FemDesign.Loads.LoadCategory>(); // sequence: load_category
    }
}