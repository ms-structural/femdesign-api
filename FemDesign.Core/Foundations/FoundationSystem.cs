using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Foundations
{
    /// <summary>
    /// Defines the Foundation System enumeration.
    /// </summary>
    public enum FoundationSystem
    {

        [XmlEnum("simple")]
        [Parseable("Simple")]
        Simple,

        [XmlEnum(Name = "surface_support_group")]
        [Parseable("SurfaceSupportGroup")]
        SurfaceSupportGroup,

        [XmlEnum(Name = "from_soil")]
        [Parseable("FromSoil")]
        FromSoil,
    }
}
