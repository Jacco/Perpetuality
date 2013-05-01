using DotNetOpenAuth.AspNet;
using JaapNL.Utilities;
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
            return View();
        }

        [HttpPost]
        public virtual ActionResult ResetPassword(string emailAddress)
        {
            try
            {
                emailAddress = emailAddress.Trim();
                try
                {
                    emailAddress = NormalizeEmailAddress(emailAddress);
                }
                catch
                {
                    throw new ApplicationException("60040 The supplied username is not a valid email address.");
                }
                long? userID = null;
                using (var ctx = new DatabaseDataContext())
                {
                    // find out language
                    userID = ctx.GetUserIDByEmail(emailAddress.ToLower());
                }
                if (!userID.HasValue)
                {
                    throw new ApplicationException("60041 User not found.");
                }

                try
                {
                    var password = Perpetuality.Utilities.ReadablePassword.GenerateReadablePassword();

                    // mail the new password
                    var client = new WebClient();
                    client.Encoding = Encoding.UTF8;
                    var body = client.DownloadString(ConfigurationManager.AppSettings["BaseURL"] + "/en/mail/?view=PasswordRequest&id=" + userID + "," + HttpUtility.UrlEncode(password));
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
                    
                    // update the database with the new password
                    using (var ctx = new DatabaseDataContext())
                    {
                        ctx.ChangeUserPasswordInternal(userID, password, true);
                   } 
                }
                catch (Exception e)
                {
                    throw new ApplicationException("60043 Password retrieval failed.", e);
                }
            }
            catch (Exception e)
            {
                EventLogger.WriteEvent(e.Message, EventLogger.EventType.Error, "Perpetuality");
            }

            return View();
        }

        public virtual ActionResult Confirm(string reference)
        {
            var ctx = new DatabaseDataContext();
            ctx.ConfirmEmailAddress(reference);
            return View();
        }

        private static void SendMail(string recipient, string name, string subject, string body)
        {
            using (var mailClient = new SmtpClient())
            {
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                using (var lMsg = new System.Net.Mail.MailMessage())
                {

                    lMsg.From = new MailAddress("noreply@perpetuality.org", "Perpetuality.org");
                    lMsg.ReplyTo = new MailAddress("noreply@perpetuality.org", "Perpetuality.org");
                    lMsg.Sender = new MailAddress("noreply+" + recipient.Replace("@", "=") + "@perpetuality.org");
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

        public virtual ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Register(string emailAddress, string password, string language)
        {
            try
            {
                long? userID = null;
                var userName = emailAddress.Trim();
                password = password.Trim();
                // validate the email address
                try
                {
                    userName = NormalizeEmailAddress(userName);
                }
                catch
                {
                    throw new ApplicationException("60001 The supplied username is not a valid email address.");
                }
                // validate the password
                if (string.IsNullOrEmpty(password) | password.Length < 6)
                {
                    throw new ApplicationException("60002 The supplied password is empty or too short.");
                }
                // store in DB
                var confirmationpwd = GenerateConfirmationHash(userName);
                using (var ctx = new DatabaseDataContext())
                {
                    if (ctx.RegisterNewUser(userName, password, confirmationpwd, false, ref userID) == 0)
                        throw new ApplicationException("60003 Registering new user failed.");
                    // send a confirmation mail
                    try
                    {
                        SendConfirmationMail(new MailAddress(userName), userID, language);
                    }
                    catch (Exception e)
                    {
                        throw new ApplicationException("60005 Sending confirmation mail failed.", e);
                    }
                }
                if (!userID.HasValue)
                    throw new ApplicationException("60005 Sending confirmation mail failed.");
                //return userID.Value;
            }
            catch (Exception e)
            {
                EventLogger.WriteEvent(e.Message, EventLogger.EventType.Error, "Perpetuality");
            }

            return View(Views.RegisterThanks);
        }

        private void SendConfirmationMail(MailAddress mailAddress, long? userID, string language)
        {
            var client = new WebClient();
            var body = client.DownloadString(ConfigurationManager.AppSettings["BaseURL"] + "/" + language + "/mail/?view=EmailConfirmation&id=" + userID);
            SendMail(mailAddress.Address, mailAddress.DisplayName, Resources.Mail.EmailConfirmation.Subject, body);
        }

        private static string GenerateConfirmationHash(string email)
        {
            if (!string.IsNullOrEmpty(email))
                return Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(email + ConfigurationManager.AppSettings["ConfirmationSecret"])));
            else
                throw new ApplicationException("Error generating confirmation hash.");
        }

        private static string NormalizeEmailAddress(string email)
        {
            var test = new MailAddress(email);
            return test.User + "@" + test.Host.ToLower();
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

        public virtual ActionResult ExternalLoginFailure()
        {
            ViewBag.ExceptionTitle = "Inloggen via extern account niet gelukt";
            ViewBag.ExceptionDescription = "Het is niet gelukt om in te loggen via deze externe account. U kunt proberen direct in te loggen via een Mijn JAAP-account.";
            return View(MVC.Shared.Views.Error);
        }

    }
}
