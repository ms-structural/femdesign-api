// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using FemDesign.Geometry;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Surface Load.
    /// </summary>
    [System.Serializable]
    public partial class SurfaceLoad: ForceLoadBase
    {
        // attributes
        /// <summary>
        /// Gets or sets the load projection.
        /// </summary>
        [XmlAttribute("load_projection")]
        public bool LoadProjection { get; set; } // bool

        // elements
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        [XmlElement("region", Order = 1)]
        public Geometry.Region Region { get; set; } // region_type
        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        [XmlElement("direction", Order = 2)]
        public Geometry.Vector3d Direction { get; set; } // point_type_3d
        /// <summary>
        /// Gets or sets the loads.
        /// </summary>
        [XmlElement("load", Order = 3)]
        public List<LoadLocationValue> Loads = new List<LoadLocationValue>(); // location_value
        [XmlIgnore]
        public bool IsConstant
        {
            get
            {
                if (this.Loads.Count > 1)
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private SurfaceLoad()
        {

        }

        /// <summary>
        /// Create a uniform surface load.
        /// </summary>
        /// <remarks>
        /// Units: <c>kN/m²</c>. The magnitude of <paramref name="load"/> is used as the load intensity and its direction
        /// defines the load direction.
        /// </remarks>
        /// <param name="region">Region to apply the surface load to.</param>
        /// <param name="load">Surface load intensity vector (magnitude in <c>kN/m²</c>).</param>
        /// <param name="loadCase">Load case to assign to the surface load.</param>
        /// <param name="loadProjection">
        /// If <c>false</c>, intensity is interpreted along the action line (e.g. dead load).
        /// If <c>true</c>, intensity is interpreted perpendicular to the direction of load (e.g. snow load).
        /// </param>
        /// <param name="comment">Optional comment.</param>
        public SurfaceLoad(Geometry.Region region, Geometry.Vector3d load, LoadCase loadCase, bool loadProjection = false, string comment = "") : this(region, new List<LoadLocationValue> { new LoadLocationValue(region.Contours[0].Edges[0].Points[0], load.Length()) }, load.Normalize(), loadCase, loadProjection, comment)
        {

        }

        /// <summary>
        /// Create a variable surface load.
        /// </summary>
        /// <remarks>
        /// Units: <c>kN/m²</c>. The <paramref name="loads"/> collection defines load intensities at the given locations.
        /// </remarks>
        /// <param name="region">Region to apply the surface load to.</param>
        /// <param name="loads">List of load location/value pairs.</param>
        /// <param name="loadDirection">Load direction (unit vector).</param>
        /// <param name="loadCase">Load case to assign to the surface load.</param>
        /// <param name="loadProjection">
        /// If <c>false</c>, intensity is interpreted along the action line (e.g. dead load).
        /// If <c>true</c>, intensity is interpreted perpendicular to the direction of load (e.g. snow load).
        /// </param>
        /// <param name="comment">Optional comment.</param>
        public SurfaceLoad(Geometry.Region region, List<LoadLocationValue> loads, Geometry.Vector3d loadDirection, LoadCase loadCase, bool loadProjection = false, string comment = "")
        {
            this.EntityCreated();
            this.LoadCase = loadCase;
            this.Comment = comment;
            this.LoadProjection = loadProjection;
            this.LoadType = ForceLoadType.Force;
            this.Region = region;
            this.Direction = loadDirection;
            foreach (LoadLocationValue _load in loads)
            {
                this.Loads.Add(_load);
            }
        }

        /// <summary>
        /// Caseless surface load
        /// </summary>
        private SurfaceLoad(Geometry.Region region, Geometry.Vector3d load, bool loadProjection = false) : this(region, new List<LoadLocationValue> { new LoadLocationValue(region.Contours[0].Edges[0].Points[0], load.Length()) }, load.Normalize(), loadProjection)
        {

        }

        /// <summary>
        /// Caseless surface load
        /// </summary>
        /// <param name="region"></param>
        /// <param name="loads"></param>
        /// <param name="loadDirection"></param>
        /// <param name="loadProjection"></param>
        private SurfaceLoad(Geometry.Region region, List<LoadLocationValue> loads, Geometry.Vector3d loadDirection, bool loadProjection = false)
        {
            this.EntityCreated();
            this.LoadProjection = loadProjection;
            this.LoadType = ForceLoadType.Force;
            this.Region = region;
            this.Direction = loadDirection;
            foreach (LoadLocationValue _load in loads)
            {
                this.Loads.Add(_load);
            }
        }

        /// <summary>
        /// Create a uniform surface load.
        /// </summary>
        /// <remarks>Units: <c>kN/m²</c>.</remarks>
        /// <param name="region">Region to apply the surface load to.</param>
        /// <param name="force">Surface load intensity vector (magnitude in <c>kN/m²</c>).</param>
        /// <param name="loadCase">Load case to assign to the surface load.</param>
        /// <param name="loadProjection">
        /// If <c>false</c>, intensity is interpreted along the action line (e.g. dead load).
        /// If <c>true</c>, intensity is interpreted perpendicular to the direction of load (e.g. snow load).
        /// </param>
        /// <param name="comment">Optional comment.</param>
        /// <returns>A <see cref="SurfaceLoad"/> instance.</returns>
        public static SurfaceLoad Uniform(Geometry.Region region, Geometry.Vector3d force, LoadCase loadCase, bool loadProjection = false, string comment = "")
        {
            return  new SurfaceLoad(region, force, loadCase, loadProjection, comment);
        }

        /// <summary>
        /// Create a uniform surface load without a load case.
        /// </summary>
        /// <remarks>
        /// Units: <c>kN/m²</c>. This is mainly intended for interoperability with the "caseless" StruXML load types.
        /// </remarks>
        /// <param name="region">Region to apply the surface load to.</param>
        /// <param name="force">Surface load intensity vector (magnitude in <c>kN/m²</c>).</param>
        /// <returns>A <see cref="SurfaceLoad"/> with <see cref="LoadBase.LoadCase"/> left unset.</returns>
        public static SurfaceLoad CaselessUniform(Geometry.Region region, Geometry.Vector3d force)
        {
            return new SurfaceLoad(region, force, false);
        }

        /// <summary>
        /// Create a variable surface load defined by three location/value pairs.
        /// </summary>
        /// <remarks>Units: <c>kN/m²</c>.</remarks>
        /// <param name="region">Region to apply the surface load to.</param>
        /// <param name="direction">Load direction.</param>
        /// <param name="loadLocationValue">List of exactly three load location/value pairs.</param>
        /// <param name="loadCase">Load case to assign to the surface load.</param>
        /// <param name="loadProjection">
        /// If <c>false</c>, intensity is interpreted along the action line (e.g. dead load).
        /// If <c>true</c>, intensity is interpreted perpendicular to the direction of load (e.g. snow load).
        /// </param>
        /// <param name="comment">Optional comment.</param>
        /// <returns>A <see cref="SurfaceLoad"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="loadLocationValue"/> does not contain exactly 3 items.</exception>
        public static SurfaceLoad Variable(Geometry.Region region, Geometry.Vector3d direction, List<LoadLocationValue> loadLocationValue, LoadCase loadCase, bool loadProjection = false, string comment = "")
        {
            if (loadLocationValue.Count != 3)
            {
                throw new System.ArgumentException("loadLocationValue must contain 3 items");
            }

            return new SurfaceLoad(region, loadLocationValue, direction, loadCase, loadProjection, comment);
        }

        /// <summary>
        /// Convert a caseless StruXML surface load to a <see cref="SurfaceLoad"/>.
        /// </summary>
        /// <param name="obj">Caseless StruXML surface load.</param>
        /// <returns>A <see cref="SurfaceLoad"/> instance.</returns>
        public static explicit operator SurfaceLoad(StruSoft.Interop.StruXml.Data.Caseless_surface_load_type obj)
        {
            var srfLoad = new SurfaceLoad();

            srfLoad.Guid = new System.Guid(obj.Guid);
            srfLoad.Action = obj.Action.ToString();
            
            srfLoad.LoadProjection = obj.Load_projection;
            srfLoad.LoadType = (ForceLoadType)Enum.Parse(typeof(ForceLoadType), obj.Load_type.ToString());

            
            srfLoad.Direction = new Vector3d(obj.Direction.X, obj.Direction.Y, obj.Direction.Z);

            // load location value
            var loadLocationValue = new List<LoadLocationValue>();
            foreach(var item in obj.Load)
            {
                loadLocationValue.Add(item);
            }
            srfLoad.Loads = loadLocationValue;

            // region

            srfLoad.Region = obj.Region;

            return srfLoad;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            string text = "";
            if (IsConstant)
            {
                text = $"{this.GetType().Name} q1: {this.Loads.First().Value * this.Direction} kN/m\u00B2, Projected: {this.LoadProjection}, Constant";
            }
            else
            {
                text = $"{this.GetType().Name} q1: {this.Loads[0].Value * this.Direction} kN/m\u00B2, q2: {this.Loads[1].Value * this.Direction} kN/m\u00B2, q3: {this.Loads[2].Value * this.Direction} kN/m\u00B2, Projected: {this.LoadProjection}, Variable";
            }

            if (LoadCase != null)
                return text + $", LoadCase: {this.LoadCase.Name}";
            else
                return text;
        }
    }
}