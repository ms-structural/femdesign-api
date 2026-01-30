using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;
using FemDesign.Results;
using FemDesign.GenericClasses;

namespace FemDesign.Examples
{
    public class Program
    {
        static void Main()
        {
            // EXAMPLE 1: CREATING A SIMPLE BEAM
            // This example shows how to model a simple supported beam,
            // and how to save it for export to FEM-Design. Before running,
            // make sure you have a window with FEM-Design open.

            // This example was last updated using the ver. 22.0.1 FEM-Design API.


            // Define geometry
            var length = 6.00;

            var p1 = new Geometry.Point3d(0.0, 0.0, 0.0);
            var p2 = new Geometry.Point3d(length, 0.0, 0.0);
            var mid = p1 + (p2 - p1) * 0.5;

            var edge = new Geometry.LineEdge(p1, p2);


            // Load material and sections from .struxml files
            var materialsDB = Materials.MaterialDatabase.DeserializeStruxml("materials.struxml");
            var material = materialsDB.MaterialByName("C35/45");
            
            var sectionsDB = Sections.SectionDatabase.DeserializeStruxml("sections.struxml");
            var section = sectionsDB.SectionByName("Concrete sections, Rectangle, 300x900");


            // Create the beam and supports
            var beam = new Bars.Beam(
                edge,
                material,
                section);

            double mStiffness = 1e10;
            var motion = new Releases.Motions(0.0, 0.0, 0.0, 0.0, mStiffness, mStiffness);
            var rotation = Releases.Rotations.Free();

            var s1 = Supports.PointSupport.Hinged(p1, identifier: "Hinged");
            var s2 = new Supports.PointSupport(p2, motion, rotation, identifier: "Z only");

            var elements = new List<GenericClasses.IStructureElement>() { beam, s1, s2 };


            // Create load cases
            var deadload = new Loads.LoadCase("Deadload", Loads.LoadCaseType.DeadLoad, Loads.LoadCaseDuration.Permanent);
            var liveload = new Loads.LoadCase("Liveload", Loads.LoadCaseType.Ordinary, Loads.LoadCaseDuration.Permanent);
            var loadcases = new List<Loads.LoadCase>() { deadload, liveload };


            // Create load combinations
            var SLS = new Loads.LoadCombination("SLS", Loads.LoadCombType.ServiceabilityCharacteristic,
                (deadload, 1.0),
                (liveload, 1.0));
            var ULS = new Loads.LoadCombination("ULS", Loads.LoadCombType.UltimateOrdinary,
                (deadload, 1.2),
                (liveload, 1.5));
            var loadCombinations = new List<Loads.LoadCombination>() { SLS, ULS };


            // Create loads
            var pointForce = Loads.PointLoad.Force(mid, new Geometry.Vector3d(0.0, 0.0, -5.0), liveload);
            var pointMoment = Loads.PointLoad.Moment(p2, new Geometry.Vector3d(0.0, 5.0, 0.0), liveload);

            var lineLoadStart = new Geometry.Vector3d(0.0, 0.0, -2.0);
            var lineLoadEnd = new Geometry.Vector3d(0.0, 0.0, -4.0);
            var lineLoad = Loads.LineLoad.VariableForce(edge, lineLoadStart, lineLoadEnd, liveload);

            var loads = new List<GenericClasses.ILoadElement>() {
                pointForce,
                pointMoment,
                lineLoad
            };


            // Add to model
            Model model = new Model(Country.S);
            model.AddElements(elements);
            model.AddLoadCases(loadcases);
            model.AddLoadCombinations(loadCombinations);
            model.AddLoads(loads);


            // Run Analysis
            using (var femDesign = new FemDesignConnection(outputDir: "My simple beam", keepOpen: true))
            {
                var analysis = FemDesign.Calculate.Analysis.StaticAnalysis();

                // Create analysis
                femDesign.Open(model);
                femDesign.RunAnalysis(analysis);

                // Read results
                var pointSupportReactions = femDesign.GetResults<PointSupportReaction>();

                // Print results
                Console.WriteLine();
                Console.WriteLine("Id         | Reaction   | Case Identifier");
                foreach (var reaction in pointSupportReactions)
                {
                    Console.WriteLine($"{reaction.Id,10} | {reaction.Fz,10} | {reaction.CaseIdentifier,10}");
                }
            }
        }
    }
}