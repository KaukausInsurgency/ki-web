using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using log4net.Config;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(KIAPI.App_Start.Startup))]
[assembly: XmlConfigurator(ConfigFile = "Log4net.config", Watch = true)]

namespace KIAPI.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            app.MapSignalR(hubConfiguration);
        }
    }
}
