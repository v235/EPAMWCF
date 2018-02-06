using DAL.Repository;
using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WCFService.BL.NSBus;
using System.IO;
using DAL.Model;
using WCFService.BL.FileProvider;

namespace WCFService.BL
{
    public class MainController:IMainController
    {
        private readonly ITaskRepository _taskRepository;
        private readonly INSBusProvider _nsBusProvider;
        private readonly IFileProvider _fileProvider;

        public MainController(ITaskRepository taskRepository, INSBusProvider nsBusProvider, IFileProvider fileProvider)
        {
            _taskRepository = taskRepository;
            _nsBusProvider = nsBusProvider;
            _fileProvider = fileProvider;
        }

        public PlaceTask Create(string url)
        {
            PlaceTask cTask = new PlaceTask
            {
                TaskId = _taskRepository.AddTask(url, "pending")
            };
            _nsBusProvider.SendTaskCreatedMessage(cTask);
            return cTask;
        }

        public TaskStatus GetStatus(string id)
        {
            TaskEntity task = _taskRepository.GetTaskById(Convert.ToInt32(id));
            return  new TaskStatus()
            {
                Id = task.Id,
                Status = task.Status
            };

        }

        public Stream Download(string id, ref string fileName)
        {
            TaskEntity task = _taskRepository.GetTaskById(Convert.ToInt32(id));
            if (string.Equals(task.Status, "done", StringComparison.OrdinalIgnoreCase))
            {
                return _fileProvider.ReadFile(Path.GetTempPath()+task.DownloadPath, ref fileName);
            }

            return null;
        }


    }
}
