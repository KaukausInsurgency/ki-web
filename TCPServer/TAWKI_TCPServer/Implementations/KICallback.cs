using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using StackExchange.Redis;
using TAWKI_TCPServer.Interfaces;
using Newtonsoft.Json;
using System.Data;

namespace TAWKI_TCPServer.Implementations
{
    class KICallback : IIPCCallback
    {
        private ILogger Logger;
        private ThrottleMapper Throttler;

        public KICallback(ILogger Logger)
        {
            this.Logger = Logger;
            Throttler = new ThrottleMapper(new CurrentTime(), 5);
        }

        public void Invoke(ISynchronizeInvoke sync, Action action)
        {
            if (!sync.InvokeRequired)
            {
                action();
            }
            else
            {
                object[] args = new object[] { };
                sync.Invoke(action, args);
            }
        }

        void IIPCCallback.OnConnect(object sender, IPCConnectedEventArgs e)
        {
            Console.WriteLine("Client has connected: " + e.address);
            Logger.Log("Client has connected: " + e.address);
        }

        void IIPCCallback.OnDisconnect(object sender, IPCConnectedEventArgs e)
        {
            Console.WriteLine("Client has disconnected: " + e.address);
            Logger.Log("Client has disconnected: " + e.address);
        }

        void IIPCCallback.OnReceive(object sender, IPCReceivedEventArgs e)
        {
            try
            {
                Logger.Log("Client Sent: " + e.data);
                Console.WriteLine("Received data from client (" + ((SocketClient)(sender)).Address + ")");

                dynamic j = Newtonsoft.Json.JsonConvert.DeserializeObject(e.data);

                // Verify the request format is valid
                if (j["Action"] != null && j["BulkQuery"] != null && j["Data"] != null && j["Destination"] != null)
                {
                    // Simply ignore/drop the request if we need to throttle the connection
                    if (!Throttler.ShouldThrottle(j["Action"]))
                    {
                        ProtocolRequest request = Utility.CreateRequest(ref j, ((SocketClient)(sender)).Address);
                        IProcessMessageStrategy processor = ProcessMessageStrategyFactory.Create(GlobalConfig.GetConfig(), Logger, ProcessMessageStrategyFactory.GetSource(request.Destination));
                        ProtocolResponse resp = processor.Process(request);
                        string jsonResp = JsonConvert.SerializeObject(resp);
                        ((SocketClient)(sender)).Write(jsonResp);
                    }          
                }
                else
                {
                    // send malformed request response
                    string jsonResponse = "{ \"Action\" : " + j["Action"] + ", \"Result\" : false, \"Error\" : \"Malformed Request Received\", \"Data\" : null }";
                    ((SocketClient)(sender)).Write(jsonResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception encountered during OnReceived handler: " + ex.Message);
                Logger.Log("Exception encountered during OnReceived handler: " + ex.Message);
                string jsonResponse = "{ \"Action\" : \"UNKNOWN\", \"Result\" : false, \"Error\" : \"Malformed JSON Request Received\", \"Data\" : null }";
                ((SocketClient)(sender)).Write(jsonResponse);
            }
        }

        void IIPCCallback.OnSend(object sender, IPCSendEventArgs e)
        {
            Logger.Log("Server Sent: " + e.data);
        }
    }
}
