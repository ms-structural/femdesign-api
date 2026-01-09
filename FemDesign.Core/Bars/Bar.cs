// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using StruSoft.Interop.StruXml.Data;

namespace FemDesign.Bars
{
    /// <summary>
    /// Represents a Bar.
    /// 
    /// Bar-element
    /// </summary>
    [XmlInclude(typeof(Beam))]
    [XmlInclude(typeof(Column))]
    [XmlInclude(typeof(Truss))]
    [XmlRoot("database", Namespace = "urn:strusoft")]
    [System.Serializable]
    public partial class Bar : NamedEntityBase, INamedEntity, IStructureElement, IStageElement, IBar
    {
        [XmlIgnore]
        private static int _barInstance = 0; // Number of bar/beam instances created
        [XmlIgnore]
        private static int _columnInstance = 0; // Number of column instances created
        [XmlIgnore]
        private static int _trussInstance = 0; // Number of truss instances created
        protected override int GetUniqueInstanceCount() // This method body must be the same as the method of the BarPart class
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
        /// Truss only.
        /// </summary>enum
        [XmlAttribute("maxforce")]
        public double _maxCompression; // non_neg_max_1e30
        [XmlIgnore]
        public double MaxCompression
        {
            get { return this._maxCompression; }
            set { this._maxCompression = RestrictedDouble.NonNegMax_1e30(value); }
        }

        /// <summary>
        /// Truss only.
        /// </summary>
        [XmlAttribute("compressions_plasticity")]
        public bool CompressionPlasticity { get; set; } // bool

        /// <summary>
        /// Truss only.
        /// </summary>
        [XmlAttribute("tension")]
        public double _maxTension; // non_neg_max_1e30

        [XmlIgnore]
        public double MaxTension
        {
            get { return this._maxTension; }
            set { this._maxTension = RestrictedDouble.NonNegMax_1e30(value); }
        }

        /// <summary>
        /// Truss only.
        /// </summary>
        [XmlAttribute("tensions_plasticity")]
        public bool TensionPlasticity { get; set; } // bool

        [XmlIgnore]
        public Geometry.Edge Edge
        {
            get
            {
                return this.BarPart.Edge;
            }
        }

        /// <summary>
        /// Gets or sets the shell model.
        /// </summary>
        [XmlAttribute("shell_model")]
        public ShellModelType ShellModel = ShellModelType.None;

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute("type")]
        public BarType Type { get; set; }

        /// <summary>
        /// Gets or sets the stage id.
        /// </summary>
        [XmlAttribute("stage")]
        public int _stageId = 1;

        [XmlIgnore]
        public int StageId
        {
            get { return _stageId; }
            set
            {
                _stageId = value;
                if (this.BarPart != null)
                    this.BarPart.StageId = value;
            }
        }

        /// <summary>
        /// Gets or sets the bar part.
        /// </summary>
        [XmlElement("bar_part", Order = 1)]
        public BarPart _barPart; // bar_part_type

        [XmlIgnore]
        public BarPart BarPart
        { 
            get
            {
                this._barPart._name = this._name + ".1";    // BarPart name must be updated with the Bar's name.
                return this._barPart;
            }
            set
            {
                this._barPart = value;
                this._name = this._barPart._name.Remove(this._barPart._name.Length - 2);    // The BarPart class is public, so the user can create a new BarPart obj. and assign it to this property. Bar name must be updated with the BarPart's name.
            }
        }

