// https://strusoft.com/

using System.Globalization;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Load Category.
    /// </summary>
    [System.Serializable]
    public partial class LoadCategory : EntityBase
    {
        /// <summary>
        /// Gets or sets the standard.
        /// </summary>
        [XmlAttribute("standard")]
        public string Standard { get; set; } // standardtype
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        [XmlAttribute("country")]
        public string Country { get; set; } // eurocodetype
        /// <summary>
        /// Name of Load Category.
        /// </summary>
        /// <value></value>
        [XmlAttribute("name")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the psi0.
        /// </summary>
        [XmlAttribute("psi_0")]
        public double _psi0 { get; set; }
        /// <summary>
        /// 𝜓₀ Factor for combination value of a variable action
        /// </summary>
        [XmlIgnore]
        public double Psi0
        {
            get { return this._psi0; }
            set { this._psi0 = RestrictedDouble.NonNegMax_10(value); }
        }
        /// <summary>
        /// Gets or sets the psi1.
        /// </summary>
        [XmlAttribute("psi_1")]
        public double _psi1 { get; set; }
        /// <summary>
        /// 𝜓₁ Factor for frequent value of a variable action
        /// </summary>
        public double Psi1
        {
            get { return this._psi1; }
            set { this._psi1 = RestrictedDouble.NonNegMax_10(value); }
        }
        /// <summary>
        /// Gets or sets the psi2.
        /// </summary>
        [XmlAttribute("psi_2")]
        public double _psi2 { get; set; }
        /// <summary>
        /// 𝜓₂ Factor for quasi-permanent value of a variable action
        /// </summary>
        public double Psi2
        {
            get { return this._psi2; }
            set { this._psi2 = RestrictedDouble.NonNegMax_10(value); }
        }
        /// <summary>
        /// Private constructor for serialization
        /// </summary>
        private LoadCategory()
        {

        }

        /// <summary>
        /// Creates a load category
        /// </summary>
        /// <param name="name">Name of the load category</param>
        /// <param name="psi0">𝜓₀ Factor for combination value of a variable action</param>
        /// <param name="psi1">𝜓₁ Factor for frequent value of a variable action</param>
        /// <param name="psi2">𝜓₂ Factor for quasi-permanent value of a variable action</param>
        public LoadCategory(string name, double psi0, double psi1, double psi2)
        {
            Country = "";
            Name = name;
            Psi0 = psi0;
            Psi1 = psi1;
            Psi2 = psi2;
        }
    }
}
