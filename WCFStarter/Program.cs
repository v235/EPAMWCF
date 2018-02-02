using Castle.Windsor;
using DAL.Repository;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using Castle.Core.Logging;
using WCFService;


namespace WCFStarter
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IoC(new WindsorContainer());

            using (var host = new WebServiceHost((DownloadService)container.Init().Resolve<IDownloadService>(),
                new Uri("http://localhost:8780/WCFService")))
            {
                host.Open();
                Console.WriteLine("Service started");
                Console.ReadKey();
            }
            Console.WriteLine("Service closed");
            Console.ReadKey();
            container.Dispose();
        }
    }
}
