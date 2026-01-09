// https://strusoft.com/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using FemDesign.Geometry;

namespace FemDesign.Shells
{
    /// <summary>
    /// Represents a Slab.
    /// </summary>
    [System.Serializable]
    public partial class Slab : NamedEntityBase, INamedEntity, IStructureElement, IStageElement, IShell
    {
        [XmlIgnore]
        private static int _plateInstance = 0; // Number of plate instances created
        [XmlIgnore]
        private static int _wallInstance = 0; // Number of wall instances created
        protected override int GetUniqueInstanceCount() // This method body must be the same as the method of the SlabPart class
        {
            switch (this.Type)
            {
                case SlabType.Plate:
                    return ++_plateInstance;
                case SlabType.Wall:
                    return ++_wallInstance;
                default:
                    throw new ArgumentException($"Incorrect type of slab: {this.Type}");
            }
        }

        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        [XmlIgnore]
        public Materials.Material Material { get; set; }
        /// <summary>
        /// Gets or sets the surface reinforcement parameters.
        /// </summary>
        [XmlIgnore]
        public Reinforcement.SurfaceReinforcementParameters SurfaceReinforcementParameters { get; set; }
        /// <summary>
        /// Gets or sets the surface reinforcement.
        /// </summary>
        [XmlIgnore]
        public List<Reinforcement.SurfaceReinforcement> SurfaceReinforcement { get; set; } = new List<Reinforcement.SurfaceReinforcement>();

        /// <summary>
        /// Gets or sets the shear control regions.
        /// </summary>
        [XmlIgnore]
        public List<Reinforcement.ShearControlRegionType> ShearControlRegions { get; set; } = new List<Reinforcement.ShearControlRegionType>();

        /// <summary>
        /// Gets or sets the punching reinforcement.
        /// </summary>
        [XmlIgnore]
        public List<Reinforcement.PunchingReinforcement> PunchingReinforcement { get; set; } = new List<Reinforcement.PunchingReinforcement>();


        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute("type")]
        public SlabType Type { get; set; }

        /// <summary>
        /// Gets or sets the stage id.
        /// </summary>
        [XmlAttribute("stage")]
        public int _stageId = 1;

        [XmlIgnore]
        public int StageId
        {
            get { return _stageId; }
            set
            {
                _stageId = value;
                if(this.SlabPart != null)
                    this.SlabPart.StageId = value;
            }
        }

        /// <summary>
        /// Gets or sets the slab part.
        /// </summary>
        [XmlElement("slab_part", Order = 1)]
        public SlabPart _slabPart;

        [XmlIgnore]
        public SlabPart SlabPart
        {
            get
            {
                this._slabPart._name = this._name + ".1";   // SlabPart name must be updated with the Slab's name.
                return this._slabPart;
            }
            set
            {
                this._slabPart = value;
                this._name = this._slabPart._name.Remove(this._slabPart._name.Length - 2);  // The SlabPart class is public, so the user can create a new SlabPart obj. and assign it to this property. Slab name must be updated with the SlabPart's name.
            }
        }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        [XmlElement("end", Order = 2)]
        public string End { get; set; } // empty_type

        [XmlIgnore]
        public Region Region
        {
            get
            {
                return this.SlabPart.Region;
            }
        }

        [XmlIgnore]
        public bool IsVariableThickness
        {
            get
            {
                if (this.SlabPart.Thickness.Count == 3)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Slab()
        {

        }

        /// <summary>
        /// Construct Slab.
        /// </summary>
        /// <param name="type">the type.</param>
        /// <param name="identifier">the identifier.</param>
        /// <param name="slabPart">the slab part.</param>
        /// <param name="material">the material.</param>
        public Slab(SlabType type, string identifier, SlabPart slabPart, Materials.Material material)
        {
            this.EntityCreated();
            this.SlabPart = slabPart;
            this.Identifier = identifier;
            this.Type = type;
            this.Material = material;
            this.End = "";
        }

        /// <summary>
        /// Plate.
        /// </summary>
        /// <param name="identifier">the identifier.</param>
        /// <param name="material">the material.</param>
        /// <param name="region">the region.</param>
        /// <param name="shellEdgeConnection">the shell edge connection.</param>
        /// <param name="eccentricity">the eccentricity.</param>
        /// <param name="orthotropy">the orthotropy.</param>
        /// <param name="thickness">the thickness.</param>
        /// <returns>The result.</returns>
        public static Slab Plate(string identifier, Materials.Material material, Geometry.Region region, EdgeConnection shellEdgeConnection, ShellEccentricity eccentricity, ShellOrthotropy orthotropy, List<Thickness> thickness)
        {
            SlabType type = SlabType.Plate;
            SlabPart slabPart = SlabPart.Define(type, identifier, region, thickness, material, shellEdgeConnection, eccentricity, orthotropy);
            Slab shell = new Slab(type, identifier, slabPart, material);
            return shell;
        }

        /// <summary>
        /// Plate.
        /// </summary>
        /// <param name="identifier">the identifier.</param>
        /// <param name="material">the material.</param>
        /// <param name="region">the region.</param>
        /// <param name="shellEdgeConnection">the shell edge connection.</param>
        /// <param name="eccentricity">the eccentricity.</param>
        /// <param name="orthotropy">the orthotropy.</param>
        /// <param name="thickness">the thickness.</param>
        /// <returns>The result.</returns>
        public static Slab Plate(string identifier, Materials.Material material, Geometry.Region region, EdgeConnection shellEdgeConnection, ShellEccentricity eccentricity, ShellOrthotropy orthotropy, double thickness)
        {
            var thicknesses = new List<Thickness>();
            var corner = region.Contours[0].Edges[0].Points[0];
            thicknesses.Add(new Thickness(corner, thickness));
            var shell = Slab.Plate(identifier, material, region, shellEdgeConnection, eccentricity, orthotropy, thicknesses);
            return shell;
        }

        /// <summary>
        /// Construct a rectangular slab in the XY plane
        /// </summary>
        /// <param name="corner"></param>
        /// <param name="widthX"></param>
        /// <param name="widthY"></param>
        /// <param name="thickness"></param>
        /// <param name="material"></param>
        /// <param name="shellEdgeConnection"></param>
        /// <param name="eccentricity"></param>
        /// <param name="orthotropy"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static Slab Plate(Geometry.Point3d corner, double widthX, double widthY, double thickness, Materials.Material material, EdgeConnection shellEdgeConnection = null, ShellEccentricity eccentricity = null, ShellOrthotropy orthotropy = null, string identifier = "P")
        {
            SlabType type = SlabType.Plate;
            var region = Geometry.Region.RectangleXY(corner, widthX, widthY);

            List<FemDesign.Shells.Thickness> thicknessObj = new List<FemDesign.Shells.Thickness>();
            thicknessObj.Add(new FemDesign.Shells.Thickness(region.Plane.Origin, thickness));

            SlabPart slabPart = SlabPart.Define(type, identifier, region, thicknessObj, material, shellEdgeConnection, eccentricity, orthotropy);

            Slab shell = new Slab(type, identifier, slabPart, material);
            return shell;
        }

        /// <summary>
        /// Construct a vertical wall from two points
        /// </summary>
        /// <param name="point0"></param>
        /// <param name="point1"></param>
        /// <param name="height"></param>
        /// <param name="thickness"></param>
        /// <param name="material"></param>
        /// <param name="shellEdgeConnection"></param>
        /// <param name="eccentricity"></param>
        /// <param name="orthotropy"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static Slab Wall(Geometry.Point3d point0, Geometry.Point3d point1, double height, double thickness, Materials.Material material, EdgeConnection shellEdgeConnection = null, ShellEccentricity eccentricity = null, ShellOrthotropy orthotropy = null, string identifier = "W")
        {
            SlabType type = SlabType.Wall;

            var translation = new Geometry.Vector3d(0, 0, height);
            var point2 = point1 + translation;
            var point3 = point0 + translation;
            var points = new List<FemDesign.Geometry.Point3d>() { point0, point1, point2, point3 };

            var plane = new Geometry.Plane(point0, point1, point3);

            // set properties
            var region = new Geometry.Region(points, plane);

            List<FemDesign.Shells.Thickness> thicknessObj = new List<FemDesign.Shells.Thickness>();
            thicknessObj.Add(new FemDesign.Shells.Thickness(region.Plane.Origin, thickness));

            SlabPart slabPart = SlabPart.Define(type, identifier, region, thicknessObj, material, shellEdgeConnection, eccentricity, orthotropy);

            Slab shell = new Slab(type, identifier, slabPart, material);
            return shell;
        }

        /// <summary>
        /// Construct a slab from four points.
        /// </summary>
        /// <param name="point0"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        /// <param name="thickness"></param>
        /// <param name="material"></param>
        /// <param name="shellEdgeConnection"></param>
        /// <param name="eccentricity"></param>
        /// <param name="orthotropy"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static Slab FromFourPoints(Geometry.Point3d point0, Geometry.Point3d point1, Geometry.Point3d point2, Geometry.Point3d point3, double thickness, Materials.Material material, EdgeConnection shellEdgeConnection = null, ShellEccentricity eccentricity = null, ShellOrthotropy orthotropy = null, string identifier = "P")
        {
            SlabType type = SlabType.Plate;

            var points = new List<Geometry.Point3d>() { point0, point1, point2, point3 };
            var plane = new Geometry.Plane(point0, point1, point3);
            var region = new Geometry.Region(points, plane);

            List<FemDesign.Shells.Thickness> thicknessObj = new List<FemDesign.Shells.Thickness>();
            thicknessObj.Add(new FemDesign.Shells.Thickness(region.Plane.Origin, thickness));

            SlabPart slabPart = SlabPart.Define(type, identifier, region, thicknessObj, material, shellEdgeConnection, eccentricity, orthotropy);

            Slab shell = new Slab(type, identifier, slabPart, material);
            return shell;
        }


        /// <summary>
        /// Wall.
        /// </summary>
        /// <param name="identifier">the identifier.</param>
        /// <param name="material">the material.</param>
        /// <param name="region">the region.</param>
        /// <param name="shellEdgeConnection">the shell edge connection.</param>
        /// <param name="eccentricity">the eccentricity.</param>
        /// <param name="orthotropy">the orthotropy.</param>
        /// <param name="thickness">the thickness.</param>
        /// <returns>The result.</returns>
        public static Slab Wall(string identifier, Materials.Material material, Geometry.Region region, EdgeConnection shellEdgeConnection, ShellEccentricity eccentricity, ShellOrthotropy orthotropy, List<Thickness> thickness)
        {
            // check if surface is vertical
            if (Math.Abs(region.Plane.LocalZ.Z) > FemDesign.Tolerance.Point3d)
            {
                throw new System.ArgumentException("Wall is not vertical! Create plate instead.");
            }

            SlabType type = SlabType.Wall;
            SlabPart slabPart = SlabPart.Define(type, identifier, region, thickness, material, shellEdgeConnection, eccentricity, orthotropy);

            Slab shell = new Slab(type, identifier, slabPart, material);
            return shell;
        }

        /// <summary>
        /// Wall.
        /// </summary>
        /// <param name="identifier">the identifier.</param>
        /// <param name="material">the material.</param>
        /// <param name="region">the region.</param>
        /// <param name="shellEdgeConnection">the shell edge connection.</param>
        /// <param name="eccentricity">the eccentricity.</param>
        /// <param name="orthotropy">the orthotropy.</param>
        /// <param name="thickness">the thickness.</param>
        /// <returns>The result.</returns>
        public static Slab Wall(string identifier, Materials.Material material, Geometry.Region region, EdgeConnection shellEdgeConnection, ShellEccentricity eccentricity, ShellOrthotropy orthotropy, double thickness)
        {
            var thicknesses = new List<Thickness>();
            var corner = region.Contours[0].Edges[0].Points[0];
            thicknesses.Add(new Thickness(corner, thickness));

            var shell = Slab.Wall(identifier, material, region, shellEdgeConnection, eccentricity, orthotropy, thicknesses);
            return shell;
        }

        /// <summary>
        /// Set EdgeConnections by indices.
        /// </summary>
        /// <param name="slab">Slab.</param>
        /// <param name="shellEdgeConnection">EdgeConnection.</param>
        /// <param name="indices">Index. List of items. Use SlabDeconstruct to extract index for each respective edge.</param>
        /// <returns>The result.</returns>
        public static Slab SetEdgeConnection(Slab slab, EdgeConnection shellEdgeConnection, List<int> indices)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Slab slabClone = slab.DeepClone();

            foreach (int index in indices)
            {
                if (index < 0 & index >= slabClone.SlabPart.GetEdgeConnections().Count)
                    throw new System.ArgumentException("Index is out of bounds.");

                slabClone.SlabPart.Region.SetEdgeConnection(shellEdgeConnection, index);
            }

            return slabClone;
        }

        /// <summary>
        /// Set EdgeConnections by indices.
        /// </summary>
        /// <param name="slab">Slab.</param>
        /// <param name="shellEdgeConnection">EdgeConnection.</param>
        /// <param name="index">Index of edge to set.</param>
        /// <returns>The result.</returns>
        public static Slab SetEdgeConnection(Slab slab, EdgeConnection shellEdgeConnection, int index)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Slab slabClone = slab.DeepClone();

            if (index < 0 & index >= slabClone.SlabPart.GetEdgeConnections().Count)
                throw new System.ArgumentException("Index is out of bounds.");

            slabClone.SlabPart.Region.SetEdgeConnection(shellEdgeConnection, index);

            return slabClone;
        }

        /// <summary>
        /// Return the edge connections on a slab
        /// </summary>
        /// <returns></returns>
        public List<Shells.EdgeConnection> GetEdgeConnections()
        {
            var edgeConnection = this.SlabPart.GetEdgeConnections();
            return edgeConnection;
        }

        /// <summary>
        /// Return the edge connections on a slab by the their name. Empty list if name does not match.
        /// The method will return the edge connections if the input name is contains in the edge connection name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Shells.EdgeConnection> GetEdgeConnections(string name)
        {
            var _edgeConnection = this.SlabPart.GetEdgeConnections();

            var edgeConnection = _edgeConnection.Where( x => x != null && x.Name.Contains(name) ).ToList();
            return edgeConnection;
        }

        /// <summary>
        /// Return the edge connections on a slab by the their guid. Emptu list if guid does not match.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public Shells.EdgeConnection GetEdgeConnection(Guid guid)
        {
            var _edgeConnection = this.SlabPart.GetEdgeConnections();

            var edgeConnection = _edgeConnection.Where(x => x != null && x.Guid == guid).ToList().FirstOrDefault();
            return edgeConnection;
        }

        /// <summary>
        /// Set average mesh size to slab.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="slab">Slab.</param>
        /// <param name="avgMeshSize">Average mesh size.</param>
        /// <returns></returns>
        public static Slab AverageSurfaceElementSize(Slab slab, double avgMeshSize)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Slab slabClone = slab.DeepClone();

            //
            slabClone.SlabPart.MeshSize = avgMeshSize;

            // return
            return slabClone;
        }


        /// <summary>
        /// Updates the material.
        /// </summary>
        /// <param name="material">the material.</param>
        public void UpdateMaterial(Materials.Material material)
        {
            this.SlabPart.ComplexMaterial = material;
            this.SlabPart.ComplexMaterialGuid = material.Guid;
        }

        /// <summary>
        /// Updates the thickness.
        /// </summary>
        /// <param name="thickness">the thickness.</param>
        public void UpdateThickness(double thickness)
        {
            var thicknessObj = new List<FemDesign.Shells.Thickness>();
            thicknessObj.Add(new FemDesign.Shells.Thickness(this.SlabPart.Region.Contours.First().Edges.First().Points.First(), thickness));
            this.SlabPart.Thickness = thicknessObj;
        }

        /// <summary>
        /// Updates the thickness.
        /// </summary>
        /// <param name="points">the points.</param>
        /// <param name="thickness">the thickness.</param>
        public void UpdateThickness(List<Geometry.Point3d> points , List<double> thickness)
        {
            if(points.Count != 3) throw new System.ArgumentException("points must have exactly three items.");
            if(thickness.Count != 3) throw new System.ArgumentException("thickness must have exactly three items.");

            var thicknessObj = points.Zip(thickness, (p, t) => new FemDesign.Shells.Thickness(p, t)).ToList();
            this.SlabPart.Thickness = thicknessObj;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            var isVariable = IsVariableThickness == true ? "Variable" : "";
            if (this.IsVariableThickness)
            {
                return $"{this.Type}{isVariable} Material: {this.SlabPart.ComplexMaterial}, Thickness: ({this.SlabPart.Thickness[0].Value}, {this.SlabPart.Thickness[1].Value}, {this.SlabPart.Thickness[2].Value}) m, {this.SlabPart.ShellEccentricity}";
            }
            else
            {
                return $"{this.Type}{isVariable} Material: {this.SlabPart.ComplexMaterial}, Thickness: {this.SlabPart.Thickness[0].Value} m, {this.SlabPart.ShellEccentricity}";
            }
        }
    }
}