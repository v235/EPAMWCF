using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    internal class MigrationEntity
    {
        public int Id { get; set; }
        public string CurrentVersion { get; set; }
        public string FileNumber { get; set; }
        public string Comment { get; set; }
        public DateTime DateApplied { get; set; }
    }
}
