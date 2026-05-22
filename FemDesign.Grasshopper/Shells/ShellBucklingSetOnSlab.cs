// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class Shells_ShellBucklingSetOnSlab : FEM_Design_API_Component
    {
        public Shells_ShellBucklingSetOnSlab() : base("ShellBuckling.SetOnSlab", "SetOnSlab", "Set ShellBuckling on a slab element.", CategoryName.Name(), SubCategoryName.Cat2b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Slab element.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Direction", "Local x direction of the buckling.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Beta", "Beta", "Beta factor.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Slab.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Shells.Slab slab = null;
            Rhino.Geometry.Vector3d direction = Rhino.Geometry.Vector3d.Unset;
            double beta = 0;

            if (!DA.GetData(0, ref slab))
            {
                return;
            }
            if (!DA.GetData(1, ref direction))
            {
                return;
            }
            if (!DA.GetData(2, ref beta))
            {
                return;
            }

            var dir = direction.FromRhino();
            DA.SetData(0, FemDesign.Shells.ShellBucklingType.SetOnSlab(slab, dir, beta));
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.BucklingDataSetOnConcreteBar;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("a7c3e1f5-8b2d-4e6a-9f01-3c5d7e9b2a4f"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quinary;
    }
}
