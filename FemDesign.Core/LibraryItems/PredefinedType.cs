// https://strusoft.com/

using FemDesign.Materials;
using StruSoft.Interop.StruXml.Data;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;


namespace FemDesign.LibraryItems
{
    /// <summary>
    /// Represents a Point Connection Types.
    /// </summary>
    [System.Serializable]
    public partial class PointConnectionTypes
    {
        /// <summary>
        /// Gets or sets the predefined types.
        /// </summary>
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType2> PredefinedTypes { get; set; }
    }

    /// <summary>
    /// Represents a Point Support Group Types.
    /// </summary>
    [System.Serializable]
    public partial class PointSupportGroupTypes
    {
        /// <summary>
        /// Gets or sets the predefined types.
        /// </summary>
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType2> PredefinedTypes { get; set; }
    }

    /// <summary>
    /// Represents a Line Connection Types.
    /// </summary>
    [System.Serializable]
    public partial class LineConnectionTypes
    {
        /// <summary>
        /// Gets or sets the predefined types.
        /// </summary>
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType3> PredefinedTypes { get; set; }
    }

    /// <summary>
    /// Represents a Line Support Group Types.
    /// </summary>
    [System.Serializable]
    public partial class LineSupportGroupTypes
    {
        /// <summary>
        /// Gets or sets the predefined types.
        /// </summary>
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType2> PredefinedTypes { get; set; }
    }

    /// <summary>
    /// Represents a Surface Connection Types.
    /// </summary>
    [System.Serializable]
    public partial class SurfaceConnectionTypes
    {
        /// <summary>
        /// Gets or sets the predefined types.
        /// </summary>
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType1> PredefinedTypes { get; set; }
    }

    /// <summary>
    /// Represents a Surface Support Types.
    /// </summary>
    [System.Serializable]
    public partial class SurfaceSupportTypes
    {
        /// <summary>
        /// Gets or sets the predefined types.
        /// </summary>
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType1> PredefinedTypes { get; set; }
    }


    /// <summary>
    /// Represents a Vehicle Types.
    /// </summary>
    [System.Serializable]
    public partial class VehicleTypes
    {
        /// <summary>
        /// Gets or sets the predefined types.
        /// </summary>
        [XmlElement("predefined_type", Order = 1)]
        public List<StruSoft.Interop.StruXml.Data.Vehicle_lib_type> PredefinedTypes { get; set; }

    }

    /// <summary>
    /// Section database.
    /// </summary>
    [System.Serializable]
    [XmlRoot("database", Namespace = "urn:strusoft")]
    public partial class VehicleDatabase
    {
        /// <summary>
        /// Gets or sets the vehicle types.
        /// </summary>
        [XmlElement("vehicle_types")]
        public LibraryItems.VehicleTypes VehicleTypes { get; set; }

        /// <summary>
        /// Deserializes from resource.
        /// </summary>
        /// <returns>The result.</returns>
        public static List<StruSoft.Interop.StruXml.Data.Vehicle_lib_type> DeserializeFromResource()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(VehicleDatabase));

            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.EndsWith("vehicles.struxml"))
                {
                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        TextReader reader = new StreamReader(stream);
                        object obj = deserializer.Deserialize(reader);
                        VehicleDatabase vehicleDatabase = (VehicleDatabase)obj;
                        reader.Close();

                        return vehicleDatabase.VehicleTypes.PredefinedTypes;
                    }
                }
            }
            throw new System.ArgumentException("Vehicle library resource not in assembly! Was project compiled without embedded resource?");
        }

        /// <summary>
        /// Deserializes from file path.
        /// </summary>
        /// <param name="filePath">the file path.</param>
        /// <returns>The result.</returns>
        public static List<StruSoft.Interop.StruXml.Data.Vehicle_lib_type> DeserializeFromFilePath(string filePath)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(VehicleDatabase));
            TextReader reader = new StreamReader(filePath);
            object obj = deserializer.Deserialize(reader);
            VehicleDatabase vehicleDatabase = (VehicleDatabase)obj;
            reader.Close();
            return vehicleDatabase.VehicleTypes.PredefinedTypes;
        }


    }

}
