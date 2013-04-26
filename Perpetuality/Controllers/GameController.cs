using Perpetuality.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            decimal? balance = null;
            decimal? creditProductionRate = null;
            DateTime? gameDate = null;
            decimal? installedPower = null;
            if (user != null)
            {
                ctx.GetPlayerState((user.Identity as GameIdentity).UserID, 1, ref balance, ref creditProductionRate, ref gameDate, ref installedPower);
                // retrieve state
                ViewBag.PlayerState = new { balance = balance.Value, rate = creditProductionRate.Value, date = (gameDate.Value - new DateTime(1970, 1, 1)).TotalMilliseconds, power = installedPower.Value };

                return View();
            }
            else
            {
                ViewBag.PlayerState = new { balance = 3000000, rate = 0, date = new DateTime(2013, 4, 20), power = 0 };
                // retrieve state
                return View();
            }
        }

        private double GetSolarPower(double longitude, double latitude)
        {
            var client = new WebClient();
            var response = client.DownloadString("http://api.perpetuality.org/v1/" + longitude.ToString() + "," + latitude.ToString());
            var dummyObject = new { @long = 0.0, lat = 0.0, solar_power = 0.0 };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(response, dummyObject);
            return 550.0 * (obj.solar_power / 255.0) * 8.765813;
        }

        [Authorize]
        public virtual JsonResult InstallPlant(double longitude, double latitude, long plantTypeID, int size)
        {
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            var ctx = new DatabaseDataContext();
            GamePrincipal user = null;
            try
            {
                user = HttpContext.User as GamePrincipal;
            }
            catch
            {
            }

            decimal? balance = null;
            decimal? creditProductionRate = null;
            DateTime? gameDate = null;
            decimal? installedPower = null;
            decimal? buildingCost = null;
            decimal? buildingPower = null;
            decimal? buildingRevenue = null;
            if (user != null)
            {
                var power = GetSolarPower(longitude, latitude);
                // call install plant
                ctx.InstallPlant(
                    (user.Identity as GameIdentity).UserID
                    , 1
                    , 1
                    , (decimal)longitude
                    , (decimal)latitude
                    , size
                    , (decimal)power
                    , false
                    , ref balance
                    , ref creditProductionRate
                    , ref gameDate
                    , ref installedPower
                    , ref buildingCost
                    , ref buildingPower
                    , ref buildingRevenue);

                result.Data = new { 
                    balance = balance.Value, 
                    rate = creditProductionRate.Value, 
                    date = gameDate.Value, power = installedPower.Value 
                };
            }
            else
            {
                // error
            }
            return result;
        }

        [Authorize]
        public virtual JsonResult CalculatePlant(double longitude, double latitude, long plantTypeID, int size)
        {
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            var ctx = new DatabaseDataContext();
            GamePrincipal user = null;
            try
            {
                user = HttpContext.User as GamePrincipal;
            }
            catch
            {
            }

            decimal? balance = null;
            decimal? creditProductionRate = null;
            DateTime? gameDate = null;
            decimal? installedPower = null;
            decimal? buildingCost = null;
            decimal? buildingPower = null;
            decimal? buildingRevenue = null;
            if (user != null)
            {
                var power = GetSolarPower(longitude, latitude);
                // call calculate plant
                ctx.InstallPlant(
                    (user.Identity as GameIdentity).UserID
                    , 1
                    , 1
                    , (decimal)longitude
                    , (decimal)latitude
                    , size
                    , (decimal)power
                    , true
                    , ref balance
                    , ref creditProductionRate
                    , ref gameDate
                    , ref installedPower
                    , ref buildingCost
                    , ref buildingPower
                    , ref buildingRevenue);

                result.Data = new { 
                      balance = balance.Value
                    , rate = creditProductionRate.Value
                    , date = gameDate.Value
                    , power = installedPower.Value
                    , plant = new {  
                            cost = buildingCost.Value
                        ,   power = buildingPower.Value
                        ,   revenue = buildingRevenue.Value
                        ,   sunpower = power
                    } 
                };
            }
            else
            {
                // error
            }
            return result;
        }

        // http://localhost:51127/en/Game/GetPowerPlants/?world=1&minlon=0&maxlon=20&minlat=50&maxlat=60

        [Authorize]
        public virtual JsonResult GetPowerPlants(long world, double minlon, double maxlon, double minlat, double maxlat)
        {
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            var ctx = new DatabaseDataContext();
            GamePrincipal user = null;
            try
            {
                user = HttpContext.User as GamePrincipal;
            }
            catch
            {
            }

            var plants = ctx.GetWorldPlayerPlants((user.Identity as GameIdentity).UserID, world, (decimal)minlon, (decimal)maxlon, (decimal)minlat, (decimal)maxlat);
            result.Data = plants.Select(x => new { lon = x.numLongitude, lat = x.numLatitude, tp = x.intPowerPlantTypeID, id = x.autID }).ToList();
            return result;
        }
    }
}
