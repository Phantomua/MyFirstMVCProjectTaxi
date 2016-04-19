using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Taxi.Models;

namespace Taxi.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                Employees user;
                using (TaxiDBEntities db = new TaxiDBEntities())
                {
                    user = db.Employees.FirstOrDefault(u => u.CallSign == model.CallSign && u.Password == model.Password);
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.CallSign, true);
                        //FormsAuthentication.RenewTicketIfOld(new FormsAuthenticationTicket(1, model.CallSign,
                        //    DateTime.Now,
                        //    DateTime.Now.AddSeconds(15), true, user.RoleTable.Name, FormsAuthentication.FormsCookiePath));
                        
                        if (user.RoleTable.Name == "Dispetcher")
                        {
                            return RedirectToAction("Index", "Dispetcher");
                        }
                        if (user.RoleTable.Name == "Driver")
                        {
                            return RedirectToAction("Index", "Driver");
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
            }
            return View(model);
        }
        
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ErrorResult()
        {
            return View("Error");
        }
    }
}
