// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// Material database.
    /// </summary>
    [XmlRoot("database", Namespace="urn:strusoft")]
    public partial class LoadCategoryDatabase
    {
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        [XmlIgnore]
        public string FilePath {get; set; }
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
        /// Gets or sets the load categories.
        /// </summary>
        [XmlElement("load_categories")]
        public LoadCategories LoadCategories { get; set;}
        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        [XmlElement("end")]
        public string End { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LoadCategoryDatabase()
        {

        }

        /// <summary>
        /// Lists the names of all Load Categories in LoadCategoryDatabase.
        /// </summary>
        /// <returns>List of load category names.</returns>
        public List<string> LoadCategoryNames()
        {
            // empty list
            List<string> list = new List<string>();

            // list material names
            if (this.LoadCategories != null)
            {
                foreach (LoadCategory loadCategory in this.LoadCategories.LoadCategory)
                {
                    list.Add(loadCategory.Name);
                }
            }
            // return
            return list;
        }

        /// <summary>
        /// Get load category from LoadCategoryDatabase by name.
        /// </summary>
        /// <param name="loadCategoryName">Name of load type</param>
        /// <returns></returns>
        public LoadCategory LoadCategoryByName(string loadCategoryName)
        {
            if (this.LoadCategories != null)
            {
                foreach (LoadCategory loadCategory in this.LoadCategories.LoadCategory)
                {
                    if (loadCategory.Name == loadCategoryName)
                    {
                        // update object information
                        loadCategory.Guid = System.Guid.NewGuid();
                        loadCategory.EntityModified();

                        // return
                        return loadCategory;
                    }
                }
            }
            throw new System.ArgumentException($"Load category was not found. Incorrect category name ({loadCategoryName}) or empty load category database.");
        }

        /// <summary>
        /// Deserialize LoadCategoryDatabase from embedded resource.
        /// </summary>
        private static LoadCategoryDatabase DeserializeResource(string countryCode)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(LoadCategoryDatabase));

            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.EndsWith("loadCoefficients_" + countryCode + ".struxml"))
                {

                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        TextReader reader = new StreamReader(stream);
                        object obj = deserializer.Deserialize(reader);
                        LoadCategoryDatabase loadCategoryDatabase = (LoadCategoryDatabase)obj;
                        reader.Close();
                        return loadCategoryDatabase;
                    }
                }
            }
            throw new System.ArgumentException($"{countryCode} has NOT been implemented. Contact us at support@strusoft.freshdesk.com if you need it.");
        }
        /// <summary>
        /// Load the default LoadCoefficientDatabase for each respective country.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="countryCode">National annex of calculation code (D/DK/EST/FIN/GB/H/N/PL/RO/S/TR)</param>
        /// <returns></returns>
        public static LoadCategoryDatabase GetDefault(string countryCode = "S")
        {
            string code = RestrictedString.EurocodeType(countryCode);
            LoadCategoryDatabase loadCategoryDatabase = LoadCategoryDatabase.DeserializeResource(code);
            loadCategoryDatabase.End = "";
            return loadCategoryDatabase;
        }

        /// <summary>
        /// Serialize LoadCoefficientsDatabase to file (.struxml).
        /// </summary>
        private void Serialize(string filepath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LoadCategoryDatabase));
            using (TextWriter writer = new StreamWriter(filepath))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}