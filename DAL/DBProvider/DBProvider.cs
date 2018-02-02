using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DAL.Model;

namespace DAL.DBProvider
{
    public class DBProvider : IDBProvider
    {
        public void DeployOrUpdateDB(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlExpression = "SELECT * FROM sysdatabases WHERE name=@name";
                using (SqlCommand command = new SqlCommand(sqlExpression, connection))
                {
                    SqlParameter nameParameter = new SqlParameter("@name", "TaskDB");
                    command.Parameters.Add(nameParameter);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            CreateDB(connection);
                        }
                        else
                        {
                            reader.Close();
                            UpdateDb(connection);

                        }
                    }
                }
            }
        }

        private void CreateDB(SqlConnection connection)
        {

            string path = GetScriptPath();
            if (!string.IsNullOrEmpty(path))
            {
                foreach (var f in Directory.GetFiles(path))
                {
                    executeScript(connection, f);
                    Thread.Sleep(1000);
                }
            }
        }

        private void UpdateDb(SqlConnection connection)
        {
            List<MigrationEntity> migrations = new List<MigrationEntity>();
            string migrationScript= "SELECT * FROM [TaskDB].[dbo].MigrationHistory";
            using (SqlCommand command = new SqlCommand(migrationScript, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var migration = new MigrationEntity()
                        {
                            Id = reader.GetInt32(0),
                            CurrentVersion = reader.GetString(1),
                            FileNumber = reader.GetString(2),
                            Comment = reader.GetString(3),
                            DateApplied = reader.GetDateTime(4)
                        };
                        migrations.Add(migration);
                    }
                }
                var m = migrations.OrderByDescending(t => t.DateApplied).FirstOrDefault();
                string path = GetScriptPath();
                if (!string.IsNullOrEmpty(path) && m != null)
                {
                    int currentFileNumber = Convert.ToInt32(m.FileNumber);
                    foreach (var f in Directory.GetFiles(path))
                    {
                        var fileNumber = f.Substring(f.LastIndexOf('\\') + 1,
                            f.LastIndexOf('.') - f.LastIndexOf('\\') - 1);
                        if (Convert.ToInt32(fileNumber) > currentFileNumber)
                        {
                            currentFileNumber = Convert.ToInt32(fileNumber);
                            executeScript(connection, f);
                        }
                    }
                }

            }
        }

        private string GetScriptPath()
        {
            var directoryInfo = Directory.GetParent(Assembly.GetExecutingAssembly()
                .Location).Parent;
            if (directoryInfo != null)
            {
                return Path.Combine(directoryInfo.Parent.Parent.FullName, "DAL", "Database");
            }

            return string.Empty;
        }

        private void executeScript(SqlConnection connection, string scriptPath)
        {
            IEnumerable<string> sqlExpressions = Regex.Split(File.ReadAllText(scriptPath), @"^\s*GO\s*$",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (string sqlExpression in sqlExpressions)
            {
                if (sqlExpression.Trim() != "")
                    using (SqlCommand command = new SqlCommand(sqlExpression, connection))
                    {
                        command.ExecuteNonQuery();
                    }
            }
        }
    }
}
