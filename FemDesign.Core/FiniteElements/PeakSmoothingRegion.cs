using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.Geometry;
using FemDesign.GenericClasses;


namespace FemDesign.FiniteElements
{
    /// <summary>
    /// Represents a Peak Smoothing Region.
    /// </summary>
    [System.Serializable]
    public partial class PeakSmoothingRegion : EntityBase, IStructureElement
    {
        /// <summary>
        /// Gets or sets the inactive.
        /// </summary>
        [XmlAttribute("inactive")]
        public bool _inactive = false;

        [XmlIgnore]
        public bool Inactive
        {
            get { return this._inactive; }
            set { this._inactive = value; }
        }

        /// <summary>
        /// Gets or sets the contours.
        /// </summary>
        [XmlElement("contour")]
        public List<Geometry.Contour> _contours;

        [XmlIgnore]
        public List<Geometry.Contour> Contours
        {
            get { return this._contours; }
            set { this._contours = value; }
        }

        [XmlIgnore]
        public Region Region
        {
            get { return new Region(this.Contours); }
            set { this._contours = value.Contours; }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private PeakSmoothingRegion()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeakSmoothingRegion"/> class.
        /// </summary>
        /// <param name="region">the region.</param>
        /// <param name="inactive">the inactive.</param>
        public PeakSmoothingRegion(Region region, bool inactive = false)
        {
            this.EntityCreated();
            this.Region = region;
            this.Inactive = inactive;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PeakSmoothingRegion"/> class.
        /// </summary>
        /// <param name="contours">the contours.</param>
        /// <param name="inactive">the inactive.</param>
        public PeakSmoothingRegion(List<Geometry.Contour> contours, bool inactive = false)
        {
            this.EntityCreated();
            this.Contours = contours;
            this.Inactive = inactive;
        }
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            if (this.Inactive)
            {
                return $"{this.GetType().Name}, Inactive";
            }
            else
            {
                return $"{this.GetType().Name}, Active";
            }
        }
    }
}
