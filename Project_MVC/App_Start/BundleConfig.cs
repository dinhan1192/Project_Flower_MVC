using System.Web;
using System.Web.Optimization;

namespace Project_MVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                      "~/Scripts/Customs/deleteNotify.js",
                      "~/Scripts/Customs/autocomplete.js",
                      "~/Scripts/Customs/checkFileSize.js",
                      "~/Scripts/Customs/checkboxselectallWithPopup.js",
                      "~/Scripts/Customs/twoDropDownListEvent.js",
                      "~/Scripts/Customs/createPopup.js",
                      "~/Scripts/Customs/ckeditor.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/cloudinaryAddImage").Include(
                      "~/Scripts/Customs/cloudinaryAddImage.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/video").Include(
                      "~/Scripts/Customs/customerMustWatchVideo.js",
                      "~/Scripts/Customs/preventSeekingVideo.js",
                      "~/Scripts/Customs/setStyleDefault.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/typeahead").Include(
                      "~/Scripts/bootstrap3-typeahead.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // layout admin
            bundles.Add(new StyleBundle("~/Css").Include(
                     //"~/Content/vendor/fontawesome-free/css/all.min.css",
                     //"~/Content/vendor/datatables/dataTables.bootstrap4.css",
                     //"~/Content/css/sb-admin.css",
                     //"~/Content/css/jquery-confirmPopup.css",
                     //layout test tiep
                     "~/Content/vendors/bootstrap/dist/css/bootstrap.min.css",
                     "~/Content/vendors/font-awesome/css/font-awesome.min.css",
                     "~/Content/vendors/nprogress/nprogress.css",
                     "~/Content/vendors/iCheck/skins/flat/green.css",
                     "~/Content/vendors/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css",
                     "~/Content/vendors/jqvmap/dist/jqvmap.min.css",
                     "~/Content/vendors/bootstrap-daterangepicker/daterangepicker.css",
                     "~/Content/build/css/custom.min.css",
                     "~/Content/css/jquery-confirmPopup.css"
                     ));

            bundles.Add(new ScriptBundle("~/Js").Include(
                      //"~/Scripts/vendor/bootstrap/js/bootstrap.bundle.min.js",
                      //"~/Scripts/vendor/jquery-easing/jquery.easing.min.js",
                      //"~/Scripts/vendor/chart.js/Chart.min.js",
                      //"~/Scripts/vendor/datatables/jquery.dataTables.js",
                      //"~/Scripts/vendor/datatables/dataTables.bootstrap4.js",
                      //"~/Scripts/js/sb-admin.min.js",
                      //"~/Scripts/js/demo/datatables-demo.js",
                      //"~/Scripts/Customs/jquery-confirmPopup.js",
                      //"~/Scripts/js/demo/chart-area-demo.js"
                      // layout test tiep
                      //"~/Scripts/vendors/jquery/dist/jquery.min.js",
                      //"~/Scripts/vendors/bootstrap/dist/js/bootstrap.min.js",
                      "~/Scripts/vendors/fastclick/lib/fastclick.js",
                      "~/Scripts/vendors/nprogress/nprogress.js",
                      "~/Scripts/vendors/Chart.js/dist/Chart.min.js",
                      "~/Scripts/vendors/gauge.js/dist/gauge.min.js",
                      "~/Scripts/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js",
                      "~/Scripts/vendors/iCheck/icheck.min.js",
                      "~/Scripts/vendors/skycons/skycons.js",
                      "~/Scripts/vendors/Flot/jquery.flot.js",
                      "~/Scripts/vendors/Flot/jquery.flot.pie.js",
                      "~/Scripts/vendors/Flot/jquery.flot.time.js",
                      "~/Scripts/vendors/Flot/jquery.flot.stack.js",
                      "~/Scripts/vendors/Flot/jquery.flot.resize.js",
                      "~/Scripts/vendors/flot.orderbars/js/jquery.flot.orderBars.js",
                      "~/Scripts/vendors/flot-spline/js/jquery.flot.spline.min.js",
                      "~/Scripts/vendors/flot.curvedlines/curvedLines.js",
                      "~/Scripts/vendors/DateJS/build/date.js",
                      "~/Scripts/vendors/jqvmap/dist/jquery.vmap.js",
                      "~/Scripts/vendors/jqvmap/dist/maps/jquery.vmap.world.js",
                      "~/Scripts/vendors/jqvmap/examples/js/jquery.vmap.sampledata.js",
                      "~/Scripts/vendors/moment/min/moment.min.js",
                      "~/Scripts/vendors/bootstrap-daterangepicker/daterangepicker.js",
                      "~/Scripts/build/js/custom.min.js",
                      "~/Scripts/Customs/jquery-confirmPopup.js"
                     ));

            bundles.Add(new StyleBundle("~/customs").Include(
                      "~/Content/Customs/completeAndInComplete.css",
                      "~/Content/Customs/ckeditor.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/fonts").Include(
                    "~/Content/LayoutAdminPage/fontawesome.css"
                    ));
            // end layout admin

            //start layout user
            bundles.Add(new StyleBundle("~/Css-frontend").Include(
                 "~/Content/Front-end/css/font-awesome.min.css",
                 "~/Content/Front-end/css/owl.carousel.css",
                 "~/Content/Front-end/css/owl.my_theme.css",
                 "~/Content/Front-end/css/owl.transitions.css",
                 "~/Content/Front-end/css/nivo-slider.css",
                 "~/Content/Front-end/css/animate.css",
                 "~/Content/Front-end/css/jquery-ui.css",
                 "~/Content/Front-end/css/jquery.fancybox.css",
                 "~/Content/Front-end/css/normalize.css",
                 "~/Content/Front-end/css/bootstrap.min.css",
                 "~/Content/Front-end/css/custom.css",
                 "~/Content/Front-end/css/meanmenu.min.css",
                 "~/Content/Front-end/css/responsive.css",
                 "~/Content/Front-end/css/main.css",
                 "~/Content/Front-end/css/style.css"
                 ));
            bundles.Add(new ScriptBundle("~/Js-frontend").Include(
                "~/Scripts/js-frontend/vendor/jquery-1.11.3.min.js",
                "~/Scripts/js-frontend/bootstrap.min.js",
                "~/Scripts/js-frontend/jquery.meanmenu.js",
                "~/Scripts/js-frontend/jquery.knob.js",
                "~/Scripts/js-frontend/jquery.fancybox.pack.js",
                "~/Scripts/js-frontend/price-slider.js",
                "~/Scripts/js-frontend/jquery.nivo.slider.pack.js",
                "~/Scripts/js-frontend/wow.js",
                "~/Scripts/js-frontend/nivo-plugin.js",
                "~/Scripts/js-frontend/jquery.scrollUp.js",
                "~/Scripts/js-frontend/owl.carousel.min.js",
                "~/Scripts/js-frontend/plugins.js",
                "~/Scripts/js-frontend/main.js"
                ));
            bundles.Add(new ScriptBundle("~/Modernizr").Include(
                "~/Scripts/js-frontend/vendor/modernizr-2.8.3.min.js"
                ));
            //End layout user


            // BundleTable.EnableOptimizations = true;
        }
    }
}
