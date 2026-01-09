using System.Xml.Serialization;


namespace FemDesign.Materials
{
    /// <summary>
    /// Represents a Timber Service Class Kdfes.
    /// </summary>
    [System.Serializable]
    public partial class TimberServiceClassKdfes
    {
        /// <summary>
        /// Service class 1, enumeration 0
        /// </summary>
        [XmlAttribute("service_class_0")]
        public double ServiceClass1 {get; set;}

        /// <summary>
        /// Service class 2, enumeration 1
        /// </summary>
        [XmlAttribute("service_class_1")]
        public double ServiceClass2 {get; set;}

        /// <summary>
        /// Service class 3, enumeration 2
        /// </summary>
        [XmlAttribute("service_class_2")]
        public double ServiceClass3 {get; set;}
                       
    }
}

