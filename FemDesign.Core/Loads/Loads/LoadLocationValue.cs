// https://strusoft.com/
using System.Collections.Generic;

namespace FemDesign.Loads
{
    /// <summary>
    /// Represents a Load Location Value.
    /// </summary>
    [System.Serializable]
    public partial class LoadLocationValue: LocationValue
    {
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LoadLocationValue()
        {

        }

        /// <summary>
        /// Internal constructor accessed by GH components and Dynamo nodes.
        /// </summary>
        /// <param name="loadPosition">the load position.</param>
        /// <param name="val">the val.</param>
        public LoadLocationValue(Geometry.Point3d loadPosition, double val)
        {
            this.X = loadPosition.X;
            this.Y = loadPosition.Y;
            this.Z = loadPosition.Z;
            this.Value = val;
        }


        /// <summary>
        /// Defines an operator overload.
        /// </summary>
        /// <param name="obj">the obj.</param>
        /// <returns>The result.</returns>
        public static implicit operator LoadLocationValue(StruSoft.Interop.StruXml.Data.Location_value obj)
        {
            var loadLocation = new LoadLocationValue();
            loadLocation.X = obj.X;
            loadLocation.Y = obj.Y;
            loadLocation.Z = obj.Z;
            loadLocation.Value = obj.Val;
            return loadLocation;
        }

        public static implicit operator StruSoft.Interop.StruXml.Data.Location_value(LoadLocationValue obj)
        {
            var loadLocationValue = new StruSoft.Interop.StruXml.Data.Location_value();
            loadLocationValue.X = obj.X;
            loadLocationValue.Y = obj.Y;
            loadLocationValue.Z = obj.Z;
            loadLocationValue.Val = obj.Value;

            return loadLocationValue;
        }
    }
}