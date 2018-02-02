using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLib.CustomExceptions
{
    public class UrnownRule:Exception
    {
        public UrnownRule(string message):base(message)
        {

        }
    }
}
