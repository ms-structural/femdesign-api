// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Composites
{
    /// <summary>
    /// Defines the Composite Section Parameter Type enumeration.
    /// </summary>
    [System.Serializable]
    public enum CompositeSectionParameterType  // number values must be expressed in mm
    {
        [XmlEnum("b")]
        b,

        [XmlEnum("b_eff")]
        bEff,
        
        [XmlEnum("bb")]
        bb,
        
        [XmlEnum("bc")]
        bc,
        
        [XmlEnum("bf")]
        bf,
        
        [XmlEnum("bt")]
        bt,
        
        [XmlEnum("cy")]
        cy,
        
        [XmlEnum("cz")]
        cz,
        
        [XmlEnum("d")]
        d,
        
        [XmlEnum("d1")]
        d1,
        
        [XmlEnum("d2")]
        d2,
        
        [XmlEnum("fill_beam")]
        FillBeam,
        
        [XmlEnum("h")]
        h,
        
        [XmlEnum("hc")]
        hc,
        
        [XmlEnum("name")]
        Name,
        
        [XmlEnum("o1")]
        o1,
        
        [XmlEnum("o2")]
        o2,
        
        [XmlEnum("t")]
        t,
        
        [XmlEnum("tf")]
        tf,
        
        [XmlEnum("tfb")]
        tfb,
        
        [XmlEnum("tft")]
        tft,
        
        [XmlEnum("th")]
        th,
        
        [XmlEnum("tw")]
        tw,
    }
}
