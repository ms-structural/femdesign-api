using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Reinforcement
{
    /// <summary>
    /// Represents stirrups.
    /// </summary>
    [System.Serializable]
    public partial class Stirrups
    {
        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        [XmlElement("region", Order = 1)]
        public List<Geometry.Region> Regions = new List<Geometry.Region>();

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        [XmlAttribute("start")]
        public double Start { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        [XmlAttribute("end")]
        public double End { get; set; }

        /// <summary>
        /// Gets or sets the distance.
        /// </summary>
        [XmlAttribute("distance")]
        public double Distance { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        public Stirrups()
        {

        }

        /// <summary>
        /// Construct stirrups by start and end distance from bar start.
        /// </summary>
        /// <param name="region">the region.</param>
        /// <param name="start">the start.</param>
        /// <param name="end">the end.</param>
        /// <param name="distance">the distance.</param>
        public Stirrups(Geometry.Region region, double start, double end, double distance)
        {
            this.Regions.Add(region);
            this.Start = start;
            this.End = end;
            this.Distance = distance;
        }

        /// <summary>
        /// Construct stirrups by start and end parameter on the bar.
        /// </summary>
        /// <param name="bar">the bar.</param>
        /// <param name="region">the region.</param>
        /// <param name="startParam">the start param.</param>
        /// <param name="endParam">the end param.</param>
        /// <param name="distance">the distance.</param>
        public Stirrups(Bars.Bar bar, Geometry.Region region, double startParam, double endParam, double distance)
        {
            this.Regions.Add(region);
            double len = bar.BarPart.Edge.Length;
            this.Start = startParam * len;
            this.End = endParam * len;
            this.Distance = distance;
        }
    }
}