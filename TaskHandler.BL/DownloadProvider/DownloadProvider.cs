using System;
using WebLib;

namespace TaskHandler.BL.DownloadProvider
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
            sd.Download(downloadPath, "https://www.tut.by/");
        }
    }
}
