using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KIWebApp.Classes;
using System.Data;

namespace KIWebApp.Models
{
    public class CapturePointModel
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string LatLong { get; set; }
        public string MGRS { get; set; }
        public string Status { get; set; }
        public bool StatusChanged { get; set; }
        public string Text { get; set; }
        public int BlueUnits { get; set; }
        public int RedUnits { get; set; }
        public int MaxCapacity { get; set; }
        public string Image { get; set; }

        public CapturePointModel() { }

        public CapturePointModel(DataRow dr)
        {
            /*
            ID = dr.Field<int>("CapturePointID");
            Type = dr.Field<string>("Type");
            Name = dr.Field<string>("Name");
            LatLong = dr.Field<string>("LatLong");
            MGRS = dr.Field<string>("MGRS");
            MaxCapacity = dr.Field<int>("MaxCapacity");
            Status = dr.Field<string>("Status");
            StatusChanged = dr.Field<ulong>("StatusChanged") == 1;  // for some reason MySql treats BIT(1) as ulong
            BlueUnits = dr.Field<int>("BlueUnits");
            RedUnits = dr.Field<int>("RedUnits");
            Image = dr.Field<string>("ImagePath");
            Text = SqlUtility.GetValueOrDefault(ref dr, "Text", "");
            */
        }
    }
}