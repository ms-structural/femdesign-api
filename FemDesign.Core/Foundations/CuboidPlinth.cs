using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Foundations
{
    /// <summary>
    /// Represents a Cuboid Plinth.
    /// </summary>
    [System.Serializable]
    public partial class CuboidPlinth
    {
        /// <summary>
        /// Gets or sets the a.
        /// </summary>
        [XmlAttribute("a")]
        public double a { get; set; }

        /// <summary>
        /// Gets or sets the b.
        /// </summary>
        [XmlAttribute("b")]
        public double b { get; set; }

        /// <summary>
        /// Gets or sets the h.
        /// </summary>
        [XmlAttribute("h")]
        public double h { get; set; }

        private CuboidPlinth()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CuboidPlinth"/> class.
        /// </summary>
        /// <param name="a">value for <paramref name="a"/>.</param>
        /// <param name="b">value for <paramref name="b"/>.</param>
        /// <param name="h">value for <paramref name="h"/>.</param>
        public CuboidPlinth(double a, double b, double h)
        {
            this.a = a;
            this.b = b;
            this.h = h;
        }


    }
}
