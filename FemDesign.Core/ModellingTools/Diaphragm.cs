using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.ModellingTools
{
    /// <summary>
    /// Represents a Diaphragm.
    /// </summary>
    [System.Serializable]
    public partial class Diaphragm: NamedEntityBase, IStructureElement, IStageElement
    {
        [XmlIgnore]
        private static int _diaphragmInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_diaphragmInstances;

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        [XmlElement("region", Order = 1)]
        public Geometry.Region Region { get; set; }

        /// <summary>
        /// Gets or sets the colouring.
        /// </summary>
        [XmlElement("colouring", Order = 2)]
        public EntityColor Colouring { get; set; }

        /// <summary>
        /// Gets or sets the stage id.
        /// </summary>
        [XmlAttribute("stage")]
        public int StageId { get; set; } = 1;

        private Diaphragm()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Diaphragm"/> class.
        /// </summary>
        /// <param name="region">the region.</param>
        /// <param name="identifier">the identifier.</param>
        public Diaphragm(Geometry.Region region, string identifier)
        {
            // create entity
            this.EntityCreated();

            // add properties
            this.Region = region;
            this.Identifier = identifier;
        }        
    }
}
