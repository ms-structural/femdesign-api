// https://strusoft.com/
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign
{
    /// <summary>
    /// Represents a Load Base.
    /// </summary>
    [System.Serializable]
    public partial class LoadBase: EntityBase, ILoadElement
    {
        /// <summary>
        /// Gets or sets the load case guid.
        /// </summary>
        [XmlAttribute("load_case")]
        public System.Guid LoadCaseGuid { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        [XmlAttribute("comment")]
        public string Comment { get; set; } // comment_string

        /// <summary>
        /// Gets or sets the load case.
        /// </summary>
        [XmlIgnore]
        public Loads.LoadCase _loadCase { get; set; }

        [XmlIgnore]
        public Loads.LoadCase LoadCase
        {
            get { return _loadCase; }
            set
            {
                _loadCase = value;
                LoadCaseGuid = value.Guid;
            }
        }

        public bool IsCaseless
        {
            get
            {
                return this.LoadCase == null;
            }
        }
    }
}