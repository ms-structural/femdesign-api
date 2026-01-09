// https://strusoft.com/
using System.Xml.Serialization;

namespace FemDesign.Calculate
{

    /// <summary>
    /// fdscript.xsd
    /// DESIGNCALC
    /// </summary>
    public partial class Design
    {
        /// <summary>
        /// Gets or sets the c max.
        /// </summary>
        [XmlElement("cmax")]
        public string CMax { get; set; } // choice?
        /// <summary>
        /// Gets or sets the g max.
        /// </summary>
        [XmlElement("gmax")]
        public string GMax { get; set; } // choice?
        /// <summary>
        /// Gets or sets the auto design.
        /// </summary>
        [XmlElement("autodesign")]
        public bool AutoDesign { get; set; } // bool
        /// <summary>
        /// Gets or sets the check.
        /// </summary>
        [XmlElement("check")]
        public bool Check { get; set; } // bool
        /// <summary>
        /// Gets or sets the apply changes.
        /// </summary>
        [XmlIgnore]
        public bool ApplyChanges { get; set; } // bool
        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        [XmlIgnore]
        public CmdUserModule Mode { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Design()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="autoDesign"></param>
        /// <param name="check"></param>
        /// <param name="loadCombination">True if you want the design to be based on Load Combination. False if you want the design to be based on Load Group</param>
        /// <param name="applyChanges">True will force FemDesign to apply the new cross sections to the model at the end of the design process.</param>
        public Design(bool autoDesign = false, bool check = true, bool loadCombination = true, bool applyChanges = false)
        {
            if (loadCombination) this.CMax = "";
            else this.GMax = "";
            this.AutoDesign = autoDesign;
            this.Check = check;
            this.ApplyChanges = applyChanges;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Design"/> class.
        /// </summary>
        /// <param name="mode">the mode.</param>
        /// <param name="autoDesign">the auto design.</param>
        /// <param name="check">the check.</param>
        /// <param name="loadCombination">the load combination.</param>
        /// <param name="applyChanges">the apply changes.</param>
        public Design(CmdUserModule mode, bool autoDesign = false, bool check = true, bool loadCombination = true, bool applyChanges = false) : this(autoDesign, check, loadCombination, applyChanges)
        {
            this.Mode = mode;
        }
    }
}