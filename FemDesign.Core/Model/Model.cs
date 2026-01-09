// https://strusoft.com/
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using FemDesign.LibraryItems;
using FemDesign.Loads;
using FemDesign.Composites;
using Newtonsoft.Json.Linq;

namespace FemDesign
{
    /// <summary>
    /// Model. Represents a complete struxml model.
    /// </summary>
    [System.Serializable]
    [XmlRoot("database", Namespace = "urn:strusoft")]
    public partial class Model
    {
        /// <summary>
        /// The actual struXML version;  should be equal to the schema version the xml file is conformed to.
        /// </summary>
        [XmlAttribute("struxml_version")]
        public string StruxmlVersion { get; set; } // versiontype
        /// <summary>
        /// Name of the StruSoft or 3rd party product what generated this XML file.
        /// </summary>
        [XmlAttribute("source_software")]
        public string SourceSoftware { get; set; } // string
        /// <summary>
        /// The data is partial data, so the oldest entity latest modification date and time is the
        /// value in UTC. If the current XML contains the whole database, the start_time value is
        /// "1970-01-01T00:00:00Z". The date and time always in UTC!
        /// </summary>
        [XmlAttribute("start_time")]
        public string StartTime { get; set; } // dateTime
        /// <summary>
        /// The data is partial data, so the newest entity latest modification date and time is this
        /// value in UTC. This date and time always in UTC!
        /// </summary>
        [XmlAttribute("end_time")]
        public string EndTime { get; set; } // dateTime
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        [XmlAttribute("guid")]
        public System.Guid Guid { get; set; } // guidtype
        /// <summary>
        /// Gets or sets the convert id.
        /// </summary>
        [XmlAttribute("convertid")]
        public string ConvertId { get; set; } // guidtype

        // set the default value that does not have to be serialised
        /// <summary>
        /// Gets or sets the soil as solid.
        /// </summary>
        [XmlAttribute("soil_as_solid")]
        public bool SoilAsSolid { get; set; } = false;

        /// <summary>Calculation code</summary>
        [XmlAttribute("standard")]
        public string Standard { get; set; } // standardtype
        /// <summary>National annex of calculation code</summary>
        [XmlAttribute("country")]
        public Country Country { get; set; } // eurocodetype
        /// <summary>
        /// Gets or sets the xmlns.
        /// </summary>
        [XmlAttribute("xmlns")]
        public string Xmlns { get; set; }

        /// <summary>
        /// Gets or sets the construction stages.
        /// </summary>
        [XmlElement("construction_stages", Order = 1)]
        public ConstructionStages ConstructionStages { get; set; }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        [XmlElement("entities", Order = 2)]
        public Entities Entities { get; set; }
        /// <summary>
        /// Gets or sets the sections.
        /// </summary>
        [XmlElement("sections", Order = 3)]
        public Sections.ModelSections Sections { get; set; }
        /// <summary>
        /// Gets or sets the materials.
        /// </summary>
        [XmlElement("materials", Order = 4)]
        public Materials.Materials Materials { get; set; }
        /// <summary>
        /// Gets or sets the reinforcing materials.
        /// </summary>
        [XmlElement("reinforcing_materials", Order = 5)]
        public Materials.ReinforcingMaterials ReinforcingMaterials { get; set; }
        /// <summary>
        /// Gets or sets the composites.
        /// </summary>
        [XmlElement("composites", Order = 6)]
        public Composites.Composites Composites { get; set; }
        /// <summary>
        /// Gets or sets the point connection types.
        /// </summary>
        [XmlElement("point_connection_types", Order = 7)]
        public LibraryItems.PointConnectionTypes PointConnectionTypes { get; set; }
        /// <summary>
        /// Gets or sets the point support group types.
        /// </summary>
        [XmlElement("point_support_group_types", Order = 8)]
        public LibraryItems.PointSupportGroupTypes PointSupportGroupTypes { get; set; }
        /// <summary>
        /// Gets or sets the line connection types.
        /// </summary>
        [XmlElement("line_connection_types", Order = 9)]
        public LibraryItems.LineConnectionTypes LineConnectionTypes { get; set; }
        /// <summary>
        /// Gets or sets the line support group types.
        /// </summary>
        [XmlElement("line_support_group_types", Order = 10)]
        public LibraryItems.LineSupportGroupTypes LineSupportGroupTypes { get; set; }
        /// <summary>
        /// Gets or sets the surface connection types.
        /// </summary>
        [XmlElement("surface_connection_types", Order = 11)]
        public LibraryItems.SurfaceConnectionTypes SurfaceConnectionTypes { get; set; }
        /// <summary>
        /// Gets or sets the surface support types.
        /// </summary>
        [XmlElement("surface_support_types", Order = 12)]
        public LibraryItems.SurfaceSupportTypes SurfaceSupportTypes { get; set; }
        /// <summary>
        /// Gets or sets the orthotropic panel types.
        /// </summary>
        [XmlElement("timber_panel_types", Order = 13)]
        public Materials.OrthotropicPanelTypes OrthotropicPanelTypes { get; set; }
        /// <summary>
        /// Gets or sets the glc panel types.
        /// </summary>
        [XmlElement("glc_panel_types", Order = 14)]
        public Materials.GlcPanelTypes GlcPanelTypes { get; set; }

        /// <summary>
        /// Gets or sets the clt panel types.
        /// </summary>
        [XmlElement("clt_panel_types", Order = 15)]
        public Materials.CltPanelTypes CltPanelTypes { get; set; }

        /// <summary>
        /// Gets or sets the ptc strand types.
        /// </summary>
        [XmlElement("ptc_strand_types", Order = 16)]
        public Reinforcement.PtcStrandType PtcStrandTypes { get; set; }

        /// <summary>
        /// Gets or sets the vehicle types.
        /// </summary>
        [XmlElement("vehicle_types", Order = 17)]
        public LibraryItems.VehicleTypes VehicleTypes { get; set; }

        /// <summary>
        /// Gets or sets the bolt types.
        /// </summary>
        [XmlElement("bolt_types", Order = 18)]
        public List<StruSoft.Interop.StruXml.Data.Bolt_lib_type> BoltTypes { get; set; }

        /// <summary>
        /// Gets or sets the bar end release types.
        /// </summary>
        [XmlElement("bar_end_lib_type", Order = 19)]
        public List<StruSoft.Interop_24.Bar_end_lib_type> BarEndReleaseTypes { get; set; }

        /// <summary>
        /// Gets or sets the geometry.
        /// </summary>
        [XmlElement("geometry", Order = 20)]
        public StruSoft.Interop.StruXml.Data.DatabaseGeometry Geometry { get; set; }

        /// <summary>
        /// Gets or sets the user defined filters.
        /// </summary>
        [XmlElement("user_defined_filter", Order = 21)]
        public List<StruSoft.Interop.StruXml.Data.Userfilter_type> UserDefinedFilters { get; set; }

        /// <summary>
        /// Gets or sets the user defined views.
        /// </summary>
        [XmlElement("user_defined_views", Order = 22)]
        public StruSoft.Interop.StruXml.Data.DatabaseUser_defined_views UserDefinedViews { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        [XmlElement("end", Order = 23)]
        public string End { get; set; }

        /// <summary>
        /// Check whether a corresponding results file (<c>.strFEM</c>) exists for a given <c>.struxml</c> model.
        /// </summary>
        /// <param name="filePath">Path to the <c>.struxml</c> file.</param>
        /// <returns><c>true</c> if a <c>.strFEM</c> file exists next to <paramref name="filePath"/>; otherwise <c>false</c>.</returns>
        internal static bool HasResults(string filePath)
        {
            var directory = System.IO.Path.GetDirectoryName(filePath);
            var fileNames = Directory.GetFiles(directory);

            var strFEM = System.IO.Path.ChangeExtension(filePath, ".strFEM");

            foreach (var filename in fileNames)
            {
                if (filename == strFEM)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Model()
        {

        }

        /// <summary>
        /// Initialize a model with elements.
        /// </summary>
        /// <param name="country">Country/Annex of the FEM-Design model.</param>
        /// <param name="elements">Structural elements.</param>
        /// <param name="loads">Load elements.</param>
        /// <param name="loadCases">Load cases.</param>
        /// <param name="loadCombinations">Load combinations.</param>
        /// <param name="loadGroups">Load groups.</param>
        /// <param name="constructionStage">Construction stages object instance.</param>
        /// <param name="soil">Soil elements.</param>
        /// <param name="overwrite">If <c>true</c>, allows overwriting objects with matching GUID (where supported).</param>
        public Model(Country country, List<IStructureElement> elements = null, List<ILoadElement> loads = null, List<Loads.LoadCase> loadCases = null, List<Loads.LoadCombination> loadCombinations = null, List<Loads.ModelGeneralLoadGroup> loadGroups = null, ConstructionStages constructionStage = null, Soil.SoilElements soil = null, bool overwrite = false)
        {
            Initialize(country);
            
            if (elements != null)
                AddElements(elements, overwrite: false);
            if (loads != null)
                AddLoads(loads, overwrite: false);
            if (loadCases != null)
                AddLoadCases(loadCases, overwrite: false);
            if (loadCombinations != null)
                AddLoadCombinations(loadCombinations, overwrite: false);
            if (loadGroups != null)
                AddLoadGroupTable(loadGroups, overwrite: false);
            if (constructionStage != null)
                SetConstructionStages(constructionStage);
            if (soil != null)
                AddSoilElement(soil);
        }

        /// <summary>
        /// Initialize default header fields and ensure required containers are created.
        /// </summary>
        /// <param name="country">Country/annex to assign to the model.</param>
        private void Initialize(Country country)
        {
            this.StruxmlVersion = "01.00.000";
            this.SourceSoftware = $"FEM-Design API SDK {Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
            this.StartTime = "1970-01-01T00:00:00.000";
            this.EndTime = System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
            this.Guid = System.Guid.NewGuid();
            this.ConvertId = "00000000-0000-0000-0000-000000000000";
            this.Standard = "EC";
            this.Country = country;
            this.End = "";

            // Check if model contains entities, sections and materials, else these needs to be initialized.
            if (this.Entities == null)
            {
                this.Entities = new Entities();
            }
            if (this.Sections == null)
            {
                this.Sections = new Sections.ModelSections();
            }
            if (this.Materials == null)
            {
                this.Materials = new Materials.Materials();
            }
            if (this.Composites == null)
            {
                this.Composites = new Composites.Composites();
            }
            if (this.ReinforcingMaterials == null)
            {
                this.ReinforcingMaterials = new Materials.ReinforcingMaterials();
            }
            if (this.LineConnectionTypes == null)
            {
                this.LineConnectionTypes = new LibraryItems.LineConnectionTypes();
                this.LineConnectionTypes.PredefinedTypes = new List<Releases.RigidityDataLibType3>();
            }
            if (this.PtcStrandTypes == null)
            {
                this.PtcStrandTypes = new Reinforcement.PtcStrandType();
            }
        }

        #region serialization
        /// <summary>
        /// Deserialize model from file (.struxml).
        /// </summary>
        /// <param name="filePath">Path to a <c>.struxml</c> file.</param>
        /// <returns>The deserialized <see cref="Model"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> does not have the <c>.struxml</c> extension.</exception>
        public static Model DeserializeFromFilePath(string filePath)
        {
            // check file extension
            if (Path.GetExtension(filePath) != ".struxml")
            {
                throw new System.ArgumentException("File extension must be .struxml! Model.DeserializeModel failed.");
            }

            //
            XmlSerializer deserializer = new XmlSerializer(typeof(Model));
            TextReader reader = new StreamReader(filePath);

            object obj;
            try
            {
                obj = deserializer.Deserialize(reader);
            }
            catch (System.InvalidOperationException ex)
            {
                throw ex; // There is an error in XML document (3, 2).
            }
            finally
            {
                // close reader
                reader.Close();
            }

            // cast type
            Model model = (Model)obj;

            if (model.Entities == null) model.Entities = new Entities();

            // prepare elements with library reference
            // Check if there are any elements of type to avoid null checks on each library type (sections, materials etc.) in each method below
            if (model.Entities.Bars.Any())
                model.GetBars();
            if (model.Entities.AdvancedFem.FictitiousShells.Any())
                model.GetFictitiousShells();
            if (model.Entities.Supports.LineSupport.Any())
                model.GetLineSupports();
            if (model.Entities.Panels.Any())
                model.GetPanels();
            if (model.Entities.Supports.PointSupport.Any())
                model.GetPointSupports();
            if (model.Entities.Slabs.Any())
                model.GetSlabs();
            if (model.Entities.Supports.SurfaceSupport.Any())
                model.GetSurfaceSupports();
            if (model.Entities.AdvancedFem.ConnectedPoints.Any())
                model.GetPointConnections();
            if (model.Entities.AdvancedFem.ConnectedLines.Any())
                model.GetLineConnections();
            if (model.Entities.AdvancedFem.SurfaceConnections.Any())
                model.GetSurfaceConnections();
            if (model.ConstructionStages != null && model.ConstructionStages.Stages.Any())
                model.GetConstructionStages();
            if (model.Entities?.Loads?.LoadCombinations != null && model.Entities.Loads.LoadCombinations.Any())
                model.GetLoadCombinations();
            if (model.Entities?.Loads?.LoadCases != null && model.Entities.Loads.LoadCases.Any())
                model.GetLoads();
            if (model.Entities?.Loads?.LoadGroupTable != null)
                model.GetLoadGroups();
            if (model.Geometry != null)
                model.GetGeometries();
            return model;
        }

        /// <summary>
        /// Serialize Model to file (.struxml).
        /// </summary>
        /// <param name="filePath">
        /// Output path. If <c>null</c>, the file will be written as <c>myModel.struxml</c> in the current working directory.
        /// If no extension is provided, <c>.struxml</c> will be used.
        /// </param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> has an extension other than <c>.struxml</c>.</exception>
        public void SerializeModel(string filePath)
        {
            if (filePath == null)
            {
                var currentDirectory = System.IO.Directory.GetCurrentDirectory();
                filePath = System.IO.Path.Combine(currentDirectory, "myModel.struxml");
            }

            // Relavive paths will be converted to full paths
            filePath = System.IO.Path.GetFullPath(filePath);

            // If path has no file extension "struxml" will be used
            if (Path.GetExtension(filePath) == "")
                filePath = Path.ChangeExtension(filePath, "struxml");

            // Check file extension
            if (Path.GetExtension(filePath) != ".struxml")
            {
                throw new System.ArgumentException("File extension must be .struxml! Model.SerializeModel failed.");
            }

            // Serialize
            XmlSerializer serializer = new XmlSerializer(typeof(Model));
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, this);
            }
        }

        /// <summary>
        /// Serialize Model to string.
        /// </summary>
        /// <returns>The serialized model as an XML string.</returns>
        public string SerializeToString()
        {
            // serialize
            XmlSerializer serializer = new XmlSerializer(typeof(Model));
            using (TextWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                return writer.ToString();
            }
        }
        #endregion

        #region addEntities

        /// <summary>
        /// Add entities to Model.
        /// </summary>
        /// <param name="bars">Bars to add.</param>
        /// <param name="fictitiousBars">Fictitious bars to add.</param>
        /// <param name="shells">Slabs to add.</param>
        /// <param name="fictitiousShells">Fictitious shells to add.</param>
        /// <param name="panels">Panels to add.</param>
        /// <param name="covers">Covers to add.</param>
        /// <param name="loads">Loads to add (e.g. point/line/surface/pressure loads).</param>
        /// <param name="loadCases">Load cases to add.</param>
        /// <param name="loadCombinations">Load combinations to add.</param>
        /// <param name="supports">Supports to add.</param>
        /// <param name="storeys">Storeys to add.</param>
        /// <param name="axes">Axes to add.</param>
        /// <param name="loadGroups">Load groups to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing objects with matching GUID (where supported).</param>
        /// <returns>The current model instance.</returns>
        public Model AddEntities(List<Bars.Bar> bars, List<ModellingTools.FictitiousBar> fictitiousBars, List<Shells.Slab> shells, List<ModellingTools.FictitiousShell> fictitiousShells, List<Shells.Panel> panels, List<ModellingTools.Cover> covers, List<object> loads, List<Loads.LoadCase> loadCases, List<Loads.LoadCombination> loadCombinations, List<ISupportElement> supports, List<StructureGrid.Storey> storeys, List<StructureGrid.Axis> axes, List<Loads.ModelGeneralLoadGroup> loadGroups, bool overwrite)
        {
            // check if model contains entities, sections and materials
            if (this.Entities == null)
            {
                this.Entities = new Entities();
            }
            if (this.Sections == null)
            {
                this.Sections = new Sections.ModelSections();
            }
            if (this.Materials == null)
            {
                this.Materials = new Materials.Materials();
            }
            if (this.ReinforcingMaterials == null)
            {
                this.ReinforcingMaterials = new Materials.ReinforcingMaterials();
            }
            if (this.LineConnectionTypes == null)
            {
                this.LineConnectionTypes = new LibraryItems.LineConnectionTypes();
                this.LineConnectionTypes.PredefinedTypes = new List<Releases.RigidityDataLibType3>();
            }

            if (bars != null)
            {
                foreach (Bars.Bar bar in bars)
                {
                    this.AddBar(bar, overwrite);
                }
            }

            if (fictitiousBars != null)
            {
                foreach (ModellingTools.FictitiousBar fictBar in fictitiousBars)
                {
                    this.AddFictBar(fictBar, overwrite);
                }
            }

            if (shells != null)
            {
                foreach (Shells.Slab shell in shells)
                {
                    this.AddSlab(shell, overwrite);
                }
            }

            if (fictitiousShells != null)
            {
                foreach (ModellingTools.FictitiousShell fictShell in fictitiousShells)
                {
                    this.AddFictShell(fictShell, overwrite);
                }
            }

            if (panels != null)
            {
                foreach (Shells.Panel panel in panels)
                {
                    this.AddPanel(panel, overwrite);
                }
            }

            if (covers != null)
            {
                foreach (ModellingTools.Cover cover in covers)
                {
                    this.AddCover(cover, overwrite);
                }
            }

            if (loads != null)
            {
                foreach (object load in loads)
                {
                    this.AddLoad(load, overwrite);
                }
            }

            if (loadCases != null)
            {
                foreach (Loads.LoadCase loadCase in loadCases)
                {
                    this.AddLoadCase(loadCase, overwrite);
                }
            }

            this.AddLoadCombinations(loadCombinations, overwrite);

            if (loadGroups != null)
            {
                this.AddLoadGroupTable(loadGroups, overwrite);
            }

            if (supports != null)
            {
                foreach (ISupportElement support in supports)
                {
                    this.AddSupport(support, overwrite);
                }
            }

            if (storeys != null)
            {
                foreach (StructureGrid.Storey storey in storeys)
                {
                    this.AddStorey(storey, overwrite);
                }
            }

            if (axes != null)
            {
                foreach (StructureGrid.Axis axis in axes)
                {
                    this.AddAxis(axis, overwrite);
                }
            }

            return this;
        }

        /// <summary>
        /// Add Bar to Model.
        /// </summary>
        /// <param name="obj">Bar to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing bar with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a bar with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddBar(Bars.Bar obj, bool overwrite)
        {
            // in model?
            bool inModel = this.BarInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.Bars.RemoveAll(x => x.Guid == obj.Guid);
            }

            // if truss
            if (obj.BarPart.SectionType == Bars.SectionType.Truss)
            {
                this.AddSection(obj.BarPart.TrussUniformSectionObj, overwrite);
                this.AddMaterial(obj.BarPart.ComplexMaterialObj, overwrite);
            }
            // if complex composite and not delta beam type
            else if (obj.BarPart.SectionType == Bars.SectionType.CompositeBeamColumn)
            {
                this.AddComplexComposite(obj.BarPart.ComplexCompositeObj, overwrite);
            }
            // if complex section but delta beam type
            else if (obj.BarPart.SectionType == Bars.SectionType.DeltaBeamColumn)
            {
                // do nothing
            }
            // if complex section
            else if (obj.BarPart.SectionType == Bars.SectionType.RegularBeamColumn)
            {
                this.AddComplexSection(obj.BarPart.ComplexSectionObj, overwrite);
                this.AddMaterial(obj.BarPart.ComplexMaterialObj, overwrite);
            }
            else
            {
                throw new System.ArgumentException("Type of bar is not supported.");
            }

            // add reinforcement
            this.AddBarReinforcements(obj, overwrite);

            // add ptc
            this.AddBarPtcs(obj, overwrite);

            // add bar
            this.Entities.Bars.Add(obj);
        }

        /// <summary>
        /// Check if Bar in Model.
        /// </summary>
        /// <param name="obj">Bar to check.</param>
        /// <returns><c>true</c> if a bar with the same GUID already exists in the model; otherwise <c>false</c>.</returns>
        private bool BarInModel(Bars.Bar obj)
        {
            foreach (Bars.Bar elem in this.Entities.Bars)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add post-tensioned cables from a bar to the model.
        /// </summary>
        /// <param name="obj">Bar containing post-tensioned cables to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing objects with matching GUID (where supported).</param>
        private void AddBarPtcs(Bars.Bar obj, bool overwrite)
        {
            foreach (Reinforcement.Ptc ptc in obj.Ptc)
            {
                this.AddPtc(ptc, overwrite);
            }
        }

        /// <summary>
        /// Add Post-tensioned cable to Model.
        /// </summary>
        /// <param name="obj">Post-tensioned cable to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing cable with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a cable with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddPtc(Reinforcement.Ptc obj, bool overwrite)
        {
            // in model?
            bool inModel = this.PtcInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.PostTensionedCables.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add material
            this.AddPtcStrandType(obj.StrandType, overwrite);

            // add ptc
            this.Entities.PostTensionedCables.Add(obj);
        }

        /// <summary>
        /// Check if a post-tensioned cable exists in the model.
        /// </summary>
        /// <param name="obj">Post-tensioned cable to check.</param>
        /// <returns><c>true</c> if a cable with the same GUID already exists in the model; otherwise <c>false</c>.</returns>
        private bool PtcInModel(Reinforcement.Ptc obj)
        {
            foreach (Reinforcement.Ptc elem in this.Entities.PostTensionedCables)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Add Post-tensioned cable to Model.
        /// </summary>
        /// <param name="obj">Concealed bar to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing concealed bar with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a concealed bar with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddConcealedBar(Reinforcement.ConcealedBar obj, bool overwrite)
        {
            // in model?
            bool inModel = this.ConcealedBarInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.HiddenBars.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add concealed bar
            this.Entities.HiddenBars.Add(obj);
        }

        /// <summary>
        /// Check if a concealed bar exists in the model.
        /// </summary>
        /// <param name="obj">Concealed bar to check.</param>
        /// <returns><c>true</c> if a concealed bar with the same GUID already exists in the model; otherwise <c>false</c>.</returns>
        private bool ConcealedBarInModel(Reinforcement.ConcealedBar obj)
        {
            foreach (Reinforcement.ConcealedBar elem in this.Entities.HiddenBars)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Add Fictitious Bar to Model.
        /// </summary>
        /// <param name="obj">Fictitious bar to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing fictitious bar with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a fictitious bar with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddFictBar(ModellingTools.FictitiousBar obj, bool overwrite)
        {
            // in model?
            bool inModel = this.FictBarInModel(obj);

            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            else if (inModel && overwrite == true)
            {
                this.Entities.AdvancedFem.FictitiousBars.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add fictitious bar
            this.Entities.AdvancedFem.FictitiousBars.Add(obj);
        }

        /// <summary>
        /// Check if Fictitious Bar in Model.
        /// </summary>
        /// <param name="obj">Fictitious bar to check.</param>
        /// <returns><c>true</c> if a fictitious bar with the same GUID already exists in the model; otherwise <c>false</c>.</returns>
        private bool FictBarInModel(ModellingTools.FictitiousBar obj)
        {
            foreach (ModellingTools.FictitiousBar elem in this.Entities.AdvancedFem.FictitiousBars)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Fictitious Shell to Model.
        /// </summary>
        /// <param name="obj">Fictitious shell to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing fictitious shell with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a fictitious shell with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddFictShell(ModellingTools.FictitiousShell obj, bool overwrite)
        {
            // in model?
            bool inModel = this.FictShellInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.AdvancedFem.FictitiousShells.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add line connection types (predefined rigidity)
            foreach (Releases.RigidityDataLibType3 predef in obj.Region.GetPredefinedRigidities())
            {
                this.AddPredefinedRigidity(predef, overwrite);
            }

            // add shell
            this.Entities.AdvancedFem.FictitiousShells.Add(obj);
        }

        /// <summary>
        /// Check if Fictitious Bar in Model.
        /// </summary>
        /// <param name="obj">Fictitious shell to check.</param>
        /// <returns><c>true</c> if a fictitious shell with the same GUID already exists in the model; otherwise <c>false</c>.</returns>
        private bool FictShellInModel(ModellingTools.FictitiousShell obj)
        {
            foreach (ModellingTools.FictitiousShell elem in this.Entities.AdvancedFem.FictitiousShells)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add ComplexSection (from Bar) to Model.
        /// </summary>
        /// <param name="complexSection">Complex section to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing complex section with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a complex section with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddComplexSection(Sections.ComplexSection complexSection, bool overwrite)
        {
            if (this.Sections == null)
            {
                this.Sections = new Sections.ModelSections();
            }
            // in model?
            bool inModel = this.ComplexSectionInModel(complexSection);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{complexSection.GetType().FullName} with guid: {complexSection.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Sections.ComplexSection.RemoveAll(x => x.Guid == complexSection.Guid);
            }

            // add complex section
            this.Sections.ComplexSection.Add(complexSection);

            // add sections in complex section
            foreach (Sections.Section section in complexSection.Sections)
            {
                this.AddSection(section, overwrite);
            }
        }

        /// <summary>
        /// Check if ComplexSection in Model.
        /// </summary>
        /// <param name="obj">Complex section to check.</param>
        /// <returns><c>true</c> if a complex section with the same GUID already exists in the model; otherwise <c>false</c>.</returns>
        private bool ComplexSectionInModel(FemDesign.Sections.ComplexSection obj)
        {
            foreach (Sections.ComplexSection elem in this.Sections.ComplexSection)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Add composite sections referenced by a complex composite to the model.
        /// </summary>
        /// <param name="obj">Complex composite containing composite section references.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing composite sections with matching GUID.</param>
        private void AddCompositeSection(Composites.ComplexComposite obj, bool overwrite)
        {
            // in model?
            // obj.Composite_section.Unique(x => x.Guid);
            var uniqueComplexCompositePart = obj.Parts.Where(x => x.CompositeSectionRef != null).GroupBy(x => x.CompositeSectionRef).Select(grp => grp.FirstOrDefault());


            foreach (var complexCompositePart in uniqueComplexCompositePart)
            {
                // initialise variable as false
                bool inModel = false;

                if (this.Composites.CompositeSection != null)
                {
                    inModel = this.Composites.CompositeSection.Any(x => x.Guid == complexCompositePart.CompositeSectionRef);

                    // check section names
                    Dictionary<string, Guid> guidDir = this.Composites.CompositeSection.ToDictionary(cs => cs.ParameterDictionary[CompositeSectionParameterType.Name], cs => cs.Guid);
                    var objName = complexCompositePart.CompositeSectionObj.ParameterDictionary[FemDesign.Composites.CompositeSectionParameterType.Name];
                    var objGuid = complexCompositePart.CompositeSectionObj.Guid;

                    // Check if a section with the same name exists but with a different GUID
                    if (guidDir.TryGetValue(objName, out var existingGuid) && objGuid != existingGuid)
                        throw new Exception("One or more composite sections have the same name. Different composite sections must have different names!");
                }
                else
                {
                    this.Composites.CompositeSection = new List<Composites.CompositeSection>();
                }


                // in model, don't overwrite
                if (inModel && overwrite == false)
                {
                    //throw new System.ArgumentException($"{complexCompositePart.GetType().FullName} with guid: {complexCompositePart.CompositeSectionRef} has already been added to model. Are you adding the same element twice?");
                }

                // in model, overwrite
                else if (inModel && overwrite == true)
                {
                    this.Composites.CompositeSection.RemoveAll(x => x.Guid == complexCompositePart.CompositeSectionRef);
                    this.Composites.CompositeSection.Add(complexCompositePart.CompositeSectionObj);
                    foreach (var part in complexCompositePart.CompositeSectionObj.Parts)
                    {
                        this.AddMaterial(part.Material, overwrite);
                        this.AddSection(part.Section, overwrite);
                    }
                }

                // not in model
                if (!inModel)

                {
                    this.Composites.CompositeSection.Add(complexCompositePart.CompositeSectionObj);
                    foreach (var part in complexCompositePart.CompositeSectionObj.Parts)
                    {
                        this.AddMaterial(part.Material, overwrite);
                        this.AddSection(part.Section, overwrite);
                    }
                }
            }
        }

        /// <summary>
        /// Add ComplexComposite to Model.
        /// if ComplexComposite is present, also CompositeSection will be created 
        /// </summary>
        /// <param name="obj">Complex composite to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing complex composite with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a complex composite with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddComplexComposite(Composites.ComplexComposite obj, bool overwrite)
        {
            // in model?
            bool inModel = false;
            // if composites present
            if (this.Composites != null)
            {
                inModel = this.Composites.ComplexComposite.Any(x => x.Guid == obj.Guid);
            }
            // if composites not present
            else
            {
                this.Composites = new Composites.Composites();
                this.Composites.ComplexComposite = new List<Composites.ComplexComposite>();
            }

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                // remove objects with the same GUID
                this.Composites.ComplexComposite.RemoveAll(x => x.Guid == obj.Guid);
                // add complex composite
                this.Composites.ComplexComposite.Add(obj);
                // add composite section
                this.AddCompositeSection(obj, overwrite);
            }

            // not in model
            if (!inModel)
            {
                // add complex composite
                this.Composites.ComplexComposite.Add(obj);

                // add composite section
                this.AddCompositeSection(obj, overwrite);
            }
        }

        /// <summary>
        /// Add Cover to Model.
        /// </summary>
        /// <param name="obj">Cover to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing cover with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a cover with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddCover(ModellingTools.Cover obj, bool overwrite)
        {
            // in model?
            bool inModel = this.CoverInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.AdvancedFem.Covers.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add cover
            this.Entities.AdvancedFem.Covers.Add(obj);
        }

        /// <summary>
        /// Check if Cover in Model.
        /// </summary>
        /// <param name="obj">Cover to check.</param>
        /// <returns><c>true</c> if a cover with the same GUID already exists in the model; otherwise <c>false</c>.</returns>
        private bool CoverInModel(ModellingTools.Cover obj)
        {
            foreach (ModellingTools.Cover elem in this.Entities.AdvancedFem.Covers)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add a surface connection to the model (advanced FEM).
        /// </summary>
        /// <param name="obj">Surface connection to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing surface connection with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a surface connection with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddSurfaceConnection(ModellingTools.SurfaceConnection obj, bool overwrite)
        {
            // advanced fem null?
            if (this.Entities.AdvancedFem == null)
            {
                this.Entities.AdvancedFem = new ModellingTools.AdvancedFem();
            }

            // surface connection null?
            if (this.Entities.AdvancedFem.SurfaceConnections == null)
            {
                this.Entities.AdvancedFem.SurfaceConnections = new List<ModellingTools.SurfaceConnection>();
            }

            // in model?
            bool inModel = this.Entities.AdvancedFem.SurfaceConnections.Any(x => x.Guid == obj.Guid);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.AdvancedFem.SurfaceConnections.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add surface connection
            this.Entities.AdvancedFem.SurfaceConnections.Add(obj);

            // add predefined rigidity
            if (obj.PredefRigidity != null)
            {
                this.AddSurfaceConnectionLibItem(obj.PredefRigidity, overwrite);
            }
        }

        /// <summary>
        /// Add a predefined surface connection rigidity library item to the model.
        /// </summary>
        /// <param name="obj">Predefined rigidity to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing item with matching GUID.</param>
        private void AddSurfaceConnectionLibItem(Releases.RigidityDataLibType1 obj, bool overwrite)
        {
            // if null create new element
            if (this.SurfaceConnectionTypes == null)
            {
                this.SurfaceConnectionTypes = new LibraryItems.SurfaceConnectionTypes();
                this.SurfaceConnectionTypes.PredefinedTypes = new List<Releases.RigidityDataLibType1>();
            }

            // in model?
            bool inModel = this.SurfaceConnectionTypes.PredefinedTypes.Any(x => x.Guid == obj.Guid);


            // in model, overwrite
            if (inModel && overwrite)
            {
                this.SurfaceConnectionTypes.PredefinedTypes.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add lib item
            this.SurfaceConnectionTypes.PredefinedTypes.Add(obj);
        }

        /// <summary>
        /// Add a connected line (line connection) to the model (advanced FEM).
        /// </summary>
        /// <param name="obj">Connected line to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing connected line with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a connected line with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddConnectedLine(ModellingTools.ConnectedLines obj, bool overwrite)
        {
            // advanced fem null?
            if (this.Entities.AdvancedFem == null)
            {
                this.Entities.AdvancedFem = new ModellingTools.AdvancedFem();
            }

            // connected lines null?
            if (this.Entities.AdvancedFem.ConnectedLines == null)
            {
                this.Entities.AdvancedFem.ConnectedLines = new List<ModellingTools.ConnectedLines>();
            }

            // in model?
            bool inModel = this.Entities.AdvancedFem.ConnectedLines.Any(x => x.Guid == obj.Guid);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.AdvancedFem.ConnectedLines.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add connected line
            this.Entities.AdvancedFem.ConnectedLines.Add(obj);

            // add predefined rigidity
            if (obj.PredefRigidity != null)
            {
                this.AddConnectedLinesLibItem(obj.PredefRigidity, overwrite);
            }
        }

        /// <summary>
        /// Add a predefined connected line rigidity library item to the model.
        /// </summary>
        /// <param name="obj">Predefined rigidity to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing item with matching GUID.</param>
        private void AddConnectedLinesLibItem(Releases.RigidityDataLibType3 obj, bool overwrite)
        {
            // if null create new element
            if (this.LineConnectionTypes == null)
            {
                this.LineConnectionTypes = new LibraryItems.LineConnectionTypes();
                this.LineConnectionTypes.PredefinedTypes = new List<Releases.RigidityDataLibType3>();
            }

            // in model?
            bool inModel = this.LineConnectionTypes.PredefinedTypes.Any(x => x.Guid == obj.Guid);


            //// in model, don't overwrite
            //if (inModel && !overwrite)
            //{
            // predefinedTypes can be duplicate
            //    throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            //}

            // in model, overwrite
            if (inModel && overwrite)
            {
                this.LineConnectionTypes.PredefinedTypes.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add lib item
            this.LineConnectionTypes.PredefinedTypes.Add(obj);
        }

        /// <summary>
        /// Add connected points (point connections) to the model (advanced FEM).
        /// </summary>
        /// <param name="obj">Connected points to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing connected points with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a connected points object with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddConnectedPoints(ModellingTools.ConnectedPoints obj, bool overwrite)
        {
            // advanced fem null?
            if (this.Entities.AdvancedFem == null)
            {
                this.Entities.AdvancedFem = new ModellingTools.AdvancedFem();
            }

            // connected points null?
            if (this.Entities.AdvancedFem.ConnectedPoints == null)
            {
                this.Entities.AdvancedFem.ConnectedPoints = new List<ModellingTools.ConnectedPoints>();
            }

            // in model?
            bool inModel = this.Entities.AdvancedFem.ConnectedPoints.Any(x => x.Guid == obj.Guid);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.AdvancedFem.ConnectedPoints.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add connected point
            this.Entities.AdvancedFem.ConnectedPoints.Add(obj);

            // add predefined rigidity
            if (obj.PredefRigidity != null)
            {
                this.AddConnectedPointsLibItem(obj.PredefRigidity, overwrite);
            }
        }

        /// <summary>
        /// Add a predefined connected points rigidity library item to the model.
        /// </summary>
        /// <param name="obj">Predefined rigidity to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing item with matching GUID.</param>
        private void AddConnectedPointsLibItem(Releases.RigidityDataLibType2 obj, bool overwrite)
        {
            // if null create new element
            if (this.PointConnectionTypes == null)
            {
                this.PointConnectionTypes = new LibraryItems.PointConnectionTypes();
                this.PointConnectionTypes.PredefinedTypes = new List<Releases.RigidityDataLibType2>();
            }

            // in model?
            bool inModel = this.PointConnectionTypes.PredefinedTypes.Any(x => x.Guid == obj.Guid);

            // in model, don't overwrite
            //if (inModel && !overwrite)
            //{
            //    throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            //}

            // in model, overwrite
            if (inModel && overwrite)
            {
                this.PointConnectionTypes.PredefinedTypes.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add lib item
            this.PointConnectionTypes.PredefinedTypes.Add(obj);
        }

        /// <summary>
        /// Add Fictitious Shell to Model.
        /// </summary>
        /// <param name="obj">Diaphragm to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing diaphragm with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a diaphragm with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddDiaphragm(ModellingTools.Diaphragm obj, bool overwrite)
        {
            // in model?
            bool inModel = this.DiaphragmInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.AdvancedFem.Diaphragms.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add line connection types (predefined rigidity)
            foreach (Releases.RigidityDataLibType3 predef in obj.Region.GetPredefinedRigidities())
            {
                this.AddPredefinedRigidity(predef, overwrite);
            }

            this.Entities.AdvancedFem.Diaphragms.Add(obj);
        }

        /// <summary>
        /// Check if Fictitious Bar in Model.
        /// </summary>
        /// <param name="obj">Diaphragm to check.</param>
        /// <returns><c>true</c> if a diaphragm with the same GUID already exists in the model; otherwise <c>false</c>.</returns>
        private bool DiaphragmInModel(ModellingTools.Diaphragm obj)
        {
            foreach (ModellingTools.Diaphragm elem in this.Entities.AdvancedFem.Diaphragms)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Add Load to Model.
        /// </summary>
        /// <param name="obj">PointLoad, LineLoad, PressureLoad, SurfaceLoad</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing load objects with matching GUID (where supported).</param>
        private void AddLoad(object obj, bool overwrite)
        {
            if (obj == null)
            {
                throw new System.ArgumentException("Passed object is null");
            }
            else if (obj.GetType() == typeof(Loads.PointLoad))
            {
                this.AddPointLoad((Loads.PointLoad)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Loads.PointSupportMotion))
            {
                this.AddPointSupportMotion((Loads.PointSupportMotion)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Loads.LineLoad))
            {
                this.AddLineLoad((Loads.LineLoad)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Loads.LineSupportMotion))
            {
                this.AddLineSupportMotionLoad((Loads.LineSupportMotion)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Loads.LineStressLoad))
            {
                this.AddLineStressLoad((Loads.LineStressLoad)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Loads.LineTemperatureLoad))
            {
                this.AddLineTemperatureLoad((Loads.LineTemperatureLoad)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Loads.PressureLoad))
            {
                this.AddPressureLoad((Loads.PressureLoad)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Loads.SurfaceLoad))
            {
                this.AddSurfaceLoad((Loads.SurfaceLoad)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Loads.SurfaceTemperatureLoad))
            {
                this.AddSurfaceTemperatureLoad((Loads.SurfaceTemperatureLoad)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Loads.MassConversionTable))
            {
                this.AddMassConversionTable((Loads.MassConversionTable)obj);
            }
            else if (obj.GetType() == typeof(Loads.Footfall))
            {
                this.AddFootfall((Loads.Footfall)obj, overwrite);
            }
            else
            {
                throw new System.ArgumentException("Passed object must be PointLoad, LineLoad, SurfaceLoad or PressureLoad");
            }
        }

        /// <summary>
        /// Add Panel to Model.
        /// </summary>
        /// <param name="obj">Panel to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing panel with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a panel with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddPanel(Shells.Panel obj, bool overwrite)
        {
            // in model?
            bool inModel = this.PanelInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.Panels.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add panel properties
            if (obj.Material != null)
            {
                this.AddMaterial(obj.Material, overwrite);
            }

            if (obj.Section != null)
            {
                this.AddSection(obj.Section, overwrite);
            }

            // Add timber application data
            if (obj.TimberPanelData != null)
            {
                // Add library types
                if (obj.TimberPanelData.PanelType != null)
                {
                    var panelType = obj.TimberPanelData.PanelType;
                    if (panelType.GetType() == typeof(FemDesign.Materials.CltPanelLibraryType))
                    {
                        this.AddCltPanelLibraryType((FemDesign.Materials.CltPanelLibraryType)panelType, overwrite);
                    }
                    else if (panelType.GetType() == typeof(FemDesign.Materials.OrthotropicPanelLibraryType))
                    {
                        this.AddTimberPanelLibraryType((FemDesign.Materials.OrthotropicPanelLibraryType)panelType, overwrite);
                    }
                    else if (panelType.GetType() == typeof(FemDesign.Materials.GlcPanelLibraryType))
                    {
                        this.AddGlcPanelLibraryType((FemDesign.Materials.GlcPanelLibraryType)panelType, overwrite);
                    }
                    else
                    {
                        throw new System.ArgumentException($"The type {panelType.GetType()} is a member of {typeof(Materials.IPanelLibraryType)} but don't have a method for adding library data to the model.");
                    }
                }
                else
                {
                    throw new System.ArgumentException($"Could not find the related library data with guid: {obj.TimberPanelData._panelTypeReference}. Failed to add panel library data.");
                }
            }
            // add line connection types from border
            foreach (Releases.RigidityDataLibType3 predef in obj.Region.GetPredefinedRigidities())
            {
                this.AddPredefinedRigidity(predef, overwrite);
            }

            // add line connection types of internal panels
            if (obj.InternalPanels != null)
            {
                foreach (InternalPanel intPanel in obj.InternalPanels.IntPanels)
                {
                    foreach (Releases.RigidityDataLibType3 predef in intPanel.Region.GetPredefinedRigidities())
                    {
                        this.AddPredefinedRigidity(predef, overwrite);
                    }
                }
            }

            // add panel
            this.Entities.Panels.Add(obj);
        }

        /// <summary>
        /// Check if Panel in Model.
        /// </summary>
        /// <param name="obj">Panel to check.</param>
        /// <returns><c>true</c> if a panel with the same GUID already exists in the model; otherwise <c>false</c>.</returns>
        private bool PanelInModel(Shells.Panel obj)
        {
            foreach (Shells.Panel elem in this.Entities.Panels)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add PointLoad to Model.
        /// </summary>
        /// <param name="obj">Point load to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing point load with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a point load with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddPointLoad(Loads.PointLoad obj, bool overwrite)
        {
            // in model?
            bool inModel = this.PointLoadInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.Loads.PointLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add point load
            this.Entities.Loads.PointLoads.Add(obj);
        }

        /// <summary>
        /// Check if PointLoad in Model.
        /// </summary>
        /// <param name="obj">Point load to check.</param>
        /// <returns><c>true</c> if a point load with the same GUID already exists in the model; otherwise <c>false</c>.</returns>
        private bool PointLoadInModel(Loads.PointLoad obj)
        {
            foreach (Loads.PointLoad elem in this.Entities.Loads.PointLoads)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Add Mass to Model.
        /// </summary>
        private void AddMass(Loads.Mass obj, bool overwrite)
        {
            // in model?
            bool inModel = this.MassInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.Loads.Masses.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add mass load
            this.Entities.Loads.Masses.Add(obj);
        }

        /// <summary>
        /// Check if Mass is in Model.
        /// </summary>
        private bool MassInModel(Loads.Mass obj)
        {
            foreach (Loads.Mass elem in this.Entities.Loads.Masses)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Add an excitation force to the model.
        /// </summary>
        /// <param name="obj">Excitation force to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing excitation force.</param>
        /// <exception cref="ArgumentException">Thrown when an excitation force already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddExcitationForce(Loads.ExcitationForce obj, bool overwrite)
        {
            // in model?
            bool inModel = this.ExcitationLoadInModel();

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"Model has already an excitation load. Only one excitation load is supported.");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.Loads.ExcitationForce = null;
            }

            // add mass load
            this.Entities.Loads.ExcitationForce = obj;
        }

        /// <summary>
        /// Check whether an excitation force already exists in the model.
        /// </summary>
        /// <returns><c>true</c> if an excitation force exists; otherwise <c>false</c>.</returns>
        private bool ExcitationLoadInModel()
        {
            if(this.Entities.Loads.ExcitationForce != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add a periodic load record to the model periodic excitation container.
        /// </summary>
        /// <param name="obj">Periodic load record to add.</param>
        /// <param name="overwrite">Currently unused.</param>
        /// <exception cref="ArgumentException">Thrown when a record with the same name already exists.</exception>
        private void AddPeriodicExcitationForceRecords(Loads.PeriodicLoad obj, bool overwrite)
        {
            // in model?
            bool inModel = this.PeriodicExcitationForceInModel();

            if (!inModel)
            {
                this.Entities.Loads.PeriodicExcitations = new PeriodicExcitation();
            }

            // check if record is in model
            if (this.Entities.Loads.PeriodicExcitations.Records.Any(x => x.Name == obj.Name))
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with Name: {obj.Name} has already been added to model.");
            }

            this.Entities.Loads.PeriodicExcitations.Records.Add(obj);
        }



        /// <summary>
        /// Add periodic excitation data to the model.
        /// </summary>
        /// <param name="obj">Periodic excitation container to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing periodic excitation data.</param>
        /// <exception cref="ArgumentException">Thrown when periodic excitation already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddPeriodicExcitationForce(PeriodicExcitation obj, bool overwrite)
        {
            // in model?
            bool inModel = this.PeriodicExcitationForceInModel();

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"Model has already a periodic excitation load. Only one excitation load is supported.");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.Loads.PeriodicExcitations = null;
            }

            // add mass load
            this.Entities.Loads.PeriodicExcitations = obj;
        }

        /// <summary>
        /// Check whether periodic excitation data already exists in the model.
        /// </summary>
        /// <returns><c>true</c> if periodic excitation data exists; otherwise <c>false</c>.</returns>
        private bool PeriodicExcitationForceInModel()
        {
            if (this.Entities.Loads.PeriodicExcitations != null)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Add PointLoad to Model.
        /// </summary>
        private void AddPointSupportMotion(Loads.PointSupportMotion obj, bool overwrite)
        {
            // in model?
            bool inModel = this.PointMotionInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.Loads.PointSupportMotionLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add point load
            this.Entities.Loads.PointSupportMotionLoads.Add(obj);
        }

        /// <summary>
        /// Check if PointLoad in Model.
        /// </summary>
        private bool PointMotionInModel(Loads.PointSupportMotion obj)
        {
            foreach (Loads.PointSupportMotion elem in this.Entities.Loads.PointSupportMotionLoads)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Add LineLoad to Model.
        /// </summary>
        private void AddLineLoad(Loads.LineLoad obj, bool overwrite)
        {
            // in model?
            bool inModel = this.LineLoadInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.Loads.LineLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add line load
            this.Entities.Loads.LineLoads.Add(obj);
        }

        /// <summary>
        /// Check if LineLoad in Model.
        /// </summary>
        private bool LineLoadInModel(Loads.LineLoad obj)
        {
            foreach (Loads.LineLoad elem in this.Entities.Loads.LineLoads)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }




        /// <summary>
        /// Add LineSupportMotiomLoad to Model.
        /// </summary>
        private void AddLineSupportMotionLoad(Loads.LineSupportMotion obj, bool overwrite)
        {
            // in model?
            bool inModel = this.LineSupportMotionLoadInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.Loads.LineSupportMotionLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add line load
            this.Entities.Loads.LineSupportMotionLoads.Add(obj);
        }

        /// <summary>
        /// Check if LineLoad in Model.
        /// </summary>
        private bool LineSupportMotionLoadInModel(Loads.LineSupportMotion obj)
        {
            foreach (Loads.LineSupportMotion elem in this.Entities.Loads.LineSupportMotionLoads)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }









        /// <summary>
        /// Add a line stress load to the model.
        /// </summary>
        /// <param name="obj">Line stress load to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing load with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a load with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddLineStressLoad(Loads.LineStressLoad obj, bool overwrite)
        {
            // line stress loads null?
            if (this.Entities.Loads.LineStressLoads == null)
            {
                this.Entities.Loads.LineStressLoads = new List<Loads.LineStressLoad>();
            }

            // in model?
            bool inModel = this.Entities.Loads.LineStressLoads.Any(x => x.Guid == obj.Guid);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.LineStressLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add line stress load
            this.Entities.Loads.LineStressLoads.Add(obj);
        }

        /// <summary>
        /// Add LineTemperatureLoad to Model.
        /// </summary>
        private void AddLineTemperatureLoad(Loads.LineTemperatureLoad obj, bool overwrite)
        {
            // in model?
            bool inModel = this.LineTemperatureLoadInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }
            else if (inModel && overwrite == true)
            {
                this.Entities.Loads.LineTemperatureLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add line temperature load
            this.Entities.Loads.LineTemperatureLoads.Add(obj);
        }

        /// <summary>
        /// Check if LineTemperatureLoad in Model.
        /// </summary>
        private bool LineTemperatureLoadInModel(Loads.LineTemperatureLoad obj)
        {
            foreach (Loads.LineTemperatureLoad elem in this.Entities.Loads.LineTemperatureLoads)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add PressureLoad to Model.
        /// </summary>
        private void AddPressureLoad(Loads.PressureLoad obj, bool overwrite)
        {
            bool inModel = this.PressureLoadInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.PressureLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add pressure load
            this.Entities.Loads.PressureLoads.Add(obj);
        }

        /// <summary>
        /// Check if PressureLoad in Model.
        /// </summary>
        private bool PressureLoadInModel(Loads.PressureLoad obj)
        {
            foreach (Loads.PressureLoad elem in this.Entities.Loads.PressureLoads)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add SurfaceLoad to Model.
        /// </summary>
        private void AddSurfaceLoad(Loads.SurfaceLoad obj, bool overwrite)
        {
            // in model?
            bool inModel = this.SurfaceLoadInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.SurfaceLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add surface load
            this.Entities.Loads.SurfaceLoads.Add(obj);
        }

        /// <summary>
        /// Check if SurfaceLoad in Model.
        /// </summary>
        private bool SurfaceLoadInModel(Loads.SurfaceLoad obj)
        {
            foreach (Loads.SurfaceLoad elem in this.Entities.Loads.SurfaceLoads)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Add SurfaceLoad to Model.
        /// </summary>
        private void AddSurfaceSupportMotionLoad(Loads.SurfaceSupportMotion obj, bool overwrite)
        {
            // in model?
            bool inModel = this.SurfaceSupportMotionLoadInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.SurfaceSupportMotionLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add surface load
            this.Entities.Loads.SurfaceSupportMotionLoads.Add(obj);
        }

        /// <summary>
        /// Check if SurfaceLoad in Model.
        /// </summary>
        private bool SurfaceSupportMotionLoadInModel(Loads.SurfaceSupportMotion obj)
        {
            foreach (var elem in this.Entities.Loads.SurfaceSupportMotionLoads)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }





        /// <summary>
        /// Add SurfaceTemperatureLoad to Model.
        /// </summary>
        private void AddSurfaceTemperatureLoad(Loads.SurfaceTemperatureLoad obj, bool overwrite)
        {
            // in model?
            bool inModel = this.SurfaceTemperatureLoadInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.SurfaceTemperatureLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add surface temperature loads
            this.Entities.Loads.SurfaceTemperatureLoads.Add(obj);
        }

        /// <summary>
        /// Check if SurfaceLoad in Model.
        /// </summary>
        private bool SurfaceTemperatureLoadInModel(Loads.SurfaceTemperatureLoad obj)
        {
            foreach (Loads.SurfaceTemperatureLoad elem in this.Entities.Loads.SurfaceTemperatureLoads)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add MassConversionTable to Model.
        /// MassConversionTable is always overwritten.
        /// </summary>
        private void AddMassConversionTable(Loads.MassConversionTable obj)
        {
            this.Entities.Loads.LoadCaseMassConversionTable = obj;
        }

        private void AddMovingLoad(Loads.MovingLoad obj, bool overwrite)
        {
            if (this.Entities.Loads.MovingLoads == null)
                this.Entities.Loads.MovingLoads = new List<StruSoft.Interop.StruXml.Data.Moving_load_type>();

            // in model?
            bool inModel = this.MovingLoadInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.MovingLoads.RemoveAll(x => x.Guid == obj.Guid.ToString());
            }

            // add moving loads
            this.Entities.Loads.MovingLoads.Add(obj);

            this.AddVehicle(obj, overwrite);
        }



        private void AddVehicle(MovingLoad obj, bool overwrite)
        {
            if (this.VehicleTypes == null)
                this.VehicleTypes = new VehicleTypes();

            if (this.VehicleTypes.PredefinedTypes == null)
                this.VehicleTypes.PredefinedTypes = new List<StruSoft.Interop.StruXml.Data.Vehicle_lib_type>();

            bool inModel = this.VehicleInModel(obj);

            //// in model, don't overwrite
            //if (inModel && !overwrite)
            //{
            //    throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            //}

            //else if (inModel && overwrite)
            //{
            //    this.VehicleTypes.PredefinedTypes.RemoveAll(x => x.AnyAttr[0].Value == obj.Guid.ToString());
            //    this.VehicleTypes.PredefinedTypes.Add(obj.Vehicle);
            //}

            if (!inModel)
            {
                this.VehicleTypes.PredefinedTypes.Add(obj.Vehicle);
            }

        }

        private bool VehicleInModel(MovingLoad obj)
        {
            foreach (var elem in this.VehicleTypes.PredefinedTypes)
            {
                if (elem.Guid == obj._vehicleGuid)
                {
                    return true;
                }
            }
            return false;
        }



        private bool MovingLoadInModel(Loads.MovingLoad obj)
        {
            foreach (var elem in this.Entities.Loads.MovingLoads)
            {
                if (elem.Guid == obj.Guid.ToString())
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Add Footfall to Model.
        /// </summary>
        private void AddFootfall(Loads.Footfall obj, bool overwrite)
        {
            // in model?
            bool inModel = this.FootfallInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.FootfallAnalysisData.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add footfall
            this.Entities.Loads.FootfallAnalysisData.Add(obj);
        }

        /// <summary>
        /// Check if Footfall in Model.
        /// </summary>
        private bool FootfallInModel(Loads.Footfall obj)
        {
            foreach (Loads.Footfall elem in this.Entities.Loads.FootfallAnalysisData)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add load cases to the model.
        /// </summary>
        /// <param name="loadCases">Load cases to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing load cases with matching GUID.</param>
        /// <returns>The current model instance.</returns>
        public Model AddLoadCases(IEnumerable<Loads.LoadCase> loadCases, bool overwrite = true)
        {
            // check if model contains entities, sections and materials
            if (this.Entities == null)
                this.Entities = new Entities();

            if (loadCases != null)
                foreach (Loads.LoadCase loadCase in loadCases)
                    this.AddLoadCase(loadCase, overwrite);

            return this;
        }

        /// <summary>
        /// Add load cases to the model.
        /// </summary>
        /// <param name="loadCases">Load cases to add.</param>
        /// <returns>The current model instance.</returns>
        public Model AddLoadCases(params Loads.LoadCase[] loadCases)
        {
            return AddLoadCases(loadCases, overwrite: true);
        }

        /// <summary>
        /// Add LoadCase to Model.
        /// </summary>
        private void AddLoadCase(Loads.LoadCase loadCase, bool overwrite)
        {
            if (loadCase is null)
                throw new ArgumentNullException("loadCase can not be null!");

            // in model?
            bool inModel = this.LoadCaseInModel(loadCase);

            // in model, don't overwrite?
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{loadCase.GetType().FullName} with guid: {loadCase.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.LoadCases.RemoveAll(x => x.Guid == loadCase.Guid);
            }

            // add load case
            if (this.LoadCaseNameTaken(loadCase))
            {
                loadCase.Name = loadCase.Name + " (1)";
            }
            this.Entities.Loads.LoadCases.Add(loadCase);
        }

        /// <summary>
        /// Check if LoadCase in Model.
        /// </summary>
        private bool LoadCaseInModel(Loads.LoadCase obj)
        {
            foreach (Loads.LoadCase elem in this.Entities.Loads.LoadCases)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if LoadCase name is in use in Model.
        /// </summary>
        private bool LoadCaseNameTaken(Loads.LoadCase obj)
        {
            foreach (Loads.LoadCase elem in this.Entities.Loads.LoadCases)
            {
                if (elem.Name == obj.Name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add load combinations to the model.
        /// </summary>
        /// <param name="loadCombinations">Load combinations to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing load combinations with matching GUID.</param>
        /// <returns>The current model instance.</returns>
        public Model AddLoadCombinations(IEnumerable<Loads.LoadCombination> loadCombinations, bool overwrite = true)
        {
            // check if model contains entities, sections and materials
            if (this.Entities == null)
                this.Entities = new Entities();

            if (loadCombinations != null)
                foreach (Loads.LoadCombination loadCombination in loadCombinations)
                    this.AddLoadCombination(loadCombination, overwrite);

            this.CheckCombItems();

            return this;
        }

        /// <summary>
        /// Add load combinations to the model.
        /// </summary>
        /// <param name="loadCombinations">Load combinations to add.</param>
        /// <returns>The current model instance.</returns>
        public Model AddLoadCombinations(params Loads.LoadCombination[] loadCombinations)
        {
            return AddLoadCombinations(loadCombinations, overwrite: true);
        }

        /// <summary>
        /// Check if any load combination has a combItem object.
        /// if it does throw exception if not all of them has a combItem.
        /// Either only combItems or no combItems :)
        /// </summary>
        private void CheckCombItems()
        {
            if (this.Entities.Loads.LoadCombinations.Any(x => x.CombItem != null) && this.Entities.Loads.LoadCombinations.Any(x => x.CombItem == null))
            {
                throw new System.ArgumentException("Some load combinations have calculation setup (combItem) while others do not.\nIf you are trying to add some Load Combinations to an already existing model, you should specify 'CombItem' for all the existing Load Combinations.");
            }
        }

        /// <summary>
        /// Add LoadGroupTable to Model.
        /// </summary>
        /// <param name="generalLoadGroups">Load groups to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing load group table.</param>
        /// <exception cref="ArgumentException">Thrown when the model already contains a load group table and <paramref name="overwrite"/> is <c>false</c>.</exception>
        public void AddLoadGroupTable(List<Loads.ModelGeneralLoadGroup> generalLoadGroups, bool overwrite)
        {
            // Null or no load groups
            if (generalLoadGroups == null || generalLoadGroups.Count == 0) return;

            // check if model contains entities, sections and materials
            if (this.Entities == null)
                this.Entities = new Entities();

            // Create load group table with the sequenced general_load_group_type
            Loads.LoadGroupTable loadGroupTable = new Loads.LoadGroupTable(generalLoadGroups);

            // in model?
            bool inModel = this.LoadGroupTableInModel();

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException("The model already contains a load group table");
            }

            // not in model, or overwrite
            this.Entities.Loads.LoadGroupTable = loadGroupTable;
        }

        /// <summary>
        /// Add LoadCombination to Model.
        /// </summary>
        private void AddLoadCombination(Loads.LoadCombination loadCombination, bool overwrite)
        {
            if (loadCombination is null)
                throw new ArgumentNullException("loadCombination");

            // in model?
            bool inModel = this.LoadCombinationInModel(loadCombination);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{loadCombination.GetType().FullName} with guid: {loadCombination.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.LoadCombinations.RemoveAll(x => x.Guid == loadCombination.Guid);
            }

            // add load combination
            if (this.LoadCombinationNameTaken(loadCombination))
            {
                loadCombination.Name = loadCombination.Name + " (1)";
            }
            this.Entities.Loads.LoadCombinations.Add(loadCombination);
        }

        /// <summary>
        /// Check if LoadCombination in Model.
        /// </summary>
        private bool LoadCombinationInModel(Loads.LoadCombination obj)
        {
            foreach (Loads.LoadCombination elem in this.Entities.Loads.LoadCombinations)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if the model already has a load group table
        /// </summary>
        private bool LoadGroupTableInModel()
        {
            if (this.Entities.Loads.LoadGroupTable == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Check if LoadCombination name is in use in Model.
        /// </summary>
        private bool LoadCombinationNameTaken(Loads.LoadCombination obj)
        {
            foreach (Loads.LoadCombination elem in this.Entities.Loads.LoadCombinations)
            {
                if (elem.Name == obj.Name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Slab to Model.
        /// </summary>
        private void AddSlab(Shells.Slab obj, bool overwrite)
        {
            // in model?
            bool inModel = this.SlabInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Slabs.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add shell properties
            this.AddMaterial(obj.Material, overwrite);
            this.AddSurfaceReinforcementParameters(obj, overwrite);

            // add SurfaceReinforcement
            this.AddSurfaceReinforcements(obj, overwrite);

            // add ShearControlRegion
            this.AddShearControlRegions(obj, overwrite);

            // add PunchingReinforcement
            this.AddPunchingReinforcements(obj, overwrite);

            // add line connection types (predefined rigidity)
            foreach (Releases.RigidityDataLibType3 predef in obj.SlabPart.Region.GetPredefinedRigidities())
            {
                this.AddPredefinedRigidity(predef, overwrite);
            }

            // add shellC
            this.Entities.Slabs.Add(obj);
        }

        /// <summary>
        /// Check if Slab in Model.
        /// </summary>
        private bool SlabInModel(Shells.Slab obj)
        {
            foreach (Shells.Slab elem in this.Entities.Slabs)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Material (reinforcing) to Model.
        /// </summary>
        private void AddReinforcingMaterial(Materials.Material obj, bool overwrite)
        {
            // in model?
            bool inModel = this.ReinforcingMaterialInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.ReinforcingMaterials.Material.RemoveAll(x => x.Guid == obj.Guid);
                this.ReinforcingMaterials.Material.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.ReinforcingMaterials.Material.Add(obj);
            }
        }

        /// <summary>
        /// Check if Material (reinforcring) in Model.
        /// </summary>
        private bool ReinforcingMaterialInModel(Materials.Material obj)
        {
            if(this.ReinforcingMaterials == null)
            {
                this.ReinforcingMaterials = new Materials.ReinforcingMaterials();
                return false;
            }

            foreach (Materials.Material elem in this.ReinforcingMaterials.Material)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Add predefined rigidity
        /// </summary>
        private void AddPredefinedRigidity(Releases.RigidityDataLibType3 obj, bool overwrite)
        {
            // in model?
            bool inModel = this.PredefRigidityInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.LineConnectionTypes.PredefinedTypes.RemoveAll(x => x.Guid == obj.Guid);
                this.LineConnectionTypes.PredefinedTypes.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.LineConnectionTypes.PredefinedTypes.Add(obj);
            }
        }

        /// <summary>
        /// Check if predefined rigidity.
        /// </summary>
        private bool PredefRigidityInModel(Releases.RigidityDataLibType3 obj)
        {
            foreach (Releases.RigidityDataLibType3 elem in this.LineConnectionTypes.PredefinedTypes)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add StructureGrid (axis or storey) to model.
        /// </summary>
        /// <param name="obj">Axis, Storey</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing objects with matching GUID.</param>
        private void AddStructureGrid(object obj, bool overwrite)
        {
            if (obj == null)
            {
                throw new System.ArgumentException("Passed object is null");
            }
            else if (obj.GetType() == typeof(StructureGrid.Axis))
            {
                this.AddAxis((StructureGrid.Axis)obj, overwrite);
            }
            else if (obj.GetType() == typeof(StructureGrid.Storey))
            {
                this.AddStorey((StructureGrid.Storey)obj, overwrite);
            }
            else
            {
                throw new System.ArgumentException("Passed object must be Axis or Storey");
            }
        }

        /// <summary>
        /// Add axis to entities.
        /// </summary>
        /// <param name="obj">Axis.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing axis with matching GUID.</param>
        private void AddAxis(StructureGrid.Axis obj, bool overwrite)
        {
            // check if axes in entities
            if (this.Entities.Axes == null)
            {
                this.Entities.Axes = new StructureGrid.Axes();
            }

            // in model?
            bool inModel = this.AxisInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Axes.Axis.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.Axes.Axis.Add(obj);
        }

        /// <summary>
        /// Check if axis in entities.
        /// </summary>
        /// <param name="obj">Axis.</param>
        /// <returns><c>true</c> if an axis with the same GUID already exists in the model; otherwise <c>false</c>.</returns>
        private bool AxisInModel(StructureGrid.Axis obj)
        {
            foreach (StructureGrid.Axis elem in this.Entities.Axes.Axis)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Storey to Model.
        /// </summary>
        /// <param name="obj">Storey.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing storey with matching GUID.</param>
        private void AddStorey(StructureGrid.Storey obj, bool overwrite)
        {
            // check if storeys in entities
            if (this.Entities.Storeys == null)
            {
                this.Entities.Storeys = new StructureGrid.Storeys();
            }

            // in model?
            bool inModel = this.StoreyInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            else if (inModel && overwrite)
            {
                this.Entities.Storeys.Storey.RemoveAll(x => x.Guid == obj.Guid);
            }

            // check if geometry is consistent
            this.ConsistenStoreyGeometry(obj);

            // add to storeys
            this.Entities.Storeys.Storey.Add(obj);
        }

        /// <summary>
        /// Check if storey in entities.
        /// </summary>
        /// <param name="obj">Storey.</param>
        /// <returns><c>true</c> if a storey with the same GUID already exists in the model; otherwise <c>false</c>.</returns>
        private bool StoreyInModel(StructureGrid.Storey obj)
        {
            foreach (StructureGrid.Storey elem in this.Entities.Storeys.Storey)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if geometry of storey is consistent with geometry of storeys aldread added.
        /// Storey origo should share XY-coordinates. Z-coordinate should be unique.
        /// Storey direction should be identical.
        /// </summary>
        /// <param name="obj">Storey to validate against storeys already added to the model.</param>
        private void ConsistenStoreyGeometry(StructureGrid.Storey obj)
        {
            foreach (StructureGrid.Storey elem in this.Entities.Storeys.Storey)
            {
                if (elem.Origo.X != obj.Origo.X || elem.Origo.Y != obj.Origo.Y)
                {
                    throw new System.ArgumentException($"Storey does not share XY-coordinates with storeys in model (point x: {elem.Origo.X}, y: {elem.Origo.Y}). If model was empty make sure all storeys added to model share XY-coordinates.");
                }
                if (!elem.Direction.Equals(obj.Direction))
                {
                    throw new System.ArgumentException($"Storey does not share direction with storeys in model (vector i: {elem.Direction.X} , j: {elem.Direction.Y}). If model was empty make sure all storeys added to model share direction.");
                }
            }
        }

        /// <summary>
        /// Add BarReinforcement(s) from Bar to Model.
        /// </summary>
        /// <param name="obj">Bar containing reinforcements to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing objects with matching GUID (where supported).</param>
        private void AddBarReinforcements(Bars.Bar obj, bool overwrite)
        {
            foreach (Reinforcement.BarReinforcement barReinf in obj.Reinforcement)
            {
                this.AddReinforcingMaterial(barReinf.Wire.ReinforcingMaterial, overwrite);
                this.AddBarReinforcement(barReinf, overwrite);
            }
        }

        /// <summary>
        /// Add BarReinforcement to Model.
        /// </summary>
        /// <param name="obj">Bar reinforcement to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing reinforcement with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a reinforcement with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddBarReinforcement(Reinforcement.BarReinforcement obj, bool overwrite)
        {
            // in model?
            bool inModel = this.BarReinforcementInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Did you add the same {obj.GetType().FullName} to different Bars?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.BarReinforcements.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.BarReinforcements.Add(obj);
        }

        /// <summary>
        /// Check if BarReinforcement in Model.
        /// </summary>
        /// <param name="obj">Bar reinforcement to check.</param>
        /// <returns>
        /// <c>true</c> if a reinforcement with the same GUID already exists for the same base bar; otherwise <c>false</c>.
        /// </returns>
        private bool BarReinforcementInModel(Reinforcement.BarReinforcement obj)
        {
            foreach (Reinforcement.BarReinforcement elem in this.Entities.BarReinforcements)
            {
                if (elem.Guid == obj.Guid)
                {
                    // If the GUID is the same and the `BaseBar` is different, it means that we are trying to add the same reinforcement object to the model that we used for a 
                    // bar previously added to the model, but we want to use this reinf. again for a different bar. In this case, we need to add this reinf. as a new object
                    // with a new GUID, and this method should return `true`, as we don't have a duplicate object case.
                    if (elem.BaseBar.Guid == obj.BaseBar.Guid)
                    {
                        return true;
                    }
                    else
                    {
                        obj.Guid = Guid.NewGuid();
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Add SurfaceReinforcement(s) from Slab to Model.
        /// </summary>
        /// <param name="obj">Slab containing surface reinforcements to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing reinforcements with matching GUID.</param>
        private void AddSurfaceReinforcements(Shells.Slab obj, bool overwrite)
        {
            foreach (Reinforcement.SurfaceReinforcement surfaceReinforcement in obj.SurfaceReinforcement)
            {
                this.AddReinforcingMaterial(surfaceReinforcement.Wire.ReinforcingMaterial, overwrite);
                this.AddSurfaceReinforcement(surfaceReinforcement, overwrite);
            }
        }


        /// <summary>
        /// Add SurfaceReinforcement to Model.
        /// </summary>
        /// <param name="obj">Surface reinforcement to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing reinforcement with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a reinforcement with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddSurfaceReinforcement(Reinforcement.SurfaceReinforcement obj, bool overwrite)
        {
            // in model?
            bool inModel = this.SurfaceReinforcementInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Did you add the same {obj.GetType().FullName} to different Slabs?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.SurfaceReinforcements.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.SurfaceReinforcements.Add(obj);

        }

        /// <summary>
        /// Check if SurfaceReinforcement in Model.
        /// </summary>
        /// <param name="obj">Surface reinforcement to check.</param>
        /// <returns>
        /// <c>true</c> if a reinforcement with the same GUID already exists for the same base shell; otherwise <c>false</c>.
        /// </returns>
        private bool SurfaceReinforcementInModel(Reinforcement.SurfaceReinforcement obj)
        {
            foreach (Reinforcement.SurfaceReinforcement elem in this.Entities.SurfaceReinforcements)
            {
                if (elem.Guid == obj.Guid)
                {
                    // If the GUID is the same and the `BaseShell` is different, it means that we are trying to add the same reinforcement object to the model that we used for a 
                    // shell previously added to the model, but we want to use this reinf. again for a different shell. In this case, we need to add this reinf. as a new object
                    // with a new GUID, and this method should return `true`, as we don't have a duplicate object case.
                    if (elem.BaseShell.Guid == obj.BaseShell.Guid)
                    {
                        return true;
                    }
                    else
                    {
                        obj.Guid = Guid.NewGuid();
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Add SurfaceReinforcementParameters to Model.
        /// </summary>
        /// <param name="slab">Slab containing surface reinforcement parameters to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing parameters with matching GUID.</param>
        private void AddSurfaceReinforcementParameters(Shells.Slab slab, bool overwrite)
        {
            if (slab.SurfaceReinforcementParameters != null)
            {
                // obj
                Reinforcement.SurfaceReinforcementParameters obj = slab.SurfaceReinforcementParameters;
                // in model?
                bool inModel = this.SurfaceReinforcementParametersInModel(obj);

                // in model, don't overwrite
                if (inModel && !overwrite)
                {
                    throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
                }

                // in model, overwrite
                else if (inModel && overwrite)
                {
                    this.Entities.SurfaceReinforcementParameters.RemoveAll(x => x.Guid == obj.Guid);
                }

                // add obj
                this.Entities.SurfaceReinforcementParameters.Add(obj);
            }
        }

        /// <summary>
        /// Check if SurfaceReinforcementParameters in Model.
        /// </summary>
        /// <param name="obj">Surface reinforcement parameters to check.</param>
        /// <returns><c>true</c> if parameters with the same GUID already exist in the model; otherwise <c>false</c>.</returns>
        private bool SurfaceReinforcementParametersInModel(Reinforcement.SurfaceReinforcementParameters obj)
        {
            foreach (Reinforcement.SurfaceReinforcementParameters elem in this.Entities.SurfaceReinforcementParameters)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Add shear control regions from a slab to the model.
        /// </summary>
        /// <param name="obj">Slab containing shear control regions to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing regions with matching GUID.</param>
        private void AddShearControlRegions(Shells.Slab obj, bool overwrite)
        {
            foreach (Reinforcement.ShearControlRegionType surfaceReinforcement in obj.ShearControlRegions)
            {
                this.AddShearControlRegion(surfaceReinforcement, overwrite);
            }
        }

        /// <summary>
        /// Add a shear control region to the model.
        /// </summary>
        /// <param name="obj">Shear control region to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing region with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a region with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddShearControlRegion(Reinforcement.ShearControlRegionType obj, bool overwrite)
        {
            // in model?
            bool inModel = this.ShearControlRegionInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Did you add the same {obj.GetType().FullName} to different Slabs?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.NoShearControlRegions.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.NoShearControlRegions.Add(obj);
        }

        /// <summary>
        /// Check if ShearControlRegion in Model.
        /// </summary>
        /// <param name="obj">Shear control region to check.</param>
        /// <returns><c>true</c> if a region with the same GUID already exists in the model; otherwise <c>false</c>.</returns>
        private bool ShearControlRegionInModel(Reinforcement.ShearControlRegionType obj)
        {
            foreach (Reinforcement.ShearControlRegionType elem in this.Entities.NoShearControlRegions)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Add SurfaceReinforcement(s) from Slab to Model.
        /// </summary>
        /// <param name="obj">Slab containing punching reinforcements to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing reinforcements with matching GUID.</param>
        private void AddPunchingReinforcements(Shells.Slab obj, bool overwrite)
        {
            foreach (Reinforcement.PunchingReinforcement punchingReinforcement in obj.PunchingReinforcement)
            {
                this.AddPunchingReinforcement(punchingReinforcement, overwrite);
            }
        }

        /// <summary>
        /// Add a punching reinforcement to the model.
        /// </summary>
        /// <param name="obj">Punching reinforcement to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing reinforcement with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a reinforcement with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddPunchingReinforcement(Reinforcement.PunchingReinforcement obj, bool overwrite)
        {
            // in model?
            bool inModel = this.PunchingReinforcementInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Did you add the same {obj.GetType().FullName} to different Slabs?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.PunchingReinforcements.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.PunchingReinforcements.Add(obj);

            this.AddPunchingArea(obj.PunchingArea, overwrite);
        }

        /// <summary>
        /// Add a punching area definition to the model.
        /// </summary>
        /// <param name="obj">Punching area to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing punching area with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a punching area with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddPunchingArea(Reinforcement.PunchingArea obj, bool overwrite)
        {
            // in model?
            bool inModel = this.PunchingAreaInModel(obj);
            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Did you add the same {obj.GetType().FullName} to different Slabs?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.PunchingArea.RemoveAll(x => x.Guid == obj.Guid);
            }

            // need a method to set local_x, local_y and region.
            // localx and localy are set according to the connected bar.
            // region is set according to the the section region of the bar

            this.Entities.PunchingArea.Add(obj);

        }

        /// <summary>
        /// Check if a punching reinforcement exists in the model.
        /// </summary>
        /// <param name="obj">Punching reinforcement to check.</param>
        /// <returns><c>true</c> if a reinforcement with the same GUID already exists in the model; otherwise <c>false</c>.</returns>
        private bool PunchingReinforcementInModel(Reinforcement.PunchingReinforcement obj)
        {
            foreach (Reinforcement.PunchingReinforcement elem in this.Entities.PunchingReinforcements)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if a punching area exists in the model.
        /// </summary>
        /// <param name="obj">Punching area to check.</param>
        /// <returns><c>true</c> if a punching area with the same GUID already exists in the model; otherwise <c>false</c>.</returns>
        private bool PunchingAreaInModel(Reinforcement.PunchingArea obj)
        {
            foreach (Reinforcement.PunchingArea elem in this.Entities.PunchingArea)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Foundation to the Model
        /// </summary>
        /// <param name="obj">Isolated Foundation, Line Foundation or Slab Foundation</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing foundations with matching GUID (where supported).</param>
        private void AddFoundation(IFoundationElement obj, bool overwrite)
        {
            if (obj == null)
            {
                throw new System.ArgumentException("Passed object is null");
            }
            else if (obj.GetType() == typeof(Foundations.IsolatedFoundation))
            {
                this.AddIsolatedFoundation((Foundations.IsolatedFoundation)obj, overwrite);
                this.AddMaterial(((Foundations.IsolatedFoundation)obj).ComplexMaterialObj, overwrite);
            }
            else if (obj.GetType() == typeof(Foundations.SlabFoundation))
            {
                this.AddSlabFoundation((Foundations.SlabFoundation)obj, overwrite);
                this.AddMaterial(((Foundations.IsolatedFoundation)obj).ComplexMaterialObj, overwrite);
            }
            else
            {
                throw new System.ArgumentException("Passed object must be IsolatedFoundation. LineFoundation or WallFoundation NOT YET implemented!");
            }
        }

        /// <summary>
        /// Add isolated foundation to the model.
        /// </summary>
        /// <param name="obj">Isolated foundation to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing foundation with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a foundation with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddIsolatedFoundation(Foundations.IsolatedFoundation obj, bool overwrite)
        {
            // in model?
            bool inModel = this.IsolatedFoundationInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Foundations.IsolatedFoundations.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.Foundations.IsolatedFoundations.Add(obj);
        }

        /// <summary>
        /// Check if isolated foundation exists in the model.
        /// </summary>
        /// <param name="obj">Isolated foundation to check.</param>
        /// <returns><c>true</c> if an isolated foundation with the same GUID already exists in the model; otherwise <c>false</c>.</returns>
        private bool IsolatedFoundationInModel(Foundations.IsolatedFoundation obj)
        {
            foreach (Foundations.IsolatedFoundation elem in this.Entities.Foundations.IsolatedFoundations)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }





        /// <summary>
        /// Add Slab foundation to Model.
        /// </summary>
        /// <param name="obj">Slab foundation to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing foundation with matching GUID.</param>
        /// <exception cref="ArgumentException">Thrown when a foundation with the same GUID already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        private void AddSlabFoundation(Foundations.SlabFoundation obj, bool overwrite)
        {
            // in model?
            bool inModel = this.SlabFoundationInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Foundations.SlabFoundations.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.Foundations.SlabFoundations.Add(obj);
        }

        /// <summary>
        /// Check if Slab foundation in Model.
        /// </summary>
        private bool SlabFoundationInModel(Foundations.SlabFoundation obj)
        {
            foreach (Foundations.SlabFoundation elem in this.Entities.Foundations.SlabFoundations)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }







        /// <summary>
        /// Add PointSupport to Model.
        /// </summary>
        private void AddSoil(Soil.SoilElements obj, bool overwrite)
        {
            // in model?
            bool inModel = this.SoilInModel();

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException("Model can only have one Soil element object");
            }

            // in model, overwrite
            // add obj
            this.Entities.SoilElements = obj;
            foreach (var stratum in obj.Strata.Stratum)
                this.AddMaterial(stratum.Material, overwrite);
        }

        /// <summary>
        /// Check if PointSupport in Model.
        /// </summary>
        private bool SoilInModel()
        {
            if (this.Entities.SoilElements != null)
                return true;
            else
                return false;
        }



        /// <summary>
        /// Add Support to Model
        /// </summary>
        /// <param name="obj">PointSupport, LineSupport or SurfaceSupport</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing supports with matching GUID.</param>
        private void AddSupport(ISupportElement obj, bool overwrite)
        {
            if (obj == null)
            {
                throw new System.ArgumentException("Passed object is null");
            }
            else if (obj.GetType() == typeof(Supports.PointSupport))
            {
                this.AddPointSupport((Supports.PointSupport)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Supports.LineSupport))
            {
                this.AddLineSupport((Supports.LineSupport)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Supports.SurfaceSupport))
            {
                this.AddSurfaceSupport((Supports.SurfaceSupport)obj, overwrite);
            }
            else
            {
                throw new System.ArgumentException("Passed object must be PointSupport, LineSupport or SurfaceSupport");
            }
        }

        /// <summary>
        /// Add PointSupport to Model.
        /// </summary>
        private void AddPointSupport(Supports.PointSupport obj, bool overwrite)
        {
            // in model?
            bool inModel = this.PointSupportInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Supports.PointSupport.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.Supports.PointSupport.Add(obj);

            // add predefined rigidity
            if (obj.Group?.PredefRigidity != null)
            {
                this.AddPointSupportGroupLibItem(obj.Group.PredefRigidity, overwrite);
            }
        }

        /// <summary>
        /// Add predefined point support rigidity to model
        /// </summary>
        private void AddPointSupportGroupLibItem(Releases.RigidityDataLibType2 obj, bool overwrite)
        {
            // if null create new element
            if (this.PointSupportGroupTypes == null)
            {
                this.PointSupportGroupTypes = new LibraryItems.PointSupportGroupTypes();
                this.PointSupportGroupTypes.PredefinedTypes = new List<Releases.RigidityDataLibType2>();
            }

            // in model?
            bool inModel = this.PointSupportGroupTypes.PredefinedTypes.Any(x => x.Guid == obj.Guid);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.PointSupportGroupTypes.PredefinedTypes.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add lib item
            this.PointSupportGroupTypes.PredefinedTypes.Add(obj);
        }

        /// <summary>
        /// Check if PointSupport in Model.
        /// </summary>
        private bool PointSupportInModel(Supports.PointSupport obj)
        {
            foreach (Supports.PointSupport elem in this.Entities.Supports.PointSupport)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Add LineSupport to Model.
        /// </summary>
        private void AddLineSupport(Supports.LineSupport obj, bool overwrite)
        {
            // in model?
            bool inModel = this.LineSupportInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Supports.LineSupport.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.Supports.LineSupport.Add(obj);

            // add lib item
            if (obj.Group?.PredefRigidity != null)
            {
                this.AddLineSupportGroupLibItem(obj.Group.PredefRigidity, overwrite);
            }
        }

        /// <summary>
        /// Check if LineSupport in Model.
        /// </summary>
        private bool LineSupportInModel(Supports.LineSupport obj)
        {
            foreach (Supports.LineSupport elem in this.Entities.Supports.LineSupport)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add predefined line support rigidity to model
        /// </summary>
        private void AddLineSupportGroupLibItem(Releases.RigidityDataLibType2 obj, bool overwrite)
        {
            // if null create new element
            if (this.LineSupportGroupTypes == null)
            {
                this.LineSupportGroupTypes = new LibraryItems.LineSupportGroupTypes();
                this.LineSupportGroupTypes.PredefinedTypes = new List<Releases.RigidityDataLibType2>();
            }

            // in model?
            bool inModel = this.LineSupportGroupTypes.PredefinedTypes.Any(x => x.Guid == obj.Guid);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.LineSupportGroupTypes.PredefinedTypes.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.LineSupportGroupTypes.PredefinedTypes.Add(obj);
        }

        /// <summary>
        /// Add SurfaceSupport to Model.
        /// </summary>
        private void AddSurfaceSupport(Supports.SurfaceSupport obj, bool overwrite)
        {
            // in model?
            bool inModel = this.SurfaceSupportInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Supports.SurfaceSupport.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.Supports.SurfaceSupport.Add(obj);

            // add lib item
            if (obj.PredefRigidity != null)
            {
                this.AddSurfaceSupportLibItem(obj.PredefRigidity, overwrite);
            }
        }

        /// <summary>
        /// Check if SurfaceSupport in Model.
        /// </summary>
        private bool SurfaceSupportInModel(Supports.SurfaceSupport obj)
        {
            foreach (Supports.SurfaceSupport elem in this.Entities.Supports.SurfaceSupport)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add predefined surface support rigidity to model
        /// </summary>
        private void AddSurfaceSupportLibItem(Releases.RigidityDataLibType1 obj, bool overwrite)
        {
            // if null create new element
            if (this.SurfaceSupportTypes == null)
            {
                this.SurfaceSupportTypes = new LibraryItems.SurfaceSupportTypes();
                this.SurfaceSupportTypes.PredefinedTypes = new List<Releases.RigidityDataLibType1>();
            }

            // in model?
            bool inModel = this.SurfaceSupportTypes.PredefinedTypes.Any(x => x.Guid == obj.Guid);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.SurfaceSupportTypes.PredefinedTypes.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.SurfaceSupportTypes.PredefinedTypes.Add(obj);
        }


        /// <summary>
        /// Add SurfaceSupport to Model.
        /// </summary>
        private void AddStiffnessPoint(Supports.StiffnessPoint obj, bool overwrite)
        {
            // in model?
            bool inModel = this.StiffnessPointInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Supports.StiffnessPoint.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.Supports.StiffnessPoint.Add(obj);
        }


        /// <summary>
        /// Check if StiffnessPoint in Model.
        /// </summary>
        private bool StiffnessPointInModel(Supports.StiffnessPoint obj)
        {
            foreach (Supports.StiffnessPoint elem in this.Entities.Supports.StiffnessPoint)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Add Material to Model.
        /// </summary>
        private void AddMaterial(Materials.Material obj, bool overwrite)
        {
            // in model?
            bool inModel = this.MaterialInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Materials.Material.RemoveAll(x => x.Guid == obj.Guid);
                this.Materials.Material.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.Materials.Material.Add(obj);
            }
        }

        /// <summary>
        /// Check if Material in Model.
        /// </summary>
        private bool MaterialInModel(Materials.Material obj)
        {
            if (this.Materials == null)
            {
                this.Materials = new Materials.Materials();
                return false;
            }

            foreach (Materials.Material elem in this.Materials.Material)
            {
                if (obj != null && elem != null)
                {
                    if (elem.Guid == obj.Guid)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void AddPtcStrandType(Reinforcement.PtcStrandLibType obj, bool overwrite)
        {
            bool inModel = this.PtcStrandTypeInModel(obj);
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }
            else if (inModel && overwrite)
            {
                this.PtcStrandTypes.PtcStrandLibTypes.RemoveAll(x => x.Guid == obj.Guid);
                this.PtcStrandTypes.PtcStrandLibTypes.Add(obj);
            }
            else if (!inModel)
                this.PtcStrandTypes.PtcStrandLibTypes.Add(obj);
        }

        private bool PtcStrandTypeInModel(Reinforcement.PtcStrandLibType obj)
        {
            foreach (Reinforcement.PtcStrandLibType elem in this.PtcStrandTypes.PtcStrandLibTypes)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Timber panel library type to Model.
        /// </summary>
        private void AddTimberPanelLibraryType(Materials.OrthotropicPanelLibraryType obj, bool overwrite)
        {
            // if null create new element
            if (this.OrthotropicPanelTypes == null)
            {
                this.OrthotropicPanelTypes = new Materials.OrthotropicPanelTypes();
                this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes = new List<Materials.OrthotropicPanelLibraryType>();
            }

            // in model?
            bool inModel = this.TimberPanelLibraryTypeInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes.RemoveAll(x => x.Guid == obj.Guid);
                this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes.Add(obj);
            }
        }

        /// <summary>
        /// Check if Timber panel library type in Model.
        /// </summary>
        private bool TimberPanelLibraryTypeInModel(Materials.OrthotropicPanelLibraryType obj)
        {
            foreach (Materials.OrthotropicPanelLibraryType elem in this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Clt panel library type to Model.
        /// </summary>
        private void AddCltPanelLibraryType(Materials.CltPanelLibraryType obj, bool overwrite)
        {
            // if null create new element
            if (this.CltPanelTypes == null)
            {
                this.CltPanelTypes = new Materials.CltPanelTypes();
                this.CltPanelTypes.CltPanelLibraryTypes = new List<Materials.CltPanelLibraryType>();
            }

            // in model?
            bool inModel = this.CltPanelLibraryTypeInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.CltPanelTypes.CltPanelLibraryTypes.RemoveAll(x => x.Guid == obj.Guid);
                this.CltPanelTypes.CltPanelLibraryTypes.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.CltPanelTypes.CltPanelLibraryTypes.Add(obj);
            }
        }

        /// <summary>
        /// Check if Clt panel library type in Model.
        /// </summary>
        private bool CltPanelLibraryTypeInModel(Materials.CltPanelLibraryType obj)
        {
            foreach (Materials.CltPanelLibraryType elem in this.CltPanelTypes.CltPanelLibraryTypes)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Glc panel library type to Model.
        /// </summary>
        private void AddGlcPanelLibraryType(Materials.GlcPanelLibraryType obj, bool overwrite)
        {
            // if null create new element
            if (this.GlcPanelTypes == null)
            {
                this.GlcPanelTypes = new Materials.GlcPanelTypes();
                this.GlcPanelTypes.GlcPanelLibraryTypes = new List<Materials.GlcPanelLibraryType>();
            }

            // in model?
            bool inModel = this.GlcPanelLibraryTypeInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.GlcPanelTypes.GlcPanelLibraryTypes.RemoveAll(x => x.Guid == obj.Guid);
                this.GlcPanelTypes.GlcPanelLibraryTypes.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.GlcPanelTypes.GlcPanelLibraryTypes.Add(obj);
            }
        }

        /// <summary>
        /// Check if Glc panel library type in Model.
        /// </summary>
        private bool GlcPanelLibraryTypeInModel(Materials.GlcPanelLibraryType obj)
        {
            foreach (Materials.GlcPanelLibraryType elem in this.GlcPanelTypes.GlcPanelLibraryTypes)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Section to Model.
        /// </summary>
        private void AddSection(FemDesign.Sections.Section obj, bool overwrite)
        {
            // in model?
            bool inModel = this.SectionInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Sections.Section.RemoveAll(x => x.Guid == obj.Guid);
                this.Sections.Section.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.Sections.Section.Add(obj);
            }
        }

        /// <summary>
        /// Check if Section in Model.
        /// </summary>
        private bool SectionInModel(FemDesign.Sections.Section obj)
        {
            if(this.Sections == null)
            {
                this.Sections = new FemDesign.Sections.ModelSections();
                return false;
            }

            foreach (FemDesign.Sections.Section elem in this.Sections.Section)
            {
                if (obj != null && elem != null)
                {
                    if (elem.Guid == obj.Guid)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Add LabelledSection to Model
        /// </summary>
        /// <param name="obj">Labelled section to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing labelled section with matching GUID.</param>
        public void AddLabelledSection(AuxiliaryResults.LabelledSection obj, bool overwrite)
        {
            if (this.Entities.LabelledSections == null)
            {
                this.Entities.LabelledSections = new AuxiliaryResults.LabelledSectionsGeometry();
            }

            // in model?
            bool inModel = this.LabelledSectionInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.LabelledSections.LabelledSections.RemoveAll(x => x.Guid == obj.Guid);
                this.Entities.LabelledSections.LabelledSections.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.Entities.LabelledSections.LabelledSections.Add(obj);
            }
        }

        /// <summary>
        /// Check if LabelledSection in Model
        /// </summary>
        private bool LabelledSectionInModel(AuxiliaryResults.LabelledSection obj)
        {
            foreach (AuxiliaryResults.LabelledSection elem in this.Entities.LabelledSections.LabelledSections)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Add ResultPoint to Model
        /// </summary>
        private void AddResultPoint(AuxiliaryResults.ResultPoint obj, bool overwrite)
        {
            if (this.Entities.ResultPoints == null)
            {
                this.Entities.ResultPoints = new AuxiliaryResults.ResultPointsGeometry();
            }

            // in model?
            bool inModel = this.ResultPointInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.ResultPoints.ResultPoints.RemoveAll(x => x.Guid == obj.Guid);
                this.Entities.ResultPoints.ResultPoints.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.Entities.ResultPoints.ResultPoints.Add(obj);
            }
        }

        /// <summary>
        /// Check if ResultPoint in Model
        /// </summary>
        private bool ResultPointInModel(AuxiliaryResults.ResultPoint obj)
        {
            foreach (AuxiliaryResults.ResultPoint elem in this.Entities.ResultPoints.ResultPoints)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Add VirtualBar to Model
        /// </summary>
        private void AddVirtualBar(AuxiliaryResults.VirtualBar obj, bool overwrite)
        {
            if (this.Entities.VirtualBarContainer == null)
            {
                this.Entities.VirtualBarContainer = new AuxiliaryResults.VirtualBarContainer();
            }

            // in model?
            bool inModel = this.VirtualBarInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.VirtualBarContainer.VirtualBars.RemoveAll(x => x.Guid == obj.Guid);
                this.Entities.VirtualBarContainer.VirtualBars.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.Entities.VirtualBarContainer.VirtualBars.Add(obj);
            }
        }

        /// <summary>
        /// Check if LabelledSection in Model
        /// </summary>
        private bool VirtualBarInModel(AuxiliaryResults.VirtualBar obj)
        {
            foreach (AuxiliaryResults.VirtualBar elem in this.Entities.VirtualBarContainer.VirtualBars)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Add PeakSmoothingRegion to Model
        /// </summary>
        private void AddPeakSmoothingRegion(FiniteElements.PeakSmoothingRegion obj, bool overwrite)
        {
            if (this.Entities.PeakSmoothingRegions == null)
                this.Entities.PeakSmoothingRegions = new List<FiniteElements.PeakSmoothingRegion>();

            // in model?
            bool inModel = this.PeakSmoothingRegionInModel(obj);

            // in model, don't overwrite
            if(inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            if(inModel && overwrite)
            {
                this.Entities.PeakSmoothingRegions.RemoveAll(p => p == obj.Guid);
                this.Entities.PeakSmoothingRegions.Add(obj);
            }

            // not in model
            if (!inModel)
            {
                this.Entities.PeakSmoothingRegions.Add(obj);
            }
        }

        /// <summary>
        /// Check if PeakSmoothingRegion in Model
        /// </summary>
        private bool PeakSmoothingRegionInModel(FiniteElements.PeakSmoothingRegion obj)
        {
            foreach (FiniteElements.PeakSmoothingRegion elem in this.Entities.PeakSmoothingRegions)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add a text annotation to the model geometry.
        /// </summary>
        /// <param name="obj">Text annotation to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing annotation with matching GUID.</param>
        public void AddTextAnnotation(Drawing.TextAnnotation obj, bool overwrite)
        {
            if (this.Geometry == null)
            {
                this.Geometry = new StruSoft.Interop.StruXml.Data.DatabaseGeometry();
            }

            if (this.Geometry.Text == null)
            {
                this.Geometry.Text = new List<StruSoft.Interop.StruXml.Data.Text_type>();
            }

            // add layer
            if (obj.StyleType.LayerObj != null)
            {
                this.AddLayer(obj.StyleType.LayerObj, overwrite);
            }

            // add text annotation
            bool inModel = this.Geometry.Text.Any(x => x.Guid == obj.Guid.ToString());
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Geometry.Text.RemoveAll(x => x.Guid == obj.Guid.ToString());
                this.Geometry.Text.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.Geometry.Text.Add(obj);
            }
        }

        /// <summary>
        /// Add a linear dimension to the model geometry.
        /// </summary>
        /// <param name="obj">Linear dimension to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing dimension with matching GUID.</param>
        public void AddLinearDimension(Drawing.DimensionLinear obj, bool overwrite)
        {
            if (this.Geometry == null)
            {
                this.Geometry = new StruSoft.Interop.StruXml.Data.DatabaseGeometry();
            }

            if (this.Geometry.Linear_dimension == null)
            {
                this.Geometry.Linear_dimension = new List<StruSoft.Interop.StruXml.Data.Dimline_type>();
            }

            // // add layer
            // if (obj.StyleType.LayerObj != null)
            // {
            //     this.AddLayer(obj.StyleType.LayerObj, overwrite);
            // }

            // add text annotation
            bool inModel = this.Geometry.Linear_dimension.Any(x => x.Guid == obj.Guid.ToString());
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Geometry.Linear_dimension.RemoveAll(x => x.Guid == obj.Guid.ToString());
                this.Geometry.Linear_dimension.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.Geometry.Linear_dimension.Add(obj);
            }
        }

        /// <summary>
        /// Add a layer to the model geometry.
        /// </summary>
        /// <param name="obj">Layer to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite an existing layer with the same name.</param>
        public void AddLayer(StruSoft.Interop.StruXml.Data.Layer_type obj, bool overwrite)
        {
            if (this.Geometry == null)
            {
                this.Geometry = new StruSoft.Interop.StruXml.Data.DatabaseGeometry();
            }

            if (this.Geometry.Layer == null)
            {
                this.Geometry.Layer = new List<StruSoft.Interop.StruXml.Data.Layer_type>();
            }

            bool inModel = this.Geometry.Layer.Any(x => x.Name == obj.Name);

            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Geometry.Layer.RemoveAll(x => x.Name == obj.Name);
                this.Geometry.Layer.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.Geometry.Layer.Add(obj);
            }
        }


        /// <summary>
        /// Add a 3D point to the model geometry.
        /// </summary>
        /// <param name="obj">Point to add.</param>
        public void AddPoint3d(Drawing.Point3d obj)
        {
            if (this.Geometry == null)
            {
                this.Geometry = new StruSoft.Interop.StruXml.Data.DatabaseGeometry();
            }

            if(this.Geometry.Point == null)
            {
                this.Geometry.Point = new List<StruSoft.Interop.StruXml.Data.Point_type>();
            }

            this.Geometry.Point.Add(obj);

            this.AddLayer(obj.Style.LayerObj, overwrite: false);
        }

        /// <summary>
        /// Add a 3D curve to the model geometry.
        /// </summary>
        /// <param name="obj">Curve to add.</param>
        public void AddCurve3d(Drawing.Curve obj)
        {
            if (this.Geometry == null)
            {
                this.Geometry = new StruSoft.Interop.StruXml.Data.DatabaseGeometry();
            }

            if(this.Geometry.Curve == null)
            {
                this.Geometry.Curve = new List<StruSoft.Interop.StruXml.Data.Curve_type>();
            }

            this.Geometry.Curve.Add(obj);

            // is layer in model?
            this.AddLayer(obj.Style.LayerObj, overwrite: false);
        }


        /// <summary>
        /// Replace construction stages in the model based on a list of stages.
        /// </summary>
        /// <param name="stages">Stages to set on the model.</param>
        /// <param name="assignModifedElement">If <c>true</c>, treat elements as modified when constructing stages.</param>
        /// <param name="assignNewElement">If <c>true</c>, treat elements as new when constructing stages.</param>
        public void SetConstructionStages(List<Stage> stages, bool assignModifedElement = false, bool assignNewElement = false)
        {
            var obj = new ConstructionStages(stages, assignModifedElement, assignNewElement);
            SetConstructionStages(obj);
        }


        /// <summary>
        /// Set the model construction stages and assign stage IDs to elements in each stage.
        /// </summary>
        /// <param name="obj">Construction stages to set.</param>
        private void SetConstructionStages(ConstructionStages obj)
        {
            if (this.ConstructionStages == null)
            {
                this.ConstructionStages = new ConstructionStages();
            }
            this.ConstructionStages = obj;

            if (obj.Stages.Count == 1)
            {
                throw new Exception("Number of Stages must be greater than 1");
            }

            foreach (var stage in obj.Stages)
            {
                if (stage.Elements != null)
                {
                    foreach (var element in stage.Elements)
                    {
                        //var newElement = element.DeepClone();
                        element.StageId = stage.Id;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Add structural elements to Model. 
        /// </summary>
        /// <typeparam name="T">Structural elements (IStructureElement).</typeparam>
        /// <param name="elements">Structural elements to be added.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing objects with matching GUID (where supported).</param>
        /// <returns>The current model instance.</returns>
        public Model AddElements<T>(IEnumerable<T> elements, bool overwrite = true) where T : IStructureElement
        {
            // check if model contains entities, sections and materials
            if (this.Entities == null)
                this.Entities = new Entities();

            foreach (var item in elements)
            {
                try
                {
                    AddEntity(item as dynamic, overwrite);
                }
                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException exeption)
                {
                    if (item == null)
                        throw new ArgumentNullException("Can not add null element to model.", exeption);
                    throw new System.NotImplementedException($"Class Model don't have a method AddEntity that accepts {item.GetType()}. ", exeption);
                }
            }

            return this;
        }

        /// <inheritdoc cref="AddElements{T}(IEnumerable{T}, bool)"/>
        public Model AddElements<T>(params T[] elements) where T : IStructureElement
        {
            return AddElements(elements, overwrite: true);
        }


        /// <summary>
        /// Add Soil to the model.
        /// </summary>
        /// <param name="element">Soil elements to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing objects with matching GUID (where supported).</param>
        /// <returns>The current model instance.</returns>
        public Model AddSoilElement(Soil.SoilElements element, bool overwrite = true)
        {
            // check if model contains entities
            if (this.Entities == null)
                this.Entities = new Entities();

            AddEntity(element as dynamic, overwrite);
            return this;
        }

        /// <summary>
        /// Adds loads to the model.
        /// </summary>
        /// <typeparam name="T">ILoadElement is any load object in FEM-Design.</typeparam>
        /// <param name="elements">Load elements to be added.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing objects with matching GUID (where supported).</param>
        /// <returns>The current model instance.</returns>
        public Model AddLoads<T>(IEnumerable<T> elements, bool overwrite = true) where T : ILoadElement
        {
            // check if model contains entities
            if (this.Entities == null)
                this.Entities = new Entities();

            foreach (var item in elements)
            {
                try
                {
                    AddEntity(item as dynamic, overwrite);
                }
                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException exeption)
                {
                    if (item == null)
                        throw new ArgumentNullException("Can not add null load to model.", exeption);
                    throw new System.NotImplementedException($"Class Model don't have a method AddEntity that accepts {item.GetType()}. ", exeption);
                }
            }

            return this;
        }

        /// <inheritdoc cref="AddLoads{T}(IEnumerable{T}, bool)"/>
        public Model AddLoads<T>(params T[] loads) where T : ILoadElement
        {
            return AddLoads(loads, overwrite: true);
        }

        /// <summary>
        /// Add supports to the model.
        /// </summary>
        /// <typeparam name="T">ISuppotElement is any support object.</typeparam>
        /// <param name="elements">Support elements to be added.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing objects with matching GUID (where supported).</param>
        /// <returns>The current model instance.</returns>
        public Model AddSupports<T>(IEnumerable<T> elements, bool overwrite = true) where T : ISupportElement
        {
            // check if model contains entities, sections and materials
            if (this.Entities == null)
            {
                this.Entities = new Entities();
            }

            foreach (var item in elements)
            {
                try
                {
                    AddEntity(item as dynamic, overwrite);
                }
                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException exeption)
                {
                    if (item == null)
                        throw new ArgumentNullException("Can not add null support to model.", exeption);
                    throw new System.NotImplementedException($"Class Model don't have a method AddEntity that accepts {item.GetType()}. ", exeption);
                }
            }

            return this;
        }

        /// <inheritdoc cref="AddSupports{T}(IEnumerable{T}, bool)"/>
        public Model AddSupports<T>(params T[] supports) where T : ISupportElement
        {
            return AddSupports(supports, overwrite: true);
        }


        public Model AddDrawings<T>(IEnumerable<T> elements, bool overwrite) where T : IDrawing
        {
            // check if model contains entities, sections and materials
            if (this.Geometry == null)
            {
                this.Geometry = new StruSoft.Interop.StruXml.Data.DatabaseGeometry();
            }

            foreach (var item in elements)
            {
                try
                {
                    AddEntity(item as dynamic, overwrite);
                }
                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException exeption)
                {
                    if (item == null)
                        throw new ArgumentNullException("Can not add null geometry to model.", exeption);
                    throw new System.NotImplementedException($"Class Model don't have a method AddEntity that accepts {item.GetType()}. ", exeption);
                }
            }

            return this;
        }


        /// <summary>
        /// Dispatch helper used by <see cref="AddElements{T}(System.Collections.Generic.IEnumerable{T}, bool)"/> and related methods.
        /// Routes to a type-specific <c>Add*</c> method.
        /// </summary>
        /// <param name="obj">Object to add.</param>
        /// <param name="overwrite">If <c>true</c>, overwrite existing objects with matching identity (where supported).</param>
        private void AddEntity(Bars.Bar obj, bool overwrite) => AddBar(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Shells.Slab obj, bool overwrite) => AddSlab(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Shells.Panel obj, bool overwrite) => AddPanel(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Reinforcement.Ptc obj, bool overwrite) => AddPtc(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Reinforcement.ConcealedBar obj, bool overwrite) => AddConcealedBar(obj, overwrite);


        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(ModellingTools.Cover obj, bool overwrite) => AddCover(obj, overwrite);

        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(ModellingTools.FictitiousShell obj, bool overwrite) => AddFictShell(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(ModellingTools.FictitiousBar obj, bool overwrite) => AddFictBar(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(ModellingTools.ConnectedPoints obj, bool overwrite) => AddConnectedPoints(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(ModellingTools.ConnectedLines obj, bool overwrite) => AddConnectedLine(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(ModellingTools.SurfaceConnection obj, bool overwrite) => AddSurfaceConnection(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(ModellingTools.Diaphragm obj, bool overwrite) => AddDiaphragm(obj, overwrite);

        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(AuxiliaryResults.LabelledSection obj, bool overwrite) => AddLabelledSection(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(AuxiliaryResults.ResultPoint obj, bool overwrite) => AddResultPoint(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(AuxiliaryResults.VirtualBar obj, bool overwrite) => AddVirtualBar(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(FiniteElements.PeakSmoothingRegion obj, bool overwrite) => AddPeakSmoothingRegion(obj, overwrite);

        #region FOUNDATIONS
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Foundations.IsolatedFoundation obj, bool overwrite) => AddIsolatedFoundation(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Foundations.SlabFoundation obj, bool overwrite) => AddSlabFoundation(obj, overwrite);

        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Soil.SoilElements obj, bool overwrite) => AddSoil(obj, overwrite);

        #endregion

        #region SUPPORTS
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Supports.PointSupport obj, bool overwrite) => AddPointSupport(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Supports.LineSupport obj, bool overwrite) => AddLineSupport(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Supports.SurfaceSupport obj, bool overwrite) => AddSurfaceSupport(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Supports.StiffnessPoint obj, bool overwrite) => AddStiffnessPoint(obj, overwrite);
        #endregion

        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(StructureGrid.Axis axis, bool overwrite) => AddAxis(axis, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(StructureGrid.Storey storey, bool overwrite) => AddStorey(storey, overwrite);

        #region LOADS
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.PointLoad obj, bool overwrite) => AddPointLoad(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.PointSupportMotion obj, bool overwrite) => AddPointSupportMotion(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.SurfaceTemperatureLoad obj, bool overwrite) => AddSurfaceTemperatureLoad(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.SurfaceLoad obj, bool overwrite) => AddSurfaceLoad(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.SurfaceSupportMotion obj, bool overwrite) => AddSurfaceSupportMotionLoad(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.PressureLoad obj, bool overwrite) => AddPressureLoad(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.LineTemperatureLoad obj, bool overwrite) => AddLineTemperatureLoad(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.LineStressLoad obj, bool overwrite) => AddLineStressLoad(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.LineLoad obj, bool overwrite) => AddLineLoad(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.LineSupportMotion obj, bool overwrite) => AddLineSupportMotionLoad(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.Footfall obj, bool overwrite) => AddFootfall(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.MassConversionTable obj, bool overwrite) => AddMassConversionTable(obj);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.MovingLoad obj, bool overwrite) => AddMovingLoad(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.Mass obj, bool overwrite) => AddMass(obj, overwrite);

        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.ExcitationForce obj, bool overwrite) => AddExcitationForce(obj, overwrite);

        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.PeriodicExcitation obj, bool overwrite) => AddPeriodicExcitationForce(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.PeriodicLoad obj, bool overwrite) => AddPeriodicExcitationForceRecords(obj, overwrite);

        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.LoadCase obj, bool overwrite) => AddLoadCase(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Loads.LoadCombination obj, bool overwrite) => AddLoadCombination(obj, overwrite);

        #endregion

        #region geometry
        // geometry (drawing) objects are actually not entities but will be put here for now. (:
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Drawing.TextAnnotation obj, bool overwrite) => AddTextAnnotation(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        private void AddEntity(Drawing.DimensionLinear obj, bool overwrite) => AddLinearDimension(obj, overwrite);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        /// <param name="obj">the obj.</param>
        /// <param name="overwrite)">the overwrite).</param>
        public void AddEntity(Drawing.Point3d obj, bool overwrite) => AddPoint3d(obj);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        /// <param name="obj">the obj.</param>
        /// <param name="overwrite)">the overwrite).</param>
        public void AddEntity(Drawing.Curve obj, bool overwrite) => AddCurve3d(obj);
        /// <inheritdoc cref="AddEntity(Bars.Bar, bool)"/>
        /// <param name="obj">the obj.</param>
        /// <param name="overwrite)">the overwrite).</param>
        public void AddEntity(StruSoft.Interop.StruXml.Data.Layer_type obj, bool overwrite) => AddLayer(obj, overwrite);
        #endregion

        #region deconstruct
        /// <summary>
        /// Get Bars from Model. 
        /// Bars will be reconstructed from Model incorporating all references: ComplexSection, Section, Material.
        /// </summary>
        internal void GetBars()
        {
            Dictionary<Guid, Sections.ComplexSection> complexSectionsMap = this.Sections.ComplexSection.ToDictionary(s => s.Guid, s => s.DeepClone());
            Dictionary<Guid, Sections.Section> sectionsMap = this.Sections.Section.ToDictionary(s => s.Guid, s => s.DeepClone());
            Dictionary<Guid, Materials.Material> materialMap = this.Materials.Material.ToDictionary(d => d.Guid, d => d.DeepClone());
            Dictionary<Guid, Composites.ComplexComposite> complexCompositeMap = new Dictionary<Guid, Composites.ComplexComposite>();
            Dictionary<Guid, Composites.CompositeSection> compositeSectionMap = new Dictionary<Guid, Composites.CompositeSection>();

            if (this.Composites != null)
            {
                // assign the material and section objects to the CompositeSectionPart
                foreach (var compositeSection in this.Composites.CompositeSection)
                {
                    foreach (var part in compositeSection.Parts)
                    {
                        part.Material = materialMap[part.MaterialRef];
                        part.Section = sectionsMap[part.SectionRef];
                    }
                }

                compositeSectionMap = this.Composites.CompositeSection.ToDictionary(s => s.Guid, s => s.DeepClone());
                complexCompositeMap = this.Composites.ComplexComposite.ToDictionary(s => s.Guid, s => s.DeepClone());

                // assign the CompositeSection object to the ComplexCompositePart
                foreach (var complexComposite in this.Composites.ComplexComposite)
                {
                    foreach (var part in complexComposite.Parts)
                    {
                        part.CompositeSectionObj = compositeSectionMap[part.CompositeSectionRef];
                    }
                }
            }

            foreach (Bars.Bar item in this.Entities.Bars)
            {
                // set type on barPart
                item.BarPart.Type = item.Type;

                // get section and material for truss
                if (item.BarPart.SectionType == Bars.SectionType.Truss)
                {
                    // section
                    try
                    {
                        item.BarPart.TrussUniformSectionObj = sectionsMap[new System.Guid(item.BarPart.ComplexSectionRef)];
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ArgumentException("No matching section found. Model.GetBars() failed.");
                    }
                    catch (ArgumentNullException)
                    {
                        throw new ArgumentNullException($"BarPart {item.BarPart.Name} BarPart.ComplexSectionRef is null");
                    }

                    // material
                    try
                    {
                        item.BarPart.ComplexMaterialObj = materialMap[item.BarPart.ComplexMaterialRef];
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ArgumentException("No matching material found. Model.GetBars() failed.");
                    }
                    catch (ArgumentNullException)
                    {
                        throw new ArgumentNullException($"BarPart {item.BarPart.Name} BarPart.ComplexMaterialRef is null");
                    }
                }
                // do nothing for beam or column with complex section (delta beam type)
                else if (item.BarPart.SectionType == Bars.SectionType.DeltaBeamColumn)
                {
                    // pass
                }
                // get section and material for beam or column with complex section (not delta beam type)
                else if (item.BarPart.SectionType == Bars.SectionType.RegularBeamColumn)
                {
                    // section
                    try
                    {
                        // complex section
                        item.BarPart.ComplexSectionObj = complexSectionsMap[new System.Guid(item.BarPart.ComplexSectionRef)];

                        // sections
                        try
                        {
                            foreach (Sections.ComplexSectionPart part in item.BarPart.ComplexSectionObj.Parts)
                            {
                                part.SectionObj = sectionsMap[part.SectionRef];
                            }
                        }
                        catch (KeyNotFoundException)
                        {
                            throw new ArgumentException("No matching section found. Model.GetBars() failed.");
                        }
                    }
                    catch (ArgumentNullException)
                    {
                        throw new ArgumentNullException($"BarPart {item.BarPart.Name} BarPart.ComplexSectionRef is null");
                    }

                    // material
                    try
                    {
                        item.BarPart.ComplexMaterialObj = materialMap[item.BarPart.ComplexMaterialRef];
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ArgumentException("No matching complex material found. Model.GetBars() failed.");
                    }
                }

                // get section and material for beam or column with complex composite section
                else if (item.BarPart.SectionType == Bars.SectionType.CompositeBeamColumn)
                {
                    try
                    {
                        // assign the ComplexComposite Object to the bar part
                        item.BarPart.ComplexCompositeObj = complexCompositeMap[item.BarPart.ComplexCompositeRef];

                        // iterate over the CompositeSection inside the ComplexComposite and assign the object from the Composites
                        foreach (Composites.ComplexCompositePart complexCompositePart in item.BarPart.ComplexCompositeObj.Parts)
                        {
                            complexCompositePart.CompositeSectionObj = compositeSectionMap[complexCompositePart.CompositeSectionRef];
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ArgumentException("No matching complex composite or composite section.");
                    }
                }
                else
                {
                    throw new System.ArgumentException("Type of bar is not supported.");
                }

                // get bar reinforcement
                foreach (Reinforcement.BarReinforcement barReinf in this.Entities.BarReinforcements)
                {
                    if (barReinf.BaseBar.Guid == item.BarPart.Guid)
                    {
                        // get wire material
                        foreach (Materials.Material material in this.ReinforcingMaterials.Material)
                        {
                            if (barReinf.Wire.ReinforcingMaterialGuid == material.Guid)
                            {
                                barReinf.Wire.ReinforcingMaterial = material;
                            }
                        }

                        // check if material found
                        if (barReinf.Wire.ReinforcingMaterial == null)
                        {
                            throw new System.ArgumentException("No matching reinforcement wire material found. Model.GetBars() failed.");
                        }
                        else
                        {
                            // add bar reinforcement to bar
                            item.Reinforcement.Add(barReinf);
                        }

                    }
                }

                // get ptc
                foreach (Reinforcement.Ptc ptc in this.Entities.PostTensionedCables)
                {
                    if (ptc.BaseObject == item.BarPart.Guid)
                    {
                        // get strand material
                        foreach (Reinforcement.PtcStrandLibType material in this.PtcStrandTypes.PtcStrandLibTypes)
                        {
                            if (ptc.StrandTypeGuid == material.Guid)
                            {
                                ptc.StrandType = material;
                            }
                        }

                        // check if material found
                        if (ptc.StrandType == null)
                        {
                            throw new System.ArgumentException("No matching ptc strand found. Model.GetBars() failed.");
                        }
                        else
                        {
                            // add ptc to bar
                            item.Ptc.Add(ptc);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get FictitiousShells from Model.
        /// FicititiousShells will be reconstruted from Model incorporating predefined EdgeConnections
        /// </summary>
        internal void GetFictitiousShells()
        {
            foreach (ModellingTools.FictitiousShell item in this.Entities.AdvancedFem.FictitiousShells)
            {
                // set line_connection_types (i.e predefined edge connections) on edge
                if (this.LineConnectionTypes != null)
                {
                    if (this.LineConnectionTypes.PredefinedTypes != null)
                    {
                        item.Region.SetPredefinedRigidities(this.LineConnectionTypes.PredefinedTypes);
                    }
                }
            }
        }

        /// <summary>
        /// Get Slabs from Model.
        /// Slabs will be reconstruted from Model incorporating all references: Material, Predefined EdgeConnections, SurfaceReinforcementParameters, SurfaceReinforcement.
        /// </summary>
        internal void GetSlabs()
        {
            foreach (Shells.Slab item in this.Entities.Slabs)
            {
                // get material
                foreach (Materials.Material _material in this.Materials.Material)
                {
                    if (_material.Guid == item.SlabPart.ComplexMaterialGuid)
                    {
                        item.Material = _material;
                    }
                }

                // set line_connection_types (i.e predefined edge connections) on edge
                if (this.LineConnectionTypes != null)
                {
                    if (this.LineConnectionTypes.PredefinedTypes != null)
                    {
                        item.SlabPart.Region.SetPredefinedRigidities(this.LineConnectionTypes.PredefinedTypes);
                    }
                }

                // get surface reinforcement parameters
                foreach (Reinforcement.SurfaceReinforcementParameters surfaceReinforcementParameter in this.Entities.SurfaceReinforcementParameters)
                {
                    if (surfaceReinforcementParameter.BaseShell.Guid == item.SlabPart.Guid)
                    {
                        item.SurfaceReinforcementParameters = surfaceReinforcementParameter;
                    }
                }

                // get surface reinforcement
                foreach (Reinforcement.SurfaceReinforcement surfaceReinforcement in this.Entities.SurfaceReinforcements)
                {
                    if (surfaceReinforcement.BaseShell.Guid == item.SlabPart.Guid)
                    {

                        // get wire material
                        foreach (Materials.Material material in this.ReinforcingMaterials.Material)
                        {
                            if (surfaceReinforcement.Wire.ReinforcingMaterialGuid == material.Guid)
                            {
                                surfaceReinforcement.Wire.ReinforcingMaterial = material;
                            }
                        }

                        // add surface reinforcement to slab
                        item.SurfaceReinforcement.Add(surfaceReinforcement);
                    }
                }

                // check if material found
                if (item.Material == null)
                {
                    throw new System.ArgumentException("No matching material found. Model.GeSlabs() failed.");
                }
            }
        }

        /// <summary>
        /// Get panels from the model and resolve references (material, section, panel types, predefined rigidities).
        /// </summary>
        internal void GetPanels()
        {
            foreach (Shells.Panel panel in this.Entities.Panels)
            {
                // get material
                if (this.Materials != null) // model with only timber plate does not have an xml element 'materials'
                {
                    foreach (Materials.Material material in this.Materials.Material)
                    {
                        if (material.Guid == panel.ComplexMaterialRef)
                        {
                            panel.Material = material;
                        }
                    }
                }

                // get section
                if (this.Sections != null) // model with only timber plate does not have an xml element 'sections'
                {
                    foreach (Sections.Section section in this.Sections.Section)
                    {
                        if (section.Guid == panel.ComplexSection)
                        {
                            panel.Section = section;
                        }
                    }
                }

                // get timber application data
                if (panel.TimberPanelData != null)
                {
                    // timber panel types
                    if (this.OrthotropicPanelTypes != null && this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes != null)
                    {
                        foreach (FemDesign.Materials.OrthotropicPanelLibraryType libItem in this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes)
                        {
                            if (libItem.Guid == panel.TimberPanelData._panelTypeReference)
                            {
                                panel.TimberPanelData.PanelType = libItem;
                            }
                        }
                    }

                    // clt panel types
                    if (this.CltPanelTypes != null && this.CltPanelTypes.CltPanelLibraryTypes != null)
                    {
                        foreach (FemDesign.Materials.CltPanelLibraryType libItem in this.CltPanelTypes.CltPanelLibraryTypes)
                        {
                            if (libItem.Guid == panel.TimberPanelData._panelTypeReference)
                            {
                                panel.TimberPanelData.PanelType = libItem;
                            }
                        }
                    }

                    // glc panel types
                    if (this.GlcPanelTypes != null && this.GlcPanelTypes.GlcPanelLibraryTypes != null)
                    {
                        foreach (FemDesign.Materials.GlcPanelLibraryType libItem in this.GlcPanelTypes.GlcPanelLibraryTypes)
                        {
                            if (libItem.Guid == panel.TimberPanelData._panelTypeReference)
                            {
                                panel.TimberPanelData.PanelType = libItem;
                            }
                        }
                    }

                    // check if libItem found
                    if (panel.TimberPanelData.PanelType == null)
                    {
                        throw new System.ArgumentException("An orthotropic/clt/glc library item was expected but not found. Can't construct Panel. Model.GetPanels() failed.");
                    }
                }

                // predefined rigidity
                if (this.LineConnectionTypes != null)
                {
                    if (this.LineConnectionTypes.PredefinedTypes != null)
                    {
                        panel.Region.SetPredefinedRigidities(this.LineConnectionTypes.PredefinedTypes);
                        foreach (InternalPanel internalPanel in panel.InternalPanels.IntPanels)
                        {
                            internalPanel.Region.SetPredefinedRigidities(this.LineConnectionTypes.PredefinedTypes);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resolve point support references (e.g. predefined rigidity) after deserialization.
        /// </summary>
        internal void GetPointSupports()
        {
            foreach (Supports.PointSupport pointSupport in this.Entities.Supports.PointSupport)
            {

                if(pointSupport.IsDirected)
                {
                    continue;
                }
                // predefined rigidity
                if (this.PointSupportGroupTypes != null && this.PointSupportGroupTypes.PredefinedTypes != null)
                {
                    foreach (Releases.RigidityDataLibType2 predefinedType in this.PointSupportGroupTypes.PredefinedTypes)
                    {
                        if (pointSupport.Group._predefRigidityRef != null && predefinedType.Guid == pointSupport.Group._predefRigidityRef.Guid)
                        {
                            pointSupport.Group.PredefRigidity = predefinedType;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resolve line support references (e.g. predefined rigidity) after deserialization.
        /// </summary>
        internal void GetLineSupports()
        {
            foreach (Supports.LineSupport lineSupport in this.Entities.Supports.LineSupport)
            {
                if(lineSupport.IsDirected)
                {
                    continue;
                }
                // predefined rigidity
                if (this.LineSupportGroupTypes != null && this.LineSupportGroupTypes.PredefinedTypes != null)
                {
                    foreach (Releases.RigidityDataLibType2 predefinedType in this.LineSupportGroupTypes.PredefinedTypes)
                    {
                        if (lineSupport.Group._predefRigidityRef != null && predefinedType.Guid == lineSupport.Group._predefRigidityRef.Guid)
                        {
                            lineSupport.Group.PredefRigidity = predefinedType;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resolve surface support references (e.g. predefined rigidity) after deserialization.
        /// </summary>
        internal void GetSurfaceSupports()
        {
            foreach (Supports.SurfaceSupport surfaceSupport in this.Entities.Supports.SurfaceSupport)
            {
                // predefined rigidity
                if (this.SurfaceSupportTypes != null && this.SurfaceSupportTypes.PredefinedTypes != null)
                {
                    foreach (Releases.RigidityDataLibType1 predefinedType in this.SurfaceSupportTypes.PredefinedTypes)
                    {
                        if (surfaceSupport._predefRigidityRef != null && predefinedType.Guid == surfaceSupport._predefRigidityRef.Guid)
                        {
                            surfaceSupport.PredefRigidity = predefinedType;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resolve point connection references (e.g. predefined rigidity) after deserialization.
        /// </summary>
        internal void GetPointConnections()
        {
            foreach (ModellingTools.ConnectedPoints connectedPoint in this.Entities.AdvancedFem.ConnectedPoints)
            {
                // predefined rigidity
                if (this.PointConnectionTypes != null && this.PointConnectionTypes.PredefinedTypes != null)
                {
                    foreach (Releases.RigidityDataLibType2 predefinedType in this.PointConnectionTypes.PredefinedTypes)
                    {
                        if (connectedPoint._predefRigidityRef != null && predefinedType.Guid == connectedPoint._predefRigidityRef.Guid)
                        {
                            connectedPoint.PredefRigidity = predefinedType;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resolve line connection references (e.g. predefined rigidity) after deserialization.
        /// </summary>
        internal void GetLineConnections()
        {
            foreach (ModellingTools.ConnectedLines connectedLine in this.Entities.AdvancedFem.ConnectedLines)
            {
                // predefined rigidity
                if (this.LineConnectionTypes != null && this.LineConnectionTypes.PredefinedTypes != null)
                {
                    foreach (Releases.RigidityDataLibType3 predefinedType in this.LineConnectionTypes.PredefinedTypes)
                    {
                        if (connectedLine._predefRigidityRef != null && predefinedType.Guid == connectedLine._predefRigidityRef.Guid)
                        {
                            connectedLine.PredefRigidity = predefinedType;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resolve surface connection references (e.g. predefined rigidity) after deserialization.
        /// </summary>
        internal void GetSurfaceConnections()
        {
            foreach (ModellingTools.SurfaceConnection connectedSurf in this.Entities.AdvancedFem.SurfaceConnections)
            {
                // predefined rigidity
                if (this.SurfaceConnectionTypes != null && this.SurfaceConnectionTypes.PredefinedTypes != null)
                {
                    foreach (Releases.RigidityDataLibType1 predefinedType in this.SurfaceConnectionTypes.PredefinedTypes)
                    {
                        if (connectedSurf._predefRigidityRef != null && predefinedType.Guid == connectedSurf._predefRigidityRef.Guid)
                        {
                            connectedSurf.PredefRigidity = predefinedType;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reconstruct construction stages by grouping elements by stage ID and resolving activated load case display names.
        /// </summary>
        internal void GetConstructionStages()
        {
            var loadCaseNames = this.Entities.Loads.LoadCases?.ToDictionary(lc => lc.Guid, lc => lc.Name);

            List<IStageElement> stageElements = new List<IStageElement>();
            stageElements.AddRange(this.Entities.Bars);
            stageElements.AddRange(this.Entities.Panels);
            stageElements.AddRange(this.Entities.Slabs);
            stageElements.AddRange(this.Entities.Supports.PointSupport);
            stageElements.AddRange(this.Entities.Supports.LineSupport);
            stageElements.AddRange(this.Entities.Supports.SurfaceSupport);
            stageElements.AddRange(this.Entities.AdvancedFem.Diaphragms);

            var elementsPerStage = stageElements.GroupBy(s => s.StageId).ToDictionary(g => g.Key, g => g.ToList());

            int i = 0;
            foreach (var stage in this.ConstructionStages.Stages)
            {
                i++; // Starts at 1
                stage.Id = i;

                if (elementsPerStage.ContainsKey(i))
                    stage.Elements = elementsPerStage[i];

                if (stage.ActivatedLoadCases != null)
                {
                    foreach (var activatedLoadCase in stage.ActivatedLoadCases)
                    {
                        if (activatedLoadCase.IsLoadCase)
                            activatedLoadCase.LoadCaseDisplayName = loadCaseNames[activatedLoadCase.LoadCaseGuid];
                        else if (activatedLoadCase.IsPTCLoadCase)
                            activatedLoadCase.LoadCaseDisplayName = "PTC " + activatedLoadCase.PTCLoadCase.ToString().ToUpper();
                        else if (activatedLoadCase.IsMovingLoad)
                            // TODO: Use the moving load name
                            activatedLoadCase.LoadCaseDisplayName = "<MovingLoad>";
                    }
                }
            }
        }

        /// <summary>
        /// Resolve load combination references to load cases and construction stages after deserialization.
        /// </summary>
        internal void GetLoadCombinations()
        {
            if (!this.Entities.Loads.LoadCases.Any())
            {
                return;
                throw new Exception("Model does not contain any load cases. Load Combinations can not be created!");
            }
            var loadCasesMap = this.Entities.Loads.LoadCases?.ToDictionary(lc => lc.Guid);
            var stageMap = this.ConstructionStages?.Stages?.ToDictionary(s => s.Id);

            foreach (var lComb in this.Entities.Loads.LoadCombinations)
            {
                foreach (Loads.ModelLoadCase mLoadCase in lComb.ModelLoadCase)
                {
                    if (mLoadCase.IsMovingLoadLoadCase)
                        continue;

                    mLoadCase.LoadCase = loadCasesMap[mLoadCase.Guid].DeepClone();
                }

                if (lComb.StageLoadCase != null && lComb.StageLoadCase.IsFinalStage == false)
                {
                    lComb.StageLoadCase.Stage = stageMap[lComb.StageLoadCase.StageIndex].DeepClone();
                }
            }
        }

        /// <summary>
        /// Resolve load group references to load cases after deserialization.
        /// </summary>
        internal void GetLoadGroups()
        {
            var loadCasesMap = this.Entities.Loads.LoadCases?.ToDictionary(lc => lc.Guid);

            foreach (var group in this.Entities.Loads.LoadGroupTable.GeneralLoadGroups)
            {

                if(group.ModelLoadGroupPermanent != null)
                {
                    foreach (var loadCaseGuid in group.ModelLoadGroupPermanent?.ModelLoadCase.Select(x => x.Guid))
                    {
                        group.ModelLoadGroupPermanent.LoadCase.Add( loadCasesMap[loadCaseGuid].DeepClone() );
                    }
                }

                if (group.ModelLoadGroupTemporary != null)
                {
                    foreach (var loadCaseGuid in group.ModelLoadGroupTemporary?.ModelLoadCase.Select(x => x.Guid))
                    {
                        if (group.ModelLoadGroupTemporary.Relationship == Loads.ELoadGroupRelationship.Custom)
                        {
                            // pass
                        }
                        group.ModelLoadGroupTemporary.LoadCase.Add(loadCasesMap[loadCaseGuid].DeepClone());
                    }
                }
            }
        }


        /// <summary>
        /// Resolve load references to their corresponding load case objects after deserialization.
        /// </summary>
        internal void GetLoads()
        {
            var loads = this.Entities.Loads.GetLoads();

            var mapCase = this.Entities.Loads.LoadCases?.ToDictionary(x => x.Guid);

            foreach (var load in loads.OfType<LoadBase>())
            {
                load.LoadCase = mapCase[load.LoadCaseGuid].DeepClone();
            }
        }

        /// <summary>
        /// Resolve model geometry references after deserialization.
        /// </summary>
        internal void GetGeometries()
        {
            var geometries = this.Geometry;

            foreach(var curve in geometries.Curve)
            {
                var layerName = curve.Style.Layer;
                curve.Style.LayerObj = this.Geometry.Layer.Where(x =>x.Name == layerName).First();
            }

            foreach (var point in geometries.Point)
            {
                var layerName = point.Style.Layer;
                point.Style.LayerObj = this.Geometry.Layer.Where(x => x.Name == layerName).First();
            }

        }

        #endregion
    }
}