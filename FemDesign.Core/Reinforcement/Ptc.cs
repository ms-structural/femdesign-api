using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using FemDesign.Bars;

namespace FemDesign.Reinforcement
{
    /// <summary>
    /// Defines the Jacking Side enumeration.
    /// </summary>
    public enum JackingSide
    {
        [Parseable("start", "Start", "START")]
        [XmlEnum("start")]
        Start,
        [Parseable("end", "End", "END")]
        [XmlEnum("end")]
        End,
        [Parseable("start_then_end", "start then end", "Start then end", "StartThenEnd", "START_THEN_END")]
        [XmlEnum("start_then_end")]
        StartThenEnd,
        [Parseable("end_then_start", "end then start", "End then start", "EndThenStart", "END_THEN_START")]
        [XmlEnum("end_then_start")]
        EndThenStart,
    }

    /// <summary>
    /// Represents a Ptc Losses.
    /// </summary>
    [System.Serializable]
    public partial class PtcLosses
    {
        /// <summary>
        /// Gets or sets the curvature coefficient.
        /// </summary>
        [XmlAttribute("curvature_coefficient")]
        public double CurvatureCoefficient { get; set; }

        /// <summary>
        /// Gets or sets the wobble coefficient.
        /// </summary>
        [XmlAttribute("wobble_coefficient")]
        public double WobbleCoefficient { get; set; }

        /// <summary>
        /// Gets or sets the anchorage set slip.
        /// </summary>
        [XmlAttribute("anchorage_set_slip")]
        public double AnchorageSetSlip { get; set; }
        /// <summary>
        /// Gets or sets the elastic shortening.
        /// </summary>
        [XmlAttribute("elastic_shortening")]
        public double ElasticShortening { get; set; }

        /// <summary>
        /// Gets or sets the creep stress.
        /// </summary>
        [XmlAttribute("creep_stress")]
        public double CreepStress { get; set; }

        /// <summary>
        /// Gets or sets the shrinkage stress.
        /// </summary>
        [XmlAttribute("shrinkage_stress")]
        public double ShrinkageStress { get; set; }

        /// <summary>
        /// Gets or sets the relaxation stress.
        /// </summary>
        [XmlAttribute("relaxation_stress")]
        public double RelaxationStress { get; set; }

        /// <summary>
        /// Private constructor for serialization
        /// </summary>
        private PtcLosses()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PtcLosses"/> class.
        /// </summary>
        /// <param name="curvatureCoefficient">the curvature coefficient.</param>
        /// <param name="wobbleCoefficient">the wobble coefficient.</param>
        /// <param name="anchorageSetSlip">the anchorage set slip.</param>
        /// <param name="elasticShortening">the elastic shortening.</param>
        /// <param name="creepStress">the creep stress.</param>
        /// <param name="shrinkageStress">the shrinkage stress.</param>
        /// <param name="relaxationStress">the relaxation stress.</param>
        public PtcLosses(double curvatureCoefficient, double wobbleCoefficient, double anchorageSetSlip, double elasticShortening, double creepStress, double shrinkageStress, double relaxationStress)
        {
            CurvatureCoefficient = curvatureCoefficient;
            WobbleCoefficient = wobbleCoefficient;
            AnchorageSetSlip = anchorageSetSlip;
            ElasticShortening = elasticShortening;
            CreepStress = creepStress;
            ShrinkageStress = shrinkageStress;
            RelaxationStress = relaxationStress;
        }
    }

    /// <summary>
    /// Defines the shape of a post-tensioned cable (PTC) element.
    /// </summary>
    [System.Serializable]
    public partial class PtcShapeType
    {
        /// <summary>
        /// Gets or sets the start point.
        /// </summary>
        [XmlElement("start_point", Order = 1)]
        public PtcShapeStart StartPoint { get; set; }

        /// <summary>
        /// Gets or sets the intermediate point.
        /// </summary>
        [XmlElement("intermediate_point", Order = 2)]
        public PtcShapeInner[] IntermediatePoint { get; set; }

        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        [XmlElement("end_point", Order = 3)]
        public PtcShapeEnd EndPoint { get; set; }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        [XmlAttribute("top")]
        public double Top { get; set; }
        /// <summary>
        /// Gets or sets the bottom.
        /// </summary>
        [XmlAttribute("bottom")]
        public double Bottom { get; set; }

        /// <summary>
        /// Private constructor for serialization
        /// </summary>
        private PtcShapeType()
        {

        }

