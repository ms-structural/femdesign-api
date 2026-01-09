using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using FemDesign.Calculate;
using Newtonsoft.Json;

namespace FemDesign.Results
{
    public static class Convert
    {
        /// <summary>
        /// The string representation of null.
        /// </summary>
        private static readonly string Null = "null";

        /// <summary>
        /// To json.
        /// </summary>
        /// <param name="value">the value.</param>
        /// <returns>The Json of an IResult object.</returns>
        public static string ToJSON(this IResult value)
        {
            if (value == null) return Null;

            try
            {
                string json = JsonConvert.SerializeObject(value);
                return json;
            }
            catch (Exception e)
            {
                //log exception but dont throw one
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// To json.
        /// </summary>
        /// <param name="value">the value.</param>
        /// <returns>The Json of an IResult object.</returns>
        public static string ToJSON(this IEnumerable<IResult> value)
        {
            if (value == null) return Null;

            try
            {
                string json = JsonConvert.SerializeObject(value);
                return json;
            }
            catch (Exception e)
            {
                //log exception but dont throw one
                throw new Exception(e.Message);
            }
        }
    }
}