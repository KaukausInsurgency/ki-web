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
        private ConnectionMultiplexer Connection;
        private ILogger Logger;
        private IConfigReader Config;

        public RedisProcessMessageStrategy(ConnectionMultiplexer connection, ILogger logger, IConfigReader config)
        {
            Connection = connection;
            Logger = logger;
            Config = config;
        }

        ProtocolResponse IProcessMessageStrategy.Process(ProtocolRequest request, ILogger logger)
        {
            ProtocolResponse response = new ProtocolResponse(request.Action);

            if (Connection == null || !Connection.IsConnected)
            {
                logger.Log("Connection to Redis Closed - Attempting to reopen...");
                try
                {
                    Connection = ConnectionMultiplexer.Connect(Config.RedisDBConnect);
                }
                catch (Exception ex)
                {
                    logger.Log("Error connecting to Redis - lost connection (" + ex.Message + ")");
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
                    int id = 0;
                    foreach (Dictionary<string, object> x in DataDictionary)
                    {
                        id += 1;
                        string serverid = Convert.ToString(x["ServerID"]);
                        string rediskey = Config.RedisActionKeys[request.Action];

                        if (serverid == null)
                        {
                            response.Error = "Error executing query against Redis (Action: " + request.Action + ") - " + "'ServerID' not found in Data request";
                            return response;
                        }

                        if (rediskey == null)
                        {
                            response.Error = "Error executing query against Redis - Action: '" + request.Action + "' not found in server configuration - please check action message or server configuration.";
                            return response;
                        }

                        IDatabase db = Connection.GetDatabase();
                        string k = rediskey + ":" + serverid + ":" + id;
                        string jdatastring = Newtonsoft.Json.JsonConvert.SerializeObject(x);
                        if (!db.StringSet(k, jdatastring))
                        {
                            response.Error = "Failed to Set Key in Redis (Key: '" + k + "')";
                            response.Result = false;
                        }
                        else
                        {
                            List<object> res = new List<object>
                            {
                                k
                            };
                            response.Data.Add(res);
                            response.Result = true;
                        }
                    }

                    response.Result = true;
                }
                catch (Exception ex)
                {
                    CatchException(logger, ref ex, ref request, ref response);
                }
            }
            else
            {
                // now deserialize this string into a list of dictionaries for parsing
                Dictionary<string, object> DataDictionary = null;

                if (request.Type == JTokenType.Object)
                    DataDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(request.Data);
                else
                    DataDictionary = new Dictionary<string, object>();

                try
                {
                    string serverid = Convert.ToString(DataDictionary["ServerID"]);
                    string rediskey = Config.RedisActionKeys[request.Action];

                    if (serverid == null)
                    {
                        response.Error = "Error executing query against Redis (Action: " + request.Action + ") - " + "'ServerID' not found in Data request";
                        return response;
                    }

                    if (rediskey == null)
                    {
                        response.Error = "Error executing query against Redis - Action: '" + request.Action + "' not found in server configuration - please check action message or server configuration.";
                        return response;
                    }

                    IDatabase db = Connection.GetDatabase();
                    string k = rediskey + ":" + serverid;
                    if (!db.StringSet(k, request.Data))
                    {
                        response.Error = "Failed to Set Key in Redis (Key: '" + k + "')";
                        response.Result = false;
                    }
                    else
                    {
                        response.Data.Add(new List<object> { 1 });
                        response.Result = true;
                    }
                }
                catch (Exception ex)
                {
                    CatchException(logger, ref ex, ref request, ref response);
                }

            }

            return response;

        }

        private void CatchException(ILogger logger, ref Exception ex, ref ProtocolRequest request, ref ProtocolResponse response)
        {
            logger.Log("Error executing query against Redis (Action: " + request.Action + ") - " + ex.Message);
            response.Error = "Error executing query against Redis (Action: " + request.Action + ") - " + ex.Message;
        }
    }
}
