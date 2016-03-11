using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InputExecuter;
using System.IO;

namespace UnitTestProject
{
    [TestClass]
    public class TestFileReader
    {
        [TestMethod]
        public void TestReadFile()
        {
            IFileReader fileReader = new InputExecuter.Fakes.StubIFileReader() 
            {
                ReadFile = ()=>  {return File.Create(@"c:\test\test.txt"); },
            };

            var newFile = fileReader.ReadFile();
            
        }
    }
}