        /// <summary>
        /// Gets or sets the truss behaviour.
        /// </summary>
        [XmlElement("truss_behaviour", Order = 2)]
        public StruSoft.Interop.StruXml.Data.Truss_chr_type TrussBehaviour { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        [XmlElement("end", Order = 3)]
        public string End = "";

        /// <summary>
        /// Gets or sets the ptc.
        /// </summary>
        [XmlIgnore]
        public List<Reinforcement.Ptc> Ptc = new List<Reinforcement.Ptc>();

        /// <summary>
        /// Gets or sets the reinforcement.
        /// </summary>
        [XmlIgnore]
        public List<Reinforcement.BarReinforcement> Reinforcement = new List<Reinforcement.BarReinforcement>();

        [XmlIgnore]
        public List<Reinforcement.BarReinforcement> Stirrups
        {
            get
            {
                return this.Reinforcement.Where(x => x.Stirrups != null).ToList();
            }
        }
        [XmlIgnore]
        public List<Reinforcement.BarReinforcement> LongitudinalBars
        {
            get
            {
                return this.Reinforcement.Where(x => x.LongitudinalBar != null).ToList();
            }
        }


        /// <summary>
        /// Determines whether steel.
        /// </summary>
        /// <returns>The result.</returns>
        public bool IsSteel()
        {
            return this.BarPart.ComplexMaterialObj.Family == Materials.Family.Steel;

        }

        /// <summary>
        /// Determines whether concrete.
        /// </summary>
        /// <returns>The result.</returns>
        public bool IsConcrete()
        {
            return this.BarPart.ComplexMaterialObj.Family == Materials.Family.Concrete;

        }

        /// <summary>
        /// Determines whether timber.
        /// </summary>
        /// <returns>The result.</returns>
        public bool IsTimber()
        {
            return this.BarPart.ComplexMaterialObj.Family == Materials.Family.Timber;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        protected Bar()
        {

        }

        /// <summary>
        /// Construct beam or column with uniform section and uniform start/end conditions
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="section">Section, same at start/end.</param>
        /// <param name="eccentricity">Analytical eccentricity, same at start and end. If null, eccentricity set to 0.</param>
        /// <param name="connectivity">Connectivity, same at start/end. Connectivity set to Rigid if null.</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, Materials.Material material, Sections.Section section, BarType type, Eccentricity eccentricity = null, Connectivity connectivity = null, string identifier = "B")
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;
            this.Identifier = identifier;

            if (eccentricity == null) { eccentricity = Eccentricity.Default; }
            if (connectivity == null) { connectivity = Connectivity.Default; }
            this.BarPart = new BarPart(edge, this.Type, material, section, eccentricity, connectivity, identifier);
        }

        /// <summary>
        /// Construct beam or column with uniform section and uniform start/end conditions
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="section">Section, same at start/end.</param>
        /// <param name="localY">Vector to orient the cross section. If null, localY will be set as a cross product between Z-Axis and the local X Axis./end</param>
        /// <param name="startEccentricity">Analytical eccentricity at start. If null, eccentricity set to 0.</param>
        /// <param name="endEccentricity">Analytical eccentricity at end. If null, eccentricity set to 0.</param>
        /// <param name="startConnectivity">Connectivity. Connectivity set to Rigid if null.</param>
        /// <param name="endConnectivity">Connectivity. Connectivity set to Rigid if null.</param>
        /// <param name="identifier">Identifier</param>
        public Bar(FemDesign.Geometry.Point3d startPoint, FemDesign.Geometry.Point3d endPoint, Materials.Material material, Sections.Section section, BarType type, Geometry.Vector3d localY = null, Eccentricity startEccentricity = null, Eccentricity endEccentricity = null, Connectivity startConnectivity = null, Connectivity endConnectivity = null, string identifier = "B")
        {
            var orientY = localY ?? (endPoint - startPoint).Cross(Geometry.Vector3d.UnitZ);
            Geometry.Edge edge = new Geometry.Edge(startPoint, endPoint, orientY);
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;
            this.Identifier = identifier;

            if (startEccentricity == null) { startEccentricity = Eccentricity.Default; }
            if (endEccentricity == null) { endEccentricity = Eccentricity.Default; }
            if (startConnectivity == null) { startConnectivity = Connectivity.Default; }
            if (endConnectivity == null) { endConnectivity = Connectivity.Default; }

            this.BarPart = new BarPart(edge, this.Type, material, section, startEccentricity, endEccentricity, startConnectivity, endConnectivity, identifier);
        }

        /// <summary>
        /// Construct beam or column with uniform section and different start/end conditions
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="section">Section, same at start/end.</param>
        /// <param name="startEccentricity">Analytical eccentricity at start. If null, eccentricity set to 0.</param>
        /// <param name="endEccentricity">Analytical eccentricity at end. If null, eccentricity set to 0.</param>
        /// <param name="startConnectivity">Start connectivity.</param>
        /// <param name="endConnectivity">End connectivity.</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section section, Eccentricity startEccentricity = null, Eccentricity endEccentricity = null, Connectivity startConnectivity = null, Connectivity endConnectivity = null, string identifier = "B")
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            if (startEccentricity == null) { startEccentricity = Eccentricity.Default; }
            if (endEccentricity == null) { endEccentricity = Eccentricity.Default; }
            if (startConnectivity == null) { startConnectivity = Connectivity.Default; }
            if (endConnectivity == null) { endConnectivity = Connectivity.Default; }

            this.EntityCreated();
            this.Type = type;
            this.Identifier = identifier;


            this.BarPart = new BarPart(edge, this.Type, material, section, startEccentricity, endEccentricity, startConnectivity, endConnectivity, identifier);
        }

