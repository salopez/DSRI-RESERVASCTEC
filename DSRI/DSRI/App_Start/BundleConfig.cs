using System.Web;
using System.Web.Optimization;

namespace DSRI
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, consulte http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-{version}.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery-ui-{version}.min.js"));
            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                       "~/Scripts/jquery-ui-{version}.js",
                       "~/Scripts/jquery-ui-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/shared").Include(
                "~/Content/bootstrap-3.2.0/dist/css/bootstrap.min.css",
                "~/Content/font-awesome-4.2.0/css/font-awesome.min.css",
                "~/Content/morris.css",
                "~/Content/jquery-jvectormap-1.2.2.css",
                "~/Content/datepicker3.css",
                "~/Content/daterangepicker-bs3.css",
                "~/Content/AdminLTE.css"
             ));


            bundles.Add(new ScriptBundle("~/bundles/shared").Include(
                   "~/Scripts/plugins/morris/morris.min.js",
                   "~/Scripts/plugins/sparkline/jquery.sparkline.min.js",
                   "~/Scripts/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                   "~/Scripts/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                   "~/Scripts/plugins/jqueryKnob/jquery.knob.js",
                   "~/Scripts/plugins/daterangepicker/daterangepicker.js",
                   "~/Scripts/plugins/datepicker/bootstrap-datepicker.js",
                   "~/Scripts/plugins/iCheck/icheck.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/endpage").Include(
                    "~/Scripts/jquery-2.1.3.min.js",
                    "~/Content/bootstrap-3.2.0/dist/js/bootstrap.min.js",
                    "~/Scripts/AdminLTE/app.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/calendar").Include(
                    "~/Scripts/bootstrap.min.js",
                    "~/Content/bootstrap-3.2.0/dist/js/bootstrap.min.js",
                    "~/Content/bootstrap-3.2.0/dist/js/bootstrap.js",
                    "~/Scripts/jquery-ui.min.js",
                    "~/Scripts/AdminLTE/app.js",
                    "~/Scripts/moment.min.js",
                    "~/Content/calendar/bower_components/fullcalendar/dist/fullcalendar.min.js",
                    "~/Content/bootstrap-3.2.0/dist/js/bootstrap.js",
                    "~/Content/bootstrap-3.2.0/dist/js/bootstrap.min.js",
                    "~/Scripts/jquery-oLoader-v0.1/js/jquery.oLoader.js",
                    "~/Content/calendar/bower_components/fullcalendar/dist/lang/es.js",
                    "~/Scripts/calendar/jquery.blockUI.js",
                    "~/Scripts/calendar/CalendarAction.js"
                    ));

            bundles.Add(new StyleBundle("~/Content/create").Include(
                "~/Content/SearchInmueble.css",
                "~/Content/daterangepicker-bs3.css",
                "~/Content/iCheck/all.css",
                "~/Content/bootstrap-colorpicker.min.css",
                "~/Content/bootstrap-timepicker.min.css",
                "~/Content/jquery-ui-timepicker-addon.css",
                "~/Content/sumoselect.css",
                "~/Content/bootstrap-toggle.min.css",
                "~/Content/Reserva/prism.css",
                "~/Content/Reserva/chosen.css",
                "~/Content/Reserva/loadings.css",
                "~/Content/jquery-ui.css"));

            bundles.Add(new ScriptBundle("~/bundles/create").Include(
                "~/Scripts/jquery-2.1.3.min.js",
                "~/Content/bootstrap-3.2.0/dist/js/jquery.min.js",
                "~/Content/bootstrap-3.2.0/dist/js/bootstrap.min.js",
                "~/Scripts/AdminLTE/app.js",
                "~/Scripts/jquery-ui-1.10.4.js",
                "~/Scripts/jquery-ui-timepicker-addon.js",
                "~/Scripts/Reserva/date.js",
                "~/Scripts/Reserva/jquery.bootstrap.wizard.min.js",
                "~/Scripts/bootstrap-toggle.min.js",
                "~/Scripts/Reserva/chosen.jquery.js",
                "~/Scripts/Reserva/Reserva.js"));

            bundles.Add(new ScriptBundle("~/bundles/EditarReserva").Include(
                "~/Scripts/jquery-2.1.3.min.js",
                "~/Content/bootstrap-3.2.0/dist/js/jquery.min.js",
                "~/Content/bootstrap-3.2.0/dist/js/bootstrap.min.js",
                "~/Scripts/AdminLTE/app.js",
                "~/Scripts/jquery-ui-1.10.4.js",
                "~/Scripts/jquery-ui-timepicker-addon.js",
                "~/Scripts/Reserva/date.js",
                "~/Scripts/Reserva/jquery.bootstrap.wizard.min.js",
                "~/Scripts/bootstrap-toggle.min.js",
                "~/Scripts/Reserva/chosen.jquery.js",
                "~/Scripts/Reserva/EditarReserva.js"
                ));

            bundles.Add(new StyleBundle("~/Content/EditarReserva").Include(
                    "~/Content/SearchInmueble.css" ,
                    "~/Content/daterangepicker-bs3.css",
                    "~/Content/iCheck/all.css",
                    "~/Content/bootstrap-colorpicker.min.css",
                    "~/Content/bootstrap-timepicker.min.css",
                    "~/Content/jquery-ui-timepicker-addon.css" ,
                    "~/Content/bootstrap-toggle.min.css" ,
                    "~/Content/actualizando.css" ,
                    "~/Content/jquery-ui.css" 
                ));

            bundles.Add(new ScriptBundle("~/bundles/ListaReservas").Include(
                "~/Content/bootstrap-3.2.0/dist/js/jquery.min.js",
                "~/Content/bootstrap-3.2.0/dist/js/bootstrap.min.js",
                "~/Scripts/plugins/datatables/jquery.dataTables.js",
                "~/Scripts/plugins/datatables/dataTables.bootstrap.js",
                "~/Scripts/AdminLTE/app.js"));



            bundles.Add(new ScriptBundle("~/bundles/general").Include(
                "~/Content/bootstrap-3.2.0/dist/js/jquery.min.js",
                "~/Content/bootstrap-3.2.0/dist/js/bootstrap.min.js",
                "~/Scripts/jquery-2.1.3.min.js",
                "~/Scripts/AdminLTE/app.js",
                "~/Scripts/jquery-ui-1.10.4.js",
                "~/Scripts/jquery-ui-1.10.4.min.js",
                "~/Scripts/jquery-ui-timepicker-addon.js",
                "~/Scripts/Reserva/date.js"
               ));


            bundles.Add(new StyleBundle("~/Content/tablas").Include(
                   "~/Scripts/plugins/DataTables-1.10.4/media/css/jquery.dataTables.css",
                   "~/Scripts/plugins/DataTables-1.10.4/extensions/TableTools/css/dataTables.tableTools.min.css"
                ));
            bundles.Add(new ScriptBundle("~/bundles/endpage").Include(
                     "~/Scripts/jquery-2.1.3.min.js",
                     "~/Content/bootstrap-3.2.0/dist/js/bootstrap.min.js",
                     "~/Scripts/AdminLTE/app.js"
             ));

            bundles.Add(new ScriptBundle("~/bundles/endpageusuarios").Include(
                       "~/Content/bootstrap-3.2.0/dist/js/jquery.min.js",
                        "~/Content/bootstrap-3.2.0/dist/js/bootstrap.min.js",
                        "~/Scripts/AdminLTE/app.js",
                        "~/Scripts/jquery-ui-1.10.4.js",
                        "~/Scripts/jquery-ui-timepicker-addon.js",
                        "~/Scripts/Reserva/date.js"
             ));
            
        }
    }
}
