﻿using System.Web;
using System.Web.Optimization;

namespace WebsiteKunstenboetiek
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/script").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/reset.css",
                      "~/Content/Site.css",
                      "~/Content/responsive.css"));
        }
    }
}
