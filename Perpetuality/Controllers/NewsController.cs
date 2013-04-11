using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Perpetuality.Controllers
{
    public partial class NewsController : Controller
    {
        //
        // GET: /News/

        public virtual ActionResult Index()
        {
            return View();
        }

    }
}
