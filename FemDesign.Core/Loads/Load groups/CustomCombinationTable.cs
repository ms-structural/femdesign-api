// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StruSoft.Interop.StruXml.Data;

namespace FemDesign.Loads
{
    /// <summary>
    /// Defines the type of load group.
    /// </summary>
    public enum LoadGroupType
    {
        Permanent,
        Temporary,
        Accidental,
        Stress,
        Seismic
    }

    /// <summary>
    /// Helper class to parse a CSV file and create custom combination table data.
    /// CSV format (no header):
    /// name,type,LG_1_Status,LG_1_Data,LG_2_Status,LG_2_Indicator,LG_2_Data,...
    /// 
    /// Where:
    /// - name: Combination name (e.g., "EC 6.10.a Sup 1")
    /// - type: Limit state (ultimate, characteristic, accidental, seismic, quasi-permanent, frequent)
    /// - For permanent/accidental/stress groups: status,data (2 columns)
    /// - For temporary groups: status,indicator,data (3 columns)
    /// </summary>
    public class CustomCombinationTable
    {
        /// <summary>
        /// List of combination records for the main load_group_table custom_table.
        /// </summary>
        public List<Ldgrp_rec_type> MainTableRecords { get; private set; }

        /// <summary>
        /// Dictionary mapping load group index to its custom table records.
        /// </summary>
        public Dictionary<int, List<object>> LoadGroupRecords { get; private set; }

        /// <summary>
        /// Parse a CSV file to create custom combination table data.
        /// </summary>
        /// <param name="csvFilePath">Path to the CSV file (no header).</param>
        /// <param name="loadGroups">List of load groups in order.</param>
        public static CustomCombinationTable FromCsv(string csvFilePath, List<ModelGeneralLoadGroup> loadGroups)
        {
            if (!File.Exists(csvFilePath))
                throw new FileNotFoundException($"CSV file not found: {csvFilePath}");

            var lines = File.ReadAllLines(csvFilePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToList();

            if (lines.Count == 0)
                throw new ArgumentException("CSV file is empty.");

            var result = new CustomCombinationTable
            {
                MainTableRecords = new List<Ldgrp_rec_type>(),
                LoadGroupRecords = new Dictionary<int, List<object>>()
            };

            // Initialize load group record lists
            for (int i = 0; i < loadGroups.Count; i++)
            {
                result.LoadGroupRecords[i] = new List<object>();
            }

            // Determine column structure based on load group types
            var loadGroupTypes = loadGroups.Select(GetLoadGroupType).ToList();

            // Calculate expected column count
            int expectedColumns = 2; // name + type
            foreach (var lgType in loadGroupTypes)
            {
                expectedColumns += (lgType == LoadGroupType.Temporary) ? 3 : 2;
            }

            foreach (var line in lines)
            {
                var columns = ParseCsvLine(line);
                
                if (columns.Length < expectedColumns)
                    throw new ArgumentException($"CSV line has {columns.Length} columns but expected {expectedColumns}. Line: {line}");

                int colIndex = 0;

                // Column 0: name
                string name = columns[colIndex++];

                // Column 1: limit_state (type)
                string limitStateStr = columns[colIndex++];
                var limitState = ParseLimitState(limitStateStr);

                // Add main table record
                result.MainTableRecords.Add(new Ldgrp_rec_type
                {
                    Name = name,
                    Limit_state = limitState
                });

                // Parse load group columns
                for (int lgIndex = 0; lgIndex < loadGroups.Count; lgIndex++)
                {
                    var lgType = loadGroupTypes[lgIndex];
                    string statusStr = columns[colIndex++];
                    var status = ParseStatus(statusStr);

                    if (lgType == LoadGroupType.Temporary)
                    {
                        // Temporary: status, indicator, data
                        string indicatorStr = columns[colIndex++];
                        var indicator = ParseIndicator(indicatorStr);
                        string data = columns[colIndex++];

                        result.LoadGroupRecords[lgIndex].Add(new Temporary_load_groupRecord
                        {
                            S = status,
                            I = indicator,
                            Data = data
                        });
                    }
                    else if (lgType == LoadGroupType.Permanent)
                    {
                        // Permanent: status, data
                        string data = columns[colIndex++];
                        result.LoadGroupRecords[lgIndex].Add(new Permanent_load_groupRecord
                        {
                            S = status,
                            Data = data
                        });
                    }
                    else if (lgType == LoadGroupType.Accidental)
                    {
                        // Accidental: status, data
                        string data = columns[colIndex++];
                        result.LoadGroupRecords[lgIndex].Add(new Accidental_load_groupRecord
                        {
                            S = status,
                            Data = data
                        });
                    }
                    else if (lgType == LoadGroupType.Stress)
                    {
                        // Stress: status, data
                        string data = columns[colIndex++];
                        result.LoadGroupRecords[lgIndex].Add(new Stress_load_groupRecord
                        {
                            S = status,
                            Data = data
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Determines the load group type from a ModelGeneralLoadGroup.
        /// </summary>
        public static LoadGroupType GetLoadGroupType(ModelGeneralLoadGroup group)
        {
            if (group.ModelLoadGroupPermanent != null) return LoadGroupType.Permanent;
            if (group.ModelLoadGroupTemporary != null) return LoadGroupType.Temporary;
            if (group.AccidentalLoadGroup != null) return LoadGroupType.Accidental;
            if (group.StressLoadGroup != null) return LoadGroupType.Stress;
            if (group.SeismicLoadGroup != null) return LoadGroupType.Seismic;
            throw new ArgumentException($"Unknown load group type for group: {group.Name}");
        }

        private static string[] ParseCsvLine(string line)
        {
            // Simple CSV parsing (doesn't handle quoted commas)
            return line.Split(',').Select(s => s.Trim()).ToArray();
        }

        private static Ldcomblimitstate ParseLimitState(string value)
        {
            var normalized = value.ToLower().Replace("-", "").Replace("_", "").Replace(" ", "");
            
            // FEM-Design abbreviations
            if (normalized == "u")
                return Ldcomblimitstate.Ultimate;
            if (normalized == "ua")
                return Ldcomblimitstate.Accidental;
            if (normalized == "us")
                return Ldcomblimitstate.Seismic;
            if (normalized == "sc")
                return Ldcomblimitstate.Characteristic;
            if (normalized == "sf")
                return Ldcomblimitstate.Frequent;
            if (normalized == "sq")
                return Ldcomblimitstate.Quasipermanent;
            
            throw new ArgumentException($"Unknown limit state: {value}. Valid values: ultimate (U), accidental (Ua), seismic (Us), characteristic (Sc), frequent (Sf), quasi-permanent (Sq)");
        }

        private static Method_ss ParseStatus(string value)
        {
            var normalized = value.ToLower();
            
            if (normalized == "mandatory" || normalized == "m")
                return Method_ss.Mandatory;
            if (normalized == "optional" || normalized == "o")
                return Method_ss.Optional;
            if (normalized == "deactivated" || normalized == "d")
                return Method_ss.Deactivated;
            
            throw new ArgumentException($"Unknown status: {value}. Valid values: mandatory (M), optional (O), deactivated (D/U)");
        }

        private static Method_is ParseIndicator(string value)
        {
            var normalized = value.ToLower();
            
            if (normalized == "general" || normalized == "g")
                return Method_is.General;
            if (normalized == "highlighted" || normalized == "hl")
                return Method_is.Highlighted;
            if (normalized == "simultaneous" || normalized == "s")
                return Method_is.Simultaneous;
            
            throw new ArgumentException($"Unknown indicator: {value}. Valid values: general (G), highlighted (H/HL), simultaneous (S)");
        }
    }
}
