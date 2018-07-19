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
        private Dictionary<string, long> _monitor;
        private Dictionary<string, long> _config;

        public ThrottleMapper(ITimer t, Dictionary<string, long> config)
        {
            _timer = t;
            _config = config;
            _monitor = new Dictionary<string, long>();
        }

        public bool ShouldThrottle(string action)
        {
            if (_config.ContainsKey(action))
            {
                if (!_monitor.ContainsKey(action))
                {
                    _monitor.Add(action, _timer.NowInSeconds());
                    return false;
                }
                else
                {
                    long minTime = _config[action];
                    long now = _timer.NowInSeconds();
                    long secondsPassed = now - _monitor[action];
                    _monitor[action] = now;
                    if (secondsPassed <= minTime)
                        return true;
                    else
                        return false;
                }
            }
            else
            {
                return false;
            }         
        }
    }
}
