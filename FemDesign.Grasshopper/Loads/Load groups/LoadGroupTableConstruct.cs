// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using FemDesign.Loads;

namespace FemDesign.Grasshopper
{
    public class LoadGroupTableConstruct : FEM_Design_API_Component
    {
        public LoadGroupTableConstruct() : base("LoadGroupTable.Construct", "LGTable", "Construct a LoadGroupTable from a list of LoadGroups.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadGroups", "LoadGroups", "List of LoadGroups to include in the LoadGroupTable.", GH_ParamAccess.list);
            pManager.AddTextParameter("CombinationMethod", "CombMethod", "Simple combination method for the load group table.\nConnect 'ValueList' to get the options.", GH_ParamAccess.item, "EN 1990 6.4.3(6.10)");
            pManager[1].Optional = true;
            pManager.AddTextParameter("CustomTableCsv", "CsvPath", "Path to CSV file for custom combination method (no header).\nRequired when CombinationMethod is 'custom'.\n\nCSV format: name,type,LG_1_Status,LG_1_Data,...\nFor temporary groups: status,indicator,data (3 columns)\nFor other groups: status,data (2 columns)", GH_ParamAccess.item);
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadGroupTable", "LGTable", "LoadGroupTable.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get load groups
            List<ModelGeneralLoadGroup> loadGroups = new List<ModelGeneralLoadGroup>();
            if (!DA.GetDataList(0, loadGroups)) { return; }

            if (loadGroups == null || loadGroups.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No load groups provided.");
                return;
            }

            // Get combination method
            string combinationMethodStr = "EN 1990 6.4.3(6.10)";
            DA.GetData(1, ref combinationMethodStr);

            LoadCombinationMethod combinationMethod = FemDesign.GenericClasses.EnumParser.Parse<LoadCombinationMethod>(combinationMethodStr);

            // Get CSV path (optional, required for custom method)
            string csvPath = null;
            DA.GetData(2, ref csvPath);

            // Validate custom method requires CSV
            if (combinationMethod == LoadCombinationMethod.Custom)
            {
                if (string.IsNullOrEmpty(csvPath))
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "CSV file path is required when using 'custom' combination method.");
                    return;
                }

                try
                {
                    // Create LoadGroupTable with custom combination from CSV
                    LoadGroupTable loadGroupTable = LoadGroupTable.FromCsv(loadGroups, csvPath);
                    DA.SetData(0, loadGroupTable);
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ex.Message);
                    return;
                }
                catch (Exception ex)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Error parsing CSV: {ex.Message}");
                    return;
                }
            }
            else
            {
                // Create standard LoadGroupTable
                LoadGroupTable loadGroupTable = new LoadGroupTable(loadGroups, combinationMethod);
                DA.SetData(0, loadGroupTable);
            }
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 1, new List<string>
            {
                "EN 1990 6.4.3(6.10)",
                "EN 1990 6.4.3(6.10.a, b)",
                "custom"
            }, null, GH_ValueListMode.DropDown);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.LoadGroup;
        public override Guid ComponentGuid => new Guid("{B4A8F2C1-3D5E-4F6A-9B7C-8E2D1A0F5C3E}");
        public override GH_Exposure Exposure => GH_Exposure.quarternary;
    }
}

