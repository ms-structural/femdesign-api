// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;

namespace FemDesign.Calculate
{
    /// <summary>
    /// Represents a Cmd User.
    /// </summary>
    [XmlRoot("cmduser")]
    [System.Serializable]
    public partial class CmdUser : CmdCommand
    {
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        [XmlAttribute("command")]
        public string _command; // token
        [XmlIgnore]
        public CmdUserModule Command
        {
            get { return (CmdUserModule)System.Enum.Parse(typeof(CmdUserModule), this._command.Split(new string[] { "; CXL $MODULE " }, System.StringSplitOptions.None)[0]); }
            set { this._command = "; CXL $MODULE " + value.ToString(); }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal CmdUser()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CmdUser"/> class.
        /// </summary>
        /// <param name="module">the module.</param>
        public CmdUser(CmdUserModule module)
        {
            this.Command = module;
        }

        /// <summary>
        /// To X Element.
        /// </summary>
        /// <returns>The result.</returns>
        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdUser>(this);
        }

    }







}