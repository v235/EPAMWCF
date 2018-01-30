using DAL.Model;
using DAL.Repository;
using System.IO;
using System.IO.Compression;

namespace TaskHandler.BL.ZipProvider
{
    public class ZipProvider:IZipProvider
    {
        private readonly ITaskRepository _taskRepository;
        public ZipProvider(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public void Zip(string path, int id)
        {
            string parent = Path.GetDirectoryName(path);
            string name = Path.GetFileName(path);
            string fullName = Path.Combine(parent, name + ".zip");
            ZipFile.CreateFromDirectory(path,fullName, CompressionLevel.Fastest, true);
            var task = _taskRepository.GetTaskById(id);
            task.Status = "done";
            task.DownloadPath = fullName;
            UpdateTaskStatusInDB(task);
        }
        private void UpdateTaskStatusInDB(TaskEntity task)
        {
            _taskRepository.UpdateTask(task);
        }
    }
}
