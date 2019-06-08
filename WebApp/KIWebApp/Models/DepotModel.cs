using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KIWebApp.Classes;
using System.Data;

namespace KIWebApp.Models
{
    public class DepotModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LatLong { get; set; }
        public string MGRS { get; set; }
        public string Capacity { get; set; }
        public string CurrentCapacity { get; set; }
        public string Status { get; set; }
        public bool StatusChanged { get; set; }
        public string ResourceString { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public DepotModel() { }
    }
}