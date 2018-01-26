using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHandler
{
    public interface IDownloadProvider
    {
        void ExecuteTask(string downloadPath);
    }
}
