// https://strusoft.com/
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace FemDesign.Calculate
{
    /// <summary>
    /// Represents a Stability.
    /// </summary>
    public partial class Stability
    {

        /// <summary>
        /// Gets or sets the comb names.
        /// </summary>
        [XmlIgnore]
        public List<string> CombNames { get; set; }

        /// <summary>
        /// Gets or sets the num shapes.
        /// </summary>
        [XmlIgnore]
        public List<int> NumShapes { get; set; }

        /// <summary>
        /// Gets or sets the positive only.
        /// </summary>
        [XmlIgnore]
        public bool PositiveOnly { get; set; }

        /// <summary>
        /// Gets or sets the number iteration.
        /// </summary>
        [XmlIgnore]
        public int numberIteration { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Stability()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stability"/> class.
        /// </summary>
        /// <param name="loadCombinations">the load combinations.</param>
        /// <param name="numShapes">the num shapes.</param>
        /// <param name="positiveOnly">the positive only.</param>
        /// <param name="numberIteration">the number iteration.</param>
        public Stability(List<string> loadCombinations, List<int> numShapes, bool positiveOnly = false, int numberIteration = 5)
        {
            if(loadCombinations.Count != numShapes.Count)
            {
                throw new System.Exception("Load combinations and number of shapes must have the same number of items!");
            }

            this.CombNames = loadCombinations;
            this.NumShapes = numShapes;
            this.PositiveOnly = positiveOnly;
            this.numberIteration = numberIteration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stability"/> class.
        /// </summary>
        /// <param name="loadCombination">the load combination.</param>
        /// <param name="numShape">the num shape.</param>
        /// <param name="positiveOnly">the positive only.</param>
        /// <param name="numberIteration">the number iteration.</param>
        public Stability(string loadCombination, int numShape, bool positiveOnly = false, int numberIteration = 5)
        {
            this.CombNames = new List<string> { loadCombination };
            this.NumShapes = new List<int> { numShape };
            this.PositiveOnly = positiveOnly;
            this.numberIteration = numberIteration;
        }

    }
}