        /// <summary>
        /// Defines the shape of a Post Tensioned Cable.
        /// </summary>
        /// <param name="start">the start.</param>
        /// <param name="end">the end.</param>
        /// <param name="intermediates">the intermediates.</param>
        public PtcShapeType(PtcShapeStart start, PtcShapeEnd end, IEnumerable<PtcShapeInner> intermediates)
        {
            DenormalizePriorInflectionParameters(ref end, ref intermediates);

            StartPoint = start;
            EndPoint = end;
            IntermediatePoint = intermediates.ToArray();
        }

        /// <summary>
        /// Change normalization on PriorInflectionPosition from normalized along parent element to be normalized between ptc shape base points.
        /// </summary>
        /// <param name="end"></param>
        /// <param name="intermediates"></param>
        private void DenormalizePriorInflectionParameters(ref PtcShapeEnd end, ref IEnumerable<PtcShapeInner> intermediates)
        {
            var sortedInner = intermediates.OrderBy(i => i.Position).ToList();
            List<PtcShapeInner> newIntermediates = new List<PtcShapeInner>();

            double Denormalize(double t, double x1, double x2)
            {
                if (t < x1 || t > x2)
                    throw new ArgumentException($"PriorInflectionPosition must be a parameter between prior end next basepoint but got (position: {t}, prior: {x1}, next: {x2})");
                return (t - x1) / (x2 - x1);
            }

            // Normalize inner
            for (int i = 0; i < sortedInner.Count; i++)
            {
                var next = sortedInner[i];
                if (next.PriorInflectionPosition.HasValue)
                {
                    double x1 = 0.0;
                    if (i > 0)
                        x1 = sortedInner[i - 1].Position;
                    double x2 = next.Position;
                    double t = next.PriorInflectionPosition.Value;

                    newIntermediates.Add(new PtcShapeInner(next.Position, next.Z, next.Tangent, Denormalize(t, x1, x2)));
                }
                else
                {
                    newIntermediates.Add(next);
                }
            }
            intermediates = newIntermediates;

            // Normalize end
            if (sortedInner.Any() && end.PriorInflectionPosition.HasValue && end.NormalizedInflectionPosition)
            {
                double x1 = sortedInner.Last().Position;
                double x2 = 1.0;
                double t = end.PriorInflectionPosition.Value;

                end = new PtcShapeEnd(end.Z, end.Tangent, Denormalize(t, x1, x2)) { NormalizedInflectionPosition = false };
            }
        }
    }

    /// <summary>
    /// Represents a Ptc Shape Start.
    /// </summary>
    [System.Serializable]
    public partial class PtcShapeStart
    {
        /// <summary>
        /// Gets or sets the z.
        /// </summary>
        [XmlAttribute("z")]
        public double Z { get; set; }

        /// <summary>
        /// Gets or sets the tangent.
        /// </summary>
        [XmlAttribute("tangent")]
        public double Tangent { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private PtcShapeStart() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PtcShapeStart"/> class.
        /// </summary>
        /// <param name="z">value for <paramref name="z"/>.</param>
        /// <param name="tangent">the tangent.</param>
        public PtcShapeStart(double z = 0.0, double tangent = 0.0)
        {
            Z = z;
            Tangent = tangent;
        }
    }

    /// <summary>
    /// Represents a Ptc Shape Inner.
    /// </summary>
    [System.Serializable]
    public partial class PtcShapeInner
    {
        /// <summary>
        /// x'
        /// </summary>
        [XmlAttribute("pos")]
        public double Position { get; set; }
        /// <summary>
        /// z'
        /// </summary>
        [XmlAttribute("z")]
        public double Z { get; set; }
        /// <summary>
        /// Gets or sets the tangent.
        /// </summary>
        [XmlAttribute("tangent")]
        public double Tangent { get; set; }
        /// <summary>
        /// Gets or sets the prior inflection position.
        /// </summary>
        [XmlAttribute("prior_inflection_pos")]
        public string _priorInflectionPosition;
        [XmlIgnore]
        public double? PriorInflectionPosition
        {
            get
            {
                return _priorInflectionPosition == null ? (double?)null : (double?)double.Parse(_priorInflectionPosition, System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                _priorInflectionPosition = value.HasValue ? ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture) : null;
            }
        }

