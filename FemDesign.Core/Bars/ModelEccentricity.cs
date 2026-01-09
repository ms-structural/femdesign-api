// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Bars
{
    /// <summary>
    /// Represents a Model Eccentricity.
    /// 
    /// This class is called ModelEccentricity as Eccentricity (ecc_value_type) is the Dynamo facing class and thus need to be named accordingly.
    /// </summary>
    [System.Serializable]
    public partial class ModelEccentricity
    {
        // attributes
        /// <summary>
        /// Gets or sets the use default physical alignment.
        /// </summary>
        [XmlAttribute("use_default_physical_alignment")]
        public bool UseDefaultPhysicalAlignment { get; set; } // bool

        // elements
        /// <summary>
        /// Gets or sets the analytical.
        /// </summary>
        [XmlElement("analytical", Order = 1)]
        public Eccentricity[] _analytical = new Eccentricity[2]; // ecc_value_type

        [XmlIgnore]
        public Eccentricity[] Analytical
        {
            get
            {
                return this._analytical;
            }
            set
            {
                if (value.Length == 1)
                {
                    this._analytical[0] = value[0];
                    this._analytical[1] = value[0];
                }
                else if (value.Length == 2)
                {
                    this._analytical[0] = value[0];
                    this._analytical[1] = value[1];                  
                }
                else
                {
                    throw new System.ArgumentException($"Incorrect length of value: {value.Length}. Length should be 1 or 2");
                }
            }
        }

        [XmlIgnore]
        public Eccentricity StartAnalytical
        {
            get
            {
                return this._analytical[0];
            }
            set
            {
                this._analytical[0] = value;
            }
        }

        [XmlIgnore]
        public Eccentricity EndAnalytical
        {
            get
            {
                return this._analytical[1];
            }
            set
            {
                this._analytical[1] = value;
            }
        }

        /// <summary>
        /// Gets or sets the physical.
        /// </summary>
        [XmlElement("physical", Order = 2)]
        public Eccentricity[] _physical = new Eccentricity[2]; // ecc_value_type

        [XmlIgnore]
        public Eccentricity[] Physical
        {
            get
            {
                return this._physical;
            }
            set
            {
                if (value.Length == 1)
                {
                    this._physical[0] = value[0];
                    this._physical[1] = value[0];
                }
                else if (value.Length == 2)
                {
                    this._physical[0] = value[0];
                    this._physical[1] = value[1];                  
                }
                else
                {
                    throw new System.ArgumentException($"Incorrect length of value: {value.Length}. Length should be 1 or 2");
                }
            }
        }

        [XmlIgnore]
        public Eccentricity StartPhysical
        {
            get
            {
                return this._physical[0];
            }
            set
            {
                this._physical[0] = value;
            }
        }

        [XmlIgnore]
        public Eccentricity EndPhysical
        {
            get
            {
                return this._physical[1];
            }
            set
            {
                this._physical[1] = value;
            }
        }
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ModelEccentricity()
        {
            
        }

        /// <summary>
        /// Construct uniform ModelEccentricity with presets from Eccentricity.
        /// </summary>
        /// <param name="eccentricity">the eccentricity.</param>
        /// <param name="useDefaultPhysicalAlignment">the use default physical alignment.</param>
        public ModelEccentricity(Eccentricity eccentricity, bool useDefaultPhysicalAlignment = false)
        {
            this.UseDefaultPhysicalAlignment = useDefaultPhysicalAlignment;
            Eccentricity[] eccentricities = new Eccentricity[2]{eccentricity, eccentricity};
            this._analytical = eccentricities;
            this._physical = eccentricities;
        }

        /// <summary>
        /// Construct non-uniform ModelEccentricity with presets from Eccentricity.
        /// </summary>
        /// <param name="startEccentricity">the start eccentricity.</param>
        /// <param name="endEccentricity">the end eccentricity.</param>
        /// <param name="useDefaultPhysicalAlignment">the use default physical alignment.</param>
        public ModelEccentricity(Eccentricity startEccentricity, Eccentricity endEccentricity, bool useDefaultPhysicalAlignment = false)
        {
            this.UseDefaultPhysicalAlignment = useDefaultPhysicalAlignment;
            this._analytical[0] = startEccentricity;
            this._analytical[1] = endEccentricity;
            this._physical[0] = startEccentricity;
            this._physical[1] = endEccentricity;
        }
    }
}