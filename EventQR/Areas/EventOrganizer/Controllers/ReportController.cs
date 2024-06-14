using EventQR.EF;
using EventQR.Models;
using EventQR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventQR.Areas.EventOrganizer.Controllers
{
    [Area("EventOrganizer")]
    [Authorize(Roles = "EventOrganizer")]
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEventOrganizer _eventService;

        private readonly Organizer _org;

        public ReportController(AppDbContext context, IEventOrganizer eventService)
        {
            _context = context;
            _eventService = eventService;

            _org = eventService.GetLoggedInEventOrg();

        }
        public  async Task< IActionResult> Index()
        {
            var events = await _context.Events.Where(e => e.EventOrganizerId == _org.UniqueId).ToListAsync();


            return View(events);
        }
        public async Task<IActionResult> GuestReport()
        {
            return View();
        }
        private IActionResult View(List<Event> events, List<SubEvent> subevent)
        {
            throw new NotImplementedException();
        }
    }
}
