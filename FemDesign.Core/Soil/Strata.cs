using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Drawing;

namespace FemDesign.Soil
{
    /// <summary>
    /// Represents a Strata.
    /// </summary>
    [System.Serializable]
    public partial class Strata : NamedEntityBase
    {
        [XmlIgnore]
        internal static int _instance = 0; // Shared instance counter for both PointSupport and LineSupport
        protected override int GetUniqueInstanceCount() => 1; // Only ONE instance can be created.

        /// <summary>
        /// Gets or sets the contour.
        /// </summary>
        [XmlElement("contour", Order = 1)]
        public Geometry.HorizontalPolygon2d Contour { get; set; }

        /// <summary>
        /// Gets or sets the stratum.
        /// </summary>
        [XmlElement("stratum", Order = 2)]
        public List<Stratum> Stratum { get; set; }

        /// <summary>
        /// Gets or sets the ground water.
        /// </summary>
        [XmlElement("water_level", Order = 3)]
        public List<GroundWater> _groundWater { get; set; }

        [XmlIgnore]
        public List<GroundWater> GroundWater
        {
            get
            {
                return _groundWater;
            }
            set
            {
                if( value.GroupBy(x => x.Name).Any(g => g.Count() > 1))
                {
                    throw new Exception("Duplicate Name found. WaterLevel names must be unique.");
                }
                _groundWater = value;
            }
        }


        /// <summary>
        /// Gets or sets the depth level limit.
        /// </summary>
        [XmlAttribute("depth_level_limit")]
        public double _depthLevelLimit { get; set; }

        [XmlIgnore]
        public double DepthLevelLimit
        {
            get
            {
                return this._depthLevelLimit;
            }
            set
            {
                this._depthLevelLimit = RestrictedDouble.ValueInClosedInterval(value, -1000000, 0);
            }
        }

        /// <remarks/>
        [XmlAttribute("default_fillings_colour")]
        public string _defaultFillingsColour { get; set; } = "B97A57";

        [XmlIgnore]
        public Color DefaultFillingsColour
        {
            get
            {
                Color col = System.Drawing.ColorTranslator.FromHtml("#" + this._defaultFillingsColour);
                return col;
            }
            set
            {
                this._defaultFillingsColour = ColorTranslator.ToHtml(value).Substring(1);
            }
        }


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Strata()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Strata"/> class.
        /// </summary>
        /// <param name="stratum">the stratum.</param>
        /// <param name="waterLevel">the water level.</param>
        /// <param name="contour">the contour.</param>
        /// <param name="levelLimit">the level limit.</param>
        /// <param name="identifier">the identifier.</param>
        public Strata(List<Stratum> stratum, List<GroundWater> waterLevel, List<Geometry.Point2d> contour, double levelLimit, string identifier = "SOIL")
        {
            this.Stratum = stratum;
            this.GroundWater = waterLevel;
            this.Contour = new Geometry.HorizontalPolygon2d(contour);
            this.DepthLevelLimit = levelLimit;
            this.Identifier = identifier;

            // Strata Object does not have a Guid. Therefore this.EntityCreated() should not be use
            this.EntityModified();
        }
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{this.GetType().Name} {this.Name}, Level Limit {DepthLevelLimit} [m], Stratum: {Stratum.Count} layer, Ground water: {GroundWater.Count} layer";
        }
    }

    /// <summary>
    /// Represents a Stratum.
    /// </summary>
    public partial class Stratum
    {
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        [XmlAttribute("material")]
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        [XmlIgnore]
        public Materials.Material _material { get; set; }

        [XmlIgnore]
        public Materials.Material Material
        {
            get
            {
                return _material;
            }
            set
            {
                if (value.Family != Materials.Family.Stratum)
                    throw new ArgumentException("Material should be type of Stratum!");
                this._material = value;
                this.Guid = this._material.Guid;
            }
        }

        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        [XmlAttribute("colour")]
        public string _colour { get; set; }
        [XmlIgnore]
        public Color? Color
        {
            get
            {
                Color col = System.Drawing.ColorTranslator.FromHtml("#" + this._colour);
                return col;
            }
            set
            {
                this._colour = ColorTranslator.ToHtml((Color)value).Substring(1);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Stratum() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stratum"/> class.
        /// </summary>
        /// <param name="soilMaterial">the soil material.</param>
        /// <param name="color">the color.</param>
        public Stratum(Materials.Material soilMaterial, Color? color = null)
        {
            this.Material = soilMaterial;
            if(color == null)
            {
                var rnd = new Random(Guid.NewGuid().GetHashCode());
                color = System.Drawing.Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            }
            this.Color = color;
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


    /// <summary>
    /// Represents a Ground Water.
    /// </summary>
    public partial class GroundWater
    {
        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        [XmlAttribute("colour")]
        public string _colour { get; set; }
        [XmlIgnore]
        public Color? Color
        {
            get
            {
                Color col = System.Drawing.ColorTranslator.FromHtml("#" + this._colour);
                return col;
            }
            set
            {
                this._colour = ColorTranslator.ToHtml((Color)value).Substring(1);
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }




        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private GroundWater() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroundWater"/> class.
        /// </summary>
        /// <param name="name">the name.</param>
        /// <param name="color">the color.</param>
        public GroundWater(string name, Color? color = null)
        {
            this.Name = name;
            if (color == null)
            {
                var rnd = new Random();
                color = System.Drawing.Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            }
            this.Color = color;
        }




    }
}