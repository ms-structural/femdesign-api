using FemDesign.GenericClasses;
using FemDesign.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>
    /// Represents a Concrete Design Config.
    /// </summary>
    [System.Serializable]
    public partial class ConcreteDesignConfig : CONFIG
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute("type")]
        public string Type = "ECRCCONFIG";

        /// <summary>
        /// Defines the Calculation Method enumeration.
        /// </summary>
        public enum CalculationMethod
        {
            [XmlEnum("0")]
            [Parseable("NominalStiffness", "nominal_stiffness", "Nominal_stiffness", "NOMINAL_STIFFNESS")]
            NominalStiffness,
            [XmlEnum("1")]
            [Parseable("NominalCurvature", "nominal_curvature", "Nominal_curvature", "NOMINAL_CURVATURE")]
            NominalCurvature
        }

        /// <summary>
        /// Gets or sets the second order calculation method.
        /// </summary>
        [XmlAttribute("s2ndOrder")]
        public CalculationMethod SecondOrderCalculationMethod { get; set; } = ConcreteDesignConfig.CalculationMethod.NominalStiffness;

        /// <summary>
        /// Crack with load combinations
        /// </summary>
        [XmlAttribute("crackw_q")]
        public bool CrackWidthQuasiPermanent { get; set; } = true;

        /// <summary>
        /// Crack with load combinations
        /// </summary>
        [XmlAttribute("crackw_f")]
        public bool CrackWidthFrequent { get; set; } = false;

        /// <summary>
        /// Crack with load combinations
        /// </summary>
        [XmlAttribute("crackw_c")]
        public bool CrackWidthCharacteristic { get; set; } = false;

        /// <summary>
        /// Cracking calculation
        /// </summary>
        [XmlAttribute("fReopeningCracks")]
        public bool ReopeningCracks { get; set; }  = false;

        /// <summary>
        /// Use upper limit for Eq. 7.11
        /// </summary>
        [XmlAttribute("fUseUpperLimitForEq711")]
        public bool UseUpperLimitForEq711 { get; set; } = false;


        /// <summary>
        /// Concrete design configuration
        /// </summary>
        public ConcreteDesignConfig()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcreteDesignConfig"/> class.
        /// </summary>
        /// <param name="secondOrder">the second order.</param>
        /// <param name="crackQuasiPermanent">the crack quasi permanent.</param>
        /// <param name="crackFrequent">the crack frequent.</param>
        /// <param name="crackCharacteristic">the crack characteristic.</param>
        /// <param name="reopeningCracks">the reopening cracks.</param>
        /// <param name="useUpperLimitForEq711">the use upper limit for eq711.</param>
        public ConcreteDesignConfig(CalculationMethod secondOrder, bool crackQuasiPermanent = true, bool crackFrequent = false, bool crackCharacteristic = false, bool reopeningCracks = false, bool useUpperLimitForEq711 = false)
        {
            SecondOrderCalculationMethod = secondOrder;
            CrackWidthQuasiPermanent = crackQuasiPermanent;
            CrackWidthFrequent = crackFrequent;
            CrackWidthCharacteristic = crackCharacteristic;
            ReopeningCracks = reopeningCracks;
            UseUpperLimitForEq711 = useUpperLimitForEq711;
        }

        /// <summary>
        /// Nominal Stiffness.
        /// </summary>
        /// <param name="crackQuasiPermanent">the crack quasi permanent.</param>
        /// <param name="crackFrequent">the crack frequent.</param>
        /// <param name="crackCharacteristic">the crack characteristic.</param>
        /// <param name="useUpperLimitForEq711">the use upper limit for eq711.</param>
        /// <returns>The result.</returns>
        public static ConcreteDesignConfig NominalStiffness(bool crackQuasiPermanent = true, bool crackFrequent = false, bool crackCharacteristic = false, bool useUpperLimitForEq711 = false)
        {
            return new ConcreteDesignConfig(CalculationMethod.NominalStiffness, crackQuasiPermanent, crackFrequent, crackCharacteristic, useUpperLimitForEq711: useUpperLimitForEq711);
        }

        /// <summary>
        /// Nominal Curvature.
        /// </summary>
        /// <param name="crackQuasiPermanent">the crack quasi permanent.</param>
        /// <param name="crackFrequent">the crack frequent.</param>
        /// <param name="crackCharacteristic">the crack characteristic.</param>
        /// <param name="useUpperLimitForEq711">the use upper limit for eq711.</param>
        /// <returns>The result.</returns>
        public static ConcreteDesignConfig NominalCurvature(bool crackQuasiPermanent = true, bool crackFrequent = false, bool crackCharacteristic = false, bool useUpperLimitForEq711 = false)
        {
            return new ConcreteDesignConfig(CalculationMethod.NominalCurvature, crackQuasiPermanent, crackFrequent, crackCharacteristic, useUpperLimitForEq711: useUpperLimitForEq711);
        }

    }
}
