using FemDesign.Geometry;
using System.Xml.Serialization;
using System;


namespace FemDesign.Reinforcement
{
    /// <summary>
    /// Represents a Punching Area.
    /// </summary>
    [System.Serializable]
    public partial class PunchingArea: EntityBase
    {
        /// <summary>
        /// Gets or sets the base shell.
        /// </summary>
        [XmlElement("base_shell", Order = 1)]
        public GuidListType BaseShell { get; set; }

        /// <summary>
        /// Gets or sets the connected bar.
        /// </summary>
        [XmlElement("connected_bar", Order = 2)]
        public GuidListType ConnectedBar { get; set; }

        /// <summary>
        /// Gets or sets the local pos.
        /// </summary>
        [XmlElement("local_pos", Order = 3)]
        public Geometry.Point3d LocalPos { get; set; }

        /// <summary>
        /// Gets or sets the local x.
        /// </summary>
        [XmlElement("local_x", Order = 4)]
        public Geometry.Vector3d LocalX { get; set; }

        /// <summary>
        /// Gets or sets the local y.
        /// </summary>
        [XmlElement("local_y", Order = 5)]
        public Geometry.Vector3d LocalY { get; set; }

        /// <summary>
        /// Gets or sets the ref points offset.
        /// </summary>
        [XmlElement("reference_points_offset", Order = 6)]
        public Geometry.Point3d RefPointsOffset { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        [XmlElement("region", Order = 7)]
        public Geometry.Region Region { get; set; }

        /// <summary>
        /// Gets or sets the downward.
        /// </summary>
        [XmlAttribute("downward")]
        public bool Downward { get; set; }

        /// <summary>
        /// Gets or sets the manual design.
        /// </summary>
        [XmlAttribute("manual_design")]
        public bool ManualDesign { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PunchingArea"/> class.
        /// </summary>
        public PunchingArea()
        {
            this.EntityCreated();
        }

        internal FemDesign.Bars.Bar FindClosestBar(Model model)
        {
            if (model == null)
                return null;

            var bars = model.Entities.Bars;
            if (bars == null)
                return null;

            FemDesign.Bars.Bar closestBar = null;
            double minDistance = 0.001;

            foreach (var bar in bars)
            {
                var curve = bar.BarPart.Edge;
                if (curve == null)
                    continue;

                // Calculate distance from LocalPos to the bar (line segment)
                double distance = DistancePointToSegment(this.LocalPos, curve);
                if (distance < minDistance)
                {
                    closestBar = bar;
                    break;
                }
            }

            // You may want to define a threshold for "attached" (e.g., < 1e-6)
            return closestBar;
        }

        // Helper method to calculate distance from point to segment
        private static double DistancePointToSegment(FemDesign.Geometry.Point3d p, FemDesign.Geometry.Edge edge)
        {
            var a = edge.Points[0];
            var b = edge.Points[1];

            // Vector from a to p
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;
            double dz = b.Z - a.Z;

            double t = ((p.X - a.X) * dx + (p.Y - a.Y) * dy + (p.Z - a.Z) * dz) / (dx * dx + dy * dy + dz * dz);
            t = System.Math.Max(0, Math.Min(1, t));

            double closestX = a.X + t * dx;
            double closestY = a.Y + t * dy;
            double closestZ = a.Z + t * dz;

            double distX = p.X - closestX;
            double distY = p.Y - closestY;
            double distZ = p.Z - closestZ;

            return Math.Sqrt(distX * distX + distY * distY + distZ * distZ);
        }
    }
}
