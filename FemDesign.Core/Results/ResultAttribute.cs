using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign.Calculate;


namespace FemDesign.Results
{
    /// <summary>
    /// Represents a Result Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public partial class ResultAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the result type.
        /// </summary>
        public readonly Type ResultType;
        /// <summary>
        /// Gets or sets the list procs.
        /// </summary>
        public readonly ListProc[] ListProcs;
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultAttribute"/> class.
        /// </summary>
        /// <param name="resultType">the result type.</param>
        /// <param name="listProc">the list proc.</param>
        public ResultAttribute(Type resultType, params ListProc[] listProc)
        {
            if (!typeof(IResult).IsAssignableFrom(resultType))
                throw new ArgumentException();

            ListProcs = listProc;
        }
    }
}