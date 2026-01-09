// https://strusoft.com/
using System;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using FemDesign.GenericClasses;

namespace FemDesign.Calculate
{
    
    /// <summary>
    /// Represents a Cmd Global Cfg.
    /// </summary>
    [XmlRoot("cmdglobalcfg")]
    [System.Serializable]
    public partial class CmdGlobalCfg : CmdCommand
    {
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        [XmlAttribute("command")]
        public string Command = "$ FEM $CODE(GLOBALCFG)"; // token

        /// <summary>
        /// Gets or sets the mesh general.
        /// </summary>
        [XmlElement("mesh_general")]
        public MeshGeneral MeshGeneral { get; set; }

        /// <summary>
        /// Gets or sets the mesh elements.
        /// </summary>
        [XmlElement("mesh_elements")]
        public MeshElements MeshElements { get; set; }

        /// <summary>
        /// Gets or sets the meshfunctions.
        /// </summary>
        [XmlElement("mesh_functions")]
        public MeshFunctions Meshfunctions { get; set; }

        /// <summary>
        /// Gets or sets the mesh prepare.
        /// </summary>
        [XmlElement("mesh_prepare")]
        public MeshPrepare MeshPrepare { get; set; }

        /// <summary>
        /// Gets or sets the peaksm method.
        /// </summary>
        [XmlElement("peaksm_method")]
        public PeaksmMethod PeaksmMethod { get; set; }

        /// <summary>
        /// Gets or sets the peaksm auto.
        /// </summary>
        [XmlElement("peaksm_auto")]
        public PeaksmAuto PeaksmAuto { get; set; }

        /// <summary>
        /// Gets or sets the soil calculation.
        /// </summary>
        [XmlElement("soil_calculation")]
        public SoilCalculation SoilCalculation { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdGlobalCfg()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdGlobalCfg"/> class.
        /// </summary>
        /// <param name="meshGeneral">the mesh general.</param>
        /// <param name="meshElements">the mesh elements.</param>
        /// <param name="meshFunctions">the mesh functions.</param>
        /// <param name="meshPrepare">the mesh prepare.</param>
        /// <param name="peaksmMethod">the peaksm method.</param>
        /// <param name="peaksmAuto">the peaksm auto.</param>
        /// <param name="soilCalculation">the soil calculation.</param>
        public CmdGlobalCfg(MeshGeneral meshGeneral, MeshElements meshElements, MeshFunctions meshFunctions, MeshPrepare meshPrepare, PeaksmMethod peaksmMethod, PeaksmAuto peaksmAuto, SoilCalculation soilCalculation)
        {
            this.MeshGeneral = meshGeneral;
            this.MeshElements = meshElements;
            this.Meshfunctions = meshFunctions;
            this.MeshPrepare = meshPrepare;
            this.PeaksmMethod = peaksmMethod;
            this.PeaksmAuto = peaksmAuto;
            this.SoilCalculation = soilCalculation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdGlobalCfg"/> class.
        /// </summary>
        /// <param name="globConfigs">the glob configs.</param>
        public CmdGlobalCfg(params GlobConfig[] globConfigs)
        {
            this.Initialize(globConfigs.ToList());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdGlobalCfg"/> class.
        /// </summary>
        /// <param name="globConfigs">the glob configs.</param>
        public CmdGlobalCfg(List<GlobConfig> globConfigs)
        {
            this.Initialize(globConfigs);
        }

        private void Initialize(List<GlobConfig> globConfigs)
        {
            //this.MeshGeneral = MeshGeneral.Default();
            //this.MeshElements = MeshElements.Default();
            //this.Meshfunctions = MeshFunctions.Default();
            //this.MeshPrepare = MeshPrepare.Default();
            //this.PeaksmMethod = PeaksmMethod.Default();
            //this.PeaksmAuto = PeaksmAuto.Default();
            //this.SoilCalculation = SoilCalculation.Default();

            List<string> types = new List<string>();
            foreach (var config in globConfigs)
            {
                string type = config.GetType().Name;
                if (types.Contains(type))
                    throw new Exception($"The input list contains items of the same type. You can only specify one {type} object in the input list!");

                switch (type)
                {
                    case nameof(Calculate.MeshGeneral):
                        this.MeshGeneral = (MeshGeneral)config;
                        break;
                    case nameof(Calculate.MeshElements):
                        this.MeshElements = (MeshElements)config;
                        break;
                    case nameof(Calculate.MeshFunctions):
                        this.Meshfunctions = (MeshFunctions)config;
                        break;
                    case nameof(Calculate.MeshPrepare):
                        this.MeshPrepare = (MeshPrepare)config;
                        break;
                    case nameof(Calculate.PeaksmMethod):
                        this.PeaksmMethod = (PeaksmMethod)config;
                        break;
                    case nameof(Calculate.PeaksmAuto):
                        this.PeaksmAuto = (PeaksmAuto)config;
                        break;
                    case nameof(Calculate.SoilCalculation):
                        this.SoilCalculation = (SoilCalculation)config;
                        break;
                    case null:
                        throw new ArgumentNullException("Input has null elements!");
                    default:
                        throw new ArgumentException($"Input has elemets with invalid type! Valid types are: {nameof(Calculate.SoilCalculation)}, {nameof(Calculate.MeshGeneral)}, {nameof(Calculate.MeshElements)}, " +
                            $"{nameof(Calculate.MeshFunctions)}, {nameof(Calculate.MeshPrepare)}, {nameof(Calculate.PeaksmMethod)}, {nameof(Calculate.PeaksmAuto)}");
                }

                types.Add(type);
            }
        }

        /// <summary>
        /// Default.
        /// </summary>
        /// <returns>The result.</returns>
        public static CmdGlobalCfg Default()
        {
            var meshGeneral = MeshGeneral.Default();
            var meshElements = MeshElements.Default();
            var meshfunctions = MeshFunctions.Default();
            var meshPrepare = MeshPrepare.Default();
            var peaksmMethod = PeaksmMethod.Default();
            var peaksmAuto = PeaksmAuto.Default();
            var soilCalculation = SoilCalculation.Default();

            var cmdGlobalCfg = new CmdGlobalCfg(meshGeneral,
                                                meshElements,
                                                meshfunctions,
                                                meshPrepare,
                                                peaksmMethod,
                                                peaksmAuto,
                                                soilCalculation);

            return cmdGlobalCfg;
        }


        /// <summary>
        /// Deserialize CmdGlobalCfg from resource.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="filePath">the file path.</param>
        public static CmdGlobalCfg DeserializeCmdGlobalCfgFromFilePath(string filePath)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(CmdGlobalCfg));
            TextReader reader = new StreamReader(filePath);
            object obj = deserializer.Deserialize(reader);
            var materialDatabase = (CmdGlobalCfg)obj;
            reader.Close();
            return materialDatabase;
        }


        /// <summary>
        /// To X Element.
        /// </summary>
        /// <returns>The result.</returns>
        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdGlobalCfg>(this);
        }

    }


