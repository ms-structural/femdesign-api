using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.GenericClasses
{
    class ModelLoadCaseComparer : IEqualityComparer<FemDesign.Loads.ModelLoadCase>
    {
        // Products are equal if their names and product numbers are equal.
        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="x">value for <paramref name="x"/>.</param>
        /// <param name="y">value for <paramref name="y"/>.</param>
        /// <returns>The result.</returns>
        public bool Equals(FemDesign.Loads.ModelLoadCase x, FemDesign.Loads.ModelLoadCase y)
        {
            //Check whether the compared objects reference the same data.
            if (x.Guid == y.Guid) return true;
            else return false;
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <param name="loadCase">the load case.</param>
        /// <returns>The result.</returns>
        public int GetHashCode(FemDesign.Loads.ModelLoadCase loadCase)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(loadCase, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashLoadCaseGuid = loadCase.Guid == null ? 0 : loadCase.Guid.GetHashCode();

            //Get hash code for the Code field.
            int hashLoadCaseGamma = loadCase.Gamma.GetHashCode();

            //Calculate the hash code for the product.
            return hashLoadCaseGuid ^ hashLoadCaseGamma;
        }
    }
}
