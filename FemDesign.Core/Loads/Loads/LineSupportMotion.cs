// https://strusoft.com/

using FemDesign.Geometry;
using StruSoft.Interop.StruXml.Data;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Line Support Motion.
    /// </summary>
    [System.Serializable]
    public partial class LineSupportMotion : SupportMotionBase
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
        /// Gets or sets the displacement.
        /// </summary>
        [XmlElement("displacement", Order = 4)]
        public LoadLocationValue[] Displacement = new LoadLocationValue[2];
        [XmlIgnore]
        public double StartDisplacementValue
        {
            get
            {
                return this.Displacement[0].Value;
            }
            set
            {
                this.Displacement[0] = new LoadLocationValue(this.Edge.Points[0], value);
            }
        }
        [XmlIgnore]
        public double EndDisplacement
        {
            get
            {
                return this.Displacement[1].Value;
            }
            set
            {
                this.Displacement[1] = new LoadLocationValue(this.Edge.Points[this.Edge.Points.Count - 1], value);
            }
        }
        [XmlIgnore]
        public Geometry.Vector3d StartDisp
        {
            get
            {
                return this.Direction.Scale(this.StartDisplacementValue);
            }
        }
        [XmlIgnore]
        public Geometry.Vector3d EndDisp
        {
            get
            {
                return this.Direction.Scale(this.EndDisplacement);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LineSupportMotion()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSupportMotion"/> class.
        /// </summary>
        /// <param name="edge">the edge.</param>
        /// <param name="constantForce">the constant force.</param>
        /// <param name="loadCase">the load case.</param>
        /// <param name="supportMotionType">the support motion type.</param>
        /// <param name="comment">the comment.</param>
        /// <param name="constLoadDir">the const load dir.</param>
        public LineSupportMotion(Geometry.Edge edge, Geometry.Vector3d constantForce, LoadCase loadCase, SupportMotionType supportMotionType, string comment = "", bool constLoadDir = true)
        {
            this.EntityCreated();
            this.LoadCase = loadCase;
            this.Comment = comment;
            this.ConstantLoadDirection = constLoadDir;
            this.SupportMotionType = supportMotionType;
            this.Edge = edge;
            this.Normal = edge.Plane.LocalZ; // Note that LineLoad normal and Edge normal are not necessarily the same.
            this.SetStartAndEndDisplacements(constantForce, constantForce);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSupportMotion"/> class.
        /// </summary>
        /// <param name="edge">the edge.</param>
        /// <param name="startForce">the start force.</param>
        /// <param name="endForce">the end force.</param>
        /// <param name="loadCase">the load case.</param>
        /// <param name="supportMotionType">the support motion type.</param>
        /// <param name="comment">the comment.</param>
        /// <param name="constLoadDir">the const load dir.</param>
        public LineSupportMotion(Geometry.Edge edge, Geometry.Vector3d startForce, Geometry.Vector3d endForce, LoadCase loadCase, SupportMotionType supportMotionType, string comment = "", bool constLoadDir = true)
        {
            this.EntityCreated();
            this.LoadCase = loadCase;
            this.Comment = comment;
            this.ConstantLoadDirection = constLoadDir;
            this.SupportMotionType = supportMotionType;
            this.Edge = edge;
            this.Normal = edge.Plane.LocalZ; // Note that LineLoad normal and Edge normal are not necessarily the same.
            this.SetStartAndEndDisplacements(startForce, endForce);
        }

        internal void SetStartAndEndDisplacements(Geometry.Vector3d startForce, Geometry.Vector3d endForce)
        {
            if (startForce.IsZero() && !endForce.IsZero())
            {
                this.Direction = endForce.Normalize();
                this.StartDisplacementValue = 0;
                this.EndDisplacement = endForce.Length();
            }

            else if (!startForce.IsZero() && endForce.IsZero())
            {
                this.Direction = startForce.Normalize();
                this.StartDisplacementValue = startForce.Length();
                this.EndDisplacement = 0;
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
                    this.StartDisplacementValue = q0;
                    this.EndDisplacement = par * q1;
                }
                else
                {
                    throw new System.ArgumentException($"StartForce and EndForce must be parallel or antiparallel.");
                }
            }
        }


        /// <summary>
        /// Create a Distributed Motion Load to be applied to an Edge [m]
        /// </summary>
        /// <param name="edge">Edge to apply the support motion to.</param>
        /// <param name="startForce">Displacement vector at the start of the edge.</param>
        /// <param name="endForce">Displacement vector at the end of the edge.</param>
        /// <param name="loadCase">Load case to assign to the support motion.</param>
        /// <param name="comment">Optional comment.</param>
        /// <param name="constLoadDir">If <c>true</c>, keeps a constant direction along the edge.</param>
        /// <returns>The created <see cref="LineSupportMotion"/>.</returns>
        public static LineSupportMotion VariableMotion(Geometry.Edge edge, Geometry.Vector3d startForce, Geometry.Vector3d endForce, LoadCase loadCase, string comment = "", bool constLoadDir = true)
        {
            return new LineSupportMotion(edge, startForce, endForce, loadCase, SupportMotionType.Motion, comment, constLoadDir);
        }


        /// <summary>
        /// Create a Distributed Rotation Load to be applied to an Edge [rad]
        /// </summary>
        /// <param name="edge">Edge to apply the support rotation to.</param>
        /// <param name="startForce">Rotation vector at the start of the edge.</param>
        /// <param name="endForce">Rotation vector at the end of the edge.</param>
        /// <param name="loadCase">Load case to assign to the support motion.</param>
        /// <param name="comment">Optional comment.</param>
        /// <param name="constLoadDir">If <c>true</c>, keeps a constant direction along the edge.</param>
        /// <returns>The created <see cref="LineSupportMotion"/>.</returns>
        public static LineSupportMotion VariableRotation(Geometry.Edge edge, Geometry.Vector3d startForce, Geometry.Vector3d endForce, LoadCase loadCase, string comment = "", bool constLoadDir = true)
        {
            return new LineSupportMotion(edge, startForce, endForce, loadCase, SupportMotionType.Rotation, comment, constLoadDir);
        }

        /// <summary>
        /// Create a UniformDistributed Motion Load to be applied to an Edge [m]
        /// </summary>
        /// <param name="edge">Edge to apply the support motion to.</param>
        /// <param name="constantForce">Displacement vector along the edge.</param>
        /// <param name="loadCase">Load case to assign to the support motion.</param>
        /// <param name="comment">Optional comment.</param>
        /// <param name="constLoadDir">If <c>true</c>, keeps a constant direction along the edge.</param>
        /// <returns>The created <see cref="LineSupportMotion"/>.</returns>
        public static LineSupportMotion UniformMotion(Geometry.Edge edge, Geometry.Vector3d constantForce, LoadCase loadCase, string comment = "", bool constLoadDir = true)
        {
            return new LineSupportMotion(edge, constantForce, loadCase, SupportMotionType.Motion, comment, constLoadDir);
        }

        /// <summary>
        /// Create a Uniform Distributed Rotation Load to be applied to an Edge [rad]
        /// </summary>
        /// <param name="edge">Edge to apply the support rotation to.</param>
        /// <param name="constantForce">Rotation vector along the edge.</param>
        /// <param name="loadCase">Load case to assign to the support motion.</param>
        /// <param name="comment">Optional comment.</param>
        /// <param name="constLoadDir">If <c>true</c>, keeps a constant direction along the edge.</param>
        /// <returns>The created <see cref="LineSupportMotion"/>.</returns>
        public static LineSupportMotion UniformRotation(Geometry.Edge edge, Geometry.Vector3d constantForce, LoadCase loadCase, string comment = "", bool constLoadDir = true)
        {
            return new LineSupportMotion(edge, constantForce, loadCase, SupportMotionType.Rotation, comment, constLoadDir);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            var units = this.SupportMotionType == SupportMotionType.Motion ? "m" : "rad";
            var text = $"{this.GetType().Name} Start: {this.StartDisp} {units}, End: {this.EndDisp} {units}";
            if (LoadCase != null)
                return text + $", LoadCase: {this.LoadCase.Name}";
            else
                return text;
        }
    }
}