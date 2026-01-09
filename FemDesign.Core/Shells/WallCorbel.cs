using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign.Shells
{
    /// <summary>
    /// Represents a Wall Corbel.
    /// </summary>
    [System.Serializable]
    public partial class WallCorbel: EntityBase, IStructureElement
    {
        /// <summary>
        /// Gets or sets the start point.
        /// </summary>
        [XmlElement("start_point", Order = 1)]
        public Geometry.Point3d StartPoint { get; set; }

        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        [XmlElement("end_point", Order = 2)]
        public Geometry.Point3d EndPoint { get; set; }

        /// <summary>
        /// Gets or sets the connectable parts.
        /// </summary>
        [XmlElement("connectable_parts", Order = 3)]
        public TwoGuidListType ConnectableParts { get; set; }
        
        // choice rigidity
        /// <summary>
        /// Gets or sets the rigidity.
        /// </summary>
        [XmlElement("rigidity", Order = 4)]
        public Releases.RigidityDataType3 Rigidity { get; set; }

        /// <summary>
        /// Gets or sets the predef rigidity ref.
        /// </summary>
        [XmlElement("predefined_rigidity", Order = 5)]
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

        // choice rigidity group
        // [XmlElement("rigidity_group", Order = 6)]

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlIgnore]
        public string Instance
        {
            get
            {
                var found = this.Name.IndexOf(".");
                return this.Name.Substring(found + 1);
            }
        }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Identifier => this.Name.Split('.')[0];


        /// <summary>
        /// Gets or sets the positive side.
        /// </summary>
        [XmlAttribute("positive_side")]
        public bool PositiveSide { get; set; } = true;

        /// <summary>
        /// Gets or sets the l.
        /// </summary>
        [XmlAttribute("l")]
        public double L { get; set; }

        /// <summary>
        /// Gets or sets the h1.
        /// </summary>
        [XmlAttribute("h1")]
        public double H1 { get; set; }

        /// <summary>
        /// Gets or sets the h2.
        /// </summary>
        [XmlAttribute("h2")]
        public double H2 { get; set; }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        [XmlAttribute("x")]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the base wall.
        /// </summary>
        [XmlAttribute("base_wall")]
        public System.Guid BaseWall { get; set; }

        /// <summary>
        /// Gets or sets the complex material.
        /// </summary>
        [XmlAttribute("complex_material")]
        public System.Guid ComplexMaterial { get; set; }
    }
}