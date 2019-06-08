using KIWebApp.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class RptScoreOverTimePlotModel
    {
        [JsonProperty(PropertyName = "x")]
        public long Date { get; set; }
        [JsonProperty(PropertyName = "y")]
        public int Count { get; set; }

        public RptScoreOverTimePlotModel(DataRow dr, string colname)
        {
            Date = DateTimeJavaScript.ToJavaScriptMilliseconds(dr.Field<DateTime>("Date"));
            Count = dr.Field<int>(colname);
        }
    }
}