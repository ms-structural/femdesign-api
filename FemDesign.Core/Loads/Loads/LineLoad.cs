// https://strusoft.com/

using FemDesign.Geometry;
using StruSoft.Interop.StruXml.Data;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Line Load.
    /// </summary>
    [System.Serializable]
    public partial class LineLoad : ForceLoadBase
    {
        /// <summary>
        /// Gets or sets the constant load direction.
        /// </summary>
        [XmlAttribute("load_dir")]
        public LoadDirType _constantLoadDirection; // load_dir_type
        [XmlIgnore]
        public bool ConstantLoadDirection
        {
            get
            {
                return this._constantLoadDirection == LoadDirType.Constant;
            }
            set
            {
                this._constantLoadDirection = value ? LoadDirType.Constant : LoadDirType.Changing;
            }
        }
        /// <summary>
        /// Gets or sets the load projection.
        /// </summary>
        [XmlAttribute("load_projection")]
        public bool LoadProjection { get; set; } // bool

        // elements
        /// <summary>
        /// Gets or sets the edge.
        /// </summary>
        [XmlElement("edge", Order = 1)]
        public Geometry.Edge Edge { get; set; } // edge_type
        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        [XmlElement("direction", Order = 2)]
        public Geometry.Vector3d Direction { get; set; } // point_type_3d
        /// <summary>
        /// Gets or sets the normal.
        /// </summary>
        [XmlElement("normal", Order = 3)]
        public Geometry.Vector3d Normal { get; set; } // point_type_3d
        /// <summary>
        /// Gets or sets the load.
        /// </summary>
        [XmlElement("load", Order = 4)]
        public LoadLocationValue[] Load = new LoadLocationValue[2];
        [XmlIgnore]
        public double StartLoad
        {
            get
            {
                return this.Load[0].Value;
            }
            set
            {
                this.Load[0] = new LoadLocationValue(this.Edge.Points[0], value);
            }
        }
        [XmlIgnore]
        public double EndLoad
        {
            get
            {
                return this.Load[1].Value;
            }
            set
            {
                this.Load[1] = new LoadLocationValue(this.Edge.Points[this.Edge.Points.Count - 1], value);
            }
        }
        [XmlIgnore]
        public Geometry.Vector3d StartForce
        {
            get
            {
                return this.Direction.Scale(this.StartLoad);
            }
        }
        [XmlIgnore]
        public Geometry.Vector3d EndForce
        {
            get
            {
                return this.Direction.Scale(this.EndLoad);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LineLoad()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineLoad"/> class.
        /// </summary>
        /// <param name="edge">the edge.</param>
        /// <param name="constantForce">the constant force.</param>
        /// <param name="loadCase">the load case.</param>
        /// <param name="loadType">the load type.</param>
        /// <param name="comment">the comment.</param>
        /// <param name="constLoadDir">the const load dir.</param>
        /// <param name="loadProjection">the load projection.</param>
        public LineLoad(Geometry.Edge edge, Geometry.Vector3d constantForce, LoadCase loadCase, ForceLoadType loadType, string comment = "", bool constLoadDir = true, bool loadProjection = false)
        {
            this.EntityCreated();
            this.LoadCase = loadCase;
            this.Comment = comment;
            this.ConstantLoadDirection = constLoadDir;
            this.LoadProjection = loadProjection;
            this.LoadType = loadType;
            this.Edge = edge;
            this.Normal = edge.Plane.LocalZ; // Note that LineLoad normal and Edge normal are not necessarily the same.
            this.SetStartAndEndForces(constantForce, constantForce);
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="edge">the edge.</param>
        /// <param name="startForce">the start force.</param>
        /// <param name="endForce">the end force.</param>
        /// <param name="loadCase">the load case.</param>
        /// <param name="loadType">the load type.</param>
        /// <param name="comment">the comment.</param>
        /// <param name="constLoadDir">the const load dir.</param>
        /// <param name="loadProjection">the load projection.</param>
        public LineLoad(Geometry.Edge edge, Geometry.Vector3d startForce, Geometry.Vector3d endForce, LoadCase loadCase, ForceLoadType loadType, string comment = "", bool constLoadDir = true, bool loadProjection = false)
        {
            this.EntityCreated();
            this.LoadCase = loadCase;
            this.Comment = comment;
            this.ConstantLoadDirection = constLoadDir;
            this.LoadProjection = loadProjection;
            this.LoadType = loadType;
            this.Edge = edge;
            this.Normal = edge.Plane.LocalZ; // Note that LineLoad normal and Edge normal are not necessarily the same.
            this.SetStartAndEndForces(startForce, endForce);
        }

        /// <summary>
        /// Set line load direction and start/end magnitudes from two vectors.
        /// </summary>
        /// <remarks>
        /// The input vectors must be parallel or antiparallel. Their lengths are used as the start/end magnitudes,
        /// and the (normalized) vector direction is stored in <see cref="Direction"/>.
        /// </remarks>
        /// <param name="startForce">Vector defining load direction and start magnitude (length).</param>
        /// <param name="endForce">Vector defining load direction and end magnitude (length).</param>
        /// <exception cref="ArgumentException">
        /// Thrown when both vectors are zero, or when they are neither parallel nor antiparallel.
        /// </exception>
        internal void SetStartAndEndForces(Geometry.Vector3d startForce, Geometry.Vector3d endForce)
        {
            if (startForce.IsZero() && !endForce.IsZero())
            {
                this.Direction = endForce.Normalize();
                this.StartLoad = 0;
                this.EndLoad = endForce.Length();
            }

            else if (!startForce.IsZero() && endForce.IsZero())
            {
                this.Direction = startForce.Normalize();
                this.StartLoad = startForce.Length();
                this.EndLoad = 0;
            }

            else if (startForce.IsZero() && endForce.IsZero())
            {
                throw new System.ArgumentException($"Both StartForce and EndForce are zero vectors. Can't set direction of LineLoad.");
            }

            // if no zero vectors - check if vectors are parallel
            else
            {
                Geometry.Vector3d v0 = startForce.Normalize();
                Geometry.Vector3d v1 = endForce.Normalize();
                double q0 = startForce.Length();
                double q1 = endForce.Length();

                int par = v0.IsParallel(v1);
                if (par != 0)
                {
                    this.Direction = v0;
                    this.StartLoad = q0;
                    this.EndLoad = par * q1;
                }
                else
                {
                    throw new System.ArgumentException($"StartForce and EndForce must be parallel or antiparallel.");
                }
            }
        }


        /// <summary>
        /// Create a variable (linearly varying) distributed force line load along an edge.
        /// </summary>
        /// <remarks>
        /// Units: <c>kN/m</c>. <paramref name="startForce"/> and <paramref name="endForce"/> must be parallel or antiparallel.
        /// </remarks>
        /// <param name="edge">Edge to apply the line load to.</param>
        /// <param name="startForce">Force per length at the start of the edge (vector direction defines load direction).</param>
        /// <param name="endForce">Force per length at the end of the edge (vector direction defines load direction).</param>
        /// <param name="loadCase">Load case to assign to the line load.</param>
        /// <param name="comment">Optional comment.</param>
        /// <param name="constLoadDir">If <c>true</c>, keeps a constant load direction along the edge.</param>
        /// <param name="loadProjection">If <c>true</c>, the load is projected.</param>
        /// <returns>A <see cref="LineLoad"/> of type <see cref="ForceLoadType.Force"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when both vectors are zero, or when they are neither parallel nor antiparallel.
        /// </exception>
        public static LineLoad VariableForce(Geometry.Edge edge, Geometry.Vector3d startForce, Geometry.Vector3d endForce, LoadCase loadCase, string comment = "", bool constLoadDir = true, bool loadProjection = false)
		{
            return new LineLoad(edge, startForce, endForce, loadCase, ForceLoadType.Force, comment, constLoadDir, loadProjection);
        }


        /// <summary>
        /// Create a variable (linearly varying) distributed moment line load along an edge.
        /// </summary>
        /// <remarks>
        /// Units: <c>kNm/m</c>. <paramref name="startForce"/> and <paramref name="endForce"/> must be parallel or antiparallel.
        /// </remarks>
        /// <param name="edge">Edge to apply the line load to.</param>
        /// <param name="startForce">Moment per length at the start of the edge (vector direction defines load direction).</param>
        /// <param name="endForce">Moment per length at the end of the edge (vector direction defines load direction).</param>
        /// <param name="loadCase">Load case to assign to the line load.</param>
        /// <param name="comment">Optional comment.</param>
        /// <param name="constLoadDir">If <c>true</c>, keeps a constant load direction along the edge.</param>
        /// <param name="loadProjection">If <c>true</c>, the load is projected.</param>
        /// <returns>A <see cref="LineLoad"/> of type <see cref="ForceLoadType.Moment"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when both vectors are zero, or when they are neither parallel nor antiparallel.
        /// </exception>
        public static LineLoad VariableMoment(Geometry.Edge edge, Geometry.Vector3d startForce, Geometry.Vector3d endForce, LoadCase loadCase, string comment = "", bool constLoadDir = true, bool loadProjection = false)
        {
            return new LineLoad(edge, startForce, endForce, loadCase, ForceLoadType.Moment, comment, constLoadDir, loadProjection);
        }

        /// <summary>
        /// Create a uniform distributed force line load along an edge.
        /// </summary>
        /// <remarks>Units: <c>kN/m</c>.</remarks>
        /// <param name="edge">Edge to apply the line load to.</param>
        /// <param name="constantForce">Force per length (vector direction defines load direction).</param>
        /// <param name="loadCase">Load case to assign to the line load.</param>
        /// <param name="comment">Optional comment.</param>
        /// <param name="constLoadDir">If <c>true</c>, keeps a constant load direction along the edge.</param>
        /// <param name="loadProjection">If <c>true</c>, the load is projected.</param>
        /// <returns>A <see cref="LineLoad"/> of type <see cref="ForceLoadType.Force"/>.</returns>
        public static LineLoad UniformForce(Geometry.Edge edge, Geometry.Vector3d constantForce, LoadCase loadCase, string comment = "", bool constLoadDir = true, bool loadProjection = false)
        {
            return new LineLoad(edge, constantForce, loadCase, ForceLoadType.Force, comment, constLoadDir, loadProjection);
        }

        /// <summary>
        /// Create a uniform distributed force line load along an edge without a load case.
        /// </summary>
        /// <remarks>
        /// Units: <c>kN/m</c>. This is mainly intended for interoperability with the "caseless" StruXML load types.
        /// </remarks>
        /// <param name="edge">Edge to apply the line load to.</param>
        /// <param name="constantForce">Force per length (vector direction defines load direction).</param>
        /// <param name="constLoadDir">If <c>true</c>, keeps a constant load direction along the edge.</param>
        /// <param name="loadProjection">If <c>true</c>, the load is projected.</param>
        /// <returns>A <see cref="LineLoad"/> with <see cref="LoadCase"/> left unset.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="constantForce"/> is a zero vector.</exception>
        public static LineLoad CaselessUniformForce(Geometry.Edge edge, Geometry.Vector3d constantForce, bool constLoadDir = true, bool loadProjection = false)
        {
            var caseless = new LineLoad();

            caseless.EntityCreated();
            caseless.ConstantLoadDirection = constLoadDir;
            caseless.LoadProjection = loadProjection;
            caseless.LoadType = ForceLoadType.Force;
            caseless.Edge = edge;
            caseless.Normal = edge.Plane.LocalZ; // Note that LineLoad normal and Edge normal are not necessarily the same.
            caseless.SetStartAndEndForces(constantForce, constantForce);

            return caseless;
        }

        /// <summary>
        /// Create a uniform distributed moment line load along an edge.
        /// </summary>
        /// <remarks>Units: <c>kNm/m</c>.</remarks>
        /// <param name="edge">Edge to apply the line load to.</param>
        /// <param name="constantForce">Moment per length (vector direction defines load direction).</param>
        /// <param name="loadCase">Load case to assign to the line load.</param>
        /// <param name="comment">Optional comment.</param>
        /// <param name="constLoadDir">If <c>true</c>, keeps a constant load direction along the edge.</param>
        /// <param name="loadProjection">If <c>true</c>, the load is projected.</param>
        /// <returns>A <see cref="LineLoad"/> of type <see cref="ForceLoadType.Moment"/>.</returns>
        public static LineLoad UniformMoment(Geometry.Edge edge, Geometry.Vector3d constantForce, LoadCase loadCase, string comment = "", bool constLoadDir = true, bool loadProjection = false)
        {
            return new LineLoad(edge, constantForce, loadCase, ForceLoadType.Moment, comment, constLoadDir, loadProjection);
        }

        /// <summary>
        /// Defines an operator overload.
        /// </summary>
        /// <param name="obj">the obj.</param>
        /// <returns>The result.</returns>
        public static explicit operator LineLoad(StruSoft.Interop.StruXml.Data.Caseless_line_load_type obj)
        {
            var lineLoad = new LineLoad();

            lineLoad.Guid = new System.Guid(obj.Guid);
            lineLoad.Action = obj.Action.ToString();
            // constant of changing
            lineLoad._constantLoadDirection = (LoadDirType)Enum.Parse(typeof(LoadDirType), obj.Load_dir.ToString());
            lineLoad.LoadProjection = obj.Load_projection;

            lineLoad.LoadType = (ForceLoadType)Enum.Parse(typeof(ForceLoadType), obj.Load_type.ToString());


            //LineLoad.Edge
            var start = obj.Edge.Point[0];
            var end = obj.Edge.Point[1];
            var normal = obj.Edge.Normal;
            var edge = new Edge(start, end, normal);

            lineLoad.Edge = edge;

            lineLoad.Direction = new Vector3d(obj.Direction.X, obj.Direction.Y, obj.Direction.Z);
            lineLoad.Normal = new Vector3d(obj.Normal.X, obj.Normal.Y, obj.Normal.Z);

            lineLoad.StartLoad = obj.Load[0].Val;
            lineLoad.EndLoad = obj.Load[1].Val;

            return lineLoad;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            var units = this.LoadType == ForceLoadType.Force ? "kN" : "kNm";
            var text = $"{this.GetType().Name} Start: {this.StartForce} {units}, End: {this.EndForce} {units}, Projected: {this.LoadProjection}";
            if (LoadCase != null)
                return text + $", LoadCase: {this.LoadCase.Name}";
            else
                return text;
        }
    }
}