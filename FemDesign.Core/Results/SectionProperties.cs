using FemDesign.Calculate;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FemDesign.Results
{
    /// <summary>
    /// Represents a Section Properties.
    /// </summary>
    [Result(typeof(SectionProperties), ListProc.SectionProperties)]
    public class SectionProperties :IResult
    {
        // use the following text to create properties Section	Composite	Height	Width	A	P	A/P	Yg	Zg	Ys	Zs	Iy	Wy	ez max	ez min	iy	Sy	Iz	Wz	ey max	ey min	iz	Sz	It	Wt	Iw	Iyz	zω	alfa1	I1	W1 min	W1 max	e2 max	e2 min	i1	S1	S01	c1	rho1	z2	alfa2	I2	W2 min	W2 max	e1 max	e1 min	i2	S2	S02	c2	rho2	z1	Other
        /// <summary>
        /// Gets or sets the section.
        /// </summary>
        public string Section { get; }
        /// <summary>
        /// Gets or sets the composite.
        /// </summary>
        public string Composite { get; }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public double Height { get; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double Width { get; }
        /// <summary>
        /// Gets or sets the a.
        /// </summary>
        public double A { get; }
        /// <summary>
        /// Gets or sets the p.
        /// </summary>
        public double P { get; }
        /// <summary>
        /// Gets or sets the ap.
        /// </summary>
        public double AP { get; }
        /// <summary>
        /// Gets or sets the yg.
        /// </summary>
        public double Yg { get; }
        /// <summary>
        /// Gets or sets the zg.
        /// </summary>
        public double Zg { get; }
        /// <summary>
        /// Gets or sets the ys.
        /// </summary>
        public double Ys { get; }
        /// <summary>
        /// Gets or sets the zs.
        /// </summary>
        public double Zs { get; }
        /// <summary>
        /// Gets or sets the iy.
        /// </summary>
        public double Iy { get; }
        /// <summary>
        /// Gets or sets the wy.
        /// </summary>
        public double Wy { get; }
        /// <summary>
        /// Gets or sets the ezmax.
        /// </summary>
        public double ezmax { get; }
        /// <summary>
        /// Gets or sets the ezmin.
        /// </summary>
        public double ezmin { get; }
        /// <summary>
        /// Gets or sets the iy.
        /// </summary>
        public double iy { get; }
        /// <summary>
        /// Gets or sets the sy.
        /// </summary>
        public double Sy { get; }
        /// <summary>
        /// Gets or sets the iz.
        /// </summary>
        public double Iz { get; }
        /// <summary>
        /// Gets or sets the wz.
        /// </summary>
        public double Wz { get; }
        /// <summary>
        /// Gets or sets the eymax.
        /// </summary>
        public double eymax { get; }
        /// <summary>
        /// Gets or sets the eymin.
        /// </summary>
        public double eymin { get; }
        /// <summary>
        /// Gets or sets the iz.
        /// </summary>
        public double iz { get; }
        /// <summary>
        /// Gets or sets the sz.
        /// </summary>
        public double Sz { get; }
        /// <summary>
        /// Gets or sets the it.
        /// </summary>
        public double It { get; }
        /// <summary>
        /// Gets or sets the wt.
        /// </summary>
        public double Wt { get; }
        /// <summary>
        /// Gets or sets the iw.
        /// </summary>
        public double Iw { get; }
        /// <summary>
        /// Gets or sets the iyz.
        /// </summary>
        public double Iyz { get; }
        /// <summary>
        /// Gets or sets the zomega.
        /// </summary>
        public double zomega { get; }
        /// <summary>
        /// Gets or sets the alfa1.
        /// </summary>
        public double alfa1 { get; }
        /// <summary>
        /// Gets or sets the i1.
        /// </summary>
        public double I1 { get; }
        /// <summary>
        /// Gets or sets the w1min.
        /// </summary>
        public double W1min { get; }
        /// <summary>
        /// Gets or sets the w1max.
        /// </summary>
        public double W1max { get; }
        /// <summary>
        /// Gets or sets the e2max.
        /// </summary>
        public double e2max { get; }
        /// <summary>
        /// Gets or sets the e2min.
        /// </summary>
        public double e2min { get; }
        /// <summary>
        /// Gets or sets the i1.
        /// </summary>
        public double i1 { get; }
        /// <summary>
        /// Gets or sets the s1.
        /// </summary>
        public double S1 { get; }
        /// <summary>
        /// Gets or sets the s01.
        /// </summary>
        public double S01 { get; }
        /// <summary>
        /// Gets or sets the c1.
        /// </summary>
        public double c1 { get; }
        /// <summary>
        /// Gets or sets the rho1.
        /// </summary>
        public double rho1 { get; }
        /// <summary>
        /// Gets or sets the z2.
        /// </summary>
        public double z2 { get; }
        /// <summary>
        /// Gets or sets the alfa2.
        /// </summary>
        public double alfa2 { get; }    // NO!
        /// <summary>
        /// Gets or sets the i2.
        /// </summary>
        public double I2 { get; }    // NO!
        /// <summary>
        /// Gets or sets the w2min.
        /// </summary>
        public double W2min { get; }    // NO!
        /// <summary>
        /// Gets or sets the w2max.
        /// </summary>
        public double W2max { get; }    // NO!
        /// <summary>
        /// Gets or sets the e1max.
        /// </summary>
        public double e1max { get; }    // NO!
        /// <summary>
        /// Gets or sets the e1min.
        /// </summary>
        public double e1min { get; }    // NO!
        /// <summary>
        /// Gets or sets the i2.
        /// </summary>
        public double i2 { get; }    // NO!
        /// <summary>
        /// Gets or sets the s2.
        /// </summary>
        public double S2 { get; }    // NO!
        /// <summary>
        /// Gets or sets the s02.
        /// </summary>
        public double S02 { get; }    // NO!
        /// <summary>
        /// Gets or sets the c2.
        /// </summary>
        public double c2 { get; }    // NO!
        /// <summary>
        /// Gets or sets the rho2.
        /// </summary>
        public double rho2 { get; }    // NO!
        /// <summary>
        /// Gets or sets the z1.
        /// </summary>
        public double z1 { get; }    // NO!
        /// <summary>
        /// Gets or sets the other.
        /// </summary>
        public string Other { get; }    // NO!


        [JsonConstructor]
        internal SectionProperties(string section, string composite, double height, double width, double a, double p, double ap, double yg, double zg, double ys, double zs, double iY, double wY, double ezmax, double ezmin, double iy, double sy, double iZ, double wz, double eymax, double eymin, double iz, double sz, double it, double wt, double iw, double iyz, double zomega, double alfa1, double i1, double w1min, double w1max, double e2max, double e2min, double i_1, double s1, double s01, double c1, double rho1, double z2, double alfa2, double i2, double w2min, double w2max, double e1max, double e1min, double i_2, double s2, double s02, double c2, double rho2, double z1, string other)
        {
            this.Section = section;
            this.Composite = composite;
            this.Height = height;
            this.Width = width;
            this.A = a;
            this.P = p;
            this.AP = ap;
            this.Yg = yg;
            this.Zg = zg;
            this.Ys = ys;
            this.Zs = zs;
            this.Iy = iY;
            this.Wy = wY;
            this.ezmax = ezmax;
            this.ezmin = ezmin;
            this.iy = iy;
            this.Sy = sy;
            this.Iz = iZ;
            this.Wz = wz;
            this.eymax = eymax;
            this.eymin = eymin;
            this.iz = iz;
            this.Sz = sz;
            this.It = it;
            this.Wt = wt;
            this.Iw = iw;
            this.Iyz = iyz;
            this.zomega = zomega;
            this.alfa1 = alfa1;
            this.I1 = i1;
            this.W1min = w1min;
            this.W1max = w1max;
            this.e2max = e2max;
            this.e2min = e2min;
            this.i1 = i_1;
            this.S1 = s1;
            this.S01 = s01;
            this.c1 = c1;
            this.rho1 = rho1;
            this.z2 = z2;
            this.alfa2 = alfa2;
            this.I2 = i2;
            this.W2min = w2min;
            this.W2max = w2max;
            this.e1max = e1max;
            this.e1min = e1min;
            this.i2 = i_2;
            this.S2 = s2;
            this.S02 = s02;
            this.c2 = c2;
            this.rho2 = rho2;
            this.z1 = z1;
            this.Other = other;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Sections)$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Sections)$|^Section\tComposite\tHeight\tWidth\tA\tP\tA/P\tYg\tZg\tYs\tZs\tIy\tWy\tez max\tez min\tiy\tSy\tIz\tWz\tey max\tey min\tiz\tSz\tIt\tWt\tIw\tIyz\tz\u03C9\t\u03B1 1\tI1\tW1 min\tW1 max\te2 max\te2 min\ti1\tS1\tS01\tc1\t\u03C1 1\tz2\t\u03B1 2\tI2\tW2 min\tW2 max\te1 max\te1 min\ti2\tS2\tS02\tc2\t\u03C1 2\tz1\tOther|^\[.+\]");
            }
        }


        internal static SectionProperties Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            // write each property in a variable following the row order
            string section = row[0];
            string composite = row[1];
            double height = Double.Parse(row[2], System.Globalization.CultureInfo.InvariantCulture);
            double width = Double.Parse(row[3], System.Globalization.CultureInfo.InvariantCulture);
            double a = Double.Parse(row[4], System.Globalization.CultureInfo.InvariantCulture);
            double p = Double.Parse(row[5], System.Globalization.CultureInfo.InvariantCulture);
            double ap = Double.Parse(row[6], System.Globalization.CultureInfo.InvariantCulture);
            double yg = Double.Parse(row[7], System.Globalization.CultureInfo.InvariantCulture);
            double zg = Double.Parse(row[8], System.Globalization.CultureInfo.InvariantCulture);
            double ys = Double.Parse(row[9], System.Globalization.CultureInfo.InvariantCulture);
            double zs = Double.Parse(row[10], System.Globalization.CultureInfo.InvariantCulture);
            double iY = Double.Parse(row[11], System.Globalization.CultureInfo.InvariantCulture);
            double wY = Double.Parse(row[12], System.Globalization.CultureInfo.InvariantCulture);
            double ezmax = Double.Parse(row[13], System.Globalization.CultureInfo.InvariantCulture);
            double ezmin = Double.Parse(row[14], System.Globalization.CultureInfo.InvariantCulture);
            double iy = Double.Parse(row[15], System.Globalization.CultureInfo.InvariantCulture);
            double sy = Double.Parse(row[16], System.Globalization.CultureInfo.InvariantCulture);
            double iZ = Double.Parse(row[17], System.Globalization.CultureInfo.InvariantCulture);
            double wz = Double.Parse(row[18], System.Globalization.CultureInfo.InvariantCulture);
            double eymax = Double.Parse(row[19], System.Globalization.CultureInfo.InvariantCulture);
            double eymin = Double.Parse(row[20], System.Globalization.CultureInfo.InvariantCulture);
            double iz = Double.Parse(row[21], System.Globalization.CultureInfo.InvariantCulture);
            double sz = Double.Parse(row[22], System.Globalization.CultureInfo.InvariantCulture);
            double it = Double.Parse(row[23], System.Globalization.CultureInfo.InvariantCulture);
            double wt = Double.Parse(row[24], System.Globalization.CultureInfo.InvariantCulture);
            double iw = Double.Parse(row[25], System.Globalization.CultureInfo.InvariantCulture);
            double iyz = Double.Parse(row[26], System.Globalization.CultureInfo.InvariantCulture);
            double zomega = Double.Parse(row[27], System.Globalization.CultureInfo.InvariantCulture);
            double alfa1 = Double.Parse(row[28], System.Globalization.CultureInfo.InvariantCulture);
            double i1 = Double.Parse(row[29], System.Globalization.CultureInfo.InvariantCulture);
            double w1min = Double.Parse(row[30], System.Globalization.CultureInfo.InvariantCulture);
            double w1max = Double.Parse(row[31], System.Globalization.CultureInfo.InvariantCulture);
            double e2max = Double.Parse(row[32], System.Globalization.CultureInfo.InvariantCulture);
            double e2min = Double.Parse(row[33], System.Globalization.CultureInfo.InvariantCulture);
            double i_1 = Double.Parse(row[34], System.Globalization.CultureInfo.InvariantCulture);
            double s1 = Double.Parse(row[35], System.Globalization.CultureInfo.InvariantCulture);
            double s01 = Double.Parse(row[36], System.Globalization.CultureInfo.InvariantCulture);
            double c1 = Double.Parse(row[37], System.Globalization.CultureInfo.InvariantCulture);
            double rho1 = Double.Parse(row[38], System.Globalization.CultureInfo.InvariantCulture);
            double z2 = Double.Parse(row[39], System.Globalization.CultureInfo.InvariantCulture);
            double alfa2 = Double.Parse(row[40], System.Globalization.CultureInfo.InvariantCulture);
            double i2 = Double.Parse(row[41], System.Globalization.CultureInfo.InvariantCulture);
            double w2min = Double.Parse(row[42], System.Globalization.CultureInfo.InvariantCulture);
            double w2max = Double.Parse(row[43], System.Globalization.CultureInfo.InvariantCulture);
            double e1max = Double.Parse(row[44], System.Globalization.CultureInfo.InvariantCulture);
            double e1min = Double.Parse(row[45], System.Globalization.CultureInfo.InvariantCulture);
            double i_2 = Double.Parse(row[46], System.Globalization.CultureInfo.InvariantCulture);
            double s2 = Double.Parse(row[47], System.Globalization.CultureInfo.InvariantCulture);
            double s02 = Double.Parse(row[48], System.Globalization.CultureInfo.InvariantCulture);
            double c2 = Double.Parse(row[49], System.Globalization.CultureInfo.InvariantCulture);
            double rho2 = Double.Parse(row[50], System.Globalization.CultureInfo.InvariantCulture);
            double z1 = Double.Parse(row[51], System.Globalization.CultureInfo.InvariantCulture);
            string other = row[52];

            return new SectionProperties(section, composite, height, width, a, p, ap, yg, zg, ys, zs, iY, wY, ezmax, ezmin, iy, sy, iZ, wz, eymax, eymin, iz, sz, it, wt, iw, iyz, zomega, alfa1, i1, w1min, w1max, e2max, e2min, i_1, s1, s01, c1, rho1, z2, alfa2, i2, w2min, w2max, e1max, e1min, i_2, s2, s02, c2, rho2, z1, other);
        }
    }
}
