// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Xml.Serialization;
using FemDesign.Materials;

namespace FemDesign.Materials
{
    /// <summary>
    /// Material database.
    /// </summary>
    [XmlRoot("database", Namespace = "urn:strusoft")]
    public partial class MaterialDatabase
    {
        [XmlIgnore]
        public string FilePath { get; set; }
        [XmlAttribute("struxml_version")]
        public string StruxmlVersion { get; set; }
        [XmlAttribute("source_software")]
        public string SourceSoftware { get; set; }
        [XmlAttribute("start_time")]
        public string StartTime { get; set; }
        [XmlAttribute("end_time")]
        public string EndTime { get; set; }
        [XmlAttribute("guid")]
        public string Guid { get; set; }
        [XmlAttribute("convertid")]
        public string ConvertId { get; set; }
        [XmlAttribute("standard")]
        public string Standard { get; set; }
        [XmlAttribute("country")]
        public string Country { get; set; }
        [XmlAttribute("xmlns")]
        public string Xmlns { get; set; }
        [XmlElement("materials")]
        public Materials Materials { get; set; } // materials
        [XmlElement("reinforcing_materials")]
        public Materials ReinforcingMaterials { get; set; } // reinforcing_materials
        [XmlElement("clt_panel_types")]
        public CltPanelTypes CltPanelTypes { get; set; } // clt_panel_types
        [XmlElement("timber_panel_types")]
        public OrthotropicPanelTypes OrthotropicPanelTypes { get; set; } // clt_panel_types
        [XmlElement("end")]
        public string End { get; set; }
        [XmlIgnore]
        private static Dictionary<string, MaterialDatabase> _defaultSectionDatabaseCache = new Dictionary<string, MaterialDatabase>();

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MaterialDatabase()
        {

        }

        /// <summary>
        /// Lists the names of all Materials in MaterialDatabase.
        /// </summary>
        /// <returns>List of material names.</returns>
        public List<string> MaterialNames()
        {
            // empty list
            List<string> list = new List<string>();

            // list material names
            if (this.Materials != null)
            {
                foreach (Material material in this.Materials.Material)
                {
                    list.Add(material.Name);
                }
            }
            if (this.ReinforcingMaterials != null)
            {
                foreach (Material material in this.ReinforcingMaterials.Material)
                {
                    list.Add(material.Name);
                }
            }
            if (this.CltPanelTypes != null)
            {
                foreach (CltPanelLibraryType panelType in this.CltPanelTypes.CltPanelLibraryTypes)
                {
                    list.Add(panelType.Name);
                }
            }
            if (this.OrthotropicPanelTypes != null)
            {
                foreach (OrthotropicPanelLibraryType panelType in this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes)
                {
                    list.Add(panelType.Name);
                }
            }

            // return
            return list;
        }

        /// <summary>
        /// Get Material from MaterialDatabase by name.
        /// </summary>
        /// <param name="materialName">Name of Material</param>
        /// <returns></returns>
        public Material MaterialByName(string materialName)
        {
            return this.Materials.Material.MaterialByName(materialName);
        }

        public List<Material> GetSoilMaterial()
        {
            var soilMaterial = new List<Material>();

            foreach(var material in this.Materials.Material)
            {
                if(material.Stratum != null)
                    soilMaterial.Add(material);
            }
            return soilMaterial;
        }

        public List<CltPanelLibraryType> GetCltPanelLibrary()
        {
            if (this.CltPanelTypes != null)
            {
                return this.CltPanelTypes.CltPanelLibraryTypes;
            }
            return null;
        }

        public List<OrthotropicPanelLibraryType> GetOrthotropicPanelLibrary()
        {
            if (this.OrthotropicPanelTypes != null)
            {
                return this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes;
            }
            return null;
        }

        public CltPanelLibraryType GetCltPanelLibraryTypeByName(string panelLibraryTypeName)
        {
            if (this.CltPanelTypes != null)
            {
                foreach (CltPanelLibraryType panelLibraryType in this.CltPanelTypes.CltPanelLibraryTypes)
                {
                    if (panelLibraryType.Name == panelLibraryTypeName)
                    {
                        // update object information
                        //panelLibraryType.Guid = System.Guid.NewGuid();
                        panelLibraryType.EntityModified();

                        // return
                        return panelLibraryType;
                    }
                }
            }
            throw new System.ArgumentException($"Material was not found. Incorrect material name ({panelLibraryTypeName}) or no CltPanelTypes present in material database.");
        }

