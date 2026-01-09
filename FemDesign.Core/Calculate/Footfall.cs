// https://strusoft.com/
using System;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// Represents a Footfall.
    /// </summary>
    public partial class Footfall
    {
        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        [XmlAttribute("TopOfSubstructure")]
        public double Top { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Footfall()
        {

        }

        /// <summary>
        /// Define calculation parameters for a footfall calculation.
        /// </summary>
        /// <param name="top">Top of substructure. Masses on this level and below are not considered in Footfall calculation.</param>
        public Footfall(double top)
        {
            this.Top = top;
        }

        /// <summary>
        /// Define default calculation parameters for a footfall calculation.
        /// </summary>
        /// <returns></returns>
        public static Footfall Default()
        {
            return new Footfall(-0.01);
        }

    }
}