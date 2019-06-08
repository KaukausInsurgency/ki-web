using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class ServerModel
    {
        public int ServerID { get; set; }
        public string ServerName { get; set; }
        public string IPAddress { get; set; }
        public string Status { get; set; }
        public string StatusImage { get; set; }
        public TimeSpan RestartTime { get; set; }
        public int OnlinePlayers { get; set; }

        public ServerModel() { }

        public ServerModel(DataRow dr)
        {
            if (dr["RestartTime"] == DBNull.Value || dr["RestartTime"] == null)
                RestartTime = new TimeSpan(0, 0, 0);
            else
                RestartTime = new TimeSpan(TimeSpan.TicksPerSecond * dr.Field<int>("RestartTime"));
           
            if (dr["Status"] != DBNull.Value && dr["Status"] != null)
            {
                Status = dr.Field<string>("Status");
                if (Status.ToUpper() == "ONLINE")
                    StatusImage = "Images/status-green-128x128.png";
                else if (Status.ToUpper() == "OFFLINE")
                    StatusImage = "Images/status-red-128x128.png";
                else
                    StatusImage = "Images/status-yellow-128x128.png";
            }
            else
            {
                Status = "Offline";
                StatusImage = "Images/status-red-128x128.png";
            }

            ServerID = dr.Field<int>("ServerID");
            ServerName = dr.Field<string>("ServerName");
            IPAddress = dr.Field<string>("IPAddress");
            OnlinePlayers = Convert.ToInt32(dr.Field<long>("OnlinePlayers"));
        }
    }
}