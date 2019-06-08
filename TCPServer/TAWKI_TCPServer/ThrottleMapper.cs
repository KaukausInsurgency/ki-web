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
        private Dictionary<string, RequestInfo> _map;

        public ThrottleMapper(ITimer t, Dictionary<string, long> config)
        {
            _timer = t;
            _map = new Dictionary<string, RequestInfo>();
            foreach (KeyValuePair<string, long> c in config)
                _map.Add(c.Key, new RequestInfo() { AverageLimit = c.Value });
        }

        public bool ShouldThrottle(string action)
        {
            if (_map.ContainsKey(action))
            {
                RequestInfo ri = _map[action];
                ri.Count++;
                if (ri.FirstRequestTimeInSeconds == -1)
                {
                    ri.FirstRequestTimeInSeconds = _timer.NowInSeconds();
                    return false;
                }
                else
                {
                    long secondsPassed = _timer.NowInSeconds() - ri.FirstRequestTimeInSeconds;
                    if (secondsPassed <= 0)
                        secondsPassed = 1;

                    if ((ri.Count / (double)secondsPassed) > ri.AverageLimit)
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
