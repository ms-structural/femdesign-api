using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Reinforcement
{
    /// <summary>
    /// Represents a Longitudinal Bar Reinforcement.
    /// </summary>
    [XmlRoot("database", Namespace = "urn:strusoft")]
    [System.Serializable]
    public partial class LongitudinalBarReinforcement : BarReinforcement
    {
        private LongitudinalBarReinforcement()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LongitudinalBarReinforcement"/> class.
        /// </summary>
        /// <param name="baseBar">the base bar.</param>
        /// <param name="wire">the wire.</param>
        /// <param name="longBar">the long bar.</param>
        public LongitudinalBarReinforcement(Guid baseBar, Wire wire, LongitudinalBar longBar) : base(baseBar, wire, longBar)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LongitudinalBarReinforcement"/> class.
        /// </summary>
        /// <param name="bar">the bar.</param>
        /// <param name="wire">the wire.</param>
        /// <param name="longBar">the long bar.</param>
        public LongitudinalBarReinforcement(Bars.Bar bar, Wire wire, LongitudinalBar longBar) : base(bar, wire, longBar)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LongitudinalBarReinforcement"/> class.
        /// </summary>
        /// <param name="reinf">the reinf.</param>
        public LongitudinalBarReinforcement(Reinforcement.BarReinforcement reinf)
        {
            BaseBar = reinf.BaseBar;
            Wire = reinf.Wire;
            LongitudinalBar = reinf.LongitudinalBar;
        }
       
    }
}
