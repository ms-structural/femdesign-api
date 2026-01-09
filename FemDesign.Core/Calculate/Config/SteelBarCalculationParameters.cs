using FemDesign.Bars;
using FemDesign.GenericClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static FemDesign.Calculate.SteelBarCalculationParameters;

namespace FemDesign.Calculate
{
    /// <summary>
    /// Calculation parameters for steel bars
    /// </summary>
    [System.Serializable]
    public class SteelBarCalculationParameters : CONFIG
    {
        /// <summary>
        /// Defines the Buckling Curve enumeration.
        /// </summary>
        public enum BucklingCurve
        {
            [XmlEnum("-1")]
            [Parseable("Auto", "auto", "AUTO")]
            Auto = -1,
            [XmlEnum("0")]
            [Parseable("a0", "A0")]
            a0,
            [XmlEnum("1")]
            [Parseable("a", "A")]
            a,
            [XmlEnum("2")]
            [Parseable("b", "B")]
            b,
            [XmlEnum("3")]
            [Parseable("c", "C")]
            c,
            [XmlEnum("4")]
            [Parseable("d", "D")]
            d,
        }

        /// <summary>
        /// Defines the Buckling Curve Lt enumeration.
        /// </summary>
        public enum BucklingCurveLt
        {
            [XmlEnum("-1")]
            [Parseable("Auto", "auto", "AUTO")]
            Auto = -1,
            [XmlEnum("0")]
            [Parseable("a", "A")]
            a,
            [XmlEnum("1")]
            [Parseable("b", "B")]
            b,
            [XmlEnum("2")]
            [Parseable("c", "C")]
            c,
            [XmlEnum("3")]
            [Parseable("d", "D")]
            d,
        }

        /// <summary>
        /// Defines the Second Order enumeration.
        /// </summary>
        public enum SecondOrder
        {
            [XmlEnum("0")]
            [Parseable("Ignore", "ignore", "IGNORE")]
            Ignore,
            [XmlEnum("1")]
            [Parseable("ConsiderIfAvailable", "consider_if_available", "Consider_if_available", "CONSIDER_IF_AVAILABLE")]
            ConsiderIfAvailable,
            [XmlEnum("2")]
            [Parseable("ConsiderAndFirstOrderDesign", "consider_and_first_order_design", "Consider_and_first_order_design", "CONSIDER_AND_FIRST_ORDER_DESIGN")]
            ConsiderAndFirstOrderDesign
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute("type")]
        public string Type = "ECCALCPARAMBARST";

        /// <summary>
        /// Flexural buckling stiff direction
        /// </summary>
        [XmlAttribute("aBucklingCurve_fx1")]
        public BucklingCurve BucklingCurveFx1 { get; set; } = BucklingCurve.Auto;

        /// <summary>
        /// Flexural buckling weak direction
        /// </summary>
        [XmlAttribute("aBucklingCurve_fx2")]
        public BucklingCurve BucklingCurveFx2 { get; set; } = BucklingCurve.Auto;

        /// <summary>
        /// Torsional-flexural buckling
        /// </summary>
        [XmlAttribute("aBucklingCurve_tf")]
        public BucklingCurve BucklingCurveTf { get; set; } = BucklingCurve.Auto;

        /// <summary>
        /// Lateral-torsional buckling bottom flange
        /// </summary>
        [XmlAttribute("aBucklingCurve_ltb")]
        public BucklingCurveLt BucklingCurveLtb { get; set; } = BucklingCurveLt.Auto;

        /// <summary>
        /// Lateral-torsional buckling top flange
        /// </summary>
        [XmlAttribute("aBucklingCurve_ltt")]
        public BucklingCurveLt BucklingCurveLtt { get; set; } = BucklingCurveLt.Auto;

        /// <summary>
        /// Gets or sets the check resistance only.
        /// </summary>
        [XmlAttribute("CheckResistanceOnly")]
        public int _checkResistanceOnly = 0;

        [XmlIgnore]
        public bool CheckResistanceOnly
        {
            get
            {
                return System.Convert.ToBoolean(this._checkResistanceOnly);
            }
            set
            {
                this._checkResistanceOnly = System.Convert.ToInt32(value);
            }
        }


        /// <summary>
        /// Gets or sets the class4 ignored.
        /// </summary>
        [XmlAttribute("class4Ignored")]
        public int _class4Ignored = 0;

