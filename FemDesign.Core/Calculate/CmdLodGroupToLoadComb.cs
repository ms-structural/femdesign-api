// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using FemDesign.Loads;
using System.Linq;

namespace FemDesign.Calculate
{
    /// <summary>
    /// command to generate load combinations from load groups.
    /// Similar "Generate" dialog from Load groups
    ///Guids designate load groups for filtering; all used is empty
    /// </summary>
    [XmlRoot("cmdldgroup2comb")]
    [System.Serializable]
    public partial class CmdLoadGroupToLoadComb : CmdCommand
    {
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        [XmlAttribute("command")]
        public string Command = "$ LOAD LDGROUP2COMB"; // token

        /// <summary>
        /// Gets or sets the guids.
        /// </summary>
        [XmlElement("GUID")]
        public List<Guid> Guids { get; set; }

        /// <summary>
        /// Gets or sets the f u.
        /// </summary>
        [XmlAttribute("fU")]
        public bool fU { get; set; } = true;

        /// <summary>
        /// Gets or sets the f ua.
        /// </summary>
        [XmlAttribute("fUa")]
        public bool fUa { get; set; } = true;

        /// <summary>
        /// Gets or sets the f us.
        /// </summary>
        [XmlAttribute("fUs")]
        public bool fUs { get; set; } = true;

        /// <summary>
        /// Gets or sets the f sq.
        /// </summary>
        [XmlAttribute("fSq")]
        public bool fSq { get; set; } = true;

        /// <summary>
        /// Gets or sets the f sf.
        /// </summary>
        [XmlAttribute("fSf")]
        public bool fSf { get; set; } = true;

        /// <summary>
        /// Gets or sets the f sc.
        /// </summary>
        [XmlAttribute("fSc")]
        public bool fSc { get; set; } = true;

        /// <summary>
        /// Gets or sets the f seis signed.
        /// </summary>
        [XmlAttribute("fSeisSigned")]
        public bool fSeisSigned { get; set; } = true;

        /// <summary>
        /// Gets or sets the f seis torsion.
        /// </summary>
        [XmlAttribute("fSeisTorsion")]
        public bool fSeisTorsion { get; set; } = true;

        /// <summary>
        /// Gets or sets the f seis zdir.
        /// </summary>
        [XmlAttribute("fSeisZdir")]
        public bool fSeisZdir { get; set; } = false;

        /// <summary>
        /// Gets or sets the f skip min dl.
        /// </summary>
        [XmlAttribute("fSkipMinDL")]
        public bool fSkipMinDL { get; set; } = true;

        /// <summary>
        /// Gets or sets the f force temp.
        /// </summary>
        [XmlAttribute("fForceTemp")]
        public bool fForceTemp { get; set; } = true;

        /// <summary>
        /// Gets or sets the f short name.
        /// </summary>
        [XmlAttribute("fShortName")]
        public bool fShortName { get; set; } = true;


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdLoadGroupToLoadComb()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdLoadGroupToLoadComb"/> class.
        /// </summary>
        /// <param name="fu">the fu.</param>
        /// <param name="fua">the fua.</param>
        /// <param name="fus">the fus.</param>
        /// <param name="fsq">the fsq.</param>
        /// <param name="fsf">the fsf.</param>
        /// <param name="fsc">the fsc.</param>
        /// <param name="fSeisSigned">flag for <paramref name="fSeisSigned"/>.</param>
        /// <param name="fSeisTorsion">flag for <paramref name="fSeisTorsion"/>.</param>
        /// <param name="fSeisZdir">flag for <paramref name="fSeisZdir"/>.</param>
        /// <param name="fSkipMinDL">flag for <paramref name="fSkipMinDL"/>.</param>
        /// <param name="fForceTemp">flag for <paramref name="fForceTemp"/>.</param>
        /// <param name="fShortName">flag for <paramref name="fShortName"/>.</param>
        public CmdLoadGroupToLoadComb(bool fu = true, bool fua = true, bool fus = true, bool fsq = true, bool fsf = true, bool fsc = true, bool fSeisSigned = true, bool fSeisTorsion = true, bool fSeisZdir = true, bool fSkipMinDL = true, bool fForceTemp = true, bool fShortName = true)
        {
            this.fU = fu;
            this.fUa = fua;
            this.fUs = fus;
            this.fSq = fsq;
            this.fSf = fsf;
            this.fSc = fsc;
            this.fSeisSigned = fSeisSigned;
            this.fSeisTorsion = fSeisTorsion;
            this.fSeisZdir = fSeisZdir;
            this.fSkipMinDL = fSkipMinDL;
            this.fForceTemp = fForceTemp;
            this.fShortName = fShortName;
        }

        /// <summary>
        /// To X Element.
        /// </summary>
        /// <returns>The result.</returns>
        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdLoadGroupToLoadComb>(this);
        }
    }
}