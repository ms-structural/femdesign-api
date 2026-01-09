// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Load Combination Case Base.
    /// </summary>
    public class LoadCombinationCaseBase // spec_load_case_item
    {
        /// <summary>
        /// Gets or sets the gamma.
        /// </summary>
        [XmlAttribute("gamma")]
        public double Gamma;

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        protected LoadCombinationCaseBase() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadCombinationCaseBase"/> class.
        /// </summary>
        /// <param name="gamma">the gamma.</param>
        public LoadCombinationCaseBase(double gamma)
        {
            Gamma = gamma;
        }
    }
}