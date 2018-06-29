using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class RptPlayerOnlineActivityModel
    {
        public List<object[]> Series { get; set; }
        public RptPlayerOnlineActivityModel()
        {
            Series = new List<object[]>();
        }
    }
}