using System.Web;
using System.Web.Optimization;

namespace EShop
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*", 
                        "~/Scripts/price-range.js", 
                        "~/Scripts/jquery.scrollUp.min.js",
                        "~/Scripts/jquery.prettyPhoto.js",
                        "~/Scripts/main.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));
            bundles.Add(new StyleBundle("~/css").Include(
                      "~/css/font-awesome.min.css",
                      "~/css/price-range.css",
                      "~/css/prettyPhoto.css",
                      "~/css/bootstrap.min.css",
                      "~/css/animate.css",
                      "~/css/main.css",
                      "~/css/responsive.css"
                ));
        }
    }
}
