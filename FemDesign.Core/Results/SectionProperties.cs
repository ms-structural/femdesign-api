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
    [Result(typeof(SectionProperties), ListProc.SectionProperties)]
    public class SectionProperties :IResult
    {
        // Latest format:
        // Section Composite Height Width A P A/P Ys Zs Iy Wy ez max ez min iy Sy Iz Wz ey max ey min iz Sz It WSVt Iω Iyz zω α1 I1 W1 min W1 max e2 max e2 min i1 S1 S01 c1 ρ1 z2 α2 I2 W2 min W2 max e1 max e1 min i2 S2 S02 c2 ρ2 z1 WelB min WelB max WplB ω min ω max cB Wwt ρ11 ρ22 ρ33 ρ12 ρ13 ρ23 Other
        public string Section { get; }
        public string Composite { get; }
        public double Height { get; }
        public double Width { get; }
        public double A { get; }
        public double P { get; }
        public double AP { get; }
        public double Ys { get; }
        public double Zs { get; }
        public double Iy { get; }
        public double Wy { get; }
        public double ezmax { get; }
        public double ezmin { get; }
        public double iy { get; }
        public double Sy { get; }
        public double Iz { get; }
        public double Wz { get; }
        public double eymax { get; }
        public double eymin { get; }
        public double iz { get; }
        public double Sz { get; }
        public double It { get; }
        public double WSVt { get; }
        public double Iomega { get; }
        public double Iyz { get; }
        public double zomega { get; }
        public double alfa1 { get; }
        public double I1 { get; }
        public double W1min { get; }
        public double W1max { get; }
        public double e2max { get; }
        public double e2min { get; }
        public double i1 { get; }
        public double S1 { get; }
        public double S01 { get; }
        public double c1 { get; }
        public double rho1 { get; }
        public double z2 { get; }
        public double alfa2 { get; }    // NO!
        public double I2 { get; }    // NO!
        public double W2min { get; }    // NO!
        public double W2max { get; }    // NO!
        public double e1max { get; }    // NO!
        public double e1min { get; }    // NO!
        public double i2 { get; }    // NO!
        public double S2 { get; }    // NO!
        public double S02 { get; }    // NO!
        public double c2 { get; }    // NO!
        public double rho2 { get; }    // NO!
        public double z1 { get; }    // NO!
        public double WelBmin { get; }
        public double WelBmax { get; }
        public double WplB { get; }
        public double omegamin { get; }
        public double omegamax { get; }
        public double cB { get; }
        public double Wwt { get; }
        public double rho11 { get; }
        public double rho22 { get; }
        public double rho33 { get; }
        public double rho12 { get; }
        public double rho13 { get; }
        public double rho23 { get; }
        public string Other { get; }    // NO!


        [JsonConstructor]
        internal SectionProperties(string section, string composite, double height, double width, double a, double p, double ap, double ys, double zs, double iY, double wY, double ezmax, double ezmin, double iy, double sy, double iZ, double wz, double eymax, double eymin, double iz, double sz, double it, double wsvt, double iomega, double iyz, double zomega, double alfa1, double i1, double w1min, double w1max, double e2max, double e2min, double i_1, double s1, double s01, double c1, double rho1, double z2, double alfa2, double i2, double w2min, double w2max, double e1max, double e1min, double i_2, double s2, double s02, double c2, double rho2, double z1, double welBmin, double welBmax, double wplB, double omegaMin, double omegaMax, double cB, double wwt, double rho11, double rho22, double rho33, double rho12, double rho13, double rho23, string other)
        {
            this.Section = section;
            this.Composite = composite;
            this.Height = height;
            this.Width = width;
            this.A = a;
            this.P = p;
            this.AP = ap;
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
            this.WSVt = wsvt;
            this.Iomega = iomega;
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
            this.WelBmin = welBmin;
            this.WelBmax = welBmax;
            this.WplB = wplB;
            this.omegamin = omegaMin;
            this.omegamax = omegaMax;
            this.cB = cB;
            this.Wwt = wwt;
            this.rho11 = rho11;
            this.rho22 = rho22;
            this.rho33 = rho33;
            this.rho12 = rho12;
            this.rho13 = rho13;
            this.rho23 = rho23;
            this.Other = other;
        }

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
                return new Regex(@"^(?'type'Sections)$|^Section\tComposite\tHeight\tWidth\tA\tP\tA/P\tYs\tZs\tIy\tWy\tez max\tez min\tiy\tSy\tIz\tWz\tey max\tey min\tiz\tSz\tIt\tWSVt\tI\u03C9\tIyz\tz\u03C9\t\u03B1 1\tI1\tW1 min\tW1 max\te2 max\te2 min\ti1\tS1\tS01\tc1\t\u03C1 1\tz2\t\u03B1 2\tI2\tW2 min\tW2 max\te1 max\te1 min\ti2\tS2\tS02\tc2\t\u03C1 2\tz1\tWelB min\tWelB max\tWplB\t\u03C9 min\t\u03C9 max\tcB\tWwt\t\u03C111\t\u03C122\t\u03C133\t\u03C112\t\u03C113\t\u03C123\tOther|^\[.+\]");
            }
        }


        internal static SectionProperties Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string section = row[0];
            string composite = row[1];
            double height = Double.Parse(row[2], System.Globalization.CultureInfo.InvariantCulture);
            double width = Double.Parse(row[3], System.Globalization.CultureInfo.InvariantCulture);
            double a = Double.Parse(row[4], System.Globalization.CultureInfo.InvariantCulture);
            double p = Double.Parse(row[5], System.Globalization.CultureInfo.InvariantCulture);
            double ap = Double.Parse(row[6], System.Globalization.CultureInfo.InvariantCulture);
            double ys = Double.Parse(row[7], System.Globalization.CultureInfo.InvariantCulture);
            double zs = Double.Parse(row[8], System.Globalization.CultureInfo.InvariantCulture);
            double iY = Double.Parse(row[9], System.Globalization.CultureInfo.InvariantCulture);
            double wY = Double.Parse(row[10], System.Globalization.CultureInfo.InvariantCulture);
            double ezmax = Double.Parse(row[11], System.Globalization.CultureInfo.InvariantCulture);
            double ezmin = Double.Parse(row[12], System.Globalization.CultureInfo.InvariantCulture);
            double iy = Double.Parse(row[13], System.Globalization.CultureInfo.InvariantCulture);
            double sy = Double.Parse(row[14], System.Globalization.CultureInfo.InvariantCulture);
            double iZ = Double.Parse(row[15], System.Globalization.CultureInfo.InvariantCulture);
            double wz = Double.Parse(row[16], System.Globalization.CultureInfo.InvariantCulture);
            double eymax = Double.Parse(row[17], System.Globalization.CultureInfo.InvariantCulture);
            double eymin = Double.Parse(row[18], System.Globalization.CultureInfo.InvariantCulture);
            double iz = Double.Parse(row[19], System.Globalization.CultureInfo.InvariantCulture);
            double sz = Double.Parse(row[20], System.Globalization.CultureInfo.InvariantCulture);
            double it = Double.Parse(row[21], System.Globalization.CultureInfo.InvariantCulture);
            double wsvt = Double.Parse(row[22], System.Globalization.CultureInfo.InvariantCulture);
            double iomega = Double.Parse(row[23], System.Globalization.CultureInfo.InvariantCulture);
            double iyz = Double.Parse(row[24], System.Globalization.CultureInfo.InvariantCulture);
            double zomega = Double.Parse(row[25], System.Globalization.CultureInfo.InvariantCulture);
            double alfa1 = Double.Parse(row[26], System.Globalization.CultureInfo.InvariantCulture);
            double i1 = Double.Parse(row[27], System.Globalization.CultureInfo.InvariantCulture);
            double w1min = Double.Parse(row[28], System.Globalization.CultureInfo.InvariantCulture);
            double w1max = Double.Parse(row[29], System.Globalization.CultureInfo.InvariantCulture);
            double e2max = Double.Parse(row[30], System.Globalization.CultureInfo.InvariantCulture);
            double e2min = Double.Parse(row[31], System.Globalization.CultureInfo.InvariantCulture);
            double i_1 = Double.Parse(row[32], System.Globalization.CultureInfo.InvariantCulture);
            double s1 = Double.Parse(row[33], System.Globalization.CultureInfo.InvariantCulture);
            double s01 = Double.Parse(row[34], System.Globalization.CultureInfo.InvariantCulture);
            double c1 = Double.Parse(row[35], System.Globalization.CultureInfo.InvariantCulture);
            double rho1 = Double.Parse(row[36], System.Globalization.CultureInfo.InvariantCulture);
            double z2 = Double.Parse(row[37], System.Globalization.CultureInfo.InvariantCulture);
            double alfa2 = Double.Parse(row[38], System.Globalization.CultureInfo.InvariantCulture);
            double i2 = Double.Parse(row[39], System.Globalization.CultureInfo.InvariantCulture);
            double w2min = Double.Parse(row[40], System.Globalization.CultureInfo.InvariantCulture);
            double w2max = Double.Parse(row[41], System.Globalization.CultureInfo.InvariantCulture);
            double e1max = Double.Parse(row[42], System.Globalization.CultureInfo.InvariantCulture);
            double e1min = Double.Parse(row[43], System.Globalization.CultureInfo.InvariantCulture);
            double i_2 = Double.Parse(row[44], System.Globalization.CultureInfo.InvariantCulture);
            double s2 = Double.Parse(row[45], System.Globalization.CultureInfo.InvariantCulture);
            double s02 = Double.Parse(row[46], System.Globalization.CultureInfo.InvariantCulture);
            double c2 = Double.Parse(row[47], System.Globalization.CultureInfo.InvariantCulture);
            double rho2 = Double.Parse(row[48], System.Globalization.CultureInfo.InvariantCulture);
            double z1 = Double.Parse(row[49], System.Globalization.CultureInfo.InvariantCulture);
            double welBmin = Double.Parse(row[50], System.Globalization.CultureInfo.InvariantCulture);
            double welBmax = Double.Parse(row[51], System.Globalization.CultureInfo.InvariantCulture);
            double wplB = Double.Parse(row[52], System.Globalization.CultureInfo.InvariantCulture);
            double omegamin = Double.Parse(row[53], System.Globalization.CultureInfo.InvariantCulture);
            double omegamax = Double.Parse(row[54], System.Globalization.CultureInfo.InvariantCulture);
            double cB = Double.Parse(row[55], System.Globalization.CultureInfo.InvariantCulture);
            double wwt = Double.Parse(row[56], System.Globalization.CultureInfo.InvariantCulture);
            double rho11 = Double.Parse(row[57], System.Globalization.CultureInfo.InvariantCulture);
            double rho22 = Double.Parse(row[58], System.Globalization.CultureInfo.InvariantCulture);
            double rho33 = Double.Parse(row[59], System.Globalization.CultureInfo.InvariantCulture);
            double rho12 = Double.Parse(row[60], System.Globalization.CultureInfo.InvariantCulture);
            double rho13 = Double.Parse(row[61], System.Globalization.CultureInfo.InvariantCulture);
            double rho23 = Double.Parse(row[62], System.Globalization.CultureInfo.InvariantCulture);
            string other = row[63];

            return new SectionProperties(section, composite, height, width, a, p, ap, ys, zs, iY, wY, ezmax, ezmin, iy, sy, iZ, wz, eymax, eymin, iz, sz, it, wsvt, iomega, iyz, zomega, alfa1, i1, w1min, w1max, e2max, e2min, i_1, s1, s01, c1, rho1, z2, alfa2, i2, w2min, w2max, e1max, e1min, i_2, s2, s02, c2, rho2, z1, welBmin, welBmax, wplB, omegamin, omegamax, cB, wwt, rho11, rho22, rho33, rho12, rho13, rho23, other);
        }
    }
}
