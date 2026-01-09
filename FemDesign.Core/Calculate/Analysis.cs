// https://strusoft.com/
using FemDesign.Shells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// </summary>
    public partial class Analysis
    {
        // elements
        /// <summary>
        /// Gets or sets the stage.
        /// </summary>
        [XmlElement("stage")]
        public Stage Stage { get; set; }

        /// <summary>
        /// Gets or sets the comb.
        /// </summary>
        [XmlElement("comb")]
        public Comb Comb { get; set; }

        /// <summary>
        /// Gets or sets the stability.
        /// </summary>
        [XmlIgnore]
        public Stability Stability { get; set; }

        /// <summary>
        /// Gets or sets the imperfection.
        /// </summary>
        [XmlIgnore]
        public Imperfection Imperfection { get; set; }

        /// <summary>
        /// Gets or sets the freq.
        /// </summary>
        [XmlElement("freq")]
        public Freq Freq { get; set; }

        /// <summary>
        /// Gets or sets the footfall.
        /// </summary>
        [XmlElement("footfall")]
        public Footfall Footfall { get; set; }

        /// <summary>
        /// Gets or sets the bedding.
        /// </summary>
        [XmlElement("bedding")]
        public Bedding Bedding { get; set; }

        /// <summary>
        /// Gets or sets the ground acc.
        /// </summary>
        [XmlElement("thgroundacc")]
        public GroundAcc GroundAcc { get; set; }

        /// <summary>
        /// Gets or sets the ex force.
        /// </summary>
        [XmlElement("thexforce")]
        public ExcitationForce ExForce { get; set; }

        /// <summary>
        /// Gets or sets the periodic ex.
        /// </summary>
        [XmlElement("periodicexc")]
        public PeriodicExcitation PeriodicEx { get; set; }


        // attributes
        /// <summary>
        /// Gets or sets the calc case.
        /// </summary>
        [XmlAttribute("calcCase")]
        public int _calcCase; 
        [XmlIgnore]
        public bool CalcCase
        {
            get
            {
                return Convert.ToBoolean(this._calcCase);
            }
            set
            {
                this._calcCase = Convert.ToInt32(value);
                if (value == true && this.Comb == null)
                    this.Comb = Comb.Default();
            }
        }

        /// <summary>
        /// Gets or sets the calc c stage.
        /// </summary>
        [XmlAttribute("calcCstage")]
        public int _calcCStage; 
        [XmlIgnore]
        public bool CalcCStage
        {
            get
            {
                return Convert.ToBoolean(this._calcCStage);
            }
            set
            {
                this._calcCStage = Convert.ToInt32(value);
                if (value == true && this.Stage == null)
                    this.Stage = Stage.Default();
            }
        }

        /// <summary>
        /// Gets or sets the calc impf.
        /// </summary>
        [XmlAttribute("calcImpf")]
        public int _calcImpf; 
        [XmlIgnore]
        public bool CalcImpf
        {
            get
            {
                return Convert.ToBoolean(this._calcImpf);
            }
            set
            {
                this._calcImpf = Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Gets or sets the calc comb.
        /// </summary>
        [XmlAttribute("calcComb")]
        public int _calcComb;
        [XmlIgnore]
        public bool CalcComb
        {
            get
            {
                return Convert.ToBoolean(this._calcComb);
            }
            set
            {
                this._calcComb = Convert.ToInt32(value);
                if (value == true && this.Comb == null)
                    this.Comb = Comb.Default();
            }
        }

        /// <summary>
        /// Gets or sets the calc g max.
        /// </summary>
        [XmlAttribute("calcGmax")]
        public int _calcGMax; 
        [XmlIgnore]
        public bool CalcGMax
        {
            get
            {
                return Convert.ToBoolean(this._calcGMax);
            }
            set
            {
                this._calcGMax = Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Gets or sets the calc stab.
        /// </summary>
        [XmlAttribute("calcStab")]
        public int _calcStab;
        [XmlIgnore]
        public bool CalcStab
        {
            get
            {
                return Convert.ToBoolean(this._calcStab);
            }
            set
            {
                this._calcStab = Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Gets or sets the calc freq.
        /// </summary>
        [XmlAttribute("calcFreq")]
        public int _calcFreq;
        [XmlIgnore]
        public bool CalcFreq
        {
            get
            {
                return Convert.ToBoolean(this._calcFreq);
            }
            set
            {
                this._calcFreq = Convert.ToInt32(value);
                if (value == true && this.Freq == null)
                    this.Freq = Freq.Default();
            }
        }

        /// <summary>
        /// Gets or sets the calc seis.
        /// </summary>
        [XmlAttribute("calcSeis")]
        public int _calcSeis;
        [XmlIgnore]
        public bool CalcSeis
        {
            get
            {
                return Convert.ToBoolean(this._calcSeis);
            }
            set
            {
                this._calcSeis = Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Gets or sets the calc footfall.
        /// </summary>
        [XmlAttribute("calcFootfall")]
        public int _calcFootfall;
        [XmlIgnore]
        public bool CalcFootfall
        {
            get
            {
                return Convert.ToBoolean(this._calcFootfall);
            }
            set
            {
                this._calcFootfall = Convert.ToInt32(value);
                if(value == true && this.Footfall == null)
                    this.Footfall = Footfall.Default();
            }
        }



        /// <summary>
        /// Gets or sets the calc bedding.
        /// </summary>
        [XmlAttribute("calcBedding")]
        public int _calcBedding;
        [XmlIgnore]
        public bool CalcBedding
        {
            get
            {
                return Convert.ToBoolean(this._calcBedding);
            }
            set
            {
                this._calcBedding = Convert.ToInt32(value);
                if (value == true && this.Bedding == null)
                    this.Bedding = Bedding.Default();
            }
        }






        /// <summary>
        /// Gets or sets the calc ground acc.
        /// </summary>
        [XmlAttribute("calcThGroundAcc")]
        public int _calcGroundAcc;
        [XmlIgnore]
        public bool CalcGroundAcc
        {
            get
            {
                return Convert.ToBoolean(this._calcGroundAcc);
            }
            set
            {
                this._calcGroundAcc = Convert.ToInt32(value);
                if (value == true && this.GroundAcc == null)
                    this.GroundAcc = GroundAcc.Default();
            }
        }

        /// <summary>
        /// Gets or sets the calc excitation force.
        /// </summary>
        [XmlAttribute("calcThExforce")]
        public int _calcExcitationForce;
        [XmlIgnore]
        public bool CalcExcitationForce
        {
            get
            {
                return Convert.ToBoolean(this._calcExcitationForce);
            }
            set
            {
                this._calcExcitationForce = Convert.ToInt32(value);
                if (value == true && this.ExForce == null)
                    this.ExForce = Calculate.ExcitationForce.Default();
            }
        }

        /// <summary>
        /// Gets or sets the calc periodic excitation.
        /// </summary>
        [XmlAttribute("calcPeriodicExc")]
        public int _calcPeriodicExcitation;
        [XmlIgnore]
        public bool CalcPeriodicExcitation
        {
            get
            {
                return Convert.ToBoolean(this._calcPeriodicExcitation);
            }
            set
            {
                this._calcPeriodicExcitation = Convert.ToInt32(value);
                if (value == true && this.PeriodicEx == null)
                    this.PeriodicEx = Calculate.PeriodicExcitation.Default();
            }
        }

        /// <summary>
        /// Gets or sets the calc design.
        /// </summary>
        [XmlAttribute("calcDesign")]
        public int _calcDesign;
        [XmlIgnore]
        public bool CalcDesign
        {
            get
            {
                return Convert.ToBoolean(this._calcDesign);
            }
            set
            {
                this._calcDesign = Convert.ToInt32(value);

            }
        }

        /// <summary>
        /// Gets or sets the elem fine.
        /// </summary>
        [XmlAttribute("elemfine")]
        public int _elemFine;
        [XmlIgnore]
        public bool ElemFine
        {
            get
            {
                return Convert.ToBoolean(this._elemFine);
            }
            set
            {
                this._elemFine = Convert.ToInt32(value);
            }
        }
        
        /// <summary>
        /// Gets or sets the diaphragm.
        /// </summary>
        [XmlAttribute("diaphragm")]
        public int _diaphragm;
        [XmlIgnore]
        public DiaphragmType Diaphragm
        {
            get
            {
                return (DiaphragmType)this._diaphragm;
            }
            set
            {
                this._diaphragm = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the peak smoothing.
        /// </summary>
        [XmlAttribute("peaksmoothing")]
        public int _peakSmoothing; 
        [XmlIgnore]
        public bool PeakSmoothing
        {
            get
            {
                return Convert.ToBoolean(this._peakSmoothing);
            }
            set
            {
                this._peakSmoothing = Convert.ToInt32(value);
            }
        }
 
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Analysis()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Analysis"/> class.
        /// </summary>
        /// <param name="stage">the stage.</param>
        /// <param name="stability">the stability.</param>
        /// <param name="imperfection">the imperfection.</param>
        /// <param name="comb">the comb.</param>
        /// <param name="freq">the freq.</param>
        /// <param name="footfall">the footfall.</param>
        /// <param name="bedding">the bedding.</param>
        /// <param name="groundAcc">the ground acc.</param>
        /// <param name="exForce">the ex force.</param>
        /// <param name="periodicEx">the periodic ex.</param>
        /// <param name="calcCase">the calc case.</param>
        /// <param name="calcCStage">the calc c stage.</param>
        /// <param name="calcImpf">the calc impf.</param>
        /// <param name="calcComb">the calc comb.</param>
        /// <param name="calcGMax">the calc g max.</param>
        /// <param name="calcStab">the calc stab.</param>
        /// <param name="calcFreq">the calc freq.</param>
        /// <param name="calcSeis">the calc seis.</param>
        /// <param name="calcFootfall">the calc footfall.</param>
        /// <param name="calcBedding">the calc bedding.</param>
        /// <param name="calcGroundAcc">the calc ground acc.</param>
        /// <param name="calcExForce">the calc ex force.</param>
        /// <param name="calcPeriodicEx">the calc periodic ex.</param>
        /// <param name="calcDesign">the calc design.</param>
        /// <param name="elemFine">the elem fine.</param>
        /// <param name="diaphragm">the diaphragm.</param>
        /// <param name="peakSmoothing">the peak smoothing.</param>
        [Obsolete("Use Analysis(..., DiaphragmType diaphragm, ...) instead. This constructor will be removed in the next version.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Analysis(Calculate.Stage stage = null, Stability stability = null, Imperfection imperfection = null, Comb comb = null, Freq freq = null, Footfall footfall = null, Bedding bedding = null, GroundAcc groundAcc = null, ExcitationForce exForce = null, PeriodicExcitation periodicEx = null, bool calcCase = false, bool calcCStage = false, bool calcImpf = false, bool calcComb = false, bool calcGMax = false, bool calcStab = false, bool calcFreq = false, bool calcSeis = false, bool calcFootfall = false, bool calcBedding = false, bool calcGroundAcc = false, bool calcExForce = false, bool calcPeriodicEx = false, bool calcDesign = false, bool elemFine = true, int diaphragm = 0, bool peakSmoothing = false) 
            : this(stage, stability, imperfection, comb, freq, footfall, bedding, groundAcc, exForce, periodicEx, calcCase, calcCStage, calcImpf, calcComb, calcGMax, calcStab, calcFreq, calcSeis, calcFootfall, calcBedding, calcGroundAcc, calcExForce, calcPeriodicEx, calcDesign, elemFine, (DiaphragmType)diaphragm, peakSmoothing)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Analysis"/> class.
        /// </summary>
        /// <param name="stage">the stage.</param>
        /// <param name="stability">the stability.</param>
        /// <param name="imperfection">the imperfection.</param>
        /// <param name="comb">the comb.</param>
        /// <param name="freq">the freq.</param>
        /// <param name="footfall">the footfall.</param>
        /// <param name="bedding">the bedding.</param>
        /// <param name="groundAcc">the ground acc.</param>
        /// <param name="exForce">the ex force.</param>
        /// <param name="periodicEx">the periodic ex.</param>
        /// <param name="calcCase">the calc case.</param>
        /// <param name="calcCStage">the calc c stage.</param>
        /// <param name="calcImpf">the calc impf.</param>
        /// <param name="calcComb">the calc comb.</param>
        /// <param name="calcGMax">the calc g max.</param>
        /// <param name="calcStab">the calc stab.</param>
        /// <param name="calcFreq">the calc freq.</param>
        /// <param name="calcSeis">the calc seis.</param>
        /// <param name="calcFootfall">the calc footfall.</param>
        /// <param name="calcBedding">the calc bedding.</param>
        /// <param name="calcGroundAcc">the calc ground acc.</param>
        /// <param name="calcExForce">the calc ex force.</param>
        /// <param name="calcPeriodicEx">the calc periodic ex.</param>
        /// <param name="calcDesign">the calc design.</param>
        /// <param name="elemFine">the elem fine.</param>
        /// <param name="diaphragm">the diaphragm.</param>
        /// <param name="peakSmoothing">the peak smoothing.</param>
        public Analysis(Calculate.Stage stage = null, Stability stability = null, Imperfection imperfection = null, Comb comb = null, Freq freq = null, Footfall footfall = null, Bedding bedding = null, GroundAcc groundAcc = null, ExcitationForce exForce = null, PeriodicExcitation periodicEx = null, bool calcCase = false, bool calcCStage = false, bool calcImpf = false, bool calcComb = false, bool calcGMax = false, bool calcStab = false, bool calcFreq = false, bool calcSeis = false, bool calcFootfall = false, bool calcBedding = false, bool calcGroundAcc = false, bool calcExForce = false, bool calcPeriodicEx = false, bool calcDesign = false, bool elemFine = true, DiaphragmType diaphragm = DiaphragmType.None, bool peakSmoothing = false)
        {
            this.Stage = stage;
            this.Comb = comb;
            this.Stability = stability;
            this.Imperfection = imperfection;
            this.Freq = freq;
            this.Footfall = footfall;
            this.Bedding = bedding;
            this.GroundAcc = groundAcc;
            this.ExForce = exForce;
            this.PeriodicEx = periodicEx;

            this.CalcCase = calcCase;
            this.CalcComb = calcComb;
            this.CalcGMax = calcGMax;
            this.CalcCStage = calcCStage;
            this.CalcImpf = calcImpf == false ? false : (imperfection != null);
            this.CalcStab = calcStab == false ? false : (stability != null);
            this.CalcFreq = calcFreq;
            this.CalcSeis = calcSeis;
            this.CalcFootfall = calcFootfall;
            this.CalcBedding = calcBedding;
            this.CalcGroundAcc = calcGroundAcc;
            this.CalcExcitationForce = calcExForce;
            this.CalcPeriodicExcitation = calcPeriodicEx;
            this.CalcDesign = calcDesign;
            this.ElemFine = elemFine;
            this.Diaphragm = diaphragm;
            this.PeakSmoothing = peakSmoothing;

            _validateAnalysisSettings();
        }

        private void _validateAnalysisSettings()
        {
            if (this.CalcStab == true && this.Stability == null)
                throw new Exception("calcStab == True. Stability must be defined!");

            if (this.CalcImpf == true && this.Imperfection == null)
                throw new Exception("calcImpf == True. Imperfection must be defined!");
            
            if (this.Stability != null && this.Imperfection != null)
                throw new Exception("Stability and Imperfection can not be run simultaneously. Consider running only Imperfection!");

            if (this.CalcCStage == true && this.Stage == null)
                throw new Exception("calcCStage == True. Stage must be defined!");

            if (this.CalcFootfall == true && this.Footfall == null)
                throw new Exception("calcFootfall == True. Footfall must be defined!");

            if (this.CalcBedding == true && this.Bedding == null)
                throw new Exception("CalcBedding == True. Bedding must be defined!");

            if (this.CalcGroundAcc == true && this.GroundAcc == null)
                throw new Exception("calcGroundAcc == True. GroundAcc must be defined!");

            if (this.CalcExcitationForce == true && this.ExForce == null)
                throw new Exception("calcExForce == True. ExForce must be defined!");

            if (this.CalcPeriodicExcitation == true && this.PeriodicEx == null)
                throw new Exception("calcPeriodicEx == True. PeriodicEx must be defined!");
        }

        /// <summary>
        /// Define a static analysis.
        /// </summary>
        /// <param name="comb">Calculation parameters for load combinations.</param>
        /// <param name="calcCase">If true, the load case calculation will be executed.</param>
        /// <param name="calccomb">If true, the load combination calculation will be executed.</param>
        /// <returns></returns>
        public static Analysis StaticAnalysis(Comb comb = null, bool calcCase = true, bool calccomb = true)
        {
            comb = comb ?? Comb.Default();
            return new Analysis(comb: comb, calcCase: calcCase, calcComb: calccomb, diaphragm: DiaphragmType.None);
        }


        /// <summary>
        /// Define an eigenfrequencies analysis.
        /// </summary>
        /// <param name="numShapes">Number of shapes.</param>
        /// <param name="maxSturm">Max number of Sturm check steps (checking missing eigenvalues).</param>
        /// <param name="x">Consider masses in global x-direction.</param>
        /// <param name="y">Consider masses in global y-direction.</param>
        /// <param name="z">Consider masses in global z-direction.</param>
        /// <param name="top">Top of substructure. Masses on this level and below are not considered in Eigenfrequency</param>
        /// <returns></returns>
        public static Analysis Eigenfrequencies(int numShapes = 3, int maxSturm = 0, bool x = true, bool y = true, bool z = true, double top = -0.01)
        {
            var freqSettings = new Freq(numShapes, maxSturm, x, y, z, top);
            return new Analysis(freq: freqSettings, calcFreq: true, diaphragm: DiaphragmType.None);
        }

        /// <summary>
        /// Define an eigenfrequencies analysis.
        /// </summary>
        /// <param name="numShapes">Number of shapes.</param>
        /// <param name="autoIteration"></param>
        /// <param name="shapeNormalisation">Mode shape normalisation type.</param>
        /// <param name="maxSturm">Max number of Sturm check steps (checking missing eigenvalues).</param>
        /// <param name="x">Consider masses in global x-direction.</param>
        /// <param name="y">Consider masses in global y-direction.</param>
        /// <param name="z">Consider masses in global z-direction.</param>
        /// <param name="top">Top of substructure. Masses on this level and below are not considered in Eigenfrequency</param>
        /// <returns></returns>
        public static Analysis Eigenfrequencies(int numShapes = 3, int autoIteration = 0, ShapeNormalisation shapeNormalisation = ShapeNormalisation.Unit, int maxSturm = 0, bool x = true, bool y = true, bool z = true, double top = -0.01)
        {
            var freqSettings = new Freq(numShapes, autoIteration, shapeNormalisation, x, y, z, maxSturm, top);
            return new Analysis(freq: freqSettings, calcFreq: true, diaphragm: DiaphragmType.None);
        }

        /// <summary>
        /// Define an eigenfrequencies analysis.
        /// </summary>
        /// <param name="freqSettings"></param>
        /// <returns></returns>
        public static Analysis Eigenfrequencies(Freq freqSettings)
        {
            return new Analysis(freq: freqSettings, calcFreq: true, diaphragm: DiaphragmType.None);
        }

        /// <summary>
        /// Define ground acceleration analysis.
        /// </summary>
        /// <param name="levelAccSpectra">If true, the level acceleration response spectra calculation will be executed.</param>
        /// <param name="deltaT">Calculation parameter for Level acceleration spectra analysis.</param>
        /// <param name="tEnd">Calculation parameter for Level acceleration spectra analysis.</param>
        /// <param name="q">Calculation parameter for Level acceleration spectra analysis.</param>
        /// <param name="timeHistory">If true, the time history calculation will be executed.</param>
        /// <param name="step">the step.</param>
        /// <param name="lastMoment">Last time moment of the time history calculation.</param>
        /// <param name="method">Integration scheme method type.</param>
        /// <param name="alpha">'alpha' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="beta">'beta' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="dmpFactor">'ksi' damping factor.</param>
        /// <returns></returns>
        public static Analysis GroundAcceleration(bool levelAccSpectra = true, double deltaT = 0.2, double tEnd = 5.0, double q = 1.0, bool timeHistory = true, int step = 5, double lastMoment = 20.0, IntegrationSchemeMethod method = IntegrationSchemeMethod.Newmark, double alpha = 0.5, double beta = 0.25, double dmpFactor = 5.0)
        {
            GroundAcc grAccSettings = new GroundAcc(levelAccSpectra, deltaT, tEnd, q, timeHistory, step, lastMoment, method, alpha, beta, dmpFactor);
            return new Analysis(groundAcc: grAccSettings, calcGroundAcc: true, diaphragm: DiaphragmType.None);
        }

        /// <summary>
        /// Define excitation force analysis.
        /// </summary>
        /// <param name="step">the step.</param>
        /// <param name="lastMoment">Last time moment of the time history calculation.</param>
        /// <param name="method">Integration scheme method type.</param>
        /// <param name="alpha">'alpha' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="beta">'beta' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="dmpFactor">'ksi' damping factor.</param>
        /// <returns></returns>
        public static Analysis ExcitationForce(int step = 5, double lastMoment = 20.0, IntegrationSchemeMethod method = IntegrationSchemeMethod.Newmark, double alpha = 0.5, double beta = 0.25, double dmpFactor = 5.0)
        {
            ExcitationForce grAccSettings = new ExcitationForce(step, lastMoment, method, alpha, beta, dmpFactor);
            return new Analysis(exForce: grAccSettings, calcExForce: true, diaphragm: DiaphragmType.None);
        }

        /// <summary>
        /// Define periodic excitation analysis.
        /// </summary>
        /// <param name="deltaT">Calculation parameter.</param>
        /// <param name="timeEnd">Calculation parameter.</param>
        /// <param name="dmpType">Damping type.</param>
        /// <param name="alpha">'alpha' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="beta">'beta' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="dmpFactor">'ksi' damping factor.</param>
        /// <returns></returns>
        public static Analysis PeriodicExcitation(double deltaT = 0.01, double timeEnd = 5.0, DampingType dmpType = DampingType.Rayleigh, double alpha = 0.5, double beta = 0.25, double dmpFactor = 5.0)
        {
            PeriodicExcitation perExSettings = new PeriodicExcitation(deltaT, timeEnd, dmpType, alpha, beta, dmpFactor);
            return new Analysis(periodicEx: perExSettings, calcPeriodicEx: true, diaphragm: DiaphragmType.None);
        }

        /// <summary>
        /// Define an construction stages analysis.
        /// </summary>
        /// <param name="ghost"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Analysis ConstructionStages(bool ghost = false)
        {
            var stage = ghost ? Stage.GhostMethod() : Stage.TrackingMethod();
            return new Analysis(stage: stage, calcCStage: true, diaphragm: DiaphragmType.None);
        }

        // TODO (missing calculation parameters in .fdscript)
        private static Analysis FootFall(Footfall footfall)
        {
            var analisys = new Analysis(footfall: footfall, calcFootfall: true, diaphragm: DiaphragmType.None);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Internal method to transfer load combination calculation parameters from LoadCombination to Analysis.
        /// </summary>
        /// <param name="model"></param>
        public void SetLoadCombinationCalculationParameters(FemDesign.Model model)
        {
            this.Comb.CombItem.Clear();
            foreach(var loadComb in model.Entities.Loads.LoadCombinations)
            {
                var combItem = loadComb.CombItem ?? Calculate.CombItem.Default();
                this.Comb.CombItem.Add(combItem);
            }
            //this.Comb.CombItem.AddRange(model.Entities.Loads.LoadCombinations.Select(x => x.CombItem));
        }


        /// <summary>
        /// Sets the comb analysis.
        /// </summary>
        /// <param name="connection">the connection.</param>
        public void SetCombAnalysis(FemDesignConnection connection)
        {
            //this.Comb.CombItem.Clear();
            // ordered load combinations in the model
            var loadCombination = connection.GetLoadCombinations();

            var nullCombItems = this.Comb.CombItem.Where(x => x.CombName == null).Count();
            var namedCombItems = this.Comb.CombItem.Where(x => x.CombName != null).Count();

            if(nullCombItems != 0 && namedCombItems != 0)
                throw new Exception("CombItem in Analysis contains both named and unnamed CombItems. This is not allowed.");


            _setCombAnalysis(loadCombination.Values.ToList());
        }

        internal void _setCombAnalysis(List<Loads.LoadCombination> loadCombination)
        {
            var combItems = this.Comb.CombItem.DeepClone();
            this.Comb.CombItem.Clear();

            foreach (var element in loadCombination)
            {

                bool isFound = false;
                int i = 0;
                foreach (var combItem in combItems)
                {
                    var combName = combItem.CombName;
                    if (combName == element.Name)
                    {
                        var indexOf = loadCombination.Select(x => x.Name).ToList().IndexOf(combName);
                        this.Comb.CombItem.Add(combItem);
                        isFound = true;
                        break;
                    }
                    i++;
                }
                if (isFound == true)
                    continue;
                else
                    this.Comb.CombItem.Add(CombItem.Default());
            }
        }


        /// <summary>
        /// Sets the stability analysis.
        /// </summary>
        /// <param name="connection">the connection.</param>
        public void SetStabilityAnalysis(FemDesignConnection connection)
        {
            // ordered load combinations in the model
            var loadCombination = connection.GetLoadCombinations();

            _setStabilityAnalysis(loadCombination.Values.ToList());
        }

        internal void _setStabilityAnalysis(List<Loads.LoadCombination> loadCombination)
        {
            // check if comb is defined. if not, create a new one. if exist, clear it
            if (this.Comb == null)
                this.Comb = new Comb();
            else
                this.Comb.CombItem.Clear();



            foreach (var element in loadCombination)
            {
                bool isFound = false;
                int i = 0;
                foreach (var combName in this.Stability.CombNames)
                {
                    if (combName == element.Name)
                    {
                        var indexOf = loadCombination.Select(x => x.Name).ToList().IndexOf(combName);
                        this.Comb.CombItem.Add(CombItem.Stability(this.Stability.NumShapes[i]));
                        isFound = true;
                        break;
                    }
                    i++;
                }
                if (isFound == true)
                    continue;
                else
                    this.Comb.CombItem.Add(CombItem.Default());
            }
        }

        /// <summary>
        /// Sets the imperfection analysis.
        /// </summary>
        /// <param name="connection">the connection.</param>
        public void SetImperfectionAnalysis(FemDesignConnection connection)
        {
            // ordered load combinations in the model
            var loadCombination = connection.GetLoadCombinations();

            _setImperfectionAnalysis(loadCombination.Values.ToList());
        }

        internal void _setImperfectionAnalysis(List<Loads.LoadCombination> loadCombination)
        {
            //check if comb is defined. if not, create a new one. if exist, clear it
            if (this.Comb == null)
                this.Comb = new Comb();
            else
                this.Comb.CombItem.Clear();

            foreach (var element in loadCombination)
            {
                bool isFound = false;
                int i = 0;
                foreach (var combName in this.Imperfection.CombNames)
                {
                    if (combName == element.Name)
                    {
                        var indexOf = loadCombination.Select(x => x.Name).ToList().IndexOf(combName);
                        this.Comb.CombItem.Add(CombItem.Imperfection(this.Imperfection.NumShapes[i]));
                        isFound = true;
                        break;
                    }
                    i++;
                }
                if (isFound == true)
                    continue;
                else
                    this.Comb.CombItem.Add(CombItem.Default());
            }
        }
    }
}