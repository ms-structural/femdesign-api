using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>
    /// Represents a Imperfection.
    /// </summary>
    public partial class Imperfection : Stability
    {
        /// <summary>
        /// Gets or sets the initialise amplitude.
        /// </summary>
        [XmlIgnore]
        public bool InitialiseAmplitude { get; set; } = true;

        private Imperfection() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Imperfection"/> class.
        /// </summary>
        /// <param name="loadCombinations">the load combinations.</param>
        /// <param name="numShapes">the num shapes.</param>
        /// <param name="positiveOnly">the positive only.</param>
        /// <param name="numberIteration">the number iteration.</param>
        /// <param name="initialiseAmplitude">the initialise amplitude.</param>
        public Imperfection(List<string> loadCombinations, List<int> numShapes, bool positiveOnly = false, int numberIteration = 5, bool initialiseAmplitude = true) : base(loadCombinations, numShapes, positiveOnly, numberIteration)
        {
            this.InitialiseAmplitude = initialiseAmplitude;
        }
    }
}
