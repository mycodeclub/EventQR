using EventQR.EF;
using EventQR.Models;
using EventQR.Models.Reports;
using EventQR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Xml;

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
            var currentEvent = _eventService.GetCurrentEvent();
            var totalGuests = await _context.Guests.Where(ts => ts.EventId == currentEvent.UniqueId).ToListAsync();

            var totalAllowedGuestsIds = await _context.Guests.Where(ts => ts.EventId == currentEvent.UniqueId)
              .Select(g=> g.AllowedSubEventsIdsCommaList).ToListAsync();

            var SubEvents = await _context.SubEvents.Where(ts => ts.EventId == currentEvent.UniqueId).ToListAsync();

            var viewmodel = new ReportView 
            {
                Guests=totalGuests, SubEvents = SubEvents ,AllowedSubEventsIdsCommaList=totalAllowedGuestsIds.ToString(),
            };


            return View(viewmodel);

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
       
        //private IActionResult View(List<Event> events, List<SubEvent> subevent)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
