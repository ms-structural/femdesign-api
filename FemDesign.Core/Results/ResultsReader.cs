using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Linq;
using System.Reflection;



namespace FemDesign.Results
{
    using ResultParserType = Func<string[], CsvParser, Dictionary<string, string>, Results.IResult>;

    /// <summary>
    /// Reads FEM-Design results from list tables text files.
    /// </summary>
    public partial class ResultsReader : CsvParser
    {
        private Regex HeaderExpression;
        private Dictionary<Type, Regex> ResultTypesIdentificationExpressions;
        private Dictionary<Type, ResultParserType> ResultTypesRowParsers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultsReader"/> class.
        /// </summary>
        /// <param name="filePath">Path to a <c>.txt</c>/<c>.csv</c> results file exported from FEM-Design.</param>
        public ResultsReader(string filePath) : base(filePath, delimiter: '\t')
        {
            Type iResultType = typeof(Results.IResult);
            var resultTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetName().FullName.StartsWith("FemDesign"))
                .SelectMany(s => s.GetTypes())
                .Where(p => iResultType.IsAssignableFrom(p))
                .Where(p => p.IsClass)
                .Where(p => p.IsPublic);

            HeaderExpression = new Regex(string.Join("|", resultTypes.Select(GetHeaderExpression).Select(r => r.ToString())));
            ResultTypesIdentificationExpressions = resultTypes.ToDictionary(t => t, GetIdentificationExpression);
            ResultTypesRowParsers = resultTypes.ToDictionary(t => t, GetRowParser);
        }

        protected sealed override void BeforeParse(Type type)
        {
            if (!typeof(Results.IResult).IsAssignableFrom(type))
                throw new ArgumentException($"{type.FullName} is not a result type of {typeof(Results.IResult).FullName}.");
            if (!ResultTypesRowParsers.ContainsKey(type))
                throw new NotImplementedException($"Parser for {type} has not yet been implemented.");

            HeaderParser = parseHeaderDefault;
            RowParser = ResultTypesRowParsers[type];

            base.BeforeParse(type);
        }

        private bool parseHeaderDefault(string line, CsvParser reader)
        {
            var match = HeaderExpression.Match(line);

            if (match.Success)
            {
                foreach (Group group in match.Groups)
                {
                    if (!string.IsNullOrEmpty(group.Value))
                        HeaderData[group.Name] = group.Value;
                }
            }

            return match.Success;
        }

        /// <summary>
        /// Parses all of a results file. Returns a mixed list of all results in file.
        /// </summary>
        /// <returns></returns>
        public List<Results.IResult> ParseAll()
        {
            Type resultType = null;
            List<Results.IResult> mixedResults = new List<Results.IResult>();

            MethodInfo method = typeof(ResultsReader).GetMethod(
                "ParseAll",
                BindingFlags.Instance | BindingFlags.Public,
                Type.DefaultBinder,
                new Type[] { typeof(bool), typeof(bool) },
                null
            );

            while (!IsDone)
            {
                do
                {
                    resultType = SniffResultType();
                } while (resultType is null && !IsDone);

                MethodInfo parseAllMethod = method.MakeGenericMethod(resultType);

                try
                {
                    dynamic obj = parseAllMethod.Invoke(this, new object[] { false, true });
                    mixedResults.AddRange(obj);
                }
                catch (TargetInvocationException e)
                {
                    throw new ParseException(resultType, "<all>", e.InnerException);
                }
            }

            return mixedResults;
        }

        /// <summary>
        /// Parses all of a results file. Returns a mixed list of all results in file.
        /// </summary>
        /// <param name="resultsFilePath">Results file.</param>
        /// <returns></returns>
        public static List<Results.IResult> Parse(string resultsFilePath)
        {
            using (var reader = new ResultsReader(resultsFilePath))
                return reader.ParseAll();
        }

        /// <summary>
        /// Read and parse result data from .csv files
        /// </summary>
        public static List<T> ParseCsvFiles<T>(List<string> csvPaths) where T : Results.IResult
        {
            List<T> results = new List<T>();
            foreach (string resultFile in csvPaths)
            {
                results.AddRange(
                    Results.ResultsReader.Parse(resultFile).ConvertAll(r => (T)r)
                );
            }
            return results;
        }

        /// <summary>
        /// Tries to figure out the corresponding type of the upcoming lines that are being read.
        /// </summary>
        /// <returns></returns>
        private Type SniffResultType()
        {
            Type resultType = null;
            while (CanPeek && resultType == null)
            {
                string line = PeekNextLine();
                if (line == null)
                    continue;

                Match match = HeaderExpression.Match(line);
                if (match.Success)
                    foreach (var type in ResultTypesIdentificationExpressions.Keys)
                    {
                        if (ResultTypesIdentificationExpressions[type].IsMatch(line))
                        {
                            resultType = type;
                            break;
                        }
                    }
            }

            if (resultType is null)
                throw new ApplicationException($"Could not identify all result types of the file {FilePath}.");

            return resultType;
        }

        private Regex GetIdentificationExpression(Type type)
        {
            if (!typeof(Results.IResult).IsAssignableFrom(type))
                throw new Exception();

            PropertyInfo propertyInfo = type.GetProperty("IdentificationExpression", BindingFlags.Static | BindingFlags.NonPublic);
            Regex identificationExpression = (Regex)propertyInfo.GetValue(null, null);

            return identificationExpression;
        }

        private Regex GetHeaderExpression(Type type)
        {
            if (!typeof(Results.IResult).IsAssignableFrom(type))
                throw new Exception();

            PropertyInfo propertyInfo = type.GetProperty("HeaderExpression", BindingFlags.Static | BindingFlags.NonPublic);
            Regex headerExpression = (Regex)propertyInfo.GetValue(null, null);

            return headerExpression;
        }

        private ResultParserType GetRowParser(Type type)
        {
            if (!typeof(Results.IResult).IsAssignableFrom(type))
                throw new Exception();

            MethodInfo mathodInfo = type.GetMethod("Parse", BindingFlags.Static | BindingFlags.NonPublic);

            return (ResultParserType)mathodInfo.CreateDelegate(typeof(ResultParserType));
        }

        /// <summary>
        /// Object Representation.
        /// </summary>
        /// <param name="myObject">the my object.</param>
        /// <returns>The result.</returns>
        public static string ObjectRepresentation(object myObject)
        {

            Type myType = myObject.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            string objRepr = $"{myType.Name},";

            foreach (PropertyInfo prop in props)
            {
                // check if prop is a list. if it is, continue to next prop
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>) || prop.Name.StartsWith("_"))
                    continue;

                object propValue = prop.GetValue(myObject, null);
                objRepr += $" {prop.Name}: {propValue},";
            }
            var newRepr = objRepr.Remove(objRepr.Length - 1, 1);

            return newRepr;
        }
    }

    /// <summary>
    /// CSV reader. Reads a comma (or other char) delimited file, line by line and parses each line to a new object. Header lines can be handled with the HeaderParser. 
    /// </summary>
    public partial class CsvParser : IDisposable
    {
        /// <summary>
        /// Gets or sets the delimiter.
        /// </summary>
        public char Delimiter { get; }
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath { get; }
        /// <summary>
        /// Gets or sets the stream.
        /// </summary>
        public StreamReader Stream;
        private Queue<string> BufferedLines = new Queue<string>();
        /// <summary>
        /// Returns a boolean representing wether the current line is a Header.
        /// </summary>
        protected Func<string, CsvParser, bool> HeaderParser;
        /// <summary>
        /// Parses a split line to a new object. Applied on every line not a header.
        /// </summary>
        protected Func<string[], CsvParser, Dictionary<string, string>, object> RowParser;
        /// <summary>
        /// Last header row read by the header parser.
        /// </summary>
        protected string Header;
        protected Dictionary<string, string> HeaderData = new Dictionary<string, string>();
        /// <summary>
        /// Gets a value indicating whether done.
        /// </summary>
        public bool IsDone { get { return Stream.Peek() == -1 && BufferedLines.Count == 0; } }
        /// <summary>
        /// Gets a value indicating whether n peek.
        /// </summary>
        public bool CanPeek { get { return Stream.Peek() != -1; } }

        protected CsvParser(string filePath, char delimiter = ',', Func<string[], CsvParser, Dictionary<string, string>, object> rowParser = null, Func<string, CsvParser, bool> headerParser = null)
        {
            FilePath = filePath;
            Stream = new StreamReader(filePath, System.Text.Encoding.UTF8, true);
            Delimiter = delimiter;
            RowParser = rowParser;
            HeaderParser = headerParser;
        }

        private string ReadLine()
        {
            if (BufferedLines.Count > 0)
                return BufferedLines.Dequeue();
            return Stream.ReadLine();
        }

        /// <summary>
        /// Returns the next line to be read without consuming it, thus it will still be read by ReadLine.
        /// </summary>
        /// <returns></returns>
        protected string PeekLine()
        {
            if (BufferedLines.Count > 0)
                return BufferedLines.Peek();

            return PeekNextLine();
        }

        /// <summary>
        /// Returns the next line that has not already been peeked. The line will not be consumed but still will be read by ReadLine.
        /// </summary>
        /// <returns></returns>
        protected string PeekNextLine()
        {
            string line = Stream.ReadLine();
            if (line == null)
                return null;

            BufferedLines.Enqueue(line);
            return line;
        }

        protected string[] ReadRow()
        {
            string line = ReadLine();
            return string.IsNullOrEmpty(line) ? null : line.Split(new char[] { Delimiter });
        }

        protected T ParseRow<T>()
        {
            var row = ReadRow();
            return row == null ? default(T) : (T)RowParser(row, this, HeaderData);
        }

        public List<T> ParseAll<T>(bool skipNull = true, bool breakOnNull = false)
        {
            BeforeParse(typeof(T));

            List<T> results = new List<T>();
            while (!this.IsDone)
            {
                string line = PeekLine();
                bool isHeader = HeaderParser(line, this);
                if (isHeader)
                {
                    while (BufferedLines.Count > 0)
                    {
                        BufferedLines.Dequeue();
                    };
                    continue;
                }

                T parsed;
                try
                {
                    parsed = ParseRow<T>();
                }
                catch (Exception e)
                {
                    throw new ParseException(typeof(T), line.Replace("\t", "  "), e);
                }
                if (parsed == null && skipNull)
                    continue;
                else if (parsed == null && breakOnNull)
                    break;
                results.Add(parsed);
            }

            results = AfterParse(results);

            return results;
        }

        protected virtual void BeforeParse(Type type)
        {
            if (RowParser is null)
                throw new ApplicationException("Row parser was not initialized properly.");
            if (HeaderParser is null)
                throw new ApplicationException("Header parser was not initialized properly.");
        }

        protected virtual List<T> AfterParse<T>(List<T> parsed)
        {
            return parsed;
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
        /// </summary>
        public void Dispose()
        {
            Stream.Dispose();
        }
    }

    /// <summary>
    /// Parsing related exceptions
    /// </summary>
    public partial class ParseException : ApplicationException
    {
        /// <summary>
        /// Gets or sets the type not parsed.
        /// </summary>
        public Type TypeNotParsed;
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class.
        /// </summary>
        /// <param name="typeNotParsed">The result type that failed to parse.</param>
        /// <param name="lineNotParsed">The input line that could not be parsed.</param>
        public ParseException(Type typeNotParsed, string lineNotParsed) : base($"Could not parse line '{lineNotParsed}' to type {typeNotParsed.GetType().FullName}")
        {
            TypeNotParsed = typeNotParsed;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class for a file with no results.
        /// </summary>
        /// <param name="typeNotParsed">The expected result type.</param>
        /// <param name="message">Additional context message.</param>
        /// <param name="path">Path to the results file.</param>
        public ParseException(Type typeNotParsed, string message, string path) : base($"No results read. Are there any results in the file? ({path})" + (string.IsNullOrEmpty(path) ? "" : ". " + message))
        {
            TypeNotParsed = typeNotParsed;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class.
        /// </summary>
        /// <param name="typeNotParsed">the type not parsed.</param>
        /// <param name="lineNotParsed">the line not parsed.</param>
        /// <param name="inner">the inner.</param>
        public ParseException(Type typeNotParsed, string lineNotParsed, Exception inner) : base($"Could not parse line '{lineNotParsed}' to type {typeNotParsed.GetType().FullName}", inner)
        {
            TypeNotParsed = typeNotParsed;
        }
        protected ParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}