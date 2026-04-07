using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public class PtcStrand : FEM_Design_API_Component
    {
        /// <summary>
        /// Initializes a new instance of the PtcStrand class.
        /// </summary>
        public PtcStrand(): base("PTC.Strand", "Strand", "Post-tensioning strands.", "FEM-Design", "Reinforcement")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Strand type name. Default is 'Y2060S7-11,3-F1-C1'.", GH_ParamAccess.item, "Y2060S7-11,3-F1-C1");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("f pk", "f pk", "Characteristic value of tensile strength [N/mm2]. Default is 2060.", GH_ParamAccess.item, 2060);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("A p", "A p", "Cross sectional area (nominal value) [mm2]. Default is 75.", GH_ParamAccess.item, 75);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("E p", "E p", "Modulus of elasticity [N/mm2]. Default is 195000.", GH_ParamAccess.item, 195000);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Rho", "Rho", "Density [kg/m3]. Default is 7819.", GH_ParamAccess.item, 7819);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("RelaxationClass", "RelaxationClass", "Relaxation class. Enter a value between 1 and 3. Default is 2.", GH_ParamAccess.item, 2);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Rho 1000", "Rho 1000", "Relaxation at 1000 hour [%]. Default is 2.5.", GH_ParamAccess.item, 2.5);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTC.Strand", "Strand", "Post-tensioning strands.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = "Y2060S7-11,3-F1-C1";
            double f_pk = 2060;
            double a_p = 75;
            double e_p = 195000;
            double density = 7819;
            int relaxationClass = 2;
            double rho_1000 = 2.5;

            DA.GetData(0, ref name);
            DA.GetData(1, ref f_pk);
            DA.GetData(2, ref a_p);
            DA.GetData(3, ref e_p);
            DA.GetData(4, ref density);
            DA.GetData(5, ref relaxationClass);
            DA.GetData(6, ref rho_1000);

            var strand = new PtcStrandLibType(name, f_pk, a_p, e_p, density, relaxationClass, rho_1000);

            DA.SetData(0, strand);
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
                return FemDesign.Properties.Resources.PtcStrand;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("E1E8E8A5-B7BB-428A-BD4D-E642BB760005"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
    }
}