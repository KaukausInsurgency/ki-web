using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class RptSortiesOverTimeModel
    {
        [JsonProperty(PropertyName = "data")]
        public List<RptSortiesOverTimePlotModel> Data { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Airframe { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string ChartType { get; set; }

        public RptSortiesOverTimeModel()
        {
            ChartType = "area";
            Data = new List<RptSortiesOverTimePlotModel>();
        }
    }
}