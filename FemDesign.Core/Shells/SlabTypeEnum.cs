using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Shells
{
    /// <summary>
    /// Defines the Slab Type enumeration.
    /// </summary>
    public enum SlabType
    {
        [XmlEnum("plate")]
        Plate,
        
        [XmlEnum("wall")]
        Wall,
    }
}
