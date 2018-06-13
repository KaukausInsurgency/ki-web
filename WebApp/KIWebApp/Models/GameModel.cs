using KIWebApp.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class GameModel
    {
        public int ServerID { get; set; }
        public string ServerName { get; set; }
        public string ServerDescription { get; set; }
        public string IPAddress { get; set; }
        public bool SimpleRadioEnabled { get; set; }
        public string SimpleRadioIPAddress { get; set; }
        public string Status { get; set; }
        public string RestartTimeString { get; set; }
        public int RestartTime { get; set; }
        
        public List<DepotModel> Depots { get; set; }
        public List<CapturePointModel> CapturePoints { get; set; }
        public List<OnlinePlayerModel> OnlinePlayers { get; set; }
        public List<SideMissionModel> Missions { get; set; }
        public List<CustomMenuItemModel> CustomMenuItems { get; set; }

        // these properties are set post construction since they depend on OnlinePlayers model being constructed
        public int OnlinePlayersCount { get; set; }
        public int RedforPlayersCount { get; set; }
        public int BluforPlayersCount { get; set; }
        public int NeutralPlayersCount { get; set; }

        public GameModel(int serverID, DataRow dr)
        {  
            ServerID = serverID;
            ServerName = dr.Field<string>("ServerName");
            ServerDescription = dr.Field<string>("ServerDescription");
            IPAddress = dr.Field<string>("IPAddress");
            SimpleRadioEnabled = dr.Field<ulong>("SimpleRadioEnabled") == 1;
            SimpleRadioIPAddress = dr.Field<string>("SimpleRadioIPAddress");
            OnlinePlayersCount = Convert.ToInt32(dr.Field<long>("OnlinePlayerCount"));
            RestartTime = SqlUtility.GetValueOrDefault(ref dr, "RestartTime", 0);
            RestartTimeString = SqlUtility.ConvertTimeTicksToStringInt(ref dr, "RestartTime");
            Status = SqlUtility.GetValueOrDefault(ref dr, "Status", "Offline");
        }

    }
}