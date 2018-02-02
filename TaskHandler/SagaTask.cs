using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;
using TaskHandler.BL.DownloadProvider;
using TaskHandler.BL.ZipProvider;
using System;
using System.IO;
using TaskHandler.BL;

namespace TaskHandler
{
    public class SagaTask :
        Saga<PlaceSagaTask>,
        IAmStartedByMessages<PlaceTask>,
        IHandleMessages<ArchiveTask>
    {
        private readonly ITaskManager _taskManager;

        public SagaTask(ITaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        static ILog log = LogManager.GetLogger<SagaTask>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<PlaceSagaTask> mapper)
        {
            mapper.ConfigureMapping<PlaceTask>(message => message.TaskId)
                .ToSaga(sagaData => sagaData.TaskId);

            mapper.ConfigureMapping<ArchiveTask>(message => message.TaskId)
                .ToSaga(sagaData => sagaData.TaskId);
        }

        public Task Handle(PlaceTask message, IMessageHandlerContext context)
        {
            log.Info($"newTask, TaskId = {message.TaskId}");
            var zipTask = _taskManager.DownloadSite(message.TaskId);
            return context.SendLocal(zipTask);
        }

        public Task Handle(ArchiveTask message, IMessageHandlerContext context)
        {
            log.Info($"newTask, ZipPath = {message.ZipPath}");
            _taskManager.ArchiveSite(message);
            MarkAsComplete();
            return Task.CompletedTask;
        }
    }
}
