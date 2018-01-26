using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public class ZipTask :
        ICommand
    {
        public int TaskId { get; set; }
        public string ZipPath { get; set; }
    }
}
