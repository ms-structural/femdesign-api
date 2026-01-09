// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;

namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd    
    /// FDSCRIPTHEADER
    /// </summary>
    [XmlRoot("fdscriptheader")]
    [System.Serializable]
    public partial class FdScriptHeader : CmdCommand
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [XmlElement("title")]
        public string Title { get; set; } // SZBUF
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        [XmlElement("version")]
        public string Version { get; set; } // SZNAME
        /// <summary>
        /// Gets or sets the module.
        /// </summary>
        [XmlElement("module")]
        public string Module { get; set; } // SZPATH (?)
        /// <summary>
        /// Gets or sets the log file.
        /// </summary>
        [XmlElement("logfile")]
        public string LogFile { get; set; } // SZPATH

        /// <summary>
        /// Gets or sets the continue on error.
        /// </summary>
        [XmlAttribute("fContinueOnError")]
        public int _continueOnError { get; set; } = 1;

        /// <summary>
        /// Gets or sets the ignore parse error.
        /// </summary>
        [XmlAttribute("fIgnoreParseError")]
        public int _ignoreParseError { get; set; } = 1;

        [XmlIgnore]
        public bool ContinueOnError
        {
            get { return this._continueOnError == 1; }
            set { this._continueOnError = System.Convert.ToInt32(value); }
        }

        [XmlIgnore]
        public bool IgnoreParseError
        {
            get { return this._ignoreParseError == 1; }
            set { this._ignoreParseError = System.Convert.ToInt32(value); }
        }


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public FdScriptHeader()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FdScriptHeader"/> class.
        /// </summary>
        /// <param name="title">the title.</param>
        /// <param name="logfile">the logfile.</param>
        public FdScriptHeader(string title, string logfile)
        {
            this.Title = title;
            this.Version = FdScript.Version;
            this.Module = "sframe";
            this.LogFile = logfile;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FdScriptHeader"/> class.
        /// </summary>
        /// <param name="logFilePath">the log file path.</param>
        public FdScriptHeader(string logFilePath)
        {
            Title = "FEM-Design script";
            Version = FdScript.Version;
            Module = "SFRAME";
            LogFile = System.IO.Path.GetFullPath(logFilePath);
        }

        /// <summary>
        /// To X Element.
        /// </summary>
        /// <returns>The result.</returns>
        public override XElement ToXElement()
        {
            return Extension.ToXElement<FdScriptHeader>(this);
        }

    }

}