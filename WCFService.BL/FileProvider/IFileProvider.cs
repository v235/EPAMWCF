using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFService.BL.FileProvider
{
    public interface IFileProvider
    {
        Stream ReadFile(string path, ref string fileName);
    }
}
