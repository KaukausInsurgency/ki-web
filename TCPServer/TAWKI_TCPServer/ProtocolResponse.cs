using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAWKI_TCPServer
{
    public struct ProtocolResponse
    {
        public string Action;
        public bool Result;
        public string Error;
        public List<List<object>> Data;

        public ProtocolResponse(string Action)
        {
            this.Action = Action;
            Error = "";
            Data = new List<List<object>>();
            Result = false;
        }
    }
}
