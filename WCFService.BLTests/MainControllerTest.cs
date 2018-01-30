using System;
using System.IO;
using NUnit.Framework;
using DAL.Repository;
using Rhino.Mocks;
using WCFService.BL;
using WCFService.BL.FileProvider;
using WCFService.BL.NSBus;
using Messages;
using DAL.Model;

namespace WCFService.BLTests
{
    [TestFixture]
    public class MainControllerTest
    {
        private MainController _mainController;
        private ITaskRepository _mockTaskRepository;
        private INSBusProvider _mockNsBusProvider;
        private IFileProvider _mockFileProvider;

        [SetUp]
        public void SetUp()
        {
            _mockTaskRepository = MockRepository.GenerateStub<ITaskRepository>();
            _mockNsBusProvider = MockRepository.GenerateStub<INSBusProvider>();
            _mockFileProvider = MockRepository.GenerateStub<IFileProvider>();
            _mainController = new MainController(_mockTaskRepository, _mockNsBusProvider, _mockFileProvider);

        }

        [Test]
        public void Should_create_task()
        {
            //Arrange
            string url = "tut.by";
            int expectedId = 1;
            _mockTaskRepository.Stub(t => t.AddTask(url)).Return(expectedId);
            //Act
            var result = _mainController.Create(url);
            //Assert
            _mockTaskRepository.AssertWasCalled(t => t.AddTask(url));
            _mockNsBusProvider.AssertWasCalled(t => t.SendTaskCreatedMessage(result));
            Assert.AreEqual(result.TaskId, expectedId);
        }

        [Test]
        public void Should_return_task_status()
        {
            //Arrange
            string id = "1";
            int TaskId = 1;
            TaskStatus expectedTask = new TaskStatus() {Id = TaskId, Status = "done"};
            _mockTaskRepository.Stub(t => t.GetTaskById(TaskId))
                .Return(new TaskEntity() {Id = TaskId, Status = "done"});
            //Act
            var result = _mainController.GetStatus(id);
            //Assert
            _mockTaskRepository.AssertWasCalled(t => t.GetTaskById(TaskId));
            Assert.AreEqual(result.Id, expectedTask.Id);
            Assert.AreEqual(result.Status, expectedTask.Status);
        }

        [Test]
        public void Should_return_download_stream_if_status_done()
        {
            //Arrange
            string id = "1";
            string fileName = string.Empty;
            int TaskId = 1;
            _mockTaskRepository.Stub(t => t.GetTaskById(TaskId))
                .Return(new TaskEntity() { Id = TaskId, Status = "done", DownloadPath = ""});
            _mockFileProvider.Stub(t => t.ReadFile("", ref fileName)).Return(new MemoryStream(new byte[10]));
            //Act
            var result = _mainController.Download(id, ref fileName);
            //Assert
            _mockTaskRepository.AssertWasCalled(t => t.GetTaskById(TaskId));
            Assert.AreEqual(result.Length, 10);
        }

        [Test]
        public void Should_return_null_if_status_not_done()
        {
            //Arrange
            string id = "1";
            string fileName = string.Empty;
            int TaskId = 1;
            _mockTaskRepository.Stub(t => t.GetTaskById(TaskId))
                .Return(new TaskEntity() { Id = TaskId, Status = "processing", DownloadPath = "" });
            _mockFileProvider.Stub(t => t.ReadFile("", ref fileName)).Return(new MemoryStream(new byte[10]));
            //Act
            var result = _mainController.Download(id, ref fileName);
            //Assert
            _mockTaskRepository.AssertWasCalled(t => t.GetTaskById(TaskId));
            Assert.AreEqual(result, null);
        }
    }
}
