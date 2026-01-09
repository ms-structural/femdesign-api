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
    /// Defines the Face enumeration.
    /// </summary>
    public enum Face
    {
        [Parseable("top", "Top", "TOP", "up", "Up", "UP")]
        [XmlEnum("top")]
        Top,

        [Parseable("mid", "Mid", "MID", "middle", "Middle", "MIDDLE")]
        [XmlEnum("mid")]
        Mid,
        
        [Parseable("bottom", "Bottom", "BOTTOM", "down", "Down", "DOWN")]
        [XmlEnum("bottom")]
        Bottom,
    }
}
