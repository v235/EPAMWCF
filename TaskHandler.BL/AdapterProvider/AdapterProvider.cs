using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLib;

namespace TaskHandler.BL.AdapterProvider
{
    public class AdapterProvider : SiteDownloader, IAdapterProvider
    {
        public void Download(string downloadPath, string url)
        {
            base.Download(downloadPath, url);
        }
    }
}
