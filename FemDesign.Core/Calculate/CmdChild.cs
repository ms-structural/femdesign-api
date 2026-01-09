// https://strusoft.com/
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// cmdchild
    /// </summary>
    [XmlRoot("cmdchild")]
    public partial class CmdChild : CmdCommand
    {
        /// <summary>
        /// Gets or sets the template path.
        /// </summary>
        [XmlText]
        public string _templatePath;

        [XmlIgnore]
        public string TemplatePath
        {
            get { return _templatePath; }
            set
            {
                string extension = Path.GetExtension(value);
                if (extension != ".dsc")
                {
                    throw new System.ArgumentException("Incorrect file-extension. Expected .dsc.");
                }

                _templatePath = value;
            }
        }
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdChild()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CmdChild"/> class.
        /// </summary>
        /// <param name="filePath">the file path.</param>
        public CmdChild(string filePath)
        {
            TemplatePath = Path.GetFullPath(filePath);
        }
        /// <summary>
        /// To X Element.
        /// </summary>
        /// <returns>The result.</returns>
        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdChild>(this);
        }
    }
}