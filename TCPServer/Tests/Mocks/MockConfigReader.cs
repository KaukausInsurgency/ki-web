using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAWKI_TCPServer.Interfaces;

namespace Tests.Mocks
{
    class MockConfigReader : IConfigReader
    {
        private bool success;
        private int port;
        private int maxconn;
        private string mysqlcon;
        private string rediscon;
        private bool upnp;
        private bool usewhitelist;
        private List<string> whitelist;
        private List<string> html;
        private Dictionary<string, string> rediskeys;
        private string redisEnvironmentKey;
        private string version;
        private string versionKey;

        public MockConfigReader()
        {
            port = 9000;
            maxconn = 4;
            mysqlcon = "Server=127.0.0.1; Database=mockdb; Uid=fake; Pwd=fake;";
            rediscon = "mockconnection";
            upnp = false;
            usewhitelist = false;
            whitelist = new List<string>();
            html = new List<string>();
            rediskeys = new Dictionary<string, string>();
            success = true;
            redisEnvironmentKey = "UT";
            version = "0.90";
            versionKey = "6A257BB3-A5EA-4FF7-81D9-B56228BAF1BD";
        }

        public MockConfigReader(List<string> html, Dictionary<string,string> rediskeys)
        {
            port = 9000;
            maxconn = 4;
            mysqlcon = "mockconnection";
            rediscon = "mockconnection";
            upnp = false;
            usewhitelist = false;
            whitelist = new List<string>();
            this.html = html;
            this.rediskeys = rediskeys;
            success = true;
            redisEnvironmentKey = "UT";
            version = "0.90";
            versionKey = "6A257BB3-A5EA-4FF7-81D9-B56228BAF1BD";
        }

        int IConfigReader.PortNumber => port;
        int IConfigReader.MaxConnections => maxconn;
        string IConfigReader.MySQLDBConnect => mysqlcon;
        string IConfigReader.RedisDBConnect => rediscon;
        bool IConfigReader.ConfigReadSuccess => success;
        bool IConfigReader.UseUPnP => upnp;
        bool IConfigReader.UseWhiteList => usewhitelist;
        List<string> IConfigReader.WhiteList => whitelist;
        List<string> IConfigReader.SupportedHTML => html;
        Dictionary<string, string> IConfigReader.RedisActionKeys => rediskeys;
        string IConfigReader.RedisEnvironmentKey => redisEnvironmentKey;
        string IConfigReader.Version => version;
        string IConfigReader.VersionKey => versionKey;
        Dictionary<string, long> IConfigReader.ActionThrottle => new Dictionary<string, long>();
    }
}
