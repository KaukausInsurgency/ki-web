using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class OnlinePlayerModel
    {
        public string UCID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string RoleImage { get; set; }
        public string Side { get; set; }
        public string Ping { get; set; }
        public string Lives { get; set; }

        public OnlinePlayerModel() { }

        public OnlinePlayerModel(DataRow dr)
        {
            UCID = dr.Field<string>("UCID");
            Name = dr.Field<string>("Name");
            Role = dr.Field<string>("Role");
            RoleImage = dr.Field<string>("RoleImage");
            Ping = dr.Field<string>("Ping");

            Side = "Neutral";
            if (dr.Field<int>("Side") == 1)
                Side = "Red";
            else if (dr.Field<int>("Side") == 2)
                Side = "Blue";

            Lives = "";
            if (dr["Lives"] != DBNull.Value && dr["Lives"] != null)
            {
                Lives = dr.Field<int>("Lives").ToString();
            }
        }
    }
}