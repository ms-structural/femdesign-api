using FemDesign.GenericClasses;
using System.Xml.Serialization;


namespace FemDesign.Reinforcement
{
    /// <summary>
    /// Represents a Punching Reinforcement.
    /// </summary>
    [System.Serializable]
    public partial class PunchingReinforcement: EntityBase
    {
        /// <summary>
        /// Gets or sets the base shell.
        /// </summary>
        [XmlElement("base_shell", Order = 1)]
        public GuidListType BaseShell { get; set; }

        /// <summary>
        /// Gets or sets the punching area ref.
        /// </summary>
        [XmlElement("punching_area", Order = 2)]
        public GuidListType PunchingAreaRef { get; set; }

        private FemDesign.Reinforcement.PunchingArea _punchingArea;

        [XmlIgnore]
        public FemDesign.Reinforcement.PunchingArea PunchingArea
        {
            get => _punchingArea;
            set
            {
                _punchingArea = value;
                PunchingAreaRef = value != null ? new GuidListType(value.Guid) : null;
            }
        }
        // choice bended_bar
        /// <summary>
        /// Gets or sets the bended bar.
        /// </summary>
        [XmlElement("bended_bar", Order = 3)]
        public BendedBar BendedBar { get; set; }

        // choice open_stirrups
        /// <summary>
        /// Gets or sets the open stirrups.
        /// </summary>
        [XmlElement("open_stirrups", Order = 4)]
        public OpenStirrups OpenStirrups { get; set; }

        // choice reinforcing_ring
        /// <summary>
        /// Gets or sets the reinforcing ring.
        /// </summary>
        [XmlElement("reinforcing_ring", Order = 5)]
        public ReinforcingRing ReinforcingRing { get; set; }

        // choice stud_rails
        /// <summary>
        /// Gets or sets the stud rails.
        /// </summary>
        [XmlElement("stud_rails", Order = 6)]
        public StudRails StudRails { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PunchingReinforcement"/> class.
        /// </summary>
        public PunchingReinforcement()
        {
            this.EntityCreated();
        }


    }

    /// <summary>
    /// Represents a Bended Bar.
    /// </summary>
    [System.Serializable]
    public partial class BendedBar
    {
        /// <summary>
        /// Gets or sets the local center.
        /// </summary>
        [XmlElement("local_center", Order = 1)]
        public Geometry.Point3d LocalCenter { get; set; }

        /// <summary>
        /// Gets or sets the wire.
        /// </summary>
        [XmlElement("wire", Order = 2)]
        public Wire Wire { get; set; }

        /// <summary>
        /// Gets or sets the tip sections length.
        /// </summary>
        [XmlAttribute("tip_sections_length")]
        public double TipSectionsLength { get; set; }

        /// <summary>
        /// Gets or sets the middle sections length.
        /// </summary>
        [XmlAttribute("middle_sections_length")]
        public double MiddleSectionsLength { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [XmlAttribute("height")]
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the angle.
        /// </summary>
        [XmlAttribute("angle")]
        public double Angle { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        [XmlAttribute("direction")]
        public string Direction { get; set; }
    }

    /// <summary>
    /// Represents a Open Stirrups.
    /// </summary>
    [System.Serializable]
    public partial class OpenStirrups
    {
        /// <summary>
        /// Gets or sets the wire.
        /// </summary>
        [XmlElement("wire", Order = 1)]
        public Wire Wire { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        [XmlElement("region", Order = 2)]
        public Geometry.Region Region { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        [XmlAttribute("width")]
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        [XmlAttribute("length")]
        public double Length { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [XmlAttribute("height")]
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the distance x.
        /// </summary>
        [XmlAttribute("distance_x")]
        public double DistanceX { get; set; }

        /// <summary>
        /// Gets or sets the distance y.
        /// </summary>
        [XmlAttribute("distance_y")]
        public double DistanceY { get; set; }
    }

    /// <summary>
    /// Represents a Reinforcing Ring.
    /// </summary>
    [System.Serializable]
    public partial class ReinforcingRing
    {
        /// <summary>
        /// Gets or sets the auxiliary reinforcement.
        /// </summary>
        [XmlElement("auxiliary_reinforcement", Order = 1)]
        public AuxiliaryReinforcement AuxiliaryReinforcement { get; set; }

        /// <summary>
        /// Gets or sets the stirrups.
        /// </summary>
        [XmlElement("stirrups", Order = 2)]
        public ReinforcingRingStirrups Stirrups { get; set; }
    }

    /// <summary>
    /// Represents a Auxiliary Reinforcement.
    /// </summary>
    [System.Serializable]
    public partial class AuxiliaryReinforcement
    {
        /// <summary>
        /// Gets or sets the wire.
        /// </summary>
        [XmlElement("wire", Order = 1)]
        public Wire Wire { get; set; }

        /// <summary>
        /// Gets or sets the inner radius.
        /// </summary>
        [XmlAttribute("inner_radius")]
        public double InnerRadius { get; set; }

        /// <summary>
        /// Gets or sets the overlap.
        /// </summary>
        [XmlAttribute("overlap")]
        public double Overlap { get; set; }
    }

    /// <summary>
    /// Represents a Reinforcing Ring Stirrups.
    /// </summary>
    [System.Serializable]
    public partial class ReinforcingRingStirrups
    {
        /// <summary>
        /// Gets or sets the wire.
        /// </summary>
        [XmlElement("wire", Order = 1)]
        public Wire Wire { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        [XmlAttribute("width")]
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [XmlAttribute("height")]
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the max distance.
        /// </summary>
        [XmlAttribute("max_distance")]
        public double MaxDistance { get; set; }
    }

    /// <summary>
    /// Represents a Stud Rails.
    /// </summary>
    [System.Serializable]
    public partial class StudRails
    {
        // choice general_product
        /// <summary>
        /// Gets or sets the general product.
        /// </summary>
        [XmlElement("general_product", Order = 1)]
        public GeneralProduct GeneralProduct { get; set; }

        // choice peikko_psb_product
        /// <summary>
        /// Gets or sets the peikko psb product.
        /// </summary>
        [XmlElement("peikko_psb_product", Order = 2)]
        public PeikkoPsbProduct PeikkoPsbProduct { get; set; }

        /// <summary>
        /// Gets or sets the pattern.
        /// </summary>
        [XmlAttribute("pattern")]
        public Pattern Pattern { get; set; }

        /// <summary>
        /// Gets or sets the s0.
        /// </summary>
        [XmlAttribute("s0")]
        public double _s0 { get; set; }

        [XmlIgnore]
        public double S0
        {
            get => _s0;
            set => _s0 = RestrictedDouble.NonZeroMax_10_1(value);
        }

        /// <summary>
        /// Gets or sets the s1.
        /// </summary>
        [XmlAttribute("s1")]
        public double _s1 { get; set; }

        [XmlIgnore]
        public double S1
        {
            get => _s1;
            set => _s1 = RestrictedDouble.NonZeroMax_10_2(value);
        }

        /// <summary>
        /// Gets or sets the s2.
        /// </summary>
        [XmlAttribute("s2")]
        public double _s2 { get; set; }

        [XmlIgnore]
        public double S2
        {
            get => _s2;
            set => _s2 = RestrictedDouble.NonZeroMax_10_2(value);
        }

        /// <summary>
        /// Gets or sets the rails on circle.
        /// </summary>
        [XmlAttribute("rails_on_circle")]
        public int _railsOnCircle;

        [XmlIgnore]
        public int RailsOnCircle
        {
            get => _railsOnCircle;
            set => _railsOnCircle = (int)RestrictedDouble.ValueInClosedInterval(value, 4, 50);
        }

        /// <summary>
        /// Gets or sets the studs on rail.
        /// </summary>
        [XmlAttribute("studs_on_rail")]
        public int _studsOnRail;

        [XmlIgnore]
        public int StudsOnRail
        {
            get => _studsOnRail;
            set => _studsOnRail = (int)RestrictedDouble.ValueInClosedInterval(value, 2, 50);
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [XmlAttribute("height")]
        public string _height;

        [XmlIgnore]
        public double Height
        {
            get => double.TryParse(_height, out var value) ? value : 0.0;
            set => _height = RestrictedDouble.ValueInClosedInterval(value, 0.01, 10).ToString();
        }

        /// <summary>
        /// Gets or sets the use minimal elements.
        /// </summary>
        [XmlAttribute("use_minimal_elements")]
        public bool UseMinimalElements { get; set; }
    }

    /// <summary>
    /// Represents a General Product.
    /// </summary>
    [System.Serializable]
    public partial class GeneralProduct
    {
        /// <summary>
        /// Gets or sets the wire.
        /// </summary>
        [XmlElement("wire", Order = 1)]
        public Wire Wire { get; set; }
    }

    /// <summary>
    /// Represents a Peikko Psb Product.
    /// </summary>
    [System.Serializable]
    public partial class PeikkoPsbProduct
    {
        /// <summary>
        /// Gets or sets the psh.
        /// </summary>
        [XmlElement("psh", Order = 1)]
        public PshData Psh { get; set; }

        /// <summary>
        /// Gets or sets the wire diameter.
        /// </summary>
        [XmlAttribute("wire_diameter")]
        public double WireDiameter { get; set; }
    }

    /// <summary>
    /// Represents a Psh Data.
    /// </summary>
    [System.Serializable]
    public partial class PshData
    {
        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        [XmlAttribute("diameter")]
        public double Diameter { get; set; }
        /// <summary>
        /// Gets or sets the cd.
        /// </summary>
        [XmlAttribute("cd")]
        public double Cd { get; set; }
        /// <summary>
        /// Gets or sets the num x direction.
        /// </summary>
        [XmlAttribute("n_x_dir")]
        public int NumXDirection { get; set; }
        /// <summary>
        /// Gets or sets the num y direciton.
        /// </summary>
        [XmlAttribute("n_y_dir")]
        public int NumYDireciton { get; set; }
    }

    /// <summary>
    /// Defines the Pattern enumeration.
    /// </summary>
    public enum Pattern
    {
        [Parseable("Radial", "radial", "RADIAL")]
        [XmlEnum("radial")]
        Radial,
        [Parseable("Orthogonal", "orthogonal", "ORTHOGONAL")]
        [XmlEnum("orthogonal")]
        Orthogonal,
        [Parseable("SemiOrthogonal", "semiorthogonal", "SEMIORTHOGONAL")]
        [XmlEnum("semi-orthogonal")]
        SemiOrthogonal
    }

    /// <summary>
    /// Defines the Direction enumeration.
    /// </summary>
    public enum Direction
    {
        [Parseable("x", "X")]
        [XmlEnum("x")]
        X,
        [Parseable("y", "Y")]
        [XmlEnum("y")]
        Y,
    }
}