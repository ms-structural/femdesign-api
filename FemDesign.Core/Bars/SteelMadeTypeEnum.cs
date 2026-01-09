using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Bars
{
    // Used for BarPart.Made
    // <bar_part ... made="rolled">
    /// <summary>
    /// Defines the Steel Made Type enumeration.
    /// </summary>
    [System.Serializable]
    public enum SteelMadeType
    {
        [XmlEnum("rolled")]
        Rolled = 0,
        
        [XmlEnum("cold_worked")]
        ColdWorked,
        
        [XmlEnum("welded")]
        Welded,
    }
}
