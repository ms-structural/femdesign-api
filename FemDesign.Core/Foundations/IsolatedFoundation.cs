using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;

using FemDesign.GenericClasses;
using FemDesign.Geometry;

namespace FemDesign.Foundations
{
    /// <summary>
    /// Represents a Isolated Foundation.
    /// </summary>
    [System.Serializable]
    public partial class IsolatedFoundation : NamedEntityBase, IStructureElement, IFoundationElement, IStageElement
    {
        [XmlIgnore]
        internal static int _instance = 0;

        protected override int GetUniqueInstanceCount() => ++_instance;

        /// <summary>
        /// Gets or sets the bedding modulus.
        /// </summary>
        [XmlAttribute("bedding_modulus")]
        [DefaultValue(10000)]
        public double BeddingModulus { get; set; } = 10000;

        /// <summary>
        /// Gets or sets the stage id.
        /// </summary>
        [XmlAttribute("stage")]
        [DefaultValue(1)]
        public int StageId { get; set; } = 1;

        /// <summary>
        /// Gets or sets the connection point.
        /// </summary>
        [XmlElement("connection_point", Order = 1)]
        public Point3d ConnectionPoint { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        [XmlElement("direction", Order = 2)]
        public Vector3d Direction { get; set; }

        /// <summary>
        /// Gets or sets the extruded solid.
        /// </summary>
        [XmlElement("extruded_solid", Order = 3)]
        public ExtrudedSolid ExtrudedSolid { get; set; }

        /// <summary>
        /// Gets or sets the parts.
        /// </summary>
        [XmlElement("referable_parts", Order = 4)]
        public RefParts Parts { get; set; }
        
        /// <summary>
        /// Gets or sets the insulation.
        /// </summary>
        [XmlElement("insulation", Order = 5)]
        public Insulation Insulation { get; set; }

        /// <summary>
        /// Gets or sets the colouring.
        /// </summary>
        [XmlElement("colouring", Order = 6)]
        public EntityColor Colouring { get; set; }

        /// <summary>
        /// Gets or sets the foundation system.
        /// </summary>
        [XmlAttribute("analythical_system")]
        [DefaultValue(FoundationSystem.Simple)]
        public FoundationSystem FoundationSystem { get; set; }

        #region MATERIAL

        /// <summary>
        /// Gets or sets the complex material ref.
        /// </summary>
        [XmlAttribute("complex_material")]
        public string _complexMaterialRef;

        [XmlIgnore]
        public System.Guid ComplexMaterialRef
        {
            get
            {
                return System.Guid.Parse(this._complexMaterialRef);
            }
            set
            {
                this._complexMaterialRef = value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the complex material obj.
        /// </summary>
        [XmlIgnore]
        public Materials.Material _complexMaterialObj;

        [XmlIgnore]
        public Materials.Material ComplexMaterialObj
        {
            get
            {
                return this._complexMaterialObj;
            }
            set
            {
                if(value.Concrete == null) { throw new System.ArgumentException("Material type must be concrete"); }
                this._complexMaterialObj = value;
                this.ComplexMaterialRef = this._complexMaterialObj.Guid;
            }
        }

        #endregion


        private IsolatedFoundation()
        {
        }

#if !ISDYNAMO
        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedFoundation"/> class.
        /// </summary>
        /// <param name="solid">the solid.</param>
        /// <param name="bedding">the bedding.</param>
        /// <param name="material">the material.</param>
        /// <param name="plane">the plane.</param>
        /// <param name="insulation">the insulation.</param>
        /// <param name="foundationSystem">the foundation system.</param>
        /// <param name="identifier">the identifier.</param>
        public IsolatedFoundation(ExtrudedSolid solid, double bedding, Materials.Material material, Plane plane, Insulation insulation = null, FoundationSystem foundationSystem = FoundationSystem.Simple, string identifier = "F")
        {
            this.Initialise(plane, solid, bedding, material, identifier);
            this.BeddingModulus = bedding;
            this.Insulation = insulation;

            if (foundationSystem == FoundationSystem.FromSoil)
            {
                throw new InvalidEnumArgumentException("FromSoil is not a valid input for Isolated Foundation!");
            }
            this.FoundationSystem = foundationSystem;
            this.Parts = foundationSystem == FoundationSystem.SurfaceSupportGroup ? new RefParts(true) : new RefParts(false);
        }
#endif



        private void Initialise(Plane plane, ExtrudedSolid solid, double bedding, Materials.Material material, string identifier = "F")
        {
            this.EntityCreated();
            this.ConnectionPoint = plane.Origin;
            this.Direction = plane.LocalX;
            this.ExtrudedSolid = solid;
            this.ComplexMaterialObj = material;
            this.BeddingModulus = bedding;
            this.Identifier = identifier;
        }


    }
}
