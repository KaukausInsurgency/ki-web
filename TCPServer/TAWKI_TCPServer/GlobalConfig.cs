using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAWKI_TCPServer.Interfaces;

namespace TAWKI_TCPServer
{
    class GlobalConfig
    {
        private static IConfigReader Config = null;
        public static void SetConfig(IConfigReader config)
        {
            Config = config;
        }

        public static IConfigReader GetConfig()
        {
            return Config;
        }
    }
}
