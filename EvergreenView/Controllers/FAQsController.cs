using Microsoft.AspNetCore.Mvc;

namespace EvergreenView.Controllers
{
    public class FAQsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}