using KIWebApp.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class RptAirframeBasicStatsModel
    {
        public string Airframe { get; set; }
        public string Time { get; set; }
        public int Sorties { get; set; }
        public string Top { get; set; }
        public RptAirframeBasicStatsModel(DataRow dr)
        {
            Airframe = dr.Field<string>("Airframe");
            Time = SqlUtility.ConvertTimeTicksToStringLong(ref dr, "Time");
            Sorties = dr.Field<int>("Sorties");
            Top = dr.Field<string>("Top");
        }
    }
}