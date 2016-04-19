using System;
using System.Linq;
using System.Web.Mvc;
using Taxi.Models;

namespace Taxi.Controllers
{
    [Authorize(Roles = "Driver")]
    public class DriverController : Controller
    {
        //
        // GET: /Driver/
        private TaxiDBEntities db = new TaxiDBEntities();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetDriverLoc()
        {
            var drv = db.Drivers.FirstOrDefault(x => x.Employees.CallSign == User.Identity.Name);
            return Json(drv, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeStatuses()
        {
            var drv = db.Drivers.FirstOrDefault(x => x.Employees.CallSign == User.Identity.Name);
            var ord = db.Order.Where(x => x.Drivers.Id == drv.Id && x.Statuses.Name == "In progress");
            var ordFinished = db.Statuses.FirstOrDefault(x => x.Name == "Finished");
            if (db.Order.Any(x => x.Drivers.Id == drv.Id && x.Statuses.Name == "In progress"))
                ord.FirstOrDefault(x=>x.IdDriver==drv.Id).IdStatus = ordFinished.Id;
            drv.Busy = false;
            //drv.Location = loc;
            db.SaveChanges();
            return View("Index");
        }

        public ActionResult GetLocation(string loc)
        {
            var driver = db.Drivers.FirstOrDefault(x => x.Employees.CallSign == User.Identity.Name);
            driver.Location = loc;
            db.SaveChanges();
            return View("Index");
        }

        public JsonResult GetAjax()
        {
            db= new TaxiDBEntities();
            var driver = db.Drivers.FirstOrDefault(x => x.Employees.CallSign == User.Identity.Name);
            if (driver == null)
                return null;
            if (db.Order.Any(x => x.Drivers.Id == driver.Id && x.Statuses.Name == "In progress"))
                return
                    Json(
                        db.Order.Where(x => x.Drivers.Id == driver.Id && x.Statuses.Name == "In progress")
                            .Select(x => new
                            {
                                x.Id,
                                x.AdressFrom,
                                x.AdressWhere,
                                x.ClientsPhone,
                                x.Description,
                                x.Cost,
                                driver.Busy
                            }), JsonRequestBehavior.AllowGet);
            return null;
        }
    }
}
