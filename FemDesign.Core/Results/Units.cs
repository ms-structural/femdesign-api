using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

#if ISDYNAMO
using Autodesk.DesignScript.Runtime;
#endif

namespace FemDesign.Results
{
    /// <summary>
    /// Defines the Length enumeration.
    /// </summary>
    public enum Length
    {
        mm,
        cm,
        dm,
        m,
        inch,
        feet,
        yd
    }
    /// <summary>
    /// Defines the Angle enumeration.
    /// </summary>
    public enum Angle
    {
        rad,
        deg
    }
    /// <summary>
    /// Defines the Sectional Data enumeration.
    /// </summary>
    public enum SectionalData
    {
        mm,
        cm,
        dm,
        m,
        inch,
        feet,
        yd
    }
    /// <summary>
    /// Defines the Force enumeration.
    /// </summary>
    public enum Force
    {
        N,
        daN,
        kN,
        MN,
        lbf,
        kips
    }
    /// <summary>
    /// Defines the Mass enumeration.
    /// </summary>
    public enum Mass
    {
        t,
        kg,
        lb,
        tonUK,
        tonUS
    }
    /// <summary>
    /// Defines the Displacement enumeration.
    /// </summary>
    public enum Displacement
    {
        mm,
        cm,
        dm,
        m,
        inch,
        feet,
        yd
    }
    /// <summary>
    /// Defines the Stress enumeration.
    /// </summary>
    public enum Stress
    {
        Pa,
        kPa,
        MPa,
        GPa
    }

    /// <summary>
    /// Represents units.
    /// </summary>
    [System.Serializable]
    #if ISDYNAMO
    [IsVisibleInDynamoLibrary(false)]
    #endif
    public partial class Units
    {
        /// <summary>
        /// Gets or sets the num.
        /// </summary>
        [XmlElement("num")]
        public int Num { get; set; }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        [XmlElement("unit")]
        public int Unit { get; set; }

        /// <summary>
        /// Gets or sets the unit results.
        /// </summary>
        [XmlIgnore]
        public UnitResults UnitResults { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Units()
        {

        }

        private Units(int num, int unit)
        {
            this.Num = num;
            this.Unit = unit;
        }

        /// <summary>
        /// Gets the units.
        /// </summary>
        /// <param name="unitResult">the unit result.</param>
        /// <returns>The result.</returns>
        public static List<Units> GetUnits(UnitResults unitResult = null)
        {
            // Define the Units for some output
            // the schema has been discussed in the following issue
            // https://github.com/strusoft/femdesign-api/issues/375

            if(unitResult == null)
            {
                unitResult = UnitResults.Default();
            }

            var unitsObj = new List<Units>()
            {
                new Units(0, 0),
                new Units(1, (int) unitResult.Angle),
                new Units(2, (int) unitResult.Length),
                new Units(3, (int) unitResult.Force),
                new Units(4, (int) unitResult.Mass),
                new Units(5, (int) unitResult.SectionalData),
                new Units(6, (int) unitResult.Displacement),
                new Units(7, (int) unitResult.Stress),
            };

            // the object between 8 and 63 are not implemented yet

            for (int i = 8; i <= 63; i++)
            {
                unitsObj.Add(new Units(i, 0));
            }

            return unitsObj;
        }

        
    }

    /// <summary>
    /// Represents a Unit Results.
    /// </summary>
    public partial class UnitResults
    {
        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public Length Length { get; set; } = Length.m;
        /// <summary>
        /// Gets or sets the angle.
        /// </summary>
        public Angle Angle { get; set; } = Angle.deg;
        /// <summary>
        /// Gets or sets the sectional data.
        /// </summary>
        public SectionalData SectionalData { get; set; } = SectionalData.mm;
        /// <summary>
        /// Gets or sets the force.
        /// </summary>
        public Force Force { get; set; } = Force.kN;
        /// <summary>
        /// Gets or sets the mass.
        /// </summary>
        public Mass Mass { get; set; } = Mass.kg;
        /// <summary>
        /// Gets or sets the displacement.
        /// </summary>
        public Displacement Displacement { get; set; } = Displacement.mm;
        /// <summary>
        /// Gets or sets the stress.
        /// </summary>
        public Stress Stress { get; set; } = Stress.MPa;

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public UnitResults()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitResults"/> class.
        /// </summary>
        /// <param name="length">the length.</param>
        /// <param name="angle">the angle.</param>
        /// <param name="sectionalData">the sectional data.</param>
        /// <param name="force">the force.</param>
        /// <param name="mass">the mass.</param>
        /// <param name="displacement">the displacement.</param>
        /// <param name="stress">the stress.</param>
        public UnitResults(Length length = Length.m, Angle angle = Angle.deg, SectionalData sectionalData = SectionalData.mm, Force force = Force.kN, Mass mass = Mass.kg, Displacement displacement = Displacement.mm, Stress stress = Stress.MPa)
        {
            this.Length = length;
            this.Angle = angle;
            this.SectionalData = sectionalData;
            this.Force = force;
            this.Mass = mass;
            this.Displacement = displacement;
            this.Stress = stress;
        }

        /// <summary>
        /// Returns the Default UnitResults
        /// </summary>
        /// <returns>The result.</returns>
        public static UnitResults Default()
        {
            return new UnitResults(Results.Length.m, Results.Angle.deg, Results.SectionalData.m, Results.Force.kN, Results.Mass.kg, Results.Displacement.m, Results.Stress.Pa);
        }
    }
}