        /// <summary>
        /// Internal property to reflect whether or not the PriorInflectionPosition is been normalized along the parent element or not. <br/><br/>
        /// 
        /// true: <see cref="PriorInflectionPosition"/> is normalized (0-1) along parant element. <br/>
        /// false: <see cref="PriorInflectionPosition"/> is normalized (0-1) between prior and next basepoints. <br/><br/>
        /// 
        /// When serializing to struxml this shold have been set to false by <see cref="PtcShapeType.DenormalizePriorInflectionParameters(ref PtcShapeEnd, ref IEnumerable{PtcShapeInner})"/>.
        /// </summary>
        internal bool NormalizedInflectionPosition = true;

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private PtcShapeInner() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Parameter along axis of element.</param>
        /// <param name="z">Height</param>
        /// <param name="tangent">Tangent of cable at the inner point</param>
        /// <param name="priorInflectionPosition">Parameter of prior inflection point, if any. Optional</param>
        public PtcShapeInner(double x, double z = 0.0, double tangent = 0.0, double? priorInflectionPosition = null)
        {
            Position = x;
            Z = z;
            Tangent = tangent;
            PriorInflectionPosition = priorInflectionPosition;
        }
    }

    /// <summary>
    /// Represents a Ptc Shape End.
    /// </summary>
    [System.Serializable]
    public partial class PtcShapeEnd
    {
        /// <summary>
        /// Gets or sets the z.
        /// </summary>
        [XmlAttribute("z")]
        public double Z { get; set; }
        /// <summary>
        /// Gets or sets the tangent.
        /// </summary>
        [XmlAttribute("tangent")]
        public double Tangent { get; set; }
        /// <summary>
        /// Gets or sets the prior inflection position.
        /// </summary>
        [XmlAttribute("prior_inflection_pos")]
        public string _priorInflectionPosition;
        [XmlIgnore]
        public double? PriorInflectionPosition
        {
            get
            {
                return _priorInflectionPosition == null ? (double?)null : (double?)double.Parse(_priorInflectionPosition, System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                _priorInflectionPosition = value.HasValue ? ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture) : null;
            }
        }

        /// <summary>
        /// Internal property to reflect whether or not the PriorInflectionPosition is been normalized along the parent element or not. <br/><br/>
        /// 
        /// true: <see cref="PriorInflectionPosition"/> is normalized (0-1) along parant element. <br/>
        /// false: <see cref="PriorInflectionPosition"/> is normalized (0-1) between prior and next basepoints. <br/><br/>
        /// 
        /// When serializing to struxml this shold have been set to false by <see cref="PtcShapeType.DenormalizePriorInflectionParameters(ref PtcShapeEnd, ref IEnumerable{PtcShapeInner})"/>.
        /// </summary>
        internal bool NormalizedInflectionPosition = true;

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private PtcShapeEnd() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PtcShapeEnd"/> class.
        /// </summary>
        /// <param name="z">value for <paramref name="z"/>.</param>
        /// <param name="tangent">the tangent.</param>
        /// <param name="priorInflectionPosition">the prior inflection position.</param>
        public PtcShapeEnd(double z = 0.0, double tangent = 0.0, double? priorInflectionPosition = null)
        {
            Z = z;
            Tangent = tangent;
            PriorInflectionPosition = priorInflectionPosition;
        }
    }

    /// <summary>
    /// Represents a Ptc Manufacturing Type.
    /// </summary>
    [System.Serializable]
    public partial class PtcManufacturingType
    {
        private double[] _positions;

        [XmlElement("positions", Order = 1)]
        public string Positions
        {
            get
            {
                string str = "";

                for (int idx = 0; idx < this._positions.Length; idx++)
                {
                    if (idx == 0)
                    {

                    }
                    else
                    {
                        str = str + " ";
                    }

                    str = str + _positions[idx].ToString(System.Globalization.CultureInfo.InvariantCulture);
                }

                return str;
            }
            set
            {

                string[] vals = value.Split(' ');
                double[] dbls = new double[vals.Length];

                for (int idx = 0; idx < vals.Length; idx++)
                {
                    dbls[idx] = double.Parse(vals[idx], System.Globalization.CultureInfo.InvariantCulture);
                }

                this._positions = dbls;
            }
        }

        /// <summary>
        /// Gets or sets the shift x.
        /// </summary>
        [XmlAttribute("shift_x")]
        public double ShiftX { get; set; }

        /// <summary>
        /// Gets or sets the shift z.
        /// </summary>
        [XmlAttribute("shift_z")]
        public double ShiftZ { get; set; }

