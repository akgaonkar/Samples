using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;
using FileParser;
using System.Diagnostics;
using ResumeProcessor;

namespace UnitTestProject
{
    [TestClass]
      public class TestFileParser
      {
        [TestMethod]
        public void TestParse()
        {
            var parser = new FileParser.Fakes.StubIParser()
            {
                Parser = () => { return new List<string>() { "Abhishek", "Gaonkar" }; }
            };

            var str = parser.Parser();
            Assert.AreEqual("Abhishek", str[0],"Testing");
        }

        [TestMethod]
        public void MyTestMethod()
        {
            using (ShimsContext.Create())
            {
                System.Fakes.ShimDateTime.NowGet = () => { return new DateTime(2010, 11, 5); };
                var fakeTime = DateTime.Now; // It is always DateTime(2010, 11, 5);
                Console.WriteLine(fakeTime.ToShortDateString());
            }
            var correctTime = DateTime.Now;
            Console.WriteLine(correctTime.ToShortDateString());            
        }
        [TestMethod]
        public void MyTestMethod2()
        {
            var test = new ResumeProcessor.Fakes.StubProcesTemplate 
            {
                 
            };
            using (ShimsContext.Create())
            {
                //ResumeProcessor.Fakes.ShimProcesTemplate = ()=>{Debug.Write("TestPassed");};
            }
        }
          
      }

}
