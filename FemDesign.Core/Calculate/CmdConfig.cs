using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FemDesign.Results;
using System.Reflection;
using System.Xml.Linq;
using System.Text;
using System.Xml.Schema;
using System.Xml;
using FemDesign.GenericClasses;
using FemDesign.Bars;
using FemDesign.Geometry;

namespace FemDesign.Calculate
{
    /// <summary>
    /// Represents a Cmd Config.
    /// </summary>
    [XmlRoot("cmdconfig")]
    [System.Serializable]
    public partial class CmdConfig : CmdCommand
    {
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        [XmlAttribute("command")]
        public string Command = "$ MODULECOM APPLYCFG";

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        [XmlAttribute("file")]
        public string FilePath { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdConfig()
        {

        }

        /// <summary>
        /// Command to apply configuration to calculation from an existing cfg file
        /// </summary>
        /// <param name="filepath"></param>
        public CmdConfig(string filepath)
        {
            this.FilePath = System.IO.Path.GetFullPath(filepath);
        }

        /// <summary>
        /// Command to apply configuration from a config object
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="config"></param>
        public CmdConfig(string filePath, Config config)
        {
            this.FilePath = System.IO.Path.GetFullPath(filePath);
            config.Serialize(this.FilePath);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CmdConfig"/> class.
        /// </summary>
        /// <param name="filePath">the file path.</param>
        /// <param name="configs">the configs.</param>
        public CmdConfig(string filePath, List<CONFIG> configs)
        {
            this.FilePath = System.IO.Path.GetFullPath(filePath);

            Config config = new Config(configs);
            config.Serialize(this.FilePath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdConfig"/> class.
        /// </summary>
        /// <param name="filePath">the file path.</param>
        /// <param name="configs">the configs.</param>
        public CmdConfig(string filePath, params CONFIG[] configs)
        {
            this.FilePath = System.IO.Path.GetFullPath(filePath);

            Config config = new Config(configs);
            config.Serialize(this.FilePath);
        }

        /// <summary>
        /// To X Element.
        /// </summary>
        /// <returns>The result.</returns>
        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdConfig>(this);
        }
    }


    /// <summary>
    /// Represents a Config.
    /// </summary>
    [System.Serializable]
    [XmlRoot("configs")]
    public partial class Config
    {
        /// <summary>
        /// Gets or sets the configs.
        /// </summary>
        [XmlElement("CONFIG")]
        public List<CONFIG> CONFIGS { get; set; }

        private Config()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        /// <param name="configs">the configs.</param>
        public Config(List<CONFIG> configs)
        {
            this.CONFIGS = configs;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        /// <param name="configs">the configs.</param>
        public Config(params CONFIG[] configs)
        {
            this.CONFIGS = configs.ToList();
        }

        /// <summary>
        /// Serializes .
        /// </summary>
        /// <param name="filePath">the file path.</param>
        public void Serialize(string filePath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Config));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using (TextWriter writer = new StreamWriter(filePath))
            {
                xmlSerializer.Serialize(writer, this, namespaces);
            }
        }
    }

    /// <summary>
    /// Base class for all CONFIG that can be use for cmdconfig
    /// </summary>
    /// 
    [XmlInclude(typeof(MasonryConfig))]
    [XmlInclude(typeof(ConcreteDesignConfig))]
    [XmlInclude(typeof(SteelDesignConfiguration))]
    [XmlInclude(typeof(SteelBarDesignParameters))]
    [XmlInclude(typeof(SteelBarCalculationParameters))]
    [System.Serializable]
    public abstract class CONFIG
    {
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }
    }
} 