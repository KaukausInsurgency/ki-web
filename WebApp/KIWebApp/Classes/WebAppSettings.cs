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
        string IAppSettings.MySqlConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["DBMySqlConnect"].ConnectionString;
        string IAppSettings.RedisConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["DBRedisConnect"].ConnectionString;
        string IAppSettings.Version => System.Configuration.ConfigurationManager.AppSettings["Version"];
        string IAppSettings.DCSClientDownload => System.Configuration.ConfigurationManager.AppSettings["DCSClientDownload"];
        string IAppSettings.DCSModDownload => System.Configuration.ConfigurationManager.AppSettings["DCSModDownload"];
        string IAppSettings.UpdaterDownload => System.Configuration.ConfigurationManager.AppSettings["UpdaterDownload"];
    }
}