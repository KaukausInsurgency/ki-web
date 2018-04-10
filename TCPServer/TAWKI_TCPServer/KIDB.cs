using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using MySql.Data;
using System.IO;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using StackExchange.Redis;

namespace TAWKI_TCPServer
{
    class KIDB
    {
        public const Int64 LUANULL = -9999;        // THIS IS IMPORTANT - CHANGING THIS WILL BREAK IF THE LUA NIL PLACEHOLDER IS NOT THE SAME!!!
        public const string ACTION_GET_SERVERID = "GetOrAddServer";
        public static string DBConnection = "";
        public static string RedisDBConnection = "";
        public static ConnectionMultiplexer RedisConnection = null;
        public static Dictionary<string, string> RedisActionKeyTable = null;
        public static List<string> SupportedHTML = null;

        public static void Invoke(ISynchronizeInvoke sync, Action action)
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

        public static ProtocolResponseSingleData SendToDBSingleData(string dest, string log, string action, string ip_address, ref dynamic j)
        {
            if (dest == "MYSQL")
            {
                return SendToMySQLDBSingleData(log, action, ip_address, ref j);
            }
            else if (dest == "REDIS")
            {
                return SendToRedisDBSingleData(log, action, ref j);
            }
            else
            {
                ProtocolResponseSingleData response = new ProtocolResponseSingleData
                {
                    Action = action,
                    Error = "Invalid Destination specified - must be either 'REDIS' or 'MYSQL'",
                    Data = null,
                    Result = false
                };
                return response;
            }
        }
        
        public static ProtocolResponseMultiData SendToDBMultiData(string dest, string log, string action, ref dynamic j)
        {
            if (dest == "MYSQL")
            {
                return SendToMySQLDBMultiData(log, action, ref j);
            }
            else if (dest == "REDIS")
            {
                return SendToRedisDBMultiData(log, action, ref j);
            }
            else
            {
                ProtocolResponseMultiData response = new ProtocolResponseMultiData
                {
                    Action = action,
                    Error = "Invalid Destination specified - must be either 'REDIS' or 'MYSQL'",
                    Data = null,
                    Result = false
                };
                return response;
            }
        }

        public static void OnConnect(object sender, IPCConnectedEventArgs e)
        {
            Console.WriteLine("Client has connected: " + e.address);
            LogToFile("Client has connected: " + e.address, e.logpath);
        }

        public static void OnDisconnect(object sender, IPCConnectedEventArgs e)
        {
            Console.WriteLine("Client has disconnected: " + e.address);
            LogToFile("Client has disconnected: " + e.address, e.logpath);
        }

        public static void OnReceived(object sender, IPCReceivedEventArgs e)
        {
            //SocketClient.SyncBegin();
            try
            {
                string _logFilePath = ((SocketClient)(sender)).LogFile;
                LogToFile(e.data, _logFilePath);
                Console.WriteLine("Received data from client (" + ((SocketClient)(sender)).Address + ")");

                dynamic j = Newtonsoft.Json.JsonConvert.DeserializeObject(e.data);

                // Verify the request format is valid
                if (j["Action"] != null && j["BulkQuery"] != null && j["Data"] != null && j["Destination"] != null)
                {
                    string action = j["Action"];
                    string destination = j["Destination"];
                    bool isBulkQuery = j["BulkQuery"];
                    string jsonResp = "";
                    if (isBulkQuery)
                    {
                        ProtocolResponseMultiData resp = SendToDBMultiData(destination, ((SocketClient)(sender)).LogFile, action, ref j);
                        if (!string.IsNullOrWhiteSpace(resp.Error))
                            resp.Result = false;
                        else
                            resp.Result = true;

                        jsonResp = JsonConvert.SerializeObject(resp);
                    }
                    else
                    {
                        ProtocolResponseSingleData resp = SendToDBSingleData(destination, ((SocketClient)(sender)).LogFile, action, ((SocketClient)(sender)).Address, ref j);
                        if (!string.IsNullOrWhiteSpace(resp.Error))
                            resp.Result = false;
                        else
                            resp.Result = true;

                        jsonResp = JsonConvert.SerializeObject(resp);
                    }            
                    ((SocketClient)(sender)).Write(jsonResp);
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
                LogToFile("Exception encountered during OnReceived handler: " + ex.Message, ((SocketClient)(sender)).LogFile);
                string jsonResponse = "{ \"Action\" : \"UNKNOWN\", \"Result\" : false, \"Error\" : \"Malformed JSON Request Received\", \"Data\" : null }";
                ((SocketClient)(sender)).Write(jsonResponse);
            }
            finally
            {
                //SocketClient.SyncEnd();
            }
        }

        public static void OnSend(object sender, IPCSendEventArgs e)
        {
            LogToFile("Server Sent: " + e.data, ((SocketClient)(sender)).LogFile);
        }

        public static void LogToFile(string data, string path)
        {
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(DateTime.Now.ToString("{0:d/M/yyyy HH:mm:ss}") + " - Log File for client");
                    sw.WriteLine(DateTime.Now.ToString("{0:d/M/yyyy HH:mm:ss}") + data);
                   // sw.WriteLine(data);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(DateTime.Now.ToString("{0:d/M/yyyy HH:mm:ss}") + data);
                    //sw.WriteLine(data);
                }
            }
        }

        private static ProtocolResponseSingleData SendToMySQLDBSingleData(string log, string action, string ip_address, ref dynamic j)
        {
            // serialize a new json string for just the data by itself
            string jdataString = Newtonsoft.Json.JsonConvert.SerializeObject(j["Data"]);
            // now deserialize this string into a list of dictionaries for parsing
            Dictionary<string, object> dataDictionary = null;

            if (((JToken)j["Data"]).Type == JTokenType.Object)
                dataDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jdataString);
            else
                dataDictionary = new Dictionary<string, object>();

            ProtocolResponseSingleData result = new ProtocolResponseSingleData
            {
                Action = action,
                Error = "",
                Data = new List<object>()
            };

            // special scenario - because we cant get the ip address of the game server from DCS, we'll get it from the socket sender object
            // and specially insert it as a parameter into the data dictionary
            if (action == ACTION_GET_SERVERID)
            {
                dataDictionary.Add("IP", ip_address);
                if (dataDictionary.ContainsKey("Description"))
                {
                    try
                    {
                        string html = Convert.ToString(dataDictionary["Description"]);
                        html = System.Web.HttpUtility.HtmlEncode(html);
                        dataDictionary["Description"] = SanitizeHTML(html);
                    }
                    catch (Exception ex)
                    {
                        LogToFile("Error sanitizing ServerDescription html string (Action: " + action + ") - " + ex.Message, log);
                        result.Error = "Error sanitizing ServerDescription html string (Action: " + action + ") - " + ex.Message;
                        return result;
                    }
                }
            }    

            MySql.Data.MySqlClient.MySqlConnection _conn = null;
            MySql.Data.MySqlClient.MySqlDataReader rdr = null;

            try
            {
                _conn = new MySql.Data.MySqlClient.MySqlConnection(DBConnection);
                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(action)
                {
                    Connection = _conn,
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                foreach (var d in dataDictionary)
                {
                    if (d.Value.GetType() == typeof(Int64) && (Int64)d.Value == LUANULL)
                        cmd.Parameters.AddWithValue(d.Key, null);
                    else
                        cmd.Parameters.AddWithValue(d.Key, d.Value);
                }
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        result.Data.Add(rdr[i]);
                    }
                }

                rdr.Close();
                _conn.Close();
            }
            catch (Exception ex)
            {
                LogToFile("Error executing query against MySQL (Action: " + action + ") - " + ex.Message, log);
                result.Error = "Error executing query against MySQL (Action: " + action + ") - " + ex.Message;
            }
            finally
            {
                if (_conn != null)
                    if (_conn.State == System.Data.ConnectionState.Open || _conn.State == System.Data.ConnectionState.Connecting)
                        _conn.Close();

                if (rdr != null)
                    if (!rdr.IsClosed)
                        rdr.Close();
            }

            return result;

        }

        private static ProtocolResponseMultiData SendToMySQLDBMultiData(string log, string action, ref dynamic j)
        {

            // serialize a new json string for just the data by itself
            string jdataString = Newtonsoft.Json.JsonConvert.SerializeObject(j["Data"]);
            // now deserialize this string into a list of dictionaries for parsing
            List<Dictionary<string, object>> dataDictionary =
                Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jdataString);

            ProtocolResponseMultiData result = new ProtocolResponseMultiData
            {
                Action = action,
                Error = "",
                Data = new List<List<object>>()
            };

            MySql.Data.MySqlClient.MySqlConnection _conn = null;
            MySql.Data.MySqlClient.MySqlDataReader rdr = null;

            try
            {
                foreach (var d in dataDictionary)
                {
                    _conn = new MySql.Data.MySqlClient.MySqlConnection(DBConnection);
                    _conn.Open();
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(action)
                    {
                        Connection = _conn,
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    foreach (var kv in d)
                    {
                        if (kv.Value.GetType() == typeof(Int64) && (Int64)kv.Value == LUANULL)
                            cmd.Parameters.AddWithValue(kv.Key, null);
                        else
                            cmd.Parameters.AddWithValue(kv.Key, kv.Value);
                    }
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        List<object> result_set = new List<object>();
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            result_set.Add(rdr[i]);
                        }
                        result.Data.Add(result_set);
                    }
                    else
                    {
                        result.Error += "No Results Returned\n";
                    }
                    rdr.Close();
                    _conn.Close();
                }
            }
            catch (Exception ex)
            {
                LogToFile("Error executing query against MySQL (Action: " + action + ") - " + ex.Message, log);
                result.Error = "Error executing query against MySQL (Action: " + action + ") - " + ex.Message;
            }
            finally
            {
                if (_conn != null)
                    if (_conn.State == System.Data.ConnectionState.Open || _conn.State == System.Data.ConnectionState.Connecting)
                        _conn.Close();

                if (rdr != null)
                    if (!rdr.IsClosed)
                        rdr.Close();
            }

            return result;
        }

        private static ProtocolResponseSingleData SendToRedisDBSingleData(string log, string action, ref dynamic j)
        {
            // Serialize the JSON Data property into its own JSON String
            string jdataString = Newtonsoft.Json.JsonConvert.SerializeObject(j["Data"]);

            // now deserialize this string into a list of dictionaries for parsing
            Dictionary<string, object> dataDictionary = null;

            if (((JToken)j["Data"]).Type == JTokenType.Object)
                dataDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jdataString);
            else
                dataDictionary = new Dictionary<string, object>();

            ProtocolResponseSingleData result = new ProtocolResponseSingleData
            {
                Action = action,
                Error = "",
                Data = new List<object>()
            };

            if (!RedisConnection.IsConnected)
            {
                LogToFile("Connection to Redis Closed - Attempting to reopen...", log);
                try
                {
                    RedisConnection = ConnectionMultiplexer.Connect(RedisDBConnection);
                }
                catch (Exception ex)
                {
                    LogToFile("Error connecting to Redis - lost connection (" + ex.Message + ")", log);
                    result.Error = "Error connecting to Redis - lost connection (" + ex.Message + ")";
                    return result;
                }
            }

            try
            {
                string serverid = Convert.ToString(dataDictionary["ServerID"]);
                string rediskey = RedisActionKeyTable[action];

                if (serverid == null)
                {
                    result.Error = "Error executing query against Redis (Action: " + action + ") - " + "'ServerID' not found in Data request";
                    return result;
                }

                if (rediskey == null)
                {
                    result.Error = "Error executing query against Redis - Action: '" + action + "' not found in server configuration - please check action message or server configuration.";
                    return result;
                }

                IDatabase db = RedisConnection.GetDatabase();
                string k = rediskey + ":" + serverid;
                if (!db.StringSet(k, jdataString))
                {
                    result.Error = "Failed to Set Key in Redis (Key: '" + k + "')";
                }
                else
                {
                    result.Data.Add(1);
                }
            }
            catch (Exception ex)
            {
                LogToFile("Error executing query against Redis (Action: " + action + ") - " + ex.Message, log);
                result.Error = "Error executing query against Redis (Action: " + action + ") - " + ex.Message;
            }         

            return result;
        }

        private static ProtocolResponseMultiData SendToRedisDBMultiData(string log, string action, ref dynamic j)
        {
            // serialize a new json string for just the data by itself
            string jdataString = Newtonsoft.Json.JsonConvert.SerializeObject(j["Data"]);
            // now deserialize this string into a list of dictionaries for parsing
            List<Dictionary<string, object>> dataDictionary =
                Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jdataString);

            ProtocolResponseMultiData result = new ProtocolResponseMultiData
            {
                Action = action,
                Error = "",
                Data = new List<List<object>>()
            };

            if (!RedisConnection.IsConnected)
            {
                LogToFile("Connection to Redis Closed - Attempting to reopen...", log);
                try
                {
                    RedisConnection = ConnectionMultiplexer.Connect(RedisDBConnection);
                }
                catch (Exception ex)
                {
                    LogToFile("Error connecting to Redis - lost connection (" + ex.Message + ")", log);
                    result.Error = "Error connecting to Redis - lost connection (" + ex.Message + ")";
                    return result;
                }
            }

            try
            {
                int id = 0;
                foreach (Dictionary<string, object> x in dataDictionary)
                {
                    id += 1;
                    string serverid = Convert.ToString(x["ServerID"]);
                    string rediskey = RedisActionKeyTable[action];

                    if (serverid == null)
                    {
                        result.Error = "Error executing query against Redis (Action: " + action + ") - " + "'ServerID' not found in Data request";
                        return result;
                    }

                    if (rediskey == null)
                    {
                        result.Error = "Error executing query against Redis - Action: '" + action + "' not found in server configuration - please check action message or server configuration.";
                        return result;
                    }

                    IDatabase db = RedisConnection.GetDatabase();
                    string k = rediskey + ":" + serverid + ":" + id;
                    string jdatastring = Newtonsoft.Json.JsonConvert.SerializeObject(x);
                    if (!db.StringSet(k, jdatastring))
                    {
                        result.Error = "Failed to Set Key in Redis (Key: '" + k + "')";
                    }
                    else
                    {
                        List<object> res = new List<object>
                        {
                            1
                        };
                        result.Data.Add(res);
                    }
                }    
            }
            catch (Exception ex)
            {
                LogToFile("Error executing query against Redis (Action: " + action + ") - " + ex.Message, log);
                result.Error = "Error executing query against Redis (Action: " + action + ") - " + ex.Message;
            }

            return result;
        }

        private static string SanitizeHTML(string html)
        {
            string shtml = html;

            foreach (string x in SupportedHTML)
            {
                // Opening tags
                { 
                    string encoded = "&lt;" + x + "&gt;";
                    string decoded = "<" + x + ">";
                    shtml = shtml.Replace(encoded, decoded);
                }

                // Closing tags
                {
                    string encoded = "&lt;/" + x + "&gt;";
                    string decoded = "</" + x + ">";
                    shtml = shtml.Replace(encoded, decoded);
                }
            }

            return shtml;
        }

    }
}
