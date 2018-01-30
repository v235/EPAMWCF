using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;
using TaskHandler.BL.DownloadProvider;
using TaskHandler.BL.ZipProvider;
using System;

namespace TaskHandler
{
    public class SagaTask :
        Saga<PlaceSagaTask>,
        IAmStartedByMessages<PlaceTask>,
        IHandleMessages<ZipTask>
    {
        private readonly IDownloadProvider _downloadProvider;
        private readonly IZipProvider _zipProvider;

        public SagaTask(IDownloadProvider downloadProvider, IZipProvider zipProvider)
        {
            _downloadProvider = downloadProvider;
            _zipProvider = zipProvider;
        }

        static ILog log = LogManager.GetLogger<SagaTask>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<PlaceSagaTask> mapper)
        {
            mapper.ConfigureMapping<PlaceTask>(message => message.TaskId)
                .ToSaga(sagaData => sagaData.TaskId);

            mapper.ConfigureMapping<ZipTask>(message => message.TaskId)
                .ToSaga(sagaData => sagaData.TaskId);
        }

        public Task Handle(PlaceTask message, IMessageHandlerContext context)
        {
            log.Info($"newTask, TaskId = {message.TaskId}");
            _downloadProvider.ExecuteTask(@"D:\\5");
            var zipTask = new ZipTask()
            {
                TaskId = message.TaskId,
                ZipPath = @"D:\\5"
            };
            return context.SendLocal(zipTask);
        }

        public Task Handle(ZipTask message, IMessageHandlerContext context)
        {
            log.Info($"newTask, ZipPath = {message.ZipPath}");
            _zipProvider.Zip(message.ZipPath, message.TaskId);
            MarkAsComplete();
            return Task.CompletedTask;
        }
    }
}
