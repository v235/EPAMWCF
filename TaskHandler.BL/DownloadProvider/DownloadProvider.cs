using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAL.Repository;
using TaskHandler.BL.AdapterProvider;
using WebLib;

namespace TaskHandler.BL.DownloadProvider
{
    public class DownloadProvider : IDownloadProvider
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IAdapterProvider _adapterProvider;
        public DownloadProvider(ITaskRepository taskRepository, IAdapterProvider adapterProvider)
        {
            _taskRepository = taskRepository;
            _adapterProvider = adapterProvider;
        }
        public void ExecuteTask(string downloadPath, int id)
        {
            var task = _taskRepository.GetTaskById(id);
            task.Status = "processing";
            _taskRepository.UpdateTask(task);
             DownloadSite(downloadPath, task.Url );
        }
        private void DownloadSite(string downloadPath, string url)
        {
            _adapterProvider.Download(downloadPath, url);
        }


    }
}
