using System.Xml.Serialization;


namespace FemDesign.ModellingTools
{
    /// <summary>
    /// Represents a Stiffness Matrix4 Type.
    /// </summary>
    [System.Serializable]
    public partial class StiffnessMatrix4Type
    {
        /// <summary>
        /// Gets or sets the xx.
        /// </summary>
        [XmlAttribute("xx")]
        public double _xx;
        [XmlIgnore]
        public double XX
        {
            get
            {
                return this._xx;
            }
            set
            {
                this._xx = RestrictedDouble.NonZeroMax_1e20(value);
            }
        }

        /// <summary>
        /// Gets or sets the xy.
        /// </summary>
        [XmlAttribute("xy")]
        public double _xy;
        [XmlIgnore]
        public double XY
        {
            get
            {
                return this._xy;
            }
            set
            {
                this._xy = RestrictedDouble.NonZeroMax_1e20(value);
            }
        }

        /// <summary>
        /// Gets or sets the yy.
        /// </summary>
        [XmlAttribute("yy")]
        public double _yy;
        [XmlIgnore]
        public double YY
        {
            get
            {
                return this._yy;
            }
            set
            {
                this._yy = RestrictedDouble.NonZeroMax_1e20(value);
            }
        }

        /// <summary>
        /// Gets or sets the gxy.
        /// </summary>
        [XmlAttribute("gxy")]
        public double _gxy;
        [XmlIgnore]
        public double GXY
        {
            get
            {
                return this._gxy;
            }
            set
            {
                this._gxy = RestrictedDouble.NonZeroMax_1e20(value);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private StiffnessMatrix4Type()
        {

        }

        /// <summary>
        /// Construct a stiffness matrix 4 type.
        /// </summary>
        /// <param name="xx"></param>
        /// <param name="xy"></param>
        /// <param name="yy"></param>
        /// <param name="gxy"></param>
        public StiffnessMatrix4Type(double xx, double xy, double yy, double gxy)
        {
            this.XX = xx;
            this.XY = xy;
            this.YY = yy;
            this.GXY = gxy;

            this.CheckMatrixValidity();
        }

        private void CheckMatrixValidity()
        {
            if (XX * YY - XY * XY <= 0.0)
                throw new System.Exception("(XX*YY)-(XY*XY) must be greater than 0.0!");
        }
    }
}