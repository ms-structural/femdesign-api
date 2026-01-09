// https://strusoft.com/
using System;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Reflection;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// fdscript root
    /// </summary>
    [XmlRoot("fdscript")]
    public partial class Bsc
    {
        /// <summary>
        /// Gets or sets the xml attrib.
        /// </summary>
        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public const string XmlAttrib = "fdscript.xsd";
        /// <summary>
        /// Gets or sets the fd script header.
        /// </summary>
        [XmlElement("fdscriptheader", Order = 1)]
        public FdScriptHeader FdScriptHeader { get; set; } // FDSCRIPTHEADER

        /// <summary>
        /// Gets or sets the cmd doc table.
        /// </summary>
        [XmlElement("cmddoctable", Order = 2)]
        public CmdDocTable _cmdDocTable;  // CMDDOCTABLE

        [XmlIgnore]
        public DocTable DocTable
        {
            get
            {
                return _cmdDocTable.DocTable;
            }
            set
            {
                this._cmdDocTable = new CmdDocTable(value);
            }
        }

        /// <summary>
        /// Gets or sets the cmd end session.
        /// </summary>
        [XmlElement("cmdendsession", Order = 3)]
        public CmdEndSession CmdEndSession { get; set; } // CMDENDSESSION
        [XmlIgnore]
        internal string Cwd { get; set; } // current work directory, string
        /// <summary>
        /// Gets or sets the bsc path.
        /// </summary>
        [XmlIgnore]
        public string BscPath { get; set; } // path to fdscript file, string

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Bsc()
        {

        }

        /// <summary>
        /// Constructor for .bsc files.
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="bscPath"></param>
        /// <param name="loadCaseCombName"></param>
        /// <param name="unitResult"></param>
        /// <param name="options"></param>
        /// <exception cref="ArgumentException"></exception>
        public Bsc(ListProc resultType, string bscPath, string loadCaseCombName = null, Results.UnitResults unitResult = null, Options options = null)
        {
            if (Path.GetExtension(bscPath) != ".bsc")
            {
                throw new ArgumentException($"File path must be '.bsc' but got '{bscPath}'");
            }
            BscPath = Path.GetFullPath(bscPath);
            Cwd = Path.GetDirectoryName(BscPath);
            DocTable = new DocTable(resultType, loadCaseCombName, unitResult, options);
            FdScriptHeader = new FdScriptHeader("Generated script.", Path.Combine(Cwd, "logfile.log"));
            CmdEndSession = new CmdEndSession();
            SerializeBsc(); // why it is in the constructor?
        }


        /// <summary>
        /// Constructor for .bsc files. Constructs a Bsc object for batch files to list specific analysis results by shape identifiers.
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="bscPath"></param>
        /// <param name="loadCombName"></param>
        /// <param name="shapeID"></param>
        /// <param name="unitResult"></param>
        /// <param name="options"></param>
        /// <exception cref="ArgumentException"></exception>
        internal Bsc(ListProc resultType, string bscPath, string loadCombName, int shapeID, Results.UnitResults unitResult = null, Options options = null)
        {
            if (Path.GetExtension(bscPath) != ".bsc")
            {
                throw new ArgumentException($"File path must be '.bsc' but got '{bscPath}'");
            }
            BscPath = Path.GetFullPath(bscPath);
            Cwd = Path.GetDirectoryName(BscPath);
            DocTable = new DocTable(resultType, loadCombName, shapeID, unitResult, options);
            FdScriptHeader = new FdScriptHeader("Generated script.", Path.Combine(Cwd, "logfile.log"));
            CmdEndSession = new CmdEndSession();
            SerializeBsc(); // why it is in the constructor?
        }

        /// <summary>
        /// Bsc Path From Result Types.
        /// </summary>
        /// <param name="resultTypes">the result types.</param>
        /// <param name="strPath">the str path.</param>
        /// <param name="units">the units.</param>
        /// <param name="options">the options.</param>
        /// <param name="allLoadCase">the all load case.</param>
        /// <returns>The result.</returns>
        public static List<string> BscPathFromResultTypes(IEnumerable<Type> resultTypes, string strPath, Results.UnitResults units = null, Options options = null, bool allLoadCase = true)
        {
            var notAResultType = resultTypes.Where(r => !typeof(Results.IResult).IsAssignableFrom(r)).FirstOrDefault();
            if (notAResultType != null)
                throw new ArgumentException($"{notAResultType.Name} is not a result type. (It does not inherit from {typeof(FemDesign.Results.IResult).FullName})");


            // Create Bsc files from resultTypes
            var listProcs = resultTypes.Select(r =>
                r.GetCustomAttribute<Results.ResultAttribute>()?.ListProcs
                ?? Enumerable.Empty<ListProc>()
            );

            var dir = Path.GetDirectoryName(strPath);
            var fileName = Path.GetFileNameWithoutExtension(strPath);

            // Create \data folder to store output
            string dataDir = Path.Combine(dir, fileName, "scripts");
            // If directory does not exist, create it
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            if (units == null)
                units = Results.UnitResults.Default();

            var batchResults = listProcs.SelectMany(lp => lp.Select(l => new Calculate.Bsc(l, Path.Combine(dataDir, $"{l}.bsc"), null, units, options)));
            var bscPathsFromResultTypes = batchResults.Select(bsc => bsc.BscPath).ToList();
            return bscPathsFromResultTypes;
        }

        public static implicit operator List<string>(Bsc bsc)
        {
            return new List<string>() { bsc.BscPath };
        }

        /// <summary>
        /// Serialize bsc.
        /// </summary>
        public void SerializeBsc()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Bsc));
            using (TextWriter writer = new StreamWriter(this.BscPath))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}