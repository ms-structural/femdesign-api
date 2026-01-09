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
        /// <summary>
        /// Gets or sets the foundations.
        /// </summary>
        [XmlElement("foundations", Order = 1)]
        public Foundations.Foundations Foundations { get; set; } = new Foundations.Foundations();

        /// <summary>
        /// Gets or sets the soil elements.
        /// </summary>
        [XmlElement("soil_elements", Order = 2)]
        public FemDesign.Soil.SoilElements SoilElements { get; set; }

        /// <summary>
        /// Gets or sets the retaining walls.
        /// </summary>
        [XmlElement("retaining_wall", Order = 3)]
        public List<StruSoft.Interop.StruXml.Data.Retaining_wall_type> RetainingWalls { get; set; }

        /// <summary>
        /// Gets or sets the bars.
        /// </summary>
        [XmlElement("bar", Order = 4)]
        public List<Bars.Bar> Bars { get; set; } = new List<Bars.Bar>();

        /// <summary>
        /// Gets or sets the column corbels.
        /// </summary>
        [XmlElement("column_corbel", Order = 5)]
        public List<Bars.ColumnCorbel> ColumnCorbels { get; set; } = new List<Bars.ColumnCorbel>();

        /// <summary>
        /// Gets or sets the steel bar hunches.
        /// </summary>
        [XmlElement("steel_bar_haunch", Order = 6)]
        public List<StruSoft.Interop.StruXml.Data.Stbar_haunch_type> SteelBarHunches { get; set; }

        /// <summary>
        /// Gets or sets the steel bar stiffeners.
        /// </summary>
        [XmlElement("steel_bar_stiffener", Order = 7)]
        public List<StruSoft.Interop.StruXml.Data.Stbar_siffener_type> SteelBarStiffeners { get; set; }

        /// <summary>
        /// Gets or sets the beam reduction zones.
        /// </summary>
        [XmlElement("rc_beam_reduction_zone", Order = 8)]
        public List<StruSoft.Interop.StruXml.Data.Beam_reduction_zone_type> BeamReductionZones { get; set; }

        /// <summary>
        /// Gets or sets the hidden bars.
        /// </summary>
        [XmlElement("hidden_bar", Order = 9)]
        public List<Reinforcement.ConcealedBar> HiddenBars { get; set; } = new List<Reinforcement.ConcealedBar>();

        /// <summary>
        /// Gets or sets the bar reinforcements.
        /// </summary>
        [XmlElement("bar_reinforcement", Order = 10)]
        public List<Reinforcement.BarReinforcement> BarReinforcements { get; set; } = new List<Reinforcement.BarReinforcement>();

        /// <summary>
        /// Gets or sets the slabs.
        /// </summary>
        [XmlElement("slab", Order = 11)]
        public List<Shells.Slab> Slabs { get; set; } = new List<Shells.Slab>();

        /// <summary>
        /// Gets or sets the shell bucklings.
        /// </summary>
        [XmlElement("shell_buckling", Order = 12)]
        public List<Shells.ShellBucklingType> ShellBucklings { get; set; } = new List<Shells.ShellBucklingType>();

        /// <summary>
        /// Gets or sets the wall corbel.
        /// </summary>
        [XmlElement("wall_corbel", Order = 13)]
        public List<Shells.WallCorbel> WallCorbel { get; set; } = new List<Shells.WallCorbel>();

        /// <summary>
        /// Gets or sets the surface reinforcement parameters.
        /// </summary>
        [XmlElement("surface_reinforcement_parameters", Order = 14)]
        public List<Reinforcement.SurfaceReinforcementParameters> SurfaceReinforcementParameters { get; set; } = new List<Reinforcement.SurfaceReinforcementParameters>();

        /// <summary>
        /// Gets or sets the surface reinforcements.
        /// </summary>
        [XmlElement("surface_reinforcement", Order = 15)]
        public List<Reinforcement.SurfaceReinforcement> SurfaceReinforcements { get; set; } = new List<Reinforcement.SurfaceReinforcement>();

        /// <summary>
        /// Gets or sets the surface reinforcement single by line.
        /// </summary>
        [XmlElement("surface_reinforcement_single_by_line", Order = 16)]
        public List<StruSoft.Interop.StruXml.Data.Surface_rf_line_type> SurfaceReinforcementSingleByLine { get; set; }

        /// <summary>
        /// Gets or sets the surface reinforcement single by rectangle.
        /// </summary>
        [XmlElement("surface_reinforcement_single_by_rectangle", Order = 17)]
        public List<StruSoft.Interop.StruXml.Data.Surface_rf_rect_type> SurfaceReinforcementSingleByRectangle { get; set; }

        /// <summary>
        /// Gets or sets the punching area.
        /// </summary>
        [XmlElement("punching_area", Order = 18)]
        public List<Reinforcement.PunchingArea> PunchingArea { get; set; } = new List<Reinforcement.PunchingArea>();

        /// <summary>
        /// Gets or sets the punching area wall.
        /// </summary>
        [XmlElement("punching_area_wall", Order = 19)]
        public List<StruSoft.Interop.StruXml.Data.Punching_area_wall_type> PunchingAreaWall { get; set; }

        /// <summary>
        /// Gets or sets the punching reinforcements.
        /// </summary>
        [XmlElement("punching_reinforcement", Order = 20)]
        public List<Reinforcement.PunchingReinforcement> PunchingReinforcements { get; set; } = new List<Reinforcement.PunchingReinforcement>();

        /// <summary>
        /// Gets or sets the no shear regions.
        /// </summary>
        [Obsolete("Use `NoShearControlRegions`", true)]
        [XmlElement("no-shear_region", Order = 21)]
        public List<Reinforcement.NoShearRegionType> NoShearRegions { get; set; } = new List<Reinforcement.NoShearRegionType>();

        /// <summary>
        /// Gets or sets the no shear control regions.
        /// </summary>
        [XmlElement("shear_control_region", Order = 22)]
        public List<Reinforcement.ShearControlRegionType> NoShearControlRegions { get; set; } = new List<Reinforcement.ShearControlRegionType>();

        /// <summary>
        /// Gets or sets the surface shear reinforcement.
        /// </summary>
        [XmlElement("surface_shear_reinforcement", Order = 23)]
        public List<StruSoft.Interop.StruXml.Data.Surface_shear_rf_type> SurfaceShearReinforcement { get; set; }

        /// <summary>
        /// Gets or sets the panels.
        /// </summary>
        [XmlElement("panel", Order = 24)]
        public List<Shells.Panel> Panels { get; set; } = new List<Shells.Panel>();

        /// <summary>
        /// Gets or sets the post tensioned cables.
        /// </summary>
        [XmlElement("post-tensioned_cable", Order = 25)]
        public List<Reinforcement.Ptc> PostTensionedCables { get; set; } = new List<Reinforcement.Ptc>();

        /// <summary>
        /// Gets or sets the loads.
        /// </summary>
        [XmlElement("loads", Order = 26)]
        public Loads.Loads Loads { get; set; } = new Loads.Loads();

        /// <summary>
        /// Gets or sets the supports.
        /// </summary>
        [XmlElement("supports", Order = 27)]
        public Supports.Supports Supports { get; set; } = new Supports.Supports();

        /// <summary>
        /// Gets or sets the advanced fem.
        /// </summary>
        [XmlElement("advanced-fem", Order = 28)]
        public ModellingTools.AdvancedFem AdvancedFem { get; set; } = new ModellingTools.AdvancedFem();

        /// <summary>
        /// Gets or sets the storeys.
        /// </summary>
        [XmlElement("storeys", Order = 29)]
        public StructureGrid.Storeys Storeys { get; set; }

        /// <summary>
        /// Gets or sets the axes.
        /// </summary>
        [XmlElement("axes", Order = 30)]
        public StructureGrid.Axes Axes { get; set; }

        /// <summary>
        /// Gets or sets the reference planes.
        /// </summary>
        [XmlElement("reference_planes", Order = 31)]
        public List<StruSoft.Interop.StruXml.Data.Refplane_type> ReferencePlanes { get; set; }

        /// <summary>
        /// Gets or sets the labelled sections.
        /// </summary>
        [XmlElement("labelled_sections_geometry", Order = 32)]
        public AuxiliaryResults.LabelledSectionsGeometry LabelledSections { get; set; }

        /// <summary>
        /// Gets or sets the result points.
        /// </summary>
        [XmlElement("result_points", Order = 33)]
        public AuxiliaryResults.ResultPointsGeometry ResultPoints { get; set; }

        /// <summary>
        /// Gets or sets the virtual bar container.
        /// </summary>
        [XmlElement("result_lines", Order = 34)]
        public AuxiliaryResults.VirtualBarContainer VirtualBarContainer { get; set; }

        /// <summary>
        /// Gets or sets the t solids.
        /// </summary>
        [XmlElement("tsolids", Order = 35)]
        public List<StruSoft.Interop.StruXml.Data.Polyhedron_type> TSolids { get; set; }

        /// <summary>
        /// Gets or sets the peak smoothing regions.
        /// </summary>
        [XmlElement("peak_smoothing_region", Order = 36)]
        public List<FiniteElements.PeakSmoothingRegion> PeakSmoothingRegions { get; set; }

        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        [XmlElement("regions", Order = 37)]
        public Geometry.Regions Regions { get; set; }

        // ref planes
        // tsolids
        // regions
    }
}