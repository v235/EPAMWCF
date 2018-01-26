using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHandler
{
    public interface IZipProvider
    {
        void Zip(string path, int id);
    }
}
