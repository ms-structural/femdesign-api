// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;
using System.Globalization;
using FemDesign.GenericClasses;

namespace FemDesign.Reinforcement
{
    /// <summary>
    /// straight (child of surface_rf_type)
    /// </summary>
    [System.Serializable]
    public partial class Straight
    {
        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        [XmlAttribute("direction")]
        public ReinforcementDirection Direction { get; set; }
        /// <summary>
        /// Gets or sets the space.
        /// </summary>
        [XmlAttribute("space")]
        public double _space; // positive_double. Spacing in meters. Required
        [XmlIgnore]
        public double Space
        {
            get {return this._space;}
            set {this._space = RestrictedDouble.Positive(value);}
        }
        /// <summary>
        /// Gets or sets the face.
        /// </summary>
        [XmlAttribute("face")]
        public string _face;
        [XmlIgnore]
        public GenericClasses.Face Face
        {
            set
            {
                if (value == GenericClasses.Face.Mid)
                {
                    this._face = null;
                }
                else
                {
                    this._face = value.ToString().ToLower();
                }
            }
            get
            {
                if (this._face == null)
                {
                    return GenericClasses.Face.Mid;
                }
                else
                {
                    return GenericClasses.EnumParser.Parse<GenericClasses.Face>(this._face);
                }
            }
        }
        /// <summary>
        /// Gets or sets the cover.
        /// </summary>
        [XmlAttribute("cover")]
        public double _cover; // positive_double. Default = 0.02
        [XmlIgnore]
        public double Cover
        {
            get {return this._cover;}
            set {this._cover = RestrictedDouble.Positive(value);}
        }

        [XmlIgnore]
        public bool MultiLayer
        {
            get
            {
                if (this.Face == GenericClasses.Face.Mid)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        [XmlIgnore]
        public bool SingleLayer
        {
            get
            {
                if (this.Face == GenericClasses.Face.Mid)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Straight()
        {

        }

        /// <summary>
        /// Private constructor accessed by static methods.
        /// </summary>
        /// <param name="direction">the direction.</param>
        /// <param name="space">the space.</param>
        /// <param name="face">the face.</param>
        /// <param name="cover">the cover.</param>
        public Straight(ReinforcementDirection direction, double space, GenericClasses.Face face, double cover = 0.02)
        {
            this.Direction = direction;
            this.Space = space;
            this.Face = face;
            this.Cover = cover;
        }

        /// <summary>
        /// Define straight reinforcement layout for surface reinforcement.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="direction">"x"/"y"</param>
        /// <param name="space">Spacing between bars.</param>
        /// <param name="face">"top"/"bottom"</param>
        /// <param name="cover">Reinforcement concrete cover.</param>
        public Straight(string direction, double space, string face, double cover = 0.02)
        {
            this.Direction = EnumParser.Parse<ReinforcementDirection>(direction);
            this.Space = space;
            this.Face = EnumParser.Parse<Face>(face);
            this.Cover = cover;
        }
    }
}