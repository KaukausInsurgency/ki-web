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
        private IConnectionMultiplexer RedisConnection;
        private IDbConnection DBConnection;

        public KICallback(ILogger Logger)
        {
            this.Logger = Logger;
            Throttler = new ThrottleMapper(new CurrentTime(), 1);
            DBConnection = new MySqlConnection(GlobalConfig.GetConfig().MySQLDBConnect);
            RedisConnection = null;
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
                    string action = j["Action"];
                    // Simply ignore/drop the request if we need to throttle the connection
                    if (!Throttler.ShouldThrottle(action))
                    {
                        // if we have broken connections, send error and return
                        if (!CheckRedisConnection() || !CheckDBConnection())
                        {
                            string jsonResponse = "{ \"Action\" : " + j["Action"] + ", \"Result\" : false, \"Error\" : \"Could not connect to Services\", \"Data\" : null }";
                            ((SocketClient)(sender)).Write(jsonResponse);
                            return;
                        }
                        ProtocolRequest request = Utility.CreateRequest(ref j, ((SocketClient)(sender)).Address);
                        IProcessMessageStrategy processor = ProcessMessageStrategyFactory.Create(GlobalConfig.GetConfig(), Logger, 
                            ProcessMessageStrategyFactory.GetSource(request.Destination), RedisConnection, DBConnection);
                        ProtocolResponse resp = processor.Process(request);
                        string jsonResp = JsonConvert.SerializeObject(resp);
                        ((SocketClient)(sender)).Write(jsonResp);
                    }          
                    else
                    {
                        Logger.Log("Too many requests - throttling request");
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

        private bool CheckRedisConnection()
        {
            if (RedisConnection == null || !RedisConnection.IsConnected)
            {
                try
                {
                    RedisConnection = ConnectionMultiplexer.Connect(GlobalConfig.GetConfig().RedisDBConnect);
                }
                catch (Exception ex)
                {
                    Logger.Log("Error - Could not connect to Redis DB - " + ex.Message);
                    if (RedisConnection != null && RedisConnection.IsConnected)
                        RedisConnection.Close();

                    return false;
                }
            }

            return true;
        }

        private bool CheckDBConnection()
        {
            if (DBConnection.State == ConnectionState.Open)
                return true;
            else
            {
                try
                {
                    DBConnection.Open();
                }
                catch (Exception ex)
                {
                    Logger.Log("Error - Could not connect to MySql DB - " + ex.Message);
                    if (DBConnection.State == ConnectionState.Open)
                        DBConnection.Close();

                    return false;
                }
                return true;
            }
        }
    }
}
