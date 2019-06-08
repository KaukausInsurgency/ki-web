using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class RptScoreOverTimeModel
    {
        [JsonProperty(PropertyName = "data")]
        public List<RptScoreOverTimePlotModel> Data { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public RptScoreOverTimeModel()
        {
            Data = new List<RptScoreOverTimePlotModel>();
        }

        public RptScoreOverTimeModel(string name) : this()
        {
            Name = name;
        }
    }
}