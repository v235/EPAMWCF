using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System.ServiceModel;
using DAL.Repository;
using WCFService;
using System.ServiceModel.Web;
using System;
using WCFService.BL;
using WCFService.BL.FileProvider;
using WCFService.ResponseManager;
using WCFService.BL.NSBus;

namespace WCFStarter
{
    internal class IoC
    {
        IWindsorContainer _container;
        public IoC(IWindsorContainer container)
        {
            _container = container;
        }

        public IWindsorContainer Init()
        {
            _container.Register(
                Component.For<IDownloadService>().ImplementedBy<DownloadService>(),
                Component.For<IMainController>().ImplementedBy<MainController>(),
                Component.For<IResponseProvider>().ImplementedBy<ResponceProvider>(),
                Component.For<ITaskRepository>().ImplementedBy<TaskRepository>(),
                Component.For<INSBusProvider>().ImplementedBy<NSBusProvider>(),
                Component.For<IFileProvider>().ImplementedBy<FileProvider>());
            return _container;
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
