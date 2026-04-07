using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public class ReinforcementPtcManufacturingType : FEM_Design_API_Component
    {
        /// <summary>
        /// Initializes a new instance of the PtcManufacturingType class.
        /// </summary>
        public ReinforcementPtcManufacturingType() : base("PtcManufacturing", "Manufacturing", "Description", "FEM-Design", "Reinforcement")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Positions", "Positions", "Normalized positions along the cable (0 to 1) [m]. Default is {0, 0.125, 0.25, 0.375, 0.5, 0.625, 0.75, 0.875, 1}.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("ShiftX", "ShiftX", "Shift in X direction [m]. Default is 0.", GH_ParamAccess.item, 0.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("ShiftZ", "ShiftZ", "Shift in Z direction [m]. Default is 0.1.", GH_ParamAccess.item, 0.1);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Manufacturing", "Manufacturing", "FemDesign.Reinforcement.PtcManufacturingType", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> positions = new List<double>();
            double shiftX = 0.0;
            double shiftZ = 0.1;
            if (!DA.GetDataList("Positions", positions))
            {
                positions = new List<double> { 0, 0.125, 0.25, 0.375, 0.5, 0.625, 0.75, 0.875, 1 };
            }
            DA.GetData("ShiftX", ref shiftX);
            DA.GetData("ShiftZ", ref shiftZ);

            var manufacturing = new PtcManufacturingType(positions, shiftX, shiftZ);

            DA.SetData("Manufacturing", manufacturing);
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
                return FemDesign.Properties.Resources.PtcManufacturing;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6323fbff-e53a-40dd-b368-8d60f04fec3c"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}