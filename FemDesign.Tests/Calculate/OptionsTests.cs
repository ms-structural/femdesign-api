using FemDesign.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FemDesign.Calculate
{
    [TestClass()]
    public class OptionsTests
    {
        [TestMethod("Options")]
        public void TestOptions()
        {
            var options = new Options(BarResultPosition.OnlyNodes, ShellResultPosition.Center, 0.50);
            Assert.IsNotNull(options);
            Assert.IsTrue(options.Bar == 0);
            Assert.IsTrue(options.SrfValues == 0);

            options = new Options(BarResultPosition.ByStep, ShellResultPosition.Vertices, 0.50);
            Assert.IsNotNull(options);
            Assert.IsTrue(options.Bar == 1);
            Assert.IsTrue(options.SrfValues == 1);

            options = new Options(BarResultPosition.ByStep, ShellResultPosition.Vertices, 0.50);
            Assert.IsNotNull(options);
            Assert.IsTrue(options.Bar == 1);
            Assert.IsTrue(options.SrfValues == 1);

            options = new Options(BarResultPosition.ResultPoints, ShellResultPosition.ResultPoints, 0.50);
            Assert.IsNotNull(options);
            Assert.IsTrue(options.Bar == 2);
            Assert.IsTrue(options.SrfValues == 2);
        }


        [TestMethod("DeserialiseGlobalcfg")]
        public void DeserialiseGlobalcfg()
        {
            var globalCfg = Calculate.CmdGlobalCfg.DeserializeCmdGlobalCfgFromFilePath(@"Calculate\cmdglobalcfg.xml");
            Assert.IsNotNull(globalCfg);
        }

        // write a test for Coldata object
        [TestMethod("Coldata")]
        public void TestColdata()
        {
            var coldata = new Coldata(1, 0);
            Assert.IsNotNull(coldata);
            Assert.IsTrue(coldata.Num == 1);
            Assert.IsTrue(coldata.Flags == 0);
            Assert.IsTrue(coldata.Width == 0);
            Assert.IsTrue(coldata.Format == null);

            Console.WriteLine(coldata.SerializeObjectToXml());

            coldata = new Coldata(1, 0, 20, "%s");
            Assert.IsNotNull(coldata);
            Assert.IsTrue(coldata.Num == 1);
            Assert.IsTrue(coldata.Flags == 0);
            Assert.IsTrue(coldata.Width == 20);
            Assert.IsTrue(coldata.Format == "%s");

            Console.WriteLine(coldata.SerializeObjectToXml());


            var coldataList = Coldata.Default();
            Assert.IsNotNull(coldataList);
            Assert.IsTrue(coldataList.Count == 64);
        }
    }
}