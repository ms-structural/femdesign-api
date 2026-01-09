using FemDesign.GenericClasses;
using FemDesign.Materials;
using FemDesign.Shells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Foundations
{
    /// <summary>
    /// Represents a Slab Foundation.
    /// </summary>
    public partial class SlabFoundation : NamedEntityBase, IStructureElement, IFoundationElement, IStageElement
    {
        [XmlIgnore]
        internal static int _instance = 0;
        protected override int GetUniqueInstanceCount() => ++_instance;

        /// <summary>
        /// Gets or sets the slab part.
        /// </summary>
        [XmlElement("slab_part", Order = 1)]
        public FemDesign.Shells.SlabPart SlabPart { get; set; }

        /// <summary>
        /// Gets or sets the parts.
        /// </summary>
        [XmlElement("referable_parts", Order = 2)]
        public RefParts Parts { get; set; }

        /// <summary>
        /// Gets or sets the insulation.
        /// </summary>
        [XmlElement("insulation", Order = 3)]
        public Insulation Insulation { get; set; }

        /// <summary>
        /// Gets or sets the colouring.
        /// </summary>
        [XmlElement("colouring", Order = 4)]
        public EntityColor Colouring { get; set; }

        /// <summary>
        /// Gets or sets the foundation system.
        /// </summary>
        [XmlAttribute("analythical_system")]
        public FoundationSystem FoundationSystem = FoundationSystem.SurfaceSupportGroup;

        /// <summary>
        /// Gets or sets the bedding modulus.
        /// </summary>
        [XmlAttribute("bedding_modulus")]
        [DefaultValue(10000)]
        public double BeddingModulus { get; set; } = 10000;

        /// <summary>
        /// Gets or sets the bedding modulus x.
        /// </summary>
        [XmlAttribute("bedding_modulus_x")]
        public double BeddingModulusX { get; set; }

        /// <summary>
        /// Gets or sets the bedding modulus y.
        /// </summary>
        [XmlAttribute("bedding_modulus_y")]
        public double BeddingModulusY { get; set; }

        /// <summary>
        /// Gets or sets the stage id.
        /// </summary>
        [XmlAttribute("stage")]
        [DefaultValue(1)]
        public int StageId { get; set; } = 1;

        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        [XmlIgnore]
        public Materials.Material Material { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlabFoundation"/> class.
        /// </summary>
        public SlabFoundation()
        {
        }

        /// <summary>
        /// Construct Slab.
        /// </summary>
        private SlabFoundation(string identifier, SlabPart slabPart, Materials.Material material)
        {
            this.EntityCreated();
            this.SlabPart = slabPart;
            this.Identifier = identifier;
            this.Material = material;
            this.Parts = new RefParts() { RefSlab = this.SlabPart.Guid, RefSupport = Guid.NewGuid() };
        }

        /// <summary>
        /// Plate.
        /// </summary>
        /// <param name="identifier">the identifier.</param>
        /// <param name="material">the material.</param>
        /// <param name="region">the region.</param>
        /// <param name="shellEdgeConnection">the shell edge connection.</param>
        /// <param name="eccentricity">the eccentricity.</param>
        /// <param name="orthotropy">the orthotropy.</param>
        /// <param name="thickness">the thickness.</param>
        /// <param name="bedding">the bedding.</param>
        /// <param name="beddingX">the bedding x.</param>
        /// <param name="beddingY">the bedding y.</param>
        /// <param name="insulation">the insulation.</param>
        /// <returns>The result.</returns>
        public static SlabFoundation Plate(string identifier, Materials.Material material, Geometry.Region region, EdgeConnection shellEdgeConnection, ShellEccentricity eccentricity, ShellOrthotropy orthotropy, List<Thickness> thickness, double bedding = 10000, double beddingX = 5000, double beddingY = 5000, Insulation insulation = null)
        {
            SlabType type = SlabType.Plate;
            SlabPart slabPart = SlabPart.Define(type, identifier, region, thickness, material, shellEdgeConnection, eccentricity, orthotropy);
            SlabFoundation shell = new SlabFoundation(identifier, slabPart, material);
            shell.BeddingModulus = bedding;
            shell.BeddingModulusX = beddingX;
            shell.BeddingModulusY = beddingY;
            shell.Insulation = insulation;
            return shell;
        }

    }
}
