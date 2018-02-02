using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            TaskCreaterClient().GetAwaiter().GetResult();
        }
        static async Task TaskCreaterClient()
        {
            while (true)
            {
                Console.WriteLine("Please add new task. Enter site url:");
                string url=Console.ReadLine();
                if (!string.IsNullOrEmpty(url))
                {
                    HTTPClient client = new HTTPClient();
                    int taskId = await client.POSTTask(url);
                    if (taskId > 0)
                    {
                        string status = string.Empty;
                        do
                        {
                            Thread.Sleep(5000);
                            status = await client.GETTaskStatus(taskId);
                            Console.WriteLine("Task status is: {0}", status);
                        } while (!status.Equals("done"));
                        string message=await client.GETTaskDownload(taskId);
                        Console.WriteLine(message);
                    }
                }
            }
        }
    }
}
