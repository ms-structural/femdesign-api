using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


#if ISDYNAMO
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion
#endif

namespace FemDesign.GenericClasses
{
    #if ISDYNAMO
    [IsVisibleInDynamoLibrary(false)]
    #endif
    /// <summary>
    /// Defines the Vertical Alignment enumeration.
    /// </summary>
    public enum VerticalAlignment
    {
        [Parseable("top", "Top", "TOP")]
        [XmlEnum("top")]
        Top,
        
        [Parseable("center", "Center", "CENTER", "centre", "Centre", "CENTRE")]
        [XmlEnum("center")]
        Center,

        [Parseable("bottom", "Bottom", "BOTTOM")]
        [XmlEnum("bottom")]
        Bottom,
    }
}
