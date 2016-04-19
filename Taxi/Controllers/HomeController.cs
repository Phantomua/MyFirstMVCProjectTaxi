using System.Web.Mvc;

namespace Taxi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Измените этот шаблон, чтобы быстро приступить к работе над приложением ASP.NET MVC.";
            return View();
        }
    }
}
