// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;


namespace FemDesign.Calculate
{
    /// <summary>
    /// Represents a Cmd End Session.
    /// </summary>
    [XmlRoot("cmdendsession")]
    [System.Serializable]
    public partial class CmdEndSession : CmdCommand
    {
        /// <summary>
        /// To X Element.
        /// </summary>
        /// <returns>The result.</returns>
        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdEndSession>(this);
        }
    }

}