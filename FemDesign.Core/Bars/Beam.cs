using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.Geometry;
using FemDesign.Materials;
using FemDesign.Sections;

namespace FemDesign.Bars
{
	/// <summary>
	/// Represents a Beam.
	/// </summary>
	[XmlRoot("database", Namespace = "urn:strusoft")]
	[System.Serializable]
	public partial class Beam : Bar
	{
		private Beam()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Beam"/> class.
		/// </summary>
		/// <param name="edge">the edge.</param>
		/// <param name="material">the material.</param>
		/// <param name="section">the section.</param>
		/// <param name="startEccentricity">the start eccentricity.</param>
		/// <param name="endEccentricity">the end eccentricity.</param>
		/// <param name="startConnectivity">the start connectivity.</param>
		/// <param name="endConnectivity">the end connectivity.</param>
		/// <param name="identifier">the identifier.</param>
		public Beam(Geometry.Edge edge, Materials.Material material, Sections.Section section, Eccentricity startEccentricity = null, Eccentricity endEccentricity = null, Connectivity startConnectivity = null, Connectivity endConnectivity = null, string identifier = "B") : base(edge, BarType.Beam, material, section, startEccentricity, endEccentricity, startConnectivity, endConnectivity, identifier)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Beam"/> class.
		/// </summary>
		/// <param name="edge">the edge.</param>
		/// <param name="material">the material.</param>
		/// <param name="startSection">the start section.</param>
		/// <param name="endSection">the end section.</param>
		/// <param name="startEccentricity">the start eccentricity.</param>
		/// <param name="endEccentricity">the end eccentricity.</param>
		/// <param name="startConnectivity">the start connectivity.</param>
		/// <param name="endConnectivity">the end connectivity.</param>
		/// <param name="identifier">the identifier.</param>
		public Beam(Geometry.Edge edge, Materials.Material material, Sections.Section startSection, Sections.Section endSection, Eccentricity startEccentricity, Eccentricity endEccentricity, Connectivity startConnectivity, Connectivity endConnectivity, string identifier) : base(edge, BarType.Beam, material, startSection, endSection, startEccentricity, endEccentricity, startConnectivity, endConnectivity, identifier)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Beam"/> class.
		/// </summary>
		/// <param name="edge">the edge.</param>
		/// <param name="material">the material.</param>
		/// <param name="sections">the sections.</param>
		/// <param name="eccentricities">the eccentricities.</param>
		/// <param name="connectivities">the connectivities.</param>
		/// <param name="identifier">the identifier.</param>
		public Beam(Geometry.Edge edge, Materials.Material material, Sections.Section[] sections, Eccentricity[] eccentricities, Connectivity[] connectivities, string identifier) : base(edge, BarType.Beam, material, sections, eccentricities, connectivities, identifier)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Beam"/> class.
        /// </summary>
        /// <param name="edge">the edge.</param>
        /// <param name="compositeSection">the composite section.</param>
        /// <param name="startEccentricity">the start eccentricity.</param>
        /// <param name="endEccentricity">the end eccentricity.</param>
        /// <param name="startConnectivity">the start connectivity.</param>
        /// <param name="endConnectivity">the end connectivity.</param>
        /// <param name="identifier">the identifier.</param>
        public Beam(Geometry.Edge edge, Composites.CompositeSection compositeSection, Eccentricity startEccentricity = null, Eccentricity endEccentricity = null, Connectivity startConnectivity = null, Connectivity endConnectivity = null, string identifier = "B") : base(edge, BarType.Beam, compositeSection, startEccentricity, endEccentricity, startConnectivity, endConnectivity, identifier)
        {
        }
    }
}
