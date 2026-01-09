using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Surface Temperature Load.
    /// </summary>
    [System.Serializable]
    public partial class SurfaceTemperatureLoad: LoadBase
    {
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        [XmlElement("region", Order=1)]
        public Geometry.Region Region { get; set; }

        [XmlIgnore]
        public Geometry.Vector3d LocalZ
        {
            get
            {
                return this.Region.LocalZ;
            }
            set
            {
                this.Region.LocalZ = value;
            }
        }

        /// <summary>
        /// Gets or sets the temperature.
        /// </summary>
        [XmlElement("temperature", Order=2)]
        public List<TopBotLocationValue> _temperature;

        [XmlIgnore]
        [Obsolete("Temperature is OBSOLETE. Use `TemperatureValues` instead")]
        public List<TopBotLocationValue> Temperature
        {
            get
            {
                return this._temperature;
            }
            set
            {
                if (value.Count == 1 || value.Count == 3)
                {
                    this._temperature = value;
                }
                else
                {
                    throw new System.ArgumentException($"Length of list is: {value.Count}, expected 1 or 3");
                }
            }
        }

        /// <summary>
        /// Gets or sets the temperature values.
        /// </summary>
        [XmlElement("temperature_values", Order = 3)]
        public List<TopBotLocationValue> _temperatureValues;

        [XmlIgnore]
        public List<TopBotLocationValue> TemperatureValues
        {
            get
            {
                return this._temperatureValues;
            }
            set
            {
                if (value.Count == 1 || value.Count == 3)
                {
                    this._temperatureValues = value;
                }
                else
                {
                    throw new System.ArgumentException($"Length of list is: {value.Count}, expected 1 or 3");
                }
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private SurfaceTemperatureLoad()
        {

        }

        /// <summary>
        /// Construct a surface temperature load by region and temperature location values (top/bottom)
        /// </summary>
        /// <param name="region">Region</param>
        /// <param name="direction">Direction of load</param>
        /// <param name="tempLocValue">List of top bottom location value. List should have 1 or 3 elements.></param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        public SurfaceTemperatureLoad(Geometry.Region region, Geometry.Vector3d direction, List<TopBotLocationValue> tempLocValue, LoadCase loadCase, string comment)
        {
            this.EntityCreated();
            this.Region = region;
            this.LocalZ = direction;
            this.TemperatureValues = tempLocValue;
            this.LoadCase = loadCase;
            this.Comment = comment;
        }
        
        /// <summary>
        /// Construct surface temperature load by region, top value and bottom value.
        /// </summary>
        /// <param name="region">Region</param>
        /// <param name="direction">Direction of load</param>
        /// <param name="topVal">Top value, temperature in celsius</param>
        /// <param name="bottomVal">Bottom value, temperature in celsius</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        public SurfaceTemperatureLoad(Geometry.Region region, Geometry.Vector3d direction, double topVal, double bottomVal, LoadCase loadCase, string comment)
        {
            this.EntityCreated();
            this.Region = region;
            this.LocalZ = direction;
            this.TemperatureValues = new List<TopBotLocationValue>{new TopBotLocationValue(region.Plane.Origin, topVal, bottomVal)};
            this.LoadCase = loadCase;
            this.Comment = comment;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            if (Temperature!= null && Temperature.Any())
            {
                if (Temperature.Count == 1)
                {
                    return $"{this.GetType().Name} - {this.Temperature[0]}";
                }
                else
                {
                    return $"{this.GetType().Name} - First:{this.Temperature[0]}  Second:{this.Temperature[1]}  Third:{this.Temperature[2]}";
                }
            }
            else
            {
                if (TemperatureValues.Count == 1)
                {
                    return $"{this.GetType().Name} - {this.TemperatureValues[0]}";
                }
                else
                {
                    return $"{this.GetType().Name} - First:{this.TemperatureValues[0]}  Second:{this.TemperatureValues[1]}  Third:{this.TemperatureValues[2]}";
                }
            }

        }
    }
}