using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign
{
    /// <summary>
    /// FEM-Design National Annex.
    /// </summary>
    public enum Country
    {
        /// <summary>
        /// Belgian annex
        /// </summary>
        [Parseable("B", "b", "belgium", "Belgium", "belgian", "Belgian")]
        [XmlEnum("B")]
        B,
        
        /// <summary>
        /// Unspecified/Common annex
        /// </summary>
        [Parseable("", "common", "Common", "COMMON", "none", "None", "NONE")]
        [XmlEnum("common")]
        COMMON,

        /// <summary>
        /// German annex
        /// </summary>
        [Parseable("D", "d", "germany", "Germany", "german", "German")]
        [XmlEnum("D")]
        D,

        /// <summary>
        /// Danish annex
        /// </summary>
        [Parseable("DK", "dk", "denmark", "Denmark", "danish", "Danish")]
        [XmlEnum("DK")]
        DK,

        /// <summary>
        /// Spanish annex
        /// </summary>
        [Parseable("E", "e", "spain", "Spain", "spanish", "Spanish")]
        [XmlEnum("E")]
        E,

        /// <summary>
        /// Estonian annex
        /// </summary>
        [Parseable("EST", "est", "estonia", "Estonia", "estonian", "Estonian")]
        [XmlEnum("EST")]
        EST, 

        /// <summary>
        /// Finnish annex
        /// </summary>
        [Parseable("FIN", "fin", "finland", "Finland", "finnish", "Finnish")]
        [XmlEnum("FIN")]
        FIN,

        /// <summary>
        /// British annex
        /// </summary>
        [Parseable("GB", "gb", "british", "British", "great britain", "Great Britain")]
        [XmlEnum("GB")]
        GB,

        /// <summary>
        /// Hungarian annex
        /// </summary>
        [Parseable("H", "h", "hungary", "Hungary", "hungarian", "Hungarian")]
        [XmlEnum("H")]
        H,

        /// <summary>
        /// Latvian annex
        /// </summary>
        [Parseable("LT", "lt", "LV", "lv", "latvia", "Latvia", "latvian", "Latvian")]
        [XmlEnum("LT")]
        LT,

        /// <summary>
        /// Norwegian annex
        /// </summary>
        [Parseable("N", "n", "norway", "Norway", "norwegian", "Norwegian")]
        [XmlEnum("N")]
        N,

        /// <summary>
        /// Dutch annex
        /// </summary>
        [Parseable("NL", "nl", "netherlands", "Netherlands", "dutch", "Dutch")]
        [XmlEnum("NL")]
        NL,

        /// <summary>
        /// Polish annex
        /// </summary>
        [Parseable("PL", "pl", "poland", "Poland", "polish", "Polish")]
        [XmlEnum("PL")]
        PL,

        /// <summary>
        /// Romanian annex
        /// </summary>
        [Parseable("RO", "ro", "romania", "Romania", "romanian", "Romanian")]
        [XmlEnum("RO")]
        RO, 

        /// <summary>
        /// Swedish annex
        /// </summary>
        [Parseable("S", "s", "SE", "sweden", "Sweden", "swedish", "Swedish", "😎")]
        [XmlEnum("S")]
        S,

        /// <summary>
        /// Turkish annex. 
        /// <br></br>
        /// <br></br>
        /// Note: the TR (Turkish) national annex is no longer supported by FEM-Design.
        /// </summary>
        [Parseable("TR", "tr", "turkey", "Turkey")]
        [XmlEnum("TR")]
        TR
    }
}
