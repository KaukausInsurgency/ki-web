using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAWKI_TCPServer.Interfaces;

namespace Tests.Mocks
{
    class MockLogger : ILogger
    {
        void ILogger.Log(string data)
        {
        }
    }
}