        private static MaterialDatabase DeserializeFromFilePath(string filePath)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(MaterialDatabase));
            TextReader reader = new StreamReader(filePath);
            object obj = deserializer.Deserialize(reader);
            MaterialDatabase materialDatabase = (MaterialDatabase)obj;
            reader.Close();
            return materialDatabase;
        }

        /// <summary>
        /// Load a custom MaterialDatabase from a .struxml file.
        /// </summary>
        /// <param name="filePath">File path to .struxml file.</param>
        /// <returns></returns>
        public static MaterialDatabase DeserializeStruxml(string filePath)
        {
            MaterialDatabase materialDatabase = MaterialDatabase.DeserializeFromFilePath(filePath);
            materialDatabase.End = "";
            return materialDatabase;
        }

        /// <summary>
        /// Deserialize MaterialDatabase from embedded resource.
        /// </summary>
        private static MaterialDatabase DeserializeResource(string countryCode)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(MaterialDatabase));

            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.EndsWith("materials_" + countryCode + ".struxml"))
                {
                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        TextReader reader = new StreamReader(stream);
                        object obj = deserializer.Deserialize(reader);
                        MaterialDatabase materialDatabase = (MaterialDatabase)obj;
                        reader.Close();

                        if (materialDatabase.Materials.Material.Count == 0)
                        {
                            throw new System.ArgumentException("The project was compiled without any materials. Add materials to your project and re-compile or use another method to construct the material database (i.e DeserializeStruxml).");
                        }

                        return materialDatabase;
                    }
                }
            }
            throw new System.ArgumentException("Material library resource not in assembly! Was project compiled without embedded resources?");
        }

        private static MaterialDatabase DeserializeResourceTimberPlate()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(MaterialDatabase));

            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.EndsWith("timberPlate.struxml"))
                {
                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        TextReader reader = new StreamReader(stream);
                        object obj = deserializer.Deserialize(reader);
                        MaterialDatabase materialDatabase = (MaterialDatabase)obj;
                        reader.Close();

                        if (materialDatabase.CltPanelTypes.CltPanelLibraryTypes.Count == 0 || materialDatabase.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes.Count == 0)
                        {
                            throw new System.ArgumentException("The project was compiled without any materials. Add materials to your project and re-compile or use another method to construct the material database (i.e DeserializeStruxml).");
                        }

                        return materialDatabase;
                    }
                }
            }
            throw new System.ArgumentException("Material library resource not in assembly! Was project compiled without embedded resources?");
        }

        /// <summary>
        /// Load the default MaterialDatabase for each respective country.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="countryCode">National annex of calculation code (B/COMMON/D/DK/E/EST/FIN/GB/H/LT/N/NL/PL/RO/S/TR), or any other name <see cref="FemDesign.Country"/> can be parsed from (e.g. "common", "LV", "Sweden"). Note: TR (Turkish) doesn't contain the plastic material properties.</param>
        /// <returns></returns>
        public static MaterialDatabase GetDefault(string countryCode = "S")
        {
            countryCode = LibraryCountryCode(countryCode);
            if (!_defaultSectionDatabaseCache.ContainsKey(countryCode))
            {
                _defaultSectionDatabaseCache[countryCode] = DeserializeResource(countryCode);
                _defaultSectionDatabaseCache[countryCode].End = "";
            }
            return _defaultSectionDatabaseCache[countryCode];
        }

        /// <summary>
        /// Map a country code (any <see cref="GenericClasses.ParseableAttribute"/> name of <see cref="FemDesign.Country"/>) to the suffix of the embedded materials_*.struxml.
        /// </summary>
        private static string LibraryCountryCode(string countryCode)
        {
            var country = GenericClasses.EnumParser.Parse<FemDesign.Country>(countryCode);
            // FEM-Design 24.00.003 renamed Latvia from "LT" to "LV", and so is the embedded library.
            return country == FemDesign.Country.LT ? "LV" : country.ToString();
        }

        public static MaterialDatabase DefaultTimberPlateLibrary()
        {
            var database = DeserializeResourceTimberPlate();
            return database;
        }

        public (List<Material> steel, List<Material> concrete, List<Material> timber, List<Material> reinforcement, List<Material> stratum, List<Material> custom) ByType()
        {
            var materialDataBaseList = new List<Material>();
            if (this.ReinforcingMaterials != null)
            {
                materialDataBaseList = this.Materials.Material.Concat(this.ReinforcingMaterials.Material).ToList();
            }
            else
                materialDataBaseList.AddRange(this.Materials.Material);

            var steel = new List<Material>();
            var timber = new List<Material>();
            var concrete = new List<Material>();
            var reinforcement = new List<Material>();
            var stratum = new List<Material>();
            var custom = new List<Material>();

            foreach (var material in materialDataBaseList)
            {
                // update object information
                //material.Guid = System.Guid.NewGuid();
                //material.EntityModified();

                if (material.Family == Family.Steel)
                {
                    steel.Add(material);
                }
                else if (material.Family == Family.Concrete)
                {
                    concrete.Add(material);
                }
                else if (material.Family == Family.Timber)
                {
                    timber.Add(material);
                }
                else if (material.Family == Family.ReinforcingSteel)
                {
                    reinforcement.Add(material);
                }
                else if (material.Family == Family.Stratum)
                {
                    stratum.Add(material);
                }
                else if (material.Family == Family.Custom)
                {
                    custom.Add(material);
                }
            }
            return (steel, concrete, timber, reinforcement, stratum, custom);
        }

        /// <summary>
        /// Serialize MaterialDatabase to file (.struxml).
        /// </summary>
        private void Serialize(string filepath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MaterialDatabase));
            using (TextWriter writer = new StreamWriter(filepath))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}