        /// <summary>
        /// Private constructor for serialization
        /// </summary>
        private PtcManufacturingType()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PtcManufacturingType"/> class.
        /// </summary>
        /// <param name="positions">the positions.</param>
        /// <param name="shiftX">the shift x.</param>
        /// <param name="shiftZ">the shift z.</param>
        public PtcManufacturingType(List<double> positions, double shiftX, double shiftZ)
        {
            _positions = positions.ToArray();
            ShiftX = shiftX;
            ShiftZ = shiftZ;
        }
    }

    /// <summary>
    /// Represents a Ptc.
    /// </summary>
    [System.Serializable]
    public partial class Ptc : EntityBase, IStructureElement
    {
        /// <summary>
        /// Gets or sets the start point.
        /// </summary>
        [XmlElement("start_point", Order = 1)]
        public Geometry.Point3d StartPoint { get; set; }

        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        [XmlElement("end_point", Order = 2)]
        public Geometry.Point3d EndPoint { get; set; }

        /// <summary>
        /// Gets or sets the local z.
        /// </summary>
        [XmlElement("local_z", Order = 3)]
        public Geometry.Vector3d LocalZ { get; set; }

        /// <summary>
        /// Gets or sets the losses.
        /// </summary>
        [XmlElement("losses", Order = 4)]
        public PtcLosses Losses { get; set; }

        /// <summary>
        /// Gets or sets the shape base points.
        /// </summary>
        [XmlElement("shape_base_points", Order = 5)]
        public PtcShapeType ShapeBasePoints { get; set; }

        /// <summary>
        /// Gets or sets the manufacturing.
        /// </summary>
        [XmlElement("manufacturing", Order = 6)]
        public PtcManufacturingType Manufacturing { get; set; }

        /// <summary>
        /// Gets or sets the base object.
        /// </summary>
        [XmlAttribute("base_object")]
        public Guid BaseObject { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the strand type guid.
        /// </summary>
        [XmlAttribute("strand_type")]
        public Guid StrandTypeGuid { get; set; }
        
        /// <summary>
        /// Gets or sets the strand type.
        /// </summary>
        [XmlIgnore]
        public PtcStrandLibType StrandType { get; set; }

        /// <summary>
        /// Gets or sets the number of strands.
        /// </summary>
        [XmlAttribute("number_of_strands")]
        public int NumberOfStrands { get; set; }

        /// <summary>
        /// Gets or sets the jacking side.
        /// </summary>
        [XmlAttribute("jacking_side")]
        public JackingSide JackingSide { get; set; }

        /// <summary>
        /// Gets or sets the jacking stress.
        /// </summary>
        [XmlAttribute("jacking_stress")]
        public double JackingStress { get; set; }

        private static int instances = 0;

        /// <summary>
        /// Private constructor for serialization
        /// </summary>
        private Ptc()
        {

        }

        /// <summary>
        /// Construct post-tension cable
        /// </summary>
        /// <param name="bar">Reference bar element</param>
        /// <param name="shape"></param>
        /// <param name="losses"></param>
        /// <param name="manufacturing"></param>
        /// <param name="strand"></param>
        /// <param name="jackingSide"></param>
        /// <param name="jackingStress"></param>
        /// <param name="numberOfStrands"></param>
        /// <param name="identifier"></param>
        public Ptc(Bars.Bar bar, PtcShapeType shape, PtcLosses losses, PtcManufacturingType manufacturing, PtcStrandLibType strand, JackingSide jackingSide, double jackingStress, int numberOfStrands = 3, string identifier = "PTC")
        {
            if (bar.BarPart.Edge.Type == "line")
            {
                var start = bar.BarPart.Edge.Points[0];
                var end = bar.BarPart.Edge.Points[1];
                Initialize(start, end, bar.BarPart.Guid, shape, losses, manufacturing, strand, jackingSide, jackingStress, numberOfStrands, identifier);
            }
            else
                throw new ArgumentException($"Bar must be of type line but got '{bar.BarPart.Edge.Type}'", "bar");
        }

        /// <summary>
        /// Construct post-tension cable
        /// </summary>
        /// <param name="slab">Reference slab element</param>
        /// <param name="line">Cable line</param>
        /// <param name="shape"></param>
        /// <param name="losses"></param>
        /// <param name="manufacturing"></param>
        /// <param name="strand"></param>
        /// <param name="jackingSide"></param>
        /// <param name="jackingStress"></param>
        /// <param name="numberOfStrands"></param>
        /// <param name="identifier"></param>
        public Ptc(Shells.Slab slab, Geometry.LineSegment line, PtcShapeType shape, PtcLosses losses, PtcManufacturingType manufacturing, PtcStrandLibType strand, JackingSide jackingSide, double jackingStress, int numberOfStrands = 3, string identifier = "PTC")
        {
            Initialize(line.StartPoint, line.EndPoint, slab.SlabPart.Guid, shape, losses, manufacturing, strand, jackingSide, jackingStress, numberOfStrands, identifier);
        }

        /// <summary>
        /// Construct post-tension cable
        /// </summary>
        /// <param name="line">Reference line element defining the cable geometry.</param>
        /// <param name="shape"></param>
        /// <param name="losses"></param>
        /// <param name="manufacturing"></param>
        /// <param name="strand"></param>
        /// <param name="jackingSide"></param>
        /// <param name="jackingStress"></param>
        /// <param name="numberOfStrands"></param>
        /// <param name="identifier"></param>
        public Ptc(Geometry.LineEdge line, PtcShapeType shape, PtcLosses losses, PtcManufacturingType manufacturing, PtcStrandLibType strand, JackingSide jackingSide, double jackingStress, int numberOfStrands = 3, string identifier = "PTC")
        {
            Initialize(line.StartPoint, line.EndPoint, Guid.Empty, shape, losses, manufacturing, strand, jackingSide, jackingStress, numberOfStrands, identifier);
        }

        private void Initialize(Geometry.Point3d start, Geometry.Point3d end, Guid baseObject, PtcShapeType shape, PtcLosses losses, PtcManufacturingType manufacturing, PtcStrandLibType strand, JackingSide jackingSide, double jackingStress, int numberOfStrands, string identifier)
        {
            StartPoint = start;
            EndPoint = end;

            BaseObject = baseObject;
            StrandType = strand;
            StrandTypeGuid = strand.Guid;
            Name = $"{identifier}.{++instances}";

            Losses = losses;
            NumberOfStrands = numberOfStrands;
            JackingSide = jackingSide;
            JackingStress = jackingStress;
            ShapeBasePoints = shape;
            Manufacturing = manufacturing;

            EntityCreated();
        }

        /// <summary>
        /// Add PTC to bar.
        /// Internal method use by GH components and Dynamo nodes.
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="ptc"></param>
        /// <param name="overwrite">Overwrite PTC on bar if a PTC sharing guid already exists on the bar?</param>
        /// <returns>The result.</returns>
        public static Bars.Bar AddPtcToBar(Bars.Bar bar, List<Ptc> ptc, bool overwrite)
        {
            // check if bar is curved and if it's not check if BarPart's and PTC line is fully overlapping
            if (!bar.BarPart.Edge.IsLine())
            {
                throw new System.ArgumentException($"Bar with guid: {bar.Guid} is not straight. PTC can only be added to straight bars.");
            }
            else
            {
                var barLine = new Geometry.Line3d(bar.BarPart.Edge.Points[0], bar.BarPart.Edge.Points[1]);
                for (int i = 0; i < ptc.Count; i++)
                {
                    var ptcLine = new Geometry.Line3d(ptc[i].StartPoint, ptc[i].EndPoint);
                    if (!ptcLine.IsLineFullyOverlapping(barLine, Tolerance.Point3d))
                    {
                        throw new System.ArgumentException("Bar's line must overlap the PTC's line.");
                    }
                }
            }

            // check if bar is a truss
            if (bar.Type == Bars.BarType.Truss)
            {
                throw new System.ArgumentException($"Bar with guid: {bar.Guid} is a Truss. PTC can only be added to beams or columns.");
            }

            // check if bar material is concrete
            if (bar.BarPart.ComplexMaterialObj.Concrete == null)
            {
                throw new System.ArgumentException("Material of bar must be concrete");
            }

            foreach (Ptc item in ptc)
            {
                // empty base object - update with current barPart guid
                if (item.BaseObject == Guid.Empty)
                {
                    item.BaseObject = bar.BarPart.Guid;
                }

                // base object equals current barPart guid 
                else if (item.BaseObject == bar.BarPart.Guid)
                {
                    // pass
                }

                // base object does not equal current barPart guid - PTC probably added to another bar already.
                else if (item.BaseObject != bar.BarPart.Guid)
                {
                    throw new System.ArgumentException($"{item.GetType().FullName} with guid: {item.Guid} has a base object guid: {item.BaseObject} that does not correnspond with the current bar");
                }

                // add PTC to current bar
                bool exists = bar.Ptc.Any(x => x.Guid == item.Guid);
                if (exists)
                {
                    if (overwrite)
                    {
                        bar.Ptc.RemoveAll(x => x.Guid == item.Guid);
                        bar.Ptc.Add(item);
                    }
                    else
                    {
                        throw new System.ArgumentException($"{item.GetType().FullName} with guid: {item.Guid} has already been added to the bar. Are you adding the same element twice?");
                    }
                }
                else
                {
                    bar.Ptc.Add(item);
                }
            }
            return bar;
        }
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{this.GetType().FullName} - {this.Name}; Strand: {this.StrandType.Name}; Jacking side: {this.JackingSide}; Jacking stress: {this.JackingStress}; Number of strands: {this.NumberOfStrands}";
        }

    }

    /// <summary>
    /// Represents a Ptc Strand Type.
    /// </summary>
    [System.Serializable]
    public partial class PtcStrandType
    {
        /// <summary>
        /// Gets or sets the ptc strand lib types.
        /// </summary>
        [XmlElement("predefined_type")]
        public List<PtcStrandLibType> PtcStrandLibTypes { get; set; } = new List<PtcStrandLibType>();
    }

    /// <summary>
    /// Represents a Ptc Strand Lib Type.
    /// </summary>
    [System.Serializable]
    public partial class PtcStrandLibType : LibraryBase
    {
        /// <summary>
        /// Gets or sets the ptc strand data.
        /// </summary>
        [XmlElement("ptc_strand_data", Order = 1)]
        public PtcStrandData PtcStrandData { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private PtcStrandLibType()
        {

        }

        /// <summary>
        /// Create a custom PTC-strand.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="f_pk">f pk [N/mm2]</param>
        /// <param name="a_p">A p [mm2]</param>
        /// <param name="e_p">E p [N/mm2]</param>
        /// <param name="rho">Rho [t/mm3]</param>
        /// <param name="relaxationClass">Relaxation class [1, 2, 3] </param>
        /// <param name="rho_1000">Rho 1000 [%]</param>
        public PtcStrandLibType(string name, double f_pk, double a_p, double e_p, double rho, int relaxationClass, double rho_1000)
        {
            Name = name;
            PtcStrandData = new PtcStrandData(f_pk, a_p, e_p, rho, relaxationClass, rho_1000);
            EntityCreated();
        }
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{this.GetType().FullName} - Name: {this.Name}";
        }
    }

    /// <summary>
    /// Strand material/type data
    /// </summary>
    [System.Serializable]
    public partial class PtcStrandData : EntityBase
    {
        /// <summary>
        /// f pk
        /// </summary>
        [XmlAttribute("f_pk")]
        public double F_pk { get; set; }

        /// <summary>
        /// Gets or sets the a p.
        /// </summary>
        [XmlAttribute("A_p")]
        public double A_p { get; set; }

        /// <summary>
        /// Gets or sets the e p.
        /// </summary>
        [XmlAttribute("E_p")]
        public double E_p { get; set; }

        /// <summary>
        /// Gets or sets the density.
        /// </summary>
        [XmlAttribute("density")]
        public double Density { get; set; }

        /// <summary>
        /// Gets or sets the relaxation class.
        /// </summary>
        [XmlAttribute("relaxation_class")]
        public int RelaxationClass { get; set; }

        /// <summary>
        /// Gets or sets the rho 1000.
        /// </summary>
        [XmlAttribute("Rho_1000")]
        public double Rho_1000 { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private PtcStrandData()
        {

        }

        /// <summary>
        /// Create a custom PTC-strand.
        /// </summary>
        /// <param name="f_pk">f pk [N/mm2]</param>
        /// <param name="a_p">A p [mm2]</param>
        /// <param name="e_p">E p [N/mm2]</param>
        /// <param name="density">Rho [t/mm3]</param>
        /// <param name="relaxationClass">Relaxation class [1, 2, 3] </param>
        /// <param name="rho_1000">Rho 1000 [%]</param>
        public PtcStrandData(double f_pk, double a_p, double e_p, double density, int relaxationClass, double rho_1000)
        {
            F_pk = f_pk;
            A_p = a_p;
            E_p = e_p;
            Density = density;
            RelaxationClass = relaxationClass;
            Rho_1000 = rho_1000;
        }
    }
}