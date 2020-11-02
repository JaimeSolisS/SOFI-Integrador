using System.Web;
using System.Web.Optimization;

namespace WebSite
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/PLUGINS/jquery/jquery.min.js"
                        //"~/Scripts/PLUGINS/jquery-cookie/jquery-cookie.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/PLUGINS/jquery/jquery-ui.min.js",
                        "~/Scripts/PLUGINS/mcustomscrollbar/jquery.mCustomScrollbar.min.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/PLUGINS/bootstrap/bootstrap.min.js" ,
                        "~/Scripts/PLUGINS/bootstrap/bootstrap-datepicker.js",
                        "~/Scripts/PLUGINS/bootstrap/bootstrap-datepicker.es.js",
                        "~/Scripts/PLUGINS/bootstrap/bootstrap-select.js",
                        "~/Scripts/PLUGINS/bootstrap/bootstrap-select-es_ES.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/scrolltopcontrol").Include(
                        "~/Scripts/PLUGINS/scrolltotop/scrolltopcontrol.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/icheck").Include(
                        "~/Scripts/PLUGINS/icheck/icheck.min.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                        "~/Scripts/PLUGINS/moment.min.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/daterangepicker").Include(
                        "~/Scripts/PLUGINS/daterangepicker/daterangepicker.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/lobibox").Include(
                        "~/Scripts/PLUGINS/lobi-plugins/lobibox.min.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/noty").Include(
            "~/Scripts/PLUGINS/noty/jquery.noty.js"
            , "~/Scripts/PLUGINS/noty/layouts/topCenter.js"
            , "~/Scripts/PLUGINS/noty/layouts/topLeft.js"
            , "~/Scripts/PLUGINS/noty/layouts/topRight.js"
            , "~/Scripts/PLUGINS/noty/themes/default.js"
            ));

            /*PLUGIN SELECT*/
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-select").Include(
                        "~/Scripts/PLUGINS/bootstrap/bootstrap-select.js"
                        ));
            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css/jquery").Include(
                      "~/Content/PLUGINS/jquery/jquery-ui.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                      "~/Content/PLUGINS/bootstrap/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/fontawesome").Include(
            "~/Content/PLUGINS/fontawesome/font-awesome.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/lobibox").Include(
                    "~/Content/PLUGINS/lobibox/lobibox.css"));

            bundles.Add(new StyleBundle("~/Content/css/SOFI").Include(
                      "~/Content/SOFI.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
