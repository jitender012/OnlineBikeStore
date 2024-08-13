﻿using System.Web;
using System.Web.Optimization;

namespace OnlineBikeStore
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

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/fontawesome/all.min.js",
                      "~/Scripts/site.js"
                    ));
           
            // Bundle for admin CSS files
            bundles.Add(new StyleBundle("~/Content/admin-css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/admin.css",                     
                      "~/Content/fontawesome.min.css"));

            // Bundle for customer CSS files
            bundles.Add(new StyleBundle("~/Content/customer-css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/customer.css",                                         
                      "~/Content/fontawesome.min.css"));


        }
    }
}
