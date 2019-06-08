using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAWKI_TCPServer.Interfaces
{
    public interface IConfigReader
    {
        int PortNumber { get; }
        int MaxConnections { get; }
        string MySQLDBConnect { get; }
        string RedisDBConnect { get; }
        bool ConfigReadSuccess { get; }
        bool UseUPnP { get; }
        bool UseWhiteList { get; }
        List<string> WhiteList { get; }
        List<string> SupportedHTML { get; }
        Dictionary<string, RedisAction> RedisActionKeys { get; }
        string RedisEnvironmentKey { get; }
        string Version { get; }
        string VersionKey { get; }
        Dictionary<string, long> ActionThrottle { get; }
    }
}
