using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using Microsoft.AspNet.SignalR;
using KIWebApp.Classes;
using MySql.Data.MySqlClient;
using StackExchange.Redis;

namespace KIWebApp.Asyncs
{


    public class GameThreadWorker : IDisposable
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private System.Threading.Timer timer_update_markers;
        private System.Threading.Timer timer_update_players;
        private System.Threading.Timer timer_update_server;
        private IHubContext hub;
        private IDAL dal;
        private ISubscriber sub;
        IDatabase redisDB;
        private const int UPDATE_MARKERS_PERIOD = 10000;
        private const int UPDATE_PLAYERS_PERIOD = 5000;
        private const int UPDATE_SERVER_PERIOD = 30000;
        private MySqlConnection MySqlConnection;
        private ConnectionMultiplexer RedisConnection;
        public int ServerID { get; private set; }

        public GameThreadWorker(int serverID)
        {
            logger.Info("Game Thread Worker Starting (ServerID: " + serverID + ")");
            MySqlConnection = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DBMySqlConnect"].ConnectionString);
            MySqlConnection.Open();
            RedisConnection = ConnectionMultiplexer.Connect(System.Configuration.ConfigurationManager.ConnectionStrings["DBRedisConnect"].ConnectionString);
            redisDB = RedisConnection.GetDatabase();
            hub = GlobalHost.ConnectionManager.GetHubContext<KIWebApp.Hubs.GameHub>();
            dal = new DAL();
            this.ServerID = serverID;
            sub = RedisConnection.GetSubscriber();
            sub.Subscribe(new RedisChannel(this.ServerID.ToString(), RedisChannel.PatternMode.Literal), this.OnRedisSubscription);

            timer_update_markers = new System.Threading.Timer(this.UpdateMarkers, null, 0, UPDATE_MARKERS_PERIOD);
            timer_update_players = new System.Threading.Timer(this.UpdatePlayers, null, 0, UPDATE_PLAYERS_PERIOD);
            timer_update_server = new System.Threading.Timer(this.UpdateServer, null, 0, UPDATE_SERVER_PERIOD);
        }

        public void Pause()
        {
            timer_update_markers.Change(Timeout.Infinite, Timeout.Infinite);
            timer_update_players.Change(Timeout.Infinite, Timeout.Infinite);
            timer_update_server.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void Resume()
        {
            timer_update_markers.Change(0, UPDATE_MARKERS_PERIOD);
            timer_update_players.Change(0, UPDATE_PLAYERS_PERIOD);
            timer_update_server.Change(0, UPDATE_SERVER_PERIOD);
        }

        public void Dispose()
        {
            logger.Info("Game Thread Worker Closing (ServerID: " + this.ServerID + ")");
            timer_update_markers.Dispose();
            timer_update_players.Dispose();
            timer_update_server.Dispose();
            sub.UnsubscribeAll(CommandFlags.None);
            MySqlConnection.Close();
            RedisConnection.Close();
        }

        private void OnRedisSubscription(RedisChannel channel, RedisValue message)
        {
            if (channel == this.ServerID.ToString())
            {
                if (!RedisConnection.IsConnected)
                {
                    logger.Warn("Redis Connection was lost - attempting to reopen...");
                    try
                    {
                        RedisConnection = ConnectionMultiplexer.Connect(System.Configuration.ConfigurationManager.ConnectionStrings["DBRedisConnect"].ConnectionString);
                        logger.Warn("Redis Connection re-established");
                    }
                    catch (Exception ex)
                    {
                        logger.Warn("Error creating Redis Connection - " + ex.Message);
                        hub.Clients.Group(ServerID.ToString()).LostRedisConnection();
                        return;
                    }
                }

                string jsonstring = redisDB.StringGet(message.ToString());
                dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstring);
                switch (message.ToString())
                {
                    case "CapturePoints":
                        { hub.Clients.Group(ServerID.ToString()).UpdateCapturePoints(json);  break; }
                    case "Depots":
                        { hub.Clients.Group(ServerID.ToString()).UpdateDepots(json); break; }
                    case "Missions":
                        { hub.Clients.Group(ServerID.ToString()).UpdateMissions(json); break; }
                    case "Chat":
                        { hub.Clients.Group(ServerID.ToString()).UpdateChat(json); break; }
                    case "Server":
                        { hub.Clients.Group(ServerID.ToString()).UpdateServer(json); break; }
                }
                
            }
            
        }

        private void UpdateMarkers(object state)
        {
            hub.Clients.Group(ServerID.ToString())
                .UpdateMarkers(dal.GetMarkers(this.ServerID));
        }

        private void UpdatePlayers(object state)
        {
            hub.Clients.Group(ServerID.ToString())
                .UpdateOnlinePlayers(dal.GetOnlinePlayers(this.ServerID));
        }

        private void UpdateServer(object state)
        {
            hub.Clients.Group(ServerID.ToString())
                .UpdateServer(dal.GetServerInfo(this.ServerID));
        }
    }
}