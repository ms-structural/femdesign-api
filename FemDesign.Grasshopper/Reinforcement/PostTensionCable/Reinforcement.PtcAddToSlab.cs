// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class PtcAddToSlab : FEM_Design_API_Component
    {
        public PtcAddToSlab() : base("PTC.AddToSlab", "AddToSlab", "Add post-tensioning cables to a slab element.", "FEM-Design", "Reinforcement")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Slab.", GH_ParamAccess.item);
            pManager.AddGenericParameter("PTC", "PTC", "Post-tensioning cables. Item or list.", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Passed slab.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Shells.Slab slab = null;
            if (!DA.GetData(0, ref slab))
            {
                return;
            }

            List<FemDesign.Reinforcement.Ptc> ptc = new List<FemDesign.Reinforcement.Ptc>();
            if (!DA.GetDataList(1, ptc))
            {
                return;
            }

            if (slab == null || ptc == null)
            {
                return;
            }

            var clonedSlab = slab.DeepClone();
            var clonedPtc = ptc.Select(x => x.DeepClone()).ToList();

            clonedSlab = FemDesign.Reinforcement.Ptc.AddPtcToSlab(clonedSlab, clonedPtc, true);

            DA.SetData(0, clonedSlab);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Ptc;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{A3F7B2C1-5D8E-4A9F-B6C2-1E3D4F5A6B7C}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
    }
}
