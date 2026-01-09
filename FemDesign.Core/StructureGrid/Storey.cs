using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign.StructureGrid
{
    /// <summary>
    /// Represents a Storey.
    /// </summary>
    [System.Serializable]
    public partial class Storey: EntityBase, IStructureElement
    {
        /// <summary>
        /// Gets or sets the origo.
        /// </summary>
        [XmlElement("origo", Order=1)]
        public Geometry.Point3d Origo { get; set; }
        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        [XmlElement("direction", Order=2)]
        public Geometry.Vector2d _direction;
        [XmlIgnore]
        public Geometry.Vector3d Direction
        {
            get
            {
                return this._direction.To3d();
            }
            set
            {
                this._direction = RestrictedObject.NonZeroFdVector2d(value.Normalize().To2d());
            }
        }

        /// <summary>
        /// Gets or sets the dimension x.
        /// </summary>
        [XmlAttribute("dimension_x")]
        public double _dimensionX; // positive double
        [XmlIgnore]
        public double DimensionX
        {
            get { return this._dimensionX; }
            set { this._dimensionX = RestrictedDouble.Positive(value); }
        }
        /// <summary>
        /// Gets or sets the dimension y.
        /// </summary>
        [XmlAttribute("dimension_y")]
        public double _dimensionY; // positive double
        [XmlIgnore]
        public double DimensionY
        {
            get { return this._dimensionY; }
            set { this._dimensionY = RestrictedDouble.Positive(value); }
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private Storey()
        {

        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="origo">Origo of storey.</param>
        /// <param name="direction">Direction of storey x'-axis. Optional, default value isGCS x-axis.</param>
        /// <param name="dimensionX">Dimension in x'-direction.</param>
        /// <param name="dimensionY">Dimension in y'-direction.</param>
        /// <param name="name">Name of storey.</param>
        public Storey(Geometry.Point3d origo, Geometry.Vector3d direction, double dimensionX, double dimensionY, string name)
        {
            this.EntityCreated();
            this.Origo = origo;
            this.Direction = direction;
            this.DimensionX = dimensionX;
            this.DimensionY = dimensionY;
            this.Name = name;
        }
    }
}