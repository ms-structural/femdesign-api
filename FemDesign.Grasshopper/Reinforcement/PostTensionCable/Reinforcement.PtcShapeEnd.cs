using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public class PtcShapeEndComponent : FEM_Design_API_Component
    {
        public PtcShapeEndComponent() : base("PtcShapeEnd", "PtcEnd", "FemDesign.Reinforcement.PtcShapeEnd", "FEM-Design", "Reinforcement")
        {
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("z'", "z'", "Height [m]. Default is 0.", GH_ParamAccess.item, 0.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Tangent", "Tangent", "Tangent of cable at end. Default is 0.", GH_ParamAccess.item, 0.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("PriorInflection x'", "PriorInflection x'", "Parameter of prior inflection point. Parameter between 0-1 along parent element. Optional", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("End", "End", "FemDesign.Reinforcement.PtcShapeEnd", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double z = 0.0;
            if (!DA.GetData("z'", ref z)) return;

            double tangent = 0.0;
            if (!DA.GetData("Tangent", ref tangent)) return;

            double? priorInflectionParam = null;
            DA.GetData("PriorInflection x'", ref priorInflectionParam);
            
            PtcShapeEnd end = new PtcShapeEnd(z, tangent, priorInflectionParam);

            DA.SetData("End", end);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.PtcShapeEnd;

        public override Guid ComponentGuid => new Guid("0c0d9f85-c235-4eea-84c9-a6fe15603581");

        public override GH_Exposure Exposure => GH_Exposure.quinary;

    }
}