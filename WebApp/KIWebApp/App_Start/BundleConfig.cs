using System.Web;
using System.Web.Optimization;

namespace KIWebApp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Tooltips bundle
            bundles.Add(new ScriptBundle("~/bundles/tooltipster").Include(
                "~/Scripts/tooltipster/tooltipster.bundle.js"));

            // dynatable bundle
            bundles.Add(new ScriptBundle("~/bundles/dynatable").Include(
                "~/Scripts/dynatable/jquery.dynatable.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                "~/Scripts/jquery.signalR-2.2.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/registertable.js"));

            // Game Map bundle
            bundles.Add(new ScriptBundle("~/bundles/gamemap").Include("~/Scripts/app/gamemap.js"));


            bundles.Add(new ScriptBundle("~/bundles/statistics").Include(
                        "~/Scripts/app/dashboard-overall.js"));


            bundles.Add(new StyleBundle("~/bundles/Content/tooltipster").Include(
                        "~/Content/tooltipster/tooltipster.bundle.min.css"));

            bundles.Add(new StyleBundle("~/bundles/Content/dynatable").Include(
                "~/Content/dynatable/jquery.dynatable.css"));

            bundles.Add(new StyleBundle("~/bundles/Content/app").Include(
                "~/Content/app/app.css"));

            bundles.Add(new StyleBundle("~/bundles/Content/statistics").Include(
                "~/Content/app/dashboard-overall.css"));
        }
    }
}