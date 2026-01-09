using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using FemDesign.Reinforcement;


namespace FemDesign.StructureGrid
{
    /// <summary>
    /// Represents axis.
    /// </summary>
    [System.Serializable]
    public partial class Axis: EntityBase, IStructureElement
    {
        /// <summary>
        /// Gets or sets the start point.
        /// </summary>
        [XmlElement("start_point", Order = 1)]
        public Geometry.Point2d _startPoint; // point_type_2d

        [XmlIgnore]
        public Geometry.Point3d StartPoint
        {
            get { return this._startPoint.To3d(); }
            set { this._startPoint = value.To2d(); }
        }

        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        [XmlElement("end_point", Order = 2)]
        public Geometry.Point2d _endPoint; // point_type_2d

        [XmlIgnore]
        public Geometry.Point3d EndPoint
        {
            get { return this._endPoint.To3d(); }
            set { this._endPoint = value.To2d(); }
        }

        /// <summary>
        /// Gets or sets the prefix.
        /// </summary>
        [XmlAttribute("prefix")]
        public string _prefix; // string15

        [XmlIgnore]
        public string Prefix
        {
             get
             {
                 return this._prefix;
             }
             set
             {
                 this._prefix = RestrictedString.Length(value, 15);
             }
        }

        /// <summary>
        /// Gets or sets the axis label.
        /// </summary>
        [XmlElement("axis_label_props", Order = 3)]
        public StruSoft.Interop.StruXml.Data.Axis_label_props AxisLabel { get; set; }


        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [XmlAttribute("id")]
        public int _id; // int_1_to_321272406

        [XmlIgnore]
        public int Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = RestrictedInteger.ValueInRange(value, 1, 321272406);
            }
        }

        [XmlIgnore]
        public string Letter
        {
            get
            {
                if(this.IdIsLetter)
                    return IntToAlphabet(this.Id);
                else
                    return null;
            }
        }

        [XmlIgnore]
        public int? Number
        {
            get
            {
                if (this.IdIsLetter)
                    return null;
                else
                    return this.Id;
            }
        }

        /// <summary>
        /// Gets or sets the id is letter.
        /// </summary>
        [XmlAttribute("id_is_letter")]
        public bool IdIsLetter { get; set; } // bool


        /// <summary>
        /// Private constructor for serialization
        /// </summary>
        private Axis()
        {

        }

        /// <summary>
        /// Convert alphabet to index. A, AA, BA, ZZA
        /// </summary>
        /// <param name="letters"></param>
        /// <returns></returns>
        private int AlphabetToInt(string letters)
        {
            int index = 0;

            foreach (char c in letters)
            {
                if (char.IsLetter(c))
                {
                    index = index * 26 + (char.ToUpper(c) - 'A' + 1);
                }
            }
            return index;

        }

        /// <summary>
        /// Int To Alphabet.
        /// </summary>
        /// <param name="index">the index.</param>
        /// <returns>The result.</returns>
        public static string IntToAlphabet(int index)
        {
            string result = "";

            while (index > 0)
            {
                index--;  // Adjust for 1-based indexing
                int remainder = index % 26;
                result = (char)('A' + remainder) + result;
                index /= 26;
            }

            return result;
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="startPoint">Start point of axis.</param>
        /// <param name="endPoint">End point of axis.</param>
        /// <param name="prefix">Prefix of axis identifier.</param>
        /// <param name="id">Number of axis identifier.</param>
        /// <param name="idIsLetter">Is identifier numbering a letter?</param>
        public Axis(Geometry.Point3d startPoint, Geometry.Point3d endPoint, string prefix, int id, bool idIsLetter)
        {
            this.EntityCreated();
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.Id = id;
            this.IdIsLetter = idIsLetter;
            this.Prefix = prefix;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Axis"/> class.
        /// </summary>
        /// <param name="startPoint">the start point.</param>
        /// <param name="endPoint">the end point.</param>
        /// <param name="number">the number.</param>
        /// <param name="prefix">the prefix.</param>
        public Axis(Geometry.Point3d startPoint, Geometry.Point3d endPoint, int number, string prefix = "")
        {
            this.EntityCreated();
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.Id = number;
            this.IdIsLetter = false;
            this.Prefix = prefix;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Axis"/> class.
        /// </summary>
        /// <param name="startPoint">the start point.</param>
        /// <param name="endPoint">the end point.</param>
        /// <param name="letter">the letter.</param>
        /// <param name="prefix">the prefix.</param>
        public Axis(Geometry.Point3d startPoint, Geometry.Point3d endPoint, string letter, string prefix = "")
        {
            this.EntityCreated();
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.Id = AlphabetToInt(letter);
            this.IdIsLetter = true;
            this.Prefix = prefix;
        }

    }
}