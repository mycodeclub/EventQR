using Microsoft.AspNetCore.Mvc;

namespace EventQR.Areas.EventOrganizer.Controllers
{
    public class TicketTemplateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
