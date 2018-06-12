using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class CustomMenuItemModel
    {
        public string MenuName { get; set; }
        public string IconClass { get; set; }
        public IHtmlContentModel Content { get; set; }
    }
}