// https://strusoft.com/

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace FemDesign.Composites
{
    /// <summary>
    /// Represents a Composite Section Parameter.
    /// </summary>
    [System.Serializable]
    public partial class CompositeSectionParameter
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public CompositeSectionParameterType Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [XmlAttribute("value")]
        public string Value { get; set; }


        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private CompositeSectionParameter()
        {

        }

        /// <summary>
        /// Construct a new CompositeSectionParameter. 
        /// </summary>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="paramValue">Parameter value. Number values must be expressed in milimeter.</param>
        internal CompositeSectionParameter(CompositeSectionParameterType paramName, string paramValue)
        {
            if ((paramName == CompositeSectionParameterType.Name) && (paramValue == null))
                throw new ArgumentException("Name parameter cannot be null.");

            this.Name = paramName;
            this.Value = paramValue;
        }
               
    }
}
