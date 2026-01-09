// https://strusoft.com/

using FemDesign.GenericClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace FemDesign.Shells
{
    /// <summary>
    /// Represents a Slab Part.
    /// </summary>
    [System.Serializable]
    public partial class SlabPart: NamedEntityPartBase, IStageElement
    {
        private static int _plateInstance = 0;
        private static int _wallInstance = 0;
        protected override int GetUniqueInstanceCount()
        {
            switch (this.SlabType)
            {
                case SlabType.Plate:
                    return ++_plateInstance;
                case SlabType.Wall:
                    return ++_wallInstance;
                default:
                    throw new ArgumentException($"Incorrect type of slab: {this.SlabType}");
            }
        }

        /// <summary>
        /// Gets or sets the slab type.
        /// </summary>
        [XmlIgnore]
        public SlabType SlabType;

        /// <summary>
        /// Get ShellEccentricity
        /// </summary>
        [XmlIgnore]
        public ShellEccentricity ShellEccentricity
        {
            get
            {
               return new ShellEccentricity(this.Alignment, this.AlignOffset, this.EccentricityCalculation, this.EccentricityByCracking); 
            }
        }

        /// <summary>
        /// Get ShellOrthotropy
        /// </summary>
        [XmlIgnore]
        public ShellOrthotropy ShellOrthotropy
        {
            get
            {
                return new ShellOrthotropy(this.OrthoAlfa, this.OrthoRatio);
            }
        }

        /// <summary>
        /// Gets or sets the complex material guid.
        /// </summary>
        [XmlAttribute("complex_material")]
        public System.Guid ComplexMaterialGuid {get; set;} // guidtype

        /// <summary>
        /// Gets or sets the complex material.
        /// </summary>
        [XmlIgnore]
        public Materials.Material ComplexMaterial { get; set; } // guidtype

        /// <summary>
        /// Gets or sets the alignment.
        /// </summary>
        [XmlAttribute("alignment")]
        public GenericClasses.VerticalAlignment Alignment { get; set; }
        /// <summary>
        /// Gets or sets the align offset.
        /// </summary>
        [XmlAttribute("align_offset")]
        public double _alignOffset; // abs_max_1e20
        [XmlIgnore]
        public double AlignOffset
        {
            get {return this._alignOffset;}
            set {this._alignOffset = RestrictedDouble.AbsMax_1e20(value);}
        }
        /// <summary>
        /// Gets or sets the ortho alfa.
        /// </summary>
        [XmlAttribute("ortho_alfa")]
        public double _orthoAlfa; // two_quadrants
        [XmlIgnore]
        public double OrthoAlfa
        {
            get {return this._orthoAlfa;}
            set {this._orthoAlfa = RestrictedDouble.TwoQuadrantsRadians(value);}
        }
        /// <summary>
        /// Gets or sets the ortho ratio.
        /// </summary>
        [XmlAttribute("ortho_ratio")]
        public double _orthoRatio; // orthotropy_type
        [XmlIgnore]
        public double OrthoRatio
        {
            get {return this._orthoRatio;}
            set {this._orthoRatio = RestrictedDouble.NonNegMax_1(value);}
        }
        /// <summary>
        /// Gets or sets the eccentricity calculation.
        /// </summary>
        [XmlAttribute("ecc_calc")]
        public bool EccentricityCalculation { get; set; } // bool
        /// <summary>
        /// Gets or sets the eccentricity by cracking.
        /// </summary>
        [XmlAttribute("ecc_crack")]
        public bool EccentricityByCracking { get; set; } // bool
        /// <summary>
        /// Gets or sets the mesh size.
        /// </summary>
        [XmlAttribute("mesh_size")]
        public double _meshSize; // non_neg_max_1e20
        [XmlIgnore]
        public double MeshSize
        {
            get {return this._meshSize;}
            set {this._meshSize = RestrictedDouble.NonNegMax_1e20(value);}
        }
        /// <summary>
        /// Gets or sets the stage id.
        /// </summary>
        [XmlAttribute("stage")]
        public int StageId { get; set; } = 1;
        //[XmlElement("contour", Order = 1)]
        //public List<Geometry.Contour> _contours;
        //[XmlIgnore]
        //public Geometry.Region _region
        //{
        //    get
        //    {

        //    }
        //    set
        //    {

        //    }
        //}
        //[XmlIgnore]
        //public Geometry.Region Region
        //{
        //    get 
        //    { 
        //        if(_region == null)
        //            return 
        //        return this._region; 
        //    }
        //    set 
        //    {
        //        this._contours = value.Contours;
        //        this._region = value; 
        //    }
        //}
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        [XmlElement("contour", Order = 1)]
        public List<Geometry.Contour> _region;
        [XmlIgnore]
        public Geometry.Region Region
        {
            get 
            { 
                return new Geometry.Region(this._region, this.Plane); 
            }
            set 
            { 
                this._region = value.Contours;
            }
        }
        [XmlIgnore]
        public Geometry.Plane Plane
        {
            get => new Geometry.Plane(this.LocalPos, this.LocalX, this.LocalY);
        }
        /// <summary>
        /// Gets or sets the thickness.
        /// </summary>
        [XmlElement("thickness", Order = 2)]
        public List<Thickness> _thickness; // sequence: location_value
        [XmlIgnore]
        public List<Thickness> Thickness
        {
            get { return this._thickness; }
            set
            {
                if ( value.Count == 1 || value.Count == 2 || value.Count == 3)
                {
                    this._thickness = value;
                }
                else
                {
                    throw new System.ArgumentException("List of thickness objects must contain either 1, 2 or 3 elements");
                }
            }
        }
        /// <summary>
        /// Gets or sets the local pos.
        /// </summary>
        [XmlElement("local_pos", Order = 3)]
        public Geometry.Point3d LocalPos {get; set;} // point_type_3d
        /// <summary>
        /// Gets or sets the local x.
        /// </summary>
        [XmlElement("local_x", Order = 4)]
        public Geometry.Vector3d _localX; // point_type_3d
        [XmlIgnore]
        public Geometry.Vector3d LocalX
        {
            get
            {
                return this._localX;
            }
            set
            {
                Geometry.Vector3d val = value.Normalize();
                Geometry.Vector3d z = this.LocalZ;

                double dot = z.Dot(val);
                if (Math.Abs(dot) < Tolerance.DotProduct)
                {
                    this._localX = val;
                    this._localY = z.Cross(val); // follows right-hand-rule
                }
                
                else
                {
                    throw new System.ArgumentException($"X-axis is not perpendicular to Z-axis. The dot-product is {dot}, but should be 0");
                }
            }
        }
        /// <summary>
        /// Gets or sets the local y.
        /// </summary>
        [XmlElement("local_y", Order = 5)]
        public Geometry.Vector3d _localY; // point_type_3d
        [XmlIgnore]
        public Geometry.Vector3d LocalY
        {
            get
            {
                return this._localY;
            }
        }
        [XmlIgnore]
        private Geometry.Vector3d _localZ;
        [XmlIgnore]
        public Geometry.Vector3d LocalZ
        {
            get
            {
                if (this.LocalX == null || this.LocalY == null)
                {
                    throw new System.ArgumentException("Impossible to construct z-axis as either this.localX or this.localY is null.");
                }
                else
                {     
                    return this.LocalX.Cross(LocalY).Normalize();
                }
            }
            set
            {
                Geometry.Vector3d val = value.Normalize();
                Geometry.Vector3d x = this.LocalX;

                double dot = x.Dot(val);
                if (Math.Abs(dot) < Tolerance.DotProduct)
                {
                    this._localZ = val;
                    this._localY = val.Cross(x); // follows right-hand-rule
                }
                
                else
                {
                    throw new System.ArgumentException($"Z-axis is not perpendicular to X-axis. The dot-product is {dot}, but should be 0");
                }
            }
        }

        /// <summary>
        /// Gets or sets the stiffness modifiers.
        /// </summary>
        [XmlElement("stiffness_modifiers", Order = 6)]
        public List<SlabStiffnessFactors> _stiffnessModifiers;
        [XmlIgnore]
        public List<SlabStiffnessFactors> StiffnessModifiers
        {
            get { return this._stiffnessModifiers; }
            set
            {
                if (value.Count == 1)
                {
                    this._stiffnessModifiers = value;
                }
                else
                {
                    throw new System.ArgumentException("List of Slab Stiffness Factors objects must contain either 1");
                }
            }
        }

        /// <summary>
        /// Gets or sets the colouring.
        /// </summary>
        [XmlElement("colouring", Order = 7)]
        public EntityColor Colouring { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        [XmlElement("end", Order = 8)]
        public string End = ""; // empty_type

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private SlabPart()
        {
            
        }

        /// <summary>
        /// Construct SlabPart.
        /// </summary>
        /// <param name="type">the type.</param>
        /// <param name="identifier">the identifier.</param>
        /// <param name="region">the region.</param>
        /// <param name="thickness">the thickness.</param>
        /// <param name="complexMaterial">the complex material.</param>
        /// <param name="alignment">the alignment.</param>
        /// <param name="orthotropy">the orthotropy.</param>
        public SlabPart(SlabType type, string identifier, Geometry.Region region, List<Thickness> thickness, Materials.Material complexMaterial, ShellEccentricity alignment, ShellOrthotropy orthotropy)
        {
            this.EntityCreated();
            this.SlabType = type;
            this.Identifier = identifier;
            this.Region = region;
            this.ComplexMaterialGuid = complexMaterial.Guid;
            this.ComplexMaterial = complexMaterial;
            this.Alignment = alignment.Alignment;
            this.AlignOffset = alignment.Eccentricity;
            this.OrthoAlfa = orthotropy.OrthoAlfa;
            this.OrthoRatio = orthotropy.OrthoRatio;
            this.EccentricityCalculation = alignment.EccentricityCalculation;
            this.EccentricityByCracking = alignment.EccentricityByCracking;
            this.Thickness = thickness;
            this.LocalPos = region.Plane.Origin;
            this._localX = region.Plane.LocalX;
            this._localY = region.Plane.LocalY;
            this.End = "";
        }

        /// <summary>
        /// Construct SlabPart with EdgeConnections.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="type">the type.</param>
        /// <param name="identifier">the identifier.</param>
        /// <param name="region">the region.</param>
        /// <param name="thickness">the thickness.</param>
        /// <param name="material">the material.</param>
        /// <param name="shellEdgeConnection">the shell edge connection.</param>
        /// <param name="eccentricity">the eccentricity.</param>
        /// <param name="orthotropy">the orthotropy.</param>
        public static SlabPart Define(SlabType type, string identifier, Geometry.Region region, List<Thickness> thickness, Materials.Material material, EdgeConnection shellEdgeConnection = null, ShellEccentricity eccentricity = null, ShellOrthotropy orthotropy = null)
        {
            shellEdgeConnection = shellEdgeConnection ?? EdgeConnection.Default;
            eccentricity = eccentricity ?? ShellEccentricity.Default;
            orthotropy = orthotropy ?? ShellOrthotropy.Default;

            // add edgeConnections to region
            region.SetEdgeConnections(shellEdgeConnection);
            
            // construct new slabPart
            SlabPart slabPart = new SlabPart(type, identifier, region, thickness, material, eccentricity, orthotropy);

            // return
            return slabPart;
        }

        /// <summary>
        /// Get EdgeConnections. Used for Deconstruct methods and when redefining EdgeConnections on existing slab. 
        /// </summary>
        /// <returns>The result.</returns>
        public List<Shells.EdgeConnection> GetEdgeConnections()
        {
            return this.Region.GetEdgeConnections();
        }


    }
}