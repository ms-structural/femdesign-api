using System;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FemDesign.GenericClasses;

namespace FemDesign
{
    /// <summary>
    /// Stage
    /// </summary>
    [System.Serializable]
    public partial class ConstructionStages
    {
        /// <summary>
        /// Gets or sets the last change.
        /// </summary>
        [XmlAttribute("last_change")]
        public string _lastChange;
        [XmlIgnore]
        internal DateTime LastChange
        {
            get
            {
                return DateTime.Parse(this._lastChange);
            }
            set
            {
                this._lastChange = value.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
            }
        }
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        [XmlAttribute("action")]
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the assign modified element.
        /// </summary>
        [XmlAttribute("auto-assign_modified_elements")]
        public bool AssignModifiedElement { get; set; } = false;

        /// <summary>
        /// Gets or sets the assign new element.
        /// </summary>
        [XmlAttribute("auto-assign_newly_created_elements")]
        public bool AssignNewElement { get; set; } = false;

        /// <summary>
        /// Gets or sets the ghost method.
        /// </summary>
        [XmlAttribute("ghost_method")]
        public bool GhostMethod { get; set; } = false;

        /// <summary>
        /// Gets or sets the time dependent analysis.
        /// </summary>
        [XmlAttribute("time-dependent_analysis")]
        public bool TimeDependentAnalysis { get; set; } = false;

        /// <summary>
        /// Gets or sets the creep strain increment limit.
        /// </summary>
        [XmlAttribute("creep_strain_increment_limit")]
        public double _creepStrainIncrementLimit { get; set; } = 0.25;  // struxml attribute

        /// <summary>
        /// creep_strain_increment_limit [thousand percent]
        /// </summary>
        [XmlIgnore]
        public double CreepStrainIncrementLimit
        {
            get
            {
                return _creepStrainIncrementLimit;
            }
            set
            {
                _creepStrainIncrementLimit = FemDesign.RestrictedDouble.ValueInHalfClosedInterval(value, 0.0, 10.0);
            }
        }

        /// <summary>
        /// Gets or sets the stages.
        /// </summary>
        [XmlElement("stage")]
        public List<Stage> Stages { get; set; } = new List<Stage>();


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public ConstructionStages()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructionStages"/> class.
        /// </summary>
        /// <param name="stages">List of construction stages.</param>
        /// <param name="assignModifedElement">If <c>true</c>, assigns modified elements to stages.</param>
        /// <param name="assignNewElement">If <c>true</c>, assigns new elements to stages.</param>
        public ConstructionStages(List<Stage> stages, bool assignModifedElement = false, bool assignNewElement = false)
        {
            // it does not required the Guid
            this.LastChange = DateTime.UtcNow;
            this.Action = "added";
            this.Stages = ConstructionStages.SortStages(stages);
            this.AssignModifiedElement = assignModifedElement;
            this.AssignNewElement = assignNewElement;
        }

        private static List<Stage> SortStages(List<Stage> stages)
        {
            var orderedStages = stages.OrderBy(x => x.Id).ToList();
            return orderedStages;
        }

    }
}
