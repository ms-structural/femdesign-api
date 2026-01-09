using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using System.ComponentModel;
using System.Linq;
using FemDesign.Releases;

namespace FemDesign.Shells
{
    /// <summary>
    /// Represents a Panel.
    /// </summary>
    [System.Serializable]
    public partial class Panel: NamedEntityBase, IStructureElement, IStageElement, IShell
    {
        /// <summary>
        /// Panel instance counter
        /// </summary>
        private static int _panelInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_panelInstances;

        /// <summary>
        /// Coordinate system
        /// </summary>
        [XmlIgnore]
        private Geometry.Plane _plane;

        [XmlIgnore]
        private Geometry.Plane Plane
        {
            get
            {
                if (this._plane == null)
                {
                    this._plane = new Geometry.Plane(this.LocalOrigin, this.LocalX, this.LocalZ.Cross(this.LocalX));
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
                this._localOrigin = value.Origin;
                this._localX = value.LocalX;
            }
        }

         /// <summary>
        /// Deinfes the position of the local coordinate system
        /// </summary>
        [XmlElement("local_pos", Order = 4)]
        public Geometry.Point3d _localOrigin;
        [XmlIgnore]
        public Geometry.Point3d LocalOrigin
        {
            get
            {
                return this._localOrigin;
            }
            set
            {
                this.Plane.Origin = value;
                this._localOrigin = value;
            }
        }
        
        /// <summary>
        /// LocalX and direction of panels
        /// </summary>
        /// <value></value>
        [XmlElement("direction", Order = 2)]
        public Geometry.Vector3d _localX;
        [XmlIgnore]
        public Geometry.Vector3d LocalX
        {
            get
            {
                return this._localX;
            }
            set
            {
                this.Plane.SetXAroundZ(value);
                this._localX = this.Plane.LocalX;
            }
        }

        [XmlIgnore]
        public Geometry.Vector3d LocalY
        {
            get
            {
                return this.Plane.LocalY;
            }
            set
            {
                this.Plane.SetYAroundZ(value);
                this._localX = this.Plane.LocalX;
            }
        }

        /// <summary>
        /// LocalZ
        /// </summary>
        [XmlIgnore]
        public Geometry.Vector3d LocalZ
        {
            get
            {
                return this.Region.LocalZ;
            }
            set
            {
                this.Region.LocalZ = value;
                
                foreach (InternalPanel intPanel in this.InternalPanels.IntPanels)
                {
                    intPanel.Region.LocalZ = value;
                }
            }
        }

        /// <summary>
        /// Region of panel
        /// </summary>
        [XmlElement("region", Order = 1)]
        public Geometry.Region _region;
        [XmlIgnore]
        public Geometry.Region Region
        {
            get
            {
                return this._region;
            }
            set
            {
                //this._region = value.RemoveEdgeConnections();
                this._region = value;
            }
        }

        /// <summary>
        /// Defines position of base line when using panel type analytical model
        /// </summary>
        /// <value></value>
        [XmlElement("anchor_point", Order = 3)]
        public Geometry.Point3d AnchorPoint { get; set; }

        /// <summary>
        /// Gets or sets the internal panels.
        /// </summary>
        [XmlElement("internal_panels", Order = 5)]
        public InternalPanels InternalPanels { get; set; }

        /// <summary>
        /// This property describes the material of the panel (if the panel is a timber panel).
        /// The PanelLibraryData referenced in this property can be either Orthotropic, CLT or GLC.
        /// </summary>
        [XmlElement("timber_application_data", Order = 6)]
        public Materials.TimberPanelType TimberPanelData { get; set; }

        /// <summary>
        /// This is a placeholder for the PanelLibraryData which is referenced in TimberPanelData
        /// </summary>
        [XmlIgnore]
        public Materials.OrthotropicPanelLibraryType OrthotropicPanelLibraryData { get; set; }

        /// <summary>
        /// This is a placeholder for the PanelLibraryData which is referenced in TimberPanelData
        /// </summary>
        [XmlIgnore]
        public Materials.CltPanelLibraryType CltPanelLibraryData { get; set; }

        /// <summary>
        /// This is a placeholder for the PanelLibraryData which is referenced in TimberPanelData
        /// </summary>
        [XmlIgnore]
        public Materials.GlcPanelLibraryType GlcPanelLibraryData { get; set; }

        /// <summary>
        /// Gets or sets the camber.
        /// </summary>
        [XmlElement("camber_simulation", Order = 7)]
        public Camber Camber { get; set; }

        /// <summary>
        /// Gets or sets the internal plastic limit forces.
        /// </summary>
        [XmlElement("internal_plastic_limit_forces", Order = 8)]
        public Releases.PlasticityType3d InternalPlasticLimitForces { get; set; }

        /// <summary>
        /// Gets or sets the internal plastic limit moments.
        /// </summary>
        [XmlElement("internal_plastic_limit_moments", Order = 9)]
        public Releases.PlasticityType3d InternalPlasticLimitMoments { get; set; }

        /// <summary>
        /// Gets or sets the external plastic limit forces.
        /// </summary>
        [XmlElement("external_plastic_limit_forces", Order = 10)]
        public Releases.PlasticityType3d ExternalPlasticLimitForces { get; set; }

        /// <summary>
        /// Gets or sets the external plastic limit moments.
        /// </summary>
        [XmlElement("external_plastic_limit_moments", Order = 11)]
        public Releases.PlasticityType3d ExternalPlasticLimitMoments { get; set; }

        /// <summary>
        /// Gets or sets the internal stiffness.
        /// </summary>
        [XmlElement("internal_stiffness", Order = 12)]
        public Releases.StiffnessWithFriction InternalStiffness { get; set; }

        /// <summary>
        /// Gets or sets the external stiffness.
        /// </summary>
        [XmlElement("external_stiffness", Order = 13)]
        public Releases.StiffnessWithFriction ExternalStiffness { get; set; }

        /// <summary>
        /// Gets or sets the internal rigidity.
        /// </summary>
        [XmlElement("internal_rigidity", Order = 14)]
        public Releases.RigidityDataType3 InternalRigidity { get; set; }

        /// <summary>
        /// Gets or sets the internal predefined rigidity.
        /// </summary>
        [XmlElement("internal_predefined_rigidity", Order = 15)]
        public GuidListType InternalPredefinedRigidity { get; set; }

        /// <summary>
        /// Gets or sets the external rigidity.
        /// </summary>
        [XmlElement("external_rigidity", Order = 16)]
        public Releases.RigidityDataType3 ExternalRigidity { get; set; } // sets region border. region edgeconnection sets edgeconnection for specific edge. default should be hinged?

        /// <summary>
        /// Gets or sets the external predefined rigidity.
        /// </summary>
        [XmlElement("external_predefined_rigidity", Order = 17)]
        public GuidListType ExternalPredefinedRigidity { get; set; }

        /// <summary>
        /// Gets or sets the colouring.
        /// </summary>
        [XmlElement("colouring", Order = 18)]
        public EntityColor Colouring { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute("type")]
        public PanelType Type { get; set; }

        /// <summary>
        /// Gets or sets the complex material ref.
        /// </summary>
        [XmlAttribute("complex_material")]
        public System.Guid ComplexMaterialRef { get; set; }
        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        [XmlIgnore]
        public Materials.Material _material;
        [XmlIgnore]
        public Materials.Material Material
        {
            set
            {
                if (value.Concrete != null || value.Steel != null)
                {
                    // FEM-Design initially restricted the material to be concrete.
                    // Material type can also be set as steel
                    this.ComplexMaterialRef = value.Guid;
                    this._material = value;
                }
                else
                {
                    throw new System.ArgumentException("Only support for concrete and steel material have been added.");
                }
            }
            get
            {
                return this._material;
            }
        }
        /// <summary>
        /// Gets or sets the complex section.
        /// </summary>
        [XmlAttribute("complex_section")]
        public System.Guid ComplexSection;
        /// <summary>
        /// Gets or sets the section.
        /// </summary>
        [XmlIgnore]
        public Sections.Section _section;
        [XmlIgnore]
        public Sections.Section Section
        {
            set
            {
                this.ComplexSection = value.Guid;
                this._section = value;
            }
            get
            {
                return this._section;
            }
        }

        /// <summary>
        /// Gets or sets the panel name.
        /// </summary>
        [XmlAttribute("panelname")]
        public string PanelName { get; set; }
        /// <summary>
        /// Gets or sets the in situ fabricated.
        /// </summary>
        [XmlAttribute("in_situ_fabricated")]
        public bool InSituFabricated { get; set; } // deault="false"
        /// <summary>
        /// Gets or sets the gap.
        /// </summary>
        [XmlAttribute("gap")]
        public double Gap { get; set; }
        /// <summary>
        /// Gets or sets the orthotropy.
        /// </summary>
        [XmlAttribute("orthotropy")]
        public double _orthotropy; // orthotropy_type
        [XmlIgnore]
        public double Orthotropy
        {
            get
            {
                return this._orthotropy;
            }
            set
            {
                this._orthotropy = RestrictedDouble.NonNegMax_1(value);
            }
        }

        /// <summary>
        /// Only for struxml export to Revit and/or Tekla. Not used in calculation.
        /// </summary>
        [XmlAttribute("thickness")]
        public double Thickness { get; set; }
        /// <summary>
        /// Gets or sets the alignment.
        /// </summary>
        [XmlAttribute("alignment")]
        public VerticalAlignment Alignment { get; set; }
        /// <summary>
        /// Gets or sets the align offset.
        /// </summary>
        [XmlAttribute("align_offset")]
        public double AlignOffset { get; set; }
        /// <summary>
        /// Gets or sets the eccentricity calculation.
        /// </summary>
        [XmlAttribute("ecc_calc")]
        public bool EccentricityCalculation { get; set; } // default="false"
        /// <summary>
        /// Gets or sets the eccentricity by cracking.
        /// </summary>
        [XmlAttribute("ecc_crack")]
        public bool EccentricityByCracking { get; set; } // default="false"
        /// <summary>
        /// Gets or sets the internal moving local.
        /// </summary>
        [XmlAttribute("internal_moving_local")]
        public bool InternalMovingLocal { get; set; } // default="false"
        /// <summary>
        /// Gets or sets the external moving local.
        /// </summary>
        [XmlAttribute("external_moving_local")]
        public bool ExternalMovingLocal { get; set; } // default="false" according to strusoft.xsd. default="True" according to FD gui.
        /// <summary>
        /// Gets or sets the forced plate.
        /// </summary>
        [XmlAttribute("forced_plate")]
        public bool ForcedPlate { get; set; } // default="false" if false a vertical panel will be represented as a wall in FD, if true a vertical panel will not be represented as a wall
        /// <summary>
        /// Gets or sets the panel width.
        /// </summary>
        [XmlAttribute("panel_width")]
        public string _panelWidth; // positive_max_100, default="1.5"
        [XmlIgnore]
        public double PanelWidth
        {
            get
            {
                return System.Convert.ToDouble(_panelWidth, CultureInfo.InvariantCulture);
            }
            set
            {
                this._panelWidth = RestrictedDouble.NonNegMax_100(value).ToString(CultureInfo.InvariantCulture);
            }
        }
        [XmlIgnore]
        public double UniformAvgMeshSize
        {
            set
            {    
                foreach (InternalPanel intPanel in this.InternalPanels.IntPanels)
                {
                    intPanel.MeshSize = value;
                }    
            }
        }

        /// <summary>
        /// Gets or sets the stage id.
        /// </summary>
        [XmlAttribute("stage")]
        public int StageId { get; set; } = 1;


        /// <summary>
        /// Gets or sets the ignored distance.
        /// </summary>
        [XmlAttribute("ignored_distance")]
        [DefaultValue(0.02)]
        public double _ignoredDistance = 0.02;

        [XmlIgnore]
        public double IgnoredDistance
        {
            get
            {
                return this._ignoredDistance;
            }
            set
            {
                this._ignoredDistance = RestrictedDouble.NonNegMax_1000(value);
            }
        }

        /// <summary>
        /// Gets or sets the ignored in stability.
        /// </summary>
        [XmlAttribute("ignored_in_stability")]
        [DefaultValue(false)]
        public bool IgnoredInStability { get; set; } = false;

        /// <summary>
        /// Set external edge connections (i.e. set edge connections on the border). 
        /// When this is performed for panels the external rigidity is changed accordingly.
        /// </summary>
        /// <param name="ec">EdgeConnection</param>
        public void SetExternalEdgeConnections(EdgeConnection ec)
        {
            // set the edge connections of the external edges of the internal panels
            if (this.InternalPanels.IntPanels.Count == 1)
            {
                this.InternalPanels.IntPanels[0].Region.SetEdgeConnections(ec);
            }
            else
            {
                throw new System.ArgumentException("Can't set external edge connections for panels with more than 1 internal panel (i.e. can only set external edge conncetions for panels with a continuous analytical model)");
            }


            // set external rigidity property to reflect the majority of edge connections of the region
            if (ec.Rigidity != null)
                this.ExternalRigidity = ec.Rigidity;
            else if (ec.PredefRigidity != null)
                this.ExternalPredefinedRigidity = ec.PredefRigidity;
        }

        /// <summary>
        /// Set external edge connections (i.e. set edge connections on the border).
        /// When this is performed for panels the external rigidity is changed accordingly.
        /// </summary>
        /// <param name="edgeConnections">EdgeConnection</param>
        public void SetExternalEdgeConnections(List<EdgeConnection> edgeConnections)
        {
            // set the edge connections of the external edges of the internal panels
            if (this.InternalPanels.IntPanels.Count == 1)
            {
                this.InternalPanels.IntPanels[0].Region.SetEdgeConnections(edgeConnections);
            }
            else
            {
                throw new System.ArgumentException("Can't set external edge connections for panels with more than 1 internal panel (i.e. can only set external edge conncetions for panels with a continuous analytical model)");
            }

            // set external rigidity property
            this.ExternalRigidity = RigidityDataType3.RigidLine();

        }

        /// <summary>
        /// Gets the edge connections.
        /// </summary>
        /// <returns>The result.</returns>
        public List<EdgeConnection> GetEdgeConnections()
        {
            // set the edge connections of the external edges of the internal panels
            if (this.InternalPanels.IntPanels.Count == 1)
            {
                return this.InternalPanels.IntPanels[0].Region.GetEdgeConnections();
            }
            else
            {
                throw new System.ArgumentException("Can't set external edge connections for panels with more than 1 internal panel (i.e. can only set external edge conncetions for panels with a continuous analytical model)");
            }
        }

        /// <summary>
        /// Gets the edge connections.
        /// </summary>
        /// <param name="name">the name.</param>
        /// <returns>The result.</returns>
        public List<EdgeConnection> GetEdgeConnections(string name)
        {
            // set the edge connections of the external edges of the internal panels
            if (this.InternalPanels.IntPanels.Count == 1)
            {
                var edges = this.InternalPanels.IntPanels[0].Region.GetEdgeConnections();
                var edgeConnection = edges.Where(x => x != null && x.Name.Contains(name)).ToList();
                return edgeConnection;
            }
            else
            {
                throw new System.ArgumentException("Can't set external edge connections for panels with more than 1 internal panel (i.e. can only set external edge conncetions for panels with a continuous analytical model)");
            }
        }

        /// <summary>
        /// Gets the edge connection.
        /// </summary>
        /// <param name="guid">the guid.</param>
        /// <returns>The result.</returns>
        public EdgeConnection GetEdgeConnection(Guid guid)
        {
            // set the edge connections of the external edges of the internal panels
            if (this.InternalPanels.IntPanels.Count == 1)
            {
                var edges = this.InternalPanels.IntPanels[0].Region.GetEdgeConnections();
                var edgeConnection = edges.Where(x => x != null && x.Guid == guid).ToList().FirstOrDefault();
                return edgeConnection;
            }
            else
            {
                throw new System.ArgumentException("Can't set external edge connections for panels with more than 1 internal panel (i.e. can only set external edge conncetions for panels with a continuous analytical model)");
            }
        }

        /// <summary>
        /// Set external edge connection at index (i.e. set edge connections at specific edge of region).
        /// </summary>
        /// <param name="ec">EdgeConnection</param>
        /// <param name="index">Index of edge to set at region of first (and only) internal panel.</param>
        public void SetExternalEdgeConnectionAtIndexForContinousAnalyticalModel(EdgeConnection ec, int index)
        {
            if (this.InternalPanels.IntPanels.Count != 1)
            {
                throw new System.ArgumentException("Can't set external edge connections for panels with more than 1 internal panel (i.e. can only set external edge conncetions for panels with a continuous analytical model)");
            }

            if (index >= 0 & index < this.InternalPanels.IntPanels[0].Region.GetEdgeConnections().Count)
            {
                this.InternalPanels.IntPanels[0].Region.SetEdgeConnection(ec, index);
            }
            else
            {
                throw new System.ArgumentException("Index is out of bounds.");
            }
        }

        /// <summary>
        /// Set external edge connection at indices (i.e. set edge connections at specific edges of region).
        /// </summary>
        /// <param name="shellEdgeConnection">EdgeConnection.</param>
        /// <param name="indices">Index. List of items</param>
        public void SetExternalEdgeConnectionsForContinuousAnalyticalModel(EdgeConnection shellEdgeConnection, List<int> indices)
        {
            if (this.InternalPanels.IntPanels.Count != 1)
            {
                throw new System.ArgumentException("Can't set external edge connections for panels with more than 1 internal panel (i.e. can only set external edge conncetions for panels with a continuous analytical model)");
            }

            foreach (int index in indices)
            {
                if (index < 0 || index >= this.InternalPanels.IntPanels[0].Region.GetEdgeConnections().Count)
                {
                    throw new System.ArgumentException("Index is out of bounds.");
                }
                
                this.SetExternalEdgeConnectionAtIndexForContinousAnalyticalModel(shellEdgeConnection, index);  
            }
        }


        /// <summary>
        /// Updates the material.
        /// </summary>
        /// <param name="material">the material.</param>
        public void UpdateMaterial(Materials.Material material)
        {
            if (this.Type == PanelType.Concrete)
            {
                if (material.Family == Materials.Family.Concrete)
                    this.ComplexMaterialRef = material.Guid;
                else
                    throw new System.ArgumentException("Material must be of type Concrete.");
            }
            else if (this.Type == PanelType.Timber)
                throw new System.NotImplementedException("Panel with Timber Material can not be updated yet! Send a request.");
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private Panel()
        {

        }

        /// <summary>
        /// Construct standard panel with "Continuous" analytical model.
        /// </summary>
        /// <param name="region">Region of shell containing panels.</param>
        /// <param name="anchorPoint"></param>
        /// <param name="type">Type of panel.</param>
        /// <param name="identifier">Name of shell.</param>
        /// <param name="panelName">Name of panel.</param>
        /// <param name="gap">Gap between panels.</param>
        /// <param name="orthotropy">Orthotropy.</param>
        /// <param name="ecc">ShellEccentricity.</param>
        /// <param name="externalMovingLocal">EdgeConnection LCS changes along edge?</param>
        /// <param name="internalPanels"></param>
        /// <param name="externalEdgeConnections"></param>
        /// <param name="material"></param>
        /// <param name="section"></param>
        internal Panel(Geometry.Region region, Geometry.Point3d anchorPoint, InternalPanels internalPanels, List<EdgeConnection> externalEdgeConnections, PanelType type, Materials.Material material, Sections.Section section, string identifier, string panelName, double gap, double orthotropy, ShellEccentricity ecc, bool externalMovingLocal)
        {
            this.EntityCreated();

            // elements
            this.Region = region;
            this.Plane = region.Plane;
            this.AnchorPoint = anchorPoint;
            this.InternalPanels = internalPanels;
            this.ExternalRigidity = RigidityDataType3.RigidLine();

            // set edge connections
            this.SetExternalEdgeConnections(externalEdgeConnections);

            // attributes
            this.Type = type;
            this.Material = material; // note that material and section are not added directly to complexMaterial and complexSection fields.
            this.Section = section;
            this.Identifier = identifier;
            this.PanelName = panelName;
            this.Gap = gap;
            this.Orthotropy = orthotropy;
            this.Alignment = ecc.Alignment;
            this.AlignOffset = ecc.Eccentricity;
            this.EccentricityCalculation = ecc.EccentricityCalculation;
            this.EccentricityByCracking = ecc.EccentricityByCracking;
            this.ExternalMovingLocal = externalMovingLocal;
        }

        /// <summary>
        /// Construct timber panel with "Continuous" analytical model.
        /// </summary>
        /// <param name="region">Region of shell containing panels.</param>
        /// <param name="anchorPoint"></param>
        /// <param name="internalPanels"></param>
        /// <param name="externalEdgeConnections">Default value for shell border EdgeConnections. Can be overwritten by EdgeConnection for each specific edge in Region.</param>
        /// <param name="timberApplicationData"></param>
        /// <param name="type">Type of panel.</param>
        /// <param name="identifier">Name of shell.</param>
        /// <param name="panelName">Name of panel.</param>
        /// <param name="gap">Gap between panels.</param>
        /// <param name="orthotropy">Orthotropy.</param>
        /// <param name="ecc">ShellEccentricity.</param>
        /// <param name="externalMovingLocal">EdgeConnection LCS changes along edge?</param>
        /// <param name="panelWidth"></param>
        internal Panel(Geometry.Region region, Geometry.Point3d anchorPoint, InternalPanels internalPanels, Materials.TimberPanelType timberApplicationData, List<EdgeConnection> externalEdgeConnections, PanelType type, string identifier, string panelName, double gap, double orthotropy, ShellEccentricity ecc, bool externalMovingLocal, double panelWidth)
        {
            this.EntityCreated();

            // elements
            this.Region = region;
            this.Plane = region.Plane;
            this.AnchorPoint = anchorPoint;
            this.InternalPanels = internalPanels;
            this.TimberPanelData = timberApplicationData;

            // set external rigidity
            this.SetExternalEdgeConnections(externalEdgeConnections);

            // set internal rigidity - not relevant for a panel with continuous analytical model

            // attributes
            this.Type = type;
            this.Identifier = identifier;
            this.PanelName = panelName;
            this.Gap = gap;
            this.Orthotropy = orthotropy;
            this.Alignment = ecc.Alignment;
            this.AlignOffset = ecc.Eccentricity;
            this.EccentricityCalculation = ecc.EccentricityCalculation;
            this.EccentricityByCracking = ecc.EccentricityByCracking;
            this.ExternalMovingLocal = externalMovingLocal;
            this.PanelWidth = panelWidth;
        }

        /// <summary>
        /// Create a default concrete shell with panels using a continuous analytical model.
        /// </summary>
        /// <param name="region">Panel region.</param>
        /// <param name="externalEdgeConnection"></param>
        /// <param name="material"></param>
        /// <param name="section"></param>
        /// <param name="identifier">Name of shell.</param>
        /// <param name="orthotropy"></param>
        /// <param name="ecc"></param>
        /// <returns></returns>
        public static Panel DefaultContreteContinuous(Geometry.Region region, EdgeConnection externalEdgeConnection, Materials.Material material, Sections.Section section, string identifier, double orthotropy, ShellEccentricity ecc)
        {
            Geometry.Point3d anchorPoint = region.Contours[0].Edges[0].Points[0];
            InternalPanel internalPanel = new InternalPanel(region);
            InternalPanels internalPanels = new InternalPanels(internalPanel);
            PanelType type = PanelType.Concrete;
            string panelName = "A";
            double gap = 0.003;
            bool externalMovingLocal = externalEdgeConnection.MovingLocal;
            
            return new Panel(region, anchorPoint, internalPanels, new List<EdgeConnection> { externalEdgeConnection }, type, material, section, identifier, panelName, gap, orthotropy, ecc, externalMovingLocal);
        }

        /// <summary>
        /// Create a default concrete shell with panels using a continuous analytical model.
        /// </summary>
        /// <param name="region">Panel region.</param>
        /// <param name="externalEdgeConnections"></param>
        /// <param name="material"></param>
        /// <param name="section"></param>
        /// <param name="identifier">Name of shell.</param>
        /// <param name="orthotropy"></param>
        /// <param name="ecc"></param>
        /// <returns></returns>
        public static Panel DefaultContreteContinuous(Geometry.Region region, List<EdgeConnection> externalEdgeConnections, Materials.Material material, Sections.Section section, string identifier, double orthotropy, ShellEccentricity ecc)
        {
            if (externalEdgeConnections?.Count == 0 || externalEdgeConnections == null)
                externalEdgeConnections = new List<EdgeConnection> { EdgeConnection.Default};

            Geometry.Point3d anchorPoint = region.Contours[0].Edges[0].Points[0];
            InternalPanel internalPanel = new InternalPanel(region);
            InternalPanels internalPanels = new InternalPanels(internalPanel);
            PanelType type = PanelType.Concrete;
            string panelName = "A";
            double gap = 0.003;
            bool externalMovingLocal = true;

            return new Panel(region, anchorPoint, internalPanels, externalEdgeConnections, type, material, section, identifier, panelName, gap, orthotropy, ecc, externalMovingLocal);
        }

        /// <summary>
        /// Create a default timber shell with panels using a continuous analytical model.
        /// </summary>
        /// <param name="region">Panel region.</param>
        /// <param name="timberPlateMaterial">Timber material. See <see cref="FemDesign.Materials.TimberPanelType"/>.</param>
        /// <param name="direction">Timber panel span direction.</param>
        /// <param name="externalEdgeConnection"></param>
        /// <param name="identifier">Name of shell.</param>
        /// <param name="eccentricity"></param>
        /// <param name="panelWidth"></param>
        /// <returns></returns>
        public static Panel DefaultTimberContinuous(Geometry.Region region, Materials.TimberPanelType timberPlateMaterial, Geometry.Vector3d direction, EdgeConnection externalEdgeConnection = null, string identifier = "TP", ShellEccentricity eccentricity = null, double panelWidth = 1.5)
        {
            if (externalEdgeConnection == null)
                externalEdgeConnection = EdgeConnection.Default;

            if (eccentricity == null)
                eccentricity = ShellEccentricity.Default;
            
            Geometry.Point3d anchorPoint = region.Contours[0].Edges[0].Points[0];
            InternalPanel internalPanel = new InternalPanel(region);
            InternalPanels internalPanels = new InternalPanels(internalPanel);
            PanelType type = PanelType.Timber;
            string panelName = "A";
            double gap = 0.01;
            double orthotropy = 1;
            bool externalMovingLocal = externalEdgeConnection.MovingLocal;

            var panel = new Panel(region, anchorPoint, internalPanels, timberPlateMaterial, new List<EdgeConnection> { externalEdgeConnection }, type, identifier, panelName, gap, orthotropy, eccentricity, externalMovingLocal, panelWidth);

            panel.LocalX = direction; // Set timber panel span direction
            
            return panel;
        }

        /// <summary>
        /// Create a default timber shell with panels using a continuous analytical model.
        /// </summary>
        /// <param name="region">Panel region.</param>
        /// <param name="timberPlateMaterial">Timber material. See <see cref="FemDesign.Materials.TimberPanelType"/>.</param>
        /// <param name="direction">Timber panel span direction.</param>
        /// <param name="externalEdgeConnections"></param>
        /// <param name="identifier">Name of shell.</param>
        /// <param name="eccentricity"></param>
        /// <param name="panelWidth"></param>
        /// <returns></returns>
        public static Panel DefaultTimberContinuous(Geometry.Region region, Materials.TimberPanelType timberPlateMaterial, Geometry.Vector3d direction, List<EdgeConnection> externalEdgeConnections, string identifier = "TP", ShellEccentricity eccentricity = null, double panelWidth = 1.5)
        {

            if (externalEdgeConnections?.Count == 0 || externalEdgeConnections == null)
                externalEdgeConnections = new List<EdgeConnection> { EdgeConnection.Default};

            if (eccentricity == null)
                eccentricity = ShellEccentricity.Default;

            Geometry.Point3d anchorPoint = region.Contours[0].Edges[0].Points[0];
            InternalPanel internalPanel = new InternalPanel(region);
            InternalPanels internalPanels = new InternalPanels(internalPanel);
            PanelType type = PanelType.Timber;
            string panelName = "A";
            double gap = 0.01;
            double orthotropy = 1;
            bool externalMovingLocal = true;

            var panel = new Panel(region, anchorPoint, internalPanels, timberPlateMaterial, externalEdgeConnections, type, identifier, panelName, gap, orthotropy, eccentricity, externalMovingLocal, panelWidth);

            panel.LocalX = direction; // Set timber panel span direction

            return panel;
        }
    }
}