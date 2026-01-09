using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.Geometry;
using FemDesign.Materials;
using FemDesign.Sections;
using StruSoft.Interop.StruXml.Data;

namespace FemDesign.Bars
{
	/// <summary>
	/// Represents a Truss.
	/// </summary>
	[XmlRoot("database", Namespace = "urn:strusoft")]
	[System.Serializable]
	public partial class Truss : Bar
	{
		private Truss()
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Truss"/> class.
		/// </summary>
		/// <param name="edge">the edge.</param>
		/// <param name="material">the material.</param>
		/// <param name="section">the section.</param>
		/// <param name="identifier">the identifier.</param>
		/// <param name="trussBehaviour">the truss behaviour.</param>
		public Truss(Geometry.Edge edge, Materials.Material material, Sections.Section section, string identifier, StruSoft.Interop.StruXml.Data.Truss_chr_type trussBehaviour = null) :base(edge, material, section, identifier, trussBehaviour)
		{
		}
	}
}
