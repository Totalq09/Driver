using BulkiAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace BulkiAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult AddRoute()
        {
            ViewBag.Title = "Add route";

            return View();
        }

        public ActionResult ListAll()
        {
            ViewBag.Title = "List all";

            return View(new RouteController().GetRoutes());
        }

        public ActionResult DailyReport()
        {
            ViewBag.Title = "Daily report";

            return View();
        }

        public ActionResult MonthlyReport()
        {
            ViewBag.Title = "Monthly report";

            var jobj = new RouteController().MonthlyReport() as OkNegotiatedContentResult<string>;

            IEnumerable<MonthlyReportItem> items = JsonConvert.DeserializeObject<IEnumerable<MonthlyReportItem>>(jobj.Content);

            return View(items);
        }

        public ActionResult Remove(int id)
        {
            var routeObj = new RouteController().GetRoute(id);
            if (routeObj is OkNegotiatedContentResult<string>)
            {
                var r = routeObj as OkNegotiatedContentResult<string>;
                Route route = JsonConvert.DeserializeObject<Route>(r.Content);

                return PartialView("RemoveRoute", route);
            }

            return View("Error", "No such route");
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
