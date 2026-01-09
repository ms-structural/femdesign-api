// https://strusoft.com/

using System.Xml.Serialization;
using System.Collections.Generic;

namespace FemDesign.Materials
{
    /// <summary>
    /// material_type --> steel
    /// </summary>
    [System.Serializable]
    public partial class Steel: MaterialBase
    {
        /// <summary>
        /// Gets or sets the creep time dependant.
        /// </summary>
        [XmlElement("tda_creep")]
        public StruSoft.Interop.StruXml.Data.Tda_creep1 CreepTimeDependant { get; set; }

        /// <summary>
        /// Gets or sets the plasticity.
        /// </summary>
        [XmlElement("plastic_analysis_data")]
        public StruSoft.Interop.StruXml.Data.Steel_pl_data Plasticity { get; set; }

        /// <summary>
        /// Gets or sets the fyk16.
        /// </summary>
        [XmlAttribute("Fyk16")]
        public double Fyk16 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fyk40.
        /// </summary>
        [XmlAttribute("Fyk40")]
        public double Fyk40 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fyk63.
        /// </summary>
        [XmlAttribute("Fyk63")]
        public double Fyk63 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fyk80.
        /// </summary>
        [XmlAttribute("Fyk80")]
        public double Fyk80 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fyk100.
        /// </summary>
        [XmlAttribute("Fyk100")]
        public double Fyk100 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fyk150.
        /// </summary>
        [XmlAttribute("Fyk150")]
        public double Fyk150 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fyk200.
        /// </summary>
        [XmlAttribute("Fyk200")]
        public double Fyk200 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fyk250.
        /// </summary>
        [XmlAttribute("Fyk250")]
        public double Fyk250 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fyk400.
        /// </summary>
        [XmlAttribute("Fyk400")]
        public double Fyk400 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fuk3.
        /// </summary>
        [XmlAttribute("Fuk3")]
        public double Fuk3 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fuk40.
        /// </summary>
        [XmlAttribute("Fuk40")]
        public double Fuk40 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fuk100.
        /// </summary>
        [XmlAttribute("Fuk100")]
        public double Fuk100 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fuk150.
        /// </summary>
        [XmlAttribute("Fuk150")]
        public double Fuk150 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fuk250.
        /// </summary>
        [XmlAttribute("Fuk250")]
        public double Fuk250 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fuk400.
        /// </summary>
        [XmlAttribute("Fuk400")]
        public double Fuk400 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the gamma m0 0.
        /// </summary>
        [XmlAttribute("gammaM0_0")]
        public double gammaM0_0 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the gamma m0 1.
        /// </summary>
        [XmlAttribute("gammaM0_1")]
        public double gammaM0_1 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the gamma m1 0.
        /// </summary>
        [XmlAttribute("gammaM1_0")]
        public double gammaM1_0 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the gamma m1 1.
        /// </summary>
        [XmlAttribute("gammaM1_1")]
        public double gammaM1_1 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the gamma m2 0.
        /// </summary>
        [XmlAttribute("gammaM2_0")]
        public double gammaM2_0 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the gamma m2 1.
        /// </summary>
        [XmlAttribute("gammaM2_1")]
        public double gammaM2_1 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the gamma m5 0.
        /// </summary>
        [XmlAttribute("gammaM5_0")]
        public double gammaM5_0 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the gamma m5 1.
        /// </summary>
        [XmlAttribute("gammaM5_1")]
        public double gammaM5_1 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the gamma mfi.
        /// </summary>
        [XmlAttribute("gammaMfi")]
        public double gammaMfi { get; set; } = 1.0; // material_base_value
        /// <summary>
        /// Gets or sets the ek.
        /// </summary>
        [XmlAttribute("Ek")]
        public string Ek { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the ed 0.
        /// </summary>
        [XmlAttribute("Ed_0")]
        public string Ed_0 { get; set; } // double
        /// <summary>
        /// Gets or sets the ed 1.
        /// </summary>
        [XmlAttribute("Ed_1")]
        public string Ed_1 { get; set; } // double
        /// <summary>
        /// Gets or sets the nu.
        /// </summary>
        [XmlAttribute("nu")]
        public string nu { get; set; } // material_nu_value
        /// <summary>
        /// Gets or sets the g.
        /// </summary>
        [XmlAttribute("G")]
        public string G { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the alfa.
        /// </summary>
        [XmlAttribute("alfa")]
        public string alfa { get; set; } // material_base_value

        /// <summary>
        /// Set the plasticity parameters for the steel material.
        /// </summary>
        /// <param name="plastic"></param>
        /// <param name="strainLimit"></param>
        /// <exception cref="System.ArgumentException"></exception>
        public void SetPlasticity(bool plastic = true, double strainLimit = 2.5)
        {
            // create a list with 4 times plastic value
            var plasticList = new List<bool> { plastic, plastic, plastic, plastic };
            var strainLimitList = new List<double> { strainLimit, strainLimit, strainLimit, strainLimit };

            SetPlasticity(plasticList, strainLimitList);
        }


        /// <summary>
        /// The method SetPlasticity is used to set the plasticity parameters for the steel material.
        /// The list must contain 1 or 4 values. The first value is used to set the plasticity for U, the second for Sq, the third for Sf and the fourth for Sc.
        /// </summary>
        /// <param name="plastic"></param>
        /// <param name="strainLimit"></param>
        /// <exception cref="System.ArgumentException"></exception>
        public void SetPlasticity(List<bool> plastic, List<double> strainLimit)
        {
            if(plastic.Count != 4 && strainLimit.Count != 4)
                throw new System.ArgumentException("Both list must contain 4 values.");

            this.Plasticity.Elasto_plastic_behaviour_U = plastic[0];
            this.Plasticity.Elasto_plastic_behaviour_Sq = plastic[1];
            this.Plasticity.Elasto_plastic_behaviour_Sf = plastic[2];
            this.Plasticity.Elasto_plastic_behaviour_Sc = plastic[3];


            // set U plastic data
            if (strainLimit[0] == 0)
            {
                this.Plasticity.Elasto_plastic_strain_limit_U = false;
                this.Plasticity.Elasto_plastic_strain_limit_option_U = 2.5; // this value will not be used
            }
            else if (strainLimit[0] > 0 && strainLimit[0] < 50)
            {
                this.Plasticity.Elasto_plastic_strain_limit_U = true;
                this.Plasticity.Elasto_plastic_strain_limit_option_U = strainLimit[0];
            }
            else
                throw new System.ArgumentException("Strain limit must be in range 0.00 - 50.00");

            // set Sq plastic data
            if (strainLimit[1] == 0)
            {
                this.Plasticity.Elasto_plastic_strain_limit_Sq = false;
                this.Plasticity.Elasto_plastic_strain_limit_option_Sq = 2.5; // this value will not be used
            }
            else if (strainLimit[1] > 0 && strainLimit[1] < 50)
            {
                this.Plasticity.Elasto_plastic_strain_limit_Sq = true;
                this.Plasticity.Elasto_plastic_strain_limit_option_Sq = strainLimit[1];
            }
            else
                throw new System.ArgumentException("Strain limit must be in range 0.00 - 50.00");

            // Set Sf plastic data
            if (strainLimit[2] == 0)
            {
                this.Plasticity.Elasto_plastic_strain_limit_Sf = false;
                this.Plasticity.Elasto_plastic_strain_limit_option_Sf = 2.5; // this value will not be used
            }
            else if (strainLimit[2] > 0 && strainLimit[2] < 50)
            {
                this.Plasticity.Elasto_plastic_strain_limit_Sf = true;
                this.Plasticity.Elasto_plastic_strain_limit_option_Sf = strainLimit[2];
            }
            else
                throw new System.ArgumentException("Strain limit must be in range 0.00 - 50.00");

            // Set Sc plastic data
            if (strainLimit[3] == 0)
            {
                this.Plasticity.Elasto_plastic_strain_limit_Sc = false;
                this.Plasticity.Elasto_plastic_strain_limit_option_Sc = 2.5; // this value will not be used
            }
            else if (strainLimit[3] > 0 && strainLimit[3] < 50)
            {
                this.Plasticity.Elasto_plastic_strain_limit_Sc = true;
                this.Plasticity.Elasto_plastic_strain_limit_option_Sc = strainLimit[3];
            }
            else
                throw new System.ArgumentException("Strain limit must be in range 0.00 - 50.00");


        }

    }
}