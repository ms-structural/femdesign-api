// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;


namespace FemDesign.Calculate
{

    /// <summary>
    /// Represents a Cmd Doc Table.
    /// </summary>
    [System.Serializable]
    public partial class CmdDocTable
    {   
        /// <summary>
        /// Gets or sets the doc table.
        /// </summary>
        [XmlElement("doctable", Order = 1)]
        public DocTable DocTable { get; set; }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        [XmlAttribute("command")]
        public string _command = "; CXL $MODULE DOCTABLE";

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdDocTable()
        {

        }

        /// <summary>
        /// CmdDocTable constructor
        /// </summary>
        /// <param name="docTable">DocTable</param>
        public CmdDocTable(DocTable docTable)
        {
            this.DocTable = docTable;
        }
    }
    
    /// <summary>
    /// Represents a Doc Table.
    /// </summary>
    [System.Serializable]
    public partial class DocTable
    {
        /// <summary>
        /// Gets or sets the list proc.
        /// </summary>
        [XmlElement("listproc", Order = 2)]
        public ListProc ListProc { get; set; }

        /// <summary>
        /// Gets or sets the fem design version.
        /// </summary>
        [XmlElement("version", Order = 3)]
        public string FemDesignVersion { get; set; } = FdScript.Version;
        

        // micro pattern to avoid an empty element
        // https://stackoverflow.com/a/610630/14969396
        /// <summary>
        /// Gets or sets the case index.
        /// </summary>
        [XmlIgnore]
        public int? CaseIndex { get; set; }
        /// <summary>
        /// Gets or sets the xml some value.
        /// </summary>
        [XmlElement("index", Order = 4)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int _xmlSomeValue { get { return CaseIndex.Value; } set { CaseIndex = value; } }
        /// <summary>
        /// Gets or sets the xml some value specified.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool _xmlSomeValueSpecified { get { return CaseIndex.HasValue; } }

        /// <summary>
        /// Gets or sets the suffix.
        /// </summary>
        [XmlElement("suffix", Order = 5)]
        public string Suffix { get; set; }



        [XmlIgnore]
        public bool AllCaseComb
        {
            get
            {
                if(Suffix == null) { return true; }
                else { return false; }
            }
        }

        /// <summary>
        /// Number of Coldata should be equal to the number of columns in the output file
        /// </summary>
        [XmlElement("coldata", Order = 6)]
        public List<Coldata> ColData = Coldata.Default(); // we are using 61 column data as it should be the maximum number of column in our result type

        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        [XmlElement("units", Order = 7)]
        public List<FemDesign.Results.Units> Units { get; set; }

        /// <summary>
        /// Gets or sets the option.
        /// </summary>
        [XmlElement("options", Order = 8)]
        public Options Option { get; set; }
        
        /// <summary>
        /// Gets or sets the res type.
        /// </summary>
        [XmlElement("restype", Order = 9)]
        public int ResType { get; set; }

        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private DocTable()
        {

        }

        /// <summary>
        /// DocTable Constructor
        /// </summary>
        internal DocTable(ListProc resultType, string loadCaseCombName = null, FemDesign.Results.UnitResults unitResult = null, Options options = null)
        {
            ListProc = resultType;

            if(loadCaseCombName != null)
            {
                if (resultType.IsLoadCase())
                {
                    Suffix = $"Ultimate - Load case: {loadCaseCombName}";
                }
                else if (resultType.IsLoadCombination())
                {
                    Suffix = $"Load Comb.: {loadCaseCombName}";
                }
            }            
            else
            {
                // return all the output related to the analysis
                // i.e eigen frequency will return all the eigen values
                CaseIndex = GetDefaultCaseIndex(resultType);
            }

            Units = Results.Units.GetUnits(unitResult);
            ResType = GetResType(resultType);
            Option = options ?? Options.GetOptions(resultType);
        }



        /// <summary>
        /// DocTable to return specific analysis results (e.g. NodalBucklingShapes, EigenFrequencies, etc.) by shape identifiers.
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="loadCombination"></param>
        /// <param name="shapeID"></param>
        /// <param name="unitResult"></param>
        /// <param name="options"></param>
        internal DocTable(ListProc resultType, string loadCombination, int shapeID, FemDesign.Results.UnitResults unitResult = null, Options options = null)
        {
            // check input
            if (shapeID <= 0)
                throw new Exception("Invalid shapeID. Parameter must be a positive, non-zero number!");

            ListProc = resultType;

            if(resultType != ListProc.NodalVibrationShape)
            {
                if (loadCombination == null)
                    throw new Exception("Load combination input cannot be null!");

                Suffix = $"{loadCombination} / {shapeID}";
            }
            else
            {
                Suffix = $"{shapeID}";
            }
            
            ResType = GetResType(resultType);
            CaseIndex = 0;  //  If the CaseIndex is set to its default value (see 'GetDefaultCaseIndex()' method), FD will ignore the suffix and the all results will be listed.
                            //  To get the specified result cases, use 0, and suffix will override the index in the batch file.
            Units = Results.Units.GetUnits(unitResult);
            Option = options ?? Options.GetOptions(resultType);
        }
        private int GetResType(ListProc resultType)
        {
            /*
            LT_CASE = 1,
            LT_CS = 2,  (construction stage)
            LT_COMB = 3,
            ...
            */

            string r = resultType.ToString();
            if (r.StartsWith("QuantityEstimation") ||
                r.EndsWith("Utilization") ||
                r.Contains("MaxComb") ||
                r.Contains("MaxLoadGroup") ||
                r.StartsWith("Fem") ||
                r.StartsWith("EigenFrequencies") ||
                r.Contains("MaxOfLoadCombinationMinMax") ||
                r == "CriticalParameters" ||
                r == "ImperfectionFactors" ||
                r.StartsWith("Equilibrium") ||
                r.StartsWith("Section"))
                return 0;
            if (r.EndsWith("LoadCase"))
                return 1;
            if (r.EndsWith("LoadCombination"))
                return 3;
            if (r.EndsWith("BucklingShape"))
                return 5;
            if (r.StartsWith("NodalVibrationShape"))
                return 6;

            throw new NotImplementedException($"'restype' index for {r} is not implemented.");
        }

        private int GetDefaultCaseIndex(ListProc resultType)
        {
            string r = resultType.ToString();
            if (r.StartsWith("QuantityEstimation") || r.EndsWith("Utilization") || r.Contains("MaxComb") || r.Contains("MaxLoadGroup") || r.StartsWith("Fem") || r.StartsWith("EigenFrequencies") || r.Contains("MaxOfLoadCombinationMinMax") || r == "CriticalParameters" || r == "ImperfectionFactors" || r.StartsWith("Section"))
                return 0;
            if (r.EndsWith("LoadCase"))
                return -65536; // All load cases
            if (r.EndsWith("LoadCombination") || r.StartsWith("NodalVibrationShape") || r.EndsWith("BucklingShape"))
                return -1; // All load combinations

            throw new FormatException($"Default case index of ResultType.{resultType} not known.");
        }
    }
}
