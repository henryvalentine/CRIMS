using System.Web;
using System.Web.Optimization;

namespace Crims.UI.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            
                bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/jquery-ui.min.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/ExportToExcel/xlsx.core.min.js",
                      "~/Scripts/ExportToExcel/alasql.min.js",
                      "~/Scripts/ExportToExcel/ReadExcel.js",
                      "~/Scripts/dual-list-box.min.js",
                      "~/Scripts/typeahead.bundle.js",
                      "~/Scripts/bootstrap-tagsinput.js",
                      "~/Scripts/inputTags.jquery.js",
                     "~/Scripts/jquery.datetimepicker.full.min.js",
                      "~/Scripts/rapheal.min.js",
                     "~/Scripts/morris.min.js",
                      "~/Scripts/jquery.sparkline.min.js",
                      "~/Scripts/jquery-jvectormap-1.2.2.min.js",
                      "~/Scripts/jquery-jvectormap-world-mill-en.js",
                      "~/Scripts/jquery.knob.js",
                      "~/Scripts/moment.min.js",
                      "~/Scripts/daterangepicker.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/bootstrap3-wysihtml5.all.min.js",
                      "~/Scripts/jquery.slimscroll.min.js",
                      "~/Scripts/DataTables/jquery.dataTables.min.js",
                      "~/Scripts/DataTables/dataTables.bootstrap.min.js",
                      "~/Scripts/dataTableDescription.js",
                      "~/Scripts/fastclick.min.js",
                      "~/Scripts/app.min.js",
                      "~/Scripts/dashboard.js",
                      "~/Scripts/custom.js",
                      "~/Scripts/respond.js"));
            
                       bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/ngDialog.css",
                      "~/Content/ngDialog-theme-default.css",
                      "~/Content/bootstrap-tagsinput.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/jquery.datetimepicker.min.css",
                      "~/Content/ionicons.min.css",
                      "~/Content/AdminLTE.min.css",
                      "~/Content/skins/_all-skins.min.css",
                      "~/Content/blue.css",
                      "~/Content/morris.css",
                      "~/Content/jquery-jvectormap-1.2.2.css",
                      "~/Content/datepicker3.css",
                      "~/Content/daterangepicker-bs3.css",
                       "~/Content/DataTables/css/dataTables.bootstrap.min.css",
                      "~/Scripts/datatables.min.css",
                      "~/Content/bootstrap3-wysihtml5.min.css"));

            bundles.Add(new StyleBundle("~/Content/css-custom").Include(
           "~/Content/custom.css"));

            bundles.Add(new StyleBundle("~/Content/css-admin-custom").Include(
           "~/Content/custom-admin.css"));
            BundleTable.EnableOptimizations = true;
        }
    }
}


