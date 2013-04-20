using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Perpetuality.Controllers
{
    public class MailController : Controller
    {
        public ActionResult Index(string view, string id)
        {
            return View(view, (object)id);
        }
    }
}
