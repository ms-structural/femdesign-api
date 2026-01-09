// https://strusoft.com/

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

using FemDesign.Sections;
using FemDesign.Materials;
using FemDesign.Geometry;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace FemDesign.Composites
{
    /// <summary>
    /// Represents a Composite Section.
    /// </summary>
    [System.Serializable]
    public partial class CompositeSection : EntityBase
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute("type")]
        public CompositeSectionType _type;

        [XmlIgnore]
        public CompositeSectionType Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        /// <summary>
        /// Gets or sets the parts.
        /// </summary>
        [XmlElement("part", Order = 1)]
        public List<CompositeSectionPart> Parts { get; set; }

        [XmlIgnore]
        public List<Material> Materials
        {
            get
            {
                List<Material> list = new List<Material>();
                foreach (var part in this.Parts)
                {
                    list.Add(part.Material);
                }

                return list;
            }
        }

        [XmlIgnore]
        public List<Section> Sections
        {
            get
            {
                List<Section> list = new List<Section>();
                foreach (var part in this.Parts)
                {
                    list.Add(part.Section);
                }

                return list;
            }
        }

        /// <summary>
        /// Gets or sets the parameter list.
        /// </summary>
        [XmlElement("property", Order = 2)]
        public List<CompositeSectionParameter> ParameterList { get; set; }

        [XmlIgnore]
        public Dictionary<CompositeSectionParameterType, string> ParameterDictionary
        {
            get
            {
                return ParameterList.ToDictionary(p => p.Name, p => p.Value);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CompositeSection()
        {

        }

        /// <summary>
        /// Create a composite section with offset. Use this constructor for beam types. For column types offset must be 0.
        /// </summary>
        /// <param name="type">CompositeType enum member.</param>
        /// <param name="materials">the materials.</param>
        /// <param name="sections">the sections.</param>
        /// <param name="offsetY">Offset of center of each section from center of steel section in Y direction. It must be expressed in meter.</param>
        /// <param name="offsetZ">Offset of center of each section from center of steel section in Z direction.  It must be expressed in meter.</param>
        /// <param name="parameters">List of composite section parameters describing the composite section (e.g. name, geometry, etc.). Values of geometry parameters must be expressed in milimeters.</param>
        /// <exception cref="ArgumentException"></exception>
        internal CompositeSection(CompositeSectionType type, List<Material> materials, List<Section> sections, double[] offsetY, double[] offsetZ, List<CompositeSectionParameter> parameters)
        {
            // check incoming data
            if ((materials.Count != sections.Count) || (materials.Count != offsetY.Length) || (materials.Count != offsetZ.Length))
            {
                throw new ArgumentException("Input variables of materials, sections, offsetY and offsetZ must have the same length.");
            }

            this.EntityCreated();
            this.Type = type;
            this.ParameterList = parameters;
            this.Parts = new List<CompositeSectionPart>();

            for (int i = 0; i < materials.Count; i++)
            {
                this.Parts.Add(new CompositeSectionPart(materials[i], sections[i], offsetY[i], offsetZ[i]));
            }
            this.CheckCompositeSectionPartList(type, this.Parts);
        }

        /// <summary>
        /// Create a composite section without offset. Use this constructor for column types.
        /// </summary>
        /// <param name="type">CompositeType enum member.</param>
        /// <param name="materials">the materials.</param>
        /// <param name="sections">the sections.</param>
        /// <param name="parameters">List of composite section parameters describing the composite section (e.g. name, geometry, etc.). Values of geometry parameters must be expressed in milimeters.</param>
        /// <exception cref="ArgumentException"></exception>
        internal CompositeSection(CompositeSectionType type, List<Material> materials, List<Section> sections, List<CompositeSectionParameter> parameters)
        {
            // check incoming data
            if (materials.Count != sections.Count)
            {
                throw new ArgumentException("Input variables of materials and sections must have the same length.");
            }
            //if (IsOffsetNeeded(type))
            //{
            //    throw new ArgumentException("Offset is required. For this type of composite section, the distance of concrete section part from the steel part must be defined.");
            //}

            this.EntityCreated();
            this.Type = type;
            this.ParameterList = parameters;
            this.Parts = new List<CompositeSectionPart>();

            for (int i = 0; i < materials.Count; i++)
            {
                this.Parts.Add(new CompositeSectionPart(materials[i], sections[i]));
            }
            this.CheckCompositeSectionPartList(type, this.Parts);
        }

        /// <summary>
        /// Create an EffectiveCompositeSlab type CompositeSection object.
        /// </summary>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="steelProfile">Steel section from database. Can be a KKR, VKR or I-profile section type.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="t">Slab thickness [mm].</param>
        /// <param name="bEff">Concrete slab effective width [mm].</param>
        /// <param name="th">Hunch thickness [mm].</param>
        /// <param name="bt">Hunch width at the top [mm].</param>
        /// <param name="bb">Hunch width at the bottom [mm].</param>
        /// <param name="filled">True if steel section is filled with concrete, false if not.</param>
        /// <returns></returns>
        public static CompositeSection EffectiveCompositeSlab(string name, Material steel, Material concrete, Section steelProfile, double t, double bEff, double th, double bt, double bb, bool filled = false)
        {
            CheckMaterialFamily(new List<Material> { steel }, concrete);

            // check input data
            CheckSteelSectionCompatibility(CompositeSectionType.EffectiveCompositeSlab, steelProfile);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = CreateEffectiveCompositeSlabSection(steelProfile, t, bEff, th, bt, bb, filled);

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name),
                new CompositeSectionParameter(CompositeSectionParameterType.t, t.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.bEff, bEff.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.th, th.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.bt, bt.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.bb, bb.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.FillBeam, filled.ToString())
            };

            return new CompositeSection(CompositeSectionType.EffectiveCompositeSlab, materials, sections, parameters);
        }

        /// <summary>
        /// Create a parametric section for EffectiveCompositeSlab type CompositeSection object.
        /// </summary>
        /// <param name="steelProfile">Steel section from database. Can be a KKR, VKR or I-profile section type.</param>
        /// <param name="t">Slab thickness [mm].</param>
        /// <param name="bEff">Concrete slab effective width [mm].</param>
        /// <param name="th">Hunch thickness [mm].</param>
        /// <param name="bt">Hunch width at the top [mm].</param>
        /// <param name="bb">Hunch width at the bottom [mm].</param>
        /// <param name="filled">True if steel section is filled with concrete, false if not.</param>
        /// <returns></returns>
        internal static List<Sections.Section> CreateEffectiveCompositeSlabSection(Section steelProfile, double t, double bEff, double th, double bt, double bb, bool filled = false)
        {
            // round inputs
            t = Math.Round(t, 2, MidpointRounding.AwayFromZero);
            bEff = Math.Round(bEff, 2, MidpointRounding.AwayFromZero);
            th = Math.Round(th, 2, MidpointRounding.AwayFromZero);
            bt = Math.Round(bt, 2, MidpointRounding.AwayFromZero);
            bb = Math.Round(bb, 2, MidpointRounding.AwayFromZero);

            // check inputs
            string steelProfileFamiliy = steelProfile.TypeName;
            var isRHS = steelProfileFamiliy == FemDesign.Sections.Family.VKR.ToString() || steelProfileFamiliy == FemDesign.Sections.Family.KKR.ToString() || steelProfile.RegionGroup.Regions?.Count > 1 || steelProfile.RegionGroup.Regions[0].Contours.Count > 1;
            if (filled && isRHS)
                throw new ArgumentException($" For {CompositeSectionType.EffectiveCompositeSlab} composite section types, the RHS profiles must be unfilled!");
            if (t <= 0 || bEff <= 0 || th < 0 || bt <= 0 || bb <= 0)
                throw new ArgumentException(" Composite section parameters must be positive, non-zero numbers!");
            if (bt < bb)
                throw new ArgumentException(" Hunch width at the top must be greater than or equal to the hunch width at the bottom!");
            if (bEff < bt)
                throw new ArgumentException(" Concrete slab effective width top must be greater than or equal to the hunch width at the top!");

            // conversion of geometric parameters from millimeters to meters
            t /= 1000;
            bEff /= 1000;
            th /= 1000;
            bt /= 1000;
            bb /= 1000;

            // definition of corner points (order of points matters!)
            List<Point3d> points = new List<Point3d>();
            //points.Add(new Point3d(-bEff / 2, heightI / 2 + t + th, 0));
            points.Add(new Point3d(-bEff / 2, 0, 0));
            points.Add(new Point3d(points[0].X, points[0].Y - t, 0));
            points.Add(new Point3d(points[1].X + (bEff - bt) / 2, points[1].Y, 0));
            points.Add(new Point3d(points[2].X + (bt - bb) / 2, points[2].Y - th, 0));
            points.Add(new Point3d(points[3].X + bb, points[3].Y, 0));
            points.Add(new Point3d(points[4].X + (bt - bb) / 2, points[4].Y + th, 0));
            points.Add(new Point3d(points[5].X + (bEff - bt) / 2, points[5].Y, 0));
            points.Add(new Point3d(points[6].X, points[6].Y + t, 0));
            var slabPoints = points.Take(points.Count).ToList();

            // create contours
            Contour steelContour = steelProfile.RegionGroup.Regions[0].Contours[0];
            List<Contour> steelContours = new List<Contour> { steelContour };

            Contour slabContour = new Contour(slabPoints);
            List<Contour> slabContours = new List<Contour> { slabContour };

            // create region groups
            RegionGroup steelRegion = new RegionGroup(new Region(steelContours));
            RegionGroup slabRegion = new RegionGroup(new Region(slabContours));
            if (filled)
            {
                Results.SectionProperties secProp = steelProfile.GetSectionProperties(Results.SectionalData.mm);
                double heightI = secProp.Height;
                double widthI = secProp.Width;
                heightI /= 1000;
                widthI /= 1000;

                points.Add(new Point3d(widthI / 2, heightI / 2, 0));
                points.Add(new Point3d(-widthI / 2, heightI / 2, 0));
                points.Add(new Point3d(-widthI / 2, -heightI / 2, 0));
                points.Add(new Point3d(widthI / 2, -heightI / 2, 0));
                var fillExtPoints = points.Skip(slabPoints.Count).Take(points.Count - slabPoints.Count).ToList();

                List<Contour> fillContours = new List<Contour> { new Contour(fillExtPoints), steelContour };
                slabRegion = new RegionGroup(new List<Region> { new Region(slabContours), new Region(fillContours) });
            }

            // create sections
            List<Section> sections = new List<Section>()
            {
                steelProfile,
                new Section(slabRegion, "custom", MaterialTypeEnum.Concrete, "Concrete section", MaterialTypeEnum.Concrete.ToString(), "--")
            };

            return sections;
        }

        /// <summary>
        /// Create a FilledHSQProfile type CompositeSection object.
        /// </summary>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="b">Intermediate width of the bottom flange [mm].</param>
        /// <param name="bt">Top flange width [mm].</param>
        /// <param name="o1">Left overhang [mm].</param>
        /// <param name="o2">Right overhang [mm].</param>
        /// <param name="h">Web hight [mm].</param>
        /// <param name="tw">Web thickness [mm].</param>
        /// <param name="tfb">Bottom flange thickness [mm].</param>
        /// <param name="tft">Top flange thickness [mm].</param>
        /// <returns></returns>
        public static CompositeSection FilledHSQProfile(string name, Material steel, Material concrete, double b, double bt, double o1, double o2, double h, double tw, double tfb, double tft)
        {
            CheckMaterialFamily(new List<Material> { steel }, concrete);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = CreateFilledHSQSection(b, bt, o1, o2, h, tw, tfb, tft);

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name),
                new CompositeSectionParameter(CompositeSectionParameterType.b, b.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.bt, bt.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.o1, o1.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.o2, o2.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.h, h.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.tw, tw.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.tfb, tfb.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.tft, tft.ToString())
            };

            return new CompositeSection(CompositeSectionType.FilledHSQProfile, materials, sections, parameters);
        }

        /// <summary>
        /// Create a parametric section for FilledHSQProfile type CompositeSection object.
        /// </summary>
        /// <param name="b">Intermediate width of the bottom flange [mm].</param>
        /// <param name="bt">Top flange width [mm].</param>
        /// <param name="o1">Left overhang [mm].</param>
        /// <param name="o2">Right overhang [mm].</param>
        /// <param name="h">Web hight [mm].</param>
        /// <param name="tw">Web thickness [mm].</param>
        /// <param name="tfb">Bottom flange thickness [mm].</param>
        /// <param name="tft">Top flange thickness [mm].</param>
        /// <returns></returns>
        internal static List<Sections.Section> CreateFilledHSQSection(double b, double bt, double o1, double o2, double h, double tw, double tfb, double tft)
        {
            // round inputs
            b = Math.Round(b, 2, MidpointRounding.AwayFromZero);
            bt = Math.Round(bt, 2, MidpointRounding.AwayFromZero);
            o1 = Math.Round(o1, 2, MidpointRounding.AwayFromZero);
            o2 = Math.Round(o2, 2, MidpointRounding.AwayFromZero);
            h = Math.Round(h, 2, MidpointRounding.AwayFromZero);
            tw = Math.Round(tw, 2, MidpointRounding.AwayFromZero);
            tfb = Math.Round(tfb, 2, MidpointRounding.AwayFromZero);
            tft = Math.Round(tft, 2, MidpointRounding.AwayFromZero);

            // check inputs
            if (b <= 0 || bt <= 0 || o1 <= 0 || o2 <= 0 || h <= 0 || tw <= 0 || tfb <= 0 || tft <= 0)
                throw new ArgumentException(" Composite section parameters must be positive, non-zero numbers!");
            if (bt < b)
                throw new ArgumentException(" Top flange width must be greater than or equal to the bottom flange intermediate width!");
            if (tw >= b / 2)
                throw new ArgumentException($" Web thickness must be smaller than b/2 = {b / 2}!");

            // conversion of geometric parameters from millimeters to meters
            b /= 1000;
            bt /= 1000;
            o1 /= 1000;
            o2 /= 1000;
            h /= 1000;
            tw /= 1000;
            tfb /= 1000;
            tft /= 1000;

            // definition of corner points (order of points matters!)
            List<Point3d> points = new List<Point3d>();
            points.Add(new Point3d(-b / 2 + tw, -h / 2, 0));
            points.Add(new Point3d(-points[0].X, points[0].Y, 0));
            points.Add(new Point3d(-points[0].X, -points[0].Y, 0));
            points.Add(new Point3d(points[0].X, -points[0].Y, 0));
            var intPoints = points.Take(4).ToList();

            points.Add(new Point3d(-b / 2, h / 2, 0));
            points.Add(new Point3d(points[4].X, -points[4].Y, 0));
            points.Add(new Point3d(points[5].X - o1, points[5].Y, 0));
            points.Add(new Point3d(points[6].X, points[6].Y - tfb, 0));
            points.Add(new Point3d(b / 2 + o2, points[7].Y, 0));
            points.Add(new Point3d(points[8].X, points[5].Y, 0));
            points.Add(new Point3d(-points[5].X, points[5].Y, 0));
            points.Add(new Point3d(-points[4].X, points[4].Y, 0));
            if (b / 2 != bt / 2)
            {
                points.Add(new Point3d(bt / 2, points[4].Y, 0));
            }
            points.Add(new Point3d(points[points.Count - 1].X, points[4].Y + tft, 0));
            points.Add(new Point3d(-points[points.Count - 1].X, points[points.Count - 1].Y, 0));
            if (b / 2 != bt / 2)
            {
                points.Add(new Point3d(points[points.Count - 1].X, points[4].Y, 0));
            }
            var extPoints = points.Skip(4).Take(points.Count - 4).ToList();

            // create contours
            var intContour = new Contour(intPoints);
            var extContour = new Contour(extPoints);
            List<Contour> contours = new List<Contour> { extContour, intContour };

            // define section data
            List<MaterialTypeEnum> materialTypes = new List<MaterialTypeEnum> { MaterialTypeEnum.SteelWelded, MaterialTypeEnum.Concrete };
            List<string> groupNames = new List<string> { "Steel section", "Concrete section" };
            List<string> typeNames = materialTypes.Select(x => x.ToString()).ToList();

            string steelSizeName = $"{bt * 1000}x{tft * 1000}-{h * 1000}x{tw * 1000}-{(b + o1 + o2) * 1000}x{tfb * 1000}";
            string concreteSizeName = $"{(b - 2 * tw) * 1000}x{h * 1000}";
            List<string> sizeNames = new List<string> { steelSizeName, concreteSizeName };


            return SectionsFromContours(contours, materialTypes, groupNames, typeNames, sizeNames);
        }

        /// <summary>
        /// Create a FilledDeltaBeamProfile type CompositeSection object.
        /// </summary>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="deltaBeamProfile">Delta beam cross-section from database. Can be a D or DR section type.</param>
        /// <param name="name">Composite section name.</param>
        /// <returns></returns>
        public static CompositeSection FilledDeltaBeamProfile(string name, Material steel, Material concrete, Section deltaBeamProfile)
        {
            CheckMaterialFamily(new List<Material> { steel }, concrete);

            // check input data
            CheckSteelSectionCompatibility(CompositeSectionType.FilledDeltaBeamProfile, deltaBeamProfile);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = CreateFilledDeltaBeamSection(deltaBeamProfile);      // !CreateFilledDeltaBeamSection() method is not impemented yet

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name)
            };

            return new CompositeSection(CompositeSectionType.FilledDeltaBeamProfile, materials, sections, parameters);
        }

        /// <summary>
        /// Create a parametric section for FilledDeltaBeamProfile type CompositeSection object.
        /// </summary>
        /// <param name="deltaBeamProfile">Steel section from database. Can be a D or DR section type.</param>
        /// <returns></returns>
        internal static List<Sections.Section> CreateFilledDeltaBeamSection(Section deltaBeamProfile)
        {
            // get contour points for concrete section part
            List<Region> steelRegions = deltaBeamProfile.RegionGroup.Regions;
            List<Edge> intSideEdgs = GetDeltaBeamSideEdges(steelRegions, out double topPtsY);
            List<Point3d> concretePts = GetDeltaBeamInteriorPoints(intSideEdgs, topPtsY);

            // create contours
            List<Contour> concreteContours = new List<Contour> { new Contour(concretePts) };

            // create region groups
            RegionGroup conreteRegion = new RegionGroup(new Region(concreteContours));

            // create sections
            List<Section> sections = new List<Section>()
            {
                deltaBeamProfile,
                new Section(conreteRegion, "custom", MaterialTypeEnum.Concrete, "Concrete section", MaterialTypeEnum.Concrete.ToString(), "--")
            };

            return sections;
        }

        /// <summary>
        /// Private method for CreateFilledDeltaBeamSection()
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        private static List<Edge> GetDeltaBeamSideEdges(List<Region> deltaBeamSteelRegions, out double topPtsY)
        {
            if (deltaBeamSteelRegions?.Count != 4)
                throw new ArgumentException("Invalid input section! The input section must be a Deltabeam profile and the number of regions must be 4!");

            topPtsY = 0;
            List<Edge> sideEdges = new List<Edge>();

            foreach (var region in deltaBeamSteelRegions)
            {
                if (region.Contours?.Count != 1)
                    throw new ArgumentException("Invalid input section! Number of contours must be 1!");

                var points = region.Contours[0].Points;
                var topPts = points.Where(p => p.Y > 0).ToList();

                if (topPts.Count == points.Count)
                {
                    topPtsY = points.Select(p => p.Y).Min();
                }
                else if (topPts.Count != points.Count && topPts.Count != 0)
                {
                    var edges = region.Contours[0].Edges;
                    var interiorEdg = edges.OrderBy(e => ((Vector3d)e.GetIntermediatePoint(0.5)).Length()).First();

                    sideEdges.Add(interiorEdg);
                }
            }

            if (sideEdges?.Count == 0 || topPtsY == 0)
                throw new Exception("Invalid section geometry! Deltabeam section origin must be (0,0,0)!");

            return sideEdges;
        }

        /// <summary>
        /// Private method for CreateFilledDeltaBeamSection()
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static List<Point3d> GetDeltaBeamInteriorPoints(List<Edge> sideEdges, double topPtsY)
        {
            Point3d leftBottPt = null;
            Point3d leftTopPt = null;
            Point3d rightBottPt = null;
            Point3d rightTopPt = null;

            foreach (var edg in sideEdges)
            {
                var pts = edg.Points.OrderBy(p => p.Y).ToList();
                Edge newEdg = new Edge(pts[0], pts[1]);
                double h = Math.Abs(pts[0].Y) + Math.Abs(pts[1].Y);
                double hi = Math.Abs(pts[0].Y) + topPtsY;
                Point3d pi = newEdg.GetIntermediatePoint(hi / h);
                if (pts[0].X < 0)
                {
                    leftBottPt = pts[0];
                    leftTopPt = pi;
                }
                else if (pts[0].X > 0)
                {
                    rightBottPt = pts[0];
                    rightTopPt = pi;
                }
                else
                {
                    throw new ArgumentException("Invalid section geometry! Section origin must be (0,0,0)!");
                }
            }

            return new List<Point3d>() { leftTopPt, leftBottPt, rightBottPt, rightTopPt };
        }

        /// <summary>
        /// Create a FilledIProfile type CompositeSection object.
        /// </summary>
        /// <param name="name">Composite section name.</param>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="steelIProfile">Steel section from database. Must be an I-shaped section type.</param>
        /// <param name="cy">Concrete cover in Y direction [mm].</param>
        /// <param name="cz">Concrete cover in Z direction [mm].</param>
        /// <returns></returns>
        public static CompositeSection FilledIProfile(string name, Material steel, Material concrete, Section steelIProfile, double cy, double cz)
        {
            CheckMaterialFamily(new List<Material> { steel }, concrete);

            // check input data
            CheckSteelSectionCompatibility(CompositeSectionType.FilledIProfile, steelIProfile);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = CreateFilledISection(steelIProfile, cy, cz);

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name),
                new CompositeSectionParameter(CompositeSectionParameterType.cy, cy.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.cz, cz.ToString())
            };

            return new CompositeSection(CompositeSectionType.FilledIProfile, materials, sections, parameters);
        }

        /// <summary>
        /// Create a parametric section for FilledIProfile type CompositeSection object.
        /// </summary>
        /// <param name="steelIProfile">Steel section from database. Can be a KKR or VKR section type.</param>
        /// <param name="cy">Concrete cover in Y direction [mm].</param>
        /// <param name="cz">Concrete cover in Z direction [mm].</param>
        /// <returns></returns>
        internal static List<Sections.Section> CreateFilledISection(Section steelIProfile, double cy, double cz)
        {
            // round inputs
            cy = Math.Round(cy, 2, MidpointRounding.AwayFromZero);
            cz = Math.Round(cz, 2, MidpointRounding.AwayFromZero);

            // check inputs
            if (cy < 0 || cz < 0)
                throw new ArgumentException(" Composite section parameters must be positive numbers!");

            // definition of corner points (order of points matters!)
            Results.SectionProperties secProp = steelIProfile.GetSectionProperties(Results.SectionalData.mm);
            double h = secProp.Height;
            double w = secProp.Width;

            // conversion of geometric parameters from millimeters to meters
            cy /= 1000;
            cz /= 1000;
            h /= 1000;
            w /= 1000;

            List<Point3d> points = new List<Point3d>();
            points.Add(new Point3d(w / 2 + cy, h / 2 + cz, 0));
            points.Add(new Point3d(-points[0].X, points[0].Y, 0));
            points.Add(new Point3d(-points[0].X, -points[0].Y, 0));
            points.Add(new Point3d(points[0].X, -points[0].Y, 0));

            // get contours
            Contour extContour = new Contour(points);
            Contour intContour = steelIProfile.RegionGroup.Regions[0].Contours[0];
            List<Contour> contours = new List<Contour> { extContour, intContour };

            // define section data
            MaterialTypeEnum matTypeIProfile = (MaterialTypeEnum)Enum.Parse(typeof(MaterialTypeEnum), steelIProfile.MaterialType);
            List<MaterialTypeEnum> materialTypes = new List<MaterialTypeEnum> { MaterialTypeEnum.Concrete, matTypeIProfile };
            List<string> groupNames = new List<string> { "Concrete section", steelIProfile.GroupName };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<string> typeNames = new List<string> { MaterialTypeEnum.Concrete.ToString(), steelIProfile.TypeName };
            List<string> sizeNames = new List<string> { "--", steelIProfile.SizeName };

            var sections = SectionsFromContours(contours, materialTypes, groupNames, typeNames, sizeNames);
            sections.Reverse();

            return sections;
        }

        /// <summary>
        /// Create a FilledCruciformProfile type CompositeSection object.
        /// </summary>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="bc">Cross-section width [mm].</param>
        /// <param name="hc">Cross-section height [mm].</param>
        /// <param name="bf">Flange width [mm].</param>
        /// <param name="tw">Web thickness [mm].</param>
        /// <param name="tf">Flange thickness [mm].</param>
        /// <returns></returns>
        public static CompositeSection FilledCruciformProfile(string name, Material steel, Material concrete, double bc, double hc, double bf, double tw, double tf)
        {
            CheckMaterialFamily(new List<Material> { steel }, concrete);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = CreateFilledCruciformSection(bc, hc, bf, tw, tf);

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name),
                new CompositeSectionParameter(CompositeSectionParameterType.bc, bc.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.hc, hc.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.bf, bf.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.tw, tw.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.tf, tf.ToString())
            };

            return new CompositeSection(CompositeSectionType.FilledCruciformProfile, materials, sections, parameters);
        }

        /// <summary>
        /// Create a parametric section for FilledCruciformProfile type CompositeSection object.
        /// </summary>
        /// <param name="bc">Cross-section width [mm].</param>
        /// <param name="hc">Cross-section height [mm].</param>
        /// <param name="bf">Flange width [mm].</param>
        /// <param name="tw">Web thickness [mm].</param>
        /// <param name="tf">Flange thickness [mm].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        internal static List<Sections.Section> CreateFilledCruciformSection(double bc, double hc, double bf, double tw, double tf)
        {
            // round inputs
            bc = Math.Round(bc, 2, MidpointRounding.AwayFromZero);
            hc = Math.Round(hc, 2, MidpointRounding.AwayFromZero);
            bf = Math.Round(bf, 2, MidpointRounding.AwayFromZero);
            tw = Math.Round(tw, 2, MidpointRounding.AwayFromZero);
            tf = Math.Round(tf, 2, MidpointRounding.AwayFromZero);

            // check inputs
            if (bc <= 0 || hc <= 0 || bf <= 0 || tw <= 0 || tf <= 0)
                throw new ArgumentException(" Composite section parameters must be positive, non-zero numbers!");
            if (bf <= tw)
                throw new ArgumentException(" Flange width must be greater than the thickness of the web!");
            if (bf >= (bc - 2 * tf) && bf >= (hc - 2 * tf))
                throw new ArgumentException(" Flange width must be smaller than the web hight/width!");
            if (tf >= ((bc - bf) / 2) || tf >= ((hc - bf) / 2))
                throw new ArgumentException(" Flange thickness is too big!");

            // conversion of geometric parameters from millimeters to meters
            bc /= 1000;
            hc /= 1000;
            bf /= 1000;
            tw /= 1000;
            tf /= 1000;


            // The cruciform section is constructed from 7 rectangles. 
            // corner points
            Point3d corner0 = new Point3d(-tw / 2, -hc / 2 + tf, 0);    // left bottom corner point of the vertical web
            Point3d corner1 = new Point3d(-bf / 2, -hc / 2, 0);     // left bottom corner point of the bottom horizontal flange
            Point3d corner2 = new Point3d(-bc / 2, -bf / 2, 0);     // left bottom corner point of the left vertical flange
            Point3d corner3 = new Point3d(-bc / 2 + tf, -tw / 2, 0);    // left bottom corner point of the left horizontal web

            Point3d[] corners = { corner0, corner1, corner2, corner3 };     // Corner points of the rectangles. Order matters!
            double[] w = { tw, bf, tf, (bc - 2 * tf - tw) / 2 };    // Width of the rectangles. Order matters!
            double[] h = { hc - 2 * tf, tf, bf, tw };   // Height of the rectangles. Order matters!

            List<Region> steelRegs = new List<Region>();
            for (int i = 0; i < corners.Length; i++)
            {
                if(i == 0)
                {
                    steelRegs.Add(Region.RectangleXY(corners[i], w[i], h[i]));  // vertical web
                }
                else if(i== 1)
                {
                    // bottom and top horizontal flanges
                    Region reg = Region.RectangleXY(corners[i], w[i], h[i]);
                    steelRegs.Add(reg);
                    List<Point3d> pts = reg.Contours[0].Points.Select(p => new Point3d(p.X, -p.Y, 0)).ToList();
                    pts.RemoveAt(pts.Count - 1);
                    steelRegs.Add(new Region(pts, new Plane(pts[0], pts[1], pts[2])));
                }
                else
                {
                    // left and right vertical flanges and horizontal webs
                    Region reg = Region.RectangleXY(corners[i], w[i], h[i]);
                    steelRegs.Add(reg);
                    List<Point3d> pts = reg.Contours[0].Points.Select(p => new Point3d(-p.X, p.Y, 0)).ToList();
                    pts.RemoveAt(pts.Count - 1);
                    steelRegs.Add(new Region(pts, new Plane(pts[0], pts[1], pts[2])));
                }
            }

            // define section attributes
            RegionGroup concreteReg = new RegionGroup(new List<Region> { steelRegs[0] });
            RegionGroup steelReg = new RegionGroup(steelRegs);

            string concreteSizeName = $"{hc * 1000}-{bc * 1000}";
            string steelSizeName = $"{bf * 1000}x{tf * 1000}-{hc * 1000}x{tw * 1000}-{bc * 1000}x{tw * 1000}";

            // create and return sections
            return new List<Section>()
            {
                new Section(steelReg, "custom", MaterialTypeEnum.SteelWelded, "Steel section", MaterialTypeEnum.SteelWelded.ToString(), steelSizeName),
                new Section(concreteReg, "custom", MaterialTypeEnum.Concrete, "Concrete section", MaterialTypeEnum.Concrete.ToString(), concreteSizeName)

            };
        }

        /// <summary>
        /// Create a FilledRHSProfile type CompositeSection object.
        /// </summary>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="steelRHSProfile">Steel section from database. Can be a KKR or VKR section type.</param>
        /// <param name="name">Composite section name.</param>
        /// <returns></returns>
        public static CompositeSection FilledRHSProfile(string name, Material steel, Material concrete, Section steelRHSProfile)
        {
            CheckMaterialFamily(new List<Material> { steel }, concrete);

            // check input data
            CheckSteelSectionCompatibility(CompositeSectionType.FilledRHSProfile, steelRHSProfile);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = CreateFilledRHSSection(steelRHSProfile);

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name)
            };

            return new CompositeSection(CompositeSectionType.FilledRHSProfile, materials, sections, parameters);
        }

        /// <summary>
        /// Create a parametric section for FilledRHSProfile type CompositeSection object.
        /// </summary>
        /// <param name="steelRHSProfile">Steel section from database. Can be a KKR or VKR section type.</param>
        /// <returns></returns>
        internal static List<Sections.Section> CreateFilledRHSSection(Section steelRHSProfile)
        {
            // get contours
            var region = steelRHSProfile.RegionGroup.Regions[0];
            List<Contour> contours = new List<Contour> { region.Contours[0], region.Contours[1] };

            // define section data
            MaterialTypeEnum matTypeRHSProfile = (MaterialTypeEnum)Enum.Parse(typeof(MaterialTypeEnum), steelRHSProfile.MaterialType);
            List<MaterialTypeEnum> materialTypes = new List<MaterialTypeEnum> { matTypeRHSProfile, MaterialTypeEnum.Concrete };
            List<string> groupNames = new List<string> { steelRHSProfile.GroupName, "Concrete section" };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<string> typeNames = new List<string> { steelRHSProfile.TypeName, MaterialTypeEnum.Concrete.ToString() };
            List<string> sizeNames = new List<string> { steelRHSProfile.SizeName, "--" };

            return SectionsFromContours(contours, materialTypes, groupNames, typeNames, sizeNames);
        }

        /// <summary>
        /// Create a FilledSteelTube type CompositeSection object.
        /// </summary>
        /// <param name="steel">Steel tube material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="d">Steel tube exterior diameter [mm].</param>
        /// <param name="t">Steel tube thickness [mm].</param>
        /// <returns></returns>
        public static CompositeSection FilledSteelTube(string name, Material steel, Material concrete, double d, double t)
        {
            CheckMaterialFamily(new List<Material> { steel }, concrete);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = CreateFilledSteelTubeSection(d, t);

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name),
                new CompositeSectionParameter(CompositeSectionParameterType.d, d.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.t, t.ToString())
            };

            return new CompositeSection(CompositeSectionType.FilledSteelTube, materials, sections, parameters);
        }

        /// <summary>
        /// Create a parametric section for FilledSteelTube type CompositeSection object.
        /// </summary>
        /// <param name="d">Steel tube exterior diameter [mm].</param>
        /// <param name="t">Steel tube thickness [mm].</param>
        /// <returns></returns>
        internal static List<Sections.Section> CreateFilledSteelTubeSection(double d, double t)
        {
            // round inputs
            d = Math.Round(d, 2, MidpointRounding.AwayFromZero);
            t = Math.Round(t, 2, MidpointRounding.AwayFromZero);

            // check inputs
            if (d <= 0 || t <= 0)
                throw new ArgumentException("Composite section parameters must be positive, non zero numbers!");
            if (d <= 2 * t)
                throw new ArgumentException($"Diameter must be greater than 2t = {2 * t}!");

            // conversion of geometric parameters from millimeters to meters
            d /= 1000;
            t /= 1000;

            var center = new Point3d(0, 0, 0);
            double extRadius = d / 2;
            double intRadius = (d - 2 * t) / 2;
            var plane = new Plane(center, Vector3d.UnitX, Vector3d.UnitY);

            var extCircle = new CircleEdge(extRadius, center, plane);
            var intCircle = new CircleEdge(intRadius, center, plane);

            // create contours
            var extContour = new Contour(new List<Edge>() { extCircle });
            var intContour = new Contour(new List<Edge>() { intCircle });
            List<Contour> contours = new List<Contour> { extContour, intContour };

            // define section data
            List<MaterialTypeEnum> materialTypes = new List<MaterialTypeEnum> { MaterialTypeEnum.SteelColdWorked, MaterialTypeEnum.Concrete };
            List<string> groupNames = new List<string> { "Steel section", "Concrete section" };
            List<string> typeNames = materialTypes.Select(x => x.ToString()).ToList();

            string steelSizeName = $"{d * 1000}-{t * 1000}";
            string concreteSizeName = $"{(d - 2 * t) * 1000}";
            List<string> sizeNames = new List<string> { steelSizeName, concreteSizeName };


            return SectionsFromContours(contours, materialTypes, groupNames, typeNames, sizeNames);
        }

        /// <summary>
        /// Create a FilledSteelTubeWithIProfile type CompositeSection object.
        /// </summary>
        /// <param name="steelTube">Steel tube's material.</param>
        /// <param name="steelI">I-shaped section's material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="steelIProfile">Steel section from database. Must be an I-shaped section type.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="d">Steel tube exterior diameter [mm].</param>
        /// <param name="t">Steel tube thickness [mm].</param>
        /// <returns></returns>
        public static CompositeSection FilledSteelTubeWithIProfile(string name, Material steelTube, Material steelI, Material concrete, Section steelIProfile, double d, double t)
        {
            CheckMaterialFamily(new List<Material> { steelI, steelTube }, concrete);

            // check input data
            CheckSteelSectionCompatibility(CompositeSectionType.FilledSteelTubeWithIProfile, steelIProfile);

            List<Material> materials = new List<Material>() { steelI, steelTube, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = CreateFilledSteelTubeWithISection(steelIProfile, d, t);

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name),
                new CompositeSectionParameter(CompositeSectionParameterType.d, d.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.t, t.ToString())
            };

            return new CompositeSection(CompositeSectionType.FilledSteelTubeWithIProfile, materials, sections, parameters);
        }

        /// <summary>
        /// Create a parametric section for FilledSteelTubeWithIProfile type CompositeSection object.
        /// </summary>
        /// <param name="steelIProfile">Steel section from database. Must be an I-shaped section type.</param>
        /// <param name="d">Steel tube exterior diameter [mm].</param>
        /// <param name="t">Steel tube thickness [mm].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        internal static List<Sections.Section> CreateFilledSteelTubeWithISection(Section steelIProfile, double d, double t)
        {
            Results.SectionProperties secProp = steelIProfile.GetSectionProperties(Results.SectionalData.mm);
            double h = secProp.Height;
            double w = secProp.Width;
            double dI = Math.Sqrt(h * h + w * w);

            // round inputs
            d = Math.Round(d, 2, MidpointRounding.AwayFromZero);
            t = Math.Round(t, 2, MidpointRounding.AwayFromZero);

            // check inputs
            if (d <= 0 || t <= 0)
                throw new ArgumentException("Composite section parameters must be positive, non-zero numbers!");
            if (d <= 2 * t)
                throw new ArgumentException($"Diameter must be greater than 2t = {2 * t}!");
            if (dI >= d - 2 * t)
                throw new ArgumentException("Steel tube and 'I' profile partly overlap each other. Increase the tube diameter or select a smaller 'I' profile!");

            // conversion of geometric parameters from millimeters to meters
            d /= 1000;
            t /= 1000;

            // definition of edges
            var center = new Point3d(0, 0, 0);
            double tubeExtRadius = d / 2;
            double tubeIntRadius = (d - 2 * t) / 2;
            var plane = new Plane(center, Vector3d.UnitX, Vector3d.UnitY);

            var tubeExtEdge = new CircleEdge(tubeExtRadius, center, plane);
            var tubeIntEdge = new CircleEdge(tubeIntRadius, center, plane);

            // create contours
            var tubeExtContour = new Contour(new List<Edge>() { tubeExtEdge });
            var tubeIntContour = new Contour(new List<Edge>() { tubeIntEdge });
            var contourIProfile = steelIProfile.RegionGroup.Regions[0].Contours[0];
            List<Contour> contours = new List<Contour> { tubeExtContour, tubeIntContour, contourIProfile };

            // define section data
            MaterialTypeEnum matTypeIProfile = (MaterialTypeEnum)Enum.Parse(typeof(MaterialTypeEnum), steelIProfile.MaterialType);
            MaterialTypeEnum matTypeTube = MaterialTypeEnum.SteelWelded;
            MaterialTypeEnum matTypeConcrete = MaterialTypeEnum.Concrete;
            List<MaterialTypeEnum> materialTypes = new List<MaterialTypeEnum> { matTypeTube, matTypeConcrete, matTypeIProfile };
            List<string> groupNames = new List<string> { "Steel section", "Concrete section", steelIProfile.GroupName };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<string> typeNames = new List<string> { matTypeTube.ToString(), matTypeConcrete.ToString(), steelIProfile.TypeName };

            string steelTubeSizeName = $"{d * 1000}-{t * 1000}";
            string concreteSizeName = $"{(d - 2 * t) * 1000}";
            List<string> sizeNames = new List<string> { steelTubeSizeName, concreteSizeName, steelIProfile.SizeName };

            var sections = SectionsFromContours(contours, materialTypes, groupNames, typeNames, sizeNames);

            // Return ordered sections. Order matters!
            return new List<Section>()
            {
                sections[2],    // steel I-profile
                sections[0],    // steel tube
                sections[1]     // concrete
            };
        }

        /// <summary>
        /// Create a FilledSteelTubeWithSteelCore type CompositeSection object.
        /// </summary>
        /// <param name="steelTube">Steel tube material.</param>
        /// <param name="steelCore">Steel core material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="d1">Steel tube exterior diameter [mm].</param>
        /// <param name="d2">Steel core diameter [mm].</param>
        /// <param name="t">Steel tube thickness [mm].</param>
        /// <returns></returns>
        public static CompositeSection FilledSteelTubeWithSteelCore(string name, Material steelTube, Material steelCore, Material concrete, double d1, double d2, double t)
        {
            CheckMaterialFamily(new List<Material> { steelTube, steelCore }, concrete);

            List<Material> materials = new List<Material>() { steelTube, steelCore, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = CreateFilledSteelTubeWithSteelCoreSection(d1, d2, t);

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name),
                new CompositeSectionParameter(CompositeSectionParameterType.d1, d1.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.d2, d2.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.t, t.ToString())
            };

            return new CompositeSection(CompositeSectionType.FilledSteelTubeWithSteelCore, materials, sections, parameters);
        }

        /// <summary>
        /// Create a parametric section for FilledSteelTubeWithSteelCore type CompositeSection object.
        /// </summary>
        /// <param name="d1">Steel tube exterior diameter [mm].</param>
        /// <param name="d2">Steel core diameter [mm].</param>
        /// <param name="t">Steel tube thickness [mm].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        internal static List<Sections.Section> CreateFilledSteelTubeWithSteelCoreSection(double d1, double d2, double t)
        {
            // round inputs
            d1 = Math.Round(d1, 2, MidpointRounding.AwayFromZero);
            d2 = Math.Round(d2, 2, MidpointRounding.AwayFromZero);
            t = Math.Round(t, 2, MidpointRounding.AwayFromZero);

            // check inputs
            if (d1 <= 0 || d2 <= 0 || t <= 0)
                throw new ArgumentException("Composite section parameters must be positive, non zero numbers!");
            if (d1 <= 2 * t)
                throw new ArgumentException($"Diameter of steel tube must be greater than 2t = {2 * t}!");
            if (d2 >= (d1 - 2 * t))
                throw new ArgumentException($"Diameter of steel core must be smaller than d1 - 2t = {d1 - 2 * t}!");

            // conversion of geometric parameters from millimeters to meters
            d1 /= 1000;
            d2 /= 1000;
            t /= 1000;

            var center = new Point3d(0, 0, 0);
            double tubeExtRadius = d1 / 2;
            double tubeIntRadius = (d1 - 2 * t) / 2;
            double coreRadius = d2 / 2;
            var plane = new Plane(center, Vector3d.UnitX, Vector3d.UnitY);

            var tubeExtEdge = new CircleEdge(tubeExtRadius, center, plane);
            var tubeIntEdge = new CircleEdge(tubeIntRadius, center, plane);
            var coreEdge = new CircleEdge(coreRadius, center, plane);

            // create contours
            var tubeExtContour = new Contour(new List<Edge>() { tubeExtEdge });
            var tubeIntContour = new Contour(new List<Edge>() { tubeIntEdge });
            var coreContour = new Contour(new List<Edge>() { coreEdge });
            List<Contour> contours = new List<Contour> { tubeExtContour, tubeIntContour, coreContour };

            // define section data
            List<MaterialTypeEnum> materialTypes = new List<MaterialTypeEnum> { MaterialTypeEnum.SteelWelded, MaterialTypeEnum.Concrete, MaterialTypeEnum.SteelRolled };
            List<string> groupNames = new List<string> { "Steel section", "Concrete section", "Steel section" };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<string> typeNames = materialTypes.Select(x => x.ToString()).ToList();

            string steelTubeSizeName = $"{d1 * 1000}-{t * 1000}";
            string concreteSizeName = $"{(d1 - 2 * t) * 1000}";
            string steelCoreSizeName = $"{(d2) * 1000}";
            List<string> sizeNames = new List<string> { steelTubeSizeName, concreteSizeName, steelCoreSizeName };

            var sections = SectionsFromContours(contours, materialTypes, groupNames, typeNames, sizeNames);
            sections.Reverse(1, 2);

            return sections;
        }

        /// <summary>
        /// Check Steel Section Compatibility.
        /// </summary>
        /// <param name="type">the type.</param>
        /// <param name="steelSection">the steel section.</param>
        public static void CheckSteelSectionCompatibility(CompositeSectionType type, Section steelSection)
        {
            // check section material
            if (steelSection.MaterialFamily != "Steel")
                throw new ArgumentException("Section group name must be Steel!");

            // check steel section TypeName
            string typeName = steelSection.TypeName;
            List<string> compatibleSectionTypes = GetCompatibleSteelSectionType(type).Select(f => f.ToString()).ToList();

            if (!compatibleSectionTypes.Any(c => c.Equals(typeName, StringComparison.OrdinalIgnoreCase) || c.Replace('_', '-').Equals(typeName, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"Invalid steel section type. Compatible section types for {type}: {string.Join(", ", compatibleSectionTypes)}.");
        }

        /// <summary>
        /// Gets the compatible steel section type.
        /// </summary>
        /// <param name="type">the type.</param>
        /// <returns>The result.</returns>
        public static List<Sections.Family> GetCompatibleSteelSectionType(CompositeSectionType type)
        {
            switch (type)
            {
                case CompositeSectionType.EffectiveCompositeSlab:
                    return new List<Sections.Family>()
                    {
                        FemDesign.Sections.Family.HE_A,
                        FemDesign.Sections.Family.HE_B,
                        FemDesign.Sections.Family.HE_M,
                        FemDesign.Sections.Family.I,
                        FemDesign.Sections.Family.IPE,
                        FemDesign.Sections.Family.UKB,
                        FemDesign.Sections.Family.UKC,
                        FemDesign.Sections.Family.KKR,
                        FemDesign.Sections.Family.VKR
                    };
                case CompositeSectionType.FilledDeltaBeamProfile:
                    return new List<Sections.Family>()
                    {
                        FemDesign.Sections.Family.D,
                        FemDesign.Sections.Family.DR
                    };
                case CompositeSectionType.FilledIProfile:
                    return new List<Sections.Family>()
                    {
                        FemDesign.Sections.Family.HE_A,
                        FemDesign.Sections.Family.HE_B,
                        FemDesign.Sections.Family.HE_M,
                        FemDesign.Sections.Family.I,
                        FemDesign.Sections.Family.IPE,
                        FemDesign.Sections.Family.UKB,
                        FemDesign.Sections.Family.UKC
                    };
                case CompositeSectionType.FilledRHSProfile:
                    return new List<Sections.Family>()
                    {
                        FemDesign.Sections.Family.KKR,
                        FemDesign.Sections.Family.VKR
                    };
                case CompositeSectionType.FilledSteelTubeWithIProfile:
                    return new List<Sections.Family>()
                    {
                        FemDesign.Sections.Family.HE_A,
                        FemDesign.Sections.Family.HE_B,
                        FemDesign.Sections.Family.HE_M,
                        FemDesign.Sections.Family.I,
                        FemDesign.Sections.Family.IPE,
                        FemDesign.Sections.Family.UKB,
                        FemDesign.Sections.Family.UKC
                    };
                default:
                    throw new ArgumentException("For this CompositeSectionType you cannot specify a steel section from database.");
            }
        }

        internal static void CheckMaterialFamily(List<Material> steelMaterials, Material concrete)
        {
            foreach(var steel in steelMaterials)
            {
                if(steel.Family != FemDesign.Materials.Family.Steel)
                    throw new ArgumentException($"Each steel material input parameter must be FemDesign.Materials.Family.Steel, but one of them is: {steel.Family}");
            }
            if (concrete.Family != FemDesign.Materials.Family.Concrete)
                throw new ArgumentException($"Concrete material input parameter must be FemDesign.Materials.Family.Concrete, but it is: {concrete.Family}");
        }

        internal void CheckCompositeSectionPartList(CompositeSectionType type, List<CompositeSectionPart> parts)
        {
            // Check list length. Occurance number is defined in the struxml schema
            int listLength = parts.Count;
            if (listLength < 2)
            {
                throw new ArgumentException($"Composite section must have at least 2 section parts, but it has {listLength}.");
            }
            else if (listLength > 8)
            {
                throw new ArgumentException($"Composite section may have up to 8 section parts, but it has {listLength}.");
            }

            // Check materials
            List<Materials.Family> materialTypes = parts.Select(p => p.Material.Family).ToList();
            int steelNum = materialTypes.Where(m => m == FemDesign.Materials.Family.Steel).Count();

            if ((!materialTypes.Contains(FemDesign.Materials.Family.Concrete)) || (!materialTypes.Contains(FemDesign.Materials.Family.Steel)))
                throw new ArgumentException("Check the material types! Composite section must contain at least one steel and one concrete section part.");
            if (type == CompositeSectionType.FilledSteelTubeWithIProfile || type == CompositeSectionType.FilledSteelTubeWithSteelCore)
            {
                if (steelNum != 2)
                    throw new ArgumentException($"{type} must have 2 steel composite section parts, but it has {steelNum}.");
            }
        }

        internal static List<Sections.Section> SectionsFromContours(List<Contour> contours, List<MaterialTypeEnum> materialTypes, List<string> groupNames, List<string> typeNames, List<string> sizeNames)
        {
            // check input lists
            if (contours.Count != materialTypes.Count || contours.Count != groupNames.Count || contours.Count != typeNames.Count || contours.Count != sizeNames.Count)
                throw new ArgumentException("Length of input lists must be the same!");

            List<Section> sections = new List<Section>();
            for(int i = 0; i < contours.Count; i++)
            {
                var region = new Region();
                if(i < contours.Count - 1)
                {
                    region = new Region(new List<Contour> { contours[i], contours[i + 1] });                    
                }
                else
                {
                    region = new Region(new List<Contour> { contours[i] });
                }

                var regionGroup = new RegionGroup(region);

                sections.Add(new Section(regionGroup, "custom", materialTypes[i], groupNames[i], typeNames[i], sizeNames[i]));
            }

            return sections;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            List<string> parameters = ParameterDictionary.Select(d => d.Key + " = " + d.Value).ToList();
            return $"{Type}, Materials: {string.Join(", ", this.Materials.Select(m => m.Name).ToList())}, Parameters: {string.Join(", ", parameters)}";
        }
    }
}