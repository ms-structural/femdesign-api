// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;

using FemDesign.GenericClasses;
using StruSoft.Interop.StruXml.Data;

namespace FemDesign.Bars
{
    /// <summary>
    /// Represents a Bar Part.
    /// 
    /// Underlying representation of a Bar-element.
    /// </summary>
    [System.Serializable]
    public partial class BarPart : NamedEntityPartBase, IStageElement
    {
        [XmlIgnore]
        private static int _barInstance = 0; // Number of bar/beam instances created
        [XmlIgnore]
        private static int _columnInstance = 0; // Number of column instances created
        [XmlIgnore]
        private static int _trussInstance = 0; // Number of truss instances created
        protected override int GetUniqueInstanceCount()
        {
            switch (this.Type)
            {
                case BarType.Beam:
                    return ++_barInstance;
                case BarType.Column:
                    return ++_columnInstance;
                case BarType.Truss:
                    return ++_trussInstance;
                default:
                    throw new System.ArgumentException($"Incorrect type of bar: {this.Type}");
            }
        }

        /// <summary>
        /// Edge field
        /// </summary>
        [XmlElement("curve", Order = 1)]
        public Geometry.Edge _edge;

        /// <summary>
        /// Gets or sets the stage id.
        /// </summary>
        [XmlAttribute("stage")]
        public int StageId { get; set; } = 1;

        /// <summary>
        /// Edge property
        /// </summary>
        [XmlIgnore]
        public Geometry.Edge Edge
        {
            get
            {
                if (this._edge.Type == "line")
                {
                    this._edge.Normal = this.LocalY;
                }

                return this._edge;
            }
            set
            {
                if (this.Type == BarType.Beam)
                {
                    // check if line or arc
                    if (value.Type == "line" || value.Type == "arc")
                    {
                        // pass
                    }
                    else
                    {
                        throw new System.ArgumentException($"Edge type: {value.Type}, is not line or arc. Circle is not supported. Consider splitting the Circle in two arches.");
                    }
                }
                else if (this.Type == BarType.Column)
                {
                    // check if line
                    if (!value.IsLine())
                    {
                        throw new System.ArgumentException($"Edge type: {value.Type}, is not line.");
                    }

                    // check if line local x equals positive global Z
                    if (!value.IsLineTangentEqualToGlobalZ())
                    {
                        throw new System.ArgumentException("Edge (Line) must be vertial with its tangent equal to positive global Z for column definition");
                    }
                }
                else if (this.Type == BarType.Truss)
                {
                    // check if line
                    if (!value.IsLine())
                    {
                        throw new System.ArgumentException($"Edge type: {value.Type}, is not line.");
                    }
                }
                else
                {
                    throw new System.ArgumentException($"Incorrect type of bar: {this.Type}");
                }

                // set value
                this._edge = value;

                // update cooridnate system to fit new edge
                this.Plane = value.Plane;
            }
        }

        [XmlIgnore]
        private Geometry.Plane _plane;

        [XmlIgnore]
        private Geometry.Plane Plane
        {
            get
            {
                if (this._plane == null)
                {
                    this._plane = this.Edge.Plane;
                    return this._plane;
                }
                else
                {
                    return this._plane;
                }
            }
            set
            {
                this._plane = value;
                this._localY = value.LocalY;
            }
        }

        [XmlIgnore]
        public Geometry.Point3d LocalOrigin
        {
            get
            {
                return this.Plane.Origin;
            }
        }

        [XmlIgnore]
        public Geometry.Vector3d LocalX
        {
            get
            {
                return this.Plane.LocalX;
            }
        }

        /// <summary>
        /// Gets or sets the local y.
        /// </summary>
        [XmlElement("local-y", Order = 2)]
        public Geometry.Vector3d _localY;

        [XmlIgnore]
        public Geometry.Vector3d LocalY
        {
            get
            {
                return this._localY;
            }
            set
            {
                this.Plane.SetYAroundX(value);
                this._localY = this.Plane.LocalY;
            }
        }

        [XmlIgnore]
        public Geometry.Vector3d LocalZ
        {
            get
            {
                return this.Plane.LocalZ;
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlIgnore]
        public BarType Type;

        [XmlIgnore]
        public SectionType SectionType
        {
            get
            {
                if (this.Type == BarType.Truss)
                {
                    return SectionType.Truss;
                }
                else if (this.Type != BarType.Truss && this.HasComplexSectionRef && !this.HasDeltaBeamComplexSectionRef)
                {
                    return SectionType.RegularBeamColumn;
                }
                else if (this.Type != BarType.Truss && this.HasComplexCompositeRef)
                {
                    return SectionType.CompositeBeamColumn;
                }
                else if (this.Type != BarType.Truss && this.HasComplexSectionRef && this.HasDeltaBeamComplexSectionRef)
                {
                    return SectionType.DeltaBeamColumn;
                }
                else
                {
                    throw new System.ArgumentException("Type of bar is not supported.");
                }
            }
        }

        /// <summary>
        /// Gets or sets the complex composite ref.
        /// </summary>
        [XmlAttribute("complex_composite")]
        public System.Guid _complexCompositeRef;

        /// <summary>
        /// Should Serialize complex Composite Ref.
        /// </summary>
        /// <returns>The result.</returns>
        public bool ShouldSerialize_complexCompositeRef()
        {
            return this.HasComplexCompositeRef;
        }

        [XmlIgnore]
        public System.Guid ComplexCompositeRef
        {
            get
            {
                return this._complexCompositeRef;
            }
            set
            {
                this._complexCompositeRef = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether s complex composite ref.
        /// </summary>
        [XmlIgnore]
        public bool HasComplexCompositeRef { get => this.ComplexCompositeRef != System.Guid.Empty; }

        /// <summary>
        /// Gets or sets the complex composite obj.
        /// </summary>
        [XmlIgnore]
        public Composites.ComplexComposite _complexCompositeObj;

        [XmlIgnore]
        public Composites.ComplexComposite ComplexCompositeObj
        {
            get
            {
                return this._complexCompositeObj;
            }
            set
            {
                this._complexCompositeObj = value;
                this.ComplexCompositeRef = value.Guid;

                // Composite bars BarPart doesn't have ComplexMaterial and ComplexSection attributes
                if (this.HasComplexSectionRef)
                {
                    this.ComplexSectionObj = null;
                    this.ComplexSectionRef = null;
                }
                if (this.HasComplexMaterialRef)
                {
                    this.ComplexMaterialObj = null;
                    this.ComplexMaterialRef = System.Guid.Empty;
                }
            }
        }

        /// <summary>
        /// Gets or sets the complex material ref.
        /// </summary>
        [XmlAttribute("complex_material")]
        public System.Guid _complexMaterialRef;

        /// <summary>
        /// Should Serialize complex Material Ref.
        /// </summary>
        /// <returns>The result.</returns>
        public bool ShouldSerialize_complexMaterialRef()
        {
            return this.HasComplexMaterialRef;
        }

        [XmlIgnore]
        public System.Guid ComplexMaterialRef
        {
            get
            {
                return this._complexMaterialRef;
            }
            set
            {
                this._complexMaterialRef = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether s complex material ref.
        /// </summary>
        [XmlIgnore]
        public bool HasComplexMaterialRef { get => this.ComplexMaterialRef != System.Guid.Empty; }

        /// <summary>
        /// Material field
        /// </summary>
        [XmlIgnore]
        public Materials.Material _complexMaterialObj;

        /// <summary>
        /// Material property. When a new material is set the ComplexMaterialRef is updated.
        /// </summary>
        [XmlIgnore]
        public Materials.Material ComplexMaterialObj
        {
            get
            {
                return this._complexMaterialObj;
            }
            set
            {
                this._complexMaterialObj = value;
                this.ComplexMaterialRef = this._complexMaterialObj.Guid;
            }
        }

        /// <summary>
        /// Gets or sets the complex section ref.
        /// </summary>
        [XmlAttribute("complex_section")]
        public string _complexSectionRef;

        /// <summary>
        /// Should Serialize complex Section Ref.
        /// </summary>
        /// <returns>The result.</returns>
        public bool ShouldSerialize_complexSectionRef()
        {
            return this.HasComplexSectionRef;
        }

        [XmlIgnore]
        public string ComplexSectionRef
        {
            get
            {
                // if truss --> guid from TrussUniformSectionObj
                if (this.Type == BarType.Truss)
                {
                    if (this.TrussUniformSectionObj == null)
                    {
                        return this._complexSectionRef;
                    }
                    else
                    {
                        var r = this.TrussUniformSectionObj.Guid.ToString();
                        this._complexSectionRef = r;
                        return r;
                    }
                }
                // else --> guid from ComplexSectionObj
                else
                {
                    if (this.ComplexSectionObj == null)
                    {
                        return this._complexSectionRef;
                    }
                    else
                    {
                        var r = this.ComplexSectionObj.Guid.ToString();
                        this._complexSectionRef = r;
                        return r;
                    }

                }
            }
            set
            {
                this._complexSectionRef = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether s complex section ref.
        /// </summary>
        [XmlIgnore]
        public bool HasComplexSectionRef { get => this.ComplexSectionRef != null; }

        /// <summary>
        /// Gets a value indicating whether s delta beam complex section ref.
        /// </summary>
        [XmlIgnore]
        public bool HasDeltaBeamComplexSectionRef { get => !System.Guid.TryParse(this.ComplexSectionRef, out System.Guid result); }

        /// <summary>
        /// Gets or sets the complex section obj.
        /// </summary>
        [XmlIgnore]
        public Sections.ComplexSection _complexSectionObj;

        [XmlIgnore]
        public Sections.ComplexSection ComplexSectionObj
        {
            get
            {
                return this._complexSectionObj;
            }
            set
            {
                this._complexSectionObj = value;
                this._complexSectionRef = this._complexSectionObj.Guid.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the truss uniform section obj.
        /// </summary>
        [XmlIgnore]
        public Sections.Section TrussUniformSectionObj;

        /// <summary>
        /// Gets or sets the steel made type.
        /// </summary>
        [XmlIgnore]
        public SteelMadeType? SteelMadeType;

        [XmlAttribute("made")]
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public SteelMadeType steelMadeType
        {
            get { return SteelMadeType.Value; }
            set { SteelMadeType = value; }
        }

        /// <summary>
        /// Should Serializesteel Made Type.
        /// </summary>
        /// <returns>The result.</returns>
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializesteelMadeType() => SteelMadeType.HasValue;

        /// <summary>
        /// Gets or sets the eccentricity calc.
        /// </summary>
        [XmlAttribute("ecc_calc")]
        public bool EccentricityCalc { get; set; } // bool


        /// <summary>
        /// Gets or sets the connectivity.
        /// </summary>
        [XmlElement("connectivity", Order = 3)]
        public Connectivity[] _connectivity = new Connectivity[2]; // connectivity_type

        [XmlIgnore]
        public Connectivity[] Connectivity
        {
            get
            {
                return this._connectivity;
            }
            set
            {
                if (this.Type == BarType.Truss)
                {
                    throw new System.Exception("Can't set connectivity to a truss");
                }
                else if (value.Length == 1)
                {
                    this._connectivity[0] = value[0];
                    this._connectivity[1] = value[0];
                }
                else if (value.Length == 2)
                {
                    this._connectivity[0] = value[0];
                    this._connectivity[1] = value[1];
                }
                else
                {
                    throw new System.ArgumentException($"Incorrect length of Connectivities: {value.Length}. Length should be 1 or 2");
                }
            }
        }

        /// <summary>
        /// Gets or sets the eccentricity type field.
        /// </summary>
        [XmlElement("eccentricity", Order = 4)]
        public ModelEccentricity _eccentricityTypeField;

        [XmlIgnore]
        public ModelEccentricity _eccentricityTypeProperty
        {
            get
            {
                // truss has no eccentricity
                if (this.Type == BarType.Truss)
                {
                    return null;
                }
                // beam and column have eccentricity and analytical eccentricity
                // must equal complexSectionObj eccentricity
                else if (this.HasComplexSectionRef && !this.HasDeltaBeamComplexSectionRef)
                {
                    var sectionEcc = this.ComplexSectionObj.Eccentricities;

                    if (this._eccentricityTypeField == null)
                    {
                        Eccentricity eccentricity = new Eccentricity();
                        this._eccentricityTypeField = new ModelEccentricity(eccentricity);
                    }

                    this._eccentricityTypeField.StartAnalytical = sectionEcc.First();
                    this._eccentricityTypeField.EndAnalytical = sectionEcc.Last();
                    return this._eccentricityTypeField;
                }
                else
                {
                    return this._eccentricityTypeField;
                }
            }
            set
            {
                this._eccentricityTypeField = value;
            }
        }

        [XmlIgnore]
        public Eccentricity[] AnalyticalEccentricity
        {
            get
            {
                if (this.Type == BarType.Truss)
                {
                    throw new System.Exception("Truss has no eccentricity");
                }
                else if (this.HasComplexSectionRef)
                {
                    return this.ComplexSectionObj.Eccentricities;
                }
                else
                {
                    return this._eccentricityTypeProperty.Analytical;
                }
            }
            set
            {
                if (this.Type == BarType.Truss)
                {
                    throw new System.Exception("Truss has no eccentricity");
                }
                else if (this.HasComplexSectionRef)
                {
                    // create new complex section obj
                    var sections = this.ComplexSectionObj.Sections;
                    var pos = this.ComplexSectionObj.Positions;
                    this.ComplexSectionObj = new Sections.ComplexSection(sections, pos, value);
                }
                else if (this.HasComplexCompositeRef)
                {
                    this._eccentricityTypeProperty.Analytical = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the buckling data.
        /// </summary>
        [XmlElement("buckling_data", Order = 5)]
        public Buckling.BucklingData BucklingData { get; set; } // buckling_data_type

        /// <summary>
        /// Gets or sets the camber type2d.
        /// </summary>
        [XmlElement("camber_type_2d", Order = 6)]
        public StruSoft.Interop.StruXml.Data.Camber_type_2d CamberType2d { get; set; }

        /// <summary>
        /// Gets or sets the stiffness modifiers.
        /// </summary>
        [XmlElement("stiffness_modifiers", Order = 7)]
        public List<BarStiffnessFactors> StiffnessModifiers { get; set; }

        /// <summary>
        /// Gets or sets the colouring.
        /// </summary>
        [XmlElement("colouring", Order = 8)]
        public EntityColor Colouring { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        [XmlElement("end", Order = 9)]
        public string End = "";

        /// <summary>
        /// Gets or sets the ecc crack.
        /// </summary>
        [XmlAttribute("ecc_crack")]
        [DefaultValue(false)]
        public bool EccCrack { get; set; }

        /// <summary>
        /// Gets or sets the first order analysis u.
        /// </summary>
        [XmlAttribute("first_order_analysis_U")]
        [DefaultValue(false)]
        public bool FirstOrderAnalysisU { get; set; }

        /// <summary>
        /// Gets or sets the first order analysis sq.
        /// </summary>
        [XmlAttribute("first_order_analysis_Sq")]
        [DefaultValue(false)]
        public bool FirstOrderAnalysisSq { get; set; }

        /// <summary>
        /// Gets or sets the first order analysis sf.
        /// </summary>
        [XmlAttribute("first_order_analysis_Sf")]
        [DefaultValue(false)]
        public bool FirstOrderAnalysisSf { get; set; }

        /// <summary>
        /// Gets or sets the first order analysis sc.
        /// </summary>
        [XmlAttribute("first_order_analysis_Sc")]
        [DefaultValue(false)]
        public bool FirstOrderAnalysisSc { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private BarPart()
        {

        }

        /// <summary>
        /// Construct beam or column with uniform section and uniform start/end conditions.
        /// </summary>
        /// <param name="edge">the edge.</param>
        /// <param name="type">the type.</param>
        /// <param name="material">the material.</param>
        /// <param name="section">the section.</param>
        /// <param name="eccentricity">the eccentricity.</param>
        /// <param name="connectivity">the connectivity.</param>
        /// <param name="identifier">the identifier.</param>
        public BarPart(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section section, Eccentricity eccentricity, Connectivity connectivity, string identifier)
        {
            if (type == BarType.Truss)
            {
                throw new System.ArgumentException($"Type: {type.ToString()}, is not of type {BarType.Beam.ToString()} or {BarType.Column.ToString()}");
            }
            else
            {
                this.EntityCreated();
                this.Type = type;
                this.Edge = edge;
                this.ComplexMaterialObj = material;
                this.ComplexSectionObj = new Sections.ComplexSection(section, eccentricity);
                this.Connectivity = new Connectivity[1] { connectivity };
                this.EccentricityCalc = true;
                this.Identifier = identifier;
            }

            this.CheckMaterialAndSectionType();
        }

        /// <summary>
        /// Construct beam or column with uniform section and different start/end conditions.
        /// </summary>
        /// <param name="edge">the edge.</param>
        /// <param name="type">the type.</param>
        /// <param name="material">the material.</param>
        /// <param name="section">the section.</param>
        /// <param name="startEccentricity">the start eccentricity.</param>
        /// <param name="endEccentricity">the end eccentricity.</param>
        /// <param name="startConnectivity">the start connectivity.</param>
        /// <param name="endConnectivity">the end connectivity.</param>
        /// <param name="identifier">the identifier.</param>
        public BarPart(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section section, Eccentricity startEccentricity, Eccentricity endEccentricity, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
            if (type == BarType.Truss)
            {
                throw new System.ArgumentException($"Type: {type.ToString()}, is not of type {BarType.Beam.ToString()} or {BarType.Column.ToString()}");
            }
            else
            {
                this.EntityCreated();
                this.Type = type;
                this.Edge = edge;
                this.ComplexMaterialObj = material;
                this.ComplexSectionObj = new Sections.ComplexSection(section, section, startEccentricity, endEccentricity);
                this.Connectivity = new Connectivity[2] { startConnectivity, endConnectivity };
                this.EccentricityCalc = true;
                this.Identifier = identifier;
            }

            this.CheckMaterialAndSectionType();
        }

        /// <summary>
        /// Construct beam or column with start/end section and different start/end conditions
        /// </summary>
        /// <param name="edge">the edge.</param>
        /// <param name="type">the type.</param>
        /// <param name="material">the material.</param>
        /// <param name="startSection">the start section.</param>
        /// <param name="endSection">the end section.</param>
        /// <param name="startEccentricity">the start eccentricity.</param>
        /// <param name="endEccentricity">the end eccentricity.</param>
        /// <param name="startConnectivity">the start connectivity.</param>
        /// <param name="endConnectivity">the end connectivity.</param>
        /// <param name="identifier">the identifier.</param>
        public BarPart(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section startSection, Sections.Section endSection, Eccentricity startEccentricity, Eccentricity endEccentricity, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
            if (type == BarType.Truss)
            {
                throw new System.ArgumentException($"Type: {type.ToString()}, is not of type {BarType.Beam.ToString()} or {BarType.Column.ToString()}");
            }
            else
            {
                this.EntityCreated();
                this.Type = type;
                this.Edge = edge;
                this.ComplexMaterialObj = material;
                this.ComplexSectionObj = new Sections.ComplexSection(startSection, endSection, startEccentricity, endEccentricity);
                this.Connectivity = new Connectivity[2] { startConnectivity, endConnectivity };
                this.EccentricityCalc = true;
                this.Identifier = identifier;
            }

            this.CheckMaterialAndSectionType();
        }

        /// <summary>
        /// Construct beam or column with non-uniform section and start/end conditions
        /// </summary>
        /// <param name="edge">the edge.</param>
        /// <param name="type">the type.</param>
        /// <param name="material">the material.</param>
        /// <param name="sections">the sections.</param>
        /// <param name="eccentricities">the eccentricities.</param>
        /// <param name="connectivities">the connectivities.</param>
        /// <param name="identifier">the identifier.</param>
        public BarPart(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section[] sections, Eccentricity[] eccentricities, Connectivity[] connectivities, string identifier)
        {
            if (type == BarType.Truss)
            {
                throw new System.ArgumentException($"Type: {type.ToString()}, is not of type {BarType.Beam.ToString()} or {BarType.Column.ToString()}");
            }
            else
            {
                this.EntityCreated();
                this.Type = type;
                this.Edge = edge;
                this.ComplexMaterialObj = material;
                this.ComplexSectionObj = new Sections.ComplexSection(sections, eccentricities);
                this.Connectivity = connectivities;
                this.EccentricityCalc = true;
                this.Identifier = identifier;
            }

            this.CheckMaterialAndSectionType();
        }

        /// <summary>
        /// Construct beam or column with non-uniform section and start/end conditions
        /// </summary>
        /// <param name="edge">the edge.</param>
        /// <param name="type">the type.</param>
        /// <param name="material">the material.</param>
        /// <param name="sections">the sections.</param>
        /// <param name="positions">the positions.</param>
        /// <param name="eccentricities">the eccentricities.</param>
        /// <param name="startConnectivity">the start connectivity.</param>
        /// <param name="endConnectivity">the end connectivity.</param>
        /// <param name="identifier">the identifier.</param>
        public BarPart(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section[] sections, double[] positions, Eccentricity[] eccentricities, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
            if (type == BarType.Truss)
            {
                throw new System.ArgumentException($"Type: {type.ToString()}, is not of type {BarType.Beam.ToString()} or {BarType.Column.ToString()}");
            }
            else
            {
                this.EntityCreated();
                this.Type = type;
                this.Edge = edge;
                this.ComplexMaterialObj = material;
                this.ComplexSectionObj = new Sections.ComplexSection(sections, positions, eccentricities);
                this.Connectivity = new Connectivity[2] { startConnectivity, endConnectivity };
                this.EccentricityCalc = true;
                this.Identifier = identifier;
            }

            this.CheckMaterialAndSectionType();
        }

        /// <summary>
        /// Construct BarPart (truss)
        /// </summary>
        /// <param name="edge">the edge.</param>
        /// <param name="type">the type.</param>
        /// <param name="material">the material.</param>
        /// <param name="section">the section.</param>
        /// <param name="identifier">the identifier.</param>
        public BarPart(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section section, string identifier)
        {
            if (type != BarType.Truss)
            {
                throw new System.ArgumentException($"Type: {type.ToString()}, is not of type {BarType.Truss.ToString()}");
            }
            else
            {
                this.EntityCreated();
                this.Type = type;
                this.Edge = edge;
                this.ComplexMaterialObj = material;
                this.TrussUniformSectionObj = section;
                this.Identifier = identifier;
            }

            this.CheckMaterialAndSectionType();
        }

        /// <summary>
        /// Construct a composite barpart with uniform section and uniform start/end conditions.
        /// </summary>
        /// <param name="edge">the edge.</param>
        /// <param name="type">the type.</param>
        /// <param name="compositeSection">the composite section.</param>
        /// <param name="eccentricity">the eccentricity.</param>
        /// <param name="connectivity">the connectivity.</param>
        /// <param name="identifier">the identifier.</param>
        public BarPart(Geometry.Edge edge, BarType type, Composites.CompositeSection compositeSection, Eccentricity eccentricity, Connectivity connectivity, string identifier)
        {
            if (type == BarType.Truss)
            {
                throw new System.ArgumentException($"Type: {type.ToString()}, is not of type {BarType.Beam.ToString()} or {BarType.Column.ToString()}");
            }
            else
            {
                this.EntityCreated();
                this.Type = type;
                this.Edge = edge;
                this.ComplexCompositeObj = new Composites.ComplexComposite(compositeSection);
                this.EccentricityCalc = true;
                this._eccentricityTypeProperty = new ModelEccentricity(eccentricity, true);
                this.Connectivity = new Connectivity[1] { connectivity };
                this.Identifier = identifier;
            }
        }

        /// <summary>
        /// Construct a composite barpart with uniform section and different start/end conditions.
        /// </summary>
        /// <param name="edge">the edge.</param>
        /// <param name="type">the type.</param>
        /// <param name="compositeSection">the composite section.</param>
        /// <param name="startEccentricity">the start eccentricity.</param>
        /// <param name="endEccentricity">the end eccentricity.</param>
        /// <param name="startConnectivity">the start connectivity.</param>
        /// <param name="endConnectivity">the end connectivity.</param>
        /// <param name="identifier">the identifier.</param>
        public BarPart(Geometry.Edge edge, BarType type, Composites.CompositeSection compositeSection, Eccentricity startEccentricity, Eccentricity endEccentricity, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
            if (type == BarType.Truss)
            {
                throw new System.ArgumentException($"Type: {type.ToString()}, is not of type {BarType.Beam.ToString()} or {BarType.Column.ToString()}");
            }
            else
            {
                this.EntityCreated();
                this.Type = type;
                this.Edge = edge;
                this.ComplexCompositeObj = new Composites.ComplexComposite(compositeSection);
                this.EccentricityCalc = true;
                this._eccentricityTypeProperty = new ModelEccentricity(startEccentricity, endEccentricity, true);
                this.Connectivity = new Connectivity[2] { startConnectivity, endConnectivity };
                this.Identifier = identifier;
            }
        }


        /// <summary>
        /// Orient this object's coordinate system to GCS
        /// </summary>
        public void OrientCoordinateSystemToGCS()
        {
            var cs = this.Plane;
            cs.AlignYAroundXToGcs();
            this.Plane = cs;
        }

        /// <summary>
        /// This method checks if the material type of a bar is consistent with the materials used in its sections.<br></br>
        /// If the bar's material is custom, the check is skipped.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the material type of the bar does not match the material type of any section.</exception>
        private void CheckMaterialAndSectionType()
        {
            // get BarPart's material
            var material = this.ComplexMaterialObj.Family;

            // section type check for custom material is unneccessary
            if (material == Materials.Family.Custom)
                return;


            // get BarPart's sections materials
            List<string> secMats = null;
            if (this.ComplexSectionObj != null)     // if it is not a composite bar
            {
                secMats = this.ComplexSectionObj.Sections.Select(s => s.MaterialFamily).ToList();
            }
            else if (this.TrussUniformSectionObj != null)   // if it is a truss
            {
                secMats = new List<string> { this.TrussUniformSectionObj.MaterialFamily };
            }
            
            // check section material type
            if(secMats != null)
            {
                foreach(var item in secMats)
                {
                    string mat;
                    if (item == "Custom")
                        continue;
                    if (item == "Hollow")
                        mat = "Concrete";
                    else
                        mat = item;

                    if (mat != material.ToString())
                        throw new ArgumentException($"Material Family ({material}) must be the same as the Section MaterialFamily ({mat})!");
                }
            }
        }
    }
}