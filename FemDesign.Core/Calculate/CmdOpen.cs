// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;

namespace FemDesign.Calculate
{

    /// <summary>
    /// fdscript.xsd
    /// CMDOPEN
    /// </summary>
    [XmlRoot("cmdopen")]
    [System.Serializable]
    public partial class CmdOpen : CmdCommand
    {
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        [XmlAttribute("command")]
        public string Command = "; CXL CS2SHELL OPEN"; // token, fixed
        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        [XmlElement("filename")]
        public string Filename { get; set; } // SZPATH

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdOpen()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CmdOpen"/> class.
        /// </summary>
        /// <param name="filepath">the filepath.</param>
        public CmdOpen(string filepath)
        {
            this.Filename = Path.GetFullPath(filepath);
        }

        /// <summary>
        /// To X Element.
        /// </summary>
        /// <returns>The result.</returns>
        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdOpen>(this);
        }
    }

}