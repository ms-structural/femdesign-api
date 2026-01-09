using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;

namespace FemDesign.Results
{
    /// <summary>
    /// FEM-Design list tables result types
    /// </summary>
    public interface IResult
    {
        /*
         * Interface cannot have a static method in language version below 8.0
         */
        //internal static IResult Parse(string[] row, Results.CsvReader reader, Dictionary<string, string> HeaderData)
        //internal static Regex IdentificationExpression { get; }
        //internal static Regex HeaderExpression { get; }
    }

    public static class ResultTypes
    {
        /// <summary>
        /// Gets or sets the all.
        /// </summary>
        public static Dictionary<string, Type> All = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetName().FullName.StartsWith("FemDesign"))
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(Results.IResult).IsAssignableFrom(p))
                .Where(p => p.IsClass)
                .Where(p => p.IsPublic)
                .ToDictionary(t => t.Name);
    }
}
