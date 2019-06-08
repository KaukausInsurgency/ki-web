using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class HtmlContentSimpleModel : IHtmlContentModel
    {
        public string Html { get; set; }
        public HtmlContentSimpleModel(string html)
        {
            Html = html;
        }
        string IHtmlContentModel.HtmlContent => Html;
    }
}