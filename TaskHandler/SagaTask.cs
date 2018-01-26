using DAL.Repository;
using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHandler
{
    public class SagaTask:
    Saga<PlaceSagaTask>,
    IAmStartedByMessages<PlaceTask>,
    IHandleMessages<ZipTask>
    {
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
            DownloadProvider tp = new DownloadProvider();
            tp.ExecuteTask(@"D:\\5");
            var zipTask = new ZipTask() {
            TaskId= message.TaskId,
            ZipPath = @"D:\\5"};
            return context.SendLocal(zipTask);
        }

        public Task Handle(ZipTask message, IMessageHandlerContext context)
        {
            log.Info($"newTask, ZipPath = {message.ZipPath}");
            ZipProvider zp = new ZipProvider(IoC.Resolve());
            zp.Zip(message.ZipPath, message.TaskId);
            MarkAsComplete();
            return Task.CompletedTask;
        }
    }
}
