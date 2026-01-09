// https://strusoft.com/
using System;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using FemDesign.Releases;


namespace FemDesign.Supports
{
    /// <summary>
    /// Represents a Surface Support.
    /// </summary>
    [System.Serializable]
    public partial class SurfaceSupport: NamedEntityBase, IStructureElement, ISupportElement, IStageElement
    {
        protected override int GetUniqueInstanceCount() => ++PointSupport._instance; // PointSupport and SurfaceSupport share the same instance counter.

        /// <summary>
        /// Gets or sets the stage id.
        /// </summary>
        [XmlAttribute("stage")]
        public int StageId { get; set; } = 1;

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        [XmlElement("region", Order= 1)]
        public Geometry.Region Region { get; set; }
        
        /// <summary>
        /// Gets or sets the rigidity.
        /// </summary>
        [XmlElement("rigidity", Order= 2)]
        public RigidityDataType1 Rigidity { get; set; }

        /// <summary>
        /// Gets or sets the predef rigidity ref.
        /// </summary>
        [XmlElement("predefined_rigidity", Order = 3)]
        public GuidListType _predefRigidityRef; // reference_type

        /// <summary>
        /// Gets or sets the predef rigidity.
        /// </summary>
        [XmlIgnore]
        public RigidityDataLibType1 _predefRigidity;

        [XmlIgnore]
        public RigidityDataLibType1 PredefRigidity
        {
            get
            {
                return this._predefRigidity;
            }
            set
            {
                this._predefRigidity = value;
                this._predefRigidityRef = new GuidListType(value.Guid);
            }
        }

        /// <summary>
        /// Gets or sets the plane.
        /// </summary>
        [XmlElement("local_system", Order= 4)]
        public Geometry.Plane Plane { get; set; }

        /// <summary>
        /// Gets or sets the colouring.
        /// </summary>
        [XmlElement("colouring", Order = 5)]
        public EntityColor Colouring { get; set; }
        /// <summary>
        /// Gets or sets the motions.
        /// </summary>
        public Motions Motions { get { return Rigidity?.Motions; } }
        /// <summary>
        /// Gets or sets the motions plasticity limits.
        /// </summary>
        public MotionsPlasticLimits MotionsPlasticityLimits { get { return Rigidity?.PlasticLimitForces; } }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private SurfaceSupport()
        {

        }

        /// <summary>
        /// Create surface support
        /// </summary>
        /// <param name="region">the region.</param>
        /// <param name="rigidity">the rigidity.</param>
        /// <param name="identifier">the identifier.</param>
        public SurfaceSupport(Geometry.Region region, RigidityDataType1 rigidity, string identifier = "S")
        {
            Initialize(region, rigidity, identifier);
        }

        /// <summary>
        /// Create surface support with only translation rigidity defined. 
        /// </summary>
        /// <param name="region">the region.</param>
        /// <param name="motions">the motions.</param>
        /// <param name="identifier">the identifier.</param>
        public SurfaceSupport(Geometry.Region region, Motions motions, string identifier = "S")
        {
            var rigidity = new RigidityDataType1(motions);
            Initialize(region, rigidity, identifier);
        }

        /// <summary>
        /// Create surface support with only translation rigidity and force plastic limits defined. 
        /// </summary>
        /// <param name="region">the region.</param>
        /// <param name="motions">the motions.</param>
        /// <param name="motionsPlasticLimits">the motions plastic limits.</param>
        /// <param name="identifier">the identifier.</param>
        [Obsolete("Constructor will be removed in FD 24.")]
        public SurfaceSupport(Geometry.Region region, Motions motions, MotionsPlasticLimits motionsPlasticLimits, string identifier = "S")
        {
            var rigidity = new RigidityDataType1(motions, motionsPlasticLimits);
            Initialize(region, rigidity, identifier);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceSupport"/> class.
        /// </summary>
        /// <param name="region">the region.</param>
        /// <param name="motions">the motions.</param>
        /// <param name="motionsPlasticLimits">the motions plastic limits.</param>
        /// <param name="detachType">the detach type.</param>
        /// <param name="identifier">the identifier.</param>
        public SurfaceSupport(Geometry.Region region, Motions motions, MotionsPlasticLimits motionsPlasticLimits, DetachType detachType = DetachType.None, string identifier = "S")
        {
            var rigidity = new RigidityDataType1(motions, motionsPlasticLimits, detachType);
            Initialize(region, rigidity, identifier);
        }

        private void Initialize(Geometry.Region region, RigidityDataType1 rigidity, string identifier)
        {
            this.EntityCreated();
            this.Identifier = identifier;
            this.Region = region;
            this.Rigidity = rigidity;
            this.Plane = region.Plane;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            bool hasPlasticLimit = false;
            if (this.Rigidity != null)
			{
                if(this.Rigidity.PlasticLimitForces != null)
                    hasPlasticLimit = true;
                return $"{this.GetType().Name}; Coord: x: {this.Plane.LocalX} y: {this.Plane.LocalY} z: {this.Plane.LocalZ}; {this.Rigidity.Motions}; PlasticLimit: {hasPlasticLimit}";
			}
            else
                return $"{this.GetType().Name}; Coord: x: {this.Plane.LocalX} y: {this.Plane.LocalY} z: {this.Plane.LocalZ}; {this.PredefRigidity.Rigidity.Motions}; PlasticLimit: {hasPlasticLimit}";
        }
    }
}