// https://strusoft.com/
using System.Collections.Generic;

namespace FemDesign
{
    /// <summary>
    /// Contains string restrictions used for property definitions.
    /// 
    /// Restrictions are defined by strusoft.xsd
    /// </summary>
    internal class RestrictedString
    {
        /// <summary>
        /// Create error message from items.
        /// </summary>
        internal static string RestrictionItemsMsg(List<string> items)
        {
            string rtn = ": ";
            for (int idx = 0; idx < items.Count; idx++)
            {
                if (idx == 0)
                {
                    // pass
                }
                else
                {
                    rtn += ", ";
                }
                rtn += items[idx];
            }
            return rtn;
        }

        /// <summary>
        /// Check if val in items.
        /// </summary>
        internal static string RestrictionItems(string val, List<string> items)
        {
            if (items.Contains(val))
            {
                return val;
            }
            else
            {
                string msg = RestrictedString.RestrictionItemsMsg(items);
                throw new System.ArgumentException($"Value must be{msg}");
            }
        }

        /// <summary>
        /// Check if length of val is smaller than len.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        internal static string Length(string val, int len)
        {
            if (val.Length > len)
            {
                throw new System.ArgumentException($"Length of value {val.Length} must be equal to {len} or shorter.");
            }
            else
            {
                return val;
            }
        }

        /// <summary>
        /// edgetype
        /// </summary>
        internal static string EdgeType(string val)
        {
            List<string> items = new List<string>(){"line", "arc", "spline", "polyline", "circle"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// eurocodetype
        /// </summary>
        internal static string EurocodeType(string val)
        {
            List<string> items = new List<string>(){"common", "H", "RO", "DK", "S", "N", "FIN", "GB", "D", "PL", "TR", "EST", "LT", "n/a"};
            return RestrictedString.RestrictionItems(val, items);
        }

        /// <summary>
        /// steelmadetype 
        /// </summary>
        internal static string SteelMadeType(string val)
        {
            List<string> items = new List<string>(){"rolled", "cold_worked", "welded"};
            return RestrictedString.RestrictionItems(val, items);
        }
    }
}