// https://strusoft.com/
using System;
using System.Xml.Serialization;
using FemDesign.Releases;

namespace FemDesign.Supports
{
    /// <summary>
    /// support_rigidity_data_type --> group
    /// </summary>
    [System.Serializable]
    public partial class Group
    {
        [XmlIgnore]
        private Geometry.Plane _plane;

        [XmlIgnore]
        private Geometry.Plane Plane
        {
            get
            {
                // if deserialized from file the cooridnate system element does not exist and has to be created from local x and local y fields.
                if (this._plane == null)
                {
                    this._plane = new Geometry.Plane(Geometry.Point3d.Origin, this._localX, this._localY);
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
        [XmlElement("local_x", Order = 1)]
        public Geometry.Vector3d _localX;

        [XmlIgnore]
        public Geometry.Vector3d LocalX
        {
            get
            {
                return this._localX;
            }
        }

        /// <summary>
        /// Gets or sets the local y.
        /// </summary>
        [XmlElement("local_y", Order = 2)]
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
        /// Gets or sets the rigidity.
        /// </summary>
        [XmlElement("rigidity", Order = 3)]
        public RigidityDataType2 Rigidity { get; set; }

        /// <summary>
        /// Gets or sets the predef rigidity ref.
        /// </summary>
        [XmlElement("predefined_rigidity", Order = 4)]
        public GuidListType _predefRigidityRef; // reference_type

        /// <summary>
        /// Gets or sets the predef rigidity.
        /// </summary>
        [XmlIgnore]
        public Releases.RigidityDataLibType2 _predefRigidity;

        [XmlIgnore]
        public Releases.RigidityDataLibType2 PredefRigidity
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

        /// <summary>
        /// Gets or sets the rigidity group.
        /// </summary>
        [XmlElement("rigidity_group", Order = 5)]
        public StruSoft.Interop.StruXml.Data.Rigidity_group_type2 RigidityGroup { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Group()
        {
            
        }

        /// <summary>
        /// Constructor by edge and rigidity. Used to create group for line support
        /// </summary>
        internal Group(Geometry.Plane plane, Motions motions, Rotations rotations)
        {
            this.Plane = plane;
            this.Rigidity = new RigidityDataType3(motions, rotations);
        }

        /// <summary>
        /// Constructor by edge and rigidity. Used to create group for line support
        /// </summary>
        internal Group(Geometry.Plane plane, Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits)
        {
            this.Plane = plane;
            this.Rigidity = new RigidityDataType3(motions, motionsPlasticLimits, rotations, rotationsPlasticLimits);
        }


        /// <summary>
        /// Constructor by edge and rigidity. Used to create group for point support
        /// </summary>
        internal Group(Geometry.Vector3d localX, Geometry.Vector3d localY, Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits)
        {
            this._localX = localX;
            this._localY = localY;
            this.Rigidity = new RigidityDataType3(motions, motionsPlasticLimits, rotations, rotationsPlasticLimits);
        }

        /// <summary>
        /// Constructor by vectors and rigidity. Used to create group for point support
        /// </summary>
        /// <param name="localX">the local x.</param>
        /// <param name="localY">the local y.</param>
        /// <param name="motions">the motions.</param>
        /// <param name="rotations">the rotations.</param>
        public Group(Geometry.Vector3d localX, Geometry.Vector3d localY, Motions motions, Rotations rotations)
        {
            this._localX = localX;
            this._localY = localY;
            this.Rigidity = new RigidityDataType3(motions, rotations);
        }

        /// <summary>
        /// Orient this object's coordinate system to GCS
        /// </summary>
        public void OrientCoordinateSystemToGCS()
        {
            var cs = this.Plane;
            cs.AlignYAroundXToGcs();
            this.Plane = cs;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"x: {this.LocalX}, y: {this.LocalY}, z: {this.LocalZ}";
        }
    }
}