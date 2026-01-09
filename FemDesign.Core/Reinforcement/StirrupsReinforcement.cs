using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Reinforcement
{
    
    /// <summary>
    /// Represents a Stirrup Reinforcement.
    /// </summary>
    [XmlRoot("database", Namespace = "urn:strusoft")]
    [System.Serializable]
    public partial class StirrupReinforcement : BarReinforcement
    {
        private StirrupReinforcement()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StirrupReinforcement"/> class.
        /// </summary>
        /// <param name="baseBar">the base bar.</param>
        /// <param name="wire">the wire.</param>
        /// <param name="stirrups">the stirrups.</param>
        public StirrupReinforcement(Guid baseBar, Wire wire, Stirrups stirrups) : base(baseBar, wire, stirrups)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StirrupReinforcement"/> class.
        /// </summary>
        /// <param name="bar">the bar.</param>
        /// <param name="wire">the wire.</param>
        /// <param name="stirrups">the stirrups.</param>
        public StirrupReinforcement(Bars.Bar bar, Wire wire, Stirrups stirrups) : base(bar, wire, stirrups)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StirrupReinforcement"/> class.
        /// </summary>
        /// <param name="reinf">the reinf.</param>
        public StirrupReinforcement(Reinforcement.BarReinforcement reinf)
        {
            BaseBar = reinf.BaseBar;
            Wire = reinf.Wire;
            Stirrups = reinf.Stirrups;
        }
    }
}
