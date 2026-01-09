using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign.GenericClasses;
using FemDesign.Loads;

namespace FemDesign
{
    /// <summary>
    /// Represents a Stage.
    /// </summary>
    [System.Serializable]
    public partial class Stage
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [XmlIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the initial stress state.
        /// </summary>
        [XmlAttribute("initial_stress_state")]
        public bool InitialStressState { get; set; } = false;

        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        [XmlAttribute("day")]
        public double _day { get; set; }

        /// <summary>
        /// End time of the stage [day]
        /// </summary>
        [XmlIgnore]
        public double Day
        {
            get { return _day; }
            set { _day = FemDesign.RestrictedDouble.NonNegMax_1e20(value);  }
        }

        /// <summary>
        /// Gets or sets the activated load cases.
        /// </summary>
        [XmlElement("activated_load_case")]
        public List<ActivatedLoadCase> ActivatedLoadCases { get; set; }

        /// <summary>
        /// Gets or sets the elements.
        /// </summary>
        [XmlIgnore]
        public List<IStageElement> Elements { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Stage()
        {

        }

        /// <summary>
        /// Construction stage.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="description">Stage description (name).</param>
        /// <param name="activatedLoadCases">LoadCases with factor and partitioning for when to be activated.</param>
        /// <param name="elements">Elements to be activated in this stage.</param>
        /// <param name="initialStressState">Initial stress state.</param>
        /// <param name="day">End time of the stage.</param>
        public Stage(int index, string description, List<ActivatedLoadCase> activatedLoadCases = null, List<IStageElement> elements = null, bool initialStressState = false, double day = 0.0)
        {
            Initialize(index, description, activatedLoadCases, elements, initialStressState, day);
        }

        /// <summary>
        /// Construction stage.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="description">Stage description (name).</param>
        /// <param name="loadCases">LoadCases to be activated (with factor 1.0).</param>
        /// <param name="elements">Elements to be activated in this stage.</param>
        /// <param name="partitioning">Partitioning for when to activate load cases.</param>
        /// <param name="initialStressState">Initial stress state.</param>
        /// <param name="day">End time of the stage.</param>
        public Stage(int index, string description, List<LoadCase> loadCases, List<IStageElement> elements, ActivationType partitioning = ActivationType.OnlyInThisStage, bool initialStressState = false, double day = 0.0)
        {
            var activatedLoadCase = loadCases.Select(l => new ActivatedLoadCase(l, 1.0, partitioning)).ToList();
            Initialize(index, description, activatedLoadCase, elements, initialStressState, day);
        }
        private void Initialize(int index, string description, List<ActivatedLoadCase> activatedLoadCases, List<IStageElement> elements, bool initialStressState, double day)
        {
            if (index <= 0)
            {
                throw new ArgumentException("index must be >= 1");
            }
            this.Id = index;
            this.Description = description;
            this.ActivatedLoadCases = activatedLoadCases;
            this.Elements = elements;
            this.InitialStressState = initialStressState;
            this.Day = day;
        }

        /// <summary>
        /// Add element to construction stage.
        /// </summary>
        /// <param name="element">Element to be activated in this stage.</param>
        public void AddElement(IStageElement element)
        {
            this.Elements.Add(element);
        }

        /// <summary>
        /// Adds a (construction stage) activated load case.
        /// </summary>
        /// <param name="loadCase">the load case.</param>
        /// <param name="factor">Load case factor.</param>
        /// <param name="partitioning">Partitioning.</param>
        public void AddLoadCase(LoadCase loadCase, double factor, ActivationType partitioning)
        {
            AddLoadCase(new ActivatedLoadCase(loadCase, factor, partitioning));
        }

        /// <summary>
        /// Adds a (construction stage) activated load case.
        /// </summary>
        /// <param name="activatedLoadCase">the activated load case.</param>
        public void AddLoadCase(ActivatedLoadCase activatedLoadCase)
        {
            this.ActivatedLoadCases.Add(activatedLoadCase);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"Stage {this.Description}";
        }

    }
}
