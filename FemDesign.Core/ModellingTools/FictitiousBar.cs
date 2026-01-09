using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using StruSoft.Interop.StruXml.Data;


namespace FemDesign.ModellingTools
{
    /// <summary>
    /// Represents a Fictitious Bar.
    /// </summary>
    [System.Serializable]
    public partial class FictitiousBar: NamedEntityBase, IStructureElement
    {
        [XmlIgnore]
        private static int _ficticiousBarInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_ficticiousBarInstances;

        /// <summary>
        /// Gets or sets the edge.
        /// </summary>
        [XmlElement("edge", Order = 1)]
        public Geometry.Edge Edge { get; set; }

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
        /// Gets or sets the connectivity.
        /// </summary>
        [XmlElement("connectivity", Order = 3)]
        public Bars.Connectivity[] _connectivity = new Bars.Connectivity[2];

        /// <summary>
        /// Gets or sets the truss behaviour.
        /// </summary>
        [XmlElement("truss_behaviour", Order = 4)]
        public Simple_truss_chr_type TrussBehaviour { get; set; }

        /// <summary>
        /// Gets or sets the colouring.
        /// </summary>
        [XmlElement("colouring", Order = 5)]
        public EntityColor Colouring { get; set; }

        [XmlIgnore]
        public Bars.Connectivity StartConnectivity
        {
            get
            {
                return this._connectivity[0];
            }
            set
            {
                this._connectivity[0] = value;
            }
        }

        [XmlIgnore]
        public Bars.Connectivity EndConnectivity
        {
            get
            {
                return this._connectivity[1];
            }
            set
            {
                this._connectivity[1] = value;
            }
        }

        /// <summary>
        /// Gets or sets the ae.
        /// </summary>
        [XmlAttribute("AE")]
        public double _ae;

        [XmlIgnore]
        public double AE
        {
            get
            {
                return this._ae;
            }
            set
            {
                this._ae = RestrictedDouble.NonZeroMax_1e15(value);
            }
        }

        /// <summary>
        /// Gets or sets the mass.
        /// </summary>
        [XmlAttribute("unit_mass")]
        public double _mass;

        [XmlIgnore]
        public double Mass
        {
            get
            {
                return this._mass;
            }
            set
            {
                this._mass = RestrictedDouble.NonNegMax_1e7(value);
            }
        }

        /// <summary>
        /// Gets or sets the itg.
        /// </summary>
        [XmlAttribute("ItG")]
        public double _itg;

        /// <summary>
        /// Only for serialization purposes!
        /// </summary>
        /// <returns>The result.</returns>
        public bool ShouldSerialize_itg() => this.TrussBehaviour == null;

        [XmlIgnore]
        public double ItG
        {
            get
            {
                return this._itg;
            }
            set
            {
                this._itg = RestrictedDouble.NonZeroMax_1e15(value);
            }
        }

        /// <summary>
        /// Gets or sets the i1e.
        /// </summary>
        [XmlAttribute("I1E")]
        public double _i1e;

        /// <summary>
        /// Only for serialization purposes!
        /// </summary>
        /// <returns>The result.</returns>
        public bool ShouldSerialize_i1e() => this.TrussBehaviour == null;


        [XmlIgnore]
        public double I1E
        {
            get
            {
                return this._i1e;
            }
            set
            {
                this._i1e = RestrictedDouble.NonZeroMax_1e15(value);
            }
        }

        /// <summary>
        /// Gets or sets the i2e.
        /// </summary>
        [XmlAttribute("I2E")]
        public double _i2e;

        /// <summary>
        /// Only for serialization purposes!
        /// </summary>
        /// <returns>The result.</returns>
        public bool ShouldSerialize_i2e() => this.TrussBehaviour == null;

