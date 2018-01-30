using System.IO;
using System.Reflection;
using DAL.Model;
using DAL.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Rhino.Mocks;
using TaskHandler.BL.ZipProvider;

namespace TaskHandlerBL.Tests
{
    [TestFixture()]
    class ZipProviderTest
    {
        private ITaskRepository _mockTaskRepository;
        string _testDirectoryPath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Test");

        private FileInfo fi;

        [SetUp]
        public void Setup()
        {
            Directory.CreateDirectory(_testDirectoryPath);
        }

        [Test]
        public void Should_create_zip_file()
        {
            //Arrange
            _mockTaskRepository = MockRepository.GenerateStub<ITaskRepository>();
            var task = new TaskEntity()
            {
                Status = "done",
                DownloadPath = _testDirectoryPath
            };
            _mockTaskRepository.Stub(t => t.GetTaskById(1)).Return(task);
            _mockTaskRepository.Stub(t => t.UpdateTask(task));
            ZipProvider zp=new ZipProvider(_mockTaskRepository);
            //Act
            zp.Zip(_testDirectoryPath, 1);

            //Assert
            _mockTaskRepository.AssertWasCalled(t=>t.GetTaskById(1));
            _mockTaskRepository.AssertWasCalled(t=>t.UpdateTask(task));
            fi = new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Test.zip"));
            Assert.AreEqual(fi.Exists, true);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(_testDirectoryPath);
            File.Delete(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Test.zip"));
        }
    }
}
