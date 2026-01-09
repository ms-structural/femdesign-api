// https://strusoft.com/
using System;
using System.Xml.Serialization;
using System.Linq;


namespace FemDesign
{
    /// <summary>
    /// Represents a Guid List Type.
    /// </summary>
    [Serializable]
    public partial class GuidListType
    {
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        [XmlAttribute("guid")]
        public Guid Guid { get; set; }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        private GuidListType()
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="GuidListType"/> class.
        /// </summary>
        /// <param name="guid">the guid.</param>
        public GuidListType(Guid guid)
        {
            this.Guid = guid;
        }

        /// <summary>
        /// Implicit conversion of a Entity to its Global Unique Identifier.
        /// </summary>
        /// <param name="entity">Entity to convert.</param>
        /// <returns>The result.</returns>
        public static implicit operator GuidListType(EntityBase entity) => new GuidListType(entity.Guid);

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{this.Guid}";
        }
    }

    /// <summary>
    /// Represents a Two Guid List Type.
    /// </summary>
    [Serializable]
    public partial class TwoGuidListType
    {
        /// <summary>
        /// Gets or sets the first.
        /// </summary>
        [XmlAttribute("first")]
        public Guid First { get; set; }

        /// <summary>
        /// Gets or sets the second.
        /// </summary>
        [XmlAttribute("second")]
        public Guid Second { get; set; }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        private TwoGuidListType()
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TwoGuidListType"/> class.
        /// </summary>
        /// <param name="first">the first.</param>
        /// <param name="second">the second.</param>
        public TwoGuidListType(Guid first, Guid second)
        {
            this.First = first;
            this.Second = second;
        }
    }

    /// <summary>
    /// Represents a Three Guid List Type.
    /// </summary>
    [Serializable]
    public partial class ThreeGuidListType
    {
        /// <summary>
        /// Gets or sets the first.
        /// </summary>
        [XmlAttribute("first")]
        public Guid First { get; set; }

        /// <summary>
        /// Gets or sets the second.
        /// </summary>
        [XmlAttribute("second")]
        public Guid Second { get; set; }

        /// <summary>
        /// Gets or sets the third.
        /// </summary>
        [XmlAttribute("third")]
        public Guid Third { get; set; }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        private ThreeGuidListType()
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ThreeGuidListType"/> class.
        /// </summary>
        /// <param name="first">the first.</param>
        /// <param name="second">the second.</param>
        /// <param name="third">the third.</param>
        public ThreeGuidListType(Guid first, Guid second, Guid third)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
        }
    }
}