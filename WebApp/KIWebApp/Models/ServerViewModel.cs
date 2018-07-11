using KIWebApp.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class ServerViewModel
    {
        public int ServerID { get; set; }
        public string Status { get; set; }
        public string RestartTimeString { get; set; }
        public int RestartTime { get; set; }

        public ServerViewModel(DataRow dr, int serverID)
        {
            RestartTimeString = SqlUtility.ConvertTimeTicksToStringInt(ref dr, "RestartTime");
            RestartTime = SqlUtility.GetValueOrDefault<int>(dr, "RestartTime", 0);
            Status = SqlUtility.GetValueOrDefault<string>(dr, "Status", "Offline");
            ServerID = serverID;
        }
    }
}