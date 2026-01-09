using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;
using FemDesign.Calculate;
using Newtonsoft.Json;
namespace FemDesign.Results
{
    /// <summary>
    /// Represents a Labelled Section Internal Force.
    /// </summary>
    [Result(typeof(LabelledSectionInternalForce), ListProc.LabelledSectionsInternalForcesLoadCase, ListProc.LabelledSectionsInternalForcesLoadCombination)]
    public partial class LabelledSectionInternalForce : IResult
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Position along the beam
        /// </summary>
        public double Pos { get; }
        /// <summary>
        /// Local Mx'
        /// </summary>
        public double Mx { get; }
        /// <summary>
        /// Local My'
        /// </summary>
        public double My { get; }
        /// <summary>
        /// Local Mx'y'
        /// </summary>
        public double Mxy { get; }
        /// <summary>
        /// Local Nx'
        /// </summary>
        public double Nx { get; }
        /// <summary>
        /// Local Ny'
        /// </summary>
        public double Ny { get; }
        /// <summary>
        /// Local Nx'y'
        /// </summary>
        public double Nxy { get; }
        /// <summary>
        /// Local Tx'z'
        /// </summary>
        public double Txz { get; }
        /// <summary>
        /// Local Ty'z'
        /// </summary>
        public double Tyz { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal LabelledSectionInternalForce(string id, double pos, double mx, double my, double mxy, double nx, double ny, double nxy, double txz, double tyz, string resultCase)
        {
            this.Id = id;
            this.Pos = pos;
            this.Mx = mx;
            this.My = my;
            this.Mxy = mxy;
            this.Nx = nx;
            this.Ny = ny;
            this.Nxy = nxy;
            this.Txz = txz;
            this.Tyz = tyz;
            this.CaseIdentifier = resultCase;
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
                return new Regex(@"^(?'type'Labelled sections), Internal forces , ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Labelled sections), Internal forces , ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^ID\tx/Type\tMx'\tMy'\tMx'y'\tNx'\tNy'\tNx'y'\tTx'z'\tTy'z'\t(Case|Comb\.)|^\[.*\]");
            }
        }

        internal static LabelledSectionInternalForce Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string id = row[0];
            double pos = Double.Parse(row[1], CultureInfo.InvariantCulture);
            double mx = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double my = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double mxy = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double nx = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double ny = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double nxy = Double.Parse(row[7], CultureInfo.InvariantCulture);
            double txz = Double.Parse(row[8], CultureInfo.InvariantCulture);
            double tyz = Double.Parse(row[9], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new LabelledSectionInternalForce(id, pos, mx, my, mxy, nx, ny, nxy, txz, tyz, lc);
        }
    }
}
