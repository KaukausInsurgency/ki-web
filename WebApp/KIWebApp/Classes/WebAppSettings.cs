using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KIWebApp.Classes
{
    public class WebAppSettings : IAppSettings
    {
        string IAppSettings.RedisKeyCapturePoint => System.Configuration.ConfigurationManager.AppSettings["RedisKeyCapturePoint"];
        string IAppSettings.RedisKeyDepot => System.Configuration.ConfigurationManager.AppSettings["RedisKeyDepot"];
        string IAppSettings.RedisKeySideMission => System.Configuration.ConfigurationManager.AppSettings["RedisKeySideMission"];
        string IAppSettings.RedisKeyChat => System.Configuration.ConfigurationManager.AppSettings["RedisKeyChat"];
        string IAppSettings.RedisEnvironmentPrefix => System.Configuration.ConfigurationManager.AppSettings["RedisEnvironmentPrefix"];
    }
}