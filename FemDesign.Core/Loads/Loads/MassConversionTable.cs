using System.Xml.Serialization;
using System.Collections.Generic;


namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Mass Conversion.
    /// </summary>
    [System.Serializable]
    public partial class MassConversion
    {
        /// <summary>
        /// Gets or sets the factor.
        /// </summary>
        [XmlAttribute("factor")]
        public double _factor;
        [XmlIgnore]
        public double Factor
        {
            get
            {
                return this._factor;
            }
            set
            {
                this._factor = RestrictedDouble.NonNegMax_10(value);
            }
        }
        /// <summary>
        /// Gets or sets the load case guid.
        /// </summary>
        [XmlAttribute("load_case")]
        public System.Guid LoadCaseGuid { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MassConversion()
        {

        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="loadCaseGuid"></param>
        internal MassConversion(double factor, System.Guid loadCaseGuid)
        {
            this.Factor = factor;
            this.LoadCaseGuid = loadCaseGuid;
        }
    }

    /// <summary>
    /// Represents a Mass Conversion Table.
    /// </summary>
    [System.Serializable]
    public partial class MassConversionTable : EntityBase, GenericClasses.ILoadElement
    {
        /// <summary>
        /// Gets or sets the mass conversions.
        /// </summary>
        [XmlElement("conversion", Order = 1)]
        public List<MassConversion> MassConversions = new List<MassConversion>();

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MassConversionTable()
        {

        }

        /// <summary>
        /// Internal constructor from a list of massConversions.
        /// </summary>
        /// <param name="massConversions"></param>
        internal MassConversionTable(List<MassConversion> massConversions)
        {
            this.EntityCreated();
            this.MassConversions = massConversions;
        }

        /// <summary>
        /// Create a new mass conversion table.
        /// </summary>
        /// <param name="factors">Factors for each mass conversion.</param>
        /// <param name="loadCases">Load case for each mass conversion.</param>
        /// <returns></returns>
        public MassConversionTable(List<double> factors, List<LoadCase> loadCases)
        {
            if (factors.Count != loadCases.Count)
            {
                throw new System.ArgumentException($"List must have equal length. Length of factors is: {factors.Count} and length of loadCases is: {loadCases.Count}");
            }

            else if (factors == null || loadCases == null)
            {
                throw new System.ArgumentException("List can not be null");
            }

            else if (factors.Count == 0 || loadCases.Count == 0)
            {
                throw new System.ArgumentException("List must have atleast 1 item");
            }

            else
            {
                List<MassConversion> items = new List<MassConversion>();
                for (int idx = 0; idx < factors.Count; idx++)
                {
                    items.Add(new MassConversion(factors[idx], loadCases[idx].Guid));
                }
                
                this.EntityCreated();
                this.MassConversions = items;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MassConversionTable"/> class.
        /// </summary>
        /// <param name="values">the values.</param>
        public MassConversionTable(params (double gamma, LoadCase lc)[] values)
        {
            // get factors and load cases
            List<double> factors = new List<double>();
            List<LoadCase> loadCases = new List<LoadCase>();

            foreach (var value in values)
            {
                factors.Add(value.gamma);
                loadCases.Add(value.lc);
            }

            List<MassConversion> items = new List<MassConversion>();
            for (int idx = 0; idx < factors.Count; idx++)
            {
                items.Add(new MassConversion(factors[idx], loadCases[idx].Guid));
            }

            this.EntityCreated();
            this.MassConversions = items;
        }

    }
}