using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using Perpetuality.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Perpetuality.Controllers
{
    public partial class HomeController : BaseController
    {
        //
        // GET: /Home/

        public virtual ActionResult Index(string language)
        {
            return View();
        }

        public virtual ActionResult Login(string language)
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Login(string language, string emailAddress, string password, bool rememberMe = false)
        {
            var ctx = new DatabaseDataContext();
            var token = "";
            var message = "";
            try
            {
                ctx.LoginUser(emailAddress, password, HostIPAddress);
            }
            catch (Exception e)
            {
                switch (e.ErrorCode())
                {
                    case 60020: 
                        message = Resources.Home.Login.LoginError_60020;
                        break;
                    case 60022:
                        message = Resources.Home.Login.LoginError_60022;
                        break;
                }
            }
            return View();
        }

        public virtual ActionResult ResetPassword()
        {
            var ctx = new DatabaseDataContext();



            return View();
        }

        [HttpPost]
        public virtual ActionResult ResetPassword(string emailAddress)
        {

            MailMessage mail = new MailMessage("jacco@jaap.nl", "martijn@jaap.nl");
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "stmp.google.com";
            mail.Subject = "this is a test email.";
            mail.Body = "this is my test email body";
            client.Send(mail);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public virtual ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, "http://localhost:51127" + Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        [AllowAnonymous]
        public virtual ActionResult ExternalLoginCallback(string returnUrl)
        {
            var ctx = new DatabaseDataContext();
            var bypass = ctx.GetSetting("ByPassPrefix");
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }
            var email = result.ExtraData["username"].ToLower();
            long id = -1;
            try
            {
                id = ctx.GetUserIDByEmail(email);
            }
            catch
            {

            }

            if (id == -1)
            {
                // new user should not get a confirmation mail
                try
                {
                    id = ctx.RegisterNewUser(
                        email,
                        Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(new Guid().ToString()))),
                        false,
                        returnUrl
                    );
                }
                catch
                {
                }
            }

            string token = "";
            if (id != -1)
            {
                try
                {
                    token = ctx.LoginUser(bypass + "_" + email, "notneeded", MvcApplication.HostIPAddress(HttpContext));
                }
                catch (Exception exception)
                {
                    switch (exception.ErrorCode())
                    {
                        case 60021:
                            ctx.ConfirmEmailAddress(Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(email))));
                            break;
                    }
                }
                // second try
                if (string.IsNullOrWhiteSpace(token))
                {
                    try
                    {
                        token = ctx.LoginUser(bypass + "_" + email, "notneeded", MvcApplication.HostIPAddress(HttpContext));
                    }
                    catch
                    {
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(token))
            {
                var tokenCookie = new HttpCookie("GameToken", token);
                tokenCookie.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(tokenCookie);
                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    if (returnUrl.Contains("{guid}"))
                    {
                        if (returnUrl.Contains("?"))
                            returnUrl = returnUrl.Replace("{guid}", "&guid=" + token);
                        else
                            returnUrl = returnUrl.Replace("{guid}", "?guid=" + token);
                    }
                }
                else
                {
                    returnUrl = Url.Action(MVC.Home.Index());
                }
                ViewBag.ReturnUrl = returnUrl;
                return View(MVC.Home.Views.Shared.ExternalLoginCallback);
            }

            return RedirectToAction("ExternalLoginFailure");
        }

    }
}
