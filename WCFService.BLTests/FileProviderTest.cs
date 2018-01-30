using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using WCFService.BL.FileProvider;

namespace WCFService.BLTests
{
    [TestFixture]
    public class FileProviderTest
    {
        string _testFilePath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "test.txt");

        [SetUp]
        public void Setup()
        {
            FileStream fs = new FileStream(_testFilePath, FileMode.OpenOrCreate);
            byte[] array = System.Text.Encoding.Default.GetBytes("testString");
            fs.Write(array, 0, array.Length);
            fs.Close();
        }

        [Test]
        public void Should_read_file()
        {
            //Arrange
            string fileName = string.Empty;
            string expectedFileName = "test.txt";
            int expectedLength = 10;
            var provider = new FileProvider();
            //Act
            var result = provider.ReadFile(_testFilePath, ref fileName);
            //Assert
            Assert.AreEqual(expectedFileName, fileName);
            Assert.AreEqual(result.Length, expectedLength);
            result.Close();
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(_testFilePath);
        }
    }
}
