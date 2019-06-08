using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KIWebApp.Classes
{
    public class WebAppVersionSettings
    {
        public static string VERSION => System.Configuration.ConfigurationManager
            .AppSettings["KIClientVersion"];
        public static string GUID => System.Configuration.ConfigurationManager
            .AppSettings["KIClientGUID"];
        public static string PATH => System.Configuration.ConfigurationManager
            .AppSettings["KIClientDownload"];
    }
}