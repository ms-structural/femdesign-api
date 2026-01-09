using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using StruSoft.Interop.StruXml.Data;
using FemDesign.Geometry;

namespace FemDesign.Drawing
{
    /// <summary>
    /// Represents a Text Annotation.
    /// </summary>
    [Serializable]
    public class TextAnnotation: GenericClasses.IStructureElement
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        [XmlElement("position")]
        public FemDesign.Geometry.Point3d Position { get; set; }
        /// <summary>
        /// Gets or sets the local x.
        /// </summary>
        [XmlElement("local_x")]
        public Vector3d LocalX { get; set; }
        /// <summary>
        /// Gets or sets the local y.
        /// </summary>
        [XmlElement("local_y")]
        public Vector3d LocalY { get; set; }
        /// <summary>
        /// Gets or sets the style type.
        /// </summary>
        [XmlElement("style")]
        public StruSoft.Interop.StruXml.Data.Style_type StyleType { get; set; }
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        [XmlAttribute("guid")]
        public System.Guid Guid { get; set; }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        [XmlAttribute("text")]
        public string _text;
        [XmlIgnore]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                var regex = "[&#x0009;&#x000A;&#x000d; -&#xfffd;]{0,1023}";
                var match = Regex.Match(value, regex);
                if (match.Success)
                {
                    _text = value;
                }
                else
                {
                    throw new System.ArgumentException($"text value: {value} has bad formatting. Expected regex pattern: {regex}");
                }
            }
        }
        /// <summary>
        /// Gets or sets the default layer.
        /// </summary>
        [XmlIgnore]
        public static string DefaultLayer => "0"; 
        /// <summary>
        /// Initializes a new instance of the <see cref="TextAnnotation"/> class.
        /// </summary>
        public TextAnnotation()
        {

        }
        /// <summary>
        /// Initialize.
        /// </summary>
        public void Initialize()
        {
            Position = FemDesign.Geometry.Point3d.Origin;
            LocalX = Vector3d.UnitX;
            LocalY = Vector3d.UnitY;
            StyleType = new Style_type
            {
                Layer = DefaultLayer,
                Font = new Text_font_type
                {
                    Script = Script_type.Default
                }
            };
            Guid = System.Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextAnnotation"/> class.
        /// </summary>
        /// <param name="position">the position.</param>
        /// <param name="localX">the local x.</param>
        /// <param name="localY">the local y.</param>
        /// <param name="text">the text.</param>
        public TextAnnotation(FemDesign.Geometry.Point3d position, Vector3d localX, Vector3d localY, string text)
        {
            this.Initialize();
            Position = position;
            LocalX = localX;
            LocalY = localY;
            Text = text;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TextAnnotation"/> class.
        /// </summary>
        /// <param name="position">the position.</param>
        /// <param name="localX">the local x.</param>
        /// <param name="localY">the local y.</param>
        /// <param name="styleType">the style type.</param>
        /// <param name="text">the text.</param>
        public TextAnnotation(FemDesign.Geometry.Point3d position, Vector3d localX, Vector3d localY, Style_type styleType, string text)
        {
            this.Initialize();
            Position = position;
            LocalX = localX;
            LocalY = localY;
            StyleType = styleType;
            Text = text;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"TextAnnotation: {this.Text} at {this.Position}";
        }

        /// <summary>
        /// Entity Created.
        /// </summary>
        public void EntityCreated()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Entity Modified.
        /// </summary>
        public void EntityModified()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines an operator overload.
        /// </summary>
        /// <param name="t">value for <paramref name="t"/>.</param>
        /// <returns>The result.</returns>
        public static implicit operator Text_type(TextAnnotation t) => new Text_type{
            Position = t.Position,
            Local_x = t.LocalX,
            Local_y = t.LocalY,
            Style = t.StyleType,
            Guid = t.Guid.ToString(),
            Text = t.Text,
        };
    }
}
