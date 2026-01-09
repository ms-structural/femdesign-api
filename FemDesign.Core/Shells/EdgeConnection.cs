// https://strusoft.com/

using System.Globalization;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.Releases;

namespace FemDesign.Shells
{
    /// <summary>
    /// Represents a Edge Connection.
    /// </summary>
    [System.Serializable]
    public partial class EdgeConnection : EdgeConnectionBase
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public bool Release { get; set; } = true;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; } // identifier
        /// <summary>
        /// Gets or sets the rigidity.
        /// </summary>
        [XmlElement("rigidity")]
        public Releases.RigidityDataType3 Rigidity { get; set; } // rigidity_data_type2(3?)
        /// <summary>
        /// Gets or sets the predef rigidity ref.
        /// </summary>
        [XmlElement("predefined_rigidity")]
        public GuidListType _predefRigidityRef; // reference_type
        /// <summary>
        /// Gets or sets the predef rigidity.
        /// </summary>
        [XmlIgnore]
        public Releases.RigidityDataLibType3 _predefRigidity;
        [XmlIgnore]
        public Releases.RigidityDataLibType3 PredefRigidity
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
        [XmlElement("rigidity_group")]
        public StruSoft.Interop.StruXml.Data.Rigidity_group_type2 RigidityGroup { get; set; }

        /// <summary>
        /// Gets or sets the edge.
        /// </summary>
        [XmlIgnore]
        public FemDesign.Geometry.Edge Edge { get; set; }

        /// <summary>
        /// Gets or sets the normal.
        /// </summary>
        [XmlIgnore]
        public FemDesign.Geometry.Vector3d Normal { get; set; }

        /// <summary>
        /// Library name of the edge connection.
        /// </summary>
        [XmlIgnore]
        public string LibraryName => PredefRigidity?.Name;
        /// <summary>
        /// Should the edge connection be added to the model as a Predefined/Library item or not?
        /// </summary>
        [XmlIgnore]
        public bool IsLibraryItem => !(PredefRigidity is null) && (Rigidity is null);
        /// <summary>
        /// Edge connection is a custom item if it is not in the library (predefined line connections library).
        /// </summary>
        [XmlIgnore]
        public bool IsCustomItem => !IsLibraryItem;

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private EdgeConnection()
        {

        }

        /// <summary>
        /// Private constructor.
        /// </summary>
        private EdgeConnection(Releases.RigidityDataType3 rigidity, string libraryName = null)
        {
            this.EntityCreated();
            this.MovingLocal = false;
            this.JoinedStartPoint = true;
            this.JoinedEndPoint = true;

            if (libraryName is null)
                this.Rigidity = rigidity;
            else
                this.PredefRigidity = new Releases.RigidityDataLibType3(rigidity, libraryName);
        }

        /// <summary>
        /// Copy properties from a EdgeConnection with a new name.
        /// </summary>
        internal static EdgeConnection CopyExisting(EdgeConnection shellEdgeConnection, string name)
        {
            // deep clone. downstreams objs contain changes made in this method, upstream objs will not.
            EdgeConnection ec = shellEdgeConnection.DeepClone();

            // downstream and uppstream objs will NOT share guid.
            ec.EntityCreated();

            //
            ec.Name = name;

            // return
            return ec;
        }

        /// <summary>
        /// EdgeConnection
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="motions">Motions.</param>
        /// <param name="rotations">Rotations.</param>
        /// <param name="libraryName">When <paramref name="libraryName"/> is not null or empty, the <see cref="EdgeConnection"/> will be treated as a <em>predefined/library</em> item. Default is to treat is a a unique <em>custom</em> edge connection.</param>
        public EdgeConnection(Releases.Motions motions, Releases.Rotations rotations, string libraryName = null) : this(new Releases.RigidityDataType3(motions, rotations), libraryName)
        {
        }

        /// <summary>
        /// EdgeConnection
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="motions">Motions.</param>
        /// <param name="motionsPlasticLimits">Motions plastic limit forces</param>
        /// <param name="rotations">Rotations.</param>
        /// <param name="rotationsPlasticLimits">Rotations plastic limit forces</param>
        /// <param name="libraryName">When <paramref name="libraryName"/> is not null or empty, the <see cref="EdgeConnection"/> will be treated as a <em>predefined/library</em> item. Default is to treat is a a unique <em>custom</em> edge connection.</param>
        public EdgeConnection(Releases.Motions motions, Releases.MotionsPlasticLimits motionsPlasticLimits, Releases.Rotations rotations, Releases.RotationsPlasticLimits rotationsPlasticLimits, string libraryName = null) : this(new Releases.RigidityDataType3(motions, motionsPlasticLimits, rotations, rotationsPlasticLimits), libraryName)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeConnection"/> class.
        /// </summary>
        /// <param name="motions">the motions.</param>
        /// <param name="motionsPlasticLimits">the motions plastic limits.</param>
        /// <param name="rotations">the rotations.</param>
        /// <param name="rotationsPlasticLimits">the rotations plastic limits.</param>
        /// <param name="friction">the friction.</param>
        /// <param name="detachType">the detach type.</param>
        /// <param name="libraryName">the library name.</param>
        public EdgeConnection(Releases.Motions motions, Releases.MotionsPlasticLimits motionsPlasticLimits, Releases.Rotations rotations, Releases.RotationsPlasticLimits rotationsPlasticLimits, double friction, DetachType detachType, string libraryName = null) : this(new Releases.RigidityDataType3(motions, motionsPlasticLimits, rotations, rotationsPlasticLimits, friction, detachType), libraryName)
        {

        }

        /// <summary>
        /// Create a default (rigid) EdgeConnection.
        /// </summary>
        public static EdgeConnection Default => EdgeConnection.Rigid;

        /// <summary>
        /// Create a hinged EdgeConnection.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        public static EdgeConnection Hinged
        {
            get
            {
                EdgeConnection _shellEdgeConnection = new EdgeConnection(Releases.RigidityDataType3.HingedLine());
                _shellEdgeConnection.Release = true;
                return _shellEdgeConnection;

            }
        }
        
        /// <summary>
        /// Create a rigid EdgeConnection.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        public static EdgeConnection Rigid
        {
            get
            {
                EdgeConnection _shellEdgeConnection = new EdgeConnection(Releases.RigidityDataType3.RigidLine());
                _shellEdgeConnection.Release = false;
                return _shellEdgeConnection;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            if(!this.IsLibraryItem)
                return $"{this.GetType().Name} {this.Rigidity.Motions} {this.Rigidity.Rotations}";
            else
                return $"{this.GetType().Name} {this.PredefRigidity.Rigidity.Motions} {this.PredefRigidity.Rigidity.Rotations} Library: {this.LibraryName}";
        }
    }
}