        [XmlIgnore]
        public bool Class4Ignored
        {
            get
            {
                return System.Convert.ToBoolean(this._class4Ignored);
            }
            set
            {
                this._class4Ignored = System.Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Gets or sets the convergency ratio.
        /// </summary>
        [XmlAttribute("convergencyratio")]
        public double ConvergencyRatio { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the f lat tor buck gen.
        /// </summary>
        [XmlAttribute("fLatTorBuckGen")]
        public int _fLatTorBuckGen = 1;

        [XmlIgnore]
        public bool LatTorBuckGen
        {
            get
            {
                return System.Convert.ToBoolean(this._fLatTorBuckGen);
            }
            set
            {
                this._fLatTorBuckGen = System.Convert.ToInt32(value);
            }
        }


        /// <summary>
        /// Gets or sets the f lat tor buck gen spec for i.
        /// </summary>
        [XmlAttribute("fLatTorBuckGenSpecForI")]
        public int _fLatTorBuckGenSpecForI = 0;

        [XmlIgnore]
        public bool LatTorBuckGenSpecForI
        {
            get
            {
                return System.Convert.ToBoolean(this._fLatTorBuckGenSpecForI);
            }
            set
            {
                this._fLatTorBuckGenSpecForI = System.Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Gets or sets the max iter step.
        /// </summary>
        [XmlAttribute("maxIterStep")]
        public int MaxIterStep { get; set; } = 50;

        /// <summary>
        /// Gets or sets the plastic ignored.
        /// </summary>
        [XmlAttribute("plasticIgnored")]
        public int _plasticIgnored = 0;

        [XmlIgnore]
        public bool PlasticIgnored
        {
            get
            {
                return System.Convert.ToBoolean(this._plasticIgnored);
            }
            set
            {
                this._plasticIgnored = System.Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Max. distance between calculated sections [m]
        /// </summary>
        [XmlAttribute("rStep")]
        public double DistanceCalculatedSection { get; set; } = 0.50;

        /// <summary>
        /// Gets or sets the s2nd order.
        /// </summary>
        [XmlAttribute("s2ndOrder")]
        public SecondOrder S2ndOrder { get; set; } = SecondOrder.ConsiderIfAvailable;

        /// <summary>
        /// Gets or sets the use equation6 41.
        /// </summary>
        [XmlAttribute("UseEqation6_41")]
        public int _useEquation6_41 = 1;

        [XmlIgnore]
        public bool UseEquation6_41
        {
            get
            {
                return System.Convert.ToBoolean(this._useEquation6_41);
            }
            set
            {
                this._useEquation6_41 = System.Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// List of BarPart GUIDs to apply the parameters to
        /// </summary>
        [XmlElement("GUID")]
        public List<Guid> Guids { get; set; }

        /// <summary>
        /// Sets the parameters on bars.
        /// </summary>
        /// <param name="bars">the bars.</param>
        public void SetParametersOnBars(List<Bar> bars)
        {
            this.Guids = bars.Select(x => x.BarPart.Guid).ToList();
        }

        /// <summary>
        /// Sets the parameters on bars.
        /// </summary>
        /// <param name="bar">the bar.</param>
        public void SetParametersOnBars(Bar bar)
        {
            if (bar.IsSteel() == false)
            {
                throw new System.ArgumentException("Bar must be steel.");
            }
            this.SetParametersOnBars(new List<Bar> { bar });
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SteelBarCalculationParameters"/> class.
        /// </summary>
        public SteelBarCalculationParameters()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SteelBarCalculationParameters"/> class.
        /// </summary>
        /// <param name="sectionsDistance">the sections distance.</param>
        /// <param name="secondOrder">the second order.</param>
        /// <param name="plasticCalculation">the plastic calculation.</param>
        /// <param name="equation641">the equation641.</param>
        /// <param name="class4">the class4.</param>
        /// <param name="ignore">the ignore.</param>
        /// <param name="convergence">the convergence.</param>
        /// <param name="iteration">the iteration.</param>
        /// <param name="stiffDirection">the stiff direction.</param>
        /// <param name="weakDirection">the weak direction.</param>
        /// <param name="torsionalDirection">the torsional direction.</param>
        /// <param name="en1993_1_1_6_3_2_2">the en1993 1 1 6 3 2 2.</param>
        /// <param name="en1993_1_1_6_3_2_3">the en1993 1 1 6 3 2 3.</param>
        /// <param name="topFlange">the top flange.</param>
        /// <param name="bottomFlange">the bottom flange.</param>
        public SteelBarCalculationParameters(double sectionsDistance, SecondOrder secondOrder, bool plasticCalculation, bool equation641, bool class4, bool ignore, double convergence, int iteration, BucklingCurve stiffDirection, BucklingCurve weakDirection, BucklingCurve torsionalDirection, bool en1993_1_1_6_3_2_2, bool en1993_1_1_6_3_2_3, BucklingCurveLt topFlange, BucklingCurveLt bottomFlange)
        {
            this.DistanceCalculatedSection = sectionsDistance;
            this.S2ndOrder = secondOrder;
            this.PlasticIgnored = plasticCalculation;
            this.UseEquation6_41 = equation641;
            this.Class4Ignored = class4;
            this.CheckResistanceOnly = ignore;
            this.ConvergencyRatio = convergence;
            this.MaxIterStep = iteration;
            this.BucklingCurveFx1 = stiffDirection;
            this.BucklingCurveFx2 = weakDirection;
            this.BucklingCurveTf = torsionalDirection;
            this.LatTorBuckGen = en1993_1_1_6_3_2_2;
            this.LatTorBuckGenSpecForI = en1993_1_1_6_3_2_3;
            this.BucklingCurveLtb = bottomFlange;
            this.BucklingCurveLtt = topFlange;
        }
    }
}