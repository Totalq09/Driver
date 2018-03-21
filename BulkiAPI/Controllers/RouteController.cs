using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using BulkiAPI.Contexts;
using BulkiAPI.Models;
using Newtonsoft.Json;

namespace BulkiAPI.Controllers
{
    public class RouteController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Routes
        public IQueryable<Route> GetRoutes()
        {
            return db.Routes.OrderByDescending(r => r.Date);
        }

        // GET: api/Routes/5
        [ResponseType(typeof(Route))]
        public IHttpActionResult GetRoute(int id)
        {
            Route route = db.Routes.Find(id);
            if (route == null)
            {
                return NotFound();
            }

            return Ok(JsonConvert.SerializeObject(route));
        }

        // POST: api/Routes
        [ResponseType(typeof(Route))]
        [HttpPost]
        [Route("transits")]
        public IHttpActionResult AddRoute(Route route)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            route.Distance = 100;
            db.Routes.Add(route);
            db.SaveChanges();

            var response = Request.CreateResponse(HttpStatusCode.OK);

            string fullyQualifiedUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            //response.Headers.Location = new Uri(fullyQualifiedUrl);

            return Redirect(fullyQualifiedUrl);
        }

        // POST: api/Routes
        [ResponseType(typeof(Route))]
        [HttpPost]
        [Route("modify")]
        public IHttpActionResult Modify(Route currentRoute, Route updatedRoute)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           
            return Ok();
        }

        [ResponseType(typeof(Route))]
        [HttpGet]
        [Route("reports/daily")]
        public IHttpActionResult GetDailyRaport()
        {
            DateTime startTime;
            DateTime endTime;

            try
            {
                var getParams = this.Request.GetQueryNameValuePairs().ToDictionary(kv => kv.Key, kv => kv.Value,
                            StringComparer.OrdinalIgnoreCase);
                string format = "yyyy-MM-dd";

                startTime = DateTime.ParseExact(getParams["start_date"], format, CultureInfo.InvariantCulture);
                endTime = DateTime.ParseExact(getParams["end_date"], format, CultureInfo.InvariantCulture);

                return Ok(GetDailyReport(startTime, endTime));
            }
            catch (Exception)
            {
                return BadRequest("There are no routes fulfilling the criteria");
            } 
        }

        DailyReport GetDailyReport(DateTime startTime, DateTime endTime)
        {
            Route[] routes = db.Routes.Where(r => DateTime.Compare(r.Date, startTime) >= 0 && DateTime.Compare(r.Date, endTime) <= 0).ToArray();

            double distance = 0f;
            decimal price = 0m;

            foreach (var route in routes)
            {
                distance += route.Distance;
                price += route.Price;
            }

            DailyReport report = new DailyReport()
            {
                total_distance = System.Math.Round(distance,2),
                total_price = System.Math.Round(price,2),
                RoutesNumber = routes.Length
            };

            return report;
        }

        [ResponseType(typeof(Route))]
        [HttpGet]
        [Route("reports/monthly")]
        public IHttpActionResult MonthlyReport()
        {
            try
            {
                DateTime time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime today = DateTime.Now;

                List<MonthlyReportItem> report = new List<MonthlyReportItem>();

                MonthlyReportItem latest = new MonthlyReportItem()
                {
                    total_distance = 0f,
                    total_price = 0m,
                    avg_distance = 0f,
                    avg_price = 0m
                };

                IQueryable<Route> routes = db.Routes.Where(r => DateTime.Compare(r.Date, time) >= 0 && DateTime.Compare(r.Date, today) <= 0);

                int i = 0;

                while (DateTime.Compare(time,today) <= 0)
                {
                    DailyReport dailyReport = GetDailyReport(time, time);

                    MonthlyReportItem item = new MonthlyReportItem()
                    {
                        date = ConvertDate(time),
                        total_distance = System.Math.Round(latest.total_distance + dailyReport.total_distance, 2),
                        total_price = latest.total_price + dailyReport.total_price,
                        avg_distance = System.Math.Round((i * latest.avg_distance + dailyReport.total_distance) / (i + 1)),
                        avg_price = System.Math.Round((i * latest.avg_price + dailyReport.total_price) / (i + 1), 2)
                    };

                    report.Add(item);

                    i++;

                    latest.total_distance = item.total_distance;
                    latest.total_price = item.total_price;
                    latest.avg_distance = item.avg_distance;
                    latest.avg_price = item.avg_price;

                    time = time.AddDays(1);
                }

                return Ok(JsonConvert.SerializeObject(report));
            }
            catch(Exception)
            {
                return BadRequest("Internal error - cannot create monthlyReport");
            }
        }

        string ConvertDate(DateTime time)
        {
            string m;

            switch(time.Month)
            {
                case 1:
                    m = "January";
                    break;
                case 2:
                    m = "February";
                    break;
                case 3:
                    m = "March";
                    break;
                case 4:
                    m = "April";
                    break;
                case 5:
                    m = "May";
                    break;
                case 6:
                    m = "June";
                    break;
                case 7:
                    m = "July";
                    break;
                case 8:
                    m = "August";
                    break;
                case 9:
                    m = "September";
                    break;
                case 10:
                    m = "October";
                    break;
                case 11:
                    m = "November";
                    break;
                case 12:
                    m = "December";
                    break;
                default:
                    m = "";
                    break;
            }

            string ending;

            switch(time.Day)
            {
                case 1:
                    ending = "st";
                    break;
                case 2:
                    ending = "nd";
                    break;
                case 3:
                    ending = "rd";
                    break;
                default:
                    ending = "th";
                    break;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(m);
            sb.Append(", ");
            sb.Append(time.Day);
            sb.Append(ending);

            return sb.ToString();
        }

        // DELETE: api/Routes/5
        [ResponseType(typeof(Route))]
        public IHttpActionResult DeleteRoute(int id)
        {
            Route route = db.Routes.Find(id);
            if (route == null)
            {
                return NotFound();
            }

            db.Routes.Remove(route);
            db.SaveChanges();

            return Ok(route);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RouteExists(int id)
        {
            return db.Routes.Count(e => e.Id == id) > 0;
        }
    }
}