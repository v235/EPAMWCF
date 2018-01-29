using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFService.BL.FileProvider
{
    public class FileProvider : IFileProvider
    {
        public Stream ReadFile(string path, ref string fileName)
        {
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                fileName = fi.Name;
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                return stream;
            }

            return null;
        }
    }
}
