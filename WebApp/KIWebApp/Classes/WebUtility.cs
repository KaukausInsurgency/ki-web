using System.Web;

namespace KIWebApp.Classes
{
    public class WebUtility
    {
        public static string GetApplicationPath(HttpContext context)
        {
            string appPath = string.Format("{0}://{1}{2}{3}",
              context.Request.Url.Scheme,
              context.Request.Url.Host,
              context.Request.Url.Port == 80 ? string.Empty : ":" + System.Web.HttpContext.Current.Request.Url.Port,
              context.Request.ApplicationPath);
            return appPath;
        }
    }
}