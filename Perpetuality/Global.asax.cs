using Perpetuality.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Perpetuality
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static string HostIPAddress(string ip)
        {
            if (ip == "::1")
                ip = "94.100.112.225";
            return ip;
        }

        public static string HostIPAddress(HttpContextBase context)
        {
            string ip = context.Request.Headers["X-FORWARDED-FOR"];
            if (string.IsNullOrEmpty(ip))
                ip = HostIPAddress(context.Request.UserHostAddress);
            return ip;
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            AuthConfig.RegisterAuth();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}