using DAL.Model;

namespace DAL.Repository
{
    public interface ITaskRepository
    {
        TaskEntity GetTaskById(int id);
        int AddTask(string url);
        void UpdateTask(TaskEntity task);
    }
}
