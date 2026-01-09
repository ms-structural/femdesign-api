using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// cmdprojdescr
    /// </summary>
    [XmlRoot("cmdprojdescr")]
    [System.Serializable]
    public class CmdProjDescr : CmdCommand
    {
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        [XmlAttribute("command")]
        public string Command = "$ MODULECOM PROJDESCR";

        /// <summary>
        /// Gets or sets the read.
        /// </summary>
        [XmlAttribute("read")]
        public int _read = 0;

        [XmlIgnore]
        public bool Read
        {
            get
            {
                return Convert.ToBoolean(this._read);
            }
            set
            {
                this._read = Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Gets or sets the reset.
        /// </summary>
        [XmlAttribute("reset")]
        public int _reset = 0;

        [XmlIgnore]
        public bool Reset
        {
            get
            {
                return Convert.ToBoolean(this._reset);
            }
            set
            {
                this._reset = Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        [XmlAttribute("szProject")]
        public string Project { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [XmlAttribute("szDescription")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the designer.
        /// </summary>
        [XmlAttribute("szDesigner")]
        public string Designer { get; set; }

        /// <summary>
        /// Gets or sets the signature.
        /// </summary>
        [XmlAttribute("szSignature")]
        public string Signature { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        [XmlAttribute("szComment")]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        [XmlElement("item")]
        public List<UserDefinedData> Items { get; set; }


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdProjDescr()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CmdProjDescr"/> class.
        /// </summary>
        /// <param name="project">the project.</param>
        /// <param name="description">the description.</param>
        /// <param name="designer">the designer.</param>
        /// <param name="signature">the signature.</param>
        /// <param name="comment">the comment.</param>
        public CmdProjDescr(string project, string description, string designer, string signature, string comment)
        {
            this.Project = project;
            this.Description = description;
            this.Designer = designer;
            this.Signature = signature;
            this.Comment = comment;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdProjDescr"/> class.
        /// </summary>
        /// <param name="project">the project.</param>
        /// <param name="description">the description.</param>
        /// <param name="designer">the designer.</param>
        /// <param name="signature">the signature.</param>
        /// <param name="comment">the comment.</param>
        /// <param name="items">the items.</param>
        public CmdProjDescr(string project, string description, string designer, string signature, string comment, List<UserDefinedData> items) : this(project, description, designer, signature, comment)
        {
            this.Items = items;
        }

        /// <summary>
        /// To X Element.
        /// </summary>
        /// <returns>The result.</returns>
        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdProjDescr>(this);
        }

    }

    /// <summary>
    /// Represents a User Defined Data.
    /// </summary>
    public class UserDefinedData
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the txt.
        /// </summary>
        [XmlAttribute("txt")]
        public string Txt { get; set; }

        private UserDefinedData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDefinedData"/> class.
        /// </summary>
        /// <param name="id">the id.</param>
        /// <param name="txt">the txt.</param>
        public UserDefinedData(string id, string txt)
        {
            this.Id = id;
            this.Txt = txt;
        }
    }
}