using EventQR.EF;
using EventQR.Models;
using EventQR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventQR.Areas.Scanner.Controllers
{
    [Area("Scanner")]
    [Authorize(Roles = "Scanner")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEventOrganizer _eventService;
        private readonly IQrCodeGenerator _qrService;
        private readonly Organizer _org;
        private readonly Event _thisEvent;
        public HomeController(AppDbContext context, IEventOrganizer eventService, IQrCodeGenerator qrService)
        {
            _context = context;
            _eventService = eventService;
            _org = eventService.GetLoggedInEventOrg();
            _qrService = qrService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Dashboard()
        {

            return View();
        }
    }
}
