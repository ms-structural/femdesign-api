// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;



namespace FemDesign.Calculate
{

    /// <summary>
    /// fdscript.xsd
    /// CMDSAVE
    /// </summary>
    [XmlRoot("cmdsave")]
    [System.Serializable]
    public partial class CmdSave : CmdCommand
    {
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        [XmlAttribute("command")]
        public string Command = "; CXL CS2SHELL SAVE"; // token, fixed
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        [XmlElement("filename")]
        public string FilePath { get; set; } // SZPATH

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdSave()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdSave"/> class.
        /// </summary>
        /// <param name="filepath">the filepath.</param>
        public CmdSave(string filepath)
        {
            this.FilePath = Path.GetFullPath(filepath);
        }


        /// <summary>
        /// To X Element.
        /// </summary>
        /// <returns>The result.</returns>
        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdSave>(this);
        }
    }


}