using Microsoft.AspNetCore.Mvc;

namespace EvergreenView.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
