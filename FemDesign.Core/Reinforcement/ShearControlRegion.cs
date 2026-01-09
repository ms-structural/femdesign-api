using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Reinforcement
{

    /// <summary>
    /// Represents a Shear Control Auto Type.
    /// </summary>
    [System.Serializable]
    public partial class ShearControlAutoType
    {
        /// <summary>
        /// Gets or sets the connected structure guid.
        /// </summary>
        [XmlAttribute("connected_structure")]
        public Guid ConnectedStructureGuid { get; set; }
    }

    /// <summary>
    /// Represents a Shear Control Region Type.
    /// </summary>
    [System.Serializable]
    public partial class ShearControlRegionType : EntityBase
    {
        /// <summary>
        /// Gets or sets the automatic.
        /// </summary>
        [XmlElement("automatic", Order = 1)]
        public ShearControlAutoType Automatic { get; set; }

        /// <summary>
        /// Gets or sets the contour.
        /// </summary>
        [XmlElement("contour", Order = 2)]
        public Geometry.Contour Contour { get; set; }

        /// <summary>
        /// Gets or sets the base plate.
        /// </summary>
        [XmlAttribute("base_plate")]
        public Guid BasePlate { get; set; }

        /// <summary>
        /// Gets or sets the ignore shear check.
        /// </summary>
        [XmlAttribute("ignore_shear_check")]
        public bool IgnoreShearCheck { get; set; }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        [XmlAttribute("x")]
        public double _x { get; set; }

        [XmlIgnore]
        public double X
        {
            get { return this._x; }
            set { this._x = RestrictedDouble.NonNegMax_20(value); }
        }

        /// <summary>
        /// Gets or sets the physical extension.
        /// </summary>
        [XmlAttribute("physical_extension")]
        public double _physicalExtension { get; set; } = 0.01;

        [XmlIgnore]
        public double PhysicalExtension
        {
            get { return this._physicalExtension; }
            set { this._physicalExtension = RestrictedDouble.ValueInClosedInterval(0.01, 100, value); }
        }

        /// <summary>
        /// Gets or sets the reduce shear forces.
        /// </summary>
        [XmlAttribute("reduce_shear_forces")]
        public bool ReduceShearForces { get; set; }

        private ShearControlRegionType() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShearControlRegionType"/> class.
        /// </summary>
        /// <param name="region">the region.</param>
        /// <param name="ignoreShearCheck">the ignore shear check.</param>
        /// <param name="reduceShearForces">the reduce shear forces.</param>
        public ShearControlRegionType(Geometry.Region region, bool ignoreShearCheck, bool reduceShearForces)
        {
            if(region.Contours.Count != 1)
            {
                throw new ArgumentException("ShearControlRegionType must be initialized with a Region with exactly one Contour.");
            }

            // One of ignoreShearCheck or ReduceShearForces must be true.
            if (ignoreShearCheck == reduceShearForces)
            {
                throw new ArgumentException("ShearControlRegionType must be initialized with either ignoreShearCheck or reduceShearForces set to true, not both.");
            }

            this.EntityCreated();
            this.Contour = region.Contours[0];
            this.IgnoreShearCheck = ignoreShearCheck;
            this.ReduceShearForces = reduceShearForces;
        }
    }
}
