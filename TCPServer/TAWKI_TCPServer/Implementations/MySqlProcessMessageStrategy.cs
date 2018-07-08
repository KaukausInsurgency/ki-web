using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAWKI_TCPServer.Interfaces;

namespace TAWKI_TCPServer.Implementations
{
    public class MySqlProcessMessageStrategy : IProcessMessageStrategy
    {
        public const string ACTION_GET_SERVERID = "GetOrAddServer";
        private IDbConnection Connection;
        private ILogger Logger;
        private IConfigReader Config;

        public MySqlProcessMessageStrategy(IDbConnection connection, ILogger Logger, IConfigReader config)
        {
            Connection = connection;
            this.Logger = Logger;
            Config = config;
        }

        ProtocolResponse IProcessMessageStrategy.Process(ProtocolRequest request)
        {
            ProtocolResponse response = new ProtocolResponse(request.Action);

            if (request.IsBulkQuery)
            {
                // now deserialize this string into a list of dictionaries for parsing
                List<Dictionary<string, object>> DataDictionary =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(request.Data);

                try
                {
                    Connection.Open();
                    foreach (var d in DataDictionary)
                    {
                        IDbCommand cmd = SqlUtility.CreateCommand(Connection, request.Action, d);
                        List<object> results = SqlUtility.InvokeCommand(cmd, out string error);

                        response.Error += error;
                        if (results != null)
                        {
                            response.Data.Add(results);
                        }
                    }
                    Connection.Close();
                    response.Result = true;
                }
                catch (Exception ex)
                {
                    CatchException(ref ex, ref request, ref response);
                }
                finally
                {
                    FinallyMySQL(ref Connection);
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

                // special scenario - because we cant get the ip address of the game server from DCS, we'll get it from the socket sender object
                // and specially insert it as a parameter into the data dictionary
                // the other special scenario is the server description request can supply - this can contain harmful html, so we must sanitize the input
                if (request.Action == ACTION_GET_SERVERID)
                {
                    DataDictionary.Add("IP", request.IPAddress);
                    if (DataDictionary.ContainsKey("Description"))
                    {
                        try
                        {
                            string html = Convert.ToString(DataDictionary["Description"]);
                            DataDictionary["Description"] = Utility.SanitizeHTML(html, ref Config);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log("Error sanitizing ServerDescription html string (Action: " + request.Action + ") - " + ex.Message);
                            response.Error = "Error sanitizing ServerDescription html string (Action: " + request.Action + ") - " + ex.Message;
                            response.Result = false;
                            return response;
                        }
                    }

                    // Check the API version that the game is using
                    if (DataDictionary.ContainsKey("Version") && DataDictionary["Version"].ToString() != Config.VersionKey)
                    {
                        Logger.Log("Client Version Mismatch (Expected: " + Config.VersionKey + ", Got: " + DataDictionary["Version"] + ")");
                        response.Error = "Version mismatch - you are running an older version of KI - the latest version is [" + Config.Version + "] - Please update to the latest version";
                        response.Result = false;
                        return response;
                    }
                    else if (!DataDictionary.ContainsKey("Version"))
                    {
                        Logger.Log("Client Version Mismatch - Client did not provide version information");
                        response.Error = "Version mismatch - you are running an older version of KI - the latest version is [" + Config.Version + "] - Please update to the latest version";
                        response.Result = false;
                        return response;
                    }
                }

                try
                {
                    Connection.Open();

                    IDbCommand cmd = SqlUtility.CreateCommand(Connection, request.Action, DataDictionary);
                    List<object> results = SqlUtility.InvokeCommand(cmd, out string error);

                    response.Error += error;
                    if (results != null)
                    {
                        response.Data.Add(results);
                    }

                    Connection.Close();
                    response.Result = true;
                }
                catch (Exception ex)
                {
                    CatchException(ref ex, ref request, ref response);
                }
                finally
                {
                    FinallyMySQL(ref Connection);
                }

            }

            return response;

        }

        private void CatchException(ref Exception ex, ref ProtocolRequest request, ref ProtocolResponse response)
        {
            Logger.Log("Error executing query against MySQL (Action: " + request.Action + ") - " + ex.Message);
            response.Error = "Error executing query against MySQL (Action: " + request.Action + ") - " + ex.Message;
        }

        private void FinallyMySQL(ref IDbConnection conn)
        {
            if (conn != null)
                if (conn.State == System.Data.ConnectionState.Open || conn.State == System.Data.ConnectionState.Connecting)
                    conn.Close();
        }
    }
}
