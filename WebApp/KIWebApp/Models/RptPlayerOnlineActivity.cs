using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class RptPlayerOnlineActivity
    {
        public List<object[]> Series { get; set; }
        public RptPlayerOnlineActivity()
        {
            Series = new List<object[]>();
        }
    }
}