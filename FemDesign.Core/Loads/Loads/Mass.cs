using FemDesign.GenericClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Mass.
    /// </summary>
    [System.Serializable]
    public partial class Mass : EntityBase, ILoadElement
    {
        /// <summary>
        /// Position of mass.
        /// </summary>
        [XmlElement("position")]
        public Geometry.Point3d Position { get; set; }

        /// <summary>
        /// Value of mass in kg.
        /// </summary>
        [XmlAttribute("value")]
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the colouring.
        /// </summary>
        [XmlElement("colouring")]
        public EntityColor Colouring { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        [XmlAttribute("comment")]
        public string Comment { get; set; } = "";

        /// <summary>
        /// Gets or sets the apply on ecc.
        /// </summary>
        [XmlAttribute("apply_on_ecc")]
        public bool ApplyOnEcc { get; set; } = false;

        private Mass()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mass"/> class.
        /// </summary>
        /// <param name="position">the position.</param>
        /// <param name="value">the value.</param>
        /// <param name="applyEcc">the apply ecc.</param>
        /// <param name="comment">the comment.</param>
        public Mass(Geometry.Point3d position, double value, bool applyEcc = false, string comment = "")
        {
            this.EntityCreated();
            this.Position = position;
            this.Value = value;
            this.Comment = comment;
            this.ApplyOnEcc = applyEcc;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{this.GetType().Name} Pos: ({this.Position}) Mass: ({this.Value} kg)";
        }


    }
}
