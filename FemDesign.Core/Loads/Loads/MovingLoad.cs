using FemDesign.Bars.Buckling;
using FemDesign.Geometry;
using FemDesign.LibraryItems;
using FemDesign.Sections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using StruSoft.Interop.StruXml.Data;
using FemDesign.GenericClasses;
using Newtonsoft.Json.Serialization;
using FemDesign.Materials;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Moving Load.
    /// </summary>
    [System.Serializable]
    public partial class MovingLoad : EntityBase, ILoadElement
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the vehicle guid.
        /// </summary>
        [XmlAttribute("vehicle")]
        public Guid _vehicleGuid { get; set; }

        /// <summary>
        /// Gets or sets the vehicle.
        /// </summary>
        [XmlIgnore]
        public StruSoft.Interop.StruXml.Data.Vehicle_lib_type Vehicle { get; set; }

        /// <summary>
        /// Gets or sets the vehicle shift x.
        /// </summary>
        [DefaultValue(0.0)]
        [XmlAttribute("vehicle_shift_x")]
        public double _vehicleShiftX = 0.0;
        [XmlIgnore]
        public double VehicleShiftX
        {
            get
            {
                return _vehicleShiftX;
            }
            set
            {
                _vehicleShiftX = RestrictedDouble.ValueInClosedInterval(value, -1000, 1000);
            }
        }

        /// <summary>
        /// Gets or sets the vehicle shift y.
        /// </summary>
        [DefaultValue(0.0)]
        [XmlAttribute("vehicle_shift_y")]
        public double _vehicleShiftY = 0.0;
        [XmlIgnore]
        public double VehicleShiftY
        {
            get
            {
                return _vehicleShiftY;
            }
            set
            {
                _vehicleShiftY = RestrictedDouble.ValueInClosedInterval(value, -1000, 1000);
            }
        }

        /// <summary>
        /// Gets or sets the division point.
        /// </summary>
        [XmlElement("division_points")]
        public int _divisionPoint;

        [XmlIgnore]
        public int DivisionPoint
        {
            get { return _divisionPoint; }
            set { _divisionPoint = RestrictedInteger.ValueInRange(value, 1, 1000); }
        }

        [XmlElement("division_distance")]
        private double _divisionDistance;

        [XmlIgnore]
        public double DivisionDistance
        {
            get { return _divisionDistance; }
            set { _divisionDistance = RestrictedDouble.ValueInClosedInterval(value, 0.1, 1000); }
        }

        [XmlElement("path_position")]
        private List<Point3d> _pathPosition;

        [XmlIgnore]
        public List<Point3d> PathPosition
        {
            get
            {
                return _pathPosition;
            }
            set
            {
                if (value.Count < 2)
                    throw new ArgumentException("List must have at least two elements.");
                _pathPosition = value;
            }
        }

        /// <summary>
        /// Gets or sets the vehicle position.
        /// </summary>
        [XmlElement("vehicle_positions")]
        public VehiclePosition VehiclePosition { get; set; }

        /// <summary>
        /// Gets or sets the return.
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("return")]
        public bool Return { get; set; } = false;

        /// <summary>
        /// Gets or sets the lock direction.
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("lock_direction")]
        public bool LockDirection { get; set; } = false;

        /// <summary>
        /// Gets or sets the cut to path.
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("cut_to_path")]
        public bool CutToPath { get; set; } = false;


        private MovingLoad() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MovingLoad"/> class.
        /// </summary>
        /// <param name="name">the name.</param>
        /// <param name="vehicle">the vehicle.</param>
        /// <param name="pathPosition">the path position.</param>
        /// <param name="vehiclePosition">the vehicle position.</param>
        /// <param name="shiftX">the shift x.</param>
        /// <param name="shiftY">the shift y.</param>
        /// <param name="returnPath">the return path.</param>
        /// <param name="lockDirection">the lock direction.</param>
        /// <param name="cutLoadsToPathExtent">the cut loads to path extent.</param>
        public MovingLoad(string name, Vehicle_lib_type vehicle, List<Point3d> pathPosition, List<Point3d> vehiclePosition, double shiftX = 0.00, double shiftY = 0.00, bool returnPath = false, bool lockDirection = false, bool cutLoadsToPathExtent = false)
        {
            this.EntityCreated();
            this.Name = name;
            this.PathPosition = pathPosition;
            this.VehiclePosition = new VehiclePosition(vehiclePosition);
            this.Vehicle = vehicle;
            this._vehicleGuid = vehicle.Guid;
            this.DivisionPoint = vehiclePosition.Count;

            this.VehicleShiftX = shiftX;
            this.VehicleShiftY = shiftY;
            this.Return = returnPath;
            this.LockDirection = lockDirection;
            this.CutToPath = cutLoadsToPathExtent;
        }


        /// <summary>
        /// Defines an operator overload.
        /// </summary>
        /// <param name="obj">the obj.</param>
        /// <returns>The result.</returns>
        public static implicit operator Moving_load_type(MovingLoad obj)
        {
            var movingLoad = new StruSoft.Interop.StruXml.Data.Moving_load_type();

            movingLoad.Name = obj.Name;
            movingLoad.Guid = obj.Guid.ToString();
            movingLoad.Vehicle_shift_x = obj.VehicleShiftX;
            movingLoad.Vehicle_shift_y = obj.VehicleShiftY;

            movingLoad.Vehicle = obj._vehicleGuid.ToString();
            movingLoad.Lock_direction = obj.LockDirection;
            movingLoad.Cut_to_path = obj.CutToPath;
            movingLoad.Return = obj.Return;

            var fdPoints = new List<StruSoft.Interop.StruXml.Data.Point_type_3d>();
            foreach(var pt in obj.PathPosition)
            {
                var fdPoint = new StruSoft.Interop.StruXml.Data.Point_type_3d();
                fdPoint.X = pt.X;
                fdPoint.Y = pt.Y;
                fdPoint.Z = pt.Z;

                fdPoints.Add(fdPoint);
            }

            var divisionPoint = new StruSoft.Interop.StruXml.Data.Path_division_number_type();
            divisionPoint.Value = obj.DivisionPoint;

            var items = new List<object>();
            items.Add(divisionPoint);

            foreach(var _obj in fdPoints)
            {
                items.Add(_obj);
            }

            movingLoad.Items = items.ToArray<object>();

            var divisionPointItems = new ItemsChoiceType3[] { ItemsChoiceType3.Division_points };
            var pathPositionsItems = Enumerable.Repeat(ItemsChoiceType3.Path_position, fdPoints.Count).ToArray();
            movingLoad.ItemsElementName = divisionPointItems.Concat(pathPositionsItems).ToArray();

            var vehiclePosition = new Moving_load_typeVehicle_positions();

            var fdVehiclePosition = new List<StruSoft.Interop.StruXml.Data.Point_type_3d>();
            foreach (var pt in obj.VehiclePosition.Position)
            {
                var fdPoint = new StruSoft.Interop.StruXml.Data.Point_type_3d();
                fdPoint.X = pt.X;
                fdPoint.Y = pt.Y;
                fdPoint.Z = pt.Z;

                fdVehiclePosition.Add(fdPoint);
            }


            vehiclePosition.Position = fdVehiclePosition;
            movingLoad.Items1 = new object[] { vehiclePosition };
            movingLoad.Items1ElementName = new Items1ChoiceType[] { Items1ChoiceType.Vehicle_positions };

            return movingLoad;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return $"{this.GetType().Name} Name: {this.Name}, Vehicle: {this.Vehicle.Name}, LockDirection: {this.LockDirection}: Return: {this.Return}, CutToPath: {this.CutToPath}";
        }
    }

    /// <summary>
    /// Represents a Vehicle Position.
    /// </summary>
    public partial class VehiclePosition
    {
        private VehiclePosition()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="VehiclePosition"/> class.
        /// </summary>
        /// <param name="Position">the position.</param>
        public VehiclePosition(List<Point3d> Position)
        {
            this.Position = Position;
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        [XmlElement("position")]
        public List<Point3d> Position { get; set; }
    }
}