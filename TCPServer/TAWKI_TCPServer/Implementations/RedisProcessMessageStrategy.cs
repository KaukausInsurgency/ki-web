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

            if (request.IsBulkQuery)
            {
                // now deserialize this string into a list of dictionaries for parsing
                List<Dictionary<string, object>> DataDictionary =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(request.Data);       

                try
                {
                    foreach (Dictionary<string, object> x in DataDictionary)
                    {
                        string error;
                        List<object> results = TrySetData(ref request, x.First(), out error);

                        if (results == null)
                        {
                            response.Error = error;
                            response.Result = false;
                        }
                        else
                        {
                            response.Data.Add(results);
                            response.Result = true;
                        }
                    }

                    response.Result = true;
                }
                catch (Exception ex)
                {
                    CatchException(ref ex, ref request, ref response);
                }
            }
            else
            {
                // now deserialize this string into a list of dictionaries for parsing
                Dictionary<string, object> DataDictionary = null;

                if (request.Type == JTokenType.Object)
                    DataDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(request.Data);
                else
                {
                    response.Error = "Error executing query against Redis - Action: '" + request.Action + "' - No data sent";
                    response.Result = false;
                    return response;
                }

                try
                {
                    string error;
                    List<object> results = TrySetData(ref request, DataDictionary.First(), out error);

                    if (results == null)
                    {
                        response.Error = error;
                        response.Result = false;
                    }
                    else
                    {
                        response.Data.Add(results);
                        response.Result = true;
                    }
                }
                catch (Exception ex)
                {
                    CatchException(ref ex, ref request, ref response);
                }

            }

            return response;

        }

        private List<object> TrySetData(ref ProtocolRequest request, KeyValuePair<string, object> pair, out string error)
        {
            error = "";

            if (!Config.RedisActionKeys.ContainsKey(request.Action))
            {
                error = "Error executing query against Redis - Action: '" + request.Action + "' not found in server configuration - please check action message or server configuration.";
                return null;
            }

            string rediskey = Config.RedisActionKeys[request.Action];
            string k = rediskey + ':' + pair.Key;
            string jdatastring = Newtonsoft.Json.JsonConvert.SerializeObject(pair.Value);
            IDatabase db = Connection.GetDatabase();
            
            if (!db.StringSet(k, jdatastring))
            {
                error = "Failed to Set Key in Redis (Key: '" + k + "')";
                return null;
            }
            else
            {
                List<object> res = new List<object>{ k };
                return res;
            }

        }

        private void CatchException(ref Exception ex, ref ProtocolRequest request, ref ProtocolResponse response)
        {
            Logger.Log("Error executing query against Redis (Action: " + request.Action + ") - " + ex.Message);
            response.Error = "Error executing query against Redis (Action: " + request.Action + ") - " + ex.Message;
        }
    }
}
