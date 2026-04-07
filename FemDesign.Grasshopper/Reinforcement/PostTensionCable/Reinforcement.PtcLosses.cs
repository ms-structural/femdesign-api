using Grasshopper.Kernel;

using System;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public class ReinforcementPtcLosses : FEM_Design_API_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public ReinforcementPtcLosses() : base("PtcLosses", "Losses", "Description", "FEM-Design", "Reinforcement")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("CurvatureCoefficient", "CurvatureCoefficient", "Curvature friction coefficient. Default is 0.05.", GH_ParamAccess.item, 0.05);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("WobbleCoefficient", "WobbleCoefficient", "Wobble friction coefficient [1/m]. Default is 0.007.", GH_ParamAccess.item, 0.007);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("AnchorageSetSlip", "AnchorageSetSlip", "Anchorage set slip [mm]. Default is 6.", GH_ParamAccess.item, 6.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("ElasticShortening", "ElasticShortening", "Elastic shortening stress [N/mm2]. Default is 0.", GH_ParamAccess.item, 0.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("CreepStress", "CreepStress", "Creep stress [N/mm2]. Default is 0.", GH_ParamAccess.item, 0.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("ShrinkageStress", "ShrinkageStress", "Shrinkage stress [N/mm2]. Default is 0.", GH_ParamAccess.item, 0.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("RelaxationStress", "RelaxationStress", "Relaxation stress [N/mm2]. Default is 0.", GH_ParamAccess.item, 0.0);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PtcLosses", "Losses", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double curvatureCoefficient = 0.0;
            double wobbleCoefficient = 0.0;
            double anchorageSetSlip = 0.0;
            double elasticShortening = 0.0;
            double creepStress = 0.0;
            double shrinkageStress = 0.0;
            double relaxationStress = 0.0;
            DA.GetData("CurvatureCoefficient", ref curvatureCoefficient);
            DA.GetData("WobbleCoefficient", ref wobbleCoefficient);
            DA.GetData("AnchorageSetSlip", ref anchorageSetSlip);
            DA.GetData("ElasticShortening", ref elasticShortening);
            DA.GetData("CreepStress", ref creepStress);
            DA.GetData("ShrinkageStress", ref shrinkageStress);
            DA.GetData("RelaxationStress", ref relaxationStress);

            var losses = new PtcLosses(curvatureCoefficient, wobbleCoefficient, anchorageSetSlip, elasticShortening, creepStress, shrinkageStress, relaxationStress);

            DA.SetData("PtcLosses", losses);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return FemDesign.Properties.Resources.PtcLosses;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6323fbff-e53a-40dd-b368-8d60f04fec3b"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}