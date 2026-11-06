// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign
{
    /// <summary>
    /// entities.
    /// </summary>
    [System.Serializable]
    public partial class Entities
    {
        // dummy elements are needed to deserialize an .struxml model correctly as order of elements is needed.
        // if dummy elements are not used for undefined types deserialization will not work properly
        // when serializing these dummy elements must be nulled. 
        [XmlElement("foundations", Order = 1)]
        public Foundations.Foundations Foundations { get; set; } = new Foundations.Foundations();

        [XmlElement("soil_elements", Order = 2)]
        public FemDesign.Soil.SoilElements SoilElements { get; set; }

        [XmlElement("retaining_wall", Order = 3)]
        public List<StruSoft.Interop.StruXml.Data.Retaining_wall_type> RetainingWalls { get; set; }

        [XmlElement("bar", Order = 4)]
        public List<Bars.Bar> Bars { get; set; } = new List<Bars.Bar>();

        [XmlElement("column_corbel", Order = 5)]
        public List<Bars.ColumnCorbel> ColumnCorbels { get; set; } = new List<Bars.ColumnCorbel>();

        [XmlElement("steel_bar_haunch", Order = 6)]
        public List<StruSoft.Interop.StruXml.Data.Stbar_haunch_type> SteelBarHunches { get; set; }

        [XmlElement("steel_bar_stiffener", Order = 7)]
        public List<StruSoft.Interop.StruXml.Data.Stbar_siffener_type> SteelBarStiffeners { get; set; }

        [XmlElement("rc_beam_reduction_zone", Order = 8)]
        public List<StruSoft.Interop.StruXml.Data.Beam_reduction_zone_type> BeamReductionZones { get; set; }

        [XmlElement("hidden_bar", Order = 9)]
        public List<Reinforcement.ConcealedBar> HiddenBars { get; set; } = new List<Reinforcement.ConcealedBar>();

        [XmlElement("bar_reinforcement", Order = 10)]
        public List<Reinforcement.BarReinforcement> BarReinforcements { get; set; } = new List<Reinforcement.BarReinforcement>();

        [XmlElement("slab", Order = 11)]
        public List<Shells.Slab> Slabs { get; set; } = new List<Shells.Slab>();

        [XmlElement("shell_buckling", Order = 12)]
        public List<Shells.ShellBucklingType> ShellBucklings { get; set; } = new List<Shells.ShellBucklingType>();

        [XmlElement("wall_corbel", Order = 13)]
        public List<Shells.WallCorbel> WallCorbel { get; set; } = new List<Shells.WallCorbel>();

        [XmlElement("surface_reinforcement_parameters", Order = 14)]
        public List<Reinforcement.SurfaceReinforcementParameters> SurfaceReinforcementParameters { get; set; } = new List<Reinforcement.SurfaceReinforcementParameters>();

        [XmlElement("surface_reinforcement", Order = 15)]
        public List<Reinforcement.SurfaceReinforcement> SurfaceReinforcements { get; set; } = new List<Reinforcement.SurfaceReinforcement>();

        [XmlElement("surface_reinforcement_single_by_line", Order = 16)]
        public List<StruSoft.Interop.StruXml.Data.Surface_rf_line_type> SurfaceReinforcementSingleByLine { get; set; }

        [XmlElement("surface_reinforcement_single_by_rectangle", Order = 17)]
        public List<StruSoft.Interop.StruXml.Data.Surface_rf_rect_type> SurfaceReinforcementSingleByRectangle { get; set; }

        [XmlElement("punching_area", Order = 18)]
        public List<Reinforcement.PunchingArea> PunchingArea { get; set; } = new List<Reinforcement.PunchingArea>();

        [XmlElement("punching_area_wall", Order = 19)]
        public List<StruSoft.Interop.StruXml.Data.Punching_area_wall_type> PunchingAreaWall { get; set; }

        [XmlElement("punching_reinforcement", Order = 20)]
        public List<Reinforcement.PunchingReinforcement> PunchingReinforcements { get; set; } = new List<Reinforcement.PunchingReinforcement>();

        [Obsolete("Use `NoShearControlRegions`")]
        [XmlIgnore] // .NET 10 no longer skips [Obsolete] members; keep it out of the XML as on earlier runtimes.
        [XmlElement("no-shear_region", Order = 21)]
        public List<Reinforcement.NoShearRegionType> NoShearRegions { get; set; } = new List<Reinforcement.NoShearRegionType>();

        [XmlElement("shear_control_region", Order = 22)]
        public List<Reinforcement.ShearControlRegionType> NoShearControlRegions { get; set; } = new List<Reinforcement.ShearControlRegionType>();

        [XmlElement("surface_shear_reinforcement", Order = 23)]
        public List<StruSoft.Interop.StruXml.Data.Surface_shear_rf_type> SurfaceShearReinforcement { get; set; }

        [XmlElement("panel", Order = 24)]
        public List<Shells.Panel> Panels { get; set; } = new List<Shells.Panel>();

        [XmlElement("post-tensioned_cable", Order = 25)]
        public List<Reinforcement.Ptc> PostTensionedCables { get; set; } = new List<Reinforcement.Ptc>();

        [XmlElement("loads", Order = 26)]
        public Loads.Loads Loads { get; set; } = new Loads.Loads();

        [XmlElement("supports", Order = 27)]
        public Supports.Supports Supports { get; set; } = new Supports.Supports();

        [XmlElement("advanced-fem", Order = 28)]
        public ModellingTools.AdvancedFem AdvancedFem { get; set; } = new ModellingTools.AdvancedFem();

        [XmlElement("storeys", Order = 29)]
        public StructureGrid.Storeys Storeys { get; set; }

        [XmlElement("axes", Order = 30)]
        public StructureGrid.Axes Axes { get; set; }

        [XmlElement("reference_planes", Order = 31)]
        public List<StruSoft.Interop.StruXml.Data.Refplane_type> ReferencePlanes { get; set; }

        [XmlElement("labelled_sections_geometry", Order = 32)]
        public AuxiliaryResults.LabelledSectionsGeometry LabelledSections { get; set; }

        [XmlElement("result_points", Order = 33)]
        public AuxiliaryResults.ResultPointsGeometry ResultPoints { get; set; }

        [XmlElement("result_lines", Order = 34)]
        public AuxiliaryResults.VirtualBarContainer VirtualBarContainer { get; set; }

        [XmlElement("tsolids", Order = 35)]
        public List<StruSoft.Interop.StruXml.Data.Polyhedron_type> TSolids { get; set; }

        [XmlElement("peak_smoothing_region", Order = 36)]
        public List<FiniteElements.PeakSmoothingRegion> PeakSmoothingRegions { get; set; }

        [XmlElement("regions", Order = 37)]
        public Geometry.Regions Regions { get; set; }

        // ref planes
        // tsolids
        // regions
    }
}