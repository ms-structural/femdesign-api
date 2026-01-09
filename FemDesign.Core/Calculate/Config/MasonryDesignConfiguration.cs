using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>
    /// Masonry configuration
    /// </summary>
    [System.Serializable]
    public partial class MasonryConfig : CONFIG
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute("type")]
        public string Type = "CCMSCONFIG";

        /// <summary>
        /// Gets or sets the ignore annex for shear strength.
        /// </summary>
        [XmlAttribute("fIgnoreAnnexForShearStrength")]
        public int _ignoreAnnexForShearStrength = 0;

        [XmlIgnore]
        public bool IgnoreAnnexForShearStrength
        {
            get
            {
                return System.Convert.ToBoolean(this._ignoreAnnexForShearStrength);
            }
            set
            {
                this._ignoreAnnexForShearStrength = System.Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Stripe width for masonry [m]
        /// </summary>
        [XmlAttribute("StripeWidth")]
        public double StripeWidth { get; set; } = 1.0;

        private MasonryConfig()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MasonryConfig"/> class.
        /// </summary>
        /// <param name="ignoreShearStrength">the ignore shear strength.</param>
        /// <param name="stripeWidth">the stripe width.</param>
        public MasonryConfig(bool ignoreShearStrength, double stripeWidth)
        {
            IgnoreAnnexForShearStrength = ignoreShearStrength;
            StripeWidth = stripeWidth;
        }
    }
}
