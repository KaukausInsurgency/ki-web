using KIWebApp.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class RptPlayerBestSortieStatsModel
    {
        public string LongestSortie { get; set; }
        public int MostKills { get; set; }
        public int MostHitsReceived { get; set; }
        public RptPlayerBestSortieStatsModel(DataRow dr)
        {
            LongestSortie = SqlUtility.ConvertTimeTicksToStringLong(ref dr, "LongestSortie");
            MostKills = dr.Field<int>("MostKills");
            MostHitsReceived = dr.Field<int>("MostHitsReceived");
        }
    }
}