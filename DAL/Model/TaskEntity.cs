using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class TaskEntity
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Status { get; set; }

        public string DownloadPath { get; set; }
    }
}
