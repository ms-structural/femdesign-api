using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using StruSoft.Interop.StruXml.Data;


namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Relation Table.
    /// </summary>
    [System.Serializable]
    public class RelationTable
    {
        /// <summary>
        /// Gets or sets the loag group relation records.
        /// </summary>
        [XmlElement("record")]
        public List<LoadGroupRelationRecords> LoagGroupRelationRecords { get; set; }

        private RelationTable() { }
    }

    /// <summary>
    /// Represents a Load Group Relation Records.
    /// </summary>
    [System.Serializable]
    public class LoadGroupRelationRecords
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public List<string> Name { get; set; }
        /// <summary>
        /// Gets or sets the factors.
        /// </summary>
        [XmlAttribute("factors")]
        public List<double> Factors { get; set; }

        private LoadGroupRelationRecords() { }
    }

}
