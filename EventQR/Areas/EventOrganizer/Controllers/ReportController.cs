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
        [HttpPost]
        public IActionResult TicketShow(string ticketName)
        {
            string imageUrl = string.Empty;

            if (ticketName == "ShowMyTicket")
            {
                imageUrl = Url.Content("~/eventqrimages/tickets/t1.png");
            }
            else if (ticketName == "ShowMyTicket1")
            {
                imageUrl = Url.Content("~/eventqrimages/tickets/t2.png");
            }  
            else if (ticketName == "ShowMyTicket2")
            {
                imageUrl = Url.Content("~/eventqrimages/tickets/t3.png");
            } 
            else if (ticketName == "ShowMyTicket3")
            {
                imageUrl = Url.Content("~/eventqrimages/tickets/t4.png");
            }
            else if (ticketName == "ShowMyTicket4")
            {
                imageUrl = Url.Content("~/eventqrimages/tickets/t.png");
            }
            else
            {
                imageUrl = Url.Content(""); 
            }

            return Json(new { success = true, message = "Ticket processed successfully!", ticketName, imageUrl });
        }
        private readonly Dictionary<string, string> _ticketImages = new()
        {
            { "ShowMyTicket", "~/eventqrimages/tickets/t1.png" },
            { "ShowMyTicket1", "~/eventqrimages/tickets/t2.png" },
            { "ShowMyTicket2", "~/eventqrimages/tickets/t3.png" },
            { "ShowMyTicket3", "~/eventqrimages/tickets/t4.png" },
            { "ShowMyTicket4", "~/eventqrimages/tickets/t.png" }
        };


        private IActionResult View(List<Event> events, List<SubEvent> subevent)
        {
            throw new NotImplementedException();
        }
    }
}
