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

namespace TaskHandlerTests
{
    [TestFixture()]
    public class SagaTaskTest
    {
        private IDownloadProvider _mockDownloadProvider;
        private IZipProvider _mockZipProvider;
        [SetUp]
        public void SetUp()
        {
            _mockDownloadProvider = MockRepository.GenerateStub<IDownloadProvider>();
            _mockZipProvider = MockRepository.GenerateStub<IZipProvider>();

        }
        [Test]
        public async Task SagaTest()
        {
            //Arrange
            SagaTask saga = new SagaTask(_mockDownloadProvider, _mockZipProvider)
            {
                Data = new PlaceSagaTask()
            };
            var context = new TestableMessageHandlerContext();
            var placeTask=new PlaceTask()
            {
                 TaskId = 1
            };
            //Act
            await saga.Handle(placeTask, context)
                .ConfigureAwait(false);
            var stage1 = (ZipTask)context.SentMessages[0].Message;
            var zipTask = new ZipTask()
            {
                TaskId = stage1.TaskId,
                ZipPath = stage1.ZipPath
            };
            await saga.Handle(zipTask, context)
                .ConfigureAwait(false);
            var stage2 = (ZipTask)context.SentMessages[0].Message;
            //Assert
            Assert.AreEqual(placeTask.TaskId, stage2.TaskId);
        }
    }
}
