using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHandler.BL.AdapterProvider
{
    public interface IAdapterProvider
    {
        void Download(string downloadPath, string url);
    }
}
