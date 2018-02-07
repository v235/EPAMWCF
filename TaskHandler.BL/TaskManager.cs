using Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskHandler.BL.DownloadProvider;
using TaskHandler.BL.ZipProvider;

namespace TaskHandler.BL
{
    public class TaskManager : ITaskManager
    {
        private readonly IDownloadProvider _downloadProvider;
        private readonly IZipProvider _zipProvider;
        public TaskManager(IDownloadProvider downloadProvider, IZipProvider zipProvider)
        {
            _downloadProvider = downloadProvider;
            _zipProvider = zipProvider;
        }
        public ArchiveTask DownloadSite(int id)
        {
            string dowTempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dowTempPath);
            _downloadProvider.ExecuteTask(dowTempPath, id);
            return new ArchiveTask()
            {
                TaskId = id,
                ZipPath = dowTempPath
            };
        }

        public void ArchiveSite(ArchiveTask task)
        {
            _zipProvider.Zip(task.ZipPath, task.TaskId);
        }

    }
}
