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
    public partial class SlabFoundation : NamedEntityBase, IStructureElement, IFoundationElement, IStageElement
    {
        [XmlIgnore]
        internal static int _instance = 0;
        protected override int GetUniqueInstanceCount() => ++_instance;

        [XmlElement("slab_part", Order = 1)]
        public FemDesign.Shells.SlabPart SlabPart { get; set; }

        [XmlElement("referable_parts", Order = 2)]
        public RefParts Parts { get; set; }

        [XmlElement("insulation", Order = 3)]
        public Insulation Insulation { get; set; }

        [XmlElement("colouring", Order = 4)]
        public EntityColor Colouring { get; set; }

        [XmlAttribute("analythical_system")]
        public FoundationSystem FoundationSystem = FoundationSystem.SurfaceSupportGroup;

        [XmlAttribute("bedding_modulus")]
        [DefaultValue(10000)]
        public double BeddingModulus { get; set; } = 10000;

        [XmlAttribute("bedding_modulus_x")]
        [DefaultValue(5000)]
        public double BeddingModulusX { get; set; } = 5000;

        [XmlAttribute("bedding_modulus_y")]
        [DefaultValue(5000)]
        public double BeddingModulusY { get; set; } = 5000;

        [XmlAttribute("stage")]
        [DefaultValue(1)]
        public int StageId { get; set; } = 1;

        [XmlIgnore]
        public Materials.Material Material { get; set; }

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
