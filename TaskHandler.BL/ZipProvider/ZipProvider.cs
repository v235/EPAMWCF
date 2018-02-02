using DAL.Model;
using DAL.Repository;
using System.IO;
using System.IO.Compression;
using System.Linq;

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
            var fileDir = Directory.GetDirectories(path).FirstOrDefault();
            string name = fileDir.Substring(fileDir.LastIndexOf('\\') + 1) + ".zip";
            string fullName = Path.Combine(Path.GetTempPath(), name);
            FileInfo fi = new FileInfo(fullName);
            if (fi.Exists)
            {
                fi.Delete();
            }

            ZipFile.CreateFromDirectory(fileDir, fullName, CompressionLevel.Fastest, true);

            Directory.Delete(path, true);

            var task = _taskRepository.GetTaskById(id);
            task.Status = "done";
            task.DownloadPath = name;
            UpdateTaskStatusInDB(task);
        }

        private void UpdateTaskStatusInDB(TaskEntity task)
        {
            _taskRepository.UpdateTask(task);
        }
    }
}