    /// <summary>
    /// Represents a Mesh General.
    /// </summary>
    public partial class MeshGeneral : GlobConfig
    {
        /// <summary>
        /// Gets or sets the adjust to loads.
        /// </summary>
        [XmlAttribute("fAdjustToLoads")]
        public int _adjustToLoads;

        [XmlIgnore]
        public bool AdjustToLoads 
        { 
            get => Convert.ToBoolean(_adjustToLoads);
            set => _adjustToLoads = Convert.ToInt32(value);
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MeshGeneral()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeshGeneral"/> class.
        /// </summary>
        /// <param name="adjustToLoad">the adjust to load.</param>
        public MeshGeneral(bool adjustToLoad = false)
        {
            AdjustToLoads = adjustToLoad;
        }

        /// <summary>
        /// Default.
        /// </summary>
        /// <returns>The result.</returns>
        public static MeshGeneral Default()
        {
            return new MeshGeneral(adjustToLoad : false);
        }
    }

    /// <summary>
    /// Represents a Mesh Elements.
    /// </summary>
    public partial class MeshElements : GlobConfig
    {
        /// <summary>
        /// Gets or sets the elem calc region.
        /// </summary>
        [XmlAttribute("fElemCalcRegion")]
        public int _elemCalcRegion;

        [XmlIgnore]
        public bool ElemCalcRegion
        {
            get => Convert.ToBoolean(_elemCalcRegion);
            set => _elemCalcRegion = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the elem size div.
        /// </summary>
        [XmlAttribute("rElemSizeDiv")]
        public double ElemSizeDiv { get; set; }

        /// <summary>
        /// Gets or sets the correct to min div num.
        /// </summary>
        [XmlAttribute("fCorrectToMinDivNum")]
        public int _correctToMinDivNum;

        [XmlIgnore]
        public bool CorrectToMinDivNum
        {
            get => Convert.ToBoolean(_correctToMinDivNum);
            set => _correctToMinDivNum = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the default division.
        /// </summary>
        [XmlAttribute("sDefaultDivision")]
        public int _defaultDivision;

        [XmlIgnore]
        public int DefaultDivision 
        {
            get => _defaultDivision;
            set => _defaultDivision = RestrictedInteger.DefaultBarElemDiv(value);
        }

        /// <summary>
        /// Gets or sets the default angle.
        /// </summary>
        [XmlAttribute("rDefaultAngle")]
        public double _defaultAngle;

        [XmlIgnore]
        public double DefaultAngle
        {
            get => _defaultAngle;
            set => _defaultAngle = RestrictedDouble.NonNegMax_90(value);
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MeshElements()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeshElements"/> class.
        /// </summary>
        /// <param name="elemCalcRegion">the elem calc region.</param>
        /// <param name="elemeSizeDiv">the eleme size div.</param>
        /// <param name="correctToMinDivNum">the correct to min div num.</param>
        /// <param name="defaultDiv">the default div.</param>
        /// <param name="defaultAngle">the default angle.</param>
        public MeshElements(bool elemCalcRegion = true, double elemeSizeDiv = 6.0, bool correctToMinDivNum = true, int defaultDiv = 2, double defaultAngle = 15.0)
        {
            this.ElemCalcRegion = elemCalcRegion;
            this.ElemSizeDiv = elemeSizeDiv;
            this.CorrectToMinDivNum = correctToMinDivNum;
            this.DefaultDivision = defaultDiv;
            this.DefaultAngle = defaultAngle;
        }

        /// <summary>
        /// Default.
        /// </summary>
        /// <returns>The result.</returns>
        public static MeshElements Default()
        {
            return new MeshElements(elemCalcRegion : true);
        }

    }

    /// <summary>
    /// Represents a Mesh Functions.
    /// </summary>
    public partial class MeshFunctions : GlobConfig
    {
        /// <summary>
        /// Gets or sets the refine locally.
        /// </summary>
        [XmlAttribute("fRefineLocally")]
        public int _refineLocally;

        [XmlIgnore]
        public bool RefineLocally
        {
            get => Convert.ToBoolean(_refineLocally);
            set => _refineLocally = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the refine max step num.
        /// </summary>
        [XmlAttribute("sRefineMaxStepNum")]
        public int RefineMaxStepNum { get; set; }

        /// <summary>
        /// Gets or sets the max iter warning.
        /// </summary>
        [XmlAttribute("fMaxIterWarning")]
        public int _maxIterWarning;

        [XmlIgnore]
        public bool MaxIterWarning
        {
            get => Convert.ToBoolean(_maxIterWarning);
            set => _maxIterWarning = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the reduce size.
        /// </summary>
        [XmlAttribute("fReduceSize")]
        public int _reduceSize;

        [XmlIgnore]
        public bool ReduceSize
        {
            get => Convert.ToBoolean(_reduceSize);
            set => _reduceSize = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the smooth step num.
        /// </summary>
        [XmlAttribute("sSmoothStepNum")]
        public int _smoothStepNum;

        [XmlIgnore]
        public int SmoothStepNum
        {
            get => _smoothStepNum;
            set => _smoothStepNum = RestrictedInteger.MeshSmoothSteps(value);
        }

        /// <summary>
        /// Gets or sets the check mesh geom.
        /// </summary>
        [XmlAttribute("fCheckMeshGeom")]
        public int _checkMeshGeom;

        [XmlIgnore]
        public bool CheckMeshGeom
        {
            get => Convert.ToBoolean(_checkMeshGeom);
            set => _checkMeshGeom = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the check geom min angle.
        /// </summary>
        [XmlAttribute("rCheckGeomMinAngle")]
        public double _checkGeomMinAngle;

        [XmlIgnore]
        public double CheckGeomMinAngle
        {
            get => _checkGeomMinAngle;
            set => _checkGeomMinAngle = RestrictedDouble.NonNegMax_90(value);
        }

        /// <summary>
        /// Gets or sets the check geom max angle.
        /// </summary>
        [XmlAttribute("rCheckGeomMaxAngle")]
        public double _checkGeomMaxAngle;

        [XmlIgnore]
        public double CheckGeomMaxAngle
        {
            get => _checkGeomMaxAngle;
            set => _checkGeomMaxAngle = RestrictedDouble.MeshMaxAngle(value);
        }

        /// <summary>
        /// Gets or sets the check geom max side ratio.
        /// </summary>
        [XmlAttribute("rCheckGeomMaxSideRatio")]
        public double _checkGeomMaxSideRatio;

        [XmlIgnore]
        public double CheckGeomMaxSideRatio
        {
            get => _checkGeomMaxSideRatio;
            set => _checkGeomMaxSideRatio = RestrictedDouble.MeshMaxRatio(value);
        }

        /// <summary>
        /// Gets or sets the check mesh over lap.
        /// </summary>
        [XmlAttribute("fCheckMeshOverlap")]
        public int _checkMeshOverLap;

        [XmlIgnore]
        public bool CheckMeshOverLap
        {
            get => Convert.ToBoolean(_checkMeshOverLap);
            set => _checkMeshOverLap = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the check mesh topology.
        /// </summary>
        [XmlAttribute("fCheckMeshTopology")]
        public int _checkMeshTopology;

        [XmlIgnore]
        public bool CheckMeshTopology
        {
            get => Convert.ToBoolean(_checkMeshTopology);
            set => _checkMeshTopology = Convert.ToInt32(value);
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MeshFunctions()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeshFunctions"/> class.
        /// </summary>
        /// <param name="refineLocally">the refine locally.</param>
        /// <param name="refineMaxStepNum">the refine max step num.</param>
        /// <param name="iterWarning">the iter warning.</param>
        /// <param name="reduceSize">the reduce size.</param>
        /// <param name="smoothStepNum">the smooth step num.</param>
        /// <param name="checkMeshGeom">the check mesh geom.</param>
        /// <param name="checkGeomMinangle">the check geom minangle.</param>
        /// <param name="checkGeomMaxangle">the check geom maxangle.</param>
        /// <param name="checkGeomMaxSideRatio">the check geom max side ratio.</param>
        /// <param name="checkMeshOverlap">the check mesh overlap.</param>
        /// <param name="checkMeshTopology">the check mesh topology.</param>
        public MeshFunctions(bool refineLocally = true, int refineMaxStepNum = 5, bool iterWarning = false, bool reduceSize = true, int smoothStepNum = 3, bool checkMeshGeom = true, double checkGeomMinangle = 10.0, double checkGeomMaxangle = 170.0, double checkGeomMaxSideRatio = 8.0, bool checkMeshOverlap = true, bool checkMeshTopology = true)
        {
            this.RefineLocally = refineLocally;
            this.RefineMaxStepNum = refineMaxStepNum;
            this.MaxIterWarning = iterWarning;
            this.ReduceSize = reduceSize;
            this.SmoothStepNum = smoothStepNum;
            this.CheckMeshGeom = checkMeshGeom;
            this.CheckGeomMinAngle = checkGeomMinangle;
            this.CheckGeomMaxAngle = checkGeomMaxangle;
            this.CheckGeomMaxSideRatio = checkGeomMaxSideRatio;
            this.CheckMeshOverLap = checkMeshOverlap;
            this.CheckMeshTopology = checkMeshTopology;
        }

        /// <summary>
        /// Default.
        /// </summary>
        /// <returns>The result.</returns>
        public static MeshFunctions Default()
        {
            return new MeshFunctions(refineLocally : true);
        }
    }

    /// <summary>
    /// Represents a Mesh Prepare.
    /// </summary>
    public partial class MeshPrepare : GlobConfig
    {
        /// <summary>
        /// Gets or sets the auto regen.
        /// </summary>
        [XmlAttribute("fAutoRegen")]
        public int _autoRegen;

        [XmlIgnore]
        public bool AutoRegen
        {
            get => Convert.ToBoolean(_autoRegen);
            set => _autoRegen = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th peak.
        /// </summary>
        [XmlAttribute("fThPeak")]
        public int _thPeak;

        [XmlIgnore]
        public bool ThPeak
        {
            get => Convert.ToBoolean(_thPeak);
            set => _thPeak = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th beam.
        /// </summary>
        [XmlAttribute("fThBeam")]
        public int _thBeam;

        [XmlIgnore]
        public bool ThBeam
        {
            get => Convert.ToBoolean(_thBeam);
            set => _thBeam = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th column.
        /// </summary>
        [XmlAttribute("fThColumn")]
        public int _thColumn;

        [XmlIgnore]
        public bool ThColumn
        {
            get => Convert.ToBoolean(_thColumn);
            set => _thColumn = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th truss.
        /// </summary>
        [XmlAttribute("fThTruss")]
        public int _thTruss;

        [XmlIgnore]
        public bool ThTruss
        {
            get => Convert.ToBoolean(_thTruss);
            set => _thTruss = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th fic beam.
        /// </summary>
        [XmlAttribute("fThFicBeam")]
        public int _thFicBeam;

        [XmlIgnore]
        public bool ThFicBeam
        {
            get => Convert.ToBoolean(_thFicBeam);
            set => _thFicBeam = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th free edge.
        /// </summary>
        [XmlAttribute("fThFreeEdge")]
        public int _thFreeEdge;

        [XmlIgnore]
        public bool ThFreeEdge
        {
            get => Convert.ToBoolean(_thFreeEdge);
            set => _thFreeEdge = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th region bordger.
        /// </summary>
        [XmlAttribute("fThRegionBorder")]
        public int _thRegionBordger;

        [XmlIgnore]
        public bool ThRegionBordger
        {
            get => Convert.ToBoolean(_thRegionBordger);
            set => _thRegionBordger = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th suppt pt.
        /// </summary>
        [XmlAttribute("fThSuppPt")]
        public int _thSupptPt;

        [XmlIgnore]
        public bool ThSupptPt
        {
            get => Convert.ToBoolean(_thSupptPt);
            set => _thSupptPt = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th supp ln.
        /// </summary>
        [XmlAttribute("fThSuppLn")]
        public int _thSuppLn;

        [XmlIgnore]
        public bool ThSuppLn
        {
            get => Convert.ToBoolean(_thSuppLn);
            set => _thSuppLn = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th supp sf.
        /// </summary>
        [XmlAttribute("fThSuppSf")]
        public int _thSuppSf;

        [XmlIgnore]
        public bool ThSuppSf
        {
            get => Convert.ToBoolean(_thSuppSf);
            set => _thSuppSf = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th edge conn.
        /// </summary>
        [XmlAttribute("fThEdgeConn")]
        public int _thEdgeConn;

        [XmlIgnore]
        public bool ThEdgeConn
        {
            get => Convert.ToBoolean(_thEdgeConn);
            set => _thEdgeConn = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th conn pt.
        /// </summary>
        [XmlAttribute("fThConnPt")]
        public int _thConnPt;

        [XmlIgnore]
        public bool ThConnPt
        {
            get => Convert.ToBoolean(_thConnPt);
            set => _thConnPt = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th conn ln.
        /// </summary>
        [XmlAttribute("fThConnLn")]
        public int _thConnLn;

        [XmlIgnore]
        public bool ThConnLn
        {
            get => Convert.ToBoolean(_thConnLn);
            set => _thConnLn = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th conn sf.
        /// </summary>
        [XmlAttribute("fThConnSf")]
        public int _thConnSf;

        [XmlIgnore]
        public bool ThConnSf
        {
            get => Convert.ToBoolean(_thConnSf);
            set => _thConnSf = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th load pt.
        /// </summary>
        [XmlAttribute("fThLoadPt")]
        public int _thLoadPt;

        [XmlIgnore]
        public bool ThLoadPt
        {
            get => Convert.ToBoolean(_thLoadPt);
            set => _thLoadPt = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th load ln.
        /// </summary>
        [XmlAttribute("fThLoadLn")]
        public int _thLoadLn;

        [XmlIgnore]
        public bool ThLoadLn
        {
            get => Convert.ToBoolean(_thLoadLn);
            set => _thLoadLn = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th load sf.
        /// </summary>
        [XmlAttribute("fThLoadSf")]
        public int _thLoadSf;

        [XmlIgnore]
        public bool ThLoadSf
        {
            get => Convert.ToBoolean(_thLoadSf);
            set => _thLoadSf = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th fix pt.
        /// </summary>
        [XmlAttribute("fThFixPt")]
        public int _thFixPt;

        [XmlIgnore]
        public bool ThFixPt
        {
            get => Convert.ToBoolean(_thFixPt);
            set => _thFixPt = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the th fix ln.
        /// </summary>
        [XmlAttribute("fThFixLn")]
        public int _thFixLn;

        [XmlIgnore]
        public bool ThFixLn
        {
            get => Convert.ToBoolean(_thFixLn);
            set => _thFixLn = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the auto rebuild.
        /// </summary>
        [XmlAttribute("fAutoRebuild")]
        public int _autoRebuild;

        [XmlIgnore]
        public bool AutoRebuild
        {
            get => Convert.ToBoolean(_autoRebuild);
            set => _autoRebuild = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the auto smooth.
        /// </summary>
        [XmlAttribute("fAutoSmooth")]
        public int _autoSmooth;

        [XmlIgnore]
        public bool AutoSmooth
        {
            get => Convert.ToBoolean(_autoSmooth);
            set => _autoSmooth = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the auto check.
        /// </summary>
        [XmlAttribute("fAutoCheck")]
        public int _autoCheck;

        [XmlIgnore]
        public bool AutoCheck
        {
            get => Convert.ToBoolean(_autoCheck);
            set => _autoCheck = Convert.ToInt32(value);
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MeshPrepare()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MeshPrepare"/> class.
        /// </summary>
        /// <param name="autoRegen">the auto regen.</param>
        /// <param name="thPeak">the th peak.</param>
        /// <param name="thBeam">the th beam.</param>
        /// <param name="thColumn">the th column.</param>
        /// <param name="thTruss">the th truss.</param>
        /// <param name="thFicBeam">the th fic beam.</param>
        /// <param name="thFreeEdge">the th free edge.</param>
        /// <param name="thRegionBorder">the th region border.</param>
        /// <param name="thSuppPt">the th supp pt.</param>
        /// <param name="thSuppLn">the th supp ln.</param>
        /// <param name="thSuppSf">the th supp sf.</param>
        /// <param name="thEdgeConn">the th edge conn.</param>
        /// <param name="thConnPt">the th conn pt.</param>
        /// <param name="thConnLn">the th conn ln.</param>
        /// <param name="thConnSf">the th conn sf.</param>
        /// <param name="thLoadPt">the th load pt.</param>
        /// <param name="thLoadLn">the th load ln.</param>
        /// <param name="thLoadSf">the th load sf.</param>
        /// <param name="thFixPt">the th fix pt.</param>
        /// <param name="thFixLn">the th fix ln.</param>
        /// <param name="autoRebuild">the auto rebuild.</param>
        /// <param name="autoSmooth">the auto smooth.</param>
        /// <param name="autoCheck">the auto check.</param>
        public MeshPrepare(bool autoRegen = true, bool thPeak = true, bool thBeam = false, bool thColumn = true, bool thTruss = false, bool thFicBeam = false, bool thFreeEdge = false, bool thRegionBorder = false, 
            bool thSuppPt = true, bool thSuppLn = false, bool thSuppSf = false, bool thEdgeConn = false, bool thConnPt = false, bool thConnLn = false, bool thConnSf = false, bool thLoadPt = false, 
            bool thLoadLn = false, bool thLoadSf = false, bool thFixPt = false, bool thFixLn = false, bool autoRebuild = true, bool autoSmooth = true, bool autoCheck = false)
        {
            this.AutoRegen = autoRegen;
            this.ThPeak = thPeak;
            this.ThBeam = thBeam;
            this.ThColumn = thColumn;
            this.ThTruss = thTruss;
            this.ThFicBeam = thFicBeam;
            this.ThFreeEdge = thFreeEdge;
            this.ThRegionBordger = thRegionBorder;
            this.ThSupptPt = thSuppPt; 
            this.ThSuppLn = thSuppLn;
            this.ThSuppSf = thSuppSf;
            this.ThEdgeConn = thEdgeConn;
            this.ThConnPt = thConnPt;
            this.ThConnLn = thConnLn;
            this.ThConnSf = thConnSf;
            this.ThLoadPt = thLoadPt;
            this.ThLoadLn = thLoadLn;
            this.ThLoadSf = thLoadSf;
            this.ThFixPt = thFixPt;
            this.ThFixLn = thFixLn;
            this.AutoRebuild = autoRebuild;
            this.AutoSmooth = autoSmooth;
            this.AutoCheck = autoCheck;
        }

        /// <summary>
        /// Default.
        /// </summary>
        /// <returns>The result.</returns>
        public static MeshPrepare Default()
        {
            return new MeshPrepare(autoRegen : true);
        }
    }

    /// <summary>
    /// Represents a Peaksm Method.
    /// </summary>
    public partial class PeaksmMethod : GlobConfig
    {
        /// <summary>
        /// Gets or sets the peak form m.
        /// </summary>
        [XmlAttribute("sPeakFormFunc_M")]
        public int _peakFormM { get; set; }

        [XmlIgnore]
        public PeaksmMethodOptions PeakFormM 
        {
            get => (PeaksmMethodOptions)_peakFormM;
            set => _peakFormM = (int)value;
        }

        /// <summary>
        /// Gets or sets the peak form n.
        /// </summary>
        [XmlAttribute("sPeakFormFunc_N")]
        public int _peakFormN { get; set; }

        [XmlIgnore]
        public PeaksmMethodOptions PeakFormN
        {
            get => (PeaksmMethodOptions)_peakFormN;
            set => _peakFormN = (int)value;
        }

        /// <summary>
        /// Gets or sets the peak form v.
        /// </summary>
        [XmlAttribute("sPeakFormFunc_V")]
        public int _peakFormV { get; set; }

        [XmlIgnore]
        public PeaksmMethodOptions PeakFormV
        {
            get => (PeaksmMethodOptions)_peakFormV;
            set => _peakFormV = (int)value;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private PeaksmMethod()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeaksmMethod"/> class.
        /// </summary>
        /// <param name="peakFormM">the peak form m.</param>
        /// <param name="peakFormN">the peak form n.</param>
        /// <param name="peakFormV">the peak form v.</param>
        public PeaksmMethod(int peakFormM = 1, int peakFormN = 1, int peakFormV = 1)
        {
            this._peakFormM = peakFormM;
            this._peakFormN = peakFormN;
            this._peakFormV = peakFormV;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeaksmMethod"/> class.
        /// </summary>
        /// <param name="peakFormM">the peak form m.</param>
        /// <param name="peakFormN">the peak form n.</param>
        /// <param name="peakFormV">the peak form v.</param>
        public PeaksmMethod(PeaksmMethodOptions peakFormM = PeaksmMethodOptions.HigherOrderShapeFunc, PeaksmMethodOptions peakFormN = PeaksmMethodOptions.HigherOrderShapeFunc, PeaksmMethodOptions peakFormV = PeaksmMethodOptions.HigherOrderShapeFunc)
        {
            this.PeakFormM = peakFormM;
            this.PeakFormN = peakFormN;
            this.PeakFormV = peakFormV;
        }

        /// <summary>
        /// Default.
        /// </summary>
        /// <returns>The result.</returns>
        public static PeaksmMethod Default()
        {
            return new PeaksmMethod(peakFormM : 1);
        }

        /// <summary>
        /// Defines the Peaksm Method Options enumeration.
        /// </summary>
        public enum PeaksmMethodOptions
        {
            [XmlEnum("0")]
            [Parseable("DontSmooth", "dontSmooth", "dontsmooth", "Don't Smooth", "Don't smooth", "don't smooth", "No smooth", "Don't", "don't", "Dont", "dont")]
            DontSmooth = 0,

            [XmlEnum("1")]
            [Parseable("HigherOrderShapeFunc", "higherOrderShapeFunc", "HigherOrderShapeFunction", "HigherOrderShape", "higherOrderShape", "HigherOrder", "higherOrder", "Higher", "Use higher order shape function", "Use higher order shape functions", "Use higher order shape", "Higher order shape function", "higher order shape function", "higher order shape", "higher order", "Higher order shape", "Higher order", "higher")]
            HigherOrderShapeFunc = 1,

            [XmlEnum("2")]
            [Parseable("ConstShapeFunc", "ConstantShapeFunc", "constShapeFunc", "constantShapeFunc", "ConstShapeFunction", "ConstantShapeFunction", "constShapeFunction", "constantShapeFunction", "ConstShape", "constShape", "Const", "const", "Use constant shape function", "Use const shape function", "Use const shape", "Constant shape function", "constant shape function", "Use constant shape", "constant shape", "const shape", "Constant", "constant")]
            ConstShapeFunc = 2,

            [XmlEnum("3")]
            [Parseable("SetToZero", "Set To Zero", "Set to zero", "Zero", "zero")]
            SetToZero = 3
        }
    }
    

    /// <summary>
    /// Represents a Peaksm Auto.
    /// </summary>
    public partial class PeaksmAuto : GlobConfig
    {
        /// <summary>
        /// Gets or sets the peak beam.
        /// </summary>
        [XmlAttribute("fPeakBeam")]
        public int _peakBeam;

        [XmlIgnore]
        public bool PeakBeam
        {
            get => Convert.ToBoolean(_peakBeam);
            set => _peakBeam = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the peak column.
        /// </summary>
        [XmlAttribute("fPeakColumn")]
        public int _peakColumn;

        [XmlIgnore]
        public bool PeakColumn
        {
            get => Convert.ToBoolean(_peakColumn);
            set => _peakColumn = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the peak truss.
        /// </summary>
        [XmlAttribute("fPeakTruss")]
        public int _peakTruss;

        [XmlIgnore]
        public bool PeakTruss
        {
            get => Convert.ToBoolean(_peakTruss);
            set => _peakTruss = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the peak fic beam.
        /// </summary>
        [XmlAttribute("fPeakFicBeam")]
        public int _peakFicBeam;

        [XmlIgnore]
        public bool PeakFicBeam
        {
            get => Convert.ToBoolean(_peakFicBeam);
            set => _peakFicBeam = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the peak plate.
        /// </summary>
        [XmlAttribute("fPeakPlate")]
        public int _peakPlate;

        [XmlIgnore]
        public bool PeakPlate
        {
            get => Convert.ToBoolean(_peakPlate);
            set => _peakPlate = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the peak wall.
        /// </summary>
        [XmlAttribute("fPeakWall")]
        public int _peakWall;

        [XmlIgnore]
        public bool PeakWall
        {
            get => Convert.ToBoolean(_peakWall);
            set => _peakWall = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the peak fic shell.
        /// </summary>
        [XmlAttribute("fPeakFicShell")]
        public int _peakFicShell;

        [XmlIgnore]
        public bool PeakFicShell
        {
            get => Convert.ToBoolean(_peakFicShell);
            set => _peakFicShell = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the peak supp pt.
        /// </summary>
        [XmlAttribute("fPeakSuppPt")]
        public int _peakSuppPt;

        [XmlIgnore]
        public bool PeakSuppPt
        {
            get => Convert.ToBoolean(_peakSuppPt);
            set => _peakSuppPt = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the peak supp ln.
        /// </summary>
        [XmlAttribute("fPeakSuppLn")]
        public int _peakSuppLn;

        [XmlIgnore]
        public bool PeakSuppLn
        {
            get => Convert.ToBoolean(_peakSuppLn);
            set => _peakSuppLn = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the peak supp sf.
        /// </summary>
        [XmlAttribute("fPeakSuppSf")]
        public int _peakSuppSf;

        [XmlIgnore]
        public bool PeakSuppSf
        {
            get => Convert.ToBoolean(_peakSuppSf);
            set => _peakSuppSf = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the peak conn pt.
        /// </summary>
        [XmlAttribute("fPeakConnPt")]
        public int _peakConnPt;

        [XmlIgnore]
        public bool PeakConnPt
        {
            get => Convert.ToBoolean(_peakConnPt);
            set => _peakConnPt = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the peak conn ln.
        /// </summary>
        [XmlAttribute("fPeakConnLn")]
        public int _peakConnLn;

        [XmlIgnore]
        public bool PeakConnLn
        {
            get => Convert.ToBoolean(_peakConnLn);
            set => _peakConnLn = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the peak conn sf.
        /// </summary>
        [XmlAttribute("fPeakConnSf")]
        public int _peakConnSf;

        [XmlIgnore]
        public bool PeakConnSf
        {
            get => Convert.ToBoolean(_peakConnSf);
            set => _peakConnSf = Convert.ToInt32(value);
        }

        /// <summary>
        /// Gets or sets the peak factor.
        /// </summary>
        [XmlAttribute("rPeakFactor")]
        public double _peakFactor;

        [XmlIgnore]
        public double PeakFactor 
        {
            get => _peakFactor;
            set => _peakFactor = RestrictedDouble.NonNegMax_5(value);
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private PeaksmAuto()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PeaksmAuto"/> class.
        /// </summary>
        /// <param name="peakBeam">the peak beam.</param>
        /// <param name="peakColumn">the peak column.</param>
        /// <param name="peakTruss">the peak truss.</param>
        /// <param name="peakficBeam">the peakfic beam.</param>
        /// <param name="peakPlate">the peak plate.</param>
        /// <param name="peakWall">the peak wall.</param>
        /// <param name="peakFicShell">the peak fic shell.</param>
        /// <param name="peakSuppPt">the peak supp pt.</param>
        /// <param name="peakSuppLn">the peak supp ln.</param>
        /// <param name="peakSuppSf">the peak supp sf.</param>
        /// <param name="peakConnPt">the peak conn pt.</param>
        /// <param name="peakConnLn">the peak conn ln.</param>
        /// <param name="peakConnSf">the peak conn sf.</param>
        /// <param name="peakFactor">the peak factor.</param>
        public PeaksmAuto(bool peakBeam = false, bool peakColumn = true, bool peakTruss = false, bool peakficBeam = false, bool peakPlate = false, bool peakWall = false, bool peakFicShell = false, bool peakSuppPt = true, bool peakSuppLn = false, bool peakSuppSf = false, bool peakConnPt = false, bool peakConnLn = false, bool peakConnSf = false, double peakFactor = 0.5)
        {
            this.PeakBeam = peakBeam;
            this.PeakColumn = peakColumn;
            this.PeakTruss = peakTruss;
            this.PeakFicBeam = peakficBeam;
            this.PeakPlate = peakPlate;
            this.PeakWall = peakWall;
            this.PeakFicShell = peakFicShell;
            this.PeakSuppPt = peakSuppPt;
            this.PeakSuppLn = peakSuppLn;
            this.PeakSuppSf = peakSuppSf;
            this.PeakConnPt = peakConnPt;
            this.PeakConnLn = peakConnLn;
            this.PeakConnSf = peakConnSf;
            this.PeakFactor = peakFactor;
        }

        /// <summary>
        /// Default.
        /// </summary>
        /// <returns>The result.</returns>
        public static PeaksmAuto Default()
        {
            return new PeaksmAuto(peakBeam : false);
        }
    }

    /// <summary>
    /// Represents a Soil Calculation.
    /// </summary>
    public partial class SoilCalculation : GlobConfig
    {
        /// <summary>
        /// Gets or sets the soil as solid.
        /// </summary>
        [XmlAttribute("fSoilAsSolid")]
        public int _soilAsSolid;

        [XmlIgnore]
        public bool SoilAsSolid
        {
            get => Convert.ToBoolean(_soilAsSolid);
            set => _soilAsSolid = Convert.ToInt32(value);
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private SoilCalculation()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoilCalculation"/> class.
        /// </summary>
        /// <param name="soilAsSolid">the soil as solid.</param>
        public SoilCalculation(bool soilAsSolid = true)
        {
            this.SoilAsSolid = soilAsSolid;
        }

        /// <summary>
        /// Default.
        /// </summary>
        /// <returns>The result.</returns>
        public static SoilCalculation Default()
        {
            return new SoilCalculation(soilAsSolid : false);
        }
    }

    /// <summary>
    /// Base class for all GlobalConfigs that can be use for CmdGlobalCfg
    /// </summary>
    public abstract class GlobConfig
    {
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return Results.ResultsReader.ObjectRepresentation(this);
        }
    }
}
