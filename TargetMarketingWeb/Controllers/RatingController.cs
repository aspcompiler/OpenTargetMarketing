using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GeocodeSharp.Google;
using TargetMarketing.Geolocation;
using TargetMarketingWeb.Models;

namespace TargetMarketingWeb.Controllers
{
    public class RatingController : Controller
    {
        // GET: Rating
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(RatingViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                GeolocationService service = new GeolocationService();
                GeocodeResult result = await service.Geocode(model.Address);
                return View("GeocodingResult", result.Geometry.Location);
            }
            catch
            {
                return View();
            }
        }
    }
}
