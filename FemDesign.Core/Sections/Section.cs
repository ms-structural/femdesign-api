// https://strusoft.com/

using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using FuzzySharp.Extractor;
using System.IO;

namespace FemDesign.Sections
{
    /// <summary>
    /// Section.
    /// </summary>
    [System.Serializable]
    public partial class Section: EntityBase
    {
        internal static int _fuzzyScore = 85;

        /// <summary>
        /// Gets or sets the region group.
        /// </summary>
        [XmlElement("region_group", Order = 1)]
        public Geometry.RegionGroup _regionGroup;

        [XmlIgnore]
        public Geometry.RegionGroup RegionGroup
        {
            set
            {
                
                Geometry.Vector3d unitZ = Geometry.Vector3d.UnitZ;
                foreach(Geometry.Region region in value.Regions)
                {
                    // check normal
                    int par = region.LocalZ.IsParallel(unitZ);
                    if (par == 1)
                    {
                        // pass
                    }
                    else if (par == -1)
                    {
                        region.LocalZ = unitZ;
                    }
                    else
                    {
                        throw new System.ArgumentException("Normal of region must be parallell with z-axis");
                    }

                    // check if z of any point is not 0
                    if (region.Contours[0].Edges[0].Points[0].Z != 0)
                    {
                        throw new System.ArgumentException("Region must lie in the XY-plane with z=0");
                    }
                }

                this._regionGroup = value;
            }
            get
            {
                return this._regionGroup;
            }
        }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        [XmlElement("end", Order = 2)]
        public string _end { get; set; } // enpty_type

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; } // string i.e. GroupName, TypeName, SizeName --> "Steel sections, CHS, 20-2.0"

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute("type")]
        public string Type { get; set; } // sectiontype

        /// <summary>
        /// Gets or sets the material type.
        /// </summary>
        [XmlAttribute("fd-mat")]
        public string MaterialType { get; set; } // int i.e. 1, 2, 3

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        [XmlAttribute("fd_name_code")]
        public string GroupName { get; set; } // string. Optional i.e. Steel section, Concrete section

        /// <summary>
        /// Gets or sets the type name.
        /// </summary>
        [XmlAttribute("fd_name_type")]
        public string TypeName { get; set; } // string. Optional i.e. CHS, HE-A

        /// <summary>
        /// Gets or sets the size name.
        /// </summary>
        [XmlAttribute("fd_name_size")]
        public string SizeName { get; set; } // string. Optional

        [XmlIgnore]
        internal string _sectionName
        {
            get
            {
                return string.Join("-", new List<string> { this.TypeName + this.SizeName });
            }
        }

        [XmlIgnore]
        internal string _sectionNameInResults
        {
            get
            {
                return string.Join(" ", new List<string> { this.GroupName, this.TypeName, this.SizeName });
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private Section()
        {

        }

        /// <summary>
        /// Construct a new section
        /// </summary>
        /// <param name="regionGroup">the region group.</param>
        /// <param name="type">the type.</param>
        /// <param name="materialTypeEnum">the material type enum.</param>
        /// <param name="groupName">the group name.</param>
        /// <param name="typeName">the type name.</param>
        /// <param name="sizeName">the size name.</param>
        public Section(Geometry.RegionGroup regionGroup, string type, Materials.MaterialTypeEnum materialTypeEnum, string groupName, string typeName, string sizeName)
        {
            this.EntityCreated();
            this.RegionGroup = regionGroup;
            this.Name = $"{groupName}, {typeName}, {sizeName}";
            this.Type = type;
            this.MaterialType = ((int)materialTypeEnum).ToString();
            this.GroupName = groupName;
            this.TypeName = typeName;
            this.SizeName = sizeName;
            this._end = "";
        }


        [XmlIgnore]
        public string MaterialFamily
        {
            get
            {
                string materialFamily = this.GroupName.Split(' ')[0];
                if (materialFamily == "Steel")
                    return "Steel";
                else if (materialFamily == "Concrete")
                    return "Concrete";
                else if (materialFamily == "Timber")
                    return "Timber";
                else if (materialFamily == "Hollow")
                    return "Hollow";
                else
                    return "Custom";
            }
        }
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{this.Name}";
        }
    }


    /// <summary>
    /// Defines the Family enumeration.
    /// </summary>
    public enum Family
    {
        IPE,
        HE_A,
        HE_B,
        HE_M,
        I, 
        U, 
        UAP, 
        UPE, 
        T, 
        TPS, 
        LE, 
        Z, 
        LU, 
        KCKR, 
        VCKR, 
        CHS,
        UPE_Swedish, 
        UKB, 
        UKC, 
        VKR, 
        KKR, 
        D, 
        DR,
        Rectangle,
        Square,
        Circle,
        SawnLumber,
        DressedLumber,
        Glulam,
        HD_F
    }


    public static class SectionExtension
    {
        /// <summary>
        /// Filter Sections By Family.
        /// </summary>
        /// <param name="sections">the sections.</param>
        /// <param name="family">the family.</param>
        /// <returns>The result.</returns>
        public static List<FemDesign.Sections.Section> FilterSectionsByFamily(this List<FemDesign.Sections.Section> sections, FemDesign.Sections.Family family)
        {
            var familyString = family.ToString().Replace("_","-");
            var sectionByFamily = sections.Where(x => x.TypeName == familyString).ToList();

            if (sectionByFamily.Count == 0)
            {
                throw new Exception($"Sections does not contain any '{family}' section");
            }

            return sectionByFamily;
        }

        /// <summary>
        /// Section By Name.
        /// </summary>
        /// <param name="sections">the sections.</param>
        /// <param name="sectionName">the section name.</param>
        /// <returns>The result.</returns>
        public static Section SectionByName(this List<FemDesign.Sections.Section> sections, string sectionName)
        {
            // abbreviation HEA 100
            var sectionNames = sections.Select(x => x._sectionName).ToArray();

            // steel section, HE-A, 100
            var sectionCompleteNames = sections.Select(x => x.Name).ToArray();

            var extracted = FuzzySharp.Process.ExtractOne(sectionName, sectionNames);
            var extract = FuzzySharp.Process.ExtractOne(sectionName, sectionCompleteNames);

            ExtractedResult<string> extr = extracted.Score > extract.Score ? extracted : extract;

            if (extr.Score < Section._fuzzyScore)
                throw new Exception($"{sectionName} can not be found!");
            else
                return sections[extr.Index];
        }


        /// <summary>
        /// Gets the section properties.
        /// </summary>
        /// <param name="section">the section.</param>
        /// <param name="sectionUnits">the section units.</param>
        /// <param name="fdInstallationDir">the fd installation dir.</param>
        /// <returns>The result.</returns>
        public static Results.SectionProperties GetSectionProperties(this Section section, Results.SectionalData sectionUnits = Results.SectionalData.mm, string fdInstallationDir = null)
        {
            // Check input
            if (section == null)
                throw new ArgumentNullException("'section' input cannot be null!");

            var secProp = GetSectionProperties(new List<Section> { section }, sectionUnits, fdInstallationDir);
            return secProp[0];
        }

        /// <summary>
        /// Gets the section properties.
        /// </summary>
        /// <param name="sections">the sections.</param>
        /// <param name="sectionUnits">the section units.</param>
        /// <param name="fdInstallationDir">the fd installation dir.</param>
        /// <returns>The result.</returns>
        public static List<Results.SectionProperties> GetSectionProperties(this List<Section> sections, Results.SectionalData sectionUnits = Results.SectionalData.mm, string fdInstallationDir = null)
        {
            // Check input
            if (sections == null || sections.Count == 0)
                throw new ArgumentNullException("'sections' input cannot be null!");


            // CREATE A MODEL
            // Get materials
            var materialDatabase = FemDesign.Materials.MaterialDatabase.GetDefault();
            var matDatabaseByType = materialDatabase.ByType();
            Materials.Material steel = matDatabaseByType.steel[0];
            Materials.Material concrete = matDatabaseByType.concrete[0];
            Materials.Material timber = matDatabaseByType.timber[0];
            Materials.Material custom = matDatabaseByType.custom[0];

            // Create bars
            var bars = new List<GenericClasses.IStructureElement>();
            int x = 0;
            foreach (Section sec in sections)
            {
                // Set material
                var mat = new Materials.Material();
                if (sec.MaterialFamily is "Concrete")
                    mat = concrete;
                else if (sec.MaterialFamily is "Steel")
                    mat = steel;
                else if (sec.MaterialFamily is "Timber")
                    mat = timber;
                else if (sec.MaterialFamily is "Hollow")
                    mat = concrete;
                else if (sec.MaterialFamily is "Custom")
                    mat = custom;

                bars.Add(new Bars.Bar(new Geometry.Point3d(x, 0, 0), new Geometry.Point3d(x, 1, 0), mat, sec, Bars.BarType.Beam));
                x++;
            }

            var model = new Model(Country.S, bars);


            // LIST SECTION PROPERTIES
            var secProp = new List<Results.SectionProperties>();

            // Run pipe
            using (var femDesign = new FemDesignConnection(fdInstallationDir, minimized: true, tempOutputDir: true))
            {
                femDesign.Open(model);

                var units = Results.UnitResults.Default();
                units.SectionalData = sectionUnits;

                secProp = femDesign._getResults<Results.SectionProperties>(units, timeStamp: true);
            }

            // Method that reorder the secProp list to match the input order using the section name
            var orderedSecProp = new List<Results.SectionProperties>();
            foreach (Section sec in sections)
            {
                var secPropItem = secProp.Find(y => y.Section == sec.Name.Replace(",", ""));
                orderedSecProp.Add(secPropItem);
            }

            return orderedSecProp;
        }

    }
}