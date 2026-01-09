// https://strusoft.com/
using System;
using System.Globalization;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign
{
    /// <summary>
    /// entity_attribs
    /// </summary>
    [Serializable]
    public abstract partial class EntityBase : IFemDesignEntity
    {
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        [XmlAttribute("guid")]
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the last change.
        /// </summary>
        [XmlAttribute("last_change")]
        public string _lastChange;

        [XmlIgnore]
        public DateTime LastChange
        {
            get
            {
                return DateTime.Parse(this._lastChange);
            }
            internal set
            {
                this._lastChange = value.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        [XmlAttribute("action")]
        public string Action { get; set; }

        /// <summary>
        /// Invoke when an instance is created.
        /// 
        /// Creates a new guid, adds timestamp and changes action.
        /// </summary>
        public void EntityCreated()
        {
            this.Guid = Guid.NewGuid();
            this.LastChange = DateTime.UtcNow;
            this.Action = "added";
        }

        /// <summary>
        /// Invoke when an instance is modified.
        /// 
        /// Changes timestamp and action.
        /// </summary>
        public void EntityModified()
        {
            this.LastChange = DateTime.UtcNow;
            this.Action = "modified";
        }

        /// <summary>
        /// Defines an operator overload.
        /// </summary>
        /// <param name="entity">the entity.</param>
        /// <returns>The result.</returns>
        public static implicit operator Guid(EntityBase entity) => entity.Guid;
    }
}