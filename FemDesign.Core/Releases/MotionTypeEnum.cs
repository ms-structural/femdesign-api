using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Releases
{
    /// <summary>
    /// Defines the Motion Type enumeration.
    /// </summary>
    public enum MotionType
    {
        [XmlEnum("motion")]
        Motion,

        [XmlEnum("rotation")]
        Rotation,
    }
}
