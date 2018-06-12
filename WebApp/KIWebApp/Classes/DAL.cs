using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KIWebApp.Models;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace KIWebApp.Classes
{
    public class DAL : IDAL
    {
        private const string SP_GET_SERVERS = "websp_GetServersList";
        private const string SP_GET_ONLINEPLAYERS = "websp_GetOnlinePlayers";
        private const string SP_GET_DEPOTS= "websp_GetDepots";
        private const string SP_GET_CAPTUREPOINTS = "websp_GetCapturePoints";
        private const string SP_GET_GAME = "websp_GetGame";
        private const string SP_GET_SIDEMISSIONS = "websp_GetSideMissions";
        private const string SP_SEARCH_TOTALS = "websp_SearchTotals";
        private const string SP_SEARCH_PLAYERS = "websp_SearchPlayers";
        private const string SP_SEARCH_SERVERS = "websp_SearchServers";
        private const string SP_GET_SERVER_INFO = "websp_GetServerInfo";
        private string _DBMySQLConnectionString;
        private string _DBRedisConnectionString;
        
       
        public DAL()
        {
            _DBMySQLConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBMySqlConnect"].ConnectionString;
            _DBRedisConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBRedisConnect"].ConnectionString;
        }

        public DAL(string mySQLConnect, string redisConnect)
        {
            _DBMySQLConnectionString = mySQLConnect;
            _DBRedisConnectionString = redisConnect;
        }

        List<CapturePointModel> IDAL.GetCapturePoints(int serverID)
        {
            IDbConnection conn = new MySqlConnection(_DBMySQLConnectionString);
            try
            {
                conn.Open();
                return ((IDAL)this).GetCapturePoints(serverID, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        List<CapturePointModel> IDAL.GetCapturePoints(int serverID, ref IDbConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
        
            IDbCommand cmd = SqlUtility.CreateCommand(conn, SP_GET_CAPTUREPOINTS, 
                new Dictionary<string, object>() { { "ServerID", serverID } });
            DataTable dt = SqlUtility.Execute(cmd);

            if (dt == null)
                return null;

            List<CapturePointModel> capturepoints = new List<CapturePointModel>();

            foreach (DataRow dr in dt.Rows)
                capturepoints.Add(new CapturePointModel(dr));

            return capturepoints;
        }

        List<DepotModel> IDAL.GetDepots(int serverID)
        {
            IDbConnection conn = new MySqlConnection(_DBMySQLConnectionString);
            try
            {
                conn.Open();
                return ((IDAL)this).GetDepots(serverID, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        List<DepotModel> IDAL.GetDepots(int serverID, ref IDbConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
        
            IDbCommand cmd = SqlUtility.CreateCommand(conn, SP_GET_DEPOTS, 
                new Dictionary<string, object>() { { "ServerID", serverID } });
            DataTable dt = SqlUtility.Execute(cmd);

            if (dt == null)
                return null;

            List<DepotModel> depots = new List<DepotModel>();

            foreach (DataRow dr in dt.Rows)
                depots.Add(new DepotModel(dr));

            return depots;
        }

        GameModel IDAL.GetGame(int serverID)
        {
            IDbConnection conn = new MySqlConnection(_DBMySQLConnectionString);
            try
            {
                conn.Open();
                return ((IDAL)this).GetGame(serverID, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        GameModel IDAL.GetGame(int serverID, ref IDbConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
       
            IDbCommand cmd = SqlUtility.CreateCommand(conn, SP_GET_GAME,
                new Dictionary<string, object>() { { "ServerID", serverID } });
            DataTable dt = SqlUtility.Execute(cmd);

            if (dt == null)
                return null;

            GameModel g = null;

            foreach (DataRow dr in dt.Rows)
            {
                g = new GameModel(serverID, dr)
                {
                    Depots = ((IDAL)this).GetDepots(serverID, ref conn),
                    CapturePoints = ((IDAL)this).GetCapturePoints(serverID, ref conn),
                    Missions = ((IDAL)this).GetSideMissions(serverID, ref conn),
                    OnlinePlayers = ((IDAL)this).GetOnlinePlayers(serverID, ref conn),


                    // Temporary mocked code
                    CustomMenuItems = new List<CustomMenuItemModel>()
                };
                g.CustomMenuItems.Add(new CustomMenuItemModel()
                {
                    MenuName = "Discord",
                    IconClass = "fab fa-discord",
                    Content = new HtmlContentIFrameModel(500, 300, "https://discordapp.com/widget?id=410076123242954753&theme=dark",
                                                        "allowtransparency = \"true\" frameborder = \"0\"")
                });
                g.CustomMenuItems.Add(new CustomMenuItemModel()
                {
                    MenuName = "Custom Menu",
                    IconClass = "fas fa-archive",
                    Content = new HtmlContentSimpleModel("<h3>Rules</h3><ul><li>No Spitting</li><li>No Lying</li><li>Be Respectful</li></ul><p>These are our rules and they will be followed!</p>")
                });
                break;
            }
            return g;
        }

        MarkerViewModel IDAL.GetMarkers(int serverID)
        {
            IDbConnection conn = new MySqlConnection(_DBMySQLConnectionString);
            try
            {
                conn.Open();
                return ((IDAL)this).GetMarkers(serverID, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        MarkerViewModel IDAL.GetMarkers(int serverID, ref IDbConnection conn)
        {
            MarkerViewModel mm = new MarkerViewModel()
            {
                Depots = ((IDAL)this).GetDepots(serverID, ref conn),
                CapturePoints = ((IDAL)this).GetCapturePoints(serverID, ref conn),
                Missions = ((IDAL)this).GetSideMissions(serverID, ref conn)
            };

            return mm;
        }

        List<OnlinePlayerModel> IDAL.GetOnlinePlayers(int serverID)
        {
            IDbConnection conn = new MySqlConnection(_DBMySQLConnectionString);
            try
            {
                conn.Open();
                return ((IDAL)this).GetOnlinePlayers(serverID, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        List<OnlinePlayerModel> IDAL.GetOnlinePlayers(int serverID, ref IDbConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
            
            IDbCommand cmd = SqlUtility.CreateCommand(conn, SP_GET_ONLINEPLAYERS,
                new Dictionary<string, object>() { { "ServerID", serverID } });
            DataTable dt = SqlUtility.Execute(cmd);

            if (dt == null)
                return null;

            List<OnlinePlayerModel> players = new List<OnlinePlayerModel>();

            foreach (DataRow dr in dt.Rows)
                players.Add(new OnlinePlayerModel(dr));

            return players;
        }

        List<ServerModel> IDAL.GetServers()
        {
            IDbConnection conn = new MySqlConnection(_DBMySQLConnectionString);
            try
            {
                conn.Open();
                return ((IDAL)this).GetServers(ref conn);
            }
            finally
            {
                conn.Close();
            }              
        }

        List<ServerModel> IDAL.GetServers(ref IDbConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
            
            IDbCommand cmd = SqlUtility.CreateCommand(conn, SP_GET_SERVERS, new Dictionary<string, object>());
            DataTable dt = SqlUtility.Execute(cmd);

            if (dt == null)
                return null;

            List<ServerModel> servers = new List<ServerModel>();

            foreach (DataRow dr in dt.Rows)
                servers.Add(new ServerModel(dr));

            return servers;
        }

        List<SideMissionModel> IDAL.GetSideMissions(int serverID)
        {
            IDbConnection conn = new MySqlConnection(_DBMySQLConnectionString);
            try
            {
                conn.Open();
                return ((IDAL)this).GetSideMissions(serverID, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        List<SideMissionModel> IDAL.GetSideMissions(int serverID, ref IDbConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
            
            IDbCommand cmd = SqlUtility.CreateCommand(conn, SP_GET_SIDEMISSIONS,
                new Dictionary<string, object>() { { "ServerID", serverID } });
            DataTable dt = SqlUtility.Execute(cmd);

            if (dt == null)
                return null;

            List<SideMissionModel> missions = new List<SideMissionModel>();

            foreach (DataRow dr in dt.Rows)
            {
                missions.Add(new SideMissionModel(dr));
            }
            return missions;
        }

        SearchResultsModel IDAL.GetSearchResults(string query)
        {
            IDbConnection conn = new MySqlConnection(_DBMySQLConnectionString);
            try
            {
                conn.Open();
                return ((IDAL)this).GetSearchResults(query, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        SearchResultsModel IDAL.GetSearchResults(string query, ref IDbConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
            SearchResultsModel results = new SearchResultsModel();
            
            IDbCommand cmd = SqlUtility.CreateCommand(conn, SP_SEARCH_TOTALS,
                         new Dictionary<string, object>() { { "Criteria", query } });
            DataTable dt = SqlUtility.Execute(cmd);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    results = new SearchResultsModel(dr);
                    break;
                }
            }
            results.Query = query;

            return results;
        }

        List<PlayerModel> IDAL.GetPlayerSearchResults(string query)
        {
            IDbConnection conn = new MySqlConnection(_DBMySQLConnectionString);
            try
            {
                conn.Open();
                return ((IDAL)this).GetPlayerSearchResults(query, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        List<PlayerModel> IDAL.GetPlayerSearchResults(string query, ref IDbConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
            List<PlayerModel> results = new List<PlayerModel>();

            IDbCommand cmd = SqlUtility.CreateCommand(conn, SP_SEARCH_PLAYERS,
                         new Dictionary<string, object>() { { "Criteria", query } });
            DataTable dt = SqlUtility.Execute(cmd);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                    results.Add(new PlayerModel(dr));
            }

            return results;
        }

        List<ServerModel> IDAL.GetServerSearchResults(string query)
        {
            IDbConnection conn = new MySqlConnection(_DBMySQLConnectionString);
            try
            {
                conn.Open();
                return ((IDAL)this).GetServerSearchResults(query, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        List<ServerModel> IDAL.GetServerSearchResults(string query, ref IDbConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
            List<ServerModel> results = new List<ServerModel>();
 
            IDbCommand cmd = SqlUtility.CreateCommand(conn, SP_SEARCH_SERVERS,
                        new Dictionary<string, object>() { { "Criteria", query } });
            DataTable dt = SqlUtility.Execute(cmd);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                    results.Add(new ServerModel(dr));
            }

            return results;
        }

        ServerViewModel IDAL.GetServerInfo(int serverID)
        {
            IDbConnection conn = new MySqlConnection(_DBMySQLConnectionString);
            try
            {
                conn.Open();
                return ((IDAL)this).GetServerInfo(serverID, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        ServerViewModel IDAL.GetServerInfo(int serverID, ref IDbConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
      
            IDbCommand cmd = SqlUtility.CreateCommand(conn, SP_GET_SERVER_INFO,
                     new Dictionary<string, object>() { { "ServerID", serverID } });
            DataTable dt = SqlUtility.Execute(cmd);

            if (dt == null)
                return null;

            ServerViewModel s = null;

            foreach (DataRow dr in dt.Rows)
            {
                s = new ServerViewModel(dr, serverID);
                break;
            }
            return s;
        }
    }
}