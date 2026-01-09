using System.Xml.Serialization;


namespace FemDesign.Materials
{
    /// <summary>
    /// Defines the Material Type Enum enumeration.
    /// </summary>
    [System.Serializable]
    public enum MaterialTypeEnum
    {
        SteelRolled  = 0,
        SteelColdWorked = 1,
        SteelWelded = 2,
        Concrete = 3,
        Timber = 4,
        Unknown = 65535,
        Undefined = -1
    }
}