// https://strusoft.com/
using System;
using System.Globalization;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace FemDesign.Materials
{
    /// <summary>
    /// material_type
    /// </summary>
    [System.Serializable]
    public partial class Material: EntityBase, IMaterial
    {
        internal static int _fuzzyScore = 80;

        [XmlAttribute("standard")]
        public string Standard { get; set; } // standardtype
        [XmlAttribute("country")]
        public string Country { get; set; } // eurocodetype
        /// <summary>
        /// Name of Material.
        /// </summary>
        /// <value></value>
        [XmlAttribute("name")]
        public string Name { get; set; } // name256
        [XmlElement("timber")]
        public Timber Timber { get; set; }
        [XmlElement("concrete")]
        public Concrete Concrete { get; set; }
        [XmlElement("custom")]
        public Custom Custom { get; set; }
        [XmlElement("steel")]
        public Steel Steel { get; set; }
        [XmlElement("reinforcing_steel")]
        public ReinforcingSteel ReinforcingSteel { get; set; }
        [XmlElement("stratum")]
        public StruSoft.Interop.StruXml.Data.Material_typeStratum Stratum { get; set; }
        [XmlElement("brick")]
        public StruSoft.Interop.StruXml.Data.Material_typeBrick Brick { get; set; }
        [XmlElement("masonry")]
        public StruSoft.Interop.StruXml.Data.Material_typeMasonry Masonry { get; set; }

        [XmlIgnore]
        public Family Family
        {
            get
            {
                if (this.Steel != null)
                    return Family.Steel;
                else if (this.Concrete != null)
                    return Family.Concrete;
                else if (this.Timber != null)
                    return Family.Timber;
                else if (this.Stratum != null)
                    return Family.Stratum;
                else if (this.ReinforcingSteel != null)
                    return Family.ReinforcingSteel;
                else
                    return Family.Custom;
            }
        }

        public override string ToString()
        {
            return $"{this.Name}";
        }

        /// <summary>
        /// Set creep and shrinkage parameters to a concrete Material.
        /// </summary>
        /// <param name="material">Material.</param>
        /// <param name="creepUls">Creep ULS.</param>
        /// <param name="creepSlq">Creep SLS Quasi-Permanent</param>
        /// <param name="creepSlf">Creep SLS Frequent</param>
        /// <param name="creepSlc">Creep SLS Characteristic</param>
        /// <param name="shrinkage">Shrinkage.</param>
        /// <returns></returns>
        public static Material ConcreteMaterialProperties(Material material, double creepUls = 0, double creepSlq = 0, double creepSlf = 0, double creepSlc = 0, double shrinkage = 0)
        {
            if (material.Concrete != null)
            {
                // deep clone. downstreams objs will have contain changes made in this method, upstream objs will not.
                Material newMaterial = material.DeepClone();
                
                // downstream and uppstream objs will NOT share guid.
                newMaterial.EntityCreated();

                // set parameters
                newMaterial.Concrete.SetMaterialParameters(creepUls, creepSlq, creepSlf, creepSlc, shrinkage);
                newMaterial.EntityModified();

                // return
                return newMaterial;
            }
            else
            {
                throw new System.ArgumentException("Material must be concrete!");
            }
        }



        /// <summary>
        /// Set material properties for timber material.
        /// </summary>
        /// <param name="material">Material.</param>
        /// <param name="ksys">System strength factor.</param>
        /// <param name="k_cr">k_cr. Must be between 0 and 1.</param>
        /// <param name="serviceClass">Service class. 1,2 or 3.</param>
        /// <param name="kdefU">kdef U/Ua/Us.</param>
        /// <param name="kdefSq">kdef Sq.</param>
        /// <param name="kdefSf">kdef Sf.</param>
        /// <param name="kdefSc">kdef Sc.</param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public static Material TimberMaterialProperties(Material material, double ksys = 1.0, double k_cr = 0.67, TimberServiceClassEnum serviceClass = TimberServiceClassEnum.ServiceClass1, double kdefU = 0.0, double kdefSq = 0.60, double kdefSf = 0.60, double kdefSc = 0.60, string newName = null)
        {
            if (material.Timber != null)
            {
                // deep clone. downstreams objs will have contain changes made in this method, upstream objs will not.
                Material newMaterial = material.DeepClone();
                
                if (newName == null)
                    newMaterial.Name += "_modified";
                else
                {
                    newMaterial.Name = newName;
                }
                // downstream and uppstream objs will NOT share guid.
                newMaterial.EntityCreated();

                // set parameters
                newMaterial.Timber.SetMaterialParameters(ksys, k_cr, serviceClass, kdefU, kdefSq, kdefSf, kdefSc);
                newMaterial.EntityModified();

                // return
                return newMaterial;
            }
            else
            {
                throw new System.ArgumentException("Material must be timber!");
            }
        }

        public static Material CustomUniaxialMaterial(string name, double mass, double e_0, double nu_0, double alfa_0)
        {
            var material = new Material();
            material.Name = name;
            material.Country = "n/a";
            material.Standard = "general";
            material.EntityCreated();

            material.Custom = new Custom(mass, e_0, nu_0, alfa_0);
            return material;
        }
    }

    public enum Family
    {
        Steel,
        Concrete,
        Timber,
        Stratum,
        ReinforcingSteel,
        Custom
    }


    public static class MaterialExtension
    {
        public static Material MaterialByName(this List<FemDesign.Materials.Material> materials, string materialName)
        {
            var materialNames = materials.Select(x => x.Name).ToArray();
            var extracted = FuzzySharp.Process.ExtractOne(materialName, materialNames);

            if (extracted.Score < Material._fuzzyScore)
                throw new Exception($"{materialName} can not be found!");

            var index = extracted.Index;
            return materials[index];
        }



        /// <summary>
        /// Set plasticy parameters to a steel Material.
        /// </summary>
        /// <param name="material"></param>
        /// <param name="plastic"></param>
        /// <param name="strainLimit"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static Material SetSteelPlasticity(this Material material, bool plastic = true, double strainLimit = 2.5)
        {
            var newMaterial = material.SetSteelPlasticity( new List<bool> { plastic, plastic, plastic, plastic }, new List<double> { strainLimit, strainLimit, strainLimit, strainLimit });
            return newMaterial;
        }

        public static Material SetSteelPlasticity(this Material material, List<bool> plastic, List<double> strainLimit)
        {
            if (material.Steel == null)
            {
                throw new System.ArgumentException("Material must be concrete!");
            }

            // deep clone. downstreams objs will have contain changes made in this method, upstream objs will not.
            Material newMaterial = material.DeepClone();
            newMaterial.EntityCreated();
            newMaterial.Steel.SetPlasticity(plastic, strainLimit);
            newMaterial.EntityModified();

            // return
            return newMaterial;
        }

        public static Material SetConcretePlasticity(this Material material, bool plastic = true, bool hardening = true, CrushingCriterion crushing = CrushingCriterion.Prager, bool tensionStrength = true, TensionStiffening tensionStiffening = TensionStiffening.Hinton, ReducedCompression reducedCompression = ReducedCompression.Vecchio1, bool reducedTransverse = false, bool ultimateStrainRebars = true)
        {
            if (material.Concrete == null)
            {
                throw new System.ArgumentException("Material must be concrete!");
            }

            // deep clone. downstreams objs will have contain changes made in this method, upstream objs will not.
            Material newMaterial = material.DeepClone();
            newMaterial.EntityCreated();
            newMaterial.Concrete.SetPlasticity(plastic, hardening, crushing, tensionStrength, tensionStiffening, reducedCompression, reducedTransverse, ultimateStrainRebars);
            newMaterial.EntityModified();

            // return
            return newMaterial;
        }

        public static Material SetCreep(this Material material, int to = 28, int humidity = 50, bool nonLinearCreep = true, StruSoft.Interop_25.Cement_type cementType = StruSoft.Interop_25.Cement_type.Class_S, bool increaseFinalValue = true)
        {
            if (material.Concrete == null)
            {
                throw new System.ArgumentException("Material must be concrete!");
            }

            // deep clone. downstreams objs will have contain changes made in this method, upstream objs will not.
            var creep = new StruSoft.Interop_25.Tda_creep2();
            creep.EN_199211_2004 = new StruSoft.Interop_25.Tda_creep_EN1992()
            {
                T0 = to,
                RH = humidity,
                Calculate_Ac_u = true,
                Ac = 0.001, // set a low value as FEM-Design will overwrite with the right value
                U = 0.001, // set a low value as FEM-Design will overwrite with the right value
                Sigma_relevant = nonLinearCreep,
                Cement_type = cementType,
                Increase_final_value = increaseFinalValue
            };


            Material newMaterial = material.DeepClone();
            newMaterial.EntityCreated();
            newMaterial.Concrete.CreepTimeDependant = creep;
            newMaterial.EntityModified();

            // return
            return newMaterial;
        }

        public static Material SetShrinkage(this Material material, int to = 28, int humidity = 50, StruSoft.Interop_25.Cement_type cementType = StruSoft.Interop_25.Cement_type.Class_S)
        {
            if (material.Concrete == null)
            {
                throw new System.ArgumentException("Material must be concrete!");
            }

            // deep clone. downstreams objs will have contain changes made in this method, upstream objs will not.
            var shrinkage = new StruSoft.Interop_25.Tda_shrinkage();
            shrinkage.EN_199211_2004 = new StruSoft.Interop_25.Tda_shrinkageEN_199211_2004()
            {
                Ts = to,
                RH = humidity,
                Calculate_Ac_u = true,
                Ac = 0.001, // set a low value as FEM-Design will overwrite with the right value
                U = 0.001, // set a low value as FEM-Design will overwrite with the right value
                Cement_type = cementType,
            };


            Material newMaterial = material.DeepClone();
            newMaterial.EntityCreated();
            newMaterial.Concrete.ShrinkageTimeDependant = shrinkage;
            newMaterial.EntityModified();

            // return
            return newMaterial;
        }

        public static Material setElasticity(this Material material, int to = 28, StruSoft.Interop_25.Cement_type cementType = StruSoft.Interop_25.Cement_type.Class_S)
        {
            if (material.Concrete == null)
            {
                throw new System.ArgumentException("Material must be concrete!");
            }

            // deep clone. downstreams objs will have contain changes made in this method, upstream objs will not.
            var elasticity = new StruSoft.Interop_25.Tda_elasticity();
            elasticity.EN_199211_2004 = new StruSoft.Interop_25.Tda_elasticityEN_199211_2004()
            {
                T0 = to,
                Cement_type = cementType,
            };


            Material newMaterial = material.DeepClone();
            newMaterial.EntityCreated();
            newMaterial.Concrete.ElasticityTimeDependant = elasticity;
            newMaterial.EntityModified();

            // return
            return newMaterial;
        }

    }
}