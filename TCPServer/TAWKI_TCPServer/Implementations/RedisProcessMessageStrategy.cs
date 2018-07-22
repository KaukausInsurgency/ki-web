using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAWKI_TCPServer.Interfaces;
using System.Data;
using Newtonsoft.Json.Linq;

namespace TAWKI_TCPServer.Implementations
{
    public class RedisProcessMessageStrategy : IProcessMessageStrategy
    {
        public const Int64 LUANULL = -9999;        // THIS IS IMPORTANT - CHANGING THIS WILL BREAK IF THE LUA NIL PLACEHOLDER IS NOT THE SAME!!!
        public const string ACTION_GET_SERVERID = "GetOrAddServer";
        private IConnectionMultiplexer Connection;
        private ILogger Logger;
        private IConfigReader Config;

        public RedisProcessMessageStrategy(IConnectionMultiplexer connection, ILogger logger, IConfigReader config)
        {
            Connection = connection;
            Logger = logger;
            Config = config;
        }

        ProtocolResponse IProcessMessageStrategy.Process(ProtocolRequest request)
        {
            ProtocolResponse response = new ProtocolResponse(request.Action);

            if (Connection == null || !Connection.IsConnected)
            {
                Logger.Log("Connection to Redis Closed - Attempting to reopen...");
                try
                {
                    Connection = ConnectionMultiplexer.Connect(Config.RedisDBConnect);
                }
                catch (Exception ex)
                {
                    Logger.Log("Error connecting to Redis - lost connection (" + ex.Message + ")");
                    response.Error = "Error connecting to Redis - lost connection (" + ex.Message + ")";
                    return response;
                }
            }

            if (!Config.RedisActionKeys.ContainsKey(request.Action))
            {
                Logger.Log("Error - Invalid Redis Action '" + request.Action + "' received from client");
                response.Error = "Error executing query against Redis - Action: '" + request.Action + "' not found in server configuration - please check action message or server configuration.";
                return response;
            }

            RedisAction ra = Config.RedisActionKeys[request.Action];

            // parse out all lua nulls and convert to real nulls
            request.Data = Utility.ParseLuaNullsFromString(request.Data);

            try
            {
                IDatabase db = Connection.GetDatabase();
                RedisUtility.PerformOperation(ra.Action, request.IsBulkQuery, ref db, ref Logger, Config.RedisEnvironmentKey, ra.Key, request.Data);
                response.Result = true;
                response.Data = new List<List<object>>();
            }
            catch (Exception ex)
            {
                CatchException(ref ex, ref request, ref response);
            }

            return response;
        }

        private void CatchException(ref Exception ex, ref ProtocolRequest request, ref ProtocolResponse response)
        {
            Logger.Log("Error executing query against Redis (Action: " + request.Action + ") - " + ex.Message);
            response.Error = "Error executing query against Redis (Action: " + request.Action + ") - " + ex.Message;
        }
    }
}
