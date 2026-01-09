// https://strusoft.com/

using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_case (child of load_combination_type)
    /// </summary>
    [Serializable]
    public partial class ModelLoadCase : LoadCombinationCaseBase
    {
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        [XmlAttribute("guid")]
        public string _guid = string.Empty;
        [XmlIgnore]
        public Guid Guid
        { // common_load_case --> guidtype indexed_guid
            get
            {
                int i = _guid.IndexOf('#');
                if (i != -1)
                    return new Guid(_guid.Substring(0, i));
                else
                    return new Guid(_guid);
            }
            set
            {
                _guid = IsMovingLoadLoadCase ? _guid = $"{value}#{Index}" : _guid = value.ToString();
            }
        }
        [XmlIgnore]
        public int Index
        {
            get
            {
                int i = _guid.IndexOf('#');
                if (i != -1)
                    return int.Parse(_guid.Substring(i + 1, _guid.Length - i));
                else
                    return -1;
            }
            set
            {
                if (value < -1)
                    throw new ArgumentException("Index must be Non-negative or -1 to indicate it is not a moving load loadcase");
                if (value == -1)
                    _guid = Guid.ToString();
                _guid = $"{Guid}#{value}";
            }
        }
        /// <summary>
        /// Gets a value indicating whether moving load load case.
        /// </summary>
        [XmlIgnore]
        public bool IsMovingLoadLoadCase { get { return _guid.IndexOf('#') != -1; } }
        /// <summary>
        /// Gets or sets the indexed guid.
        /// </summary>
        [XmlIgnore]
        public string IndexedGuid => _guid;
        /// <summary>
        /// Gets or sets the load case.
        /// </summary>
        [XmlIgnore]
        public LoadCase LoadCase { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelLoadCase"/> class.
        /// </summary>
        public ModelLoadCase()
        {
            // parameterless constructor for serialization
        }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="loadCase">LoadCase reference.</param>
        /// <param name="gamma">Gamma value.</param>
        public ModelLoadCase(LoadCase loadCase, double gamma)
        {
            this.Guid = loadCase.Guid;
            this.LoadCase = loadCase;
            this.Gamma = gamma;
        }
    }
}
