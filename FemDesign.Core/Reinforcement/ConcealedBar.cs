// https://strusoft.com/
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using FemDesign.GenericClasses;
using FemDesign.Geometry;


namespace FemDesign.Reinforcement
{
    /// <summary>
    /// Represents a Concealed Bar.
    /// </summary>
    [System.Serializable]
    public partial class ConcealedBar: NamedEntityBase, IStructureElement
    {
        private static int _concealedBarInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_concealedBarInstances;

        /// <summary>
        /// Gets or sets the rectangle.
        /// </summary>
        [XmlElement("rectangle", Order = 1)]
        public RectangleType Rectangle { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        [XmlElement("start", Order = 2)]
        public Point3d Start { get; set; }

        /// <summary>
        /// Gets or sets the buckling data.
        /// </summary>
        [XmlElement("buckling_data", Order = 3)]
        public Bars.Buckling.BucklingData BucklingData { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        [XmlElement("end", Order = 4)]
        public string End = "";

        /// <summary>
        /// Gets or sets the base shell.
        /// </summary>
        [XmlAttribute("base_shell")]
        public Guid BaseShell { get; set; }

        /// <summary>
        /// Gets or sets the axis in longer side.
        /// </summary>
        [XmlAttribute("axis_in_longer_side")]
        [DefaultValue(true)]
        public bool AxisInLongerSide { get; set; } = true;


        /// <summary>
        /// Gets or sets the reinforcement.
        /// </summary>
        [XmlIgnore]
        public List<BarReinforcement> Reinforcement = new List<BarReinforcement>();

        [XmlIgnore]
        public List<BarReinforcement> Stirrups
        {
            get
            {
                return this.Reinforcement.Where(x => x.Stirrups != null).ToList();
            }
        }
        [XmlIgnore]
        public List<BarReinforcement> LongitudinalBars
        {
            get
            {
                return this.Reinforcement.Where(x => x.LongitudinalBar != null).ToList();
            }
        }

        private ConcealedBar()
        {

        }

        /// <summary>
        /// Concealed bar constructor.
        /// </summary>
        /// <param name="slab">Base shell element.</param>
        /// <param name="rectangle">Rectangle area where the concealed bar is specified. Must be inside the SlabPart region boundary.</param>
        /// <param name="axisInLongerSide">If true, the axis of the concealed bar is parallel to the longer side of the rectangle, otherwise it is parallel to the shorter side.</param>
        /// <param name="identifier">Structural element identifier.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public ConcealedBar(Shells.Slab slab, RectangleType rectangle, bool axisInLongerSide = true, string identifier = "CB")
        {
            if (slab.SlabPart.ComplexMaterial.Concrete == null)
            {
                throw new System.ArgumentException("Slab material must be concrete!");
            }

            this.EntityCreated();
            this.BaseShell = slab.SlabPart.Guid;
            this.AxisInLongerSide = axisInLongerSide;
            if (axisInLongerSide == false)
            {
                this.Start = rectangle.BaseCorner + rectangle.LocalX * (rectangle.DimX / 2);
            }
            this.Rectangle = rectangle;
            this.Identifier = identifier;
        }

    }
}
