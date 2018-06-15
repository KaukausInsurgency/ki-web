using System.Web;
using System.Web.Optimization;

namespace KIWebApp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Scripts

            bundles.Add(new StyleBundle("~/bundles/shared").Include(
                        "~/Scripts/shared/navigation.js",
                        "~/Scripts/shared/shared.js"));

            bundles.Add(new ScriptBundle("~/bundles/tooltipster").Include(
                "~/Scripts/tooltipster/tooltipster.bundle.js"));

            bundles.Add(new ScriptBundle("~/bundles/dynatable").Include(
                "~/Scripts/dynatable/jquery.dynatable.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                "~/Scripts/jquery.signalR-2.2.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/mustachejs").Include(
                "~/Scripts/mustache.js/mustache.js"));





            // Styles

            bundles.Add(new StyleBundle("~/bundles/Content/shared").Include(
                        "~/Content/css/shared/reset.css",
                        "~/Content/css/shared/style.css"));

            bundles.Add(new StyleBundle("~/bundles/Content/tooltipster").Include(
                        "~/Content/css/vendor/tooltipster/tooltipster.bundle.min.css",
                        "~/Content/css/vendor/tooltipster/plugins/tooltipster/sideTip/themes/tooltipster-sideTip-borderless.min.css"));

            bundles.Add(new StyleBundle("~/bundles/Content/dynatable").Include(
                "~/Content/css/vendor/dynatable/jquery.dynatable.css"));

        }
    }
}