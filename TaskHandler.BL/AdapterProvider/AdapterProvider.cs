using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebLib;

namespace TaskHandler.BL.AdapterProvider
{
    public class AdapterProvider : SiteDownloader, IAdapterProvider
    {
        public void Download(string downloadPath, string url)
        {
            url = UrlValidator(url);
            base.Download(downloadPath, url);
        }
        private string UrlValidator(string url)
        {
            string patternHTTP = @"^http://www\W\w{0,}\W\w{0,}";

            Regex regexHTTP = new Regex(patternHTTP);
            if (regexHTTP.IsMatch(url))
            {
                return url;
            }
            string patternWWW = @"^www\W\w{0,}\W\w{0,}";
            Regex regexWWW = new Regex(patternWWW);
            if (regexWWW.IsMatch(url))
            {
                return "http://" + url;
            }
            return "http://www." + url;
        }
    }
}
