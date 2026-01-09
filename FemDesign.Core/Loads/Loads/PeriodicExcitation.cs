using FemDesign.GenericClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Periodic Excitation.
    /// </summary>
    [System.Serializable]
    public partial class PeriodicExcitation : FemDesign.GenericClasses.ILoadElement
    {
        /// <summary>
        /// Gets or sets the records.
        /// </summary>
        [XmlElement("record")]
        public List<PeriodicLoad> Records { get; set; } = new List<PeriodicLoad>();

        /// <summary>
        /// Gets or sets the last change.
        /// </summary>
        [XmlAttribute("last_change")]
        public DateTime LastChange { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        [XmlAttribute("action")]
        public string Action { get; set; } = "added";

        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        [XmlIgnore]
        public Guid Guid { get; set; }
        /// <summary>
        /// Entity Created.
        /// </summary>
        public void EntityCreated()
        {
            return;
        }
        /// <summary>
        /// Entity Modified.
        /// </summary>
        public void EntityModified()
        {
            return;
        }

        internal PeriodicExcitation() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodicExcitation"/> class.
        /// </summary>
        /// <param name="records">the records.</param>
        public PeriodicExcitation(List<PeriodicLoad> records)
        {
            this.Records = records;
        }
    }

    /// <summary>
    /// Represents a Periodic Load.
    /// </summary>
    [System.Serializable]
    public partial class PeriodicLoad : FemDesign.GenericClasses.ILoadElement
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        [XmlAttribute("frequency")]
        public double _frequency;

        [XmlIgnore]
        public double Frequency
        {
            get { return this._frequency; }
            set { this._frequency = RestrictedDouble.PositiveMax_1000(value); }
        }

        /// <summary>
        /// Gets or sets the case.
        /// </summary>
        [XmlElement("case")]
        public List<PeriodicCase> Case { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodicLoad"/> class.
        /// </summary>
        public PeriodicLoad() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="frequency">Hz</param>
        /// <param name="cases"></param>
        public PeriodicLoad(string name, double frequency, List<PeriodicCase> cases)
        {
            this.Name = name;
            this.Frequency = frequency;
            this.Case = cases;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="frequency">Hz</param>
        /// <param name="cases"></param>
        public PeriodicLoad(string name, double frequency, params PeriodicCase[] cases)
        {
            this.Name = name;
            this.Frequency = frequency;
            this.Case = cases.ToList();
        }


        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        [XmlIgnore]
        public Guid Guid { get; set; }
        /// <summary>
        /// Entity Created.
        /// </summary>
        public void EntityCreated()
        {
            return;
        }
        /// <summary>
        /// Entity Modified.
        /// </summary>
        public void EntityModified()
        {
            return;
        }
    }

    /// <summary>
    /// Represents a Periodic Case.
    /// </summary>
    public partial class PeriodicCase
    {
        /// <summary>
        /// Defines the Shape enumeration.
        /// </summary>
        public enum Shape
        {
            [Parseable("cos", "Cos", "c")]
            [XmlEnum("cos")]
            Cos,
            [Parseable("sin", "Sin", "s")]
            [XmlEnum("sin")]
            Sin,
        }

        /// <summary>
        /// Gets or sets the factor.
        /// </summary>
        [XmlAttribute("factor")]
        public double _factor;

        [XmlIgnore]
        public double Factor
        {
            get { return this._factor; }
            set { this._factor = RestrictedDouble.NonNegMax_1000(value); }
        }

        /// <summary>
        /// Gets or sets the phase.
        /// </summary>
        [XmlAttribute("phase")]
        public Shape phase { get; set; }

        /// <summary>
        /// Gets or sets the load case guid.
        /// </summary>
        [XmlAttribute("load_case")]
        public System.Guid LoadCaseGuid { get; set; }

        /// <summary>
        /// Gets or sets the load case.
        /// </summary>
        [XmlIgnore]
        public LoadCase _loadCase { get; set; }

        [XmlIgnore]
        public LoadCase LoadCase
        {
            get { return _loadCase; }
            set
            {
                _loadCase = value;
                LoadCaseGuid = value.Guid;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodicCase"/> class.
        /// </summary>
        public PeriodicCase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodicCase"/> class.
        /// </summary>
        /// <param name="factor">the factor.</param>
        /// <param name="phase">the phase.</param>
        /// <param name="LoadCase">the load case.</param>
        public PeriodicCase(double factor, Shape phase, LoadCase LoadCase)
        {
            this.Factor = factor;
            this.phase = phase;
            this.LoadCase = LoadCase;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodicCase"/> class.
        /// </summary>
        /// <param name="factor">the factor.</param>
        /// <param name="phase">the phase.</param>
        /// <param name="LoadCaseGuid">the load case guid.</param>
        public PeriodicCase(double factor, Shape phase, Guid LoadCaseGuid)
        {
            this.Factor = factor;
            this.phase = phase;
            this.LoadCaseGuid = LoadCaseGuid;
        }

    }

}
