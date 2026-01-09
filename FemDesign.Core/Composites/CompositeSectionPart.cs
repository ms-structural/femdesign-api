// https://strusoft.com/

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace FemDesign.Composites
{
    /// <summary>
    /// Represents a Composite Section Part.
    /// </summary>
    [System.Serializable]
    public partial class CompositeSectionPart
    {
        /// <summary>
        /// Gets or sets the material ref.
        /// </summary>
        [XmlAttribute("material")]
        public Guid _materialRef;

        [XmlIgnore]
        public Guid MaterialRef
        {
            get 
            { 
                return this._materialRef; 
            }
            set 
            { 
                this._materialRef = value; 
            }
        }

        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        [XmlIgnore]
        public Materials.Material _material;

        [XmlIgnore]
        public Materials.Material Material
        {
            get
            {
                return this._material;
            }
            set
            {
                this._material = value;
                this.MaterialRef = value.Guid;
            }
        }

        /// <summary>
        /// Gets or sets the section ref.
        /// </summary>
        [XmlAttribute("section")]
        public Guid _sectionRef;

        [XmlIgnore]
        public Guid SectionRef
        {
            get { return this._sectionRef; }
            set { this._sectionRef = value; }
        }

        /// <summary>
        /// Gets or sets the section.
        /// </summary>
        [XmlIgnore]
        public Sections.Section _section;

        [XmlIgnore]
        public Sections.Section Section
        {
            get 
            { 
                return this._section; 
            }
            set
            {
                if (value.SizeName == null || value.SizeName == " ")
                    throw new ArgumentException("SizeName in Section object, must be a non-empty value.");
                this._section = value;
                this._sectionRef = value.Guid;
            }
        }

        /// <summary>
        /// Gets or sets the offset y.
        /// </summary>
        [XmlAttribute("cog_offset_x")]
        public double _offsetY;

        [XmlIgnore]
        public double OffsetY
        {
            get { return _offsetY; }
            set { _offsetY = value; }
        }

        /// <summary>
        /// Gets or sets the offset z.
        /// </summary>
        [XmlAttribute("cog_offset_y")]
        public double _offsetZ;

        [XmlIgnore]
        public double OffsetZ
        {
            get { return _offsetZ; }
            set { _offsetZ = value; }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CompositeSectionPart()
        {

        }

        /// <summary>
        /// Construct a new composite section part with offset. This is for composite section types where the centre of the concrete and steel sections are not at the same point (e.g. beam composite types)
        /// </summary>
        /// <param name="material">Material of composite section part. Can be steel or concrete.</param>
        /// <param name="section">Section part.</param>
        /// <param name="offsetY">Offset of concrete section's centre from steel section's center in Y direction. It must be expressed in meter.</param>
        /// <param name="offsetZ">Offset of concrete section's centre from steel section's center in Z direction. It must be expressed in meter.</param>
        internal CompositeSectionPart(Materials.Material material, Sections.Section section, double offsetY, double offsetZ)
        {
            CheckCompositeSectionPart(material, section);

            this.Material = material;
            this.Section = section;
            this.OffsetY = offsetY;
            this.OffsetZ = offsetZ;
        }

        /// <summary>
        /// Construct a new composite section part without offset. This is for composite section types where the centre of the concrete and steel sections are at the same point (e.g. column composite types).
        /// </summary>
        /// <param name="material">Material of composite section part. Can be steel or concrete.</param>
        /// <param name="section">Section part.</param>
        /// <exception cref="ArgumentException"></exception>
        internal CompositeSectionPart(Materials.Material material, Sections.Section section)
        {
            CheckCompositeSectionPart(material, section);

            this.Material = material;
            this.Section = section;
        }

        internal void CheckCompositeSectionPart(Materials.Material material, Sections.Section section)
        {
            if ((material.Family == Materials.Family.Steel) || (material.Family == Materials.Family.Concrete))
            {
                //pass
            }
            else
            {
                throw new ArgumentException("Material must be steel or concrete!");
            }
            if (section.MaterialFamily != material.Family.ToString())
                throw new ArgumentException("Section material type doesn't match the specified material type");
        }
                
    }
}
