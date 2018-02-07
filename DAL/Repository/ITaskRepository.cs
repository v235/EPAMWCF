using System;
using System.Threading.Tasks;
using DAL.Model;

namespace DAL.Repository
{
    public interface ITaskRepository
    {
        TaskEntity GetTaskById(int id);
        int AddTask(string url, string status);
        void UpdateTask(TaskEntity task);
    }
}
