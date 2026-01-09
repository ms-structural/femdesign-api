using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Text;

using FemDesign.GenericClasses;

namespace FemDesign
{
    /// <summary>
    /// Represents a S Factor Type.
    /// </summary>
    [System.Serializable]
    public partial class SFactorType
    {
        /// <summary>
        /// Gets or sets the sc.
        /// </summary>
        [XmlAttribute("Sc")]
        public double _sc { get; set; }
        [XmlIgnore]
        public double Sc
        {
            get { return this._sc; }
            set { this._sc = RestrictedDouble.NonNegMax_1e20(value); }
        }

        /// <summary>
        /// Gets or sets the sf.
        /// </summary>
        [XmlAttribute("Sf")]
        public double _sf { get; set; }
        [XmlIgnore]
        public double Sf
        {
            get { return this._sf; }
            set { this._sf = RestrictedDouble.NonNegMax_1e20(value); }
        }

        /// <summary>
        /// Gets or sets the sq.
        /// </summary>
        [XmlAttribute("Sq")]
        public double _sq { get; set; }
        [XmlIgnore]
        public double Sq
        {
            get { return this._sq; }
            set { this._sq = RestrictedDouble.NonNegMax_1e20(value); }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public SFactorType()
        {

        }

        /// <summary>
        /// Create (construction stage) SLS factors.
        /// </summary>
        /// <param name="sc">Factor value for Characteristic combinations</param>
        /// <param name="sf">Factor value for Frequent combinations</param>
        /// <param name="sq">Factor value for Quasi-permanent combinations.</param>
        public SFactorType(double sc, double sf, double sq)
        {
            this.Sc = sc;
            this.Sf = sf;
            this.Sq = sq;
        }
    }
}
