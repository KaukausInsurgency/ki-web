using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class SearchResultsModel
    {
        public SearchResultsModel()
        {
            PlayerResults = 0;
            ServerResults = 0;
        }

        public SearchResultsModel(DataRow dr)
        {
            PlayerResults = Convert.ToInt32(dr.Field<long>("PlayerResults"));
            ServerResults = Convert.ToInt32(dr.Field<long>("ServerResults"));
        }
        public int PlayerResults;
        public int ServerResults;
        public string Query;
    }
}