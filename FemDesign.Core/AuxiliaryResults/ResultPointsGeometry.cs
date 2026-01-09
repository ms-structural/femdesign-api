using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign;
using FemDesign.Geometry;
using FemDesign.GenericClasses;
using StruSoft.Interop.StruXml.Data;

namespace FemDesign.AuxiliaryResults
{
    /// <summary>
    /// Class to contain list in entities. For serialization purposes only.
    /// </summary>
    [System.Serializable]
    public partial class ResultPointsGeometry
    {
        /// <summary>
        /// Gets or sets the result points.
        /// </summary>
        [XmlElement("result_point", Order = 1)]
        public List<ResultPoint> ResultPoints = new List<ResultPoint>();
    }


    /// <summary>
    /// Result Points. Used for extracting detailed results in a Point.
    /// </summary>
    [System.Serializable]
    public partial class ResultPoint : NamedEntityBase, IStructureElement
    {
        [XmlIgnore]
        private static int _resultPointinstances = 0;
        /// <summary>
        /// Reset Instance Count.
        /// </summary>
        public static void ResetInstanceCount() => _resultPointinstances = 0;
        protected override int GetUniqueInstanceCount() => ++_resultPointinstances;

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        [XmlElement("position")]
        public Point3d Position { get; set; }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        [XmlElement("font")]
        public ResPointFont Font { get; set; }

        /// <summary>
        /// Gets or sets the base entity.
        /// </summary>
        [XmlAttribute("base_entity")]
        public Guid BaseEntity { get; set; }

        /// <summary>
        /// Parameterless contructor for serialization
        /// </summary>
        private ResultPoint() { }


        /// <summary>
        /// Initializes a new instance of the <see cref="ResultPoint"/> class.
        /// </summary>
        /// <param name="position">the position.</param>
        /// <param name="element">the element.</param>
        /// <param name="identifier">the identifier.</param>
        public ResultPoint(Point3d position, IStructureElement element, string identifier = "PT")
        {
            Initialize(position, element, identifier);
        }

        private void Initialize(Point3d position, IStructureElement element, string identifier)
        {
            this.EntityCreated();

            Position = position;

            _checkValidDistance(element);

            Guid refGuid;

            if (element is Shells.Slab slab)
            {
                refGuid = new Guid( slab.SlabPart.Guid.ToString() );
            }
            else if (element is Bars.Bar bar)
            {
                refGuid = new Guid( bar.BarPart.Guid.ToString() );
            }
            else
            {
                throw new NotImplementedException($"result point at {element.GetType()} has not been implemented");
            }

            BaseEntity = refGuid;
            Identifier = identifier;
            Font = new ResPointFont();
        }

        /// <summary>
        /// check Valid Distance.
        /// </summary>
        /// <param name="element">the element.</param>
        public void _checkValidDistance(IStructureElement element)
        {
            var onStructure = this.Position.OnStructuralElement(element);

            if (onStructure != true)
                throw new Exception("Point is not part of the structural element!");
        }


        /// <summary>
        /// Represents a Res Point Font.
        /// </summary>
        [System.Serializable]
        public class ResPointFont
        {
            /// <summary>
            /// Gets or sets the script.
            /// </summary>
            [XmlAttribute("script")]
            public string Script = "default";

            /// <summary>
            /// Parameterless contructor for serialization
            /// </summary>
            public ResPointFont() { }
        }
    }
}