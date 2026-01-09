// https://strusoft.com/

using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Releases
{
    /// <summary>
    /// Defines the Detach Type enumeration.
    /// </summary>
    [System.Serializable]
    public enum DetachType
    {
        [Parseable("")]
        [XmlEnum("")]
        None,

        [Parseable("x_tens")]
        [XmlEnum("x_tens")]
        X_Tension,

        [Parseable("x_comp")]
        [XmlEnum("x_comp")]
        X_Compression,

        [Parseable("y_tens")]
        [XmlEnum("y_tens")]
        Y_Tension,

        [Parseable("y_comp")]
        [XmlEnum("y_comp")]
        Y_Compression,

        [Parseable("z_tens")]
        [XmlEnum("z_tens")]
        Z_Tension,

        [Parseable("z_comp")]
        [XmlEnum("z_comp")]
        Z_Compression,
    }
}
