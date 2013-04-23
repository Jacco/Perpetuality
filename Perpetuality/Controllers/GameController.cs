using Perpetuality.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Perpetuality.Controllers
{
    public partial class GameController : BaseController
    {
        //
        // GET: /Game/

        public virtual ActionResult Index()
        {
            //
            var ctx = new DatabaseDataContext();
            GamePrincipal user = null;
            try
            {
                user = HttpContext.User as GamePrincipal;
            }
            catch
            {
            }
            var state = ctx.GetPlayerState((user.Identity as GameIdentity).UserID, 1).Single();
            if (user != null)
            {
                // retrieve state
                ViewBag.PlayerState = new { balance = state.numBalance, rate = state.numCreditProductionRate };

                return View();
            }
            else
            {
                // retrieve state
                return View();
            }
        }

    }
}
