// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    /// <summary>
    /// Represents a Region Group.
    /// </summary>
    [System.Serializable]
    public partial class RegionGroup
    {
        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        [XmlElement("region")]
        public List<Region> Regions = new List<Region>(); // sequence: region_type

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private RegionGroup()
        {

        }

        /// <summary>
        /// Construct region group from single region
        /// </summary>
        /// <param name="region">the region.</param>
        public RegionGroup(Region region)
        {
            this.Regions.Add(region);
        }

        /// <summary>
        /// Construct region group from list of regions
        /// </summary>
        /// <param name="regions">the regions.</param>
        public RegionGroup(List<Region> regions)
        {
            this.Regions = regions;
        }


    }
}