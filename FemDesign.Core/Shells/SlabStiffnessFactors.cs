using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign.GenericClasses;
using System.Xml.Serialization;

namespace FemDesign.Shells
{
    /// <summary>
    /// Represents a Slab Stiffness Factors.
    /// </summary>
    [System.Serializable]
    public partial class SlabStiffnessFactors
    {
        /// <summary>
        /// Gets or sets the factors.
        /// </summary>
        [XmlElement("factors", Order = 1)]
        public List<SlabStiffnessRecord> _factors { get; set; }

        [XmlIgnore]
        public List<SlabStiffnessRecord> Factors
        {
            get
            {
                return this._keyPairAnalysysFactors.Values.ToList();
            }
            set { this._factors = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlabStiffnessFactors"/> class.
        /// </summary>
        public SlabStiffnessFactors()
        {

        }

        /// <summary>
        /// Gets or sets the key pair analysys factors.
        /// </summary>
        [XmlIgnore]
        public Dictionary<StiffnessAnalysisType, SlabStiffnessRecord> _keyPairAnalysysFactors { get; set; }

        /// <summary>
        /// Default.
        /// </summary>
        /// <returns>The result.</returns>
        public static Dictionary<StiffnessAnalysisType, SlabStiffnessRecord> Default()
        {
            var stiffRecordDefault = new SlabStiffnessRecord(1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
            return SameAllCalculation(stiffRecordDefault);
        }



        /// <summary>
        /// Same All Calculation.
        /// </summary>
        /// <param name="stiffRecord">the stiff record.</param>
        /// <returns>The result.</returns>
        public static Dictionary<StiffnessAnalysisType, SlabStiffnessRecord> SameAllCalculation(SlabStiffnessRecord stiffRecord)
        {
            var defaultValues = new Dictionary<StiffnessAnalysisType, SlabStiffnessRecord>();

            defaultValues.Add(StiffnessAnalysisType.FirstOrderU, stiffRecord);

            defaultValues.Add(StiffnessAnalysisType.FirstOrderSq, stiffRecord);
            defaultValues.Add(StiffnessAnalysisType.FirstOrderSf, stiffRecord);
            defaultValues.Add(StiffnessAnalysisType.FirstOrderSc, stiffRecord);

            defaultValues.Add(StiffnessAnalysisType.SecondOrderU, stiffRecord);
            defaultValues.Add(StiffnessAnalysisType.SecondOrderSq, stiffRecord);
            defaultValues.Add(StiffnessAnalysisType.SecondOrderSf, stiffRecord);
            defaultValues.Add(StiffnessAnalysisType.SecondOrderSc, stiffRecord);

            defaultValues.Add(StiffnessAnalysisType.Stability, stiffRecord);
            defaultValues.Add(StiffnessAnalysisType.Dynamic, stiffRecord);

            return defaultValues;
        }
    }
}