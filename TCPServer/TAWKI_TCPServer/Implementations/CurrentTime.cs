using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAWKI_TCPServer.Interfaces;

namespace TAWKI_TCPServer.Implementations
{
    class CurrentTime : ITimer
    {
        long ITimer.NowInSeconds()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
        }
    }
}
