using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;

namespace WCFService.BL.NSBus
{
    public interface INSBusProvider
    {
        void SendTaskCreatedMessage(PlaceTask task);
    }
}
