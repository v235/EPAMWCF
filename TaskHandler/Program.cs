﻿using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHandler
{
    class Program
    {
        static void Main(string[] args)
        {

            AsyncMain().GetAwaiter().GetResult();

        }

        static async Task AsyncMain()
        {
            var endpointConfiguration = new EndpointConfiguration("TaskHandler");
            var container = new IoC(new WindsorContainer());

            endpointConfiguration.UseContainer<WindsorBuilder>(
                customizations => { customizations.ExistingContainer(container.Init()); });
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();
            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Delayed(delayed => delayed.NumberOfRetries(0));
            endpointConfiguration.UseSerialization<JsonSerializer>();
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);

            container.Dispose();
        }
    }
}
