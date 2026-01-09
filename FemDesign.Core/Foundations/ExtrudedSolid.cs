using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.ComponentModel;

namespace FemDesign.Foundations
{
    /// <summary>
    /// Represents a Extruded Solid.
    /// </summary>
    public partial class ExtrudedSolid
    {
        /// <summary>
        /// Gets or sets the thickness.
        /// </summary>
        [XmlAttribute("thickness")]
        public double Thickness { get; set; }

        /// <summary>
        /// Gets or sets the above.
        /// </summary>
        [XmlAttribute("abobe")]
        [DefaultValue(false)]
        public bool Above { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        [XmlElement("region")]
        public FemDesign.Geometry.Region Region { get; set; }

        /// <summary>
        /// Gets or sets the plinth.
        /// </summary>
        [XmlElement("cuboid_plinth")]
        public CuboidPlinth Plinth { get; set; }

        private ExtrudedSolid()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtrudedSolid"/> class.
        /// </summary>
        /// <param name="thickness">the thickness.</param>
        /// <param name="region">the region.</param>
        /// <param name="above">the above.</param>
        /// <param name="plinth">the plinth.</param>
        public ExtrudedSolid(double thickness, Geometry.Region region, bool above = false, CuboidPlinth plinth = null)
        {
            this.Thickness = thickness;
            this.Region = region;
            this.Above = above;
            this.Plinth = plinth;
        }

    }
}
