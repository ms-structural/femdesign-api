// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.Geometry;
using FemDesign.GenericClasses;


namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Footfall.
    /// </summary>
    [System.Serializable]
    public partial class Footfall : EntityBase, ILoadElement
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name;
        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        [XmlAttribute("comment")]
        public string Comment;
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        [XmlElement("region")] 
        public Region Region;
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        [XmlElement("position")]
        public Point3d Position;

        [XmlIgnore]
        public bool IsSelfExcitation
        {
            get { return Region != null && Position == null; }
        }

        [XmlIgnore]
        public bool IsFullExcitation
        {
            get { return Position != null && Region == null; }
        }

        private static int selfExcitationInstances = 0;
        private static int fullExcitationInstances = 0;

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Footfall()
        {

        }

        /// <summary>
        /// Create a Footfall self excitation
        /// </summary>
        /// <param name="region">Contours defining a region to apply footfall analysis on. </param>
        /// <param name="identifier"></param>
        /// <param name="comment"></param>
        public Footfall(Region region, string identifier = "SE", string comment = null)
        {
            InitializeSelfExcitation(region, identifier, comment);
        }

        /// <summary>
        /// Create a Footfall full excitation
        /// </summary>
        /// <param name="position"></param>
        /// <param name="identifier"></param>
        /// <param name="comment"></param>
        public Footfall(Point3d position, string identifier = "FE", string comment = null)
        {
            InitializeFullExcitation(position, identifier, comment);
        }

        private void InitializeSelfExcitation(Region region, string identifier, string comment)
        {
            Region = region;
            Footfall.selfExcitationInstances++;
            Name = $"{identifier}.{selfExcitationInstances}";
            Comment = string.IsNullOrEmpty(comment) ? null : comment;
            this.EntityCreated();
        }

        private void InitializeFullExcitation(Point3d position, string identifier, string comment)
        {
            Position = position;
            Footfall.fullExcitationInstances++;
            Name = $"{identifier}.{fullExcitationInstances}";
            Comment = string.IsNullOrEmpty(comment) ? null : comment;
            this.EntityCreated();
        }
    }
}