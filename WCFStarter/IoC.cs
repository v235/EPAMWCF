using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System.ServiceModel;
using DAL.Repository;
using WCFService;
using System.ServiceModel.Web;
using System;

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
                Component.For<ITaskRepository>().ImplementedBy<TaskRepository>());

            return _container;
        }
        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
