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
        public string RestartTime { get; set; }

        public ServerViewModel(DataRow dr, int serverID)
        {
            if (dr["RestartTime"] == DBNull.Value || dr["RestartTime"] == null)
                RestartTime = new TimeSpan(0, 0, 0).ToString();
            else
                RestartTime = new TimeSpan(TimeSpan.TicksPerSecond * dr.Field<int>("RestartTime")).ToString();

            Status = "Offline";
            if (dr["Status"] != DBNull.Value && dr["Status"] != null)
                Status = dr.Field<string>("Status");

            ServerID = serverID;
        }
    }
}