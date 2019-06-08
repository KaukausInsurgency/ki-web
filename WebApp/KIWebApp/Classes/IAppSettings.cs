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
    }
}