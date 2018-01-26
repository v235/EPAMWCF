using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WcfServiceApp
{
    public class Global : System.Web.HttpApplication
    {
        public WindsorContainer Container { get; protected set; }

        protected void Application_Start(object sender, EventArgs e)
        {
            Container = new WindsorContainer();
            Container.AddFacility<WcfFacility>();
            Container.Register(
                Component.For<ITaskRepository>().ImplementedBy<TaskRepository>(),
                Component.For<IDownloadService>().ImplementedBy<DownloadService>().Named("DownloadService"));
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}