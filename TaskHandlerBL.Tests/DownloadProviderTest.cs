using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using DAL.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Rhino.Mocks;
using TaskHandler.BL.AdapterProvider;
using TaskHandler.BL.DownloadProvider;

namespace TaskHandlerBL.Tests
{
    [TestFixture()]
    public class DownloadProviderTest
    {
        private ITaskRepository _mockTaskRepository;
        private IAdapterProvider _mockAdapterProvider;

        [SetUp]
        public void Setup()
        {
            _mockTaskRepository = MockRepository.GenerateStub<ITaskRepository>();
            _mockAdapterProvider = MockRepository.GenerateStub<IAdapterProvider>();
        }

        [Test]
        public void Should_execute_task()
        {
            //Arrange
            var task = new TaskEntity() { Id = 1, Url = "test", DownloadPath = "test" };
            _mockTaskRepository.Stub(t => t.GetTaskById(task.Id)).Return(task);
            DownloadProvider dp=new DownloadProvider(_mockTaskRepository, _mockAdapterProvider);
            //Act
            dp.ExecuteTask(task.DownloadPath, task.Id);
            //Assert
            _mockTaskRepository.AssertWasCalled(t=>t.GetTaskById(task.Id));
            _mockAdapterProvider.AssertWasCalled(t=>t.Download(task.DownloadPath, task.Url));
        }
    }
}
