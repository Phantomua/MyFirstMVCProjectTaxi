using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using Taxi.Models;

namespace Taxi.Controllers
{
    [Authorize(Roles = "Dispetcher")]
    public class DispetcherController : Controller
    {
        TaxiDBEntities db = new TaxiDBEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Order()
        {
            using (TaxiDBEntities db = new TaxiDBEntities())
            {
                ObservableCollection<Tariffs> tarifsCollection = new ObservableCollection<Tariffs>();
                foreach (var i in db.Tariffs)
                {
                    tarifsCollection.Add(i);
                }
                ViewBag.ASD = tarifsCollection;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Order(Order order)
        {
            using (TaxiDBEntities db = new TaxiDBEntities())
            {
                ObservableCollection<Tariffs> tarifsCollection = new ObservableCollection<Tariffs>();
                foreach (var i in db.Tariffs)
                {
                    tarifsCollection.Add(i);
                }
                ViewBag.ASD = tarifsCollection;
                var driver = db.Drivers.FirstOrDefault(x => x.Id == order.IdDriver);
                if (driver != null)
                {
                    var tar = db.Tariffs.FirstOrDefault(x => x.Id == order.IdTariff);
                    db.Order.Add(new Order()
                    {
                        Id = Guid.NewGuid(),
                        AdressFrom = order.AdressFrom,
                        AdressWhere = order.AdressWhere,
                        ClientsPhone = order.ClientsPhone,
                        DateTime = DateTime.Now,
                        IdDispetcher = db.Dispetchers.FirstOrDefault(x => x.Employees.CallSign == User.Identity.Name).Id,
                        IdDriver = driver.Id,
                        IdTariff = order.IdTariff,
                        Kilometrage = order.Kilometrage,
                        Description = order.Description,
                        IdStatus = db.Statuses.FirstOrDefault(x => x.Name.Contains("In progress")).Id,
                        Cost = (order.Kilometrage/1000)*Convert.ToInt32(tar.Cost) + tar.InitialCost
                    });
                    driver.Busy = true;
                    db.SaveChanges();
                }
                else
                {
                    return View("Error");
                }
            }
            return View(order);
        }

        public ActionResult ViewOrders()
        {
            ObservableCollection<Order> orderCollection = new ObservableCollection<Order>();
            foreach (var i in db.Order)
            {
                orderCollection.Add(i);
            }
            ViewBag.Order = orderCollection;
            return View();
        }

        [HttpPost]
        public ActionResult ViewOrders(Order order)
        {
            return View(order);
        }

        public JsonResult GetAjax()
        {
            using (TaxiDBEntities db = new TaxiDBEntities())
            {
                ObservableCollection<Drivers> driverCollection = new ObservableCollection<Drivers>();
                foreach (var i in db.Drivers)
                {
                    if (!i.Busy)
                        driverCollection.Add(i);
                }
                return Json(driverCollection.Select(x => new
                {
                    location = x.Location,
                    id = x.Id
                }), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditOrder(string id)
        {
            TaxiDBEntities db = new TaxiDBEntities();
            ObservableCollection<Tariffs> tarifsCollection = new ObservableCollection<Tariffs>();
            foreach (var i in db.Tariffs)
            {
                tarifsCollection.Add(i);
            }
            ViewBag.ASD = tarifsCollection;
            ObservableCollection<Statuses> statusesCollection = new ObservableCollection<Statuses>();
            foreach (var i in db.Statuses)
            {
                statusesCollection.Add(i);
            }
            ViewBag.Statuses = statusesCollection;
            Guid ID = Guid.Parse(id);
            return View(db.Order.FirstOrDefault(x=>x.Id==ID));
        }

        [HttpPost]
        public ActionResult EditOrder(Order order)
        {
            TaxiDBEntities db = new TaxiDBEntities();
            var ord = db.Order.FirstOrDefault(x => x.Id == order.Id);
            {
                ord.AdressFrom = order.AdressFrom;
                ord.AdressWhere = order.AdressWhere;
                ord.ClientsPhone = order.ClientsPhone;
                ord.IdDispetcher = db.Dispetchers.FirstOrDefault(x => x.Employees.CallSign == User.Identity.Name).Id;
                ord.IdDriver =
                    db.Drivers.FirstOrDefault(x => x.Employees.CallSign == order.Drivers.Employees.CallSign).Id;
                ord.IdTariff = order.IdTariff;
                ord.Kilometrage = order.Kilometrage;
                ord.Description = order.Description;
                ord.IdStatus = order.IdStatus;
                ord.Cost = order.Cost;
            }
            db.SaveChanges();
            return RedirectToAction("ViewOrders");
        }

        public ActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEmployee(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Employees user;
                    user =
                        db.Employees.FirstOrDefault(u => u.CallSign == model.CallSign);
                if (user == null)
                {
                        try
                        {
                            db.Employees.Add(new Employees()
                            {
                                Id = Guid.NewGuid(),
                                CallSign = model.CallSign,
                                Password = model.Password,
                                Name = model.Name,
                                LastName = model.LastName,
                                MiddleName = model.MiddleName,
                                Adress = model.Adress,
                                DateOfBorn = Convert.ToDateTime(model.DateOfBorn),
                                DateOfHiring = DateTime.Today,
                                PhoneNumber = model.PhoneNumber,
                                Sex = model.Sex,
                                IdRole = db.RoleTable.FirstOrDefault(x => x.DisplayName == model.Role).Id
                            });
                            db.SaveChanges();
                            using (TaxiDBEntities _db = new TaxiDBEntities())
                            {

                                if (model.Role == "Диспетчер")
                                    _db.Dispetchers.Add(new Dispetchers()
                                    {
                                        Id = Guid.NewGuid(),
                                        IdEmployee = _db.Employees.FirstOrDefault(x => x.CallSign == model.CallSign).Id
                                    });
                                if (model.Role == "Водій")
                                {
                                    _db.Drivers.Add(new Drivers()
                                    {
                                        Id = Guid.NewGuid(),
                                        IdEmployee = _db.Employees.FirstOrDefault(x => x.CallSign == model.CallSign).Id,
                                        Busy = false,
                                        Location = "",
                                        DateOfHiring = DateTime.Now,
                                        DriverLicenseId = model.DriverLicenseId
                                    });
                                }
                                _db.SaveChanges();
                            }
                        }
                        catch (Exception)
                        {
                            return RedirectToAction("ErrorResult", "Account");
                        }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }
            return View(model);
        }

        public ActionResult NewShift()
        {
            ObservableCollection<Cars> carsCollection = new ObservableCollection<Cars>();
            foreach (var i in db.Cars)
            {
                if (i.Shifts.All(x => x.Date.Date != DateTime.Today))
                carsCollection.Add(i);
            }
            ViewBag.cars = carsCollection;
            ObservableCollection<string> driversCollection = new ObservableCollection<string>();
            foreach (var i in db.Drivers)
            {
                if (i.Shifts.All(x => x.Date.Date != DateTime.Today))
                    driversCollection.Add(i.Employees.CallSign);
            }
            ViewBag.Drivers = driversCollection;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewShift(Shifts shift)
        {
            if (ModelState.IsValid)
            {
                db.Shifts.Add(new Shifts()
                {
                    Id = Guid.NewGuid(),
                    IdCar = shift.IdCar,
                    IdDriver = db.Drivers.FirstOrDefault(x=>x.Id==shift.IdDriver).Id,
                    Date = DateTime.Now
                });
            }
            return View(shift);
        }

        public ActionResult ViewShifts()
        {
            ObservableCollection<Shifts> shiftsCollection = new ObservableCollection<Shifts>();
            foreach (var i in db.Shifts)
            {
                shiftsCollection.Add(i);
            }
            ViewBag.Shifts = shiftsCollection;
            return View();
        }

        public ActionResult AddCar()
        {
            return View();
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
            return View();
        }

        public ActionResult ViewCars()
        {
            ViewBag.Cars = db.Cars.ToList();
            return View();
        }

        public ActionResult EditCar(string id)
        {
            Guid ID = Guid.Parse(id);
            return View(db.Cars.FirstOrDefault(x=>x.Id==ID));
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
            return RedirectToAction("ViewCars");
        }
    }
}
