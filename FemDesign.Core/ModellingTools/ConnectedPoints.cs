using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using FemDesign.GenericClasses;
using FemDesign.Releases;
using FemDesign.Geometry;


namespace FemDesign.ModellingTools
{
    /// <summary>
    /// Represents a Connected Points.
    /// </summary>
    [System.Serializable]
    public partial class ConnectedPoints : NamedEntityBase, IStructureElement
    {
        [XmlIgnore]
        private static int _connectedPointInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_connectedPointInstances;

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        [XmlElement("point", Order = 1)]
        public Point3d[] _points;

        [XmlIgnore]
        public Point3d[] Points
        {
            get
            {
                return this._points;
            }
            set
            {
                if (value.Length == 2)
                {
                    this._points = value;
                }
                else
                {
                    throw new System.ArgumentException($"Length of points: {value.Length}, should be 2.");
                }
            }
        }

        [XmlIgnore]
        private Geometry.Plane _plane;

        [XmlIgnore]
        public Geometry.Plane Plane
        {
            get
            {
                if (this._plane == null)
                {
                    this._plane = new Geometry.Plane(new Point3d(0, 0, 0), this.LocalX, this.LocalY);
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
                this._localX = value.LocalX;
                this._localY = value.LocalY;
            }
        }

        /// <summary>
        /// Gets or sets the local x.
        /// </summary>
        [XmlElement("local_x", Order = 2)]
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
                this._localY = this.Plane.LocalY;
            }
        }

