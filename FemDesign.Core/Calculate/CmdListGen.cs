// https://strusoft.com/
// https://strusoft.com/
using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Collections.Generic;

namespace FemDesign.Calculate
{
   
    /// <summary> 
    /// fdscript.xsd
    /// CMDLISTGEN
    /// </summary>
    [XmlRoot("cmdlistgen")]
    [System.Serializable]
    public partial class CmdListGen : CmdCommand
    {
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        [XmlAttribute("command")]
        public string Command = "$ MODULECOM LISTGEN"; // token, fixed.

        /// <summary>
        /// Gets or sets the bsc file.
        /// </summary>
        [XmlAttribute("bscfile")]
        public string BscFile { get; set; } // string

        /// <summary>
        /// Gets or sets the bsc.
        /// </summary>
        [XmlIgnore]
        public Bsc Bsc { get; set; }

        /// <summary>
        /// Gets or sets the out file.
        /// </summary>
        [XmlAttribute("outfile")]
        public string OutFile { get; set; } // string

        /// <summary>
        /// Gets or sets the regional.
        /// </summary>
        [XmlAttribute("regional")]
        public bool _regional { get; set; }

        [XmlIgnore]
        public bool Regional
        {
            get
            {
                return this._regional;
            }
            set
            {
                this._regional = value;
            }
        }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        [XmlAttribute("headers")]
        public bool _headers { get; set; }

        [XmlIgnore]
        public bool Headers
        {
            get
            {
                return this._headers;
            }
            set
            {
                this._headers = value;
            }
        }

        /// <summary>
        /// Gets or sets the fill cells.
        /// </summary>
        [XmlAttribute("fillcells")]
        public bool _fillCells { get; set; }

        [XmlIgnore]
        public bool FillCells
        {
            get
            {
                return this._fillCells;
            }
            set
            {
                this._fillCells = value;
            }
        }

        /// <summary>
        /// Gets or sets the ignore case name.
        /// </summary>
        [XmlAttribute("ignorecasename")]
        public bool _ignoreCaseName { get; set; }

        [XmlIgnore]
        public bool IgnoreCaseName
        {
            get
            {
                return this._ignoreCaseName;
            }
            set
            {
                this._ignoreCaseName = value;
            }
        }

        /// <summary>
        /// Gets or sets the structure elements.
        /// </summary>
        [XmlIgnore]
        public List<FemDesign.GenericClasses.IStructureElement> StructureElements { get; set; }

        [XmlElement("GUID")]
        public List<Guid> _elementGuids
        {
            get
            {
                List<Guid> result = new List<Guid>();

                if (this.StructureElements != null)
                    foreach(var element in StructureElements)
                        result.Add(element.Guid);
                return result;
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdListGen()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdListGen"/> class.
        /// </summary>
        /// <param name="bscPath">the bsc path.</param>
        /// <param name="outPath">the out path.</param>
        /// <param name="regional">the regional.</param>
        public CmdListGen(string bscPath, string outPath, bool regional = false)
        {
            OutFile = Path.GetFullPath(outPath);
            BscFile = Path.GetFullPath(bscPath);
            Regional = regional;
            FillCells = true;
            Headers = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdListGen"/> class.
        /// </summary>
        /// <param name="bscPath">the bsc path.</param>
        /// <param name="outPath">the out path.</param>
        /// <param name="elements">the elements.</param>
        /// <param name="regional">the regional.</param>
        public CmdListGen(string bscPath, string outPath, FemDesign.GenericClasses.IStructureElement elements, bool regional = false) : this(bscPath, outPath, regional)
        {
            if(elements != null)
                StructureElements = new List<FemDesign.GenericClasses.IStructureElement> { elements };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdListGen"/> class.
        /// </summary>
        /// <param name="bscPath">the bsc path.</param>
        /// <param name="outPath">the out path.</param>
        /// <param name="elements">the elements.</param>
        /// <param name="regional">the regional.</param>
        public CmdListGen(string bscPath, string outPath, List<FemDesign.GenericClasses.IStructureElement> elements, bool regional = false) : this(bscPath, outPath, regional)
        {
            if(elements != null && elements.Count != 0)
                StructureElements = elements;
        }

        /// <summary>
        /// To X Element.
        /// </summary>
        /// <returns>The result.</returns>
        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdListGen>(this);
        }
    }
}