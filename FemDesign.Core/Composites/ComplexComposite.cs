// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace FemDesign.Composites
{
    /// <summary>
    /// Represents a Complex Composite.
    /// </summary>
    [System.Serializable]
    public partial class ComplexComposite : EntityBase
    {
        /// <summary>
        /// Gets or sets the parts.
        /// </summary>
        [XmlElement("composite_section")]
        public List<ComplexCompositePart> Parts = new List<ComplexCompositePart>();

        [XmlIgnore]
        public CompositeSection[] CompositeSections
        {
            get
            {
                return this.Parts.Select(s => s.CompositeSectionObj).ToArray();
            }
            set
            {
                if (value.Length > 2)
                {
                    throw new System.ArgumentException($"Length of input composite sections: {value.Length}, does not match number of allowed composite sections: 2." +
                        $" It is ambigious how the sections should be positioned. Create new complex composite section or match input CompositeSection's length.");
                }
                else if (this.Parts.Count == value.Length)
                {
                    for (int idx = 0; idx < value.Length; idx++)
                    {
                        this.Parts[idx].CompositeSectionRef = value[idx].Guid;
                        this.Parts[idx].CompositeSectionObj = value[idx];
                    }
                }
                else if (this.Parts.Count == 2 && value.Length == 1)
                {
                    this.Parts[0].CompositeSectionRef = value[0].Guid;
                    this.Parts[0].CompositeSectionObj = value[0];

                    this.Parts[1].CompositeSectionRef = value[0].Guid;
                    this.Parts[1].CompositeSectionObj = value[0];
                }
            }
        }

        [XmlIgnore]
        public double[] Positions
        {
            get
            {
                return this.Parts.Select(p => p.Pos).ToArray();
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ComplexComposite()
        {

        }

        /// <summary>
        /// Create complex composite from a composite section. Same cross-section at both ends of bar.
        /// </summary>
        /// <param name="compositeSection">Composite section</param>
        internal ComplexComposite(CompositeSection compositeSection)
        {
            this.EntityCreated();
            this.Parts.Add(new ComplexCompositePart(0, compositeSection));
            this.Parts.Add(new ComplexCompositePart(1, compositeSection));

        }

        //public static void CheckComplexComposite(ComplexComposite complex)
        //{
        //    if (complex.Positions[0] != 0)
        //    {
        //        throw new System.Exception($"First position of complex section must be 0 but is: {complex.Parts[0].Pos}");
        //    }
        //    else if (complex.Positions.Last() != 1)
        //    {
        //        throw new System.Exception($"Last position of complex section must be 1 but is: {complex.Parts[0].Pos}");
        //    }
        //    else if (complex.Parts.Count > 2)
        //    {
        //        throw new System.Exception($"Length of Parts can be up to 2 but is: {complex.Parts.Count}");
        //    }
        //    else if(complex.Parts.Where(p => p != complex.Parts[0]).Any())
        //    {
        //        throw new System.Exception($"The same composite section must be used for each composite section part. Variable cross-sections for composite bars are not available in Fem-Design.");
        //    }
        //}
    }
}