        /// <summary>
        /// Gets or sets the local y.
        /// </summary>
        [XmlElement("local_y", Order = 3)]
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
                this.Plane.SetYAroundZ(value);
                this._localX = this.Plane.LocalX;
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
            set
            {
                this.Plane.SetZAroundX(value);
                this._localX = this.Plane.LocalX;
                this._localY = this.Plane.LocalY;
            }
        }

        // rigidity data choice

        /// <summary>
        /// Gets or sets the rigidity.
        /// </summary>
        [XmlElement("rigidity", Order = 4)]
        public RigidityDataType2 Rigidity { get; set; }

        /// <summary>
        /// Gets or sets the predef rigidity ref.
        /// </summary>
        [XmlElement("predefined_rigidity", Order = 5)]
        public GuidListType _predefRigidityRef;

        /// <summary>
        /// Gets or sets the predef rigidity.
        /// </summary>
        [XmlIgnore]
        public RigidityDataLibType2 _predefRigidity;

        [XmlIgnore]
        public RigidityDataLibType2 PredefRigidity
        {
            get
            {
                return this._predefRigidity;
            }
            set
            {
                this._predefRigidity = value;
                this._predefRigidityRef = new GuidListType(value.Guid);
            }
        }

        // rigidity group choice


        /// <summary>
        /// Gets or sets the references.
        /// </summary>
        [XmlElement("ref", Order = 6)]
        public GuidListType[] References { get; set; }

        /// <summary>
        /// Gets or sets the colouring.
        /// </summary>
        [XmlElement("colouring", Order = 7)]
        public EntityColor Colouring { get; set; }

        /// <summary>
        /// Gets or sets the interface.
        /// </summary>
        [XmlAttribute("interface")]
        public double _interface;

        [XmlIgnore]
        public double Interface
        {
            get
            {
                return this._interface;
            }
            set
            {
                this._interface = RestrictedDouble.NonNegMax_1(value);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private ConnectedPoints()
        {

        }

        /// <summary>
        /// Create a connected point between 2 points using rigidity. 
        /// </summary>
        /// <param name="firstPoint">the first point.</param>
        /// <param name="secondPoint">the second point.</param>
        /// <param name="rigidity">the rigidity.</param>
        /// <param name="references">the references.</param>
        /// <param name="identifier">the identifier.</param>
        public ConnectedPoints(Point3d firstPoint, Point3d secondPoint, RigidityDataType2 rigidity, GuidListType[] references, string identifier = "CP")
        {
            Initialize(firstPoint, secondPoint, rigidity, references, identifier);
        }

        /// <summary>
        /// Create a connected point between 2 points with rigidity (motions, rotations). 
        /// </summary>
        /// <param name="firstPoint">First connected point</param>
        /// <param name="secondPoint">Second connected point</param>
        /// <param name="motions">Motions rigidity</param>
        /// <param name="rotations">Rotations rigidity</param>
        /// <param name="references">Reference element</param>
        /// <param name="identifier">Name of connected point</param>
        public ConnectedPoints(Point3d firstPoint, Point3d secondPoint, Motions motions, Rotations rotations, IEnumerable<EntityBase> references, string identifier = "CP")
        {
            GuidListType[] refs = references.Select(r => new GuidListType(r)).ToArray();
            RigidityDataType2 rigidity = new RigidityDataType2(motions, rotations);
            Initialize(firstPoint, secondPoint, rigidity, refs, identifier);
        }

        /// <summary>
        /// Create a connected point between 2 points with rigidity (motions, rotations). 
        /// </summary>
        /// <param name="firstPoint">the first point.</param>
        /// <param name="secondPoint">the second point.</param>
        /// <param name="motions">the motions.</param>
        /// <param name="rotations">the rotations.</param>
        /// <param name="references">the references.</param>
        /// <param name="identifier">the identifier.</param>
        public ConnectedPoints(Point3d firstPoint, Point3d secondPoint, Motions motions, Rotations rotations, GuidListType[] references, string identifier = "CP")
        {
            RigidityDataType2 rigidity = new RigidityDataType2(motions, rotations);
            Initialize(firstPoint, secondPoint, rigidity, references, identifier);
        }

        /// <summary>
        /// Create a connected point between 2 points with rigidity (motions, rotations) and plastic limits (forces, moments). 
        /// </summary>
        /// <param name="firstPoint">the first point.</param>
        /// <param name="secondPoint">the second point.</param>
        /// <param name="motions">the motions.</param>
        /// <param name="motionsPlasticLimits">the motions plastic limits.</param>
        /// <param name="rotations">the rotations.</param>
        /// <param name="rotationsPlasticLimits">the rotations plastic limits.</param>
        /// <param name="references">the references.</param>
        /// <param name="identifier">the identifier.</param>
        public ConnectedPoints(Point3d firstPoint, Point3d secondPoint, Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits, IEnumerable<EntityBase> references, string identifier = "CP")
        {
            GuidListType[] refs = references.Select(r => new GuidListType(r)).ToArray();
            RigidityDataType2 rigidity = new RigidityDataType2(motions, motionsPlasticLimits, rotations, rotationsPlasticLimits);
            Initialize(firstPoint, secondPoint, rigidity, refs, identifier);
        }

        /// <summary>
        /// Create a connected point between 2 points with rigidity (motions, rotations) and plastic limits (forces, moments). 
        /// </summary>
        /// <param name="firstPoint">the first point.</param>
        /// <param name="secondPoint">the second point.</param>
        /// <param name="motions">the motions.</param>
        /// <param name="motionsPlasticLimits">the motions plastic limits.</param>
        /// <param name="rotations">the rotations.</param>
        /// <param name="rotationsPlasticLimits">the rotations plastic limits.</param>
        /// <param name="references">the references.</param>
        /// <param name="identifier">the identifier.</param>
        public ConnectedPoints(Point3d firstPoint, Point3d secondPoint, Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits, GuidListType[] references, string identifier = "CP")
        {
            RigidityDataType2 rigidity = new RigidityDataType2(motions, motionsPlasticLimits, rotations, rotationsPlasticLimits);
            Initialize(firstPoint, secondPoint, rigidity, references, identifier);
        }

        /// <summary>
        /// Create a connected point between 2 points with coordinate system and rigidity (motions, rotations). 
        /// </summary>
        /// <param name="plane">the plane.</param>
        /// <param name="firstPoint">the first point.</param>
        /// <param name="secondPoint">the second point.</param>
        /// <param name="motions">the motions.</param>
        /// <param name="rotations">the rotations.</param>
        /// <param name="references">the references.</param>
        /// <param name="identifier">the identifier.</param>
        public ConnectedPoints(Plane plane, Point3d firstPoint, Point3d secondPoint, Motions motions, Rotations rotations, GuidListType[] references, string identifier = "CP")
        {
            RigidityDataType2 rigidity = new RigidityDataType2(motions, rotations);
            //InitializeWithCoordinates(plane, firstPoint, secondPoint, rigidity, references, identifier);
            Initialize(firstPoint, secondPoint, rigidity, references, identifier);
            this.LocalX = plane.LocalX;
            this.LocalY = plane.LocalY;
        }

        /// <summary>
        /// Create a connected point between 2 points with coordinate system, rigidity (motions, rotations) and plastic limits (forces, moments). 
        /// </summary>
        /// <param name="plane">the plane.</param>
        /// <param name="firstPoint">the first point.</param>
        /// <param name="secondPoint">the second point.</param>
        /// <param name="motions">the motions.</param>
        /// <param name="motionsPlasticLimits">the motions plastic limits.</param>
        /// <param name="rotations">the rotations.</param>
        /// <param name="rotationsPlasticLimits">the rotations plastic limits.</param>
        /// <param name="references">the references.</param>
        /// <param name="identifier">the identifier.</param>
        public ConnectedPoints(Plane plane, Point3d firstPoint, Point3d secondPoint, Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits, GuidListType[] references, string identifier = "CP")
        {
            RigidityDataType2 rigidity = new RigidityDataType2(motions, motionsPlasticLimits, rotations, rotationsPlasticLimits);
            Initialize(firstPoint, secondPoint, rigidity, references, identifier);
            this.Plane = plane;

        }

        private void Initialize(Point3d firstPoint, Point3d secondPoint, RigidityDataType2 rigidity, GuidListType[] references, string identifier)
        {
            this.EntityCreated();

            this.Points = new Point3d[2]
            {
                firstPoint,
                secondPoint
            };
            this.Plane = new Plane(new Point3d(0, 0, 0), Vector3d.UnitX, Vector3d.UnitY);
            this.LocalY = Vector3d.UnitY;
            this.Rigidity = rigidity;
            this.References = references;
            this.Identifier = identifier;
        }        
    }
}
