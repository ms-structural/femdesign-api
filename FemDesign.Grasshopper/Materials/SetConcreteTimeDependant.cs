using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using FemDesign.Grasshopper;
using System.Windows.Forms;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using FemDesign.Loads;
using Grasshopper.Kernel.Special;
using StruSoft.Interop;
using FemDesign.Materials;

namespace FemDesign.Grasshopper
{
    public class MaterialSetTDA : GH_SwitcherComponent
    {
        /// </summary>
        public MaterialSetTDA() : base("SetConcreteTDA", "SetConcreteTDA", "Set concrete time dependant analysis", CategoryName.Name(), SubCategoryName.Cat4a())
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "Concrete material.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = false;


        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
        }

        protected override void RegisterEvaluationUnits(EvaluationUnitManager mngr)
        {

            EvaluationUnit evaluationUnit = new EvaluationUnit("ExtendableComponent", "ExtComp", "A Test Component");
            mngr.RegisterUnit(evaluationUnit);

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "EN 1992-1-1:2004", "EN 1992-1-1:2004", "", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Integer(), "t0", "t0", "", GH_ParamAccess.item, new GH_Integer(28));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Integer(), "Humidity", "Humidity", "", GH_ParamAccess.item, new GH_Integer(50));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "NonLinearCreep", "NonLinearCreep", "", GH_ParamAccess.item, new GH_Boolean(true));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_String(), "CementType", "CementType", "Cement Type as integer according to FemDesign API.", GH_ParamAccess.item, new GH_String("Class_S"));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(StruSoft.Interop_25.Cement_type)).ToList();

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "IncreaseFinalValue", "IncreaseFinalValue", "", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            GH_ExtendableMenu gH_ExtendableMenu0 = new GH_ExtendableMenu(0, "");
            gH_ExtendableMenu0.Name = "Creep";
            gH_ExtendableMenu0.Expand();
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[0]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[1]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[2]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[3]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[4]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[5]);

            evaluationUnit.AddMenu(gH_ExtendableMenu0);

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "EN 1992-1-1:2004", "EN 1992-1-1:2004", "", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Integer(), "t0", "t0", "", GH_ParamAccess.item, new GH_Integer(28));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Integer(), "Humidity", "Humidity", "", GH_ParamAccess.item, new GH_Integer(50));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_String(), "CementType", "CementType", "Cement Type as integer according to FemDesign API.", GH_ParamAccess.item, new GH_String("Class_S"));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(StruSoft.Interop_25.Cement_type)).ToList();


            GH_ExtendableMenu gH_ExtendableMenu1 = new GH_ExtendableMenu(1, "");
            gH_ExtendableMenu1.Name = "Shrinkage";
            gH_ExtendableMenu1.Expand();
            gH_ExtendableMenu1.RegisterInputPlug(evaluationUnit.Inputs[6]);
            gH_ExtendableMenu1.RegisterInputPlug(evaluationUnit.Inputs[7]);
            gH_ExtendableMenu1.RegisterInputPlug(evaluationUnit.Inputs[8]);
            gH_ExtendableMenu1.RegisterInputPlug(evaluationUnit.Inputs[9]);
            evaluationUnit.AddMenu(gH_ExtendableMenu1);

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "EN 1992-1-1:2004", "EN 1992-1-1:2004", "", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Integer(), "t0", "t0", "", GH_ParamAccess.item, new GH_Integer(28));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_String(), "CementType", "CementType", "Cement Type as integer according to FemDesign API.", GH_ParamAccess.item, new GH_String("Class_S"));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(StruSoft.Interop_25.Cement_type)).ToList();

            GH_ExtendableMenu gH_ExtendableMenu2 = new GH_ExtendableMenu(2, "");
            gH_ExtendableMenu2.Name = "Elasticity";
            gH_ExtendableMenu2.Expand();
            gH_ExtendableMenu2.RegisterInputPlug(evaluationUnit.Inputs[10]);
            gH_ExtendableMenu2.RegisterInputPlug(evaluationUnit.Inputs[11]);
            gH_ExtendableMenu2.RegisterInputPlug(evaluationUnit.Inputs[12]);
            evaluationUnit.AddMenu(gH_ExtendableMenu2);
        }



        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA, EvaluationUnit unit)
        {
            FemDesign.Materials.Material material = null;
            DA.GetData(0, ref material);

            // creep
            bool en1992_creep = false;
            DA.GetData(1, ref en1992_creep);

            int t0_creep = 0;
            DA.GetData(2, ref t0_creep);

            int humidity = 0;
            DA.GetData(3, ref humidity);

            bool nonLinearCreep = false;
            DA.GetData(4, ref nonLinearCreep);

            string cementType_creep = "Class_S";
            DA.GetData(5, ref cementType_creep);
            Enum.TryParse<StruSoft.Interop_25.Cement_type>(cementType_creep, out StruSoft.Interop_25.Cement_type _cementType_creep);

            bool increaseFinalValue = false;
            DA.GetData(6, ref increaseFinalValue);

            // shrinkage
            bool en1992_shrinkage = false;
            DA.GetData(7, ref en1992_shrinkage);

            int t0_shrinkage = 0;
            DA.GetData(8, ref t0_shrinkage);

            int humidity_shrinkage = 0;
            DA.GetData(9, ref humidity_shrinkage);

            string cementType_shrinkage = "Class_S"; ;
            DA.GetData(10, ref cementType_shrinkage);
            Enum.TryParse<StruSoft.Interop_25.Cement_type>(cementType_shrinkage, out StruSoft.Interop_25.Cement_type _cementType_shrinkage);

            // elasticity
            bool en1992_elasticity = false;
            DA.GetData(11, ref en1992_elasticity);

            int t0_elasticity = 0;
            DA.GetData(12, ref t0_elasticity);

            string cementType_elasticity = "Class_S";
            DA.GetData(13, ref cementType_elasticity);
            Enum.TryParse<StruSoft.Interop_25.Cement_type>(cementType_elasticity, out StruSoft.Interop_25.Cement_type _cementType_elasticity);


            // apply the method creep, shrinkage and elasticity using the booleans
            if (en1992_creep)
            {
                material = material.SetCreep(t0_creep, humidity, nonLinearCreep, _cementType_creep, increaseFinalValue);
            }
            if (en1992_shrinkage)
            {
                material = material.SetShrinkage(t0_shrinkage, humidity_shrinkage, _cementType_shrinkage);
            }
            if (en1992_elasticity)
            {
                material = material.setElasticity(t0_elasticity, _cementType_elasticity);
            }

            DA.SetData(0, material);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                return FemDesign.Properties.Resources.MaterialSetConcreteMaterialProperties;
            }
        }

        //To recompute component/solve instance method.
        protected void setModelProps()
        {
            ((GH_DocumentObject)this).ExpireSolution(true);
        }


        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{A80FC5B7-F548-4DBF-97C4-AE73FA07CCC6}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}