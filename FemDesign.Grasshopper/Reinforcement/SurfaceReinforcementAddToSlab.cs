// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Forms;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class SurfaceReinforcementAddToSlab: FEM_Design_API_Component
    {
        public SurfaceReinforcementAddToSlab(): base("SurfaceReinforcement.AddToSlab", "AddToSlab", "Add surface reinforcement to slab.", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Slab.", GH_ParamAccess.item);
            pManager.AddGenericParameter("SurfaceReinforcement", "SurfaceReinforcement", "SurfaceReinforcment to add to slab. Item or list.", GH_ParamAccess.list);
            pManager.AddVectorParameter("xDir", "xDir", "Reinforcement direction", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("yDir", "yDir", "Reinforcement direction", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Passed slab.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            FemDesign.Shells.Slab slab = null;
            if (!DA.GetData(0, ref slab))
            {
                return;
            }
            
            List<FemDesign.Reinforcement.SurfaceReinforcement> surfaceReinforcement = new List<FemDesign.Reinforcement.SurfaceReinforcement>();
            if (!DA.GetDataList(1, surfaceReinforcement))
            {
                return;
            }

            Rhino.Geometry.Vector3d _xDir = Rhino.Geometry.Vector3d.Unset;
            DA.GetData(2, ref _xDir);

            Rhino.Geometry.Vector3d _yDir = Rhino.Geometry.Vector3d.Unset;
            DA.GetData(3, ref _yDir);


            // Check inputs
            if (slab == null || surfaceReinforcement == null)
            {
                return;
            }
            // check planes
            foreach (var reinf in surfaceReinforcement)
            {
                if (slab.Region.Plane == reinf.Region.Plane)
                {
                    throw new Exception($"Surface reinforcement (GUID: {reinf.Guid}) is on a different plane to the slab (GUID: {slab.Guid})!");
                }
            }
            // check ovarlap
            this.CheckOverlap(slab, surfaceReinforcement);


            var _surfaceReinforcement = surfaceReinforcement.DeepClone();

            FemDesign.Geometry.Vector3d xDir;
            if (_xDir == Rhino.Geometry.Vector3d.Unset)
                xDir = slab.SlabPart.LocalX;
            else
                xDir = _xDir.FromRhino();

            FemDesign.Geometry.Vector3d yDir;
            if (_yDir == Rhino.Geometry.Vector3d.Unset)
                yDir = slab.SlabPart.LocalY;
            else
                yDir = _yDir.FromRhino();

            FemDesign.Shells.Slab obj = FemDesign.Reinforcement.SurfaceReinforcement.AddReinforcementToSlab(slab, _surfaceReinforcement, xDir, yDir);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.RebarAddToElement;
                ;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{928F0C21-878A-4B27-A61C-79832AEB2C88}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        private void CheckOverlap(Shells.Slab slab, List<FemDesign.Reinforcement.SurfaceReinforcement> surfaceReinforcement)
        {
            //Transfer FemDesign API Regions into Rhino Plane objects
            var slabPlane = slab.SlabPart.Region.Plane.ToRhino();
            var reinfSurfacesPlanes = surfaceReinforcement.Select(r => r.Region.Plane.ToRhino()).ToList();          

            //Transfer FemDesign API Contour points into Rhino Point3d objects
            var slabPts = slab.SlabPart.Region.Contours.Select(c => c.Points.Select(p => p.ToRhino()).ToList()).ToList();
            var reinfSurfacesPts = surfaceReinforcement.Select(r => r.Region.Contours.Select(c => c.Points.Select(p => p.ToRhino()).ToList()).ToList()).ToList();

            // surface boundary points
            List<Point3d> slabBoundPts = slabPts[0];
            List<List<Point3d>> reinfSurfacesBoundPts = reinfSurfacesPts.Select(p => p[0]).ToList();

            // holes contour points
            var slabHolesPts = new List<List<Point3d>>();
            if (slabPts.Count > 1)
            {
                for (int i = 1; i < slabPts.Count; i++)
                {
                    slabHolesPts.Add(slabPts[i]);
                }
            }
            var reinfSurfacesHolesPts = new List<List<List<Point3d>>>();
            foreach (var oneSurfPoints in reinfSurfacesPts)
            {
                var oneSurfaceHolesPts = new List<List<Point3d>>();
                if (oneSurfPoints.Count > 1)
                {
                    for (int i = 1; i < oneSurfPoints.Count; i++)
                    {
                        oneSurfaceHolesPts.Add(oneSurfPoints[i]);
                    }
                }
                reinfSurfacesHolesPts.Add(oneSurfaceHolesPts);
            }

            // surface boundary curves
            PolylineCurve slabBound = new PolylineCurve(slabBoundPts);
            List<PolylineCurve> reinfBounds = new List<PolylineCurve>();
            foreach (var ptList in reinfSurfacesBoundPts)
            {
                reinfBounds.Add(new PolylineCurve(ptList));
            }

            // holes contour curves
            List<PolylineCurve> slabHoleCurves = new List<PolylineCurve>();
            foreach (var ptList in slabHolesPts)
            {
                slabHoleCurves.Add(new PolylineCurve(ptList));
            }
            List<List<PolylineCurve>> reinfSurfacesHoleCurves = new List<List<PolylineCurve>>();
            foreach (var oneSurfePtList in reinfSurfacesHolesPts)
            {
                List<PolylineCurve> oneSurfHoleCurves = new List<PolylineCurve>();
                foreach (var ptList in oneSurfePtList)
                {
                    oneSurfHoleCurves.Add(new PolylineCurve(ptList));
                }
                reinfSurfacesHoleCurves.Add(oneSurfHoleCurves);
            }
            
            // check if reinf surf boundary curves are in the slab surface's region
            for (int i = 0; i < reinfSurfacesBoundPts.Count; i++)    // i = index of SurfaceReinforcement item
            {
                var slabRelations = Contains(slabBound, reinfSurfacesBoundPts[i], slabPlane);

                if (IsAllCoincident(slabRelations))
                    break;
                if(IsAllInside(slabRelations))
                {
                    foreach (var hole in slabHoleCurves)
                    {
                        var holeRelations = Contains(hole, reinfSurfacesBoundPts[i], slabPlane);

                        if(IsCoincidentOrInside(holeRelations))
                        {
                            throw new Exception($"The surface reinforcement (GUID: {surfaceReinforcement[i].Guid}) is placed in a hole (slab GUID: {slab.Guid}). Reinforcement and slab surfaces must overlap!");
                        }
                    }
                }
                if(IsCoincidentOrOutside(slabRelations))
                {
                    var reinfRelations = Contains(reinfBounds[i], slabBoundPts, reinfSurfacesPlanes[i]);

                    if (IsAllInside(reinfRelations))
                    {
                        foreach (var hole in reinfSurfacesHoleCurves[i])
                        {
                            var holeRelations = Contains(hole, slabBoundPts, reinfSurfacesPlanes[i]);

                            if (IsCoincidentOrInside(holeRelations))
                            {
                                throw new Exception($"The slab (GUID: {slab.Guid}) is placed in a hole in the reinforcement's surface (reinforcement GUID: {surfaceReinforcement[i].Guid}). Reinforcement and slab surfaces must overlap!");
                            }
                        }
                    }
                    if (IsCoincidentOrOutside(reinfRelations))
                    {
                        throw new Exception($"Reinforcement (GUID: {surfaceReinforcement[i].Guid}) and slab (GUID: {slab.Guid}) surfaces are not overlapping!");
                    }
                }
            }
        }
        private List<PointContainment> Contains(Curve curve, List<Point3d> pts, Plane plane)
        {
            List<PointContainment> relations = new List<PointContainment>();
            foreach (var pt in pts)
            {
                relations.Add(curve.Contains(pt, plane, 1E-5));
            }

            return relations;
        }
        private bool IsAllCoincident(List<PointContainment> relation)
        {
            if (relation == null) return false;

            return relation.All(r => r == PointContainment.Coincident);
        }
        private bool IsAllInside(List<PointContainment> relation)
        {
            if(relation == null) return false;

            return relation.All(r => r == PointContainment.Inside);
        }
        private bool IsAllOutside(List<PointContainment> relation)
        {
            if (relation == null) return false;

            return relation.All(r => r == PointContainment.Outside);
        }
        private bool IsCoincidentOrInside(List<PointContainment> relation)
        {
            if (relation == null) return false;

            return relation.All(r => r == PointContainment.Coincident || r == PointContainment.Inside);
        }
        private bool IsCoincidentOrOutside(List<PointContainment> relation)
        {
            if (relation == null) return false;

            return relation.All(r => r == PointContainment.Coincident || r == PointContainment.Outside);
        }
    }   
}