using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace FemDesign.Foundations
{
    /// <summary>
    /// Represents a Ref Parts.
    /// </summary>
    public partial class RefParts
    {
        /// <summary>
        /// Gets or sets the ref support.
        /// </summary>
        [XmlAttribute("ref_support")]
        public Guid RefSupport { get; set; }

        /// <summary>
        /// Gets or sets the ref slab.
        /// </summary>
        [XmlAttribute("ref_slab")]
        public Guid RefSlab { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefParts"/> class.
        /// </summary>
        public RefParts()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refSlab">the ref slab.</param>
        public RefParts(bool refSlab = false)
        {
            this.RefSupport = Guid.NewGuid();                   // What ref is referencing with the Guid?
            if (refSlab) { this.RefSlab = Guid.NewGuid(); }     // What ref is referencing with the Guid?
        }


    }
}
