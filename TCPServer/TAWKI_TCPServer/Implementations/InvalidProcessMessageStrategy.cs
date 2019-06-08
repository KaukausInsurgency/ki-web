using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAWKI_TCPServer.Interfaces;

namespace TAWKI_TCPServer.Implementations
{
    public class InvalidProcessMessageStrategy : IProcessMessageStrategy
    {
        public InvalidProcessMessageStrategy()
        {

        }

        ProtocolResponse IProcessMessageStrategy.Process(ProtocolRequest request)
        {
            ProtocolResponse response = new ProtocolResponse
            {
                Action = request.Action,
                Error = "Invalid Destination specified - must be either 'REDIS' or 'MYSQL'",
                Data = null,
                Result = false
            };
            return response;
        }
    }
}
