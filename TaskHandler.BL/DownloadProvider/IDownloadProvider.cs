using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHandler.BL.DownloadProvider
{
    public interface IDownloadProvider
    {
        void ExecuteTask(string downloadPath);
    }
}
