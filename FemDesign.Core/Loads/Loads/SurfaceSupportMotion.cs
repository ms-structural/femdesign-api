// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Surface Support Motion.
    /// </summary>
    [System.Serializable]
    public partial class SurfaceSupportMotion : SupportMotionBase
    {
        // elements
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        [XmlElement("region", Order = 1)]
        public Geometry.Region Region { get; set; }
        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        [XmlElement("direction", Order = 2)]
        public Geometry.Vector3d Direction { get; set; }
        /// <summary>
        /// Gets or sets the displacements.
        /// </summary>
        [XmlElement("displacement", Order = 3)]
        public List<LoadLocationValue> Displacements = new List<LoadLocationValue>();
        [XmlIgnore]
        public bool IsConstant
        {
            get
            {
                if (this.Displacements.Count > 1)
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private SurfaceSupportMotion()
        {

        }

        /// <summary>
        /// Uniform surface support motion
        /// </summary>
        /// <param name="region"></param>
        /// <param name="displacement"></param>
        /// <param name="loadCase"></param>
        /// <param name="comment"></param>
        public SurfaceSupportMotion(Geometry.Region region, Geometry.Vector3d displacement, LoadCase loadCase, string comment = "") : this(region, new List<LoadLocationValue> { new LoadLocationValue(region.Contours[0].Edges[0].Points[0], displacement.Length()) }, displacement.Normalize(), loadCase, comment)
        {

        }

        /// <summary>
        /// Variable surface support motion
        /// </summary>
        /// <param name="region">the region.</param>
        /// <param name="displacements">the displacements.</param>
        /// <param name="loadDirection">the load direction.</param>
        /// <param name="loadCase">the load case.</param>
        /// <param name="comment">the comment.</param>
        public SurfaceSupportMotion(Geometry.Region region, List<LoadLocationValue> displacements, Geometry.Vector3d loadDirection, LoadCase loadCase, string comment = "")
        {
            this.EntityCreated();
            this.LoadCase = loadCase;
            this.Comment = comment;
            this.SupportMotionType = SupportMotionType.Motion;
            this.Region = region;
            this.Direction = loadDirection;
            foreach (LoadLocationValue _load in displacements)
            {
                this.Displacements.Add(_load);
            }
        }

        /// <summary>
        /// Create uniform surface support motion
        /// </summary>
        /// <param name="region"></param>
        /// <param name="displacement"></param>
        /// <param name="loadCase"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static SurfaceSupportMotion Uniform(Geometry.Region region, Geometry.Vector3d displacement, LoadCase loadCase, string comment = "")
        {
            return new SurfaceSupportMotion(region, displacement, loadCase, comment);
        }

        /// <summary>
        /// Create variable surface support motion
        /// </summary>
        /// <param name="region"></param>
        /// <param name="direction"></param>
        /// <param name="loadLocationValue"></param>
        /// <param name="loadCase"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static SurfaceSupportMotion Variable(Geometry.Region region, Geometry.Vector3d direction, List<LoadLocationValue> loadLocationValue, LoadCase loadCase, string comment = "")
        {
            if (loadLocationValue.Count != 3)
            {
                throw new System.ArgumentException("loadLocationValue must contain 3 items");
            }

            return new SurfaceSupportMotion(region, loadLocationValue, direction, loadCase, comment);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            string text = "";
            if (IsConstant)
            {
                text = $"{this.GetType().Name} u1: {this.Displacements.First().Value * this.Direction} m\u00B2, Constant";
            }
            else
            {
                text = $"{this.GetType().Name} u1: {this.Displacements[0].Value * this.Direction} m\u00B2, u2: {this.Displacements[1].Value * this.Direction} m\u00B2, u3: {this.Displacements[2].Value * this.Direction} m\u00B2, Variable";
            }

            if (LoadCase != null)
                return text + $", LoadCase: {this.LoadCase.Name}";
            else
                return text;
        }
    }
}