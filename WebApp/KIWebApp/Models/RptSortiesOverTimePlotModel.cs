using KIWebApp.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class RptSortiesOverTimePlotModel
    {
        [JsonProperty(PropertyName = "x")]
        public long X { get; set; }
        [JsonProperty(PropertyName = "y")]
        public int Y { get; set; }
        [JsonProperty(PropertyName = "kills")]
        public int Kills { get; set; }
        [JsonProperty(PropertyName = "deaths")]
        public int Deaths { get; set; }
        public RptSortiesOverTimePlotModel(DataRow dr)
        {
            X = DateTimeJavaScript.ToJavaScriptMilliseconds(dr.Field<DateTime>("Date"));
            Y = dr.Field<int>("Sorties");
            Kills = dr.Field<int>("Kills");
            Deaths = dr.Field<int>("Deaths");
        }
    }
}