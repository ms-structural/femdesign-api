// https://strusoft.com/

using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace FemDesign.Sections
{
    /// <summary>
    /// Complex section.
    /// strusoft.xsd: complex_section_type
    /// </summary>
    [System.Serializable]
    public partial class ComplexSection : EntityBase
    {
        /// <summary>
        /// Gets or sets the parts.
        /// </summary>
        [XmlElement("section")]
        public List<ComplexSectionPart> Parts = new List<ComplexSectionPart>();
        [XmlIgnore]
        public Sections.Section[] Sections
        {
            get
            {
                return this.Parts.Select(x => x.SectionObj).ToArray();
            }
            set
            {
                if (this.Parts.Count == value.Length)
                {
                    for (int idx = 0; idx < value.Length; idx++)
                    {
                        this.Parts[idx].SectionRef = value[idx].Guid;
                        this.Parts[idx].SectionObj = value[idx];
                    }
                }
                else if (this.Parts.Count == 2 && value.Length == 1)
                {
                    this.Parts[0].SectionRef = value[0].Guid;
                    this.Parts[0].SectionObj = value[0];

                    this.Parts[1].SectionRef = value[0].Guid;
                    this.Parts[1].SectionObj = value[0];
                }
                else if (this.Parts.Count != value.Length)
                {
                    throw new System.ArgumentException($"Length of input sections: {value.Length}, does not match length of existing sections: {this.Sections.Length}. It is ambigious how the sections should be positioned. Create new complex section or match input sections length.");
                }
            }
        }
        [XmlIgnore]
        public double[] Positions
        {
            get
            {
                return this.Parts.Select(x => x.Pos).ToArray();
            }
        }
        [XmlIgnore]
        public Bars.Eccentricity[] Eccentricities
        {
            get
            {
                return this.Parts.Select(x => x.Eccentricity).ToArray();
            }
            set
            {
                if (this.Parts.Count == value.Length)
                {
                    for (int idx = 0; idx < value.Length; idx++)
                    {
                        this.Parts[idx].Eccentricity = value[idx];
                    }
                }
                else if(this.Parts.Count == 2 && value.Length == 1)
                {
                    this.Parts[0].Eccentricity = value[0];
                    this.Parts[1].Eccentricity = value[0];
                }
                else if (this.Parts.Count != value.Length)
                {
                    throw new System.ArgumentException($"Length of input eccentricities: {value.Length}, does not match length of existing eccentricities: {this.Eccentricities.Length}. It is ambigious how the eccentrictity should be positioned. Create new complex section or match input eccentricities length.");
                }
            }
        }

        /// <summary>
        /// Check Start End.
        /// </summary>
        public void CheckStartEnd()
        {
            if (this.Parts[0].Pos != 0)
            {
                throw new System.Exception($"First position of complex section must be 0 but is: {this.Parts[0].Pos}");
            }
            if (this.Parts.Last().Pos != 1)
            {
                throw new System.Exception($"Last position of complex section must be 1 but is: {this.Parts[0].Pos}");
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ComplexSection()
        {

        }

        /// <summary>
        /// Create simple complex section from section and eccentricity. Equal cross-section and eccentricity at both ends of bar.
        /// </summary>
        /// <param name="section">Cross-section</param>
        /// <param name="eccentricity">Eccentricity</param>
        internal ComplexSection(Section section, Bars.Eccentricity eccentricity)
        {
            this.EntityCreated();
            this.Parts.Add(new ComplexSectionPart(0, section, eccentricity));
            this.Parts.Add(new ComplexSectionPart(1, section, eccentricity));
        }

        /// <summary>
        /// Create complex section from a start and end section and eccentricity.
        /// </summary>
        internal ComplexSection(Section startSection, Section endSection, Bars.Eccentricity startEccentricity, Bars.Eccentricity endEccentricity)
        {
            this.EntityCreated();
            this.Parts.Add(new ComplexSectionPart(0, startSection, startEccentricity));
            this.Parts.Add(new ComplexSectionPart(1, endSection, endEccentricity));
        }

        /// <summary>
        /// Create complex section from sections and eccentricity
        /// </summary>
        internal ComplexSection(Section[] sections, Bars.Eccentricity[] eccentricities)
        {
            // sections
            var mySections = new Section[2];
            if (sections.Length == 1)
            {
                // create new list with start/end section
                mySections = new Section[2]{ sections[0], sections[0] };
            }
            else if (sections.Length == 2)
            {
                mySections = sections;
            }
            else
            {
                throw new System.ArgumentException($"Length of sections: {sections.Length}, must be 1 (uniform) or 2 (start/end section)");
            }

            // eccentricities
            var myEccentricities = new Bars.Eccentricity[2];
            if (eccentricities.Length == 1)
            {
                // create new list with start/end section
                myEccentricities = new Bars.Eccentricity[2]{ eccentricities[0], eccentricities[0] };
            }
            else if (eccentricities.Length == 2)
            {
                myEccentricities = eccentricities;
            }
            else
            {
                throw new System.ArgumentException($"Length of eccentricities: {eccentricities.Length}, must be 1 (uniform) or 2 (start/end eccentricity)");
            }

            // construct complex section
            var myPositions = new double[2]{0, 1};
            if (mySections.Length == myPositions.Length && myPositions.Length == myEccentricities.Length)
            {
                this.EntityCreated();
                for (int idx = 0; idx < mySections.Length; idx++)
                {
                    this.Parts.Add(new ComplexSectionPart(myPositions[idx], mySections[idx], myEccentricities[idx]));
                }
            }
            else
            {
                throw new System.ArgumentException($"Input arguments have different length. sections: {sections.Length}, positions: {myPositions.Length}, eccentricities: {eccentricities.Length}");
            }
        }

        internal ComplexSection(Section[] sections, double[] positions, Bars.Eccentricity[] eccentricities)
        {
            if (sections.Length < 2)
            {
                throw new System.ArgumentException($"Number of sections: {sections.Length}, must be 2 or more");
            }
            if (sections.Length == positions.Length && positions.Length == eccentricities.Length)
            {
                this.EntityCreated();
                for (int idx = 0; idx < sections.Length; idx++)
                {
                    this.Parts.Add(new ComplexSectionPart(positions[idx], sections[idx], eccentricities[idx]));
                }
            }
            else
            {
                throw new System.ArgumentException($"Input arguments have different length. sections: {sections.Length}, positions: {positions.Length}, eccentricities: {eccentricities.Length}");
            }

            // check parts
            this.CheckStartEnd();
        }


        /// <summary>
        /// Construct a complex section from a list of ModelSections
        /// </summary>
        /// <param name="complexSectionParts">List of model sections</param>
        internal ComplexSection(List<ComplexSectionPart> complexSectionParts)
        {
            this.EntityCreated();
            this.Parts = complexSectionParts;
        }
    }
}