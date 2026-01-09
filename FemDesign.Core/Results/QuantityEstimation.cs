using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using FemDesign.GenericClasses;
using FemDesign.Calculate;
using Newtonsoft.Json;

namespace FemDesign.Results
{
    /// <summary>
    /// Defines the I Quantity Estimation Result interface.
    /// </summary>
    public interface IQuantityEstimationResult : IResult
    {
        /// <summary>
        /// Element name identifier
        /// </summary>
        string Id { get; }
        /// <summary>
        /// Storey identifier
        /// </summary>
        string Storey { get; }
        /// <summary>
        /// Structural element type
        /// </summary>
        string Structure { get; }
        /// <summary>
        /// Material quality identifier
        /// </summary>
        string Quality { get; }

        /// <summary>
        /// Total weight
        /// </summary>
        double TotalWeight { get; }

        /// <summary>
        /// CO2 footprint (A1-A3)
        /// </summary>
        double? CO2Footprint { get; }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, Concrete" result
    /// </summary>
    [Result(typeof(QuantityEstimationConcrete), ListProc.QuantityEstimationConcrete)]
    public partial class QuantityEstimationConcrete : IQuantityEstimationResult
    {
        /// <summary>
        /// Storey identifier
        /// </summary>
        public string Storey { get; }
        /// <summary>
        /// Structural element type
        /// </summary>
        public string Structure { get; }
        /// <summary>
        /// Element name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Material quality identifier
        /// </summary>
        public string Quality { get; }
        /// <summary>
        /// Section
        /// </summary>
        public string Section { get; }
        /// <summary>
        /// Height
        /// </summary>
        public double Height { get; }
        /// <summary>
        /// Width
        /// </summary>
        public double Width { get; }
        /// <summary>
        /// Length||Area Quantity
        /// </summary>
        public double SubTotal { get; }
        /// <summary>
        /// Volume quantity
        /// </summary>
        public double Volume { get; }
        /// <summary>
        /// Total weight
        /// </summary>
        public double TotalWeight { get; }
        /// <summary>
        /// Formwork quantity
        /// </summary>
        public double Formwork { get; }
        /// <summary>
        /// Reinforcement weight per length/area
        /// </summary>
        public double Reinforcement { get; }

        /// <summary>
        /// CO2 footprint (A1-A3)
        /// </summary>
        public double? CO2Footprint { get; }

        [JsonConstructor]
        internal QuantityEstimationConcrete(string storey, string structure, string id, string quality, string section, double height, double width, double subTotal, double volume, double totalWeight, double formwork, double reinforcement, double? c02Footprint)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Section = section;
            Height = height;
            Width = width;
            SubTotal = subTotal;
            Volume = volume;
            TotalWeight = totalWeight;
            Formwork = formwork;
            Reinforcement = reinforcement;
            CO2Footprint = c02Footprint;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Quantity estimation), (?'result'Concrete)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^Quantity estimation, Concrete|^Storey\tStruct\.\tIdentifier\tQuality\tSection/\tHeight\tWidth\tTotal length\[m\]/\tVolume\tTotal weight\tFormwork\tReinforcement\tCO2 footprint \(A1-A3\)|\t*\[.+\]|^TOTAL\t");
            }
        }

        internal static QuantityEstimationConcrete Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string storey = row[0] == "-" ? null : row[0];
            string structure = row[1];
            string id = row[2];
            string quality = row[3];
            string section = row[4];
            double height = row[5] == "" ? 0 : Double.Parse(row[5], CultureInfo.InvariantCulture);
            double width = row[6] == "" ? 0 : Double.Parse(row[6], CultureInfo.InvariantCulture);
            double subTotal = row[7] == "" ? 0 : Double.Parse(row[7], CultureInfo.InvariantCulture);
            double volume = double.Parse(row[8], CultureInfo.InvariantCulture);
            double totalWeight = double.Parse(row[9], CultureInfo.InvariantCulture);
            double formwork = double.Parse(row[10], CultureInfo.InvariantCulture);
            double reinforcement = row[11] == "" ? 0 : Double.Parse(row[11], CultureInfo.InvariantCulture);

            double? co2Footprint;
            if (row[12].Contains("there is no matching value in the CO2 database!"))
            {
                co2Footprint = null;
            }
            else
            {
                co2Footprint = double.Parse(row[12], CultureInfo.InvariantCulture);
            }
            //string test = HeaderData["result"];
            return new QuantityEstimationConcrete(storey, structure, id, quality, section, height, width, subTotal, volume, totalWeight, formwork, reinforcement, co2Footprint);
        }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, Reinforcement" result
    /// </summary>
    [Result(typeof(QuantityEstimationReinforcement), ListProc.QuantityEstimationReinforcement)]
    public partial class QuantityEstimationReinforcement : IQuantityEstimationResult
    {
        /// <summary>
        /// Storey identifier
        /// </summary>
        public string Storey { get; }
        /// <summary>
        /// Structural element type
        /// </summary>
        public string Structure { get; }
        /// <summary>
        /// Element name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Material quality identifier
        /// </summary>
        public string Quality { get; }
        /// <summary>
        /// Diameter
        /// </summary>
        public double Diameter { get; }
        /// <summary>
        /// Total weigt
        /// </summary>
        public double TotalWeight { get; }
        /// <summary>
        /// CO2 footprint (A1-A3)
        /// </summary>
        public double? CO2Footprint { get; }

        [JsonConstructor]
        internal QuantityEstimationReinforcement(string storey, string structure, string id, string quality, double diameter, double totalWeight, double? cO2Footprint)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Diameter = diameter;
            TotalWeight = totalWeight;
            CO2Footprint = cO2Footprint;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Quantity estimation), (?'result'Reinforcement)$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^Quantity estimation, Reinforcement|^Storey\tStruct\.\tIdentifier\tQuality\tDiameter\tTotal weight\tCO2 footprint \(A1-A3\)|\t*\[.+\]|^TOTAL\t");
            }
        }

        internal static QuantityEstimationReinforcement Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string storey = row[0] == "-" ? null : row[0];
            string structure = row[1];
            string id = row[2];
            string quality = row[3];
            double diameter = double.Parse(row[4], CultureInfo.InvariantCulture);
            double totalWeight = double.Parse(row[5], CultureInfo.InvariantCulture);

            double? co2Footprint;
            if (row[6].Contains("there is no matching value in the CO2 database!"))
            {
                co2Footprint = null;
            }
            else
            {
                co2Footprint = double.Parse(row[6], CultureInfo.InvariantCulture);
            }

            return new QuantityEstimationReinforcement(storey, structure, id, quality, diameter, totalWeight, co2Footprint);
        }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, Steel" result
    /// </summary>
    [Result(typeof(QuantityEstimationSteel), ListProc.QuantityEstimationSteel)]
    public partial class QuantityEstimationSteel : IQuantityEstimationResult
    {
        /// <summary>
        /// Storey identifier
        /// </summary>
        public string Storey { get; }
        /// <summary>
        /// Structural element type
        /// </summary>
        public string Structure { get; }
        /// <summary>
        /// Element name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Material quality identifier
        /// </summary>
        public string Quality { get; }
        /// <summary>
        /// Section/Thickness identifier
        /// </summary>
        public string Section { get; }
        /// <summary>
        /// Weight per length [t/m, t/m2]
        /// </summary>
        public double UnitWeight { get; }
        /// <summary>
        /// Length||Area Quantity
        /// </summary>
        public double SubTotal { get; }
        /// <summary>
        /// Total weight
        /// </summary>
        public double TotalWeight { get; }
        /// <summary>
        /// Painted area
        /// </summary>
        public double PaintedArea { get; }
        /// <summary>
        /// CO2 footprint (A1-A3)
        /// </summary>
        public double? CO2Footprint { get; }

        [JsonConstructor]
        internal QuantityEstimationSteel(string storey, string structure, string id, string quality, string section, double unitWeight, double subtotal, double totalWeight, double paintedArea, double? co2Footprint)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Section = section;
            UnitWeight = unitWeight;
            SubTotal = subtotal;
            TotalWeight = totalWeight;
            PaintedArea = paintedArea;
            CO2Footprint = co2Footprint;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Quantity estimation), (?'result'Steel)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^Quantity estimation, Steel|^Storey\tStruct\.\tIdentifier\tQuality\tSection/\tUnit weight\tTotal length\[m\]/\tTotal weight\tPainted area\tCO2 footprint \(A1-A3\)|\t*\[.+\]|^TOTAL\t");
            }
        }

        internal static QuantityEstimationSteel Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string storey = row[0] == "-" ? null : row[0];
            string structure = row[1];
            string id = row[2];
            string quality = row[3];
            string section = row[4];
            double unitWeight = double.Parse(row[5], CultureInfo.InvariantCulture);
            double subtotal = double.Parse(row[6], CultureInfo.InvariantCulture);
            double totalWeight = double.Parse(row[7], CultureInfo.InvariantCulture);
            double paintedArea = double.Parse(row[8], CultureInfo.InvariantCulture);

            double? co2Footprint;
            if (row[9].Contains("there is no matching value in the CO2 database!"))
            {
                co2Footprint = null;
            }
            else
            {
                co2Footprint = double.Parse(row[9], CultureInfo.InvariantCulture);
            }

            return new QuantityEstimationSteel(storey, structure, id, quality, section, unitWeight, subtotal, totalWeight, paintedArea, co2Footprint);
        }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, Timber" result
    /// </summary>
    [Result(typeof(QuantityEstimationTimber), ListProc.QuantityEstimationTimber)]
    public partial class QuantityEstimationTimber : IQuantityEstimationResult
    {
        /// <summary>
        /// Storey identifier
        /// </summary>
        public string Storey { get; }
        /// <summary>
        /// Structural element type
        /// </summary>
        public string Structure { get; }
        /// <summary>
        /// Element name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Material quality identifier
        /// </summary>
        public string Quality { get; }
        /// <summary>
        /// Section/Thickness identifier
        /// </summary>
        public string Section { get; }
        /// <summary>
        /// Weight per length [t/m, t/m2]
        /// </summary>
        public double UnitWeight { get; }
        /// <summary>
        /// Length||Area Quantity
        /// </summary>
        public double SubTotal { get; }
        /// <summary>
        /// Total weight
        /// </summary>
        public double TotalWeight { get; }
        /// <summary>
        /// Painted area
        /// </summary>
        public double PaintedArea { get; }
        /// <summary>
        /// CO2 footprint (A1-A3)
        /// </summary>
        public double? CO2Footprint { get; }

        [JsonConstructor]
        internal QuantityEstimationTimber(string storey, string structure, string id, string quality, string section, double unitWeight, double subtotal, double totalWeight, double paintedArea, double? co2Footprint)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Section = section;
            UnitWeight = unitWeight;
            SubTotal = subtotal;
            TotalWeight = totalWeight;
            PaintedArea = paintedArea;
            CO2Footprint = co2Footprint;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Quantity estimation), (?'result'Timber)$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^Quantity estimation, Timber|^Storey\tStruct\.\tIdentifier\tQuality\tSection/\tUnit weight\tTotal length\[m\]/\tTotal weight\tPainted area\tCO2 footprint \(A1-A3\)|\t*\[.+\]|^TOTAL\t");
            }
        }

        internal static QuantityEstimationTimber Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string storey = row[0] == "-" ? null : row[0];
            string structure = row[1];
            string id = row[2];
            string quality = row[3];
            string section = row[4];
            double unitWeight = double.Parse(row[5], CultureInfo.InvariantCulture);
            double subtotal = double.Parse(row[6], CultureInfo.InvariantCulture);
            double totalWeight = double.Parse(row[7], CultureInfo.InvariantCulture);
            double paintedArea = double.Parse(row[8], CultureInfo.InvariantCulture);

            double? co2Footprint;
            if ( row[9].Contains("there is no matching value in the CO2 database!") )
            {
                co2Footprint = null;
            }
            else
            {
                co2Footprint = double.Parse(row[9], CultureInfo.InvariantCulture);
            }
            return new QuantityEstimationTimber(storey, structure, id, quality, section, unitWeight, subtotal, totalWeight, paintedArea, co2Footprint);
        }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, Timber panel" result
    /// </summary>
    [Result(typeof(QuantityEstimationTimberPanel), ListProc.QuantityEstimationTimberPanel)]
    public partial class QuantityEstimationTimberPanel : IQuantityEstimationResult
    {
        /// <summary>
        /// Storey identifier
        /// </summary>
        public string Storey { get; }
        /// <summary>
        /// Structural element type
        /// </summary>
        public string Structure { get; }
        /// <summary>
        /// Element name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Material quality identifier
        /// </summary>
        public string Quality { get; }
        /// <summary>
        /// Thickness [mm]
        /// </summary>
        public double Thickness { get; }
        /// <summary>
        /// Panel type
        /// </summary>
        public string PanelType { get; }
        /// <summary>
        /// Length of panel
        /// </summary>
        public double Length { get; }
        /// <summary>
        /// Width of panel
        /// </summary>
        public double Width { get; }
        /// <summary>
        /// Area of panel
        /// </summary>
        public double Area { get; }
        /// <summary>
        /// Weight per unit
        /// </summary>
        public double Weight { get; }
        /// <summary>
        /// Number of panels of type
        /// </summary>
        public int Count { get; }
        /// <summary>
        /// Total weight
        /// </summary>
        public double TotalWeight { get; }
        /// <summary>
        /// CO2 footprint (A1-A3)
        /// </summary>
        public double? CO2Footprint { get; }

        [JsonConstructor]
        internal QuantityEstimationTimberPanel(string storey, string structure, string id, string quality, double thickness, string panelType, double length, double width, double area, double weight, int count, double totalWeight, double? cO2Footprint)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Thickness = thickness;
            PanelType = panelType;
            Length = length;
            Width = width;
            Area = area;
            Weight = weight;
            Count = count;
            TotalWeight = totalWeight;
            CO2Footprint = cO2Footprint;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Quantity estimation), (?'result'Timber panel)$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^Quantity estimation, Timber panel|^Storey\tStruct\.\tIdentifier\tQuality\tThickness\tPanel type\tLength\tWidth\tArea\tWeight\tPcs\tTotal weight\tCO2 footprint \(A1-A3\)|\t*\[.+\]|^TOTAL\t");
            }
        }

        internal static QuantityEstimationTimberPanel Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string storey = row[0] == "-" ? null : row[0];
            string structure = row[1];
            string id = row[2];
            string quality = row[3];
            double thickness = double.Parse(row[4], CultureInfo.InvariantCulture);
            string panelType = row[5];
            double length = double.Parse(row[6], CultureInfo.InvariantCulture);
            double width = double.Parse(row[7], CultureInfo.InvariantCulture);
            double area = double.Parse(row[8], CultureInfo.InvariantCulture);
            double weight = double.Parse(row[9], CultureInfo.InvariantCulture);
            int pcs = int.Parse(row[10], CultureInfo.InvariantCulture);
            double totalWeight = double.Parse(row[11], CultureInfo.InvariantCulture);

            double? co2Footprint;
            if (row[12].Contains("there is no matching value in the CO2 database!"))
            {
                co2Footprint = null;
            }
            else
            {
                co2Footprint = double.Parse(row[12], CultureInfo.InvariantCulture);
            }
            return new QuantityEstimationTimberPanel(storey, structure, id, quality, thickness, panelType, length, width, area, weight, pcs, totalWeight, co2Footprint);
        }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, Timber" result
    /// </summary>
    [Result(typeof(QuantityEstimationProfiledPlate), ListProc.QuantityEstimationProfiledPanel)]
    public partial class QuantityEstimationProfiledPlate : IQuantityEstimationResult
    {
        /// <summary>
        /// Storey identifier
        /// </summary>
        public string Storey { get; }
        /// <summary>
        /// Structural element type
        /// </summary>
        public string Structure { get; }
        /// <summary>
        /// Element name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Material quality identifier
        /// </summary>
        public string Quality { get; }
        /// <summary>
        /// Section/Thickness identifier
        /// </summary>
        public string Section { get; }
        /// <summary>
        /// Thickness
        /// </summary>
        public double Thickness { get; }
        /// <summary>
        /// Panel type
        /// </summary>
        public string Type { get; }
        /// <summary>
        /// Length of plate
        /// </summary>
        public double Length { get; }
        /// <summary>
        /// Width of plate
        /// </summary>
        public double Width { get; }
        /// <summary>
        /// Area of the plate
        /// </summary>
        public double Area { get; }
        /// <summary>
        /// Weight of single plate
        /// </summary>
        public double Weight { get; }
        /// <summary>
        /// Count/Sum of sections
        /// </summary>
        public int Count { get; }
        /// <summary>
        /// Total weight of all plates
        /// </summary>
        public double TotalWeight { get; }
        /// <summary>
        /// CO2 footprint (A1-A3)
        /// </summary>
        public double? CO2Footprint { get; }

        [JsonConstructor]
        internal QuantityEstimationProfiledPlate(string id, string storey, string structure, string quality, string section, double thickness, string type, double length, double width, double area, double weight,  int count, double totalWeight, double? cO2Footprint)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Section = section;
            Thickness = thickness;
            Type = type;
            Length = length;
            Width = width;
            Area = area;
            Weight = weight;
            Count = count;
            TotalWeight = totalWeight;
            CO2Footprint = cO2Footprint;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Quantity estimation), (?'result'Profiled panel)$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^Quantity estimation, Profiled panel$|^Storey\tStruct\.\tIdentifier\tQuality\tSection\tThickness\tPanel type\tLength\tWidth\tArea\tWeight\tPcs\tTotal weight\tCO2 footprint \(A1-A3\)|\t*\[.+\]|^TOTAL\t");
            }
        }

        internal static QuantityEstimationProfiledPlate Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string storey = row[0] == "-" ? null : row[0];
            string structure = row[1];
            string id = row[2];
            string quality = row[3];
            string section = row[4];
            double thickness = double.Parse(row[5], CultureInfo.InvariantCulture);
            string type = row[6];
            double length = double.Parse(row[7], CultureInfo.InvariantCulture);
            double width = double.Parse(row[8], CultureInfo.InvariantCulture);
            double area = double.Parse(row[9], CultureInfo.InvariantCulture);
            double weight = double.Parse(row[10], CultureInfo.InvariantCulture);
            int count = int.Parse(row[11], CultureInfo.InvariantCulture);
            double totalWeight = double.Parse(row[12], CultureInfo.InvariantCulture);

            double? co2Footprint;
            if (row[13].Contains("there is no matching value in the CO2 database!"))
            {
                co2Footprint = null;
            }
            else
            {
                co2Footprint = double.Parse(row[13], CultureInfo.InvariantCulture);
            }

            return new QuantityEstimationProfiledPlate(storey, structure, id, quality, section, thickness, type, length, width, area, weight, count, totalWeight, co2Footprint);
        }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, Concrete" result
    /// </summary>
    [Result(typeof(QuantityEstimationMasonry), ListProc.QuantityEstimationMasonry)]
    public partial class QuantityEstimationMasonry : IQuantityEstimationResult
    {
        /// <summary>
        /// Storey identifier
        /// </summary>
        public string Storey { get; }
        /// <summary>
        /// Structural element type
        /// </summary>
        public string Structure { get; }
        /// <summary>
        /// Element name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Material quality identifier
        /// </summary>
        public string Quality { get; }
        /// <summary>
        /// Thickness
        /// </summary>
        public double Thickness { get; }
        /// <summary>
        /// Weight per length or area unit
        /// </summary>
        public double UnitWeight { get; }
        /// <summary>
        /// LArea Quantity
        /// </summary>
        public double TotalArea { get; }
        /// <summary>
        /// Total weight
        /// </summary>
        public double TotalWeight { get; }
        /// <summary>
        /// CO2 footprint (A1-A3)
        /// </summary>
        public double? CO2Footprint { get; }

        [JsonConstructor]
        internal QuantityEstimationMasonry(string storey, string structure, string id, string quality, double thickness, double unitWeight, double totalArea, double totalWeight, double? cO2Footprint)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Thickness = thickness;
            UnitWeight = unitWeight;
            TotalArea = totalArea;
            TotalWeight = totalWeight;
            CO2Footprint = cO2Footprint;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Quantity estimation), (?'result'Masonry)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^Quantity estimation, Masonry|^Storey\tStruct\.\tIdentifier\tQuality\tThickness\tUnit weight\tTotal area\tTotal weight\tCO2 footprint \(A1-A3\)|\t*\[.+\]|^TOTAL\t");
            }
        }

        internal static QuantityEstimationMasonry Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string storey = row[0] == "-" ? null : row[0];
            string structure = row[1];
            string id = row[2];
            string quality = row[3];
            double thickness = double.Parse(row[4], CultureInfo.InvariantCulture);
            double unitWeight = double.Parse(row[5], CultureInfo.InvariantCulture);
            double totalArea = double.Parse(row[6], CultureInfo.InvariantCulture);
            double totalWeight = double.Parse(row[7], CultureInfo.InvariantCulture);

            double? co2Footprint;
            if (row[8].Contains("there is no matching value in the CO2 database!"))
            {
                co2Footprint = null;
            }
            else
            {
                co2Footprint = double.Parse(row[8], CultureInfo.InvariantCulture);
            }

            return new QuantityEstimationMasonry(storey, structure, id, quality, thickness, unitWeight, totalArea, totalWeight, co2Footprint);
        }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, General" result
    /// </summary>
    [Result(typeof(QuantityEstimationGeneral), ListProc.QuantityEstimationGeneral)]
    public partial class QuantityEstimationGeneral : IQuantityEstimationResult
    {
        /// <summary>
        /// Storey identifier
        /// </summary>
        public string Storey { get; }
        /// <summary>
        /// Structural element type
        /// </summary>
        public string Structure { get; }
        /// <summary>
        /// Element name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Material quality identifier
        /// </summary>
        public string Quality { get; }
        /// <summary>
        /// Section/Thickness identifier
        /// </summary>
        public string Section { get; }
        /// <summary>
        /// Weight per length [t/m, t/m2]
        /// </summary>
        public double UnitWeight { get; }
        /// <summary>
        /// Length||Area Quantity
        /// </summary>
        public double SubTotal { get; }
        /// <summary>
        /// Total weight
        /// </summary>
        public double TotalWeight { get; }
        /// <summary>
        /// Painted area
        /// </summary>
        public double PaintedArea { get; }
        /// <summary>
        /// CO2 footprint (A1-A3)
        /// </summary>
        public double? CO2Footprint { get; }

        [JsonConstructor]
        internal QuantityEstimationGeneral(string storey, string structure, string id, string quality, string section, double unitWeight, double subtotal, double totalWeight, double paintedArea, double? cO2Footprint)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Section = section;
            UnitWeight = unitWeight;
            SubTotal = subtotal;
            TotalWeight = totalWeight;
            PaintedArea = paintedArea;
            CO2Footprint = cO2Footprint;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The result.</returns>
        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Quantity estimation), (?'result'General)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^Quantity estimation, General|^Storey\tStruct\.\tIdentifier\tQuality\tSection/\tUnit weight\tTotal length\[m\]/\tTotal weight\tPainted area\tCO2 footprint \(A1-A3\)|\t*\[.+\]|^\t*\[.+\]|^TOTAL\t");
            }
        }

        internal static QuantityEstimationGeneral Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string storey = row[0] == "-" ? null : row[0];
            string structure = row[1];
            string id = row[2];
            string quality = row[3];
            string section = row[4];
            double unitWeight = double.Parse(row[5], CultureInfo.InvariantCulture);
            double subtotal = double.Parse(row[6], CultureInfo.InvariantCulture);
            double totalWeight = double.Parse(row[7], CultureInfo.InvariantCulture);
            double paintedArea = double.Parse(row[8], CultureInfo.InvariantCulture);

            double? co2Footprint;
            if (row[9].Contains("there is no matching value in the CO2 database!"))
            {
                co2Footprint = null;
            }
            else
            {
                co2Footprint = double.Parse(row[9], CultureInfo.InvariantCulture);
            }

            return new QuantityEstimationGeneral(storey, structure, id, quality, section, unitWeight, subtotal, totalWeight, paintedArea, co2Footprint);
        }
    }
}
