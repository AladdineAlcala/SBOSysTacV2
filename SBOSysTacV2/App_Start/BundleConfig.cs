using System.Web.Optimization;

namespace SBOSysTacV2
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/javascripts").Include(
                        "~/Content/bower_components/jquery/dist/jquery.min.js",
                        //"~/Content/bower_components/jquery-ui/jquery-ui.min.js",
                        "~/Content/bower_components/bootstrap/dist/js/bootstrap.min.js",
                        "~/Scripts/moment.min.js",
                        "~/Content/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
                        "~/Content/bower_components/fastclick/lib/fastclick.js",
                        "~/Content/bower_components/datatables.net/js/jquery.dataTables.min.js",
                        "~/Content/bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js",
                        "~/Content/bower_components/datatables.net/extensions/Responsive/js/dataTables.responsive.min.js",
                        "~/Content/bower_components/datatables.net/extensions/Buttons/js/dataTables.buttons.min.js",
                        "~/Content/bower_components/datatables.net/extensions/Select/js/dataTables.select.min.js",
                        "~/Scripts/bootstrap-datetimepicker.min.js",
                        "~/Content/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js",
                        "~/Scripts/datetime-moment.js",
                        "~/Scripts/typeahead.bundle.min.js",
                        "~/Content/bower_components/select2-4.0.6-rc.1/dist/js/select2.full.min.js",
                        //"~/Content/bower_components/select2-4.0.6-rc.1/dist/js/select2.min.js",
                        "~/Content/bower_components/sweetalert/sweetalert2.min.js",
                        "~/Content/dist/js/adminlte.min.js",
                        "~/Scripts/sidebar.js"
                         ));

            bundles.Add(new ScriptBundle("~/bundles/AjaxExtensions").Include(
                            "~/Scripts/jquery.unobtrusive-ajax.js",
                            "~/Scripts/jquery.validate.js",
                            "~/Scripts/jquery.validate.unobtrusive.js"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/Charts").Include(
                "~/Content/bower_components/chartjsvs2.8/Chart.min.js",
                "~/Content/bower_components/chartjsvs2.8/chartjs-plugin-datalabels.min.js"

            ));

            bundles.Add(new ScriptBundle("~/bundles/PrintDatatable").Include(
                "~/Content/bower_components/datatablesPrint/js/buttons.flash.min.js",
                "~/Content/bower_components/datatablesPrint/js/buttons.html5.min.js",
                "~/Content/bower_components/datatablesPrint/js/buttons.print.min.js",
                "~/Content/bower_components/datatablesPrint/js/pdfmake.min.js",
                "~/Content/bower_components/datatablesPrint/js/vfs_fonts.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css_styles").Include(
                      "~/Content/bower_components/bootstrap/dist/css/bootstrap.min.css",
                      "~/Content/bower_components/font-awesome/css/font-awesome.min.css",
                      "~/Content/bower_components/Ionicons/css/ionicons.min.css",
                      "~/Content/bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css",
                      "~/Content/bower_components/datatables.net/extensions/Responsive/css/responsive.dataTables.min.css",
                      "~/Content/bower_components/datatables.net/extensions/Select/css/select.dataTables.min.css",
                      "~/Content/bower_components/datatables.net/extensions/Select/css/select.bootstrap.min.css",
                      "~/Content/bootstrap-datetimepicker.min.css",
                      "~/Content/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css",
                      //"~/Content/bower_components/select2/dist/css/select2.min.css",
                      "~/Content/bower_components/select2-4.0.6-rc.1/dist/css/select2.min.css",
                      "~/Content/dist/css/AdminLTE.min.css",
                      "~/Content/dist/css/skins/_all-skins.min.css",
                      "~/Content/dist/css/skins/skin-green.min.css",
                      "~/Content/customStyle/spinner.css",
                      "~/Content/customStyle/typehead.css",
                      "~/Content/bower_components/sweetalert/sweetalert2.min.css",
                      "~/Content/customStyle/customStyle.css" 
                       ));
        }


    }
}