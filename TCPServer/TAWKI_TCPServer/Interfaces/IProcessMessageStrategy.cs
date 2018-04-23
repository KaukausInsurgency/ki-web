using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAWKI_TCPServer.Interfaces
{
    public interface IProcessMessageStrategy
    {
        ProtocolResponse Process(ProtocolRequest request, ILogger logger);
    }
}
