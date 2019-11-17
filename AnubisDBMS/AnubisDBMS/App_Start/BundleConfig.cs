using System.Web;
using System.Web.Optimization;

namespace AnubisDBMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        //"~/Scripts/jquery.js"));
                        "~/Scripts/jQuery.print.js",
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/Js").Include(
                "~/Scripts/jquery.dataTables.min.js",
                "~/Scripts/jszip.min.js",
                "~/Scripts/moment.js",
                "~/Scripts/buttons.html5.min.js",
                "~/Scripts/buttons.print.min.js",
                "~/Scripts/buttons.semanticui.min.js",
                "~/Scripts/vfs_fonts.js",
                "~/Scripts/buttons.flash.min.js",
                "~/Scripts/dataTables.buttons.min.js",
                "~/Scripts/dataTables.semanticui.min.js",
                "~/Scripts/moment-with-locales.js",
                "~/Scripts/respond.js",
                "~/Scripts/dataTables.fixedHeader.min.js",
                "~/Scripts/pdfmake.min.js",
                "~/Scripts/fullcalendar.min.js",
                "~/Scripts/jquery.qtip.min.js",
                "~/Scripts/mdtimepicker.js",
                "~/Scripts/jquery.timepicker.js",
                "~/Scripts/funcionesGlobales.js"

               ));

            bundles.Add(new StyleBundle("~/bundles/JsLibraries").Include(
                "~/Content/Libraries/Semantic/semantic.min.js",
                "~/Content/Libraries/BootstrapDatepicker/bootstrap-datepicker.min.js",
                "~/Content/Libraries/BootstrapDatepicker/locales/bootstrap-datepicker.es.min.js",
                "~/Scripts/wickedpicker.min.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/jquery-ui.css",
                "~/Content/jquery.dataTables.min.css",
                "~/Content/dataTables.semanticui.min.css",
               "~/Content/buttons.semanticui.min.css",
                "~/Content/fixedHeader.semanticui.min.css",
                //"~/Content/fullcalendar.min.css",
                //"~/Content/fullcalendar.print.css",
                //"~/Content/jquery.qtip.min.css",
                //"~/Content/mdtimepicker.css",
                //"~/Content/jquery.timepicker.css",
                "~/Content/wickedpicker.min.css",
                "~/Content/Css/Site.css"
                ));


            bundles.Add(new StyleBundle("~/bundles/CssLibraries").Include(
                "~/Content/Libraries/Semantic/semantic.min.css",
                "~/Content/Libraries/BootstrapDatepicker/bootstrap-datepicker3.standalone.min.css"
                ));
        }
    }
}