        [XmlIgnore]
        public double I2E
        {
            get
            {
                return this._i2e;
            }
            set
            {
                this._i2e = RestrictedDouble.NonZeroMax_1e15(value);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private FictitiousBar()
        {

        }

        /// <summary>
        /// Bended bar type FictitiousBar constructor.
        /// </summary>
        /// <param name="edge">the edge.</param>
        /// <param name="localY">the local y.</param>
        /// <param name="startConnectivity">the start connectivity.</param>
        /// <param name="endConnectivity">the end connectivity.</param>
        /// <param name="identifier">the identifier.</param>
        /// <param name="ae">the ae.</param>
        /// <param name="itg">the itg.</param>
        /// <param name="i1e">the i1e.</param>
        /// <param name="i2e">the i2e.</param>
        /// <param name="mass">the mass.</param>
        public FictitiousBar(Geometry.Edge edge, Geometry.Vector3d localY, Bars.Connectivity startConnectivity, Bars.Connectivity endConnectivity, string identifier = "BF", double ae = 1e+07, double itg = 1e+07, double i1e = 1e+07, double i2e = 1e+07, double mass = 0.1)
        {
            this.EntityCreated();
            this.Edge = edge;
            this.LocalY = localY;
            this.StartConnectivity = startConnectivity;
            this.EndConnectivity = endConnectivity;
            this.Identifier = identifier;
            this.AE = ae;
            this.ItG = itg;
            this.I1E = i1e;
            this.I2E = i2e;
            this.Mass = mass;
        }

        /// <summary>
        /// Truss type FictitiousBar constructor.
        /// </summary>
        /// <param name="edge">the edge.</param>
        /// <param name="localY">the local y.</param>
        /// <param name="identifier">the identifier.</param>
        /// <param name="ae">the ae.</param>
        /// <param name="mass">the mass.</param>
        /// <param name="trussBehaviour">If null, elastic behaviour is set.</param>
        public FictitiousBar(Geometry.Edge edge, Geometry.Vector3d localY, string identifier, double ae = 1e+07, double mass = 0.1, Simple_truss_chr_type trussBehaviour = null)
        {
            this.EntityCreated();
            this.Edge = edge;
            this.LocalY = localY;
            this.Identifier = identifier;
            this.AE = ae;
            this.Mass = mass;
            this.TrussBehaviour = trussBehaviour ?? Simple_truss_chr_type.Elastic();
        }

        /// <summary>
        /// Orient this object's coordinate system to GCS.
        /// </summary>
        public void OrientCoordinateSystemToGCS()
        {
            var cs = this.Plane;
            cs.AlignYAroundXToGcs();
            this.Plane = cs;
        }

        /// <summary>
        /// Create truss behaviour for FictitiousBar objects.
        /// </summary>
        /// <param name="compBehav">Default is elastic truss behaviour. If `Elastic` is set, `compLimit` is ignored.</param>
        /// <param name="tensBehav">Default is elastic truss behaviour. If `Elastic` is set, `tensLimit` is ignored.</param>
        /// <param name="compLimit">Compression limit force value [kN]. Ignored if `compBehav` is set to `Elastic`.</param>
        /// <param name="tensLimit">Tension limit force value [kN]. Ignored if `tensBehav` is set to `Elastic`.</param>
        /// <returns></returns>
        public static Simple_truss_chr_type SetTrussBehaviour(ItemChoiceType1 compBehav = ItemChoiceType1.Elastic, ItemChoiceType1 tensBehav = ItemChoiceType1.Elastic, double compLimit = 1e+15, double tensLimit = 1e+15)
        {
            // compression
            Simple_truss_behaviour_type compression;
            if (compBehav is ItemChoiceType1.Elastic)
            {
                compression = Simple_truss_behaviour_type.Elastic();
            }
            else
            {
                compression = new Simple_truss_behaviour_type(new Simple_truss_capacity_type(compLimit), compBehav);
            }

            // tension
            Simple_truss_behaviour_type tension;
            if (tensBehav is ItemChoiceType1.Elastic)
            {
                tension = Simple_truss_behaviour_type.Elastic();
            }
            else
            {
                tension = new Simple_truss_behaviour_type(new Simple_truss_capacity_type(tensLimit), tensBehav);
            }

            return new Simple_truss_chr_type(compression, tension);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            if(TrussBehaviour is null)
            {
                return $"Bended Bar, Start: {Edge.Points.First()}, End: {Edge.Points.Last()}, Length: {Edge.Length} m, AE: {AE} kN, Mass: {Mass} t/m, ItG: {ItG} kNm2, I1E: {I1E} kNm2, I2E: {I2E} kNm2;";
            }
            else
            {
                return $"Truss, Start: {Edge.Points.First()}, End: {Edge.Points.Last()}, Length: {Edge.Length} m, AE: {AE} kN, Mass: {Mass} t/m;";
            }
        }
    }
}