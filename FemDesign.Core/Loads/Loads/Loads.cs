// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using System.Xml;
using System.Linq;
using StruSoft.Interop.StruXml.Data;
using System.IO;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents loads.
    /// </summary>
    [System.Serializable]
    [XmlRoot("loads", Namespace = "urn:strusoft")]
    public partial class Loads
    {
        // dummy elements are needed to deserialize an .struxml model correctly as order of elements is needed.
        // if dummy elements are not used for undefined types deserialization will not work properly
        // when serializing these dummy elements must be nulled. 
        /// <summary>
        /// Gets or sets the point loads.
        /// </summary>
        [XmlElement("point_load", Order = 1)]
        public List<PointLoad> PointLoads = new List<PointLoad>(); // point_load_type

        /// <summary>
        /// Gets or sets the line loads.
        /// </summary>
        [XmlElement("line_load", Order = 2)]
        public List<LineLoad> LineLoads = new List<LineLoad>(); // line_load_type

        /// <summary>
        /// Gets or sets the pressure loads.
        /// </summary>
        [XmlElement("pressure_load", Order = 3)]
        public List<PressureLoad> PressureLoads = new List<PressureLoad>(); // pressure_load_type

        /// <summary>
        /// Gets or sets the surface loads.
        /// </summary>
        [XmlElement("surface_load", Order = 4)]
        public List<SurfaceLoad> SurfaceLoads = new List<SurfaceLoad>(); // surface_load_type

        /// <summary>
        /// Gets or sets the line temperature loads.
        /// </summary>
        [XmlElement("line_temperature_variation_load", Order = 5)]
        public List<LineTemperatureLoad> LineTemperatureLoads = new List<LineTemperatureLoad>(); // line_temperature_load_type

        /// <summary>
        /// Gets or sets the surface temperature loads.
        /// </summary>
        [XmlElement("surface_temperature_variation_load", Order = 6)]
        public List<SurfaceTemperatureLoad> SurfaceTemperatureLoads = new List<SurfaceTemperatureLoad>(); // surface_temperature_variation_load

        /// <summary>
        /// Gets or sets the line stress loads.
        /// </summary>
        [XmlElement("line_stress_load", Order = 7)]
        public List<LineStressLoad> LineStressLoads = new List<LineStressLoad>(); // line_stress_load

        /// <summary>
        /// Gets or sets the surface stress loads.
        /// </summary>
        [XmlElement("surface_stress_load", Order = 8)]
        public List<StruSoft.Interop.StruXml.Data.Surface_stress_load_type> SurfaceStressLoads { get; set; } // surface_stress_load_type

        /// <summary>
        /// Gets or sets the point support motion loads.
        /// </summary>
        [XmlElement("point_support_motion_load", Order = 9)]
        public List<PointSupportMotion> PointSupportMotionLoads = new List<PointSupportMotion>(); // point_support_motion_load_type

        /// <summary>
        /// Gets or sets the line support motion loads.
        /// </summary>
        [XmlElement("line_support_motion_load", Order = 10)]
        public List<LineSupportMotion> LineSupportMotionLoads = new List<LineSupportMotion>(); // line_support_motion_load_type

        /// <summary>
        /// Gets or sets the surface support motion loads.
        /// </summary>
        [XmlElement("surface_support_motion_load", Order = 11)]
        public List<SurfaceSupportMotion> SurfaceSupportMotionLoads { get; set;} = new List<SurfaceSupportMotion>(); // surface_support_motion_load_type

        /// <summary>
        /// Gets or sets the masses.
        /// </summary>
        [XmlElement("mass", Order = 12)]
        public List<FemDesign.Loads.Mass> Masses { get; set;} = new List<FemDesign.Loads.Mass>(); // mass_point_type

        /// <summary>
        /// Gets or sets the load case mass conversion table.
        /// </summary>
        [XmlElement("load_case_mass_conversion_table", Order = 13)]
        public MassConversionTable LoadCaseMassConversionTable { get; set; } // mass_conversion_type

        /// <summary>
        /// Gets or sets the seismic loads.
        /// </summary>
        [XmlElement("seismic_load", Order = 14)]
        public StruSoft.Interop.StruXml.Data.Seismic_load_type SeismicLoads { get; set; } // seismic_load_type

        /// <summary>
        /// Gets or sets the footfall analysis data.
        /// </summary>
        [XmlElement("footfall_analysis_data", Order = 15)]
        public List<Footfall> FootfallAnalysisData = new List<Footfall>(); // footfall_type

        /// <summary>
        /// Gets or sets the ground accelerations.
        /// </summary>
        [XmlElement("ground_acceleration", Order = 16)]
        public StruSoft.Interop.StruXml.Data.Ground_acceleration_type GroundAccelerations { get; set; } // ground_acceleration_type

        /// <summary>
        /// Gets or sets the excitation force.
        /// </summary>
        [XmlElement("excitation_force", Order = 17)]
        public ExcitationForce ExcitationForce { get; set; }

        /// <summary>
        /// Gets or sets the periodic excitations.
        /// </summary>
        [XmlElement("periodic_excitation", Order = 18)]
        public PeriodicExcitation PeriodicExcitations { get; set; } // periodic_excitation_type

        /// <summary>
        /// Gets or sets the moving loads.
        /// </summary>
        [XmlElement("moving_load", Order = 19)]
        public List<StruSoft.Interop.StruXml.Data.Moving_load_type> MovingLoads { get; set; } // moving_load_type

        /// <summary>
        /// Gets or sets the load cases.
        /// </summary>
        [XmlElement("load_case", Order = 20)]
        public List<LoadCase> LoadCases = new List<LoadCase>(); // load_case_type

        /// <summary>
        /// Gets or sets the load combinations.
        /// </summary>
        [XmlElement("load_combination", Order = 21)]
        public List<LoadCombination> LoadCombinations = new List<LoadCombination>(); // load_combination_type

        /// <summary>
        /// Gets or sets the load group table.
        /// </summary>
        [XmlElement("load_group_table", Order = 22)]
        public LoadGroupTable LoadGroupTable { get; set; } // load_group_table_type


        /// <summary>
        /// Get all load elements stored in this <see cref="Loads"/> container.
        /// </summary>
        /// <remarks>
        /// This is a convenience method that returns a flat list of objects implementing
        /// <see cref="FemDesign.GenericClasses.ILoadElement"/> (e.g. point/line/surface loads, support motions,
        /// temperature/stress loads, masses, and excitation definitions).
        /// </remarks>
        /// <returns>All load elements in this instance, in StruXML serialization order.</returns>
        public List<FemDesign.GenericClasses.ILoadElement> GetLoads()
        {
            var objs = new List<FemDesign.GenericClasses.ILoadElement>();
            objs.AddRange(this.PointLoads);
            objs.AddRange(this.PointSupportMotionLoads);
            objs.AddRange(this.LineLoads);
            objs.AddRange(this.LineSupportMotionLoads);
            objs.AddRange(this.LineStressLoads);
            objs.AddRange(this.LineTemperatureLoads);
            objs.AddRange(this.PressureLoads);
            objs.AddRange(this.SurfaceLoads);
            objs.AddRange(this.SurfaceSupportMotionLoads);
            objs.AddRange(this.SurfaceTemperatureLoads);
            objs.AddRange(this.FootfallAnalysisData);
            objs.AddRange(this.Masses);
            objs.Add(this.ExcitationForce);
            objs.Add(this.PeriodicExcitations);
            //objs.Add(this.GroundAccelerations);
            //objs.Add(this.SeismicLoads);
            return objs;
        }

        /// <summary>
        /// Gets the <see cref="ModelGeneralLoadGroup">ModelGeneralLoadGroup</see> objects of the LoadGroupTable.
        /// </summary>
        /// <returns>List of <see cref="ModelGeneralLoadGroup">ModelGeneralLoadGroup</see> objects</returns>
        public List<ModelGeneralLoadGroup> GetLoadGroups()
        {
            List<ModelGeneralLoadGroup> loadGroups = new List<ModelGeneralLoadGroup>();

            if (LoadGroupTable == null) return loadGroups;

            foreach (ModelGeneralLoadGroup generalLoadGroup in LoadGroupTable.GeneralLoadGroups)
                loadGroups.Add(generalLoadGroup);
            return loadGroups;

        }


        /// <summary>
        /// Deserialize a <see cref="Loads"/> object from a <c>.struxml</c> file on disk.
        /// </summary>
        /// <param name="filePath">Path to a <c>.struxml</c> file containing a <c>&lt;loads&gt;</c> element.</param>
        /// <returns>The deserialized <see cref="Loads"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> does not have the <c>.struxml</c> extension.</exception>
        public static Loads DeserializeFromFilePath(string filePath)
        {
            // check file extension
            if (Path.GetExtension(filePath) != ".struxml")
            {
                throw new System.ArgumentException("File extension must be .struxml! Model.DeserializeModel failed.");
            }

            //
            //XmlSerializer deserializer = new XmlSerializer(typeof(Loads));
            var reader = new XmlTextReader(filePath);

            reader.ReadToDescendant("loads"); //tag which matches the InnerObject

            XmlSerializer serializer = new XmlSerializer(typeof(Loads));

            var obj = serializer.Deserialize(reader.ReadSubtree()); //this gives serializer the part of XML that is for  the innerObject data

            reader.Close(); //now skip the rest 


            Loads loads = (Loads)obj;

            return loads;
        }
    }
}