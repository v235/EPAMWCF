using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;

namespace DAL.Repository
{
    public class TaskRepository : ITaskRepository
    {
        string connectionString;

        public TaskRepository()
        {
            connectionString= @"data source=EPBYBREW0144\;initial catalog=TaskDB;integrated security=True";
        }

        public TaskEntity GetTaskById(int id)
        {
            TaskEntity task = new TaskEntity();
            string sqlExpression = "SELECT Id, url, status, downloadPath  FROM [TaskHolder] WHERE Id=@id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlExpression, connection))
                {
                    SqlParameter idParam = new SqlParameter("@id", id);
                    command.Parameters.Add(idParam);
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                task.Id = reader.GetInt32(0);
                                task.Url = reader.GetString(1);
                                task.Status = reader.GetString(2);
                                if (reader.GetValue(3) == null)
                                    task.DownloadPath = string.Empty;
                                else
                                    task.DownloadPath = reader.GetValue(3).ToString();
                            }
                        }
                        return task;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        public int AddTask(string url)
        {
            string sqlExpression = "INSERT INTO [TaskHolder] (url, status) values(@url, @status)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter urlParam = new SqlParameter("@url", url);
                command.Parameters.Add(urlParam);
                SqlParameter statusParam = new SqlParameter("@status", "processing");
                command.Parameters.Add(statusParam);
                try
                {
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT @@IDENTITY";
                    return Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void UpdateTask(TaskEntity task)
        {
            string sqlExpression = "UPDATE [TaskHolder] SET status = @status, downloadPath=@downloadPath WHERE Id=@id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter idParam = new SqlParameter("@id", task.Id);
                command.Parameters.Add(idParam);
                SqlParameter statusParam = new SqlParameter("@status", "done");
                command.Parameters.Add(statusParam);
                SqlParameter downloadPathParam = new SqlParameter("@downloadPath", task.DownloadPath);
                command.Parameters.Add(downloadPathParam);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
