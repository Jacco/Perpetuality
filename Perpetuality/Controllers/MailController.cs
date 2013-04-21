using Perpetuality.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Perpetuality.Controllers
{
    public partial class MailController : Controller
    {
        public virtual ActionResult Index(string view, string id)
        {
            var ctx = new DatabaseDataContext();
            ViewBag.DatabaseDataContext = ctx;
            return View(view, (object)id);
        }
    }
}
