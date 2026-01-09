// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{

    /// <summary>
    /// fdscript.xsd
    /// </summary>
    public partial class Comb
    {
        /// <summary>
        /// Maximum iteration number
        /// </summary>
        [XmlAttribute("NLEmaxiter")]
        public int NLEmaxiter { get; set; } = 30;
        
        /// <summary>
        /// Deafult load step in % of the total load
        /// </summary>
        [XmlAttribute("PLdefloadstep")]
        public int PLdefloadstep { get; set; } = 20;

        /// <summary>
        /// Minimum load step [%]
        /// </summary>
        [XmlAttribute("PLminloadstep")]
        public int PLminloadstep { get; set; } = 2;

        /// <summary>
        /// Keep reduced load step after it has been reduced by the solver
        /// </summary>
        [XmlAttribute("PlKeepLoadStep")]
        public bool PlKeepLoadStep { get; set; } = true;

        /// <summary>
        /// Global tolerance value [‰]
        /// </summary>
        [XmlAttribute("PlTolerance")]
        public int PlTolerance { get; set; } = 1;

        /// <summary>
        /// Maximum equilibrium iteration number
        /// </summary>
        [XmlAttribute("PLmaxeqiter")]
        public int PLmaxeqiter { get; set; } = 50;

        /// <summary>
        /// Number of layers in the elasto-plastic shells
        /// </summary>
        [XmlAttribute("PlShellLayers")]
        public int PlShellLayers { get; set; } = 10;
        
        /// <summary>
        /// Consider Mohr-Coulomb criteria
        /// </summary>
        [XmlAttribute("NLSMohr")]
        public bool NLSMohr { get; set; } = true;
        
        /// <summary>
        /// Initial load step [%]
        /// </summary>
        [XmlAttribute("NLSinitloadstep")]
        public int NLSinitloadstep { get; set; } = 10;
        
        /// <summary>
        /// Minimum load step [%]
        /// </summary>
        [XmlAttribute("NLSminloadstep")]
        public int NLSminloadstep { get; set; } = 10;
        
        /// <summary>
        /// Volume ratio of nonlinearly active elements in one step [%]
        /// </summary>
        [XmlAttribute("NLSactiveelemratio")]
        public int NLSactiveelemratio { get; set; } = 5;
        
        /// <summary>
        /// Volume ratio of plastic elements in one step [%]
        /// </summary>
        [XmlAttribute("NLSplasticelemratio")]
        public int NLSplasticelemratio { get; set; } = 5;

        /// <summary>
        /// One load step in % of the total load
        /// </summary>
        [XmlAttribute("CRloadstep")]
        public int CRloadstep { get; set; } = 20;

        /// <summary>
        /// Maximum iteration number
        /// </summary>
        [XmlAttribute("CRmaxiter")]
        public int CRmaxiter { get; set; } = 30;
        
        /// <summary>
        /// Allowed stiffness change error [%]
        /// </summary>
        [XmlAttribute("CRstifferror")]
        public int CRstifferror { get; set; } = 2;

        /// <summary>
        /// Gets or sets the comb item.
        /// </summary>
        [XmlElement("combitem")]
        public List<CombItem> CombItem { get; set; } = new List<CombItem>();
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Comb()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Comb"/> class.
        /// </summary>
        /// <param name="nLEmaxiter">value for <paramref name="nLEmaxiter"/>.</param>
        /// <param name="pLdefloadstep">value for <paramref name="pLdefloadstep"/>.</param>
        /// <param name="pLminloadstep">value for <paramref name="pLminloadstep"/>.</param>
        /// <param name="plKeepLoadStep">the pl keep load step.</param>
        /// <param name="plTolerance">the pl tolerance.</param>
        /// <param name="pLmaxeqiter">value for <paramref name="pLmaxeqiter"/>.</param>
        /// <param name="plShellLayers">the pl shell layers.</param>
        /// <param name="nLSMohr">value for <paramref name="nLSMohr"/>.</param>
        /// <param name="nLSinitloadstep">value for <paramref name="nLSinitloadstep"/>.</param>
        /// <param name="nLSminloadstep">value for <paramref name="nLSminloadstep"/>.</param>
        /// <param name="nLSactiveelemratio">value for <paramref name="nLSactiveelemratio"/>.</param>
        /// <param name="nLSplasticelemratio">value for <paramref name="nLSplasticelemratio"/>.</param>
        /// <param name="cRloadstep">value for <paramref name="cRloadstep"/>.</param>
        /// <param name="cRmaxiter">value for <paramref name="cRmaxiter"/>.</param>
        /// <param name="cRstifferror">value for <paramref name="cRstifferror"/>.</param>
        /// <param name="combItem">the comb item.</param>
        public Comb(int nLEmaxiter = 30, int pLdefloadstep = 20, int pLminloadstep = 2, bool plKeepLoadStep = true, int plTolerance = 1, int pLmaxeqiter = 50, int plShellLayers = 10, bool nLSMohr = true, int nLSinitloadstep = 10, int nLSminloadstep = 10, int nLSactiveelemratio = 5, int nLSplasticelemratio = 5, int cRloadstep = 20, int cRmaxiter = 30, int cRstifferror = 2, List<CombItem> combItem = null)
        {
            NLEmaxiter = nLEmaxiter;
            PLdefloadstep = pLdefloadstep;
            PLminloadstep = pLminloadstep;
            PlKeepLoadStep = plKeepLoadStep;
            PlTolerance = plTolerance;
            PLmaxeqiter = pLmaxeqiter;
            PlShellLayers = plShellLayers;
            NLSMohr = nLSMohr;
            NLSinitloadstep = nLSinitloadstep;
            NLSminloadstep = nLSminloadstep;
            NLSactiveelemratio = nLSactiveelemratio;
            NLSplasticelemratio = nLSplasticelemratio;
            CRloadstep = cRloadstep;
            CRmaxiter = cRmaxiter;
            CRstifferror = cRstifferror;
            CombItem = combItem ?? new List<CombItem>();
        }



        /// <summary>
        /// Set default calculation parameters for load combinations.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        public static Comb Default()
        {
            return new Comb();
        }
    }
}