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
        private const string SP_GET_GAMEMAP = "websp_GetGameMap";
        private const string SP_GET_LAYERS = "websp_GetGameMapLayers";
        private const string SP_GET_DEPOTS= "websp_GetDepots";
        private const string SP_GET_CAPTUREPOINTS = "websp_GetCapturePoints";
        private const string SP_GET_GAME = "websp_GetGame";
        private const string SP_GET_SIDEMISSIONS = "websp_GetSideMissions";
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

            GameModel g = new GameModel();

            foreach (DataRow dr in dt.Rows)
            {
                TimeSpan rt;
                if (dr["RestartTime"] == DBNull.Value || dr["RestartTime"] == null)
                    rt = new TimeSpan(0, 0, 0);
                else
                    rt = new TimeSpan(TimeSpan.TicksPerSecond * dr.Field<int>("RestartTime"));

                string status = "Offline";
                if (dr["Status"] != DBNull.Value && dr["Status"] != null)
                    status = dr.Field<string>("Status");

                g.ServerID = serverID;
                g.ServerName = dr.Field<string>("ServerName");
                g.IPAddress = dr.Field<string>("IPAddress");
                g.OnlinePlayersCount = Convert.ToInt32(dr.Field<long>("OnlinePlayerCount"));
                g.RestartTime = rt.ToString();
                g.Status = status;
                g.Depots = ((IDAL)this).GetDepots(serverID, ref conn);
                g.CapturePoints = ((IDAL)this).GetCapturePoints(serverID, ref conn);
                g.Missions = ((IDAL)this).GetSideMissions(serverID, ref conn);
                g.OnlinePlayers = ((IDAL)this).GetOnlinePlayers(serverID, ref conn);
                g.Map = ((IDAL)this).GetGameMap(serverID, ref conn);
                break;
            }
            return g;
        }

        GameMapModel IDAL.GetGameMap(int serverID)
        {
            IDbConnection conn = new MySqlConnection(_DBMySQLConnectionString);
            try
            {
                conn.Open();
                return ((IDAL)this).GetGameMap(serverID, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        GameMapModel IDAL.GetGameMap(int serverID, ref IDbConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
        
            IDbCommand cmd = SqlUtility.CreateCommand(conn, SP_GET_GAMEMAP,
                new Dictionary<string, object>() { { "ServerID", serverID } });
            DataTable dt = SqlUtility.Execute(cmd);

            if (dt == null)
                return null;

            GameMapModel map = new GameMapModel
            {
                MapExists = false,
                Layers = new List<MapLayerModel>()
            };

            foreach (DataRow dr in dt.Rows)
            {
                map.ImagePath = dr.Field<string>("ImagePath");
                map.DCSOriginPosition = new Position(dr.Field<double>("X"), dr.Field<double>("Y"));
                map.Resolution = new Resolution(dr.Field<double>("Width"), dr.Field<double>("Height"));
                map.Ratio = dr.Field<double>("Ratio");
                map.Layers = ((IDAL)this).GetMapLayers(dr.Field<int>("GameMapID"), ref conn);
                map.MapExists = true;
                break;
            }
            return map;
        }

        List<MapLayerModel> IDAL.GetMapLayers(int mapID)
        {
            IDbConnection conn = new MySqlConnection(_DBMySQLConnectionString);
            try
            {
                conn.Open();
                return ((IDAL)this).GetMapLayers(mapID, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        List<MapLayerModel> IDAL.GetMapLayers(int mapID, ref IDbConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
            
            IDbCommand cmd = SqlUtility.CreateCommand(conn, SP_GET_LAYERS,
                new Dictionary<string, object>() { { "MapID", mapID } });
            DataTable dt = SqlUtility.Execute(cmd);

            if (dt == null)
                return null;

            List<MapLayerModel> layers = new List<MapLayerModel>();

            foreach (DataRow dr in dt.Rows)
            {
                MapLayerModel layer = new MapLayerModel(new Resolution(dr.Field<double>("Width"), dr.Field<double>("Height")), dr.Field<string>("ImagePath"));
                layers.Add(layer);
            }
            return layers;
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

            {
                IDbCommand cmd = SqlUtility.CreateCommand(conn, SP_SEARCH_PLAYERS,
                         new Dictionary<string, object>() { { "Criteria", query } });
                DataTable dt = SqlUtility.Execute(cmd);

                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                        results.PlayerResults.Add(new PlayerModel(dr));
                }
            }

            {
                IDbCommand cmd = SqlUtility.CreateCommand(conn, SP_SEARCH_SERVERS,
                        new Dictionary<string, object>() { { "Criteria", query } });
                DataTable dt = SqlUtility.Execute(cmd);

                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                        results.ServerResults.Add(new ServerModel(dr));
                }
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