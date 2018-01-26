using System;
using System.Net;
using System.IO;
using DAL.Model;
using DAL.Repository;
using System.ServiceModel.Web;
using Messages;
using NServiceBus;

namespace WcfServiceApp
{
    public class DownloadService : IDownloadService
    {
        private readonly ITaskRepository _taskRepository;

        public DownloadService()
        {
            _taskRepository = new TaskRepository();
        }

        public PlaceTask CreateTask(string url)
        {
            PlaceTask cTask = new PlaceTask();
            try
            {
                cTask.TaskId = _taskRepository.AddTask(url);
            }
            catch
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
            }
            SendCommand(cTask);
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Created;
            return cTask;

        }

        public TaskStatus GetTaskStatus(string id)
        {
            try
            {
                TaskEntity task = _taskRepository.GetTaskById(Convert.ToInt32(id));
                TaskStatus taskStatus = new TaskStatus()
                {
                    Id = task.Id,
                    Status = task.Status
                };
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return (taskStatus);
            }
            catch
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                return new TaskStatus();
            }
        }

        public Stream Download(string id)
        {
            try
            {

                TaskEntity task = _taskRepository.GetTaskById(Convert.ToInt32(id));
                if (string.Equals(task.Status, "done", StringComparison.OrdinalIgnoreCase))
                {
                    FileInfo fi = new FileInfo(task.DownloadPath);
                    if (fi.Exists)
                    {
                        WebOperationContext.Current.OutgoingResponse.ContentType = "application/octet-stream";
                        WebOperationContext.Current.OutgoingResponse.Headers.Add("content-disposition", "inline; filename=" + fi.Name);
                        FileStream fs = new FileStream(task.DownloadPath, FileMode.Open, FileAccess.Read);
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                        return (fs);
                    }
                }
            }
            catch
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                return null;
            }
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
            return null;
        }
        #region NSB
        private async void SendCommand(PlaceTask task)
        {
            var endpointConfiguration = new EndpointConfiguration("WCFService");

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PlaceTask), "TaskHandler");

            endpointConfiguration.UseSerialization<JsonSerializer>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            await endpointInstance.Send(task)
                          .ConfigureAwait(false);

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
        #endregion
    }
}