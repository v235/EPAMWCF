using System;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Rhino.Mocks;
using TaskHandler;
using TaskHandler.BL.DownloadProvider;
using TaskHandler.BL.ZipProvider;
using NServiceBus.Testing;
using Messages;
using TaskHandler.BL;

namespace TaskHandlerTests
{
    [TestFixture()]
    public class SagaTaskTest
    {
        private ITaskManager _mockTaskManager;
        [SetUp]
        public void SetUp()
        {
            _mockTaskManager = MockRepository.GenerateStub<ITaskManager>();
        }
        [Test]
        public async Task SagaTest()
        {
            //Arrange
            SagaTask saga = new SagaTask(_mockTaskManager)
            {
                Data = new PlaceSagaTask()
            };
            var context = new TestableMessageHandlerContext();
            var placeTask=new PlaceTask()
            {
                 TaskId = 1
            };
            var zipTask = new ArchiveTask()
            {
                TaskId = placeTask.TaskId,
                ZipPath = ""
            };
            _mockTaskManager.Stub(t => t.DownloadSite(placeTask.TaskId)).Return(zipTask);
            //Act
            await saga.Handle(placeTask, context)
                .ConfigureAwait(false);
            var stage1 = (ArchiveTask)context.SentMessages[0].Message;

            await saga.Handle(zipTask, context)
                .ConfigureAwait(false);
            var stage2 = (ArchiveTask)context.SentMessages[0].Message;
            //Assert
            _mockTaskManager.AssertWasCalled(t=>t.DownloadSite(stage1.TaskId));
            _mockTaskManager.AssertWasCalled(t=>t.ArchiveSite(stage1));
            Assert.AreEqual(placeTask.TaskId, stage2.TaskId);
        }
    }
}