        /// <summary>
        /// Construct beam or column with start/end section and different start/end conditions
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="startSection">Start section.</param>
        /// <param name="endSection">End section.</param>
        /// <param name="startEccentricity">Analytical eccentricity at start. If null, eccentricity set to 0.</param>
        /// <param name="endEccentricity">Analytical eccentricity at end. If null, eccentricity set to 0.</param>
        /// <param name="startConnectivity">Start connectivity.</param>
        /// <param name="endConnectivity">End connectivity.</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section startSection, Sections.Section endSection, Eccentricity startEccentricity, Eccentricity endEccentricity, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;
            this.Identifier = identifier;
            this.BarPart = new BarPart(edge, this.Type, material, startSection, endSection, startEccentricity, endEccentricity, startConnectivity, endConnectivity, identifier);
        }


        /// <summary>
        /// Construct beam or column with start/end section and different start/end conditions
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="sections">List of sections, 1 (uniform) or 2 (start/end) items.</param>
        /// <param name="eccentricities">List of analytical eccentricities, 1 (uniform) or 2 (start/end) items.</param>
        /// <param name="connectivities">List of connectivities, 1 (uniform) or 2 (start/end) items.</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section[] sections, Eccentricity[] eccentricities, Connectivity[] connectivities, string identifier)
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;
            this.Identifier = identifier;
            this.BarPart = new BarPart(edge, this.Type, material, sections, eccentricities, connectivities, identifier);
        }

        /// <summary>
        /// Construct beam or column with non-uniform section and different start/end conditions
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="sections">List of sections, 2 or more items.</param>
        /// <param name="positions">List of parametric (0-1) section positions, 2 or more items.</param>
        /// <param name="eccentricities">List of analytical eccentricities, 2 or more items.</param>
        /// <param name="startConnectivity">Start connectivity.</param>
        /// <param name="endConnectivity">End connectivity.</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section[] sections, double[] positions, Eccentricity[] eccentricities, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;
            this.Identifier = identifier;
            this.BarPart = new BarPart(edge, this.Type, material, sections, positions, eccentricities, startConnectivity, endConnectivity, identifier);
        }


        /// <summary>
        /// Construct a truss element.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="material"></param>
        /// <param name="section"></param>
        /// <param name="trussBehaviour"></param>
        /// <param name="identifier"></param>
        /// <exception cref="System.Exception"></exception>
        public Bar(Geometry.Edge edge, Materials.Material material, Sections.Section section, string identifier = "B", Truss_chr_type trussBehaviour = null)
        {
            this.EntityCreated();
            this.Type = BarType.Truss;
            this.Identifier = identifier;
            if(trussBehaviour != null)
            {
                this.TrussBehaviour = trussBehaviour;
            }
            else
            {
                this.TrussBehaviour = Truss_chr_type.Elastic();
            }
            this.BarPart = new BarPart(edge, this.Type, material, section, identifier);
        }


        /// <summary>
        /// Construct a composite beam or composite column with uniform section and uniform start/end conditions.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="compositeSection"></param>
        /// <param name="eccentricity">Analytical eccentricity, same at start/end. Eccentricity set to 0,0 if null.</param>
        /// <param name="connectivity">Connectivity, same at start/end. Connectivity set to Rigid if null.</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Composites.CompositeSection compositeSection, Eccentricity eccentricity = null, Connectivity connectivity = null, string identifier = "B")
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;

            if (eccentricity == null) { eccentricity = Eccentricity.Default; }
            if (connectivity == null) { connectivity = Connectivity.Default; }
            this.BarPart = new BarPart(edge, this.Type, compositeSection, eccentricity, connectivity, identifier);
        }

        /// <summary>
        /// Construct composite beam or composite column with uniform section and different start/end conditions.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="compositeSection"></param>
        /// <param name="startEccentricity">Analytical start eccentricity</param>
        /// <param name="endEccentricity">Analytical end eccentricity</param>
        /// <param name="startConnectivity">Start connectivity</param>
        /// <param name="endConnectivity">End connectivity</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Composites.CompositeSection compositeSection, Eccentricity startEccentricity = null, Eccentricity endEccentricity = null, Connectivity startConnectivity = null, Connectivity endConnectivity = null, string identifier = "B")
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;

            if (startEccentricity == null) { startEccentricity = Eccentricity.Default; }
            if (endEccentricity == null) { endEccentricity = Eccentricity.Default; }
            if (startConnectivity == null) { startConnectivity = Connectivity.Default; }
            if (endConnectivity == null) { endConnectivity = Connectivity.Default; }

            this.BarPart = new BarPart(edge, this.Type, compositeSection, startEccentricity, endEccentricity, startConnectivity, endConnectivity, identifier);
        }

        /// <summary>
        /// Construct a truss element.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="material"></param>
        /// <param name="section"></param>
        /// <param name="identifier"></param>
        /// <returns>The result.</returns>
        /// <exception cref="System.Exception"></exception>
        public static Bar Truss(Geometry.Edge edge, Materials.Material material, Sections.Section section, string identifier)
        {
            var truss = new Bar();

            truss.EntityCreated();
            truss.Type = BarType.Truss;
            truss.Identifier = identifier;
            truss.BarPart = new BarPart(edge, truss.Type, material, section, identifier);
            return truss;
        }

        /// <summary>
        /// Change the analytical model of the bar into a shell model.
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentException"></exception>
        public void ToShellModel(ShellModelType type)
        {
            if (this.Type == BarType.Truss)
                throw new ArgumentException("Analytical models of Truss objects cannot be altered into shell models!");
            if (this.BarPart.HasComplexCompositeRef || this.BarPart.HasDeltaBeamComplexSectionRef)
                throw new ArgumentException("Analytical models of composite bars or bars with Deltabeam sections cannot be altered into shell models!");

            this.ShellModel = type;
        }

        /// <summary>
        /// Updates the section.
        /// </summary>
        /// <param name="section">the section.</param>
        public void UpdateSection(Sections.Section section)
        {
            if(this.Type == BarType.Truss)
            {
                this.UpdateTrussSection(section);
            }
            else if(!this.BarPart.HasComplexCompositeRef || !this.BarPart.HasDeltaBeamComplexSectionRef)
            {
                var sections = new Sections.Section[2] { section, section };
                this.UpdateSection(sections);
            }
            else
            {
                throw new NotImplementedException("Bar with Composite Section can not be updated yet! Send a request.");
            }
        }

        /// <summary>
        /// Update Sections for a Bar Element. Composite Section and Delta Beam are not implemented yet. Send us a request.
        /// </summary>
        /// <param name="sections"></param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public void UpdateSection(Sections.Section[] sections)
        {
            if(!this.BarPart.HasComplexCompositeRef || !this.BarPart.HasDeltaBeamComplexSectionRef)
            {
                if (this.Type != BarType.Truss)
                {
                    if (sections.Length < 2)
                    {
                        throw new System.ArgumentException($"Number of sections: {sections.Length}, must be 2 or more");
                    }

                    for (int i = 0; i < sections.Length; i++)
                    {
                        this.BarPart.ComplexSectionObj.Parts[i].SectionObj = sections[i];
                        this.BarPart.ComplexSectionObj.Parts[i].SectionRef = sections[i].Guid;
                    }
                }
                else if (this.Type == BarType.Truss)
                {
                    throw new System.ArgumentException($"Number of sections: {sections.Length}, must be 1.");
                }
            }
            else
            {
                throw new NotImplementedException("Bar with Composite or Delta Section can not be updated yet! Send a request.");
            }
        }
        private void UpdateTrussSection(Sections.Section section)
        {
            this.BarPart.TrussUniformSectionObj = section;
        }

        /// <summary>
        /// Update Sections for a Bar Element. Material for Composite Section and Delta Beam are not implemented yet. Send us a request.
        /// </summary>
        /// <param name="material"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void UpdateMaterial(Materials.Material material)
        {
            if (!this.BarPart.HasComplexCompositeRef || !this.BarPart.HasDeltaBeamComplexSectionRef)
            {
                this.BarPart.ComplexMaterialObj = material;
                this.BarPart.ComplexMaterialRef = material.Guid;
            }
            else
            {
                throw new NotImplementedException("Bar with Composite Section or Delta Beam can not be updated yet! Send a request.");
            }
        }


        /// Update entities if this bar should be "reconstructed"
        public void UpdateEntities()
        {
            if (this.Type == BarType.Truss)
            {
                this.EntityCreated();
                this.BarPart.EntityCreated();
            }
            else
            {
                this.EntityCreated();
                this.BarPart.EntityCreated();
                this.BarPart.ComplexSectionObj.EntityCreated();
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            if (this.Type == BarType.Beam || this.Type == BarType.Column)
            {
                if (this.BarPart.HasComplexCompositeRef)
                {
                    var compositeSection = this.BarPart.ComplexCompositeObj.CompositeSections[0];
                    var name = compositeSection.ParameterDictionary[Composites.CompositeSectionParameterType.Name];
                    List<string> compositeMaterials = compositeSection.Materials.Select(m => m.Name).ToList();

                    return $"{this.Type} Start: {this.BarPart.Edge.Points.First()}, End: {this.BarPart.Edge.Points.Last()}, Length: {this.BarPart.Edge.Length} m, Section: Composite-{compositeSection.Type}/{name}, Materials: {string.Join(", ", compositeMaterials)}";
                }
                if (this.BarPart.HasDeltaBeamComplexSectionRef)
                {
                    return $"{this.Type} Start: {this.BarPart.Edge.Points.First()}, End: {this.BarPart.Edge.Points.Last()}, Length: {this.BarPart.Edge.Length} m, Sections: Delta section type, Material: Delta beam";
                }
                return $"{this.Type} Start: {this.BarPart.Edge.Points.First()}, End: {this.BarPart.Edge.Points.Last()}, Length: {this.BarPart.Edge.Length} m, Sections: ({this.BarPart.ComplexSectionObj.Sections.First()._sectionName}, {this.BarPart.ComplexSectionObj.Sections.Last()._sectionName}), Material: {this.BarPart.ComplexMaterialObj}";
            }
            else if (this.Type == BarType.Truss)
            {
                return $"{this.Type} Start: {this.BarPart.Edge.Points.First()}, End: {this.BarPart.Edge.Points.Last()}, Length: {this.BarPart.Edge.Length} m, Section: {this.BarPart.TrussUniformSectionObj._sectionName}, Material: {this.BarPart.ComplexMaterialObj}";
            }
            else
            {
                return base.ToString();
            }
        }
    }

    /// <summary>
    /// Defines the Shell Model Type enumeration.
    /// </summary>
    [System.Serializable]
    public enum ShellModelType
    {
        /// <summary>
        /// Bar model.
        /// </summary>
        [Parseable("none", "None", "NONE")]
        [XmlEnum("none")]
        None,

        /// <summary>
        /// Shell model. Contains only shell elements.
        /// </summary>
        [Parseable("simple", "Simple", "SIMPLE")]
        [XmlEnum("simple")]
        Simple,

        /// <summary>
        /// Shell model. Fictitious bars on the boundary of the shell model.
        /// </summary>
        [Parseable("complex", "Complex", "COMPLEX")]
        [XmlEnum("complex")]
        Complex
    }
}