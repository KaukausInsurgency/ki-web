using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class CustomMenuItemModel
    {
        public string MenuName { get; set; }
        public string IconClass { get; set; }
        public IHtmlContentModel Content { get; set; }

        public CustomMenuItemModel(DataRow dr)
        {
            MenuName = dr.Field<string>("MenuName");
            IconClass = dr.Field<string>("IconClass");
            Content = new HtmlContentSimpleModel(dr.Field<string>("HtmlContent"));
        }
    }
}