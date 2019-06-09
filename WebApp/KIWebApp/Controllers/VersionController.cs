using KIWebApp.Classes;
using KIWebApp.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KIWebApp.Controllers
{
    public class VersionController : ApiController
    {
        readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IAppSettings AppSettings;
        private IDAL_Meta DAL;

        public VersionController()
        {
            AppSettings = new WebAppSettings();
            DAL = new DAL_Meta();
        }

        // GET api/version
        [HttpGet]       
        public HttpResponseMessage Get()
        {
            Logger.Info("GetVersionInfo requested");
            VersionResponse result = GetVersionInfo();
            if (result != null)
                return Request.CreateResponse(HttpStatusCode.OK, result);
            else
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error obtaining version information");
        }

        private VersionResponse GetVersionInfo()
        {
            VersionInfoModel model = DAL.GetVersionInfo();
            if (model == null)
                return null;

            string appPath = Classes.WebUtility.GetApplicationPath(System.Web.HttpContext.Current);
            string downloadURIClient = $"{appPath}{AppSettings.DCSClientDownload}";
            string downloadURIMod = $"{appPath}{AppSettings.DCSModDownload}";

            return new VersionResponse(model)
            {
                DownloadURLClient = downloadURIClient,
                DownloadURLMod = downloadURIMod
            };
        }

        private class VersionResponse
        {
            public string DCSClientVersion { get; set; }
            public string DCSClientGUID { get; set; }
            public string DCSModVersion { get; set; }
            public string DCSModGUID { get; set; }
            public string DownloadURLMod { get; set; }
            public string DownloadURLClient { get; set; }

            public VersionResponse(VersionInfoModel model)
            {
                DCSClientGUID = model.DCSClientGUID;
                DCSClientVersion = model.DCSClientVersion;
                DCSModGUID = model.DCSModGUID;
                DCSModVersion = model.DCSModVersion;
            }
        }
    }
}
