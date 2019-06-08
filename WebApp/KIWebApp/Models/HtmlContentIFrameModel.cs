using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KIWebApp.Models
{
    public class HtmlContentIFrameModel : IHtmlContentModel
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public string Source { get; set; }
        public string Attributes { get; set; }

        public HtmlContentIFrameModel(int h, int w, string src, string attr)
        {
            Height = h;
            Width = w;
            Source = src;
            Attributes = attr;
        }

        private string BuildHtml()
        {
            string html = "<iframe src=\"" + Source + "\" width=\"" + Width + "\" height=\"" + Height + "\" " + Attributes + "></iframe>";
            return html;
        }

        string IHtmlContentModel.HtmlContent { get => BuildHtml(); }
    }
}