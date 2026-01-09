// https://strusoft.com/

using System;

namespace FemDesign.Geometry
{
   internal class Degree
    {
        /// <summary>
        /// Convert degrees to radians.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="degree">the degree.</param>
        public static double ToRadians(double degree)
        {
            return (Math.PI / 180) * degree;
        }

        /// <summary>
        /// Convert radians to degrees.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="radians">the radians.</param>
        public static double FromRadians(double radians)
        {
            return (180 / Math.PI) * radians;
        }
    }
}