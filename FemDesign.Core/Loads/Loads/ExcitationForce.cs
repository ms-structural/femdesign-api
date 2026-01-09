using StruSoft.Interop.StruXml.Data;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Diagnostics;


namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Excitation Force.
    /// </summary>
    [System.Serializable]
    public partial class ExcitationForce : FemDesign.GenericClasses.ILoadElement
    {
        /// <summary>
        /// Gets or sets the diagram.
        /// </summary>
        [XmlElement("diagram")]
        public List<Diagram> Diagram { get; set; }

        /// <summary>
        /// Gets or sets the combination.
        /// </summary>
        [XmlElement("combination")]
        public List<ExcitationForceCombination> Combination { get; set; }

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
        /// Initializes a new instance of the <see cref="ExcitationForce"/> class.
        /// </summary>
        public ExcitationForce() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcitationForce"/> class.
        /// </summary>
        /// <param name="diagrams">the diagrams.</param>
        /// <param name="combinations">the combinations.</param>
        public ExcitationForce(List<Diagram> diagrams, List<ExcitationForceCombination> combinations)
        {
            this.Diagram = diagrams;
            this.Combination = combinations;
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
    /// Represents a Excitation Force Combination.
    /// </summary>
    [System.Serializable]
    public partial class ExcitationForceCombination
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the d t.
        /// </summary>
        [XmlAttribute("dt")]
        public double dT { get; set; }

        /// <summary>
        /// Gets or sets the records.
        /// </summary>
        [XmlElement("record")]
        public List<ExcitationForceLoadCase> records { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcitationForceCombination"/> class.
        /// </summary>
        public ExcitationForceCombination() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcitationForceCombination"/> class.
        /// </summary>
        /// <param name="name">the name.</param>
        /// <param name="dT">value for <paramref name="dT"/>.</param>
        /// <param name="records">the records.</param>
        public ExcitationForceCombination(string name, double dT, List<ExcitationForceLoadCase> records)
        {
            this.Name = name;
            this.dT = dT;
            this.records = records;
        }
    }

    /// <summary>
    /// Represents a Excitation Force Load Case.
    /// </summary>
    [System.Serializable]
    public partial class ExcitationForceLoadCase
    {
        /// <summary>
        /// Gets or sets the load case guid.
        /// </summary>
        [XmlAttribute("load_case")]
        public Guid _loadCaseGuid { get; set; }

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
                _loadCaseGuid = value.Guid;
            }
        }


        /// <summary>
        /// Gets or sets the diagram name.
        /// </summary>
        [XmlAttribute("diagram")]
        public string _diagramName { get; set; }

        /// <summary>
        /// Gets or sets the diagram.
        /// </summary>
        [XmlIgnore]
        public Diagram _diagram { get; set; }

        [XmlIgnore]
        public Diagram Diagram
        {
            get { return _diagram; }
            set
            {
                _diagram = value;
                _diagramName = value.Name;
            }
        }

        /// <summary>
        /// Gets or sets the force.
        /// </summary>
        [XmlAttribute("factor")]
        public double Force { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ExcitationForceLoadCase"/> class.
        /// </summary>
        public ExcitationForceLoadCase() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcitationForceLoadCase"/> class.
        /// </summary>
        /// <param name="loadCase">the load case.</param>
        /// <param name="force">the force.</param>
        /// <param name="diagram">the diagram.</param>
        public ExcitationForceLoadCase(LoadCase loadCase, double force, Diagram diagram)
        {
            this.LoadCase = loadCase;
            this.Force = force;
            this.Diagram = diagram;
        }
    }

    /// <summary>
    /// Represents a Diagram.
    /// </summary>
    [System.Serializable]
    public partial class Diagram
    {
        /// <summary>
        /// Gets or sets the records.
        /// </summary>
        [XmlElement("record")]
        public List<TimeHistoryDiagram> _records;

        [XmlIgnore]
        public List<TimeHistoryDiagram> Records
        {
            get
            {
                // add element to the first index of _records using system.linq
                return _records.Prepend(new TimeHistoryDiagram(0, _startValue)).ToList();
            }
            set
            {
                if (value[0].Time != 0)
                {
                    throw new System.ArgumentException("First record must have time = 0");
                }

                _startValue = value[0].Value;
                _records = value.Skip(1).ToList();

            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        [XmlAttribute("direction")]
        public Direction Direction { get; set; } = Direction.Horizontal;

        /// <summary>
        /// Gets or sets the start value.
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("start_value")]
        public double _startValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Diagram"/> class.
        /// </summary>
        public Diagram() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Diagram"/> class.
        /// </summary>
        /// <param name="name">the name.</param>
        /// <param name="records">the records.</param>
        public Diagram(string name, List<TimeHistoryDiagram> records)
        {
            this.Name = name;
            this.Records = records;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Diagram"/> class.
        /// </summary>
        /// <param name="name">the name.</param>
        /// <param name="times">the times.</param>
        /// <param name="values">the values.</param>
        public Diagram(string name, List<double> times, List<double> values)
        {
            Name = name;
            if (times.Count != values.Count)
                throw new System.ArgumentException("Time and values must have the same length.");
            Records = times.Zip(values, (t, v) => new TimeHistoryDiagram(t, v)).ToList();
        }

    }

    /// <summary>
    /// Represents a Time History Diagram.
    /// </summary>
    [System.Serializable]
    public partial class TimeHistoryDiagram
    {
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        [XmlAttribute("T")]
        public double Time { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [XmlAttribute("value")]
        [Browsable(false)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double _value;

        [XmlIgnore]
        public double Value
        {
            get { return _value; }
            set
            {
                if (value < -1.00 || value > 1.00)
                {
                    throw new System.ArgumentException("Value must be in the range -1.00 to 1.00");
                }
                _value = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeHistoryDiagram"/> class.
        /// </summary>
        public TimeHistoryDiagram() { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeHistoryDiagram"/> class.
        /// </summary>
        /// <param name="time">the time.</param>
        /// <param name="value">the value.</param>
        public TimeHistoryDiagram(double time, double value)
        {
            this.Time = time;
            this.Value = value;
        }
    }

    /// <summary>
    /// Defines the Direction enumeration.
    /// </summary>
    public enum Direction
    {
        [XmlEnum("horizontal")]
        Horizontal,
        [XmlEnum("vertical")]
        Vertical,
    }

}
