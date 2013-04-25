// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace Perpetuality.Controllers
{
    public partial class HomeController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public HomeController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected HomeController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Login()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Login);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Confirm()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Confirm);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ExternalLogin()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ExternalLogin);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ExternalLoginCallback()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ExternalLoginCallback);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public HomeController Actions { get { return MVC.Home; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Home";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Home";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string Login = "Login";
            public readonly string Logout = "Logout";
            public readonly string ResetPassword = "ResetPassword";
            public readonly string Confirm = "Confirm";
            public readonly string Register = "Register";
            public readonly string Profile = "Profile";
            public readonly string ExternalLogin = "ExternalLogin";
            public readonly string ExternalLoginCallback = "ExternalLoginCallback";
            public readonly string ExternalLoginFailure = "ExternalLoginFailure";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string Login = "Login";
            public const string Logout = "Logout";
            public const string ResetPassword = "ResetPassword";
            public const string Confirm = "Confirm";
            public const string Register = "Register";
            public const string Profile = "Profile";
            public const string ExternalLogin = "ExternalLogin";
            public const string ExternalLoginCallback = "ExternalLoginCallback";
            public const string ExternalLoginFailure = "ExternalLoginFailure";
        }


        static readonly ActionParamsClass_Index s_params_Index = new ActionParamsClass_Index();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Index IndexParams { get { return s_params_Index; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Index
        {
            public readonly string language = "language";
        }
        static readonly ActionParamsClass_Login s_params_Login = new ActionParamsClass_Login();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Login LoginParams { get { return s_params_Login; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Login
        {
            public readonly string language = "language";
            public readonly string emailAddress = "emailAddress";
            public readonly string password = "password";
            public readonly string rememberMe = "rememberMe";
        }
        static readonly ActionParamsClass_Confirm s_params_Confirm = new ActionParamsClass_Confirm();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Confirm ConfirmParams { get { return s_params_Confirm; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Confirm
        {
            public readonly string reference = "reference";
        }
        static readonly ActionParamsClass_ResetPassword s_params_ResetPassword = new ActionParamsClass_ResetPassword();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ResetPassword ResetPasswordParams { get { return s_params_ResetPassword; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ResetPassword
        {
            public readonly string emailAddress = "emailAddress";
        }
        static readonly ActionParamsClass_Register s_params_Register = new ActionParamsClass_Register();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Register RegisterParams { get { return s_params_Register; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Register
        {
            public readonly string emailAddress = "emailAddress";
            public readonly string password = "password";
            public readonly string language = "language";
        }
        static readonly ActionParamsClass_Profile s_params_Profile = new ActionParamsClass_Profile();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Profile ProfileParams { get { return s_params_Profile; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Profile
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_ExternalLogin s_params_ExternalLogin = new ActionParamsClass_ExternalLogin();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ExternalLogin ExternalLoginParams { get { return s_params_ExternalLogin; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ExternalLogin
        {
            public readonly string provider = "provider";
            public readonly string returnUrl = "returnUrl";
        }
        static readonly ActionParamsClass_ExternalLoginCallback s_params_ExternalLoginCallback = new ActionParamsClass_ExternalLoginCallback();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ExternalLoginCallback ExternalLoginCallbackParams { get { return s_params_ExternalLoginCallback; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ExternalLoginCallback
        {
            public readonly string returnUrl = "returnUrl";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string ChangeEmail = "ChangeEmail";
                public readonly string ChangePassword = "ChangePassword";
                public readonly string Confirm = "Confirm";
                public readonly string Index = "Index";
                public readonly string Login = "Login";
                public readonly string Profile = "Profile";
                public readonly string Register = "Register";
                public readonly string RegisterThanks = "RegisterThanks";
                public readonly string ResetPassword = "ResetPassword";
            }
            public readonly string ChangeEmail = "~/Views/Home/ChangeEmail.cshtml";
            public readonly string ChangePassword = "~/Views/Home/ChangePassword.cshtml";
            public readonly string Confirm = "~/Views/Home/Confirm.cshtml";
            public readonly string Index = "~/Views/Home/Index.cshtml";
            public readonly string Login = "~/Views/Home/Login.cshtml";
            public readonly string Profile = "~/Views/Home/Profile.cshtml";
            public readonly string Register = "~/Views/Home/Register.cshtml";
            public readonly string RegisterThanks = "~/Views/Home/RegisterThanks.cshtml";
            public readonly string ResetPassword = "~/Views/Home/ResetPassword.cshtml";
            static readonly _SharedClass s_Shared = new _SharedClass();
            public _SharedClass Shared { get { return s_Shared; } }
            [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
            public partial class _SharedClass
            {
                static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
                public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
                public class _ViewNamesClass
                {
                    public readonly string ExternalLoginCallback = "ExternalLoginCallback";
                }
                public readonly string ExternalLoginCallback = "~/Views/Home/Shared/ExternalLoginCallback.cshtml";
            }
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_HomeController : Perpetuality.Controllers.HomeController
    {
        public T4MVC_HomeController() : base(Dummy.Instance) { }

        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string language);

        public override System.Web.Mvc.ActionResult Index(string language)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "language", language);
            IndexOverride(callInfo, language);
            return callInfo;
        }

        partial void LoginOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string language);

        public override System.Web.Mvc.ActionResult Login(string language)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Login);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "language", language);
            LoginOverride(callInfo, language);
            return callInfo;
        }

        partial void LoginOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string language, string emailAddress, string password, bool rememberMe);

        public override System.Web.Mvc.ActionResult Login(string language, string emailAddress, string password, bool rememberMe)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Login);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "language", language);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "emailAddress", emailAddress);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "password", password);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "rememberMe", rememberMe);
            LoginOverride(callInfo, language, emailAddress, password, rememberMe);
            return callInfo;
        }

        partial void LogoutOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult Logout()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Logout);
            LogoutOverride(callInfo);
            return callInfo;
        }

        partial void ResetPasswordOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult ResetPassword()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ResetPassword);
            ResetPasswordOverride(callInfo);
            return callInfo;
        }

        partial void ConfirmOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string reference);

        public override System.Web.Mvc.ActionResult Confirm(string reference)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Confirm);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "reference", reference);
            ConfirmOverride(callInfo, reference);
            return callInfo;
        }

        partial void ResetPasswordOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string emailAddress);

        public override System.Web.Mvc.ActionResult ResetPassword(string emailAddress)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ResetPassword);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "emailAddress", emailAddress);
            ResetPasswordOverride(callInfo, emailAddress);
            return callInfo;
        }

        partial void RegisterOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult Register()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Register);
            RegisterOverride(callInfo);
            return callInfo;
        }

        partial void RegisterOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string emailAddress, string password, string language);

        public override System.Web.Mvc.ActionResult Register(string emailAddress, string password, string language)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Register);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "emailAddress", emailAddress);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "password", password);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "language", language);
            RegisterOverride(callInfo, emailAddress, password, language);
            return callInfo;
        }

        partial void ProfileOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult Profile()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Profile);
            ProfileOverride(callInfo);
            return callInfo;
        }

        partial void ProfileOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Perpetuality.Models.Profile model);

        public override System.Web.Mvc.ActionResult Profile(Perpetuality.Models.Profile model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Profile);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            ProfileOverride(callInfo, model);
            return callInfo;
        }

        partial void ExternalLoginOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string provider, string returnUrl);

        public override System.Web.Mvc.ActionResult ExternalLogin(string provider, string returnUrl)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ExternalLogin);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "provider", provider);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "returnUrl", returnUrl);
            ExternalLoginOverride(callInfo, provider, returnUrl);
            return callInfo;
        }

        partial void ExternalLoginCallbackOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string returnUrl);

        public override System.Web.Mvc.ActionResult ExternalLoginCallback(string returnUrl)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ExternalLoginCallback);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "returnUrl", returnUrl);
            ExternalLoginCallbackOverride(callInfo, returnUrl);
            return callInfo;
        }

        partial void ExternalLoginFailureOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult ExternalLoginFailure()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ExternalLoginFailure);
            ExternalLoginFailureOverride(callInfo);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
