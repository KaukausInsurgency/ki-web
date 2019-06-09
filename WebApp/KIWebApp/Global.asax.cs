using KIWebApp.Classes;
using KIWebApp.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KIWebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MvcApplication));

        public override void Init()
        {
            base.Init();
            this.Error += Application_Error;
        }

        void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            logger.Error(ex);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalFilters.Filters.Add(new InitViewBagFilterProperty(new WebAppSettings()), 0);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = true;
            //AuthConfig.RegisterAuth();

            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Log4net.config")));
        }
    }
}