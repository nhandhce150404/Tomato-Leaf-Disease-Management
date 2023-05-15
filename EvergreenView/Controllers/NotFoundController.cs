using Microsoft.AspNetCore.Mvc;

namespace EvergreenView.Controllers
{
    public class NotFoundController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}