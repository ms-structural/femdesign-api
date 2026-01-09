using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Foundations
{
    /// <summary>
    /// Represents foundations.
    /// </summary>
    public partial class Foundations
    {
        /// <summary>
        /// Gets a value indicating whether olated foundations.
        /// </summary>
        [XmlElement("isolated_foundation")]
        public List<IsolatedFoundation> IsolatedFoundations = new List<IsolatedFoundation>();

        /// <summary>
        /// Gets or sets the wall foundation field.
        /// </summary>
        [XmlElement("wall_foundation")]
        public List<StruSoft.Interop.StruXml.Data.Lnfoundation_type> wall_foundationField = new List<StruSoft.Interop.StruXml.Data.Lnfoundation_type>();

        /// <summary>
        /// Gets or sets the slab foundations.
        /// </summary>
        [XmlElement("foundation_slab")]
        public List<SlabFoundation> SlabFoundations = new List<SlabFoundation>();


        /// <summary>
        /// Gets the foundations.
        /// </summary>
        /// <returns>The result.</returns>
        public List<dynamic> GetFoundations()
        {
            var objs = new List<dynamic>();
            objs.AddRange(this.IsolatedFoundations);
            objs.AddRange(this.wall_foundationField); // to implement
            objs.AddRange(this.SlabFoundations); // to implement
            return objs;
        }
    }
}