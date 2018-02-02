using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;

namespace TaskHandler.BL
{
    public interface ITaskManager
    {
        ArchiveTask DownloadSite(int id);
        void ArchiveSite(ArchiveTask task);
    }
}
