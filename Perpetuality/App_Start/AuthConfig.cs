using DotNetOpenAuth.AspNet.Clients;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Perpetuality.App_Start
{
    public class AuthConfig
    {
        public static void RegisterAuth()
        {
            OAuthWebSecurity.RegisterClient(new FacebookClient(
                appId: ConfigurationSettings.AppSettings["facebook_appid"],
                appSecret: ConfigurationSettings.AppSettings["facebook_appsecret"])
            );
        }
    }
}