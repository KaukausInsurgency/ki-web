using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class VersionInfoModel
    {
        public string DCSClientVersion { get; set; }
        public string DCSClientGUID { get; set; }
        public string DCSModVersion { get; set; }
        public string DCSModGUID { get; set; }
    }
}