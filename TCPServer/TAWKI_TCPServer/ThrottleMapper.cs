using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAWKI_TCPServer.Interfaces;

namespace TAWKI_TCPServer
{
    public class ThrottleMapper
    {
        private ITimer _timer;
        private Dictionary<string, long> _throttleDictionary;
        private long _minTime;

        public ThrottleMapper(ITimer t, long mintime)
        {
            _timer = t;
            _minTime = mintime;
            _throttleDictionary = new Dictionary<string, long>();
        }

        public bool ShouldThrottle(string action)
        {
            if (!_throttleDictionary.ContainsKey(action))
            {
                _throttleDictionary.Add(action, _timer.NowInSeconds());
                return false;
            }
            else
            {
                long now = _timer.NowInSeconds();
                long secondsPassed = now - _throttleDictionary[action];
                _throttleDictionary[action] = now;
                if (secondsPassed <= _minTime)
                    return true;
                else
                    return false;
            }
        }
    }
}
