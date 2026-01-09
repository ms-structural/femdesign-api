using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a construction stage entry in a load combination.
    /// </summary>
    public class StageLoadCase : LoadCombinationCaseBase
    {
        /// <summary>
        /// Gets or sets the stage type.
        /// </summary>
        [XmlAttribute("type")]
        public string _stageType;

        private Stage _stage;
        /// <summary>
        /// Construction stage referenced by this load combination entry.
        /// </summary>
        [XmlIgnore]
        public Stage Stage { get { return _stage; }  set { _stage = value; _stageType = $"cs.{value.Id}"; } }

        private const string _finalStage = "final_cs";
        /// <summary>
        /// Gets a value indicating whether this entry represents the final construction stage.
        /// </summary>
        public bool IsFinalStage => _stageType == _finalStage;
        /// <summary>
        /// Gets the stage index (ID) parsed from <see cref="_stageType"/>, or <c>-1</c> for final stage.
        /// </summary>
        public int StageIndex => IsFinalStage ? -1 : int.Parse(_stageType.Substring(3));

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private StageLoadCase()
        {
        }

        /// <summary>
        /// Create a stage entry for the given construction stage.
        /// </summary>
        /// <param name="stage">Construction stage.</param>
        /// <param name="gamma">Load factor (gamma) applied to this stage entry.</param>
        public StageLoadCase(Stage stage, double gamma)
        {
            Gamma = gamma;
            Stage = stage;
        }

        /// <summary>
        /// Create an entry representing the final construction stage.
        /// </summary>
        /// <param name="gamma">Load factor (gamma) applied to the final stage entry.</param>
        /// <returns>A <see cref="StageLoadCase"/> representing the final stage.</returns>
        public static StageLoadCase FinalStage(double gamma)
        {
            return new StageLoadCase()
            {
                _stageType = _finalStage,
                Gamma = gamma
            };
        }
    }
}
