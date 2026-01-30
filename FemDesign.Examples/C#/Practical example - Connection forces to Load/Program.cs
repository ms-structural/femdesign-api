using FemDesign.GenericClasses;
using FemDesign.Geometry;
using FemDesign.Loads;
using FemDesign.Results;
using StruSoft.Interop.StruXml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var loads = new List<ILoadElement>();


            var units = new UnitResults
            {
                Force = Force.N,
                Length = Length.m
            };


            using (var femDesign = new FemDesignConnection(keepOpen: true, fdInstallationDir: "C:\\Program Files\\StruSoft\\FEM-Design 25"))
            {
                var filePath = "fea.str";
                femDesign.Open(filePath);

                var cnnForces = femDesign.GetResults<Results.LineConnectionForce>(units);
                var feaLnCnn = femDesign.GetResults<Results.LineConnectionElement>();

                var nodeDictionary = femDesign.GetFeaNodes().ToDictionary(n => n.NodeId, n => new Geometry.Point3d(n.X, n.Y, n.Z));

                var loadCaseComb = cnnForces.Select(x => x.CaseIdentifier).Distinct().ToList();

                var caseCombs = new List<LoadCase>();
                foreach(var caseComb in loadCaseComb)
                {
                    var loadCase = new LoadCase(caseComb, LoadCaseType.Ordinary, LoadCaseDuration.Permanent);
                    caseCombs.Add(loadCase);
                }
                var combDictionary = caseCombs.ToDictionary(x => x.Name);

                foreach(var cnn in feaLnCnn)
                {
                    var point1 = nodeDictionary[cnn.Node1];
                    var point2 = nodeDictionary[cnn.Node2];
                    var edge = new Geometry.Edge(point1, point2);

                    foreach(var ldCase in loadCaseComb)
                    {
                        var forces = cnnForces.Where(f => f.CaseIdentifier == ldCase).Where(f => f.ElementId == cnn.ElementId).Last();

                        var force = new Vector3d(forces.Fx, forces.Fy, forces.Fz);
                        var moment = new Vector3d(forces.Mx, forces.My, forces.Mz);

                        try
                        {
                            var lineForce = new Loads.LineLoad(edge, force, combDictionary[ldCase], ForceLoadType.Force);
                            var lineMoment = new Loads.LineLoad(edge, moment, combDictionary[ldCase], ForceLoadType.Moment);
                            loads.Add(lineForce);
                            loads.Add(lineMoment);
                        }
                        catch (Exception e)
                        {
                            //
                        }

                    }
                }

                var newModel = new Model(Country.S);
                newModel.AddLoads(loads);
                newModel.AddLoadCases(caseCombs);

                femDesign.Open(newModel);
            }
        }
    }
}