// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Materials
{
    /// <summary>
    /// rfmaterial_type --> reinforcing_steel.
    /// </summary>
    [System.Serializable]
    public partial class ReinforcingSteel
    {
        /// <summary>
        /// Gets or sets the fyk.
        /// </summary>
        [XmlAttribute("fyk")]
        public string Fyk {get; set;} // non_neg_max_1e10
        /// <summary>
        /// Gets or sets the es.
        /// </summary>
        [XmlAttribute("Es")]
        public string Es {get; set;} // non_neg_max_1e10
        /// <summary>
        /// Gets or sets the epsilon uk.
        /// </summary>
        [XmlAttribute("Epsilon_uk")]
        public string Epsilon_uk {get; set;} // non_neg_max_1e10
        /// <summary>
        /// Gets or sets the epsilon ud.
        /// </summary>
        [XmlAttribute("Epsilon_ud")]
        public string Epsilon_ud {get; set;} // non_neg_max_1e10
        /// <summary>
        /// Gets or sets the k.
        /// </summary>
        [XmlAttribute("k")]
        public string k {get; set;} // rc_k_value

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ReinforcingSteel()
        {

        }
    }
}