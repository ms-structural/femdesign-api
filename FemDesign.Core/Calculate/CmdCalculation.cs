// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;

namespace FemDesign.Calculate
{

    /// <summary>
    /// fdscript.xsd
    /// CMDCALCULATION
    /// </summary>
    [XmlRoot("cmdcalculation")]
    [System.Serializable]
    public partial class CmdCalculation : CmdCommand
    {
        /// <summary>
        /// Gets or sets the analysis.
        /// </summary>
        [XmlElement("analysis")]
        public Analysis Analysis { get; set; } // ANALYSIS
        /// <summary>
        /// Gets or sets the design.
        /// </summary>
        [XmlElement("design")]
        public Design Design { get; set; } // DESIGNCALC
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        [XmlAttribute("command")]
        public string Command = "; CXL $MODULE CALC"; // token

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public CmdCalculation()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CmdCalculation"/> class.
        /// </summary>
        /// <param name="analysis">the analysis.</param>
        public CmdCalculation(Analysis analysis)
        {
            this.Analysis = analysis;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdCalculation"/> class.
        /// </summary>
        /// <param name="analysis">the analysis.</param>
        /// <param name="design">the design.</param>
        public CmdCalculation(Analysis analysis, Design design)
        {
            this.Analysis = analysis;
            this.Design = design;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdCalculation"/> class.
        /// </summary>
        /// <param name="design">the design.</param>
        public CmdCalculation(Design design)
        {
            this.Design = design;
        }

        /// <summary>
        /// To X Element.
        /// </summary>
        /// <returns>The result.</returns>
        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdCalculation>(this);
        }
    }


}