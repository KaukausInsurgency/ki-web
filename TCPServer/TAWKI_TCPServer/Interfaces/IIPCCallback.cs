using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAWKI_TCPServer.Interfaces
{
    public interface IIPCCallback
    {
        void OnConnect(object sender, IPCConnectedEventArgs e);
        void OnDisconnect(object sender, IPCConnectedEventArgs e);
        void OnReceive(object sender, IPCReceivedEventArgs e);
        void OnSend(object sender, IPCSendEventArgs e);
    }
}
