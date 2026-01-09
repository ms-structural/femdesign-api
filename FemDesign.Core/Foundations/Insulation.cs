using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.ComponentModel;

namespace FemDesign.Foundations
{
    /// <summary>
    /// Represents a Insulation.
    /// </summary>
    public partial class Insulation
    {

        /// <summary>
        /// Gets or sets the e modulus.
        /// </summary>
        [XmlAttribute("e_modulus")]
        public double E_modulus { get; set; }

        /// <summary>
        /// Gets or sets the thickness.
        /// </summary>
        [XmlAttribute("thickness")]
        public double Thickness { get; set; }

        /// <summary>
        /// Gets or sets the density.
        /// </summary>
        [XmlAttribute("density")]
        public double Density { get; set; }

        /// <summary>
        /// Gets or sets the gamma m u.
        /// </summary>
        [XmlAttribute("gamma_m_u")]
        public double Gamma_m_u { get; set; }

        /// <summary>
        /// Gets or sets the gamma m uas.
        /// </summary>
        [XmlAttribute("gamma_m_uas")]
        public double Gamma_m_uas { get; set; }

        /// <summary>
        /// Gets or sets the limit stress.
        /// </summary>
        [XmlAttribute("limit_stress")]
        public double _limit_stress { get; set; }

        [XmlIgnore]
        public double Limit_stress
        {
            get
            {
                return  this._limit_stress;
            }
            set
            {
                _limit_stress = RestrictedDouble.ValueInClosedInterval(value, 1, 1e8);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Insulation"/> class.
        /// </summary>
        public Insulation()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Insulation"/> class.
        /// </summary>
        /// <param name="e">value for <paramref name="e"/>.</param>
        /// <param name="thickness">the thickness.</param>
        /// <param name="density">the density.</param>
        /// <param name="limitStress">the limit stress.</param>
        /// <param name="gamma_m_u">the gamma m u.</param>
        /// <param name="gamma_m_uas">the gamma m uas.</param>
        public Insulation(double e, double thickness, double density, double limitStress, double gamma_m_u = 1.2, double gamma_m_uas = 1.0)
        {
            this.E_modulus = e;
            this.Thickness = thickness;
            this.Density = density;
            this.Gamma_m_u = gamma_m_u;
            this.Gamma_m_uas = gamma_m_uas;
            this.Limit_stress = limitStress;
        }
    }
}
