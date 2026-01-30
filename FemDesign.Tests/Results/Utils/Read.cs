using FemDesign.Calculate;
using FemDesign.Sections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FemDesign.Results
{
    [TestClass]
    public class Read
    {
        [TestMethod]
        [TestCategory("FEM-Design required")]
        public void GetResultMethodTests()
        {
            string filepath = "Results\\Assets\\General.str";

            using (var connection = new FemDesignConnection())
            {
                connection.Open(filepath);

                var model = connection.GetModel();
                var bars = model.Entities.Bars;
                var elements = new List<FemDesign.Bars.Bar> { bars[1], bars[2] };
                var elemIds = elements.Select(e => e.BarPart.Name).ToList();

                var loads = connection.GetLoads();
                var loadCases = loads.LoadCases.Select(c => c.Name).ToList();
                var loadCombs = loads.LoadCombinations.Select(c => c.Name).ToList();

                //------------------------------------------------------------------
                // Test GetResults() method
                //------------------------------------------------------------------

                // Get all of the BarDisplacement results
                var allRes = connection.GetResults<BarDisplacement>().OrderBy(r => r.Id).ToList();
                Assert.IsTrue(allRes.Count != 0);

                List<BarDisplacement> filteredAllRes = new List<BarDisplacement>();
                foreach (var id in elemIds)
                {
                    var filteredRes = allRes.Where(r => r.Id == id).ToList();
                    filteredAllRes.AddRange(filteredRes);
                }
                filteredAllRes = filteredAllRes.OrderBy(r => r.Pos).OrderBy(r => r.CaseIdentifier).OrderBy(r => r.Id).ToList();


                // Get all of the BarDisplacement results by structural elements
                var structElements = elements.Select(e => (FemDesign.GenericClasses.IStructureElement)e).ToList();
                var allResByElements = connection.GetResults<BarDisplacement>(elements: structElements).OrderBy(r => r.Pos).OrderBy(r => r.CaseIdentifier).OrderBy(r => r.Id).ToList();
                Assert.IsTrue(allRes.Count != 0);


                // Check
                Assert.AreEqual(allResByElements.Count, filteredAllRes.Count);
                for (int i = 0; i < allResByElements.Count; i++)
                {
                    PropertyInfo[] properties = typeof(BarDisplacement).GetProperties();
                    properties = properties.Where(p => p.Name != nameof(BarDisplacement.CaseIdentifier)).ToArray();

                    foreach (var prop in properties)
                    {
                        var item1 = prop.GetValue(allResByElements[i]);
                        var item2 = prop.GetValue(filteredAllRes[i]);
                        Assert.AreEqual(item1, item2);
                    }
                    string caseId1 = allResByElements[i].CaseIdentifier.Replace(" - selected objects", "");
                    string caseId2 = filteredAllRes[i].CaseIdentifier;
                    Assert.AreEqual(caseId1, caseId2);
                }


            }
        }

        //[TestCategory("FEM-Design required")]
        //[TestMethod]
        //public void TestGetSectionProperties()
        //{
        //    var sectionsDB = Sections.SectionDatabase.DeserializeStruxml("Results\\Utils\\Sections.struxml");
        //    List<Section> sections = sectionsDB.Sections.Section;
        //    var sectionProps = sections.GetSectionProperties();

        //    List<string[]> resValList = new List<string[]>();   // Each string array in the list represents a section property result line from  'SectionPropertiesResultFile.csv', where the cell values are split into array elements.
        //    int n = 0;
        //    using (var reader = new StreamReader("Results\\Utils\\SectionPropertiesResultFile.csv"))
        //    {
        //        while (!reader.EndOfStream)
        //        {
        //            var row = reader.ReadLine();

        //            if (row != "")
        //            {
        //                n++;
        //                if (n > 3)
        //                {
        //                    resValList.Add(row.Split('\t'));
        //                }
        //            }
        //        }
        //    }


        //    // Check section properties
        //    Assert.IsTrue(sectionProps.Count == 5, "Check 'Results\\Utils\\Sections.struxml'! It must contain 5 sections!");
        //    Assert.IsTrue(sectionProps.Count == resValList.Count, "Check 'Results\\Utils\\SectionPropertiesResultFile.csv'! " +
        //        "The number of result lines must be the same as the number of 'sectionProps.Count'!");

        //    PropertyInfo[] objProps = typeof(Results.SectionProperties).GetProperties();
        //    Assert.IsTrue(resValList[0].Length == objProps.Length);

        //    //for (int i = 0; i < sectionProps.Count; i++)
        //    //{
        //    //    for (int j = 0; j < objProps.Length; j++)
        //    //    {
        //    //        var objPropValue = objProps[j].GetValue(sectionProps[i]);    // Value of 'property j' from 'section i' if using GetSectionProperties()
        //    //        var resVal = resValList[i][j];  // Individual section property value (e.g. Height, A, Iz,...) from file.
        //    //        string message = "Section property values from 'GetSectionProperties()' must be the same as in 'SectionPropertiesResultFile.csv'!";

        //    //        var type = objPropValue.GetType();
        //    //        if (type == typeof(double))
        //    //        {
        //    //            object compareVal = Double.Parse(resVal, System.Globalization.CultureInfo.InvariantCulture);
        //    //            Assert.IsTrue(objPropValue == compareVal, message);
        //    //        }
        //    //        Assert.IsTrue(objPropValue.ToString() == resVal, message);
        //    //    }
        //    //}
        //}

        [TestCategory("FEM-Design required")]
        [TestMethod]
        public void TestGetStabilityResults()
        {
            string struxmlPath = "Results\\Utils\\ReadBucklingShapesTest.struxml";
            Model model = Model.DeserializeFromFilePath(struxmlPath);

            var stability = new Calculate.Stability(new List<string> { "LC2ULS" }, new List<int> { 10 }, false, 5);
            FemDesign.Calculate.Analysis analysis = new FemDesign.Calculate.Analysis(stability: stability, calcComb: true, calcStab: true, diaphragm: DiaphragmType.None);


            using (var femDesign = new FemDesignConnection(fdInstallationDir: @"C:\Program Files\StruSoft\FEM-Design 25\", outputDir: "My analyzed model", keepOpen: false))
            {
                femDesign.RunAnalysis(model, analysis);

                var resultsBuckling = new List<Results.NodalBucklingShape>();
                var critParam = new List<Results.CriticalParameter>();

                List<string> allCombNames = model.Entities.Loads.LoadCombinations.Select(r => r.Name).ToList();
                var combName = new List<string>() { allCombNames[1] };
                List<int> id = new List<int>() { 5, 3 };
                foreach (string c in combName)
                {
                    foreach (int i in id)
                    {
                        var res = femDesign.GetStabilityResults<Results.NodalBucklingShape>(c, i);
                        resultsBuckling.AddRange(res);
                        var crit = femDesign.GetStabilityResults<Results.CriticalParameter>(c, i);
                        critParam.AddRange(crit);
                    }
                }

                Assert.IsNotNull(resultsBuckling);
                Assert.IsNotNull(critParam);
            }
        }

        //[TestCategory("FEM-Design required")]
        //[TestMethod]
        //public void TestGetEigenResults()
        //{
        //    // get model data
        //    string struxmlPath = "Results\\Stability\\ReadBucklingShapesTest.struxml";
        //    Model model = Model.DeserializeFromFilePath(struxmlPath);
        //    List<string> loadCombinations = model.Entities.Loads.LoadCombinations.Select(l => l.Name).ToList();

        //    // setup stability analysis
        //    List<List<string>> validCombos = new List<List<string>>
        //    {
        //        new List<string>(){ "LC1ULS" },
        //        new List<string>(){ "LC2ULS" },
        //        loadCombinations
        //    };
        //    List<List<string>> invalidCombos = new List<List<string>>
        //    {
        //        new List<string>(){ "Lc1uLs" },
        //        new List<string>(){ "LC5ULS" }
        //    };
        //    List<List<string>> combos = validCombos.Concat(invalidCombos).ToList();
        //    List<List<int>> reqShapes = new List<List<int>>
        //    {
        //        new List<int>(){ 8},
        //        new List<int>(){ 8},
        //        new List<int>(){ 10, 15 },
        //        new List<int>(){ 8 },
        //        new List<int>(){ 8 },
        //    };

        //    var stab = new List<Calculate.Stability>();
        //    var analysis = new List<Calculate.Analysis>();
        //    for (int i = 0; i < combos.Count; i++)
        //    {
        //        stab.Add(new Calculate.Stability(combos[i], reqShapes[i], false, 5));
        //        analysis.Add(new FemDesign.Calculate.Analysis(stability: stab[i], calcComb: true, calcStab: true));

        //    }


        //    List<List<string>> combos2 = invalidCombos.Concat(validCombos).ToList();
        //    List<List<int>> shapeIds = new List<List<int>>
        //    {
        //        new List<int>(){ 8, 4, 10 },
        //        new List<int>(){ 1, 3, 7, 8 },
        //        new List<int>(){ 10, 15 },
        //        new List<int>(){ 8 },
        //        new List<int>(){ 8 },
        //    };
        //    var bucklRes = new List<List<NodalBucklingShape>>();
        //    var bucklRes2 = new List<List<NodalBucklingShape>>();
        //    var critParams = new List<List<CriticalParameter>>();
        //    var critParams2 = new List<List<CriticalParameter>>();

        //    using (var femDesign = new FemDesignConnection(fdInstallationDir: @"C:\Program Files\StruSoft\FEM-Design 23\", outputDir: "StabilityResultsTest", keepOpen: false))
        //    {
        //        // open model
        //        femDesign.Open(model);


        //        for (int i = 0; i < combos.Count; i++)
        //        {
        //            // run analysis
        //            femDesign.RunAnalysis(analysis[i]);

        //            // get results
        //            bucklRes.Add(femDesign.GetEigenResults<Results.NodalBucklingShape>(combos[i], shapeIds[i]));
        //            bucklRes2.Add(femDesign.GetEigenResults<Results.NodalBucklingShape>(combos2[i], shapeIds[i]));
        //            critParams.Add(femDesign.GetEigenResults<Results.CriticalParameter>(combos[i], shapeIds[i]));
        //            critParams2.Add(femDesign.GetEigenResults<Results.CriticalParameter>(combos2[i], shapeIds[i]));
        //        }
        //    }

        //    // check results
        //    for (int i = 0; i < validCombos.Count; i++)
        //    {
        //        Assert.IsTrue(bucklRes[i].Count != 0);
        //    }
        //    for (int i = validCombos.Count; i < combos.Count; i++)
        //    {
        //        Assert.IsTrue(bucklRes[i].Count == 0);
        //    }
        //    for (int i = 0; i < invalidCombos.Count; i++)
        //    {
        //        Assert.IsTrue(bucklRes2[i].Count == 0);
        //    }
        //    for (int i = invalidCombos.Count + 1; i < combos2.Count; i++)
        //    {
        //        Assert.IsTrue(bucklRes2[i].Count == 0);
        //    }
        //}


    }
}
