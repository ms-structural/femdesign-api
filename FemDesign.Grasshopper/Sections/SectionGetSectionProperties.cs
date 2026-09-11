// https://strusoft.com/
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using FemDesign.Sections;
using FemDesign.Reinforcement;
using System.Threading.Tasks;
using FemDesign.Results;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using FemDesign.Loads;
using Grasshopper.Kernel.Special;

namespace FemDesign.Grasshopper
{
    public class SectionGetSetionProperties : FEM_Design_API_Component
    {
        public SectionGetSetionProperties() : base("Section.Properties", "SecProps", "Retrieve geometric properties for cross-sections.", CategoryName.Name(), SubCategoryName.Cat4b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Sections", "Sections", "Section list.", GH_ParamAccess.list);
            pManager.AddTextParameter("Units", "Units", "Units:\nmm\ncm\ndm\nm\ninch\nfeet\nyd", GH_ParamAccess.item, "mm");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_DoubleParam("Height", "h", "Section height.");
            pManager.Register_DoubleParam("Width", "w", "Section width.");
            pManager.Register_DoubleParam("Area", "A", "Section area.");
            pManager.Register_DoubleParam("Iy", "Iy", "Moment of inertia about the local y-axis.");
            pManager.Register_DoubleParam("Iz", "Iz", "Moment of inertia about the local z-axis.");
            pManager.Register_DoubleParam("Wy", "Wy", "Section modulus about the local y-axis.");
            pManager.Register_DoubleParam("Wz", "Wz", "Section modulus about the local z-axis.");
            pManager.Register_DoubleParam("It", "It", "Torsional moment of inertia.");
            pManager.Register_GenericParam("General", "Gen", "General section properties.\n" +
                "0 - Height\n1 - Width\n2 - A\n3 - P\n4 - A/P\n5 - Ys\n6 - Zs\n7 - Iy\n8 - Wy\n9 - ez.max\n10 - ez.min\n11 - iy\n12 - Sy\n" +
                "13 - Iz\n14 - Wz\n15 - ey.max\n16 - ey.min\n17 - iz\n18 - Sz\n19 - It\n20 - WSVt\n21 - Iomega\n22 - Iyz\n23 - zomega");
            pManager.Register_GenericParam("Principal 1", "Principal 1", "Section properties computed around the first principal axis.\n" +
                "0 - alfa1\n1 - I1\n2 - W1.min\n3 - W1.max\n4 - e2.max\n5 - e2.min\n6 - i1\n7 - S1\n8 - S01\n9 - c1\n10 - rho1\n11 - z2");
            pManager.Register_GenericParam("Principal 2", "Principal 2", "Section properties computed around the second principal axis.\n" +
                "0 - alfa2\n1 - I2\n2 - W2.min\n3 - W2.max\n4 - e1.max\n5 - e1.min\n6 - i2\n7 - S2\n8 - S02\n9 - c2\n10 - rho2\n11 - z1");
            pManager.Register_StringParam("SectionNames", "Names", "Section names.");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get inputs
            var sections = new List<Section>();
            if (!DA.GetDataList(0, sections)) { return; }

            string sectionalData = "mm";
            DA.GetData(1, ref sectionalData);

            SectionalData unit;
            if (!Enum.TryParse(sectionalData, true, out unit))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"'{sectionalData}' is not a valid unit. Use mm, cm, dm, m, inch, feet or yd.");
                return;
            }


            #region Set Current Directory
            bool fileExist = OnPingDocument().IsFilePathDefined;
            if (!fileExist)
            {
                // hops issue
                //var folderPath = System.IO.Directory.GetCurrentDirectory();
                string tempPath = System.IO.Path.GetTempPath();
                System.IO.Directory.SetCurrentDirectory(tempPath);
            }
            else
            {
                var filePath = OnPingDocument().FilePath;
                var currentDir = System.IO.Path.GetDirectoryName(filePath);
                System.IO.Directory.SetCurrentDirectory(currentDir);
            }
            #endregion



            // Create Task to get section properties
            List<Results.SectionProperties> secProps = new List<Results.SectionProperties>();
            var t = Task.Run(() =>
            {
                secProps = sections.GetSectionProperties(unit);
            });

            t.ConfigureAwait(false);
            try
            {
                t.Wait();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

            List<string> genPropNames = new List<string>()
            {
                nameof(Results.SectionProperties.Height),
                nameof(Results.SectionProperties.Width),
                nameof(Results.SectionProperties.A),
                nameof(Results.SectionProperties.P),
                nameof(Results.SectionProperties.AP),
                nameof(Results.SectionProperties.Ys),
                nameof(Results.SectionProperties.Zs),
                nameof(Results.SectionProperties.Iy),
                nameof(Results.SectionProperties.Wy),
                nameof(Results.SectionProperties.ezmax),
                nameof(Results.SectionProperties.ezmin),
                nameof(Results.SectionProperties.iy),
                nameof(Results.SectionProperties.Sy),
                nameof(Results.SectionProperties.Iz),
                nameof(Results.SectionProperties.Wz),
                nameof(Results.SectionProperties.eymax),
                nameof(Results.SectionProperties.eymin),
                nameof(Results.SectionProperties.iz),
                nameof(Results.SectionProperties.Sz),
                nameof(Results.SectionProperties.It),
                nameof(Results.SectionProperties.WSVt),
                nameof(Results.SectionProperties.Iomega),
                nameof(Results.SectionProperties.Iyz),
                nameof(Results.SectionProperties.zomega)
            };
            List<string> firstPropNames = new List<string>()
            {
                nameof(Results.SectionProperties.alfa1),
                nameof(Results.SectionProperties.I1),
                nameof(Results.SectionProperties.W1min),
                nameof(Results.SectionProperties.W1max),
                nameof(Results.SectionProperties.e2max),
                nameof(Results.SectionProperties.e2min),
                nameof(Results.SectionProperties.i1),
                nameof(Results.SectionProperties.S1),
                nameof(Results.SectionProperties.S01),
                nameof(Results.SectionProperties.c1),
                nameof(Results.SectionProperties.rho1),
                nameof(Results.SectionProperties.z2)
            };
            List<string> secondPropNames = new List<string>()
            {
                nameof(Results.SectionProperties.alfa2),
                nameof(Results.SectionProperties.I2),
                nameof(Results.SectionProperties.W2min),
                nameof(Results.SectionProperties.W2max),
                nameof(Results.SectionProperties.e1max),
                nameof(Results.SectionProperties.e1min),
                nameof(Results.SectionProperties.i2),
                nameof(Results.SectionProperties.S2),
                nameof(Results.SectionProperties.S02),
                nameof(Results.SectionProperties.c2),
                nameof(Results.SectionProperties.rho2),
                nameof(Results.SectionProperties.z1)
            };

            PropertyInfo[] genProps = genPropNames.Select(n => typeof(Results.SectionProperties).GetProperty(n)).ToArray();
            PropertyInfo[] firstProps = firstPropNames.Select(n => typeof(Results.SectionProperties).GetProperty(n)).ToArray();
            PropertyInfo[] secondProps = secondPropNames.Select(n => typeof(Results.SectionProperties).GetProperty(n)).ToArray();
            List<PropertyInfo[]> propList = new List<PropertyInfo[]>() { genProps, firstProps, secondProps };

            List<DataTree<double>> trees = new List<DataTree<double>>() { };
            for (int i = 0; i < propList.Count; i++)
            {
                var tree = new DataTree<double>();
                int path = 0;
                foreach(var sec in secProps)
                {
                    var values = propList[i].Select(p => (double)p.GetValue(sec));
                    tree.AddRange(values, new GH_Path(path));
                    path++;
                }
                trees.Add(tree);
            }

            DataTree<double> general = trees[0];
            DataTree<double> firstP = trees[1];
            DataTree<double> secondP = trees[2];

            // Set outputs
            DA.SetDataList("Height", secProps.Select(s => s.Height).ToList());
            DA.SetDataList("Width", secProps.Select(s => s.Width).ToList());
            DA.SetDataList("Area", secProps.Select(s => s.A).ToList());
            DA.SetDataList("Iy", secProps.Select(s => s.Iy).ToList());
            DA.SetDataList("Iz", secProps.Select(s => s.Iz).ToList());
            DA.SetDataList("Wy", secProps.Select(s => s.Wy).ToList());
            DA.SetDataList("Wz", secProps.Select(s => s.Wz).ToList());
            DA.SetDataList("It", secProps.Select(s => s.It).ToList());
            DA.SetDataTree(8, general);
            DA.SetDataTree(9, firstP);
            DA.SetDataTree(10, secondP);
            DA.SetDataList("SectionNames", secProps.Select(s => s.Section).ToList());
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SectionGetSectionProperties;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{3AFD6834-3CF0-4F1E-AE3A-C4A1C796420C}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}