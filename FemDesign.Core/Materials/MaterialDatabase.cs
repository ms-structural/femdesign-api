// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Xml.Serialization;

namespace FemDesign.Materials
{
    /// <summary>
    /// Material database.
    /// </summary>
    [XmlRoot("database", Namespace = "urn:strusoft")]
    public partial class MaterialDatabase
    {
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        [XmlIgnore]
        public string FilePath { get; set; }
        /// <summary>
        /// Gets or sets the struxml version.
        /// </summary>
        [XmlAttribute("struxml_version")]
        public string StruxmlVersion { get; set; }
        /// <summary>
        /// Gets or sets the source software.
        /// </summary>
        [XmlAttribute("source_software")]
        public string SourceSoftware { get; set; }
        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        [XmlAttribute("start_time")]
        public string StartTime { get; set; }
        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        [XmlAttribute("end_time")]
        public string EndTime { get; set; }
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        [XmlAttribute("guid")]
        public string Guid { get; set; }
        /// <summary>
        /// Gets or sets the convert id.
        /// </summary>
        [XmlAttribute("convertid")]
        public string ConvertId { get; set; }
        /// <summary>
        /// Gets or sets the standard.
        /// </summary>
        [XmlAttribute("standard")]
        public string Standard { get; set; }
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        [XmlAttribute("country")]
        public string Country { get; set; }
        /// <summary>
        /// Gets or sets the xmlns.
        /// </summary>
        [XmlAttribute("xmlns")]
        public string Xmlns { get; set; }
        /// <summary>
        /// Gets or sets the materials.
        /// </summary>
        [XmlElement("materials")]
        public Materials Materials { get; set; } // materials
        /// <summary>
        /// Gets or sets the reinforcing materials.
        /// </summary>
        [XmlElement("reinforcing_materials")]
        public Materials ReinforcingMaterials { get; set; } // reinforcing_materials
        /// <summary>
        /// Gets or sets the clt panel types.
        /// </summary>
        [XmlElement("clt_panel_types")]
        public CltPanelTypes CltPanelTypes { get; set; } // clt_panel_types
        /// <summary>
        /// Gets or sets the orthotropic panel types.
        /// </summary>
        [XmlElement("timber_panel_types")]
        public OrthotropicPanelTypes OrthotropicPanelTypes { get; set; } // clt_panel_types
        /// <summary>
        /// Gets or sets the end.
        /// </summary>
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
            if (this.Materials != null)
            {
                foreach (Material material in this.Materials.Material)
                {
                    if (material.Name == materialName)
                    {
                        // update object information
                        //material.Guid = System.Guid.NewGuid();
                        material.EntityModified();

                        // return
                        return material;
                    }
                }
            }
            if (this.ReinforcingMaterials != null)
            {
                foreach (Material material in this.ReinforcingMaterials.Material)
                {
                    if (material.Name == materialName)
                    {
                        // update object information
                        //material.Guid = System.Guid.NewGuid();
                        material.EntityModified();

                        // return
                        return material;
                    }
                }
            }
            throw new System.ArgumentException($"Material was not found. Incorrect material name ({materialName}) or empty material database.");
        }

        /// <summary>
        /// Gets the soil material.
        /// </summary>
        /// <returns>The result.</returns>
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

        /// <summary>
        /// Gets the clt panel library.
        /// </summary>
        /// <returns>The result.</returns>
        public List<CltPanelLibraryType> GetCltPanelLibrary()
        {
            if (this.CltPanelTypes != null)
            {
                return this.CltPanelTypes.CltPanelLibraryTypes;
            }
            return null;
        }

        /// <summary>
        /// Gets the orthotropic panel library.
        /// </summary>
        /// <returns>The result.</returns>
        public List<OrthotropicPanelLibraryType> GetOrthotropicPanelLibrary()
        {
            if (this.OrthotropicPanelTypes != null)
            {
                return this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes;
            }
            return null;
        }

        /// <summary>
        /// Gets the clt panel library type by name.
        /// </summary>
        /// <param name="panelLibraryTypeName">the panel library type name.</param>
        /// <returns>The result.</returns>
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
        /// <param name="countryCode">National annex of calculation code (B/COMMON/D/DK/E/EST/FIN/GB/H/LT/N/NL/PL/RO/S/TR). Note: TR (Turkish) doesn't contain the plastic material properties.</param>
        /// <returns></returns>
        public static MaterialDatabase GetDefault(string countryCode = "S")
        {
            RestrictedString.EurocodeType(countryCode);
            if (!_defaultSectionDatabaseCache.ContainsKey(countryCode))
            {
                _defaultSectionDatabaseCache[countryCode] = DeserializeResource(countryCode);
                _defaultSectionDatabaseCache[countryCode].End = "";
            }
            return _defaultSectionDatabaseCache[countryCode];
        }

        /// <summary>
        /// Default Timber Plate Library.
        /// </summary>
        /// <returns>The result.</returns>
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