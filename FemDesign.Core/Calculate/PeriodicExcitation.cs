// https://strusoft.com/
using FemDesign.GenericClasses;
using System;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// </summary>
    public partial class PeriodicExcitation
    {
        /// <summary>
        /// Gets or sets the delta t.
        /// </summary>
        [XmlAttribute("deltat")]
        public double _deltaT = 0.01;
        [XmlIgnore]
        public double DeltaT
        {
            get
            {
                return this._deltaT;
            }
            set
            {
                this._deltaT = value;
            }
        }

        /// <summary>
        /// Gets or sets the t end.
        /// </summary>
        [XmlAttribute("tend")]
        public double _tEnd = 5.0;
        [XmlIgnore]
        public double TEnd
        {
            get
            {
                return this._tEnd;
            }
            set
            {
                this._tEnd = value;
            }
        }

        /// <summary>
        /// Gets or sets the dmp type.
        /// </summary>
        [XmlAttribute("dampeningtype")]
        public int _dmpType = 0;
        [XmlIgnore]
        public DampingType DmpType
        {
            get
            {
                return (DampingType)_dmpType;
            }
            set
            {
                this._dmpType = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the alpha.
        /// </summary>
        [XmlAttribute("alpha")]
        public double _alpha = 0.0;
        [XmlIgnore]
        public double Alpha
        {
            get
            {
                return this._alpha;
            }
            set
            {
                this._alpha = value;
            }
        }

        /// <summary>
        /// Gets or sets the beta.
        /// </summary>
        [XmlAttribute("beta")]
        public double _beta = 0.0;
        [XmlIgnore]
        public double Beta
        {
            get
            {
                return this._beta;
            }
            set
            {
                this._beta = value;
            }
        }

        /// <summary>
        /// Gets or sets the dmp factor.
        /// </summary>
        [XmlAttribute("ksi")]
        public double _dmpFactor = 5.0;
        [XmlIgnore]
        public double DmpFactor
        {
            get
            {
                return this._dmpFactor;
            }
            set
            {
                this._dmpFactor = value;
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public PeriodicExcitation()
        {

        }

        /// <summary>
        /// Define calculation parameters for periodic excitation.
        /// </summary>
        /// <param name="deltaT">Calculation parameter [s].</param>
        /// <param name="timeEnd">Calculation parameter [s].</param>
        /// <param name="dmpType">Damping type.</param>
        /// <param name="alpha">'alpha' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="beta">'beta' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="dampingFactor">'ksi' damping factor.</param>
        public PeriodicExcitation(double deltaT = 0.01, double timeEnd = 5.0, DampingType dmpType = DampingType.Rayleigh, double alpha = 0, double beta = 0, double dampingFactor = 5.0)
        {
            this.DeltaT = deltaT;
            this.TEnd = timeEnd;
            this.DmpType = dmpType;
            this.Alpha = alpha;
            this.Beta = beta;
            this.DmpFactor = dampingFactor;
        }

        /// <summary>
        /// Define default calculation parameters for periodic excitation.
        /// </summary>
        /// <returns></returns>
        public static PeriodicExcitation Default()
        { 
            return new PeriodicExcitation(0.01, 5.0, DampingType.Rayleigh, 0, 0, 5.0); 
        }
    }

    /// <summary>
    /// Defines the Damping Type enumeration.
    /// </summary>
    public enum DampingType
    {
        [Parseable("Rayleigh")]
        Rayleigh = 0,
        [Parseable("KelvinVoigt")]
        KelvinVoigt = 1
    }
}
