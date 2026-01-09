using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign.ModellingTools
{
    /// <summary>
    /// Represents a Fictitious Shell.
    /// </summary>
    [System.Serializable]
    public partial class FictitiousShell: NamedEntityBase, IStructureElement
    {
        
        [XmlIgnore]
        private static int _ficticiousShellInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_ficticiousShellInstances;

        [XmlIgnore]
        private Geometry.Plane _plane;

        [XmlIgnore]
        private Geometry.Plane Plane
        {
            get
            {
                if (this._plane == null)
                {
                    this._plane = new Geometry.Plane(this.LocalOrigin, this.LocalX, this.LocalY);
                    return this._plane;
                }
                else
                {
                    return this._plane;
                }
            }
            set
            {
                this._plane = value;
                this._localOrigin = value.Origin;
                this._localX = value.LocalX;
                this._localY = value.LocalY;
            }
        }
        
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        [XmlElement("region", Order=1)]
        public Geometry.Region _region;
        [XmlIgnore]
        public Geometry.Region Region
        {
            get
            {
                return this._region;
            }
            set
            {
                this._region = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the local origin.
        /// </summary>
        [XmlElement("local_pos", Order=2)]
        public Geometry.Point3d _localOrigin;
        [XmlIgnore]
        public Geometry.Point3d LocalOrigin
        {
            get
            {
                return this._localOrigin;
            }
            set
            {
                this.Plane.Origin = value;
                this._localOrigin = value;
            }
        }

        /// <summary>
        /// Gets or sets the local x.
        /// </summary>
        [XmlElement("local_x", Order=3)]
        public Geometry.Vector3d _localX;
        [XmlIgnore]
        public Geometry.Vector3d LocalX
        {
            get
            {
                return this._localX;
            }
            set
            {
                this.Plane.SetXAroundZ(value);
                this._localX = this.Plane.LocalX;
                this._localY = this.Plane.LocalY;
            }
        }

        /// <summary>
        /// Gets or sets the local y.
        /// </summary>
        [XmlElement("local_y", Order=4)]
        public Geometry.Vector3d _localY;
        [XmlIgnore]
        public Geometry.Vector3d LocalY
        {
            get
            {
                return this._localY;
            }
            set
            {
                this.Plane.SetYAroundZ(value);
                this._localX = this.Plane.LocalX;
                this._localY = this.Plane.LocalY;
            }
        }

        [XmlIgnore]
        public Geometry.Vector3d LocalZ
        {
            get
            {
                return this.Plane.LocalZ;
            }
            set
            {
                this.Plane.SetZAroundX(value);
                this._localX = this.Plane.LocalX;
                this._localY = this.Plane.LocalY;
            }
        }

        /// <summary>
        /// Gets or sets the membrane stiffness.
        /// </summary>
        [XmlElement("membrane_stiffness", Order=5)]
        public StiffnessMatrix4Type MembraneStiffness { get; set; }

        /// <summary>
        /// Gets or sets the flexural stiffness.
        /// </summary>
        [XmlElement("flexural_stiffness", Order=6)]
        public StiffnessMatrix4Type FlexuralStiffness { get; set; }

        /// <summary>
        /// Gets or sets the shear stiffness.
        /// </summary>
        [XmlElement("shear_stiffness", Order=7)]
        public StiffnessMatrix2Type ShearStiffness { get; set; }

        /// <summary>
        /// Gets or sets the colouring.
        /// </summary>
        [XmlElement("colouring", Order = 8)]
        public EntityColor Colouring { get; set; }

        /// <summary>
        /// Density in t/m2
        /// </summary>
        [XmlAttribute("density")]
        public double _density;
        [XmlIgnore]
        public double Density
        {
            get
            {
                return this._density;
            }
            set
            {
                this._density = RestrictedDouble.NonNegMax_1e20(value);
            }
        }

        /// <summary>
        /// t1 in m
        /// </summary>
        [XmlAttribute("t1")]
        public double _t1;
        [XmlIgnore]
        public double T1
        {
            get
            {
                return this._t1;
            }
            set
            {
                this._t1 = RestrictedDouble.NonZeroMax_1e20(value);
            }
        }

        /// <summary>
        /// t2 in m
        /// </summary>
        [XmlAttribute("t2")]
        public double _t2;
        [XmlIgnore]
        public double T2
        {
            get
            {
                return this._t2;
            }
            set
            {
                this._t2 = RestrictedDouble.NonZeroMax_1e20(value);
            }
        }

        /// <summary>
        /// alfa_1 in 1/°C
        /// </summary>
        [XmlAttribute("alfa_1")]
        public double _alfa1;
        [XmlIgnore]
        public double Alpha1
        {
            get
            {
                return this._alfa1;
            }
            set
            {
                this._alfa1 = RestrictedDouble.NonNegMax_1e20(value);
            }
        }

        /// <summary>
        /// alfa_2 in 1/°C
        /// </summary>
        [XmlAttribute("alfa_2")]
        public double _alfa2;
        [XmlIgnore]
        public double Alpha2
        {
            get
            {
                return this._alfa2;
            }
            set
            {
                this._alfa2 = RestrictedDouble.NonNegMax_1e20(value);
            }
        }

        /// <summary>
        /// Avg. mesh size in m
        /// </summary>
        [XmlAttribute("mesh_size")]
        public double _meshSize;
        [XmlIgnore]
        public double MeshSize
        {
            get
            {
                return this._meshSize;
            }
            set
            {
                this._meshSize = RestrictedDouble.NonNegMax_1e20(value);
            }
        }

        /// <summary>
        /// Ignore stiffness in stability and imperfection calculation
        /// </summary>
        /// <value></value>
        [XmlAttribute("ignore_in_st_imp_calculation")]
        public bool IgnoreInStImpCalculation { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private FictitiousShell()
        {

        }

        /// <summary>
        /// Construct virtual shell
        /// </summary>
        /// <param name="region"></param>
        /// <param name="membraneStiffness"></param>
        /// <param name="flexuralStiffness"></param>
        /// <param name="shearStiffness"></param>
        /// <param name="density"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="alpha1"></param>
        /// <param name="alpha2"></param>
        /// <param name="ignoreInStImpCalc"></param>
        /// <param name="identifier"></param>
        /// <param name="meshSize"></param>
        public FictitiousShell(Geometry.Region region, StiffnessMatrix4Type membraneStiffness, StiffnessMatrix4Type flexuralStiffness, StiffnessMatrix2Type shearStiffness, double density, double t1, double t2, double alpha1, double alpha2, bool ignoreInStImpCalc, double meshSize, string identifier)
        {
            this.EntityCreated();
            this.Region = region;
            this.Plane = region.Plane;
            this.MembraneStiffness = membraneStiffness;
            this.FlexuralStiffness = flexuralStiffness;
            this.ShearStiffness = shearStiffness;
            this.Density = density;
            this.T1 = t1;
            this.T2 = t2;
            this.Alpha1 = alpha1;
            this.Alpha2 = alpha2;
            this.IgnoreInStImpCalculation = ignoreInStImpCalc;
            this.MeshSize = meshSize;
            this.Identifier = identifier;
        }

        /// <summary>
        /// Set EdgeConnection by indices.
        /// </summary>
        /// <param name="fictShell">Fictitious Shell</param>
        /// <param name="edgeConnection">EdgeConnection</param>
        /// <param name="indices">Index. List of items. Deconstruct fictitious shell to extract index for each respective edge.</param>
        /// <returns></returns>
        public static FictitiousShell UpdateEdgeConnection(FictitiousShell fictShell, Shells.EdgeConnection edgeConnection, List<int> indices)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            FictitiousShell clone = fictShell.DeepClone();

            foreach (int index in indices)
            {
                if (index >= 0 & index < fictShell.Region.GetEdgeConnections().Count)
                {
                    // pass
                }
                else
                {
                    throw new System.ArgumentException("Index is out of bounds.");
                }
                
                //
                clone.Region.SetEdgeConnection(edgeConnection, index);  
            }

            //
            return clone;
        }

        /// <summary>
        /// Set EdgeConnections by indices.
        /// </summary>
        /// <param name="fictShell">FictitiousShell.</param>
        /// <param name="edgeConnection">EdgeConnection.</param>
        /// <param name="index">Index of edge to set.</param>
        /// <returns></returns>
        public static FictitiousShell UpdateEdgeConnection(FictitiousShell fictShell, Shells.EdgeConnection edgeConnection, int index)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            FictitiousShell fictShellClone = fictShell.DeepClone();

            if (index < 0 & index >= fictShellClone.Region.GetEdgeConnections().Count)
                throw new System.ArgumentException("Index is out of bounds.");

            fictShellClone.Region.SetEdgeConnection(edgeConnection, index);

            return fictShellClone;
        }

    }
}