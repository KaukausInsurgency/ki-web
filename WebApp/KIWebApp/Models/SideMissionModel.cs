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
        public string TaskName { get; set; }
        public string TaskDesc { get; set; }
        public string IconClass { get; set; }
        public string Status { get; set; }
        public bool StatusChanged { get; set; }
        public string TimeRemaining { get; set; }
        public double TimeInactive { get; set; }
        public string LatLong { get; set; }
        public string MGRS { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}