using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using FemDesign.GenericClasses;
using System.Drawing;
using FemDesign.Bars;

namespace FemDesign.Calculate
{
    /// <summary>
    /// Represents a Cmd Design Group.
    /// </summary>
    [XmlRoot("cmddesgroup")]
    [System.Serializable]
    public class CmdDesignGroup : CmdCommand
    {
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        [XmlAttribute("command")]
        public string Command = "$ CODE_COM DESGROUP"; // token, fixed
        /// <summary>
        /// Name group
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }
        /// <summary>
        /// Color group
        /// </summary>
        [XmlAttribute("color")]
        public string _color { get; set; } = "0xff0000";
        /// <summary>
        /// Color group
        /// </summary>
        [XmlIgnore]
        public Color? Color
        {
            get
            {
                Color col = System.Drawing.ColorTranslator.FromHtml("#" + this._color.Remove(0,2));
                return col;
            }
            set
            {
                this._color = "0x" + ColorTranslator.ToHtml((Color)value).Substring(1);
            }
        }

        /// <summary>
        /// Gets or sets the design group cache.
        /// </summary>
        [XmlIgnore]
        public static Dictionary<string, Calculate.CmdDesignGroup> _designGroupCache = new Dictionary<string, Calculate.CmdDesignGroup>();
        /// <summary>
        /// To delete a group use force="true" and empty guid list.
        /// </summary>
        [XmlAttribute("force")]
        public bool Force { get; set; } = false;
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute("type")]
        public DesignGroupType Type { get; set; }
        /// <summary>
        /// Entities Guid.
        /// all entities guids must be conforming to the group type. remember that you must pass analytical entities!
        /// </summary>
        [XmlElement("GUID")]
        public List<Guid> Guids { get; set; }
        /// <summary>
        /// Gets or sets the elements.
        /// </summary>
        [XmlIgnore]
        public List<FemDesign.GenericClasses.IStructureElement> Elements { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdDesignGroup"/> class.
        /// </summary>
        public CmdDesignGroup()
        {

        }

        /// <summary>
        /// Define a design group of bars
        /// </summary>
        /// <param name="name"></param>
        /// <param name="elements"></param>
        /// <param name="type"></param>
        /// <param name="color"></param>
        public CmdDesignGroup(string name, List<FemDesign.GenericClasses.IStructureElement> elements, DesignGroupType type, Color? color = null) : this(name, elements, color)
        {
            Type = type;
        }


        /// <summary>
        /// Define a design group of bars
        /// </summary>
        /// <param name="name"></param>
        /// <param name="elements"></param>
        /// <param name="color"></param>
        public CmdDesignGroup(string name, List<FemDesign.GenericClasses.IStructureElement> elements, Color? color = null)
        {
            _validateGroup(elements);

            Name = name;

            // assign guids if elements are provided
            if(elements.Count != 0)
            {
                if (elements[0] is FemDesign.Bars.Bar)
                {
                    var bars = elements.Cast<FemDesign.Bars.Bar>().ToList();
                    Guids = bars.Select(x => x.BarPart.Guid).ToList();
                }
                else if (elements[0] is FemDesign.Shells.Slab)
                {
                    var slabs = elements.Cast<FemDesign.Shells.Slab>().ToList();
                    Guids = slabs.Select(x => x.SlabPart.Guid).ToList();
                }
                else
                    throw new Exception($"There is not Design Group Type eligible for {elements[0].GetType().Name}");
                
                Type = assignGroupType(elements);
                Elements = elements;
            }
            else // if no elements, guids will be empty 
            {
                Force = true;
            }

            if (color == null)
            {
                var rnd = new Random();
                color = System.Drawing.Color.FromArgb(255, 0, 0);
            }

            Color = color;

        }

        /// <summary>
        /// Cmd Steel Bar Design Group.
        /// </summary>
        /// <param name="name">the name.</param>
        /// <param name="elements">the elements.</param>
        /// <param name="color">the color.</param>
        /// <returns>The result.</returns>
        public static CmdDesignGroup CmdSteelBarDesignGroup(string name, List<FemDesign.GenericClasses.IStructureElement> elements, Color? color = null)
        {
            var cmdDesignGroup = new CmdDesignGroup(name, elements, DesignGroupType.SteelBars, color);
            return cmdDesignGroup;
        }

        //TODO
        // The design group require to have elements of the same type.
        // i.e. not mixed list of TimberBar, SteelBar and ConcreteBar.
        // Additionally, Concrete Bar group is required to have elements with same bar length.
        private void _validateGroup(List<FemDesign.GenericClasses.IStructureElement> elements)
        {
            if (elements.Count == 0)
                return;

            isSameType(elements);

            if (elements[0] is FemDesign.Bars.Bar bar)
            {
                var material = bar.BarPart.ComplexMaterialObj;
                if (material.Concrete != null)
                    _validateConcreteBeam(elements);
                else if (material.Steel != null || material.Timber != null)
                    return;
            }
        }


        private void _validateConcreteBeam(List<FemDesign.GenericClasses.IStructureElement> elements)
        {
            var bars = elements.Cast<FemDesign.Bars.Bar>().ToList();

            // same material
            var refMaterial = bars[0].BarPart.ComplexMaterialObj.Guid;
            var sameMaterial = bars.All(x => x.BarPart.ComplexMaterialObj.Guid == refMaterial);

            if (!sameMaterial)
                throw new Exception("Concrete material must be the same within the design group!");

            // same section
            var refSectionStart = bars[0].BarPart.ComplexSectionObj.Sections[0].Guid;
            var refSectionEnd = bars[0].BarPart.ComplexSectionObj.Sections[1].Guid;

            var sameSectionStart = bars.All(x => x.BarPart.ComplexSectionObj.Sections[0].Guid == refSectionStart);
            var sameSectionEnd = bars.All(x => x.BarPart.ComplexSectionObj.Sections[1].Guid == refSectionEnd);

            if (!sameSectionStart && !sameSectionEnd)
                throw new Exception("Concrete section must be the same within the design group!");

            // same length
            var refLength = bars[0].BarPart.Edge.Length;
            var sameLength = bars.All(x => Math.Abs(x.BarPart.Edge.Length - refLength) < Tolerance.LengthComparison);

            if (!sameLength)
                throw new Exception("Concrete bars must be have the same length within the design group!");
            
        }

        /// <summary>
        /// Check if elements in group have the same type
        /// </summary>
        /// <param name="elements"></param>
        /// <exception cref="Exception"></exception>
        private void isSameType(List<FemDesign.GenericClasses.IStructureElement> elements)
        {
            var barCount = elements.OfType<FemDesign.Bars.Bar>().Count();
            var slabCount = elements.OfType<FemDesign.Shells.Slab>().Count();
            if (barCount != elements.Count && slabCount != elements.Count)
            {
                throw new Exception("The list of elements contains objects of different types such as Bar, Slab, and Wall.The list must only include objects of the same type.");
            }
        }


        /// <summary>
        /// To X Element.
        /// </summary>
        /// <returns>The result.</returns>
        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdDesignGroup>(this);
        }

        private DesignGroupType assignGroupType(List<FemDesign.GenericClasses.IStructureElement> elements)
        {
            if (elements[0] is FemDesign.Bars.Bar)
            {
                var families = elements.Cast<FemDesign.Bars.Bar>().Select(x => x.BarPart.ComplexMaterialObj.Family).Distinct().ToList().First();

                switch (families)
                {
                    case Materials.Family.Steel:
                        return DesignGroupType.SteelBars;
                    case Materials.Family.Concrete:
                        return DesignGroupType.RCBars;
                    case Materials.Family.Timber:
                        return DesignGroupType.TimberBars;
                    default:
                        throw new NotImplementedException($"There is not Design Group Type eligible for {elements[0].GetType().Name}");
                }
            }
            else if (elements[0] is FemDesign.Shells.Panel)
            {
                var families = elements.Cast<FemDesign.Shells.Panel>().Select(x => x.Material.Family).Distinct().ToList().First();

                if (families == Materials.Family.Timber)
                    return DesignGroupType.TimberPanelCLT;
                else
                    throw new NotImplementedException("Only the TimberCLTPanel type is eligible for the creation of a design group. It's not possible to create a design group for other types.");
            }
            else
                throw new Exception($"There is not Design Group Type eligible for {elements[0].GetType().Name}");
        }
    }

    /// <summary>
    /// Type of elements to set on a design group.
    /// </summary>
    public enum DesignGroupType
    {
        [XmlEnum("FNISOLATED")]
        FoundationIsolated,
        [XmlEnum("FNWALL")]
        FoundationWall,
        [XmlEnum("FNSLAB")]
        FoundationSlab,
        [XmlEnum("RCSURFLONG")]
        RCSurfaceLong,
        [XmlEnum("RCSURFSHEAR")]
        RCSurfaceShear,
        [XmlEnum("RCPUNCH")]
        RCPunchingShear,
        [XmlEnum("STBARSHELL")]
        SteelShellBars,
        [XmlEnum("STJOINT")]
        SteelJoints,
        [XmlEnum("TMBAR")]
        TimberBars,
        [XmlEnum("TMPANEL")]
        TimberPanels,
        [XmlEnum("TMPANELCLT")]
        TimberPanelCLT,
        [XmlEnum("MS")]
        Masonry,
        [XmlEnum("COCOLUMN")]
        ConcreteColumns,
        [XmlEnum("CODELTABEAM")]
        ConcreteDeltaBeams,
        [XmlEnum("RCBAR")]
        RCBars,
        [XmlEnum("RCHIDDENBAR")]
        RCHiddenBars,
        [XmlEnum("STBAR")]
        SteelBars,

        //[XmlEnum("STFIRE")]
        //FireSteelBars,
        //[XmlEnum("TMFIRE")]
        //FireTorsionBars,
        //[XmlEnum("CLTFIRE")]
        //FireanelCLT
    }


}