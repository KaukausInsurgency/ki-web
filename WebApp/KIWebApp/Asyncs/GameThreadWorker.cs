using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using Microsoft.AspNet.SignalR;
using KIWebApp.Classes;
using MySql.Data.MySqlClient;
using StackExchange.Redis;
using System.Data;

namespace KIWebApp.Asyncs
{


    public class GameThreadWorker : IDisposable
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IHubContext hub;
        private IDAL dal;
        private ISubscriber sub;
        private IDatabase redisDB;
        private IAppSettings appSettings;
        private IDbConnection MySqlConnection;
        private IConnectionMultiplexer RedisConnection;
        private System.Threading.Timer timer_poll_server;
        private System.Threading.Timer timer_poll_onlineplayers;
        private const int POLL_SERVER_PERIOD = 30000;
        private const int POLL_ONLINEPLAYERS_PERIOD = 10000;
        public int ServerID { get; private set; }

        public GameThreadWorker(int serverID, IAppSettings appSettings)
        {
            logger.Info("Game Thread Worker Starting (ServerID: " + serverID + ")");
            MySqlConnection = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DBMySqlConnect"].ConnectionString);
            MySqlConnection.Open();
            RedisConnection = ConnectionMultiplexer.Connect(System.Configuration.ConfigurationManager.ConnectionStrings["DBRedisConnect"].ConnectionString);
            redisDB = RedisConnection.GetDatabase();
            hub = GlobalHost.ConnectionManager.GetHubContext<KIWebApp.Hubs.GameHub>();
            dal = new DAL();
            this.ServerID = serverID;
            this.appSettings = appSettings;
            sub = RedisConnection.GetSubscriber();
            sub.Subscribe(new RedisChannel(appSettings.RedisEnvironmentPrefix + ":" + this.ServerID.ToString() + ":*", RedisChannel.PatternMode.Pattern), this.OnRedisSubscription);
            timer_poll_server = new System.Threading.Timer(this.UpdateServer, null, 0, POLL_SERVER_PERIOD);
            timer_poll_onlineplayers = new Timer(this.UpdateOnlinePlayers, null, 0, POLL_ONLINEPLAYERS_PERIOD);
        }

        // constructor used in unit tests ONLY
        public GameThreadWorker(int serverID, IAppSettings appSettings, IHubContext hub, IDAL dal, IDbConnection dbconn, IConnectionMultiplexer redisconn)
        {
            this.ServerID = ServerID;
            this.appSettings = appSettings;
            this.hub = hub;
            this.dal = dal;
            this.MySqlConnection = dbconn;
            this.RedisConnection = redisconn;
            redisDB = RedisConnection.GetDatabase();
            sub = RedisConnection.GetSubscriber();
            sub.Subscribe(new RedisChannel(this.appSettings.RedisEnvironmentPrefix + ":" + this.ServerID.ToString() + ":*", RedisChannel.PatternMode.Pattern), this.OnRedisSubscription);
            timer_poll_server = new System.Threading.Timer(this.UpdateServer, null, 0, POLL_SERVER_PERIOD);
            timer_poll_server.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void Dispose()
        {
            logger.Info("Game Thread Worker Closing (ServerID: " + this.ServerID + ")");
            sub.UnsubscribeAll(CommandFlags.None);
            MySqlConnection.Close();
            RedisConnection.Close();
            timer_poll_server.Dispose();
            timer_poll_onlineplayers.Dispose();
        }

        private void OnRedisSubscription(RedisChannel channel, RedisValue message)
        {
            logger.Info("Redis Subscription On Channel " + channel);
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
                    logger.Error("Error creating Redis Connection - " + ex.Message);
                    logger.Error(ex);
                    hub.Clients.Group(ServerID.ToString()).OnServerError("An internal server error occurred");
                    return;
                }
            }

            try
            {
                dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(message.ToString());
                string channelString = channel.ToString();
                if (channelString.Contains(appSettings.RedisKeyCapturePoint))
                    hub.Clients.Group(ServerID.ToString()).UpdateCapturePoints(json);
                else if (channelString.Contains(appSettings.RedisKeyDepot))
                    hub.Clients.Group(ServerID.ToString()).UpdateDepots(json);
                else if (channelString.Contains(appSettings.RedisKeySideMission))
                    hub.Clients.Group(ServerID.ToString()).UpdateMissions(json);
                else if (channelString.Contains(appSettings.RedisKeyChat))
                    hub.Clients.Group(ServerID.ToString()).UpdateChat(json);
            }
            catch (Exception ex)
            {
                logger.Error("Error invoking SignalR callback - " + ex.Message);
                logger.Error(ex);
                hub.Clients.Group(ServerID.ToString()).OnServerError("An internal server error occurred");
            }
            
        }

        private void UpdateServer(object state)
        {
            hub.Clients.Group(ServerID.ToString()).UpdateServer(dal.GetServerInfo(this.ServerID));
        }

        private void UpdateOnlinePlayers(object state)
        {
            hub.Clients.Group(ServerID.ToString()).UpdateOnlinePlayers(dal.GetOnlinePlayers(this.ServerID));
        }
    }
}