using System;
using System.Linq;
using System.Xml.Serialization;


namespace FemDesign.Shells
{
    [System.Serializable]
    public partial class ShellBucklingType: EntityBase
    {
        [XmlElement("direction", Order = 1)]
        public Geometry.Vector3d LocalX { get; set; }

        [XmlElement("contour", Order = 2)]
        public Geometry.Contour Contour { get; set; }

        [XmlAttribute("base_shell")]
        public Guid BaseShell { get; set; }

        [XmlAttribute("beta")]
        public double Beta { get; set; }

        /// <summary>
        /// Set ShellBuckling on a Slab element.
        /// </summary>
        /// <param name="slab">Slab element.</param>
        /// <param name="direction">Local x direction of the buckling.</param>
        /// <param name="beta">Beta factor.</param>
        /// <returns>Slab with ShellBuckling set.</returns>
        public static Slab SetOnSlab(Slab slab, Geometry.Vector3d direction, double beta)
        {
            if (slab.Material.Family != Materials.Family.Concrete)
                throw new ArgumentException("Shell buckling can only be set on slabs with a concrete material.");

            slab = slab.DeepClone();

            var shellBuckling = new ShellBucklingType();
            shellBuckling.EntityCreated();
            shellBuckling.BaseShell = slab.SlabPart.Guid;
            shellBuckling.LocalX = direction;
            shellBuckling.Contour = slab.SlabPart._region.First();
            shellBuckling.Beta = beta;

            slab.ShellBuckling = shellBuckling;
            return slab;
        }
    }
}