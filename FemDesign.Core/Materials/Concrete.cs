// https://strusoft.com/

using StruSoft.Interop_24;
using System.Xml.Serialization;

namespace FemDesign.Materials
{
    /// <summary>
    /// material_type --> concrete
    /// </summary>
    [System.Serializable]
    public partial class Concrete: MaterialBase
    {
        /// <summary>
        /// Gets or sets the creep time dependant.
        /// </summary>
        [XmlElement("tda_creep")]
        public StruSoft.Interop_24.Tda_creep2 CreepTimeDependant { get; set; }

        /// <summary>
        /// Gets or sets the shrinkage time dependant.
        /// </summary>
        [XmlElement("tda_shrinkage")]
        public StruSoft.Interop_24.Tda_shrinkage ShrinkageTimeDependant { get; set; }

        /// <summary>
        /// Gets or sets the elasticity time dependant.
        /// </summary>
        [XmlElement("tda_elasticity")]
        public StruSoft.Interop_24.Tda_elasticity ElasticityTimeDependant { get; set; }

        /// <summary>
        /// Gets or sets the plasticity.
        /// </summary>
        [XmlElement("plastic_analysis_data")]
        public StruSoft.Interop_24.Concrete_pl_data Plasticity { get; set; }

        /// <summary>
        /// Gets or sets the fck.
        /// </summary>
        [XmlAttribute("Fck")]
        public string Fck { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fck cube.
        /// </summary>
        [XmlAttribute("Fck_cube")]
        public string Fck_cube { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fctk.
        /// </summary>
        [XmlAttribute("Fctk")]
        public string Fctk { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fctm.
        /// </summary>
        [XmlAttribute("Fctm")]
        public string Fctm { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the ecm.
        /// </summary>
        [XmlAttribute("Ecm")]
        public string Ecm { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the gamma c 0.
        /// </summary>
        [XmlAttribute("gammaC_0")]
        public string gammaC_0 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the gamma c 1.
        /// </summary>
        [XmlAttribute("gammaC_1")]
        public string gammaC_1 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the gamma ce.
        /// </summary>
        [XmlAttribute("gammaCE")]
        public string gammaCE { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the gamma s 0.
        /// </summary>
        [XmlAttribute("gammaS_0")]
        public string gammaS_0 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the gamma s 1.
        /// </summary>
        [XmlAttribute("gammaS_1")]
        public string gammaS_1 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the alfa cc.
        /// </summary>
        [XmlAttribute("alfaCc")]
        public string alfaCc { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the alfa ct.
        /// </summary>
        [XmlAttribute("alfaCt")]
        public string alfaCt { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the fcd 0.
        /// </summary>
        [XmlAttribute("Fcd_0")]
        public string Fcd_0 { get; set; } // double
        /// <summary>
        /// Gets or sets the fcd 1.
        /// </summary>
        [XmlAttribute("Fcd_1")]
        public string Fcd_1 { get; set; } // double
        /// <summary>
        /// Gets or sets the fctd 0.
        /// </summary>
        [XmlAttribute("Fctd_0")]
        public string Fctd_0 { get; set; } // double
        /// <summary>
        /// Gets or sets the fctd 1.
        /// </summary>
        [XmlAttribute("Fctd_1")]
        public string Fctd_1 { get; set; } // double
        /// <summary>
        /// Gets or sets the ecd 0.
        /// </summary>
        [XmlAttribute("Ecd_0")]
        public string Ecd_0 { get; set; } // double
        /// <summary>
        /// Gets or sets the ecd 1.
        /// </summary>
        [XmlAttribute("Ecd_1")]
        public string Ecd_1 { get; set; } // double
        /// <summary>
        /// Gets or sets the epsc2.
        /// </summary>
        [XmlAttribute("Epsc2")]
        public string Epsc2 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the epscu2.
        /// </summary>
        [XmlAttribute("Epscu2")]
        public string Epscu2 { get; set; } // material_base_value

        /// <summary>
        /// Gets or sets the epsc3.
        /// </summary>
        [XmlAttribute("Epsc3")]
        public string Epsc3 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the epscu3.
        /// </summary>
        [XmlAttribute("Epscu3")]
        public string Epscu3 { get; set; } // material_base_value
        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        [XmlAttribute("environment")]
        public string Environment { get; set; } // integer
        /// <summary>
        /// Gets or sets the creep.
        /// </summary>
        [XmlAttribute("creep")]
        public double Creep { get; set; } // material_0base_value
        /// <summary>
        /// Gets or sets the creep slq.
        /// </summary>
        [XmlAttribute("creep_sls")]
        public double CreepSlq { get; set;} // material_0base_value
        /// <summary>
        /// Gets or sets the creep slf.
        /// </summary>
        [XmlAttribute("creep_slf")]
        public double CreepSlf { get; set;} // material_0base_value
        /// <summary>
        /// Gets or sets the creep slc.
        /// </summary>
        [XmlAttribute("creep_slc")]
        public double CreepSlc { get; set;} // material_0base_value
        /// <summary>
        /// Gets or sets the shrinkage.
        /// </summary>
        [XmlAttribute("shrinkage")]
        public double Shrinkage { get; set; } // non_neg_max_1000
        /// <summary>
        /// Gets or sets the nu.
        /// </summary>
        [XmlAttribute("nu")]
        public string nu { get; set; } // material_nu_value
        /// <summary>
        /// Gets or sets the alfa.
        /// </summary>
        [XmlAttribute("alfa")]
        public string alfa { get; set; } // material_base_value
        // [XmlAttribute("stability")]
        // public string stability { get; set; } // reduction_factor_type

        /// <summary>
        /// Set Material parameters.
        /// </summary>
        internal void SetMaterialParameters(double creepUls, double creepSlq, double creepSlf, double creepSlc, double shrinkage)
        {
            this.Creep = creepUls;
            this.CreepSlq = creepSlq;
            this.CreepSlf = creepSlf;
            this.CreepSlc = creepSlc;
            this.Shrinkage = shrinkage;
        }

        internal void SetPlasticity(bool plastic = true, bool hardening = true, CrushingCriterion crushing = CrushingCriterion.Prager, bool tensionStrength = true, TensionStiffening tensionStiffening = TensionStiffening.Hinton, ReducedCompression reducedCompression = ReducedCompression.Vecchio1, bool reducedTransverse = false, bool ultimateStrainRebars = true )
        {
            var plasticity = new StruSoft.Interop_24.Concrete_pl_attribs();
            plasticity.Elasto_plastic_behaviour = plastic;
            plasticity.Plastic_hardening = hardening;

            if(crushing == CrushingCriterion.None)
            {
                plasticity.Concrete_crushing = false;
            }
            else
            {
                plasticity.Concrete_crushing = true;
                plasticity.Concrete_crushing_option = (StruSoft.Interop_24.Cc_type)crushing;
            }

            plasticity.Tension_strength = tensionStrength;

            if(tensionStiffening == TensionStiffening.None)
            {
                plasticity.Tension_stiffening = false;
            }
            else
            {
                plasticity.Tension_stiffening = true;
                plasticity.Tension_stiffening_option = (StruSoft.Interop_24.Ts_type)tensionStiffening;

                if(tensionStiffening == TensionStiffening.Hinton)
                    plasticity.Tension_stiffening_param = 0.5;
                else if(tensionStiffening == TensionStiffening.Vecchio)
                    plasticity.Tension_stiffening_param = 500;
                else if(tensionStiffening == TensionStiffening.Linear)
                    plasticity.Tension_stiffening_param = 11;
                else if(tensionStiffening == TensionStiffening.Cervera)
                    plasticity.Tension_stiffening_param = 0.150;
            }

            if(reducedCompression == ReducedCompression.None)
            {
                plasticity.Reduced_compression_strength = false;
            }
            else
            {
                plasticity.Reduced_compression_strength = true;
                plasticity.Reduced_compression_strength_option = (StruSoft.Interop_24.Rcsm_type) reducedCompression;

                if(reducedCompression == ReducedCompression.Cervera)
                    plasticity.Reduced_compression_strength_param = 0.550;
                else if(reducedCompression == ReducedCompression.EN_1992_1_1_2023)
                    plasticity.Reduced_compression_strength_param = 110;
            }


            plasticity.Reduced_transverse_shear_stiffnes = reducedTransverse;
            plasticity.Ultimate_strain = ultimateStrainRebars;


            this.Plasticity.U = plasticity;
            this.Plasticity.Sq = plasticity;
            this.Plasticity.Sf = plasticity;
            this.Plasticity.Sc = plasticity;
        }


    }

    /// <summary>
    /// Defines the Crushing Criterion enumeration.
    /// </summary>
    public enum CrushingCriterion
    {
        /// <remarks/>
        Crisfield,

        /// <remarks/>
        Cervera,

        /// <remarks/>
        Hinton,

        /// <remarks/>
        Prager,
        None,
    }

    /// <summary>
    /// Defines the Tension Stiffening enumeration.
    /// </summary>
    public enum TensionStiffening
    {
        Hinton,
        Vecchio,
        Linear,
        Cervera,
        None,
    }

    /// <summary>
    /// Defines the Reduced Compression enumeration.
    /// </summary>
    public enum ReducedCompression
    {
        [System.Xml.Serialization.XmlEnumAttribute("Vecchio 1")]
        Vecchio1,

        [System.Xml.Serialization.XmlEnumAttribute("Vecchio 2")]
        Vecchio2,

        [System.Xml.Serialization.XmlEnumAttribute("Cervera")]
        Cervera,

        [System.Xml.Serialization.XmlEnumAttribute("EN 1992-1-1:2023")]
        EN_1992_1_1_2023,

        [System.Xml.Serialization.XmlEnumAttribute("Model Code 2010")]
        ModelCode2010,

        None,
    }
}