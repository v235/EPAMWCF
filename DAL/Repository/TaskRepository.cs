using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DBProvider;
using DAL.Model;

namespace DAL.Repository
{
    public class TaskRepository : ITaskRepository
    {
        string connectionString;
        private readonly IDBProvider _dbProvider;

        public TaskRepository(IDBProvider dbProvider)
        {
            connectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["TaskDB"].ConnectionString;
            _dbProvider = dbProvider;
            _dbProvider.DeployOrUpdateDB(connectionString);
        }

        public TaskEntity GetTaskById(int id)
        {
            TaskEntity task = new TaskEntity();
            string sqlExpression = "SELECT Id, url, status, downloadPath  FROM [TaskDB].[dbo].TaskHolder WHERE Id=@id";
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

        public int AddTask(string url, string status)
        {
            string sqlExpression = "INSERT INTO [TaskDB].[dbo].TaskHolder (url, status) values(@url, @status)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter urlParam = new SqlParameter("@url", url);
                command.Parameters.Add(urlParam);
                SqlParameter statusParam = new SqlParameter("@status", status);
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
            //new
            string sqlExpressionDelete = "DELETE FROM [TaskDB].[dbo].TaskHolder WHERE Id=@id;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpressionDelete, connection);
                SqlParameter idParamDel = new SqlParameter("@id", task.Id);
                command.Parameters.Add(idParamDel);
                try
                {
                    command.ExecuteNonQuery();
                    string sqlExpressionIdentityON = "SET IDENTITY_INSERT [TaskDB].[dbo].TaskHolder ON";
                    SqlCommand commandOn = new SqlCommand(sqlExpressionIdentityON, connection);
                    commandOn.ExecuteNonQuery();
                    string sqlExpressionUpdate =
                        "INSERT INTO [TaskDB].[dbo].TaskHolder(Id, url, status, downloadPath)VALUES(@id, @url, @status, @downloadPath)";
                    SqlCommand commandUp = new SqlCommand(sqlExpressionUpdate, connection);
                    SqlParameter idParam = new SqlParameter("@id", task.Id);
                    commandUp.Parameters.Add(idParam);
                    SqlParameter urlParam = new SqlParameter("@url", task.Url);
                    commandUp.Parameters.Add(urlParam);
                    SqlParameter statusParam = new SqlParameter("@status", task.Status);
                    commandUp.Parameters.Add(statusParam);
                    SqlParameter downloadPathParam = new SqlParameter("@downloadPath", task.DownloadPath);
                    commandUp.Parameters.Add(downloadPathParam);
                    commandUp.ExecuteNonQuery();
                    string sqlExpressionIdentityOff = "SET IDENTITY_INSERT [TaskDB].[dbo].TaskHolder OFF";
                    SqlCommand commandOff = new SqlCommand(sqlExpressionIdentityOff, connection);
                    commandOff.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            //

            //    string sqlExpression = "UPDATE [TaskDB].[dbo].TaskHolder SET status = @status, downloadPath=@downloadPath WHERE Id=@id";
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();
            //    SqlCommand command = new SqlCommand(sqlExpression, connection);
            //SqlParameter idParam = new SqlParameter("@id", task.Id);
            //command.Parameters.Add(idParam);
            //SqlParameter statusParam = new SqlParameter("@status", task.Status);
            //command.Parameters.Add(statusParam);
            //SqlParameter downloadPathParam = new SqlParameter("@downloadPath", task.DownloadPath);
            //command.Parameters.Add(downloadPathParam);
            //    try
            //    {
            //        command.ExecuteNonQuery();
            //    }
            //    catch (Exception e)
            //    {
            //        throw e;
            //    }
            // }
        }

    }
}
