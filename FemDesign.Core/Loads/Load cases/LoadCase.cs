// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Load Case.
    /// </summary>
    [System.Serializable]
    public partial class LoadCase: EntityBase
    {
        private static Regex _caseNamePattern = new Regex(@"^[ -#%'-;=?A-\uFFFD]{1,80}$");
        // attributes
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public string _name;

        // attributes
        [XmlIgnore]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (!_caseNamePattern.IsMatch(value))
                    throw new ArgumentException("'Name' is not valid!");
                this._name = value;
            }
        }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute("type")]
        public LoadCaseType Type { get; set; } // loadcasetype_type

        /// <summary>
        /// Gets or sets the duration class.
        /// </summary>
        [XmlAttribute("duration_class")]
        public LoadCaseDuration _durationClass; // loadcasedurationtype

        [XmlIgnore]
        public LoadCaseDuration DurationClass // loadcasedurationtype
        { 
            get => _durationClass;
            set
            {
                if(Type == LoadCaseType.Diaphragm && value != LoadCaseDuration.ShortTerm)
                    throw new ArgumentException("Diaphragm load cases must have ShortTerm duration class!");
                else
                    _durationClass = value;
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LoadCase()
        {

        }

        /// <summary>
        /// Create a LoadCase
        /// </summary>
        /// <param name="name">Name/Identifier of LoadCase.</param>
        /// <param name="type">One of "static", "dead_load", "shrinkage", "seis_max", "seis_sxp", "seis_sxm", "seis_syp", "seis_sym", "soil_dead_load", "prestressing", "fire", "deviation", "notional".</param>
        /// <param name="durationClass">One of "permanent", "long-term", "medium-term", "short-term", "instantaneous".</param>
        public LoadCase(string name, LoadCaseType type, LoadCaseDuration durationClass)
        {
            this.EntityCreated();
            this.Name = name;
            this.Type = type;
            this.DurationClass = durationClass;
        }

        /// <summary>
        /// Get the first <see cref="LoadCase"/> in a list with a matching <see cref="Name"/>.
        /// </summary>
        /// <remarks>
        /// Name matching is case-sensitive. Returns <c>null</c> if no match is found.
        /// </remarks>
        /// <param name="loadCases">List of load cases to search.</param>
        /// <param name="name">Load case name to match against <see cref="Name"/>.</param>
        /// <returns>The first matching <see cref="LoadCase"/>, or <c>null</c> if not found.</returns>
        public static LoadCase LoadCaseFromListByName(List<LoadCase> loadCases, string name)
        {
            foreach (LoadCase _loadCase in loadCases)
            {
                if (_loadCase.Name == name)
                {
                    return _loadCase;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{this.GetType().Name} ID: {this.Name}, Type: {this.Type}, Duration: {this.DurationClass}";
        }
    }
}