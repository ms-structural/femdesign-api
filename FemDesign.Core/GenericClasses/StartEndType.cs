using System;
using System.Xml.Serialization;


namespace FemDesign
{
    /// <summary>
    /// Represents a Start End Type.
    /// </summary>
    [System.Serializable]
    public partial class StartEndType
    {
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
        private StartEndType()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StartEndType"/> class.
        /// </summary>
        /// <param name="start">the start.</param>
        /// <param name="end">the end.</param>
        public StartEndType(double start, double end)
        {
            this.Start = start;
            this.End = end;
        }
    }
}