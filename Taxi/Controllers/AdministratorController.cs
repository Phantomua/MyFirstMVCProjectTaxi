using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using Taxi.Models;

namespace Taxi.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        private TaxiDBEntities db = new TaxiDBEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CIndex()
        {
            return View("Car/CIndex");
        }

        public ActionResult AddCar()
        {
            return View("Car/Add");
        }

        public ActionResult ViewCars()
        {
            ViewBag.Cars = db.Cars.ToList();
            return View("Car/ViewCars");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCar(Cars cars)
        {
            if (ModelState.IsValid)
            {
                db.Cars.Add(new Cars()
                {
                    Id = Guid.NewGuid(),
                    BodyNumber = cars.BodyNumber,
                    NumberPlate = cars.NumberPlate,
                    Type = cars.Type,
                    Kilometrage = cars.Kilometrage,
                    Description = cars.Description,
                    DateOfLastTS = cars.DateOfLastTS
                });
                db.SaveChanges();
            }
            return View("Car/Add");
        }

       
        public ActionResult EditCar(string id)
        {
            Guid _id = Guid.Parse(id);
            return View(db.Cars.FirstOrDefault(x => x.Id == _id));
        }

        [HttpPost]
        public ActionResult EditCar(Cars cars)
        {
            var car = db.Cars.FirstOrDefault(x => x.Id == cars.Id);
            {
                car.NumberPlate = cars.NumberPlate;
                car.Kilometrage = cars.Kilometrage;
                car.Type = cars.Type;
                car.Description = cars.Description;
                car.DateOfLastTS = cars.DateOfLastTS;
                car.BodyNumber = cars.BodyNumber;
            }
            db.SaveChanges();
            return RedirectToAction("EditCar");
        }

        public ActionResult DeleteCar(string id)
        {
            var _id = Guid.Parse(id);
            var car = db.Cars.FirstOrDefault(x => x.Id == _id);
            db.Cars.Remove(car);
            db.SaveChanges();
            return RedirectToAction("ViewCars");
        }
    }
}
