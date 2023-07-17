using Microsoft.AspNetCore.Mvc;

namespace BootCamp.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
