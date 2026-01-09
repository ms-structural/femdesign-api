// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// pressure_load_type.
    /// </summary>
    [System.Serializable]
    public partial class PressureLoad: ForceLoadBase
    {
        // attributes
        /// <summary>
        /// Gets or sets the load projection.
        /// </summary>
        [XmlAttribute("load_projection")]
        public bool LoadProjection { get; set; } // bool
        
        /// <summary>
        /// Gets or sets the z0.
        /// </summary>
        [XmlAttribute("z0")]
        public double _z0; // abs_max_1e20
        [XmlIgnore]
        public double Z0
        {
            get {return this._z0;}
            set {this._z0 = RestrictedDouble.AbsMax_1e20(value);}
        }
        
        /// <summary>
        /// Gets or sets the q0.
        /// </summary>
        [XmlAttribute("q0")]
        public double _q0; // abs_max_1e20
        [XmlIgnore]
        public double Q0
        {
            get {return this._q0;}
            set {this._q0 = RestrictedDouble.AbsMax_1e20(value);}
        }
        
        /// <summary>
        /// Gets or sets the qh.
        /// </summary>
        [XmlAttribute("qh")]
        public double _qh; // abs_max_1e20
        [XmlIgnore]
        public double Qh
        {
            get {return this._qh;}
            set {this._qh = RestrictedDouble.AbsMax_1e20(value);}
        }

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
        public Geometry.Vector3d Direction { get; set; } // point_type_3c

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private PressureLoad()
        {

        }

        /// <summary>
        /// Pressure load
        /// </summary>
        /// <param name="region"></param>
        /// <param name="loadDirection">Vector. Direction of force.</param>
        /// <param name="z0">Surface level of soil/water (on the global Z axis).</param>
        /// <param name="q0">Load intensity at the surface level.</param>
        /// <param name="qh">Increment of load intensity per meter (along the global Z axis).</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        /// <param name="loadProjection"></param>
        /// <param name="loadType"></param>
        public PressureLoad(Geometry.Region region, Geometry.Vector3d loadDirection, double z0, double q0, double qh, LoadCase loadCase, string comment, bool loadProjection, ForceLoadType loadType)
        {
            // base
            this.EntityCreated();
            this.LoadCase = loadCase;
            this.Comment = comment;
            this.LoadProjection = loadProjection;
            this.LoadType = loadType;
            this.Region = region;
            this.Direction = loadDirection; 

            // specific
            this.Z0 = z0;
            this.Q0 = q0;
            this.Qh = qh;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}