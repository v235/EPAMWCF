using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Messages;
using TaskStatus = DAL.Model.TaskStatus;

namespace Client
{
    public class HTTPClient
    {

        public async Task<int> POSTTask(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(
                        new Uri("http://localhost:8780/WCFService/download?url=" + url), new StringContent(""));
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<PlaceTask>(content);
                        return res.TaskId;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return 0;
        }

        public async Task<string> GETTaskStatus(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(
                        new Uri("http://localhost:8780/WCFService/download/" + id + "/status"));
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<TaskStatus>(content);
                        return res.Status;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return string.Empty;
        }

        public async Task<string> GETTaskDownload(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(
                        new Uri("http://localhost:8780/WCFService/download/" + id + "/artifact"));
                    if (response.IsSuccessStatusCode)
                    {
                        var inputStream = await response.Content.ReadAsStreamAsync();
                        using (Stream outputStream =
                            new FileStream(
                                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                    response.Content.Headers.ContentDisposition.FileName), FileMode.OpenOrCreate))
                        {
                            byte[] buffer = new byte[4096];
                            int bytesRead;
                            do
                            {
                                bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                                outputStream.Write(buffer, 0, bytesRead);
                            } while (bytesRead != 0);

                            return "File download to path:" +
                                   Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                       response.Content.Headers.ContentDisposition.FileName);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "File isn`t download";
        }
    }
}
