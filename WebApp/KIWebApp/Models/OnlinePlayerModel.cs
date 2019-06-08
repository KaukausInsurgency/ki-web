using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class OnlinePlayerModel
    {
        public string UCID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public int Side { get; set; }
        public string Ping { get; set; }
        public string Lives { get; set; }

        public OnlinePlayerModel() { }
    }
}