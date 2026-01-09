using System;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;

using FemDesign;
using FemDesign.Calculate;
using FemDesign.Bars;
using System.Globalization;
using FemDesign.Results;
using System.Text.RegularExpressions;
using System.Text;
using FemDesign.AuxiliaryResults;
using FemDesign.Geometry;
using FemDesign.Materials;
using FemDesign.GenericClasses;
using FemDesign.Sections;
using static System.Collections.Specialized.BitVector32;
using Newtonsoft.Json.Serialization;

namespace FemDesign
{
    /// <summary>
    /// FEM-Design real-time connection. Use this to open a <see cref="Model"/>, run <see cref="Analysis"/>, <see cref="Design"/>, extract results and more.
    /// </summary>
    public class FemDesignConnection : IDisposable
    {
        private readonly int fdVersion = 24;
        private readonly PipeConnection _connection;
        private readonly Process _process;
        /// <summary>
        /// Gets a value indicating whether s exited.
        /// </summary>
        public bool HasExited { get; private set; }
        /// <summary>
        /// Gets a value indicating whether connected.
        /// </summary>
        public bool IsConnected => _connection._inputPipe.IsConnected;
        /// <summary>
        /// Gets a value indicating whether disconnected.
        /// </summary>
        public bool IsDisconnected => !IsConnected;

        /// <summary>
        /// File path of th open file in the current Pipe.
        /// WARNING
        /// If a user manually open a file in a pipe instance, the file path location will be equal to null.
        /// </summary>
        private string CurrentOpenModel;
        /// <summary>
        /// Gets or sets the verbosity.
        /// </summary>
        public Verbosity Verbosity { get; private set; }
        /// <summary>
        /// Gets or sets the default verbosity.
        /// </summary>
        public const Verbosity DefaultVerbosity = Verbosity.ScriptLogLinesOnly;

        /// <summary>
        /// Keep FEM-Design open after <see cref="Dispose"/> is called.
        /// </summary>
        private bool _keepOpen;
                
        /// <summary>
        /// Represents a callback.
        /// </summary>
        /// <param name="output">the output.</param>
        public delegate void OnOutputEvent(string output);
        /// <summary>
        /// Occurs whenever FEM-Design writes a new log message.
        /// 
        /// Verbosity may be adjusted using <see cref="SetVerbosity(Verbosity)"/>
        /// </summary>
        public OnOutputEvent OnOutput { get; set; } = null;

        /// <summary>
        /// Open a new instance of FEM-Design and connect to it.
        /// </summary>
        /// <param name="fdInstallationDir">FEM-Design software installation directory.</param>
        /// <param name="minimized">Open FEM-Design as a minimized window.</param>
        /// <param name="keepOpen">If true FEM-Design will be left open and have to be manually exited.</param>
        /// <param name="outputDir">the output dir.</param>
        /// <param name="tempOutputDir"><code>BE CAREFUL!</code>If true the <paramref name="outputDir"/> will be deleted on exit. This option has no effect unless <paramref name="outputDir"/> has been specified.</param>
        /// <param name="verbosity"></param>
        public FemDesignConnection(
            string fdInstallationDir = @"C:\Program Files\StruSoft\FEM-Design 24\",
            bool minimized = false,
            bool keepOpen = false,
            string outputDir = null,
            bool tempOutputDir = false,
            Verbosity verbosity = DefaultVerbosity)
        {
            string installDir = SetFemDesignDirectory(fdInstallationDir);
            string pathToFemDesign = Path.Combine(installDir, "fd3dstruct.exe");

            string pipeName = "FdPipe" + Guid.NewGuid().ToString();
            var startInfo = new ProcessStartInfo()
            {
                FileName = pathToFemDesign,
                Arguments = "/p " + pipeName,
                UseShellExecute = false,
                Verb = "open",
            };

            startInfo.EnvironmentVariables["FD_NOLOGO"] = "1";

            if (minimized)
            {
                startInfo.EnvironmentVariables["FD_NOGUI"] = "1";
            }

            OutputDir = outputDir;
            if (tempOutputDir)
                _outputDirsToBeDeleted.Add(OutputDir);

            _keepOpen = keepOpen;

            _connection = new PipeConnection(pipeName);
            try
            {
                _process = Process.Start(startInfo);
            }
            catch
            {
                throw new Exception(@"fd3dstruct.exe has not been found. `C:\Program Files\StruSoft\FEM-Design 24\` does not exist!");
            }

            _process.Exited += _processExited;
            _connection.WaitForConnection();

            // Forward all output messages from pipe (except echo guid commands).
            _connection.OnOutput += (message) =>
            {
                message = message.Replace(">echo ", "");
                bool isGuid = Guid.TryParse(message, out Guid _);
                if (isGuid == false)
                    OnOutput?.Invoke(message);
            };

            SetVerbosity(verbosity);
        }

        /// <summary>
        /// Retrieve the default FEM-Design installation directory or specify a custom directory path for FemDesignConnection.
        /// </summary>
        /// <param name="fdInstallationDir">FEM-Design software installation directory. If set to `null`, the default directory will be used.</param>
        /// <returns>The installation directory path.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public string SetFemDesignDirectory(string fdInstallationDir)
        {
            // Check if specified directory exists
            if (!string.IsNullOrEmpty(fdInstallationDir) && Directory.Exists(fdInstallationDir))
                return fdInstallationDir;

            var defaultDirs = new List<string>()
            {
                @"C:\Program Files\StruSoft\FEM-Design 24\",
                @"C:\Program Files\StruSoft\FEM-Design 24 Educational\",
                @"C:\Program Files\StruSoft\FEM-Design 24 Student\",
                @"C:\Program Files\StruSoft\FEM-Design 24 Night Install\"   // for StruSoft employees
            };

            foreach (var dir in defaultDirs)
            {
                if (Directory.Exists(dir))
                    return dir;
            }

            throw new ArgumentNullException($"Default FEM-Design installation directory is not found. Input directory `{fdInstallationDir}` does not exist!");
        }

        private void _processExited(object sender, EventArgs e)
        {
            this.HasExited = true;
        }

        /// <summary>
        /// Disconnects the current connection. FEM-Design will be left open for normal usage.
        /// </summary>
        public void Disconnect()
        {
            this._connection.Send("detach"); // Tell FEM-Design to detach from the pipe
            this._connection.Disconnect();
        }

        /// <summary>
        /// FEM-Design will be left open, when the <see cref="FemDesignConnection"/> is disposed.
        /// </summary>
        public void KeepOpen()
        {
            _keepOpen = true;
        }

        /// <summary>
        /// Run a script and wait for it to finish.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="filename"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void RunScript(FdScript script, string filename = "script")
        {
            if (script == null) throw new ArgumentNullException("script");

            string scriptPath = OutputFileHelper.GetFdScriptPath(OutputDir, filename);

            script.Serialize(scriptPath);
            this._connection.Send("runUTF8 " + scriptPath);
            this._connection.WaitForCommandToFinish();
        }

        /// <summary>
        /// Run a script and wait for it to finish.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="filename"></param>
        /// <returns>The result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task RunScriptAsync(FdScript script, string filename = "script")
        {
            if (script == null) throw new ArgumentNullException("script");

            string scriptPath = OutputFileHelper.GetFdScriptPath(OutputDir, filename);

            script.Serialize(scriptPath);
            this._connection.Send("runUTF8 " + scriptPath);
            await this._connection.WaitForCommandToFinishAsync();
        }

        /// <summary>
        /// Open a file in FEM-Design application.
        /// </summary>
        /// <param name="filePath">the file path.</param>
        /// <param name="disconnect">Set to True to disconnect to the pipe and leave FEM-Design Open.</param>
        public void Open(string filePath, bool disconnect = false)
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            this.RunScript(new FdScript(logfile, new CmdOpen(filePath)), "OpenModel");
            this.CurrentOpenModel = filePath;
            if (disconnect) this.Disconnect();
        }

        /// <summary>
        /// Open a file in FEM-Design application.
        /// </summary>
        /// <param name="filePath">the file path.</param>
        /// <returns>The result.</returns>
        public async Task OpenAsync(string filePath)
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            await this.RunScriptAsync(new FdScript(logfile, new CmdOpen(filePath)));

        }

        /// <summary>
        /// Open a <see cref="Model"/> in FEM-Design application.
        /// </summary>
        /// <param name="model">Model to be opened.</param>
        /// <param name="disconnect">Set to True to disconnect to the pipe and leave FEM-Design Open.</param>
        public void Open(Model model, bool disconnect = false)
        {
            var struxml = OutputFileHelper.GetStruxmlPath(OutputDir);
            // Model must be serialized to a file to be opened in FEM-Design.
            model.SerializeModel(struxml);
            this.Open(struxml, disconnect);
        }

        /// <summary>
        /// Set global settings for a FEM-Design model.
        /// </summary>
        /// <param name="cmdglobalconfig">Global configuration object.</param>
        public void SetGlobalConfig(Calculate.CmdGlobalCfg cmdglobalconfig)
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            var script = new FdScript(
                logfile,
                cmdglobalconfig
            );
            this.RunScript(script, "SetGlobalConfig");
        }

        /// <summary>
        /// Set global settings for a FEM-Design model.
        /// </summary>
        /// <param name="globConfig">Can be any type of configuration from CmdGlobalCfg (e.g. MeshGeneral, MeshSettings, Meshfunctions, etc.). See the properties of Calculate.CmdGlobalCfg class.</param>
        public void SetGlobalConfig(params Calculate.GlobConfig[] globConfig)
        {
            var cmd = new CmdGlobalCfg(globConfig);
            this.SetGlobalConfig(cmd);
        }

        /// <summary>
        /// Set global settings for a FEM-Design model using a global configuration file.
        /// </summary>
        /// <param name="filepath">Filepath of the global configuration file.</param>
        public void SetGlobalConfig(string filepath)
        {
            var globalCfg = Calculate.CmdGlobalCfg.DeserializeCmdGlobalCfgFromFilePath(filepath);
            this.SetGlobalConfig(globalCfg);
        }


        /// <summary>
        /// Set design settings for a FEM-Design model using a configuration file.
        /// </summary>
        /// <param name="filepath">Filepath of the configuration file.</param>
        public void SetConfig(string filepath)
        {
            var cmdconfig = new Calculate.CmdConfig(filepath);

            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            var script = new FdScript(
                logfile,
                cmdconfig
            );
            this.RunScript(script, "SetConfig");
        }

        /// <summary>
        /// Set design settings for a FEM-Design model.
        /// </summary>
        /// <param name="configs">Configuration objects.</param>
        public void SetConfig(params CONFIG[] configs)
        {
            var filePath = OutputFileHelper.GetConfigfilePath(OutputDir);
            var cmdconfig = new Calculate.CmdConfig(filePath, configs);

            this.SetConfig(cmdconfig);
        }

        /// <summary>
        /// Sets the config.
        /// </summary>
        /// <param name="cmdConfig">the cmd config.</param>
        public void SetConfig(CmdConfig cmdConfig)
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            var script = new FdScript(
                logfile,
                cmdConfig
            );
            this.RunScript(script, "SetConfig");
        }

        /// <summary>
        /// Sets the proj description.
        /// </summary>
        /// <param name="cmdProjDescr">the cmd proj descr.</param>
        public void SetProjDescription(CmdProjDescr cmdProjDescr)
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            var script = new FdScript(
                logfile,
                cmdProjDescr
            );
            this.RunScript(script, "SetConfig");
        }

        /// <summary>
        /// Sets the proj description.
        /// </summary>
        /// <param name="project">the project.</param>
        /// <param name="description">the description.</param>
        /// <param name="designer">the designer.</param>
        /// <param name="signature">the signature.</param>
        /// <param name="comment">the comment.</param>
        /// <param name="items">the items.</param>
        public void SetProjDescription(string project, string description, string designer, string signature, string comment, List<UserDefinedData> items = null)
        {
            var cmdProjdescr = new CmdProjDescr(project, description, designer, signature, comment, items);
            this.SetProjDescription(cmdProjdescr);
        }


        /// <summary>
        /// Open a <see cref="Model"/> in FEM-Design application.
        /// </summary>
        /// <param name="model">Model to be opened.</param>
        /// <returns>The result.</returns>
        public async Task OpenAsync(Model model)
        {
            var struxml = OutputFileHelper.GetStruxmlPath(OutputDir);
            // Model must be serialized to a file to be opened in FEM-Design.
            model.SerializeModel(struxml);
            await this.OpenAsync(struxml);
        }

        /// <summary>
        /// Runs an analysis task on the current model in FEM-Design.
        /// </summary>
        /// <param name="analysis">the analysis.</param>
        public void RunAnalysis(Analysis analysis)
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            FdScript script;

            if(analysis.CalcCase || analysis.CalcComb)
            {
                if (analysis.Comb == null) analysis.Comb = Comb.Default();
                var analysis_static = Analysis.StaticAnalysis(analysis.Comb, analysis.CalcCase, analysis.CalcComb);

                if (analysis_static.Comb.CombItem.Any(CombItem => CombItem.CombName != null) || analysis_static.Comb.CombItem.Count() == 0)
                {
                    analysis_static.SetCombAnalysis(this);
                }
                script = new FdScript(
                    logfile,
                    new CmdUser(CmdUserModule.RESMODE),
                    new CmdCalculation(analysis_static));
                this.RunScript(script, "RunStaticAnalysis");
            }

            if (analysis.Stability != null)
            {
                var statbility_analysis = analysis.DeepClone();
                statbility_analysis.SetStabilityAnalysis(this);

                script = new FdScript(
                    logfile,
                    new CmdUser(CmdUserModule.RESMODE),
                    new CmdCalculation(statbility_analysis));
                this.RunScript(script, "RunStabilityAnalysis");
            }

            if (analysis.Imperfection != null)
            {
                var imperfection_analysis = analysis.DeepClone();
                imperfection_analysis.SetImperfectionAnalysis(this);

                script = new FdScript(
                    logfile,
                    new CmdUser(CmdUserModule.RESMODE),
                    new CmdCalculation(analysis));
                this.RunScript(script, "RunImperfectionAnalysis");
            }

            if (analysis.CalcFreq)
            {
                if (analysis.Freq == null) analysis.Freq = Freq.Default();
                var freq_analysis = Analysis.Eigenfrequencies(analysis.Freq);

                script = new FdScript(
                    logfile,
                    new CmdUser(CmdUserModule.RESMODE),
                    new CmdCalculation(freq_analysis));
                this.RunScript(script, "RunFreqAnalysis");
            }

            // it requires a refactor
            if (analysis.Footfall != null || analysis.PeriodicEx != null || analysis.ExForce != null || analysis.GroundAcc != null)
            {
                analysis.CalcCase = false;
                analysis.CalcComb = false;
                analysis.CalcFreq = false;
                analysis.CalcImpf = false;
                analysis.CalcStab = false;

                script = new FdScript(
                    logfile,
                    new CmdUser(CmdUserModule.RESMODE),
                    new CmdCalculation(analysis));
                this.RunScript(script, "RunAnalysis");
            }
        }

        /// <summary>
        /// Opens <paramref name="model"/> in FEM-Design and runs the analysis.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="analysis"></param>
        public void RunAnalysis(Model model, Analysis analysis)
        {
            this.Open(model);
            this.RunAnalysis(analysis);
        }

        /// <summary>
        /// Runs a design task on the current model in FEM-Design.
        /// </summary>
        /// <param name="userModule"></param>
        /// <param name="design"></param>
        /// <param name="designGroups"></param>
        /// <exception cref="ArgumentException"></exception>
        public void RunDesign(CmdUserModule userModule, Design design, List<CmdDesignGroup> designGroups = null)
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);

            #region DESIGN_GROUP

            if (designGroups != null && designGroups.Count != 0)
            {
                // delete previously define design group
                var cmdcommands = new List<CmdCommand>();

                foreach (var desGroup in CmdDesignGroup._designGroupCache)
                {
                    var emptyDesignGroup = new CmdDesignGroup(desGroup.Key, new List<FemDesign.GenericClasses.IStructureElement>(), desGroup.Value.Type);
                    cmdcommands.Add(emptyDesignGroup);
                }

                foreach (var desGroup in designGroups)
                {
                    cmdcommands.Add(desGroup);

                    if (!CmdDesignGroup._designGroupCache.ContainsKey(desGroup.Name))
                    {
                        CmdDesignGroup._designGroupCache[desGroup.Name] = desGroup;
                    }
                }

                var _script = new FdScript(logfile, cmdcommands);
                this.RunScript(_script, "DesignGroup");
            }
            else // delete previously define design group
            {
                var cmdcommands = new List<CmdCommand>();

                foreach (var desGroup in CmdDesignGroup._designGroupCache)
                {
                    var emptyDesignGroup = new CmdDesignGroup(desGroup.Key, new List<FemDesign.GenericClasses.IStructureElement>(), desGroup.Value.Type);
                    cmdcommands.Add(emptyDesignGroup);
                }

                var _script = new FdScript(logfile, cmdcommands);
                this.RunScript(_script, "DeleteDesignGroup");

                CmdDesignGroup._designGroupCache.Clear();
            }
            #endregion



            if (userModule == CmdUserModule.RESMODE)
            {
                throw new ArgumentException("User Module can not be 'RESMODE'!");
            }

            var script = new FdScript(
                    logfile,
                    new CmdUser(userModule),
                    new CmdCalculation(design)
                );

            this.RunScript(script, $"RunDesign_{userModule}");

            if (design.ApplyChanges == true)
                this.ApplyDesignChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ApplyDesignChanges()
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);

            var script = new FdScript(
                    logfile,
                    new CmdApplyDesignChanges()
                );

            this.RunScript(script, $"ApplyDesignChanges");
        }

        /// <summary>
        /// Opens <paramref name="model"/> in FEM-Design and runs the design.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="design"></param>
        /// <param name="userModule"></param>
        public void RunDesign(Model model, Design design, CmdUserModule userModule)
        {
            this.Open(model);
            this.RunDesign(userModule, design);
        }

        /// <summary>
        /// Sets the verbosity.
        /// </summary>
        /// <param name="verbosity">the verbosity.</param>
        public void SetVerbosity(Verbosity verbosity)
        {
            Verbosity = verbosity;
            _connection.Send("v " + (int)verbosity);
        }

        /// <summary>
        /// End Session.
        /// </summary>
        public void EndSession()
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);

            var script = new FdScript(
                logfile,
                new CmdEndSession()
            );

            this.RunScript(script, "EndSession");
        }

        /// <summary>
        /// Retrieves the currently opened model with all available elements as a <see cref="Model"/> object.
        /// </summary>
        /// <returns>The result.</returns>
        public Model GetModel()
        {
            string struxmlPath = OutputFileHelper.GetStruxmlPath(OutputDir, "model_saved");
            string logfilePath = OutputFileHelper.GetLogfilePath(OutputDir);
            RunScript(new FdScript(logfilePath, new CmdSave(struxmlPath)), "GetModel");
            return Model.DeserializeFromFilePath(struxmlPath);
        }

        /// <summary>
        /// Save the documentation in a docx file.
        /// </summary>
        /// <param name="docxFilePath"></param>
        private void SaveDocx(string docxFilePath)
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);

            var script = new FdScript(
                logfile,
                new CmdSaveDocx(docxFilePath)
            );

            this.RunScript(script, "SaveDocx");
        }

        /// <summary>
        /// Save the documentation in a docx file using a .bsc template file
        /// </summary>
        /// <param name="docxFilePath">.docx file path where the documentatio will be saved</param>
        /// <param name="templatePath">template .dsc file path to apply to the documentation</param>
        public void SaveDocx(string docxFilePath, string templatePath = null)
        {
            if (templatePath != null)
                this.ApplyDocumentationTemplate(templatePath);
            this.SaveDocx(docxFilePath);
        }

        /// <summary>
        /// Apply a template to a documentation
        /// </summary>
        /// <param name="templatePath"></param>
        public void ApplyDocumentationTemplate(string templatePath)
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);

            var script = new FdScript(
                logfile,
                new CmdChild(templatePath)
            );

            this.RunScript(script, "SaveDocxFromTemplate");
        }

        /// <summary>
        /// Retrieves the loads from the currently opened model with all available elements as a <see cref="Loads.Loads"/> object.
        /// </summary>
        /// <returns>The result.</returns>
        public Loads.Loads GetLoads()
        {
            string struxmlPath = OutputFileHelper.GetStruxmlPath(OutputDir, "model_loads_saved");
            string logfilePath = OutputFileHelper.GetLogfilePath(OutputDir);
            RunScript(new FdScript(logfilePath, new CmdSave(struxmlPath)), "GetLoads");
            return Loads.Loads.DeserializeFromFilePath(struxmlPath);
        }

        /// <summary>
        /// Retrieves the load combinations from the currently opened model/> object.
        /// </summary>
        /// <returns>The result.</returns>
        public Dictionary<int, Loads.LoadCombination> GetLoadCombinations()
        {
            var loadCombinations = this.GetLoads().LoadCombinations;

            var dictLoadComb = new Dictionary<int, Loads.LoadCombination>();

            if (loadCombinations == null)
                throw new Exception("There are no load combinations in the model");

            int index = 0;
            foreach (var comb in loadCombinations)
            {
                dictLoadComb.Add(index, comb);
                index++;
            }

            return dictLoadComb;
        }


        /// <summary>
        /// Generate Fea Model.
        /// </summary>
        public void GenerateFeaModel()
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);

            var script = new FdScript(
                logfile,
                new CmdUser(CmdUserModule.RESMODE),
                new CmdCalculation()
            );

            this.RunScript(script, "GenerateFea");
        }

        /// <summary>
        /// Retrieve node geometry data from the finite element mesh.
        /// </summary>
        /// <param name="length">Unit of length measurement. Default is meter.</param>
        /// <returns></returns>
        public List<Results.FemNode> GetFeaNodes(Results.Length length = Results.Length.m)
        {
            var units = Results.UnitResults.Default();
            units.Length = length;

            return _readResults<FemNode>(p => true, null, null, false, null, units);
        }

        /// <summary>
        /// Retrieve line element data from the finite element mesh.
        /// </summary>
        /// <param name="length">Unit of length measurement. Default is meter.</param>
        /// <returns></returns>
        public List<Results.FemBar> GetFeaBars(Results.Length length = Results.Length.m)
        {
            var units = Results.UnitResults.Default();
            units.Length = length;

            return _readResults<FemBar>(p => true, null, null, false, null, units);
        }

        /// <summary>
        /// Retrieve surface element data from the finite element mesh.
        /// </summary>
        /// <param name="length">Unit of length measurement. Default is meter.</param>
        /// <returns></returns>
        public List<Results.FemShell> GetFeaShells(Results.Length length = Results.Length.m)
        {
            var units = Results.UnitResults.Default();
            units.Length = length;

            return _readResults<FemShell>(p => true, null, null, false, null, units);
        }

        /// <summary>
        /// Retrieve the line connection element data from the finite element model.
        /// </summary>
        /// <returns></returns>
        public List<Results.LineConnectionElement> GetFeaLineConnections()
        {
            return _readResults<LineConnectionElement>(p => true, null, null, false, null);
        }

        /// <summary>
        /// Retrieve the point connection element data from the finite element model.
        /// </summary>
        /// <returns></returns>
        public List<Results.PointConnectionElement> GetFeaPointConnections()
        {
            return _readResults<PointConnectionElement>(p => true, null, null, false, null);
        }

        /// <summary>
        /// Retrieve the line support element data from the finite element model.
        /// </summary>
        /// <returns></returns>
        public List<Results.LineSupportElement> GetFeaLineSupports()
        {
            return _readResults<LineSupportElement>(p => true, null, null, false, null);
        }

        /// <summary>
        /// Retrieve the point support element data from the finite element model.
        /// </summary>
        /// <returns></returns>
        public List<Results.PointSupportElement> GetFeaPointSupports()
        {
            return _readResults<PointSupportElement>(p => true, null, null, false, null);
        }

        /// <summary>
        /// Retrieve finite element data.
        /// </summary>
        /// <param name="units">Unit of length measurement. Default is meter.</param>
        /// <returns></returns>
        public Results.FiniteElement GetFeaModel(Results.Length units = Results.Length.m)
        {
            var feaNode = GetFeaNodes(units);
            var feaBar = GetFeaBars(units);
            var feaShell = GetFeaShells(units);
            var pointConnections = GetFeaPointConnections();
            var lineConnections = GetFeaLineConnections();
            var pointSupports = GetFeaPointSupports();
            var lineSupports = GetFeaLineSupports();

            var fdFea = new Results.FiniteElement
            {
                FemNode = feaNode,
                FemBar = feaBar,
                FemShell = feaShell,
                FemPointConnection = pointConnections,
                FemLineConnection = lineConnections,
                FemPointSupport = pointSupports,
                FemLineSupport = lineSupports
            };

            return fdFea;
        }

        /// <summary>
        /// Create a list of result points.
        /// </summary>
        /// <param name="resultPoints"></param>
        public void CreateResultPoint(List<CmdResultPoint> resultPoints)
        {
            // FdScript commands
            List<CmdCommand> commands = new List<CmdCommand>
            {
                new CmdUser(CmdUserModule.RESMODE)
            };
            commands.AddRange(resultPoints);

            // Run the script
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            var script = new FdScript(logfile, commands);
            this.RunScript(script, "CreateResultPoints");
        }

        public List<T> GetResultsOnPoints<T>(AuxiliaryResults.ResultPoint resultPoint, Results.UnitResults units = null) where T : Results.IResult
        {
            return GetResultsOnPoints<T>(new List<CmdResultPoint> { resultPoint }, units);
        }

        public List<T> GetResultsOnPoints<T>(List<AuxiliaryResults.ResultPoint> resultPoints, Results.UnitResults units = null) where T : Results.IResult
        {
            var cmdResultPoints = resultPoints.Select(x => (CmdResultPoint)x).ToList();
            return GetResultsOnPoints<T>(cmdResultPoints, units);
        }


        /// <summary>
        /// Retrieve results for multiple result points.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="resultPoints">List of result point .fdscript commands.</param>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private List<T> GetResultsOnPoints<T>(List<CmdResultPoint> resultPoints, Results.UnitResults units = null) where T : Results.IResult
        {
            var options = new Options(BarResultPosition.ResultPoints, ShellResultPosition.ResultPoints);
            var listProcs = (typeof(T).GetCustomAttribute<Results.ResultAttribute>()?.ListProcs ?? Enumerable.Empty<ListProc>()).ToList();

            // Check listProcs input parameter. Must have at least one item in the list. Type <T> object can have multiple ListProc attribute.
            if (!listProcs.Any())
                throw new ArgumentNullException("No existing ListProc for type <T>.");

            // Get .bsc and .csv file paths for .fdscript file generation
            (List<string> bscPaths, List<string> csvPaths) = _createBscCsvFilePaths(listProcs, loadCaseCombName: null, shapeId: null, null, units, options);

            // List commands
            List<CmdListGen> listGenCommands = new List<CmdListGen>();
            for (int i = 0; i < bscPaths.Count; i++)
                listGenCommands.Add(new CmdListGen(bscPaths[i], csvPaths[i]));

            // FdScript commands
            List<CmdCommand> scriptCommands = new List<CmdCommand>();
            scriptCommands.AddRange(resultPoints);
            scriptCommands.AddRange(listGenCommands);

            // Run the script
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            var script = new FdScript(logfile, scriptCommands.ToArray());
            this.RunScript(script, $"Get{typeof(T).Name}Results");

            return Results.ResultsReader.ParseCsvFiles<T>(csvPaths);
        }

        /// <summary>
        /// Retrieve results using a batch (.bsc) file.
        /// </summary>
        /// <param name="element">the element.</param>
        /// <param name="inputBscPath"></param>
        /// <param name="outputCsvPath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<string> GetResultsFromBsc(string inputBscPath, string outputCsvPath = null, List<IStructureElement> element = null)
        {
            // Check input
            if (outputCsvPath == null)
            {
                outputCsvPath = System.IO.Path.ChangeExtension(inputBscPath, "csv");
            }
            if (System.IO.Path.GetExtension(outputCsvPath) != ".csv")
                throw new Exception("Extension output file must be .csv");

            // Create .fdscript and list results
            _listResultsByFdScript("GetResultsFromBsc", new List<string> { inputBscPath }, new List<string> { outputCsvPath }, element);

            // Read results
            var results = System.IO.File.ReadAllLines(outputCsvPath, System.Text.Encoding.UTF8).Select(x => x.Replace("\t", ",")).ToList();

            return results;
        }

        /// <summary>
        /// Retreive all of the results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <param name="options">Optional. Options to set up the output location.</param>
        /// <param name="elements">Optional. Structural elements for which the results should be return.</param>
        /// <returns>List of results of type <typeparamref name="T"/> if any could be retrieved. If the model has no results of type <typeparamref name="T"/> or cannot access them at the moment, then the list will be empty.</returns>
        public List<T> GetResults<T>(Results.UnitResults units = null, Options options = null, List<FemDesign.GenericClasses.IStructureElement> elements = null) where T : Results.IResult
        {
            return _getResults<T>(units, options, elements, false);
        }

        /// <summary>
        /// Retreive load case results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="loadCase">Optional. Load case name for which the results should be return.</param>
        /// <param name="element">Optional. Structural element for which the results should be return.</param>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <param name="options">Optional. Options to set up the output location.</param>
        /// <returns>List of results of type <typeparamref name="T"/> if any could be retrieved. If the model has no results of type <typeparamref name="T"/> or cannot access them at the moment, then the list will be empty.</returns>
        public List<T> GetLoadCaseResults<T>(string loadCase = null, FemDesign.GenericClasses.IStructureElement element = null, Results.UnitResults units = null, Options options = null) where T : Results.IResult
        {
            List<string> loadCases = loadCase != null ? new List<string> { loadCase } : null;
            List<GenericClasses.IStructureElement> elements = element != null ? new List<GenericClasses.IStructureElement> { element } : null;

            return _getLoadCaseResults<T>(loadCases, elements, units, options);
        }

        /// <summary>
        /// Retreive load case results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="loadCase">Load case name for which the results should be return.</param>
        /// <param name="elements">Structural elements for which the results should be return.</param>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <param name="options">Optional. Options to set up the output location.</param>
        /// <returns>List of results of type <typeparamref name="T"/> if any could be retrieved. If the model has no results of type <typeparamref name="T"/> or cannot access them at the moment, then the list will be empty.</returns>
        public List<T> GetLoadCaseResults<T>(string loadCase, List<FemDesign.GenericClasses.IStructureElement> elements, Results.UnitResults units = null, Options options = null) where T : Results.IResult
        {
            List<string> loadCases = loadCase != null ? new List<string> { loadCase } : null;

            return _getLoadCaseResults<T>(loadCases, elements, units, options);
        }

        /// <summary>
        /// Retreive load case results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="loadCases">Load case names for which the results should be return.</param>
        /// <param name="element">Optional. Structural element for which the results should be return.</param>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <param name="options">Optional. Options to set up the output location.</param>
        /// <returns>List of results of type <typeparamref name="T"/> if any could be retrieved. If the model has no results of type <typeparamref name="T"/> or cannot access them at the moment, then the list will be empty.</returns>
        public List<T> GetLoadCaseResults<T>(List<string> loadCases, FemDesign.GenericClasses.IStructureElement element = null, Results.UnitResults units = null, Options options = null) where T : Results.IResult
        {
            List<GenericClasses.IStructureElement> elements = element != null ? new List<GenericClasses.IStructureElement> { element } : null;

            return _getLoadCaseResults<T>(loadCases, elements, units, options);
        }

        /// <summary>
        /// Retreive load case results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="loadCases">Load case names for which the results should be return.</param>
        /// <param name="elements">Structural elements for which the results should be return.</param>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <param name="options">Optional. Options to set up the output location.</param>
        /// <returns>List of results of type <typeparamref name="T"/> if any could be retrieved. If the model has no results of type <typeparamref name="T"/> or cannot access them at the moment, then the list will be empty.</returns>
        public List<T> GetLoadCaseResults<T>(List<string> loadCases, List<FemDesign.GenericClasses.IStructureElement> elements, Results.UnitResults units = null, Options options = null) where T : Results.IResult
        {
            return _getLoadCaseResults<T>(loadCases, elements, units, options);
        }

        /// <summary>
        /// Retreive load combination results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="loadCombination">Optional. Load combination name for which the results should be return.</param>
        /// <param name="element">Optional. Structural element for which the results should be return.</param>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <param name="options">Optional. Options to set up the output location.</param>
        /// <returns>List of results of type <typeparamref name="T"/> if any could be retrieved. If the model has no results of type <typeparamref name="T"/> or cannot access them at the moment, then the list will be empty.</returns>
        /// <exception cref="ArgumentException"></exception>
        public List<T> GetLoadCombinationResults<T>(string loadCombination = null, FemDesign.GenericClasses.IStructureElement element = null, Results.UnitResults units = null, Options options = null) where T : Results.IResult
        {
            List<string> loadCombinations = loadCombination != null ? new List<string> { loadCombination } : null;
            List<GenericClasses.IStructureElement> elements = element != null ? new List<GenericClasses.IStructureElement> { element } : null;

            return _getLoadCombinationResults<T>(loadCombinations, elements, units, options);
        }

        /// <summary>
        /// Retreive load combination results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="loadCombination">Load combination name for which the results should be return.</param>
        /// <param name="elements">Structural elements for which the results should be return.</param>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <param name="options">Optional. Options to set up the output location.</param>
        /// <returns>List of results of type <typeparamref name="T"/> if any could be retrieved. If the model has no results of type <typeparamref name="T"/> or cannot access them at the moment, then the list will be empty.</returns>
        /// <exception cref="ArgumentException"></exception>
        public List<T> GetLoadCombinationResults<T>(string loadCombination, List<FemDesign.GenericClasses.IStructureElement> elements, Results.UnitResults units = null, Options options = null) where T : Results.IResult
        {
            List<string> loadCombinations = loadCombination != null ? new List<string> { loadCombination } : null;

            return _getLoadCombinationResults<T>(loadCombinations, elements, units, options);
        }

        /// <summary>
        /// Retreive load combination results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="loadCombinations">Load combination names for which the results should be return.</param>
        /// <param name="element">Optional. Structural element for which the results should be return.</param>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <param name="options">Optional. Options to set up the output location.</param>
        /// <returns>List of results of type <typeparamref name="T"/> if any could be retrieved. If the model has no results of type <typeparamref name="T"/> or cannot access them at the moment, then the list will be empty.</returns>
        /// <exception cref="ArgumentException"></exception>
        public List<T> GetLoadCombinationResults<T>(List<string> loadCombinations, FemDesign.GenericClasses.IStructureElement element = null, Results.UnitResults units = null, Options options = null) where T : Results.IResult
        {
            List<GenericClasses.IStructureElement> elements = element != null ? new List<GenericClasses.IStructureElement> { element } : null;

            return _getLoadCombinationResults<T>(loadCombinations, elements, units, options);
        }

        /// <summary>
        /// Retreive load combination results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="loadCombinations">Load combination names for which the results should be return.</param>
        /// <param name="elements">Structural elements for which the results should be return.</param>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <param name="options">Optional. Options to set up the output location.</param>
        /// <returns>List of results of type <typeparamref name="T"/> if any could be retrieved. If the model has no results of type <typeparamref name="T"/> or cannot access them at the moment, then the list will be empty.</returns>
        /// <exception cref="ArgumentException"></exception>
        public List<T> GetLoadCombinationResults<T>(List<string> loadCombinations, List<FemDesign.GenericClasses.IStructureElement> elements, Results.UnitResults units = null, Options options = null) where T : Results.IResult
        {
            return _getLoadCombinationResults<T>(loadCombinations, elements, units, options);
        }

        /// <summary>
        /// Retreive all of the quantity estimation results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <returns></returns>
        public List<T> GetQuantities<T>(Results.UnitResults units = null) where T : Results.IQuantityEstimationResult
        {
            return _getQuantities<T>(units);
        }

        /// <summary>
        /// Retreive the stability results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="loadCombination">Optional. Load combination name for which the results should be return.</param>
        /// <param name="shapeId">Optional. Shape identifier for which the results should be return.</param>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <param name="options">Optional. Options to set up the output location.</param>
        /// <returns>List of results of type <typeparamref name="T"/> if any could be retrieved. If the model has no results of type <typeparamref name="T"/> or cannot access them at the moment, then the list will be empty.</returns>
        public List<T> GetStabilityResults<T>(string loadCombination = null, int? shapeId = null, Results.UnitResults units = null, Options options = null) where T : IResult
        {
            List<int> shapeIds = shapeId.HasValue ? new List<int> { (int)shapeId } : null;
            List<string> loadCombinations = loadCombination != null ? new List<string> { loadCombination } : null;

            return _getStabilityResults<T>(loadCombinations, shapeIds, units, options);
        }

        /*  WIP - GitHub case: 876
         *  
        /// <summary>
        /// Get results for those result types where FEM-Design uses eigen solver. E.g. imperfections, stability analysis, vibrations.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loadCombinations">Load combination names for which you want to list the results.</param>
        /// <param name="shapeIds">Shape identifiers for which you want to list the results. Must be positive, non-zero numbers.</param>
        /// <param name="elements">Structural elements for which you want to list the results.</param>
        /// <param name="units"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public List<T> GetEigenResults<T>(List<string> loadCombinations, List<int> shapeIds, List<FemDesign.GenericClasses.IStructureElement> elements = null, Results.UnitResults units = null, Options options = null) where T : IResult
        {
            // Check inputs
            if (loadCombinations == null || loadCombinations.Count == 0)
                throw new ArgumentException("loadCombinations input cannot be null or empty!");
            if (shapeIds == null || shapeIds.Count == 0)
                throw new ArgumentException("shapeIds input cannot be null or empty!");


            var listProcs = (typeof(T).GetCustomAttribute<Results.ResultAttribute>()?.ListProcs ?? Enumerable.Empty<ListProc>()).ToList();

            // Time stamp in file names (bsc, csv, fdscript). Required for Grasshopper components, otherwise Grasshoper cannot access the files at runtime.
            var currentTime = "__" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_fff");

            // Get .bsc and .csv file paths for .fdscript file generation
            var (bscPaths, csvPaths) = _createBscCsvFilePaths(listProcs, loadCombinations, shapeIds, currentTime, units, options);

            // Generate .csv result files
            _listResultsByFdScript($"Get{typeof(T).Name}Results" + currentTime, bscPaths, csvPaths, elements);

            return ParseCsvFiles<T>(csvPaths);
        }
        */


        /// <summary>
        /// Save.
        /// </summary>
        /// <param name="filePath">the file path.</param>
        public void Save(string filePath)
        {
            if (System.IO.Path.GetExtension(filePath) != ".str" && System.IO.Path.GetExtension(filePath) != ".struxml")
            {
                throw new Exception("Only .str and .struxml extensions are valid!");
            }

            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            var script = new FdScript(logfile, new CmdSave(filePath));
            this.RunScript(script);
        }

        /// <summary>
        /// Run Interaction Surface.
        /// </summary>
        /// <param name="bar">the bar.</param>
        /// <param name="offset">the offset.</param>
        /// <param name="fUlt">flag for <paramref name="fUlt"/>.</param>
        /// <returns>The result.</returns>
        public FemDesign.Results.InteractionSurface RunInteractionSurface(FemDesign.Bars.Bar bar, double offset = 0.0, bool fUlt = true)
        {
            var bars = new List<GenericClasses.IStructureElement> { bar };
            return RunInteractionSurface(bars, offset, fUlt)[0];
        }

        /// <summary>
        /// Load Group To Load Comb.
        /// </summary>
        /// <param name="fu">the fu.</param>
        /// <param name="fua">the fua.</param>
        /// <param name="fus">the fus.</param>
        /// <param name="fsq">the fsq.</param>
        /// <param name="fsf">the fsf.</param>
        /// <param name="fsc">the fsc.</param>
        /// <param name="fSeisSigned">flag for <paramref name="fSeisSigned"/>.</param>
        /// <param name="fSeisTorsion">flag for <paramref name="fSeisTorsion"/>.</param>
        /// <param name="fSeisZdir">flag for <paramref name="fSeisZdir"/>.</param>
        /// <param name="fSkipMinDL">flag for <paramref name="fSkipMinDL"/>.</param>
        /// <param name="fForceTemp">flag for <paramref name="fForceTemp"/>.</param>
        /// <param name="fShortName">flag for <paramref name="fShortName"/>.</param>
        public void LoadGroupToLoadComb(bool fu = true, bool fua = true, bool fus = true, bool fsq = true, bool fsf = true, bool fsc = true, bool fSeisSigned = true, bool fSeisTorsion = true, bool fSeisZdir = true, bool fSkipMinDL = true, bool fForceTemp = true, bool fShortName = true)
        {
            var cmdLoadGroupToLoadComb = new CmdLoadGroupToLoadComb(fu, fua, fus, fsq, fsf, fsc, fSeisSigned, fSeisTorsion, fSeisZdir, fSkipMinDL, fForceTemp, fShortName);

            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);

            var script = new FdScript(logfile, cmdLoadGroupToLoadComb);

            this.RunScript(script, "LoadGroupToLoadComb");
        }

        /// <summary>
        /// Run Interaction Surface.
        /// </summary>
        /// <param name="bars">the bars.</param>
        /// <param name="offset">the offset.</param>
        /// <param name="fUlt">flag for <paramref name="fUlt"/>.</param>
        /// <returns>The result.</returns>
        public List<FemDesign.Results.InteractionSurface> RunInteractionSurface(List<FemDesign.GenericClasses.IStructureElement> bars, double offset = 0.0, bool fUlt = true)
        {
            string outFile = OutputFileHelper.GetIntSrffilePath(OutputDir);

            var model = new Model(Country.COMMON, bars, overwrite: true);
            this.Open(model);

            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);

            var script = new FdScript(logfile, new CmdUser(CmdUserModule.RCDESIGN));
            foreach (var bar in bars)
            {
                var _bar = (Bars.Bar)bar;
                var newName = System.IO.Path.GetFileNameWithoutExtension(outFile) + _bar.BarPart.Guid + ".txt";
                script.Add(new CmdInteractionSurface(_bar, newName, offset, fUlt));
            }

            this.RunScript(script);

            var intSurfaces = new List<FemDesign.Results.InteractionSurface>();

            foreach (var bar in bars)
            {
                var _bar = (Bars.Bar)bar;
                var newName = System.IO.Path.GetFileNameWithoutExtension(outFile) + _bar.BarPart.Guid + ".txt";
                var intSrf = FemDesign.Results.InteractionSurface.ReadFromFile(newName);
                intSurfaces.Add(intSrf);
            }
            return intSurfaces;
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            if (_keepOpen) Disconnect();

            this._connection.Dispose();

            // TODO: Delete the files when they are not locked by FEM-Design
            this._deleteOutputDirectories();
        }

        private void _deleteOutputDirectories()
        {
            foreach (string dir in _outputDirsToBeDeleted)
                if (Directory.Exists(dir))
                    _deleteFolderIfNotUsed(dir);
        }

        private static void _deleteFolderIfNotUsed(string folderPath)
        {
            _removeReadOnlyAttribute(folderPath, out List<string> subDirectories);

            try
            {
                // update directory info
                foreach (var dir in subDirectories)
                {
                    var directoryInfo = new DirectoryInfo(dir);
                    directoryInfo.Refresh();
                }

                Directory.Delete(folderPath, true);
            }
            catch (IOException ex)
            {
                // Check if the exception is due to a file or folder being in use
                if ((ex.HResult & 0xFFFF) == 32 || (ex.HResult & 0xFFFF) == 33)
                {
                    // The folder or a file is in use, so we can't delete it yet
                    return;
                }
                // The exception is not related to a file or folder being in use, rethrow it
                throw;
            }
        }

        private static void _removeReadOnlyAttribute(string folderPath, out List<string> subDirectories)
        {
            // get directories & subdirectories
            subDirectories = new List<string> { folderPath };
            int i = 0;
            while (i < subDirectories.Count)
            {
                var subSubDirs = Directory.GetDirectories(subDirectories[i]).ToList();
                if (subSubDirs?.Count > 0)
                    subDirectories.AddRange(subSubDirs);

                i++;
            }

            // check if directory is read-only
            foreach (var dir in subDirectories)
            {
                var directoryInfo = new DirectoryInfo(dir);
                if ((directoryInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    // Remove read-only attribute
                    directoryInfo.Attributes &= ~FileAttributes.ReadOnly;
                }
            }
        }

        /// <summary>
        /// Create .bsc files, and return the .bsc and .csv file paths.
        /// </summary>
        private (List<string> bscPaths, List<string> csvPaths) _createBscCsvFilePaths(List<ListProc> listProcs, string loadCaseCombName = null, int? shapeId = null, string currentTime = null, Results.UnitResults units = null, Options options = null)
        {
            if (listProcs == null)
                throw new ArgumentNullException("listProcs input cannot be null!");

            string suffix1 = loadCaseCombName != null ? "__" + loadCaseCombName : null;
            string suffix2 = shapeId != null ? "_Shape-" + shapeId : null;

            // Input bsc and output csv file paths
            var bscPaths = listProcs.Select(l => OutputFileHelper.GetBscPath(OutputDir, l.ToString() + suffix1 + suffix2 + currentTime)).ToList();
            var csvPaths = listProcs.Select(l => OutputFileHelper.GetCsvPath(OutputDir, l.ToString() + suffix1 + suffix2 + currentTime)).ToList();

            // Create bsc files
            if (shapeId.HasValue)
            {
                var bscs = listProcs.Zip(bscPaths, (l, p) => new Bsc(l, p, loadCaseCombName, (int)shapeId, units, options)).ToList();
            }
            else
            {
                var bscs = listProcs.Zip(bscPaths, (l, p) => new Bsc(l, p, loadCaseCombName, units, options)).ToList();
            }

            return (bscPaths, csvPaths);
        }

        /// <summary>
        /// Create .bsc files, and return the .bsc and .csv file paths.
        /// </summary>
        private (List<string> bscPaths, List<string> csvPaths) _createBscCsvFilePaths(List<ListProc> listProcs, List<string> loadCaseCombNames, List<int> shapeIds, string currentTime = null, Results.UnitResults units = null, Options options = null)
        {
            // Initialize outputs
            List<string> bscPaths = new List<string>();
            List<string> csvPaths = new List<string>();

            // Check inputs
            if (loadCaseCombNames != null && shapeIds != null)
            {
                foreach (var combo in loadCaseCombNames)
                {
                    foreach (var id in shapeIds)
                    {
                        var item = _createBscCsvFilePaths(listProcs, combo, id, currentTime, units, options);
                        bscPaths.AddRange(item.bscPaths);
                        csvPaths.AddRange(item.csvPaths);
                    }
                }
            }
            else if (loadCaseCombNames != null && shapeIds == null)
            {
                foreach (var combo in loadCaseCombNames)
                {
                    var item = _createBscCsvFilePaths(listProcs, combo, shapeId: null, currentTime, units, options);
                    bscPaths.AddRange(item.bscPaths);
                    csvPaths.AddRange(item.csvPaths);
                }
            }
            else if (loadCaseCombNames == null && shapeIds != null)
            {
                foreach (var id in shapeIds)
                {
                    var item = _createBscCsvFilePaths(listProcs, loadCaseCombName: null, id, currentTime, units, options);
                    bscPaths.AddRange(item.bscPaths);
                    csvPaths.AddRange(item.csvPaths);
                }
            }
            else if (loadCaseCombNames == null && shapeIds == null)
            {
                return _createBscCsvFilePaths(listProcs, loadCaseCombName: null, shapeId: null, currentTime, units, options);
            }

            return (bscPaths, csvPaths);
        }

        /// <summary>
        /// Create and run the .fdscript file that lists the result tables and creates the .csv output files.
        /// </summary>
        private void _listResultsByFdScript(string scriptFileName, List<string> bscPaths, List<string> csvPaths, List<FemDesign.GenericClasses.IStructureElement> elements = null)
        {
            // FdScript commands
            List<CmdCommand> scriptCommands = new List<CmdCommand>();
            for (int i = 0; i < bscPaths.Count; i++)
                scriptCommands.Add(new CmdListGen(bscPaths[i], csvPaths[i], elements));

            // Run the script
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            var script = new FdScript(logfile, scriptCommands);
            this.RunScript(script, scriptFileName);
        }

        /// <summary>
        /// <strong>For internal use only!</strong>
        /// </summary>
        private List<T> _readResults<T>(Func<ListProc, bool> filter, List<string> loadCaseCombNames = null, List<int> shapeIds = null, bool timeStamp = false, List<GenericClasses.IStructureElement> elements = null, UnitResults units = null, Options options = null) where T : IResult
        {
            var listProcs = (typeof(T).GetCustomAttribute<Results.ResultAttribute>()?.ListProcs.Where(filter) ?? Enumerable.Empty<ListProc>()).ToList();
            if (!listProcs.Any())
                throw new ArgumentException("T parameter must be a result type that matches the provided filter!");

            // Time stamp in file names (bsc, csv, fdscript). Required for Grasshopper components, otherwise Grasshoper cannot access the files at runtime.
            string currentTime = timeStamp ? ("__" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_fff")) : null;

            // Get .bsc and .csv file paths for .fdscript file generation
            var (bscPaths, csvPaths) = _createBscCsvFilePaths(listProcs, loadCaseCombNames, shapeIds, currentTime, units, options);

            // Generate .csv result files
            _listResultsByFdScript($"Get{typeof(T).Name}Results" + currentTime, bscPaths, csvPaths, elements);

            return Results.ResultsReader.ParseCsvFiles<T>(csvPaths);
        }

        /// <summary>
        /// Retreive results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <param name="options">Optional. Options to set up the output location.</param>
        /// <param name="elements">Structural elements for which the results should be return.</param>
        /// <param name="timeStamp">If true, the current time will be included in the file names.</param>
        /// <returns>List of results of type <typeparamref name="T"/> if any could be retrieved. If the model has no results of type <typeparamref name="T"/> or cannot access them at the moment, then the list will be empty.</returns>
        internal List<T> _getResults<T>(Results.UnitResults units = null, Options options = null, List<FemDesign.GenericClasses.IStructureElement> elements = null, bool timeStamp = false) where T : Results.IResult
        {
            return _readResults<T>(p => true, null, null, timeStamp, elements, units, options);
        }

        /// <summary>
        /// Retreive load case results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="loadCases">Optional. Load case names for which the results should be return.</param>
        /// <param name="elements">Optional. Structural elements for which the results should be return.</param>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <param name="options">Optional. Options to set up the output location.</param>
        /// <param name="timeStamp">If true, the current time will be included in the file names.</param>
        /// <returns>List of results of type <typeparamref name="T"/> if any could be retrieved. If the model has no results of type <typeparamref name="T"/> or cannot access them at the moment, then the list will be empty.</returns>
        /// <exception cref="ArgumentException"></exception>
        internal List<T> _getLoadCaseResults<T>(List<string> loadCases = null, List<FemDesign.GenericClasses.IStructureElement> elements = null, Results.UnitResults units = null, Options options = null, bool timeStamp = false) where T : Results.IResult
        {
            return _readResults<T>(p => p.IsLoadCase() == true, loadCases, null, timeStamp, elements, units, options);
        }

        /// <summary>
        /// Retreive load combination results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="loadCombinations">Optional. Load combination names for which the results should be return.</param>
        /// <param name="elements">Optional. Structural elements for which the results should be return.</param>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <param name="options">Optional. Options to set up the output location.</param>
        /// <param name="timeStamp">If true, the current time will be included in the file names.</param>
        /// <returns>List of results of type <typeparamref name="T"/> if any could be retrieved. If the model has no results of type <typeparamref name="T"/> or cannot access them at the moment, then the list will be empty.</returns>
        /// <exception cref="ArgumentException"></exception>
        internal List<T> _getLoadCombinationResults<T>(List<string> loadCombinations = null, List<FemDesign.GenericClasses.IStructureElement> elements = null, Results.UnitResults units = null, Options options = null, bool timeStamp = false) where T : Results.IResult
        {
            return _readResults<T>(p => p.IsLoadCombination() == true, loadCombinations, null, timeStamp, elements, units, options);
        }

        /// <summary>
        /// Retreive all of the quantity estimation results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <param name="timeStamp">If true, the current time will be included in the file names.</param>
        /// <returns></returns>
        internal List<T> _getQuantities<T>(Results.UnitResults units = null, bool timeStamp = false) where T : Results.IQuantityEstimationResult
        {
            return _readResults<T>(p => p.IsQuantityEstimation() == true, null, null, timeStamp, null, units);
        }

        /// <summary>
        /// Retreive the stability results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="loadCombinations">Optional. Load combination names for which the results should be return.</param>
        /// <param name="shapeIds">Optional. Shape identifiers for which the results should be return.</param>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <param name="options">Optional. Options to set up the output location.</param>
        /// <param name="timeStamp">If true, the current time will be included in the file names.</param>
        /// <returns>List of results of type <typeparamref name="T"/> if any could be retrieved. If the model has no results of type <typeparamref name="T"/> or cannot access them at the moment, then the list will be empty.</returns>
        /// <exception cref="ArgumentException"></exception>
        internal List<T> _getStabilityResults<T>(List<string> loadCombinations = null, List<int> shapeIds = null, Results.UnitResults units = null, Options options = null, bool timeStamp = false) where T : IResult
        {
            List<T> results = _readResults<T>(p => true, null, null, timeStamp, null, units, options);   // Temporary solution. loadCombinations & shapeIds inputs are null to get all of the results. This part must be updated later. Related issue: https://github.com/strusoft/femdesign-api/issues/876 

            // Filter results by load combination and shape identifier
            string loadCombPropertyName;
            string shapeIdPropertyName;

            if (typeof(T) == typeof(NodalBucklingShape))
            {
                loadCombPropertyName = nameof(NodalBucklingShape.CaseIdentifier);
                shapeIdPropertyName = nameof(NodalBucklingShape.Shape);
            }
            else if (typeof(T) == typeof(CriticalParameter))
            {
                loadCombPropertyName = nameof(CriticalParameter.CaseIdentifier);
                shapeIdPropertyName = nameof(CriticalParameter.Shape);
            }
            else
            {
                throw new ArgumentException("This method cannot be used with the specified type.");
            }

            if (loadCombinations != null)
            {
                results = Results.Utils.UtilResultMethods.FilterResultsByLoadCombination(results, loadCombPropertyName, loadCombinations);
            }
            if (shapeIds != null)
            {
                results = Results.Utils.UtilResultMethods.FilterResultsByShapeId(results, shapeIdPropertyName, shapeIds);
            }

            return results;
        }

        private string _outputDir;
        public string OutputDir
        {
            get { return _outputDir; }
            set
            {
                if (string.IsNullOrEmpty(value)) // Use temp dir
                {
                    _outputDir = Path.Combine(Directory.GetCurrentDirectory(), "FEM-Design API");
                    //_outputDirsToBeDeleted.Add(_outputDir);
                }
                else // Use given directory
                    _outputDir = Path.GetFullPath(value);
            }
        }
        private List<string> _outputDirsToBeDeleted = new List<string>();
    }

    /*
    FEM-Design usage with named pipe.

    To initiate:
    1:  Create a WIN32 named pipe for duplex mode, message oriented
    1a: optional: Create another pipe for back channel, named appending 'b'.
    2:  Launch FD with command line argument `/p Name` passing the name you used at creation. FD will open it right at start and exit if can't. After successful launch it listens to commands while the usual interface is active. You can combine it with the windowless / minimized mode to hide the window. It also attaches to the back channel pipe at this moment, if unable, all output is permanently disabled.
    3:  Send commands through the pipe.
    4:  FD will exit if 'exit' command received or the pipe is closed on this end.

    FD only reads the main pipe and only writes the back channel (if supplied), allowing this end to never read.
    While the pipe is duplexand, it can be used in both direction. If it gets clogged in one direction (by not reading what the other end sends), the write can get blocked too.
    The document recommends using another pipe for a back channel.
    By default nothing is written to the back channel, you need to set output level (using verbosity, v, comand) or use commands with implicit reply.
    FD buffers all outgoing messages till they can be sent over, if this end is lazy to read, it will not clog, however they will accumulate in memory.
     */
    internal class PipeConnection : IDisposable
    {
        /// <summary>
        /// Connect to FEM-Design using Named Pipe
        /// </summary>
        /// <param name="pipeBaseName"></param>
        /// <exception cref="Exception"></exception>
        public PipeConnection(string pipeBaseName = "FdPipe1")
        {
            // Encoding that allow to have special character
            // https://nicolaiarocci.com/how-to-read-windows-1252-encoded-files-with-.netcore-and-.net5-/
            //_encoding = System.Text.Encoding.GetEncoding(1252);
            //_encoding = System.Text.Encoding.ASCII;
            _encoding = System.Text.Encoding.UTF8;

            string input_name = pipeBaseName;
            string output_name = pipeBaseName + "b";

            _inputPipe = create_pipe(input_name);
            _outputPipe = create_pipe(output_name);

            _startOutputThread();

            // this is what check status does...
            if (_inputPipe == null) { throw new Exception("setup failed"); }
        }

        /// <summary>
        /// Wait For Connection.
        /// </summary>
        public void WaitForConnection()
        {
            _inputPipe.WaitForConnection();
        }

        /// <summary>
        /// Represents a callback.
        /// </summary>
        /// <param name="output">the output.</param>
        public delegate void OnOutputEvent(string output);
        /// <summary>
        /// Gets or sets the on output.
        /// </summary>
        public OnOutputEvent OnOutput { get; set; } = null;

        /// <summary>
        /// Send command to FEM-Design using the named pipe.
        /// </summary>
        /// <param name="command">Command to excecute in FEM-Design.</param>
        public void Send(string command)
        {
            /*
            The command format is
            [!]<cmd> [args]
            there is no delimiter at the end, the pipe message counts.
            FEM-Design reads the pipe immediately and puts the commands in a queue. The queue is processed when it's READYSTATE
            is ready for another command, finishing execution of the previous or a current script.
            
            The !requests out of bound execution. That is not supported by very command and mainly
            serves to manipulate the queue itself, verbosity or check the communicaiton is alive.
        
            Messages are text in 9bit ANSI (codepage), limited to 4096 bytes.

            Commands:
        
            exit
            Stop the FD process
        
            detach
            Close the pipe and continue in normal interface mode
        
            clear [in|out]
            Flush the FD mesage queue for the direction, or both if send without in or out parameters.
            Has no Effect on what is already issued to the pipe
        
            echo [txt]
            Write txt to output
        
            stat
            Write queue and processing status to output
        
            v [N]
            Set verbosity control (bits)
               1: Enable basic output
               2: Echo all INPUT commands
               4: FEM-Design log-lines
               8: Script log lines
              16: Calculation window messages (except fortran)
             *32: Progress window title
        
            Commands echo and stat always creates output, otherwise nothing is written at V = 0
            * Not yet supported
        
            run [scriptfile]
            Execute script as from tools / run script menu.
            *Note: When using Unicode commands, add the suffix 'UTF8'. E.g.: "runUTF8 [scriptfile]".
        
            cmd [command]
            Execute command as if typed into the command window. No warranty!
            *Note: When using Unicode commands, add the suffix 'UTF8'. E.g.: "cmdUTF8 [command]".
        
            esc
            Escape during calculation to break/stop it.
            */

            if (_inputPipe.CanWrite == false) throw new Exception("Can't write to pipe");
            var buffer = _encoding.GetBytes(command);
            try
            {
                _inputPipe.Write(buffer, 0, buffer.Length);
                _inputPipe.Flush();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Cannot access a closed pipe.")
                {
                    throw new Exception("'FEM-Design Connection' has been closed! Open a new FEM-Design connection if you want to comunicate with FEM-Design.");
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        internal void WaitForCommandToFinish()
        {
            var guid = Guid.NewGuid();
            this.Send("echo " + guid);
            this._waitForOutput(guid);
        }

        internal async Task WaitForCommandToFinishAsync()
        {
            var guid = Guid.NewGuid();
            this.Send("echo " + guid);
            await this._waitForOutputAsync(guid);
        }

        // ----------------------------------------------------------------------------------------

        /// <summary>
        /// Disconnect.
        /// </summary>
        public void Disconnect()
        {
            _inputPipe.Disconnect();
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            _inputPipe.Dispose();
            _outputPipe.Dispose();
            _disposeWorker();
        }

        // ----------------------------------------------------------------------------------------

        private static NamedPipeServerStream create_pipe(string name)
        {
            // it seems that c# autoprefixes the \\.\pipe\ string, no need to add
#pragma warning disable CA1416 // Validate platform compatibility
            var pipe = new NamedPipeServerStream(name,
                PipeDirection.InOut,
                NamedPipeServerStream.MaxAllowedServerInstances,
                PipeTransmissionMode.Message,
                PipeOptions.None,
                BUFFER_SIZE, BUFFER_SIZE
                );
#pragma warning restore CA1416 // Validate platform compatibility
            return pipe;
        }

        private void _waitForOutput(Guid guid)
        {
            this._waitForOutput(guid.ToString());
        }

        private async Task _waitForOutputAsync(Guid guid)
        {
            await this._waitForOutputAsync(guid.ToString());
        }

        private void _waitForOutput(string output)
        {
            _waitForOutputAsync(output).Wait();
        }

        private async Task _waitForOutputAsync(string output)
        {
            var tcs = new TaskCompletionSource<bool>();
            void onOutput(string msg)
            {
                if (msg == output)
                {
                    this.OnOutput -= onOutput;
                    tcs.SetResult(true);
                }
            }
            this.OnOutput += onOutput;

            await tcs.Task;
        }

        // ----------------------------------------------------------------------------------------

        private void _disposeWorker()
        {
            _sendOutputToEvent = false;
            if (_outputWorker.IsBusy)
            {
                _outputWorker.CancelAsync();
            }
        }

        private void _startOutputThread()
        {
            _outputWorker.WorkerSupportsCancellation = true;
            _outputWorker.DoWork += _onOutputThread;

            _outputWorker.WorkerReportsProgress = true;
            _outputWorker.ProgressChanged += _onOuputThreadEvent;

            if (_outputWorker.IsBusy == true) { throw new Exception("background thread already started... bug?"); }
            _outputWorker.RunWorkerAsync();
        }

        private int GetProcessCount()
        {
            return Process.GetProcessesByName("fd3dstruct").Length;
        }

        private void _onOutputThread(object sender, DoWorkEventArgs e)
        {
            try
            {
                _outputPipe.WaitForConnection();
                while (_outputWorker.CancellationPending == false)
                {
                    var buffer = new byte[BUFFER_SIZE + 1];
                    var read = _outputPipe.Read(buffer, 0, BUFFER_SIZE);
                    var line = _encoding.GetString(buffer, 0, read);
                    if (line == null) { continue; }

                    _outputWorker.ReportProgress(0, line);

                    if (GetProcessCount() < 1)
                    {
                        this.Dispose();
                        throw new Exception("FEM-Design has been closed!");
                    };
                }
            }
            catch (Exception ex)
            {
                if (ex is Exception)
                {
                    throw new Exception(ex.Message);
                }
                if (ex is Exception)
                {
                    _outputWorker.ReportProgress(1, ex);
                }
            }

        }

        private void _onOuputThreadEvent(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is string line)
            {
                if (_sendOutputToEvent)
                {
                    OnOutput?.Invoke(line);
                }
            }
            else if (e.UserState is Exception ex)
            {
                if (_sendOutputToEvent)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("invalid progress object");
            }
        }

        //internal void Disconnect()
        //{
        //    this._inputPipe.Disconnect();
        //    this._outputPipe.Disconnect();
        //}

        // ----------------------------------------------------------------------------------------

        internal readonly NamedPipeServerStream _inputPipe;
        internal readonly NamedPipeServerStream _outputPipe;
        readonly BackgroundWorker _outputWorker = new BackgroundWorker();
        private readonly System.Text.Encoding _encoding;
        private const int BUFFER_SIZE = 4096;
        private bool _sendOutputToEvent = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Verbosity
    {
        EchoAndStatOnly = 0,
        /// <summary>
        /// Enable basic output
        /// </summary>
        BasicOnly = 1,
        /// <summary>
        /// Echo all INPUT commands
        /// </summary>
        InputOnly = 2,
        /// <summary>
        /// FEM-Design log-lines
        /// </summary>
        LogLinesOnly = 4,
        /// <summary>
        /// Script log lines
        /// </summary>
        ScriptLogLinesOnly = 8,
        /// <summary>
        /// Calculation window messages (except fortran)
        /// </summary>
        CalculationMessagesOnly = 16,
        /// <summary>
        /// Progress window title
        /// </summary>
        ProgressWindowTitleOnly = 32,

        None = EchoAndStatOnly,
        Low = BasicOnly | InputOnly,
        Normal = ScriptLogLinesOnly | CalculationMessagesOnly,
        High = BasicOnly | InputOnly | LogLinesOnly | ScriptLogLinesOnly | CalculationMessagesOnly,
        All = BasicOnly | InputOnly | LogLinesOnly | ScriptLogLinesOnly | CalculationMessagesOnly | ProgressWindowTitleOnly,
    }

    public static class OutputFileHelper
    {
        private const string _scriptsDirectory = "scripts";
        private const string _resultsDirectory = "results";
        private const string _bscDirectory = "bsc";

        private const string _logFileName = "logfile.log";
        private const string _struxmlFileName = "model.struxml";
        private const string _strFileName = "model.str";
        private const string _docxFileName = "model.docx";

        private const string _configFileName = "config.xml";
        private const string _globalConfigFileName = "globalConfig.xml";

        private const string _intSrfFileName = "intSrf.txt";

        private const string _fdscriptFileExtension = ".fdscript";
        private const string _bscFileExtension = ".bsc";
        private const string _csvFileExtension = ".csv";
        /// <summary>
        /// Gets the logfile path.
        /// </summary>
        /// <param name="baseDir">the base dir.</param>
        /// <returns>The result.</returns>
        public static string GetLogfilePath(string baseDir)
        {
            string logfilePath = Path.Combine(baseDir, _logFileName);
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);
            return Path.GetFullPath(Path.Combine(baseDir, _logFileName));
        }

        /// <summary>
        /// Gets the int srffile path.
        /// </summary>
        /// <param name="baseDir">the base dir.</param>
        /// <returns>The result.</returns>
        public static string GetIntSrffilePath(string baseDir)
        {
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);
            return Path.GetFullPath(Path.Combine(baseDir, _intSrfFileName));
        }

        /// <summary>
        /// Gets the configfile path.
        /// </summary>
        /// <param name="baseDir">the base dir.</param>
        /// <returns>The result.</returns>
        public static string GetConfigfilePath(string baseDir)
        {
            string dir = Path.Combine(baseDir, _scriptsDirectory);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return Path.GetFullPath(Path.Combine(dir, _configFileName));
        }

        /// <summary>
        /// Gets the global configfile path.
        /// </summary>
        /// <param name="baseDir">the base dir.</param>
        /// <returns>The result.</returns>
        public static string GetGlobalConfigfilePath(string baseDir)
        {
            string dir = Path.Combine(baseDir, _scriptsDirectory);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return Path.GetFullPath(Path.Combine(dir, _globalConfigFileName));
        }

        /// <summary>
        /// Gets the struxml path.
        /// </summary>
        /// <param name="baseDir">the base dir.</param>
        /// <param name="modelName">the model name.</param>
        /// <returns>The result.</returns>
        public static string GetStruxmlPath(string baseDir, string modelName = null)
        {
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);
            string path;
            if (string.IsNullOrEmpty(modelName))
                path = Path.GetFullPath(Path.Combine(baseDir, _struxmlFileName));
            else
                path = Path.GetFullPath(Path.Combine(baseDir, Path.ChangeExtension(Path.GetFileName(modelName), "struxml")));

            return path;
        }

        /// <summary>
        /// Gets the docx path.
        /// </summary>
        /// <param name="baseDir">the base dir.</param>
        /// <param name="modelName">the model name.</param>
        /// <returns>The result.</returns>
        public static string GetDocxPath(string baseDir, string modelName = null)
        {
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);
            string path;
            if (string.IsNullOrEmpty(modelName))
                path = Path.GetFullPath(Path.Combine(baseDir, _docxFileName));
            else
                path = Path.GetFullPath(Path.Combine(baseDir, Path.ChangeExtension(Path.GetFileName(modelName), "docx")));

            return path;
        }

        /// <summary>
        /// Gets the str path.
        /// </summary>
        /// <param name="baseDir">the base dir.</param>
        /// <param name="modelName">the model name.</param>
        /// <returns>The result.</returns>
        public static string GetStrPath(string baseDir, string modelName = null)
        {
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);
            string path;
            if (string.IsNullOrEmpty(modelName))
                path = Path.GetFullPath(Path.Combine(baseDir, _strFileName));
            else
                path = Path.GetFullPath(Path.Combine(baseDir, Path.ChangeExtension(Path.GetFileName(modelName), "str")));

            return path;
        }
        /// <summary>
        /// Gets the bsc path.
        /// </summary>
        /// <param name="baseDir">the base dir.</param>
        /// <param name="fileName">the file name.</param>
        /// <returns>The result.</returns>
        public static string GetBscPath(string baseDir, string fileName)
        {
            string dir = Path.Combine(baseDir, _scriptsDirectory, _bscDirectory);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            fileName = Path.ChangeExtension(fileName, _bscFileExtension);
            string path = Path.GetFullPath(Path.Combine(dir, fileName));
            return path;
        }
        /// <summary>
        /// Gets the csv path.
        /// </summary>
        /// <param name="baseDir">the base dir.</param>
        /// <param name="fileName">the file name.</param>
        /// <returns>The result.</returns>
        public static string GetCsvPath(string baseDir, string fileName)
        {
            string dir = Path.Combine(baseDir, _resultsDirectory);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            fileName = Path.ChangeExtension(fileName, _csvFileExtension);
            string path = Path.GetFullPath(Path.Combine(dir, fileName));
            return path;
        }
        /// <summary>
        /// Gets the fd script path.
        /// </summary>
        /// <param name="baseDir">the base dir.</param>
        /// <param name="fileName">the file name.</param>
        /// <returns>The result.</returns>
        public static string GetFdScriptPath(string baseDir, string fileName = "script")
        {
            string dir = Path.Combine(baseDir, _scriptsDirectory);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);            
            fileName = Path.GetFileName(Path.ChangeExtension(fileName, _fdscriptFileExtension));
            string path = Path.GetFullPath(Path.Combine(dir, fileName));
            return path;
        }

        /// <summary>
        /// Determines whether ascii.
        /// </summary>
        /// <param name="filePath">the file path.</param>
        /// <returns>The result.</returns>
        public static bool IsASCII(string filePath)
        {
            // Encode the string using ASCII encoding
            byte[] asciiBytes = Encoding.ASCII.GetBytes(filePath);

            // Decode the ASCII-encoded bytes back to a string
            string decodedString = Encoding.ASCII.GetString(asciiBytes);

            // Compare the original string with the decoded string
            return filePath.Equals(decodedString);
        }

    }
}
