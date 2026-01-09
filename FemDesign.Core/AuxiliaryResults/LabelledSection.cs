using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign;
using FemDesign.Geometry;
using FemDesign.GenericClasses;

namespace FemDesign.AuxiliaryResults
{
    /// <summary>
    /// Labelled section. Used for extracting detailed results along a section line or polyline.
    /// </summary>
    [System.Serializable]
    public partial class LabelledSection : NamedEntityBase, IStructureElement
    {
        [XmlIgnore]
        private static int _labelledSectioninstances = 0;
        /// <summary>
        /// Reset Instance Count.
        /// </summary>
        public static void ResetInstanceCount() => _labelledSectioninstances = 0;
        protected override int GetUniqueInstanceCount() => ++_labelledSectioninstances;

        /// <summary>
        /// Gets or sets the line segment.
        /// </summary>
        [XmlElement("line_segment")]
        public LineSegment _lineSegment;
        /// <summary>
        /// Gets or sets the polyline.
        /// </summary>
        [XmlElement("polyline")]
        public Polyline _polyline;
        /// <summary>
        /// The verticies of the labelled section. 2 verticies for line geometry and 3 or more for polyline geometry.
        /// </summary>
        [XmlIgnore]
        public List<Point3d> Verticies { 
            get
            {
                var verticies = new List<Point3d>();
                if (_lineSegment != null)
                    verticies.AddRange(_lineSegment.Verticies);
                if (_polyline != null)
                    verticies.AddRange(_polyline.Verticies);
                return verticies;
            }
            set
            {
                if (value.Count == 2)
                {
                    _lineSegment = new LineSegment(value[0], value[1]);
                    _polyline = null;
                }
                else if (value.Count > 2)
                {
                    _lineSegment = null;
                    _polyline = new Polyline(value);
                }
                else
                    throw new ArgumentException($"LabelledSection must have at least 2 verticies.");
            }
        }
        
        /// <summary>
        /// Parameterless contructor for serialization
        /// </summary>
        private LabelledSection() { }

        /// <summary>
        /// Construct a labelled section
        /// </summary>
        /// <param name="verticies">Verticies</param>
        /// <param name="identifier">Identifier</param>
        public LabelledSection(List<Point3d> verticies, string identifier = "LS")
        {
            Initialize(verticies, identifier);
        }

        /// <inheritdoc cref="LabelledSection(List{Point3d}, string)"/>
        /// <param name="identifier">the identifier.</param>
        /// <param name="verticies">the verticies.</param>
        public LabelledSection(string identifier = "LS", params Point3d[] verticies)
        {
            Initialize(verticies.ToList(), identifier);
        }

        private void Initialize(List<Point3d> verticies, string identifier)
        {
            this.EntityCreated();

            Verticies = verticies;
            Identifier = identifier;
        }
    }
}
