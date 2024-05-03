using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventQR.Areas.Scanner.Controllers
{
    [Area("Scanner")]
    [Authorize(Roles = "Scanner")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
