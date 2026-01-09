// https://strusoft.com/
using System;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>    
    /// fdscript.xsd
    /// </summary>
    public partial class Stage
    {
        /// <summary>
        /// Gets or sets the ghost.
        /// </summary>
        [XmlAttribute("ghost")]
        public bool _ghost { get; set; }

        [XmlIgnore]
        public bool Ghost
        {
            get { return _ghost; }
            set { _ghost = value; }
        }

        /// <summary>
        /// Gets or sets the time dependent analysis.
        /// </summary>
        [XmlAttribute("tda")]
        public bool _timeDependentAnalysis { get; set; }
        [XmlIgnore]
        public bool TimeDependentAnalysis
        {
            get { return _timeDependentAnalysis; }
            set { _timeDependentAnalysis = value; }
        }

        /// <summary>
        /// Gets or sets the creep increment limit.
        /// </summary>
        [XmlAttribute("creepincrementlimit")]
        public double _creepIncrementLimit { get; set; } = 0.25;

        /// <summary>
        /// creep_strain_increment_limit [thousand percent]
        /// </summary>
        [XmlIgnore]
        public double? CreepIncrementLimit
        {
            get
            {
                return _creepIncrementLimit;
            }
            set
            {
                var creep = (double)value;
                _creepIncrementLimit = FemDesign.RestrictedDouble.ValueInHalfClosedInterval(creep, 0.0, 10.0);
            }
        }


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Stage()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stage"/> class.
        /// </summary>
        /// <param name="ghost">the ghost.</param>
        /// <param name="timeDependent">the time dependent.</param>
        /// <param name="creepLimit">the creep limit.</param>
        public Stage(bool ghost = false, bool timeDependent = false, double? creepLimit = null)
        {
            this.Ghost = ghost;
            this.CreepIncrementLimit = creepLimit == null ? 2.5 : creepLimit;
            this.TimeDependentAnalysis = timeDependent;
        }

        /// <summary>
        /// Default Construction stages method (incremental).
        /// </summary>
        /// <returns>The result.</returns>
        public static Stage Default()
        {
            return new Stage(false);
        }
        /// <summary>
        /// Construction stages method.
        /// </summary>
        /// <param name="ghost">Ghost construction method. True/false. If false incremental method is used.</param>
        /// <returns>The result.</returns>
        public static Stage Define(bool ghost = false)
        {
            return new Stage(ghost);
        }

        /// <summary>
        /// Incremental construction stage method.
        /// </summary>
        /// <returns></returns>
        public static Stage GhostMethod()
        {
            return new Stage(true);
        }

        /// <summary>
        /// Ghost construction stage method.
        /// </summary>
        /// <returns></returns>
        public static Stage TrackingMethod()
        {
            return new Stage(false);
        }
    }
}