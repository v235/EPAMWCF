using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskHandler.BL.AdapterProvider;
using TaskHandler.BL.DownloadProvider;
using TaskHandler.BL.ZipProvider;
using WebLib;

namespace TaskHandler
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
                Component.For<IDownloadProvider>().ImplementedBy<DownloadProvider>(),
                Component.For<IZipProvider>().ImplementedBy<ZipProvider>(),
                Component.For<ITaskRepository>().ImplementedBy<TaskRepository>(),
                Component.For<IAdapterProvider>().ImplementedBy<AdapterProvider>());
            return _container;
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
