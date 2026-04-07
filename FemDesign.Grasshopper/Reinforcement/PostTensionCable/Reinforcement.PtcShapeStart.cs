using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;
using FemDesign.Reinforcement;
namespace FemDesign.Grasshopper
{
    public class PtcShapeStartComponent : FEM_Design_API_Component
    {
        public PtcShapeStartComponent() : base("PtcShapeStart", "PtcStart", "FemDesign.Reinforcement.PtcShapeStart", "FEM-Design", "Reinforcement")
        {
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("z'", "z'", "Height [m]. Default is 0.", GH_ParamAccess.item, 0.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Tangent", "Tangent", "Tangent of cable at start. Default is 0.", GH_ParamAccess.item, 0.0);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Start", "Start", "FemDesign.Reinforcement.PtcShapeStart", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double z = 0.0;
            DA.GetData("z'", ref z);
            double tangent = 0.0;
            DA.GetData("Tangent", ref tangent);

            PtcShapeStart start = new PtcShapeStart(z, tangent);

            DA.SetData("Start", start);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.PtcShapeStart;

        public override Guid ComponentGuid => new Guid("a3c7f499-f956-414f-b2b5-104ac864ebf3");

        public override GH_Exposure Exposure => GH_Exposure.quinary;

    }
}