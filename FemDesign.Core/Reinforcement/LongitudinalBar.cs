using System;
using System.Xml.Serialization;


namespace FemDesign.Reinforcement
{
    /// <summary>
    /// Represents a Longitudinal Bar.
    /// </summary>
    [System.Serializable]
    public partial class LongitudinalBar
    {
        /// <summary>
        /// Gets or sets the position2d.
        /// </summary>
        [XmlElement("cross-sectional_position", Order = 1)]
        public Geometry.Point2d Position2d { get; set; }

        /// <summary>
        /// Gets or sets the anchorage.
        /// </summary>
        [XmlElement("anchorage", Order = 2)]
        public StartEndType Anchorage { get; set; }

        /// <summary>
        /// Gets or sets the prescribed lengthening.
        /// </summary>
        [XmlElement("prescribed_lengthening", Order = 3)]
        public StartEndType PrescribedLengthening { get; set; }

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
        /// Gets or sets the auxiliary.
        /// </summary>
        [XmlAttribute("auxiliary")]
        public bool Auxiliary { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        public LongitudinalBar()
        {

        }

        /// <summary>
        /// Construct longitudinal bar using start and end distance from bar start
        /// </summary>
        /// <param name="position"></param>
        /// <param name="startAnchorage">Start anchorage in meters.</param>
        /// <param name="endAnchorage">End anchorage in meters.</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="auxiliary"></param>
        public LongitudinalBar(Geometry.Point2d position, double startAnchorage, double endAnchorage, double start, double end, bool auxiliary)
        {
            this.Position2d = position;
            this.Anchorage = new StartEndType(startAnchorage, endAnchorage);
            this.Start = start;
            this.End = end;
            this.Auxiliary = auxiliary;
        }

        /// <summary>
        /// Construct longitudinal bar using start and end param from bar start
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="position"></param>
        /// <param name="startAnchorage">Start anchorage in meters.</param>
        /// <param name="endAnchorage">End anchorage in meters.</param>
        /// <param name="startParam"></param>
        /// <param name="endParam"></param>
        /// <param name="auxiliary"></param>
        public LongitudinalBar(Bars.Bar bar, Geometry.Point2d position, double startAnchorage, double endAnchorage, double startParam, double endParam, bool auxiliary)
        {
            this.Position2d = position;
            this.Anchorage = new StartEndType(startAnchorage, endAnchorage);
            var len = bar.BarPart.Edge.Length;
            this.Start = startParam * len;
            this.End = endParam * len;
            this.Auxiliary = auxiliary;
        }
    }
}