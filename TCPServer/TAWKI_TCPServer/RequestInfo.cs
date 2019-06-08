using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAWKI_TCPServer
{
    class RequestInfo
    {
        public long Count { get; set; }
        public long FirstRequestTimeInSeconds { get; set; } = -1;
        public long AverageLimit { get; set; }
        public RequestInfo()
        {
            FirstRequestTimeInSeconds = -1;
            Count = 0;
            AverageLimit = 0;
        }
    }
}
