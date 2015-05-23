using System.Web;
using System.Web.Optimization;

namespace mfc.web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/jquery").Include(
                        "~/Content/js/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/jqueryui").Include(
                        "~/Content/js/jquery-ui-{version}.js",
                        "~/Content/js/datepicker-ru.js"));

            bundles.Add(new ScriptBundle("~/jqueryval").Include(
                        "~/Content/js/jquery.unobtrusive*",
                        "~/Content/js/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bootstrap").Include(
                        "~/Content/js/bootstrap.min.js",
                        "~/Content/js/npm.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/js/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/css/site.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/css/bootstrap.min.css",
                "~/Content/css/bootstrap-theme.min.css"));
        }
    }
}