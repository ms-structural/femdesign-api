// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    /// <summary>
    /// Configures the shared FemDesignConnectionHub and exposes a lightweight handle.
    /// </summary>
    public class FemDesignConnectionComponent: FEM_Design_API_Component
    {
        private Guid _handle = Guid.Empty;
        public FemDesignConnectionComponent() : base("FEM-Design.Connection", "Connection", "Create or configure a shared FEM-Design connection. Use it to specify the 'Connection' for the LiveLink components.\n\n" +
            "Note: Removing this component will automatically close the FEM-Design window. To keep FEM-Design open after closing the connection, use the 'Disconnect' component.", CategoryName.Name(), SubCategoryName.Cat8())
        {
            ObjectChanged += FemDesignConnectionComponent_ObjectChanged;
        }

        private void FemDesignConnectionComponent_ObjectChanged(object sender, GH_ObjectChangedEventArgs e)
        {
            if (e.Type is GH_ObjectEventType.Enabled)
            {
                CloseConnection();
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("FEM-Design dir", "FEM-Design dir", "Path to the FEM-Design installation directory.", GH_ParamAccess.item, @"C:\\Program Files\\StruSoft\\FEM-Design 25\\");
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddBooleanParameter("Minimized", "Minimized", "If true, FEM-Design window will open in a minimised mode.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddTextParameter("OutputDir", "OutputDir", "The directory where the script, log and result files will be saved. By default, the files will be written to the same directory as your .gh script.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddBooleanParameter("DeleteOutputFolder", "DeleteOutputFolder", "Delete output directory on disconnect.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "Shared FEM-Design connection handle.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string fdDir = @"C:\\Program Files\\StruSoft\\FEM-Design 25\\";
            DA.GetData("FEM-Design dir", ref fdDir);

            bool minimized = false;
            DA.GetData("Minimized", ref minimized);

            string outputDir = null;
            DA.GetData("OutputDir", ref outputDir);

            bool deleteOutput = false;
            DA.GetData("DeleteOutputFolder", ref deleteOutput);

            // Set working directory similarly to existing component behavior
            bool fileExist = OnPingDocument().IsFilePathDefined;
            if (!fileExist)
            {
                string tempPath = System.IO.Path.GetTempPath();
                System.IO.Directory.SetCurrentDirectory(tempPath);
            }
            else
            {
                var filePath = OnPingDocument().FilePath;
                var currentDir = System.IO.Path.GetDirectoryName(filePath);
                System.IO.Directory.SetCurrentDirectory(currentDir);
            }

            CloseConnection();
            _handle = FemDesignConnectionHub.Create(fdDir, minimized, outputDir, deleteOutput);

            // Emit a handle object to wire downstream
            DA.SetData("Connection", new FemDesign.Grasshopper.FemDesignHubHandle(_handle));
        }

        private void CloseConnection()
        {
            if (_handle != Guid.Empty)
            {
                FemDesignConnectionHub.DisposeAsync(_handle).GetAwaiter().GetResult();
                _handle = Guid.Empty;
            }
        }

        public override void RemovedFromDocument(GH_Document document)
        {
            try
            {
                CloseConnection();
            }
            catch { }

            base.RemovedFromDocument(document);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_Connection;
        public override Guid ComponentGuid => new Guid("{CBDD03EE-9F56-42FC-AA8D-7C45C152EF1C}");
        public override GH_Exposure Exposure => GH_Exposure.primary;
    }
}


