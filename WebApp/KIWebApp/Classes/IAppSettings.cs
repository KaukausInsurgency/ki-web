using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KIWebApp.Classes
{
    public interface IAppSettings
    {
        string RedisKeyCapturePoint { get; }
        string RedisKeyDepot { get; }
        string RedisKeySideMission { get; }
        string RedisKeyChat { get; }
        string RedisEnvironmentPrefix { get; }
        string MySqlConnectionString { get; }
        string RedisConnectionString { get; }
        string Version { get; }
        string DCSClientDownload { get; }
        string DCSModDownload { get; }
    }
}