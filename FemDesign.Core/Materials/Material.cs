// https://strusoft.com/
using System;
using System.Globalization;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace FemDesign.Materials
{
    /// <summary>
    /// Represents a Material.
    /// </summary>
    [System.Serializable]
    public partial class Material: EntityBase, IMaterial
    {
        internal static int _fuzzyScore = 80;

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
        /// Name of Material.
        /// </summary>
        /// <value></value>
        [XmlAttribute("name")]
        public string Name { get; set; } // name256
        /// <summary>
        /// Gets or sets the timber.
        /// </summary>
        [XmlElement("timber")]
        public Timber Timber { get; set; }
        /// <summary>
        /// Gets or sets the concrete.
        /// </summary>
        [XmlElement("concrete")]
        public Concrete Concrete { get; set; }
        /// <summary>
        /// Gets or sets the custom.
        /// </summary>
        [XmlElement("custom")]
        public Custom Custom { get; set; }
        /// <summary>
        /// Gets or sets the steel.
        /// </summary>
        [XmlElement("steel")]
        public Steel Steel { get; set; }
        /// <summary>
        /// Gets or sets the reinforcing steel.
        /// </summary>
        [XmlElement("reinforcing_steel")]
        public ReinforcingSteel ReinforcingSteel { get; set; }
        /// <summary>
        /// Gets or sets the stratum.
        /// </summary>
        [XmlElement("stratum")]
        public StruSoft.Interop.StruXml.Data.Material_typeStratum Stratum { get; set; }
        /// <summary>
        /// Gets or sets the brick.
        /// </summary>
        [XmlElement("brick")]
        public StruSoft.Interop.StruXml.Data.Material_typeBrick Brick { get; set; }
        /// <summary>
        /// Gets or sets the masonry.
        /// </summary>
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

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
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

        /// <summary>
        /// Custom Uniaxial Material.
        /// </summary>
        /// <param name="name">the name.</param>
        /// <param name="mass">the mass.</param>
        /// <param name="e_0">value for <paramref name="e_0"/>.</param>
        /// <param name="nu_0">the nu 0.</param>
        /// <param name="alfa_0">the alfa 0.</param>
        /// <returns>The result.</returns>
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

    /// <summary>
    /// Defines the Family enumeration.
    /// </summary>
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
        /// <summary>
        /// Material By Name.
        /// </summary>
        /// <param name="materials">the materials.</param>
        /// <param name="materialName">the material name.</param>
        /// <returns>The result.</returns>
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

        /// <summary>
        /// Sets the steel plasticity.
        /// </summary>
        /// <param name="material">the material.</param>
        /// <param name="plastic">the plastic.</param>
        /// <param name="strainLimit">the strain limit.</param>
        /// <returns>The result.</returns>
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

        /// <summary>
        /// Sets the concrete plasticity.
        /// </summary>
        /// <param name="material">the material.</param>
        /// <param name="plastic">the plastic.</param>
        /// <param name="hardening">the hardening.</param>
        /// <param name="crushing">the crushing.</param>
        /// <param name="tensionStrength">the tension strength.</param>
        /// <param name="tensionStiffening">the tension stiffening.</param>
        /// <param name="reducedCompression">the reduced compression.</param>
        /// <param name="reducedTransverse">the reduced transverse.</param>
        /// <param name="ultimateStrainRebars">the ultimate strain rebars.</param>
        /// <returns>The result.</returns>
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

        /// <summary>
        /// Sets the creep.
        /// </summary>
        /// <param name="material">the material.</param>
        /// <param name="to">the to.</param>
        /// <param name="humidity">the humidity.</param>
        /// <param name="nonLinearCreep">the non linear creep.</param>
        /// <param name="cementType">the cement type.</param>
        /// <param name="increaseFinalValue">the increase final value.</param>
        /// <returns>The result.</returns>
        public static Material SetCreep(this Material material, int to = 28, int humidity = 50, bool nonLinearCreep = true, StruSoft.Interop_24.Cement_type cementType = StruSoft.Interop_24.Cement_type.Class_S, bool increaseFinalValue = true)
        {
            if (material.Concrete == null)
            {
                throw new System.ArgumentException("Material must be concrete!");
            }

            // deep clone. downstreams objs will have contain changes made in this method, upstream objs will not.
            var creep = new StruSoft.Interop_24.Tda_creep2();
            creep.EN_199211_2004 = new StruSoft.Interop_24.Tda_creep_EN1992()
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

        /// <summary>
        /// Sets the shrinkage.
        /// </summary>
        /// <param name="material">the material.</param>
        /// <param name="to">the to.</param>
        /// <param name="humidity">the humidity.</param>
        /// <param name="cementType">the cement type.</param>
        /// <returns>The result.</returns>
        public static Material SetShrinkage(this Material material, int to = 28, int humidity = 50, StruSoft.Interop_24.Cement_type cementType = StruSoft.Interop_24.Cement_type.Class_S)
        {
            if (material.Concrete == null)
            {
                throw new System.ArgumentException("Material must be concrete!");
            }

            // deep clone. downstreams objs will have contain changes made in this method, upstream objs will not.
            var shrinkage = new StruSoft.Interop_24.Tda_shrinkage();
            shrinkage.EN_199211_2004 = new StruSoft.Interop_24.Tda_shrinkageEN_199211_2004()
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

        /// <summary>
        /// set Elasticity.
        /// </summary>
        /// <param name="material">the material.</param>
        /// <param name="to">the to.</param>
        /// <param name="cementType">the cement type.</param>
        /// <returns>The result.</returns>
        public static Material setElasticity(this Material material, int to = 28, StruSoft.Interop_24.Cement_type cementType = StruSoft.Interop_24.Cement_type.Class_S)
        {
            if (material.Concrete == null)
            {
                throw new System.ArgumentException("Material must be concrete!");
            }

            // deep clone. downstreams objs will have contain changes made in this method, upstream objs will not.
            var elasticity = new StruSoft.Interop_24.Tda_elasticity();
            elasticity.EN_199211_2004 = new StruSoft.Interop_24.Tda_elasticityEN_199211_2004()
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