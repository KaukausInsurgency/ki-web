using KIWebApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KIWebApp.Filters
{
    public class InitViewBagFilterProperty : ActionFilterAttribute
    {
        private IAppSettings AppSettings;

        public InitViewBagFilterProperty(IAppSettings appSettings)
        {
            AppSettings = appSettings;
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.DownloadUrlUpdater = AppSettings.UpdaterDownload;
        }
    }
}