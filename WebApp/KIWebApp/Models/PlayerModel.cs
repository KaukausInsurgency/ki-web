using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class PlayerModel
    {
        public string UCID { get; set; }
        public string Name { get; set; }

        public PlayerModel(DataRow dr)
        {
            UCID = dr.Field<string>("UCID");
            Name = dr.Field<string>("Name");
        }
    }
}