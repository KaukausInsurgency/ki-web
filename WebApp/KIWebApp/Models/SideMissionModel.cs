using KIWebApp.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class SideMissionModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public bool StatusChanged { get; set; }
        public string TimeRemaining { get; set; }
        public double TimeInactive { get; set; }
        public string LatLong { get; set; }
        public string MGRS { get; set; }

        public SideMissionModel(DataRow dr)
        {
            ID = dr.Field<int>("ServerMissionID");
            Name = dr.Field<string>("Name");
            Desc = dr.Field<string>("Description");
            Image = dr.Field<string>("ImagePath");
            Status = dr.Field<string>("Status");
            StatusChanged = dr.Field<ulong>("StatusChanged") == 1;  // for some reason MySQL treats BIT(1) as ulong
            LatLong = dr.Field<string>("LatLong");
            MGRS = dr.Field<string>("MGRS");

            TimeRemaining = SqlUtility.ConvertTimeTicksToStringDouble(ref dr, "TimeRemaining");

            if (dr["TimeInactive"] != DBNull.Value && dr["TimeInactive"] != null)
                TimeInactive = ((TimeSpan)(DateTime.Now - dr.Field<DateTime>("TimeInactive"))).TotalSeconds;
            else
                TimeInactive = 0;
        }
    }
}