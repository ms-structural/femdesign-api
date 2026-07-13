// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Forms;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class TextAnnotation : FEM_Design_API_Component
    {
        public static readonly List<string> HorAlignValueList = Enum.GetNames(typeof(StruSoft.Interop.StruXml.Data.Hor_align)).ToList();
        public static string HorAlignValueListDescription
        {
            get
            {
                var str = "";
                foreach (var h in HorAlignValueList)
                {
                    str += "\n" + h;
                }
                return str;
            }
        }
        public static readonly List<string> VerAlignValueList = Enum.GetNames(typeof(StruSoft.Interop.StruXml.Data.Ver_align)).ToList();
        public static string VerAlignValueListDescription
        {
            get
            {
                var str = "";
                foreach (var h in VerAlignValueList)
                {
                    str += "\n" + h;
                }
                return str;
            }
        }

        public TextAnnotation() : base("TextAnnotation", "TxtAnnot", "Create a text annotation.", CategoryName.Name(), "ModellingTools")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Point|Plane", "Point|Plane", "Position and orientation of text. [m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Text", "Text", "Text.", GH_ParamAccess.item);
            pManager.AddNumberParameter("FontSize", "FontSize", "Font size of text. [m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddColourParameter("Colour", "Colour", "Colour of text. [ARGB]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("HorisontalAligment", "HorAlign", $"Horisontal alignement of text. Connect 'ValueList' to get the options: {HorAlignValueListDescription}", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("VerticalAligment", "VerAlign", $"Vertical alignement of text. Connect 'ValueList' to get the options: {VerAlignValueListDescription}", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Layer", "Layer", "Layer. If omitted, the text is placed on layer 'TEXT'.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TextAnnotation", "TextAnnotation", "TextAnnotation.", GH_ParamAccess.item);
        }
        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 4, HorAlignValueList, null, GH_ValueListMode.DropDown);
            ValueListUtils.UpdateValueLists(this, 5, VerAlignValueList, null, GH_ValueListMode.DropDown);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var plane = Plane.WorldXY;
            if (!DA.GetData(0, ref plane))
            {
                // pass
            }

            string text = null;
            if (!DA.GetData(1, ref text))
            {
                return;
            }


            var textAnnot = new Drawing.TextAnnotation(plane.Origin.FromRhino(), plane.XAxis.FromRhino(), plane.YAxis.FromRhino(), text);
            textAnnot.StyleType.Layer = "TEXT";
            textAnnot.StyleType.LayerObj = new StruSoft.Interop.StruXml.Data.Layer_type
            {
                Name = "TEXT",
                Colour = "000000",
                Hidden = false,
                Protected = false
            };

            double size = 0;
            if (DA.GetData(2, ref size))
            {
                textAnnot.StyleType.Font.Size = size;
            }

            System.Drawing.Color color = System.Drawing.Color.Black;
            if (DA.GetData(3, ref color))
            {
                textAnnot.StyleType.SetColor = color;
            }

            string horAlign = null;
            if (DA.GetData(4, ref horAlign))
            {
                if (Enum.TryParse(horAlign, out StruSoft.Interop.StruXml.Data.Hor_align horAlignEnum))
                {
                    textAnnot.StyleType.Font.H_align = horAlignEnum;
                }
                else
                {
                    throw new System.ArgumentException($"Invalid horisontal alignment value: {horAlign}, must be one of the following values: {HorAlignValueListDescription}");
                }
            }

            string verAlign = null;
            if (DA.GetData(5, ref verAlign))
            {
                if (Enum.TryParse(verAlign, out StruSoft.Interop.StruXml.Data.Ver_align verAlignEnum))
                {
                    textAnnot.StyleType.Font.V_align = verAlignEnum;
                }
                else
                {
                    throw new System.ArgumentException($"Invalid vertical alignment value: {verAlign}, must be one of the following values: {VerAlignValueListDescription}");
                }
            }

            StruSoft.Interop.StruXml.Data.Layer_type layer = null;
            if (DA.GetData(6, ref layer) && layer != null)
            {
                textAnnot.StyleType.Layer = layer.Name;
                textAnnot.StyleType.LayerObj = layer;
            }

            DA.SetData(0, textAnnot);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.TextAnnotation;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("24767AB6-9D7E-4DE3-BB1A-5F0AF6470987"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;
    }
}