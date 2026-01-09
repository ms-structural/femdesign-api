using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using FemDesign;
using FemDesign.GenericClasses;
using FemDesign.Releases;

namespace FemDesign.Supports
{
	/// <summary>
	/// Represents a Stiffness Point.
	/// </summary>
	public partial class StiffnessPoint : EntityBase, IStructureElement, ISupportElement
	{
		/// <summary>
		/// Gets or sets the point.
		/// </summary>
		[XmlIgnore]
		public FemDesign.Geometry.Point3d Point { get; set; }

		/// <summary>
		/// Gets or sets the x.
		/// </summary>
		[XmlAttribute("x")]
		public double X;

		/// <summary>
		/// Gets or sets the y.
		/// </summary>
		[XmlAttribute("y")]
		public double Y;

		/// <summary>
		/// Gets or sets the z.
		/// </summary>
		[XmlAttribute("z")]
		public double Z;

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		[XmlAttribute("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the surface.
		/// </summary>
		[XmlIgnore]
		public FemDesign.Supports.SurfaceSupport Surface { get; set; }

		/// <summary>
		/// Gets or sets the surface support.
		/// </summary>
		[XmlAttribute("surface_support")]
		public Guid SurfaceSupport;

		/// <summary>
		/// Gets or sets the rigidity.
		/// </summary>
		[XmlElement("rigidity")]
		public RigidityDataType0 Rigidity { get; set; }
		/// <summary>
		/// Gets or sets the motions.
		/// </summary>
		public Motions Motions { get { return Rigidity?.Motions; } }
		/// <summary>
		/// Gets or sets the motions plasticity limits.
		/// </summary>
		public MotionsPlasticLimits MotionsPlasticityLimits { get { return Rigidity?.PlasticLimitForces; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="StiffnessPoint"/> class.
		/// </summary>
		public StiffnessPoint()
		{
		}

		private void Initialise(FemDesign.Supports.SurfaceSupport surface, FemDesign.Geometry.Point3d point)
		{
			this.SurfaceSupport = surface.Guid;
			this.X = point.X;
			this.Y = point.Y;
			this.Z = point.Z;
			this.EntityCreated();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StiffnessPoint"/> class.
		/// </summary>
		/// <param name="surface">the surface.</param>
		/// <param name="point">the point.</param>
		/// <param name="motions">the motions.</param>
		/// <param name="MotionsPlasticityLimits">the motions plasticity limits.</param>
		/// <param name="name">the name.</param>
		public StiffnessPoint(FemDesign.Supports.SurfaceSupport surface, FemDesign.Geometry.Point3d point, Motions motions, MotionsPlasticLimits MotionsPlasticityLimits = null, string name = null)
		{
			this.Initialise(surface, point);
			this.Rigidity = new RigidityDataType0(motions, MotionsPlasticityLimits);
			this.Name = name;
		}
	}
}