﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KIWebApp.Models;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using StackExchange.Redis;

namespace KIWebApp.Classes
{
    public class DAL : IDAL
    {
        private const string SP_GET_SERVERS = "websp_GetServersList";
        private const string SP_GET_ONLINEPLAYERS = "websp_GetOnlinePlayers";
        private const string SP_GET_GAME = "websp_GetGame";
        private const string SP_SEARCH_TOTALS = "websp_SearchTotals";
        private const string SP_SEARCH_PLAYERS = "websp_SearchPlayers";
        private const string SP_SEARCH_SERVERS = "websp_SearchServers";
        private const string SP_GET_SERVER_INFO = "websp_GetServerInfo";
        private const string SP_GET_CUSTOM_MENU_ITEMS = "websp_GetCustomMenuItems";
        private string _DBMySQLConnectionString;
        private string _DBRedisConnectionString;

        
        public IAppSettings AppSettings { get; set; }

        public DAL()
        {
            _DBMySQLConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBMySqlConnect"].ConnectionString;
            _DBRedisConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBRedisConnect"].ConnectionString;
            AppSettings = new WebAppSettings();
        }

        public DAL(string mySQLConnect, string redisConnect, IAppSettings AppSettings)
        {
            _DBMySQLConnectionString = mySQLConnect;
            _DBRedisConnectionString = redisConnect;
            this.AppSettings = AppSettings;
        }

        List<CapturePointModel> IDAL.GetCapturePoints(int serverID)
        {
            IConnectionMultiplexer conn = null;
            try
            {
                conn = ConnectionMultiplexer.Connect(_DBRedisConnectionString);
                return ((IDAL)this).GetCapturePoints(serverID, ref conn);
            }
            finally
            {
                if (conn != null && conn.IsConnected)
                    conn.Close();
            }
        }

        List<CapturePointModel> IDAL.GetCapturePoints(int serverID, ref IConnectionMultiplexer conn)
        {
            return HashGetAllModelCollection<int, CapturePointModel>(ref conn, serverID, AppSettings.RedisEnvironmentPrefix, AppSettings.RedisKeyCapturePoint);
        }

        List<DepotModel> IDAL.GetDepots(int serverID)
        {
            IConnectionMultiplexer conn = null;
            try
            {
                conn = ConnectionMultiplexer.Connect(_DBRedisConnectionString);
                return ((IDAL)this).GetDepots(serverID, ref conn);
            }
            finally
            {
                if (conn != null && conn.IsConnected)
                    conn.Close();
            }
        }

        List<DepotModel> IDAL.GetDepots(int serverID, ref IConnectionMultiplexer conn)
        {
            return HashGetAllModelCollection<int, DepotModel>(ref conn, serverID, AppSettings.RedisEnvironmentPrefix, AppSettings.RedisKeyDepot);
        }

        GameModel IDAL.GetGame(int serverID)
        {
            IDbConnection conn = new MySqlConnection(_DBMySQLConnectionString);
            IConnectionMultiplexer redisconn = null;
            try
            {
                conn.Open();
                redisconn = ConnectionMultiplexer.Connect(_DBRedisConnectionString);
                return ((IDAL)this).GetGame(serverID, ref conn, ref redisconn);
            }
            finally
            {
                conn.Close();
            }
        }

        GameModel IDAL.GetGame(int serverID, ref IDbConnection dbconn, ref IConnectionMultiplexer redisconn)
        {
            if (dbconn.State == ConnectionState.Closed || dbconn.State == ConnectionState.Broken)
                dbconn.Open();
       
            IDbCommand cmd = SqlUtility.CreateCommand(dbconn, SP_GET_GAME,
                new Dictionary<string, object>() { { "ServerID", serverID } });
            DataTable dt = SqlUtility.Execute(cmd);

            if (dt == null)
                return null;

            GameModel g = null;

            foreach (DataRow dr in dt.Rows)
            {
                g = new GameModel(serverID, dr)
                {
                    Depots = ((IDAL)this).GetDepots(serverID, ref redisconn),
                    CapturePoints = ((IDAL)this).GetCapturePoints(serverID, ref redisconn),
                    Missions = ((IDAL)this).GetSideMissions(serverID, ref redisconn),
                    OnlinePlayers = ((IDAL)this).GetOnlinePlayers(serverID, ref dbconn),
                    Chat = ((IDAL)this).GetChatMessages(serverID, ref redisconn),
                    CustomMenuItems = ((IDAL)this).GetCustomMenuItems(serverID, ref dbconn),
                };

                g.OnlinePlayersCount = g.OnlinePlayers.Count;
                g.RedforPlayersCount = g.OnlinePlayers.Count(op => op.Side == 1);
                g.BluforPlayersCount = g.OnlinePlayers.Count(op => op.Side == 2);
                g.NeutralPlayersCount = g.OnlinePlayers.Count(op => op.Side == 0);
                break;
            }
            return g;
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

            List<OnlinePlayerModel> players = new List<OnlinePlayerModel>();

            IDbCommand cmd = SqlUtility.CreateCommand(conn, SP_GET_ONLINEPLAYERS,
                new Dictionary<string, object>() { { "ServerID", serverID } });
            DataTable dt = SqlUtility.Execute(cmd);

            foreach (DataRow dr in dt.Rows)
            {
                OnlinePlayerModel player = new OnlinePlayerModel
                {
                    UCID = dr.Field<string>("UCID"),
                    Name = dr.Field<string>("Name"),
                    Role = dr.Field<string>("Role"),
                    Side = dr.Field<int>("Side"),
                    Ping = dr.Field<string>("Ping"),
                    Lives = SqlUtility.GetValueOrDefault<int>(dr, "Lives", 0).ToString()
                };
                players.Add(player);
            }
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
            IConnectionMultiplexer conn = null ;
            try
            {
                conn = ConnectionMultiplexer.Connect(_DBRedisConnectionString);
                return ((IDAL)this).GetSideMissions(serverID, ref conn);
            }
            finally
            {
                if (conn != null && conn.IsConnected)
                    conn.Close();
            }
        }

        List<SideMissionModel> IDAL.GetSideMissions(int serverID, ref IConnectionMultiplexer conn)
        {
            return HashGetAllModelCollection<int, SideMissionModel>(ref conn, serverID, AppSettings.RedisEnvironmentPrefix, AppSettings.RedisKeySideMission);
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

        List<CustomMenuItemModel> IDAL.GetCustomMenuItems(int serverID)
        {
            IDbConnection conn = new MySqlConnection(_DBMySQLConnectionString);
            try
            {
                conn.Open();
                return ((IDAL)this).GetCustomMenuItems(serverID, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        List<CustomMenuItemModel> IDAL.GetCustomMenuItems(int serverID, ref IDbConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();

            List<CustomMenuItemModel> model = new List<CustomMenuItemModel>();

            IDbCommand cmd = SqlUtility.CreateCommand(conn, SP_GET_CUSTOM_MENU_ITEMS,
                new Dictionary<string, object>() { { "ServerID", serverID } });
            DataTable dt = SqlUtility.Execute(cmd);

            if (dt == null)
                return model;       

            foreach (DataRow dr in dt.Rows)
                model.Add(new CustomMenuItemModel(dr));

            return model;
        }

        List<ChatModel> IDAL.GetChatMessages(int serverID)
        {
            IConnectionMultiplexer conn = null;
            try
            {
                conn = ConnectionMultiplexer.Connect(_DBRedisConnectionString);
                return ((IDAL)this).GetChatMessages(serverID, ref conn);
            }
            finally
            {
                if (conn != null && conn.IsConnected)
                    conn.Close();
            }
        }

        List<ChatModel> IDAL.GetChatMessages(int serverID, ref IConnectionMultiplexer conn)
        {
            return LRangeModelCollection<int, ChatModel>(ref conn, serverID, AppSettings.RedisEnvironmentPrefix, AppSettings.RedisKeyChat);
        }



        private List<ReturnT> HashGetAllModelCollection<T, ReturnT>(ref IConnectionMultiplexer conn, T serverID, string EnvironmentPrefix, string Key) where ReturnT : new()
        {
            List<ReturnT> model = new List<ReturnT>();
            IDatabase db = conn.GetDatabase();
            HashEntry[] v = db.HashGetAll(RedisUtility.BuildRedisKey(EnvironmentPrefix, Key, serverID));

            if (v == null || v.Length == 0)
            {
                return model;
            }
            else
            {
                foreach(HashEntry val in v)
                {
                    model.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<ReturnT>(val.Value));
                }

                return model;
            }
        }

        private List<ReturnT> LRangeModelCollection<T, ReturnT>(ref IConnectionMultiplexer conn, T serverID, string EnvironmentPrefix, string Key) where ReturnT : new()
        {
            List<ReturnT> model = new List<ReturnT>();

            IDatabase db = conn.GetDatabase();
            RedisValue[] v = db.ListRange(RedisUtility.BuildRedisKey(EnvironmentPrefix, Key, serverID), 0, -1);

            if (v == null || v.Length == 0)
            {
                return model;
            }
            else
            {
                foreach (RedisValue val in v)
                {
                    model.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<ReturnT>(val));
                }

                return model;
            }
        }
    }
}