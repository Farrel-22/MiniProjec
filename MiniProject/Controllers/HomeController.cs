using Microsoft.AspNetCore.Mvc;

namespace MiniProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
