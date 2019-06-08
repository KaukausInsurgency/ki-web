using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAWKI_TCPServer.Interfaces;

namespace Tests.Mocks
{
    class MockTimer : ITimer
    {
        private long _counter = 0;
        long ITimer.NowInSeconds()
        {
            _counter += 1;
            return _counter;
        }
        public void SetCounter(long value)
        {
            _counter = value;
        }
    }
}
