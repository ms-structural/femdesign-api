using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.GenericClasses
{
    /// <summary>
    /// Represents strings that can be parsed to the enum by the <seealso cref="EnumParser"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public partial class ParseableAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the names.
        /// </summary>
        public readonly string[] Names;

        /// <summary>
        /// Represents strings that can be parsed to the enum by the <seealso cref="EnumParser"/>
        /// </summary>
        /// <param name="name">Alternative name to be parsed to the enum</param>
        public ParseableAttribute(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }

            Names = new[] { name };
        }

        /// <summary>
        /// Represents strings that can be parsed to the enum by the <seealso cref="EnumParser"/>
        /// </summary>
        /// <param name="names">Alternative names to be parsed to the enum</param>
        public ParseableAttribute(params string[] names)
        {
            if (names == null || names.Any(x => x == null))
            {
                throw new ArgumentNullException();
            }

            Names = names;
        }
    }

    public static class EnumParser
    {
        /// <summary>
        /// Parse a string to <typeparamref name="TEnum"/>. String must be one of the values of <see cref="ParseableAttribute"/> on the enum.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TEnum Parse<TEnum>(string value) where TEnum : struct
        {
            try
            {
                return ParseEnumImpl<TEnum>.Values[value];
            }
            catch (KeyNotFoundException)
            {
                var possible = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
                string possibleString = string.Join(", ", possible);
                throw new ArgumentException($"Could not parse value '{value}' to type {typeof(TEnum)}. Must be one of the following values '{possibleString}'.");
            }
        }

        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct
        {
            return ParseEnumImpl<TEnum>.Values.TryGetValue(value, out result);
        }

        private static class ParseEnumImpl<TEnum> where TEnum : struct
        {
            /// <summary>
            /// Gets or sets the values.
            /// </summary>
            public static readonly Dictionary<string, TEnum> Values = new Dictionary<string, TEnum>();

            static ParseEnumImpl()
            {
                var nameAttributes = typeof(TEnum)
                    .GetFields()
                    .Select(x => new
                    {
                        Value = x,
                        Names = x.GetCustomAttributes(typeof(ParseableAttribute), false)
                            .Cast<ParseableAttribute>()
                    });

                var degrouped = nameAttributes.SelectMany(
                    x => x.Names.SelectMany(y => y.Names),
                    (x, y) => new { Value = x.Value, Name = y });

                foreach (var x in degrouped)
                {
                    Values.Add(x.Name, (TEnum)x.Value.GetValue(x));
                }
            }
        }
    }
}
