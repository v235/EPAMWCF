using DAL.Model;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLib;

namespace TaskHandler
{
    public class DownloadProvider : IDownloadProvider
    {
        public void ExecuteTask(string downloadPath)
        {
            DownloadSite(downloadPath);
        }
        private void DownloadSite(string downloadPath)
        {
            SiteDownloader sd = new SiteDownloader();
            try
            {
                sd.Download(downloadPath, "https://www.tut.by/");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
