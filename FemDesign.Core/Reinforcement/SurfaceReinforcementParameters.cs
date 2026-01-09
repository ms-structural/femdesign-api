// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Reinforcement
{
    /// <summary>
    /// Represents a Surface Reinforcement Parameters.
    /// 
    /// Shell reinforcement parameters
    /// </summary>
    [System.Serializable]
    public partial class SurfaceReinforcementParameters: EntityBase
    {
        /// <summary>
        /// Gets or sets the single layer reinforcement.
        /// </summary>
        [XmlAttribute("single_layer_reinforcement")]
        public bool SingleLayerReinforcement { get; set; } // bool. Default = false

        /// <summary>
        /// Gets or sets the base shell.
        /// </summary>
        [XmlElement("base_shell", Order=1)]
        public GuidListType BaseShell { get; set; } // guid_list_type // reference to slabPart of slab
        /// <summary>
        /// Gets or sets the center.
        /// </summary>
        [XmlElement("center", Order=2)]
        public Center Center { get; set; }
        /// <summary>
        /// Gets or sets the x direction.
        /// </summary>
        [XmlElement("x_direction", Order=3)]
        public Geometry.Vector3d XDirection { get; set; } // point_type_3d
        /// <summary>
        /// Gets or sets the y direction.
        /// </summary>
        [XmlElement("y_direction", Order = 4)]
        public Geometry.Vector3d YDirection { get; set; } // point_type_3d

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private SurfaceReinforcementParameters()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceReinforcementParameters"/> class.
        /// </summary>
        /// <param name="singleLayerReinforcement">the single layer reinforcement.</param>
        /// <param name="baseShell">the base shell.</param>
        /// <param name="center">the center.</param>
        /// <param name="xDirection">value for <paramref name="xDirection"/>.</param>
        /// <param name="yDirection">value for <paramref name="yDirection"/>.</param>
        public SurfaceReinforcementParameters(bool singleLayerReinforcement, GuidListType baseShell, Center center, Geometry.Vector3d xDirection, Geometry.Vector3d yDirection)
        {
            // object information
            this.EntityCreated();

            // single layer reinforcement?
            if (singleLayerReinforcement)
            {
                this.SingleLayerReinforcement = singleLayerReinforcement;
            }

            // other properties
            this.BaseShell = baseShell;
            this.Center = center;
            this.XDirection = xDirection;
            this.YDirection = yDirection;
        }

        /// <summary>
        /// Straight reinforcement layout on slab.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="slab">the slab.</param>
        /// <param name="singleLayerReinforcement">the single layer reinforcement.</param>
        /// <param name="xDir">value for <paramref name="xDir"/>.</param>
        /// <param name="yDir">value for <paramref name="yDir"/>.</param>
        public static SurfaceReinforcementParameters Straight(Shells.Slab slab, bool singleLayerReinforcement = false, Geometry.Vector3d xDir = null, Geometry.Vector3d yDir = null)
        {
            GuidListType baseShell = new GuidListType(slab.SlabPart.Guid);
            Center center = Center.Straight();
            Geometry.Vector3d xDirection = xDir ?? slab.SlabPart.LocalX;
            Geometry.Vector3d yDirection = yDir ?? slab.SlabPart.LocalY;
            return new SurfaceReinforcementParameters(singleLayerReinforcement, baseShell, center, xDirection, yDirection);
        }
    }
}