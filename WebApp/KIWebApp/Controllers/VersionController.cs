using KIWebApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KIWebApp.Controllers
{
    public class VersionController : ApiController
    {
        // GET api/version
        [HttpGet]       
        public HttpResponseMessage Get()
        {
            VersionResponse result = GetSpecificVersion(WebAppVersionSettings.VERSION,
                                                        WebAppVersionSettings.GUID,
                                                        WebAppVersionSettings.PATH);    
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // GET api/version/type
        [HttpGet]   
        public HttpResponseMessage Get(string id)
        {
            VersionResponse result;
            if (id.ToLower() == "test")
                result = GetSpecificVersion(WebAppVersionSettings.VERSION, WebAppVersionSettings.GUID, WebAppVersionSettings.PATH);
            else if (id.ToLower() == "prod")
                result = GetSpecificVersion(WebAppVersionSettings.VERSION, WebAppVersionSettings.GUID, WebAppVersionSettings.PATH);
            else
                result = new VersionResponse() { Version = "Bad Version Type", GUID = "", DownloadURL = "Bad Version Type" };

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        private VersionResponse GetSpecificVersion(string version, string guid, string path)
        {
            string appPath = string.Format("{0}://{1}{2}{3}",
              System.Web.HttpContext.Current.Request.Url.Scheme,
              System.Web.HttpContext.Current.Request.Url.Host,
              System.Web.HttpContext.Current.Request.Url.Port == 80 ? string.Empty : ":" + System.Web.HttpContext.Current.Request.Url.Port,
              System.Web.HttpContext.Current.Request.ApplicationPath);
            string downloadURI = appPath + path;

            return new VersionResponse(){ Version = version, GUID = guid, DownloadURL = downloadURI };
        }

        private class VersionResponse
        {
            public string Version { get; set; }
            public string GUID { get; set; }
            public string DownloadURL { get; set; }
        }
    }
}
