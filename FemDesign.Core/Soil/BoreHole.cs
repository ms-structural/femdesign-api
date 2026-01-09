using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using StruSoft.Interop.StruXml.Data;

namespace FemDesign.Soil
{
    /// <summary>
    /// Represents a Bore Hole.
    /// </summary>
    [System.Serializable]
    public partial class BoreHole : NamedEntityBase
    {
        [XmlIgnore]
        private static int _boreHoleInstances = 0;
        /// <summary>
        /// Reset Instance Count.
        /// </summary>
        public static void ResetInstanceCount() => _boreHoleInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_boreHoleInstances;

        /// <summary>
        /// Gets or sets the strata levels.
        /// </summary>
        [XmlElement]
        public List<double> StrataLevels { get; set; }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        [XmlAttribute("x")]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        [XmlAttribute("y")]
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the final ground level.
        /// </summary>
        [XmlAttribute("final_ground_level")]
        public double _finalGroundLevel { get; set; }

        [XmlIgnore]
        public double FinalGroundLevel
        {
            get
            {
                return _finalGroundLevel;
            }
            set
            {
                this._finalGroundLevel = RestrictedDouble.ValueInClosedInterval(value, -1e6, 10000);
            }
        }

        /// <summary>
        /// Gets or sets the whole level data.
        /// </summary>
        [XmlElement("whole_level_data")]
        public AllLevels WholeLevelData { get; set; }
        /// <summary>
        /// Gets or sets the colouring.
        /// </summary>
        [XmlElement("colouring")]
        public EntityColor Colouring { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private BoreHole() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoreHole"/> class.
        /// </summary>
        /// <param name="x">value for <paramref name="x"/>.</param>
        /// <param name="y">value for <paramref name="y"/>.</param>
        /// <param name="finalGroundLevel">the final ground level.</param>
        /// <param name="allLevels">the all levels.</param>
        /// <param name="identifier">the identifier.</param>
        public BoreHole(double x, double y, double finalGroundLevel = 0.00, AllLevels allLevels = null, string identifier = "BH")
        {
            this.X = x;
            this.Y = y;
            this.FinalGroundLevel = finalGroundLevel;
            this.WholeLevelData = allLevels;
            this.Identifier = identifier;
            this.EntityCreated();
        }

        /// <summary>
        /// Ground Level.
        /// </summary>
        /// <param name="x">value for <paramref name="x"/>.</param>
        /// <param name="y">value for <paramref name="y"/>.</param>
        /// <param name="finalGroundLevel">the final ground level.</param>
        /// <param name="identifier">the identifier.</param>
        /// <returns>The result.</returns>
        public static BoreHole GroundLevel(double x, double y, double finalGroundLevel, string identifier = "BH")
        {
            var boreHole = new BoreHole(x, y, finalGroundLevel, identifier: identifier);
            return boreHole;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            if(this.WholeLevelData != null)
                return $"{this.GetType().Name} {this.Name}, X: {this.X} Y: {this.Y}, FinalGroundLevel: {this.FinalGroundLevel} [m], Ground Level Depth: {this.WholeLevelData._strataTopLevels} [m], Water Levels Depth: {this.WholeLevelData._waterLevels} [m]";
            else
                return $"{this.GetType().Name} {this.Name}, X: {this.X} Y: {this.Y}, FinalGroundLevel: {this.FinalGroundLevel} [m]";
        }
    }

    /// <summary>
    /// Represents a All Levels.
    /// </summary>
    public partial class AllLevels
    {
        /// <summary>
        /// Gets or sets the strata top levels.
        /// </summary>
        [XmlElement("strata_top_levels")]
        public string _strataTopLevels;

        [XmlIgnore]
        public List<double> StrataTopLevels
        {
            get
            {
                if (_strataTopLevels != null)
                {
                    return _strataTopLevels.Split(' ').Select(x => double.Parse(x)).ToList();
                }
                else return null;
            }

            set
            {
                if (value != null)
                {
                    _strataTopLevels = String.Join(" ", value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the water levels.
        /// </summary>
        [XmlElement("water_levels")]
        public string _waterLevels;

        [XmlIgnore]
        public List<double> WaterLevels
        {
            get
            {
                if (_waterLevels != null)
                {
                    return _waterLevels.Split(' ').Select(x => double.Parse(x)).ToList();
                }
                else return null;
            }
            set
            {
                if(value != null)
                {
                    _waterLevels = String.Join(" ", value);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllLevels"/> class.
        /// </summary>
        public AllLevels() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllLevels"/> class.
        /// </summary>
        /// <param name="strataTopLevels">the strata top levels.</param>
        /// <param name="waterLevels">the water levels.</param>
        public AllLevels(List<double> strataTopLevels, List<double> waterLevels)
        {
            StrataTopLevels = strataTopLevels;
            WaterLevels = waterLevels;
        }
    }
}