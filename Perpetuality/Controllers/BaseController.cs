using Perpetuality.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Perpetuality
{
    public class GameIdentity : IIdentity
    {
        public GameIdentity(string name, long id)
        {
            this.Name = name;
            this.UserID = id;
        }

        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(this.Name); }
        }

        public string Name { get; private set; }
        public long UserID { get; private set; }
    }

    public class GamePrincipal : IPrincipal
    {
        public GamePrincipal(GameIdentity identity, string[] roles = null)
        {
            this.Identity = identity;
            this.Roles = roles;
        }

        public string[] Roles { get; private set; }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}

namespace Perpetuality.Controllers
{
    public partial class BaseController : Controller
    {
        public string JAAPToken;
        public string HostIPAddress;

        //
        // GET: /Base/

        public void ExtraInitialization()
        {
            HostIPAddress = MvcApplication.HostIPAddress(HttpContext);
            if (Request.Cookies["GameToken"] != null)
                JAAPToken = Request.Cookies["GameToken"].Value;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ExtraInitialization();
        }

        private CultureInfo GetCultureInfo(AuthorizationContext filterContext)
        {
            switch ((string)filterContext.RouteData.Values["language"])
            {
                case "nl":
                    return CultureInfo.CreateSpecificCulture("nl-NL");
                case "en":
                    return CultureInfo.CreateSpecificCulture("en-US");
                case "pt":
                    return CultureInfo.CreateSpecificCulture("pt-PT");
                case "de":
                    return CultureInfo.CreateSpecificCulture("de-DE");
                case "es":
                    return CultureInfo.CreateSpecificCulture("es-ES");
                case "fr":
                    return CultureInfo.CreateSpecificCulture("fr-FR");
                case "it":
                    return CultureInfo.CreateSpecificCulture("it-IT");
                case "jp":
                    return CultureInfo.CreateSpecificCulture("ja-JP");
                default:
                    return CultureInfo.CreateSpecificCulture("en-US");
            }
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            Thread.CurrentThread.CurrentCulture = GetCultureInfo(filterContext);
            Thread.CurrentThread.CurrentUICulture = GetCultureInfo(filterContext);

            var ctx = new DatabaseDataContext();
            bool hasAuthorizeAttribute = filterContext.ActionDescriptor
                .GetCustomAttributes(typeof(AuthorizeAttribute), false)
                .Any();
            if (!filterContext.IsChildAction && (!filterContext.HttpContext.Request.IsAjaxRequest() || hasAuthorizeAttribute))
            {
                GetUserProfileResult p = null;
                if (!string.IsNullOrWhiteSpace(JAAPToken))
                {
                    try
                    {
                        p = ctx.GetUserProfile(JAAPToken, HostIPAddress);
                    }
                    catch
                    {
                        var c = new HttpCookie("GameToken");
                        c.Expires = DateTime.Now.AddYears(-1);
                        Response.Cookies.Add(c);
                        return;
                    }
                    if (p != null)
                    {
                        filterContext.HttpContext.User = new GamePrincipal(new GameIdentity(p.strEmailAddress, p.autID));
                    }
                }
                else if (hasAuthorizeAttribute)
                {
                    var location = ControllerContext.RequestContext.RouteData.Values["mijnjaap"] ?? "mijn-jaap";
                    filterContext.Result = new RedirectResult("~/" + location + "/login?returnUrl=" + HttpUtility.UrlEncode(ControllerContext.RequestContext.HttpContext.Request.Url.PathAndQuery));
                }
            }
            base.OnAuthorization(filterContext);
        }
    }
}
