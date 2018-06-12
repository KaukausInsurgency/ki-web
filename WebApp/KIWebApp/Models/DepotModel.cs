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
        public string Status { get; set; }
        public bool StatusChanged { get; set; }
        public string Resources { get; set; }
        public string Image { get; set; }

        public DepotModel() { }

        public DepotModel(DataRow dr)
        {
            ID = dr.Field<int>("DepotID");
            Name = dr.Field<string>("Name");
            LatLong = dr.Field<string>("LatLong");
            MGRS = dr.Field<string>("MGRS");
            Status = dr.Field<string>("Status");
            StatusChanged = dr.Field<ulong>("StatusChanged") == 1;  // for some reason MySQL treats BIT(1) as ulong
            Resources = dr.Field<string>("Resources");
            Image = dr.Field<string>("ImagePath");

            int currentcap = dr.Field<int>("CurrentCapacity");
            int cap = dr.Field<int>("Capacity");

            // if the cap or currentcap are -1, these are marked as supplier depots with infinite resources
            Capacity = (cap == -1 || currentcap == -1) ? "Infinite" : (currentcap + " / " + cap);
        }
    }
}