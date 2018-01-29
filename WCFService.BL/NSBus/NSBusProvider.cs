using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace WCFService.BL.NSBus
{
    public class NSBusProvider : INSBusProvider
    {
        public void SendTaskCreatedMessage(PlaceTask task)
        {
            SendCommand(task);
        }

        private async void SendCommand(PlaceTask task)
        {
            var endpointConfiguration = new EndpointConfiguration("WCFService");

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PlaceTask), "TaskHandler");

            endpointConfiguration.UseSerialization<JsonSerializer>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            await endpointInstance.Send(task)
                .ConfigureAwait(false);

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
