using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
namespace FemDesign.Soil
{
    /// <summary>
    /// Represents a Soil Elements.
    /// </summary>
    public partial class SoilElements
    {
        /// <summary>
        /// Gets or sets the strata.
        /// </summary>
        [XmlElement("strata", Order = 1)]
        public Strata Strata { get; set; }

        /// <summary>
        /// Gets or sets the bore holes.
        /// </summary>
        [XmlElement("borehole", Order = 2)]
        public List<BoreHole> BoreHoles { get; set; }

        // Filling
        // Excavation
        // Pipe

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public SoilElements()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoilElements"/> class.
        /// </summary>
        /// <param name="strata">the strata.</param>
        /// <param name="boreholes">the boreholes.</param>
        public SoilElements(Strata strata, List<BoreHole> boreholes)
        {
            this.Strata = strata;
            this.BoreHoles = boreholes;
            this.ValidateData();
        }

        private void ValidateData()
        {
            var stratumCount = this.Strata.Stratum.Count;
            var groundWaterCount = this.Strata.GroundWater.Count;

            foreach(var borehole in this.BoreHoles)
            {
                if(borehole.WholeLevelData != null)
                {
                    if(borehole.WholeLevelData.StrataTopLevels.Count != stratumCount)
                    {
                        throw new Exception($"Borehole '{borehole.Name}' must have {stratumCount} number of Strata Levels");
                    }
                    if (borehole.WholeLevelData.WaterLevels.Count != groundWaterCount)
                    {
                        throw new Exception($"Borehole's Waterlevels list must have {groundWaterCount} values");
                    }
                }
            }
        }
    }
}