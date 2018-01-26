using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHandler
{
    public static class IoC
    {
        public static ITaskRepository Resolve()
        {
            var container = new WindsorContainer();

            container.Register(Component.For<ITaskRepository>().ImplementedBy<TaskRepository>());
            return container.Resolve<ITaskRepository>();
        } 
    }
}
