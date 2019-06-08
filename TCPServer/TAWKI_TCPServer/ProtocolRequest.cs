using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAWKI_TCPServer
{
    public struct ProtocolRequest
    {
        public string Action;
        public string Destination;
        public string IPAddress;
        public bool IsBulkQuery;
        public string Data;
        public Newtonsoft.Json.Linq.JTokenType Type;
    }
}
