using KIWebApp.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class PlayerModel
    {
        public string UCID { get; set; }
        public string Name { get; set; }
        public bool Banned { get; set; }
        public string GameTime { get; set; }
        public int Sorties { get; set; }
        public int Kills { get; set; }

        public PlayerModel(DataRow dr)
        {
            UCID = dr.Field<string>("UCID");
            Name = dr.Field<string>("Name");
            Banned = dr.Field<ulong>("Banned") == 1;    // for some reason MySql treats BIT(1) as ulong
            Sorties = Convert.ToInt32(dr.Field<long>("Sorties"));
            Kills = Convert.ToInt32(dr.Field<long>("Kills"));
            GameTime = SqlUtility.ConvertTimeTicksToStringLong(ref dr, "GameTime");
        }
    }
}