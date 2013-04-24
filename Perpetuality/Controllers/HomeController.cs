using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using Perpetuality.Data;
using Perpetuality.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
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
                token = ctx.LoginUser(emailAddress, password, HostIPAddress);

                if (!string.IsNullOrWhiteSpace(token))
                {
                    var tokenCookie = new HttpCookie("GameToken", token);
                    tokenCookie.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(tokenCookie);
                }
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
            return RedirectToAction(Index());
        }

        public virtual ActionResult Logout()
        {
            var ctx = new DatabaseDataContext();
            try
            {
                ctx.LogoutUser(JAAPToken, HostIPAddress);
            }
            catch
            {
                // ignore the exception: 60030 Supplied token could not be converted to a guid.
            }
            // FormsAuthentication.SignOut();
            if (Response.Cookies != null)
            {
                var c = new HttpCookie("GameToken");
                c.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Response.SetCookie(c);
            }
            return RedirectToAction(Index());
        }

        public virtual ActionResult ResetPassword()
        {
            var ctx = new DatabaseDataContext();



            return View();
        }

        private static void SendMail(string recipient, string name, string subject, string body)
        {
            using (var mailClient = new SmtpClient())
            {
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                using (var lMsg = new System.Net.Mail.MailMessage())
                {

                    lMsg.From = new MailAddress("woning@jaap.nl", "JAAP.NL");
                    lMsg.ReplyTo = new MailAddress("woning@jaap.nl", "JAAP.NL");
                    lMsg.Sender = new MailAddress("woning+" + recipient.Replace("@", "=") + "@jaap.nl");
                    lMsg.To.Add(new MailAddress(recipient, name));
                    lMsg.Subject = subject;
                    lMsg.Body = "";

                    if (!String.IsNullOrEmpty(body))
                    {
                        Byte[] lBytes = System.Text.Encoding.UTF8.GetBytes(body);
                        MemoryStream lHtmlStream = new MemoryStream(lBytes);
                        AlternateView lHtmlView = new AlternateView(lHtmlStream, MediaTypeNames.Text.Html);
                        lMsg.AlternateViews.Add(lHtmlView);
                    }
                    mailClient.Send(lMsg);
                }
            }
        }

        [HttpPost]
        public virtual ActionResult ResetPassword(string emailAddress)
        {
            // find out language
            var ctx = new DatabaseDataContext();
            var id = ctx.GetUserIDByEmail(emailAddress.ToLower());

            var client = new WebClient();
            client.Encoding = Encoding.UTF8;
            var body = client.DownloadString(Request.Url.Host + "/en/mail/?view=EmailConfirmation&id=" + id.ToString());
            var subject = client.ResponseHeaders["X-JaapMail-Subject"];
            var recipient = client.ResponseHeaders["X-JaapMail-Recipient-Email"];
            var name = client.ResponseHeaders["X-JaapMail-Recipient-Name"];
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Replace("<", "");
                name = name.Replace(">", "");
            }
            var error = client.ResponseHeaders["X-JaapMail-Error"];
            SendMail(recipient, name, subject, body);

            return View();
        }

        public virtual ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Register(string emailAddress)
        {



            return View();
        }

        public virtual ActionResult Profile()
        {
            var ctx = new DatabaseDataContext();
            var profile = ctx.GetUserProfile(JAAPToken , HostIPAddress);
            return View(new Profile(profile));
        }

        [HttpPost]
        public virtual ActionResult Profile(Profile model)
        {
            var ctx = new DatabaseDataContext();
            ctx.UpdateUserProfile(JAAPToken, HostIPAddress, model.Name, model.Language);
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public virtual ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, ConfigurationManager.AppSettings["BaseURL"] + Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
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
