using System;
using System.IO;
using NUnit.Framework;
using WCFService;
using DAL.Repository;
using Rhino.Mocks;
using System.Net;
using DAL.Model;
using Messages;
using WCFService.ResponseManager;
using WCFService.BL;

namespace WCFServiceTests
{
    [TestFixture]
    public class WCFServerTest
    {
        private DownloadService _service;
        private IMainController _mainController;
        private IResponseProvider _mockResponseProvider;

        [SetUp]
        public void SetUp()
        {
            _mainController = MockRepository.GenerateStub<IMainController>();
            _mockResponseProvider = MockRepository.GenerateStub<IResponseProvider>();
            _service = new DownloadService(_mainController, _mockResponseProvider);
        }

        [Test]
        public void CreateTask_should_Creat_New_Task_and_return_New_Task_id()
        {
            //Arrange
            string url = "test.test";
            _mainController.Stub(t => t.Create(url)).Return(new PlaceTask(){TaskId = 1});
            _mockResponseProvider.Stub(t => t.ResponseCreated(null)).Return(HttpStatusCode.Created);
            //Act
            var result = _service.CreateNewTask(url);
            //Assert
            _mainController.AssertWasCalled(t => t.Create(url));
            _mockResponseProvider.AssertWasCalled(t => t.ResponseCreated(null));
            Assert.AreEqual(result.TaskId, 1);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("test")]
        public void CreateTask_should_return_BadRequest(string url)
        {
            //Arrange
            _mainController.Stub(t => t.Create(url)).Throw(new Exception());
            _mockResponseProvider.Stub(t => t.ResponseCreated(null)).Return(HttpStatusCode.BadRequest);
            //Act
            _service.CreateNewTask(url);
            //Assert
            _mockResponseProvider.AssertWasCalled(t => t.ResponseBadRequest(null));
        }

        [Test]
        public void GetTaskStatus_should_return_Task_status()
        {
            //Arrange
            string id = "1";
            _mainController.Stub(t => t.GetStatus(id))
                .Return(new TaskStatus() {Id = 1, Status = "done"});
            _mockResponseProvider.Stub(t => t.ResponseOk(null)).Return(HttpStatusCode.OK);
            //Act
            var result = _service.GetTaskStatus(id);
            //Assert
            _mainController.AssertWasCalled(t => t.GetStatus(id));
            _mockResponseProvider.AssertWasCalled(t => t.ResponseOk(null));
            Assert.AreEqual(result.Id, 1);
            Assert.AreEqual(result.Status, "done");
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("1")]
        public void GetTaskStatus_should_return_BadRequest(string id)
        {
            //Arrange
            _mainController.Stub(t => t.GetStatus(id)).Throw(new Exception());
            _mockResponseProvider.Stub(t => t.ResponseBadRequest(null)).Return(HttpStatusCode.BadRequest);
            //Act
            _service.GetTaskStatus(id);
            //Assert
            _mockResponseProvider.AssertWasCalled(t => t.ResponseBadRequest(null));
        }

        [Test]
        public void Download_should_download_file()
        {
            //Arrange
            string id = "1";
            string fileName = string.Empty;
            MemoryStream stream=new MemoryStream(new byte[10]);
            _mainController.Stub(t => t.Download(id, ref fileName)).Return(stream);
            _mockResponseProvider.Stub(t => t.ResponseContentType(null)).Return("");
            _mockResponseProvider.Stub(t => t.ResponseBadRequest(null)).Return(HttpStatusCode.OK);
            //Act
            var result = _service.Download(id);
            //Assert
            _mainController.AssertWasCalled(t => t.Download(id, ref fileName));
            _mockResponseProvider.AssertWasCalled(t => t.ResponseContentType(null));
            _mockResponseProvider.AssertWasCalled(t => t.ResponseOk(null));
            Assert.AreEqual(result.Length, 10);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("1")]
        public void Download_should_return_BadRequest(string id)
        {
            //Arrange
            string fileName = string.Empty;
            _mainController.Stub(t => t.Download(id, ref fileName)).Throw(new Exception());
            _mockResponseProvider.Stub(t => t.ResponseBadRequest(null)).Return(HttpStatusCode.BadRequest);
            //Act
            _service.Download(id);
            //Assert
            _mockResponseProvider.AssertWasCalled(t => t.ResponseBadRequest(null));
        }
    }
}
