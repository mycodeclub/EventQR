using Microsoft.AspNetCore.Mvc;

namespace EventQR.Areas.EventOrganizer.Controllers
{
    public class InviteTemplateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
