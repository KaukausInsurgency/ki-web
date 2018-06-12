using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class GameModel
    {
        public int ServerID { get; set; }
        public string ServerName { get; set; }
        public string IPAddress { get; set; }
        public bool SimpleRadioEnabled { get; set; }
        public string SimpleRadioIPAddress { get; set; }
        public string Status { get; set; }
        public string RestartTime { get; set; }
        public int OnlinePlayersCount { get; set; }
        public int RedforPlayersCount { get; set; }
        public int BluforPlayersCount { get; set; }
        public List<DepotModel> Depots { get; set; }
        public List<CapturePointModel> CapturePoints { get; set; }
        public List<OnlinePlayerModel> OnlinePlayers { get; set; }
        public List<SideMissionModel> Missions { get; set; }
        public List<CustomMenuItemModel> CustomMenuItems { get; set; }

    }
}