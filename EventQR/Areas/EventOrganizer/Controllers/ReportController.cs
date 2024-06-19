using EventQR.EF;
using EventQR.Models;
using EventQR.Models.Reports;
using EventQR.Services;
using EventQR.ViewModels.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data.Common;

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
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.Where(e => e.EventOrganizerId == _org.UniqueId).ToListAsync();


            return View(events);
        }

        public async Task<IActionResult> EventReport(Guid eventId)
        {
        {
            if (eventId.Equals(Guid.Empty))
            {
                var thisEvent = _eventService.GetCurrentEvent();
                eventId = thisEvent.UniqueId;
              .Select(g=> g.AllowedSubEventsIdsCommaList).ToListAsync();

            var SubEvents = await _context.SubEvents.Where(ts => ts.EventId == currentEvent.UniqueId).ToListAsync();

            var viewmodel = new ReportView 
            {
                Guests=totalGuests, SubEvents = SubEvents ,AllowedSubEventsIdsCommaList=totalAllowedGuestsIds.ToString(),
            };


            return View(viewmodel);

            }
            //-----------------------------------------------------------------------------------

            EventReportVM eventReportVM = new EventReportVM()
            {
                SubEvents = new List<SubEventVM>(),
                Guests = new List<GuestVM>()
            };

            var dbSubEvent = await _context.SubEvents.Where(s => s.EventId == eventId).ToListAsync();
            foreach (var s in dbSubEvent)
            {
                eventReportVM.SubEvents.Add(new SubEventVM()
                {
                    SubEventName = s.SubEventName,
                    SubEventId = s.UniqueId,
                    Start = s.StartDateTime.Value,
                    End = s.EndDateTime.Value,
                });
            }


            var dbGuests = await _context.Guests.Where(ts => ts.EventId == eventId).ToListAsync();

            foreach (var g in dbGuests)
            {
                var vmGuest = new GuestVM()
                {
                    GuestId = g.UniqueId,
                    Name = g.Name,
                    MySubEvents = new List<SubEventVM>() { }
                };
                if (!string.IsNullOrWhiteSpace(g.AllowedSubEventsIdsCommaList))
                {
                    var sbEvents = dbSubEvent.Where(e => g.AllowedSubEventsIdsCommaList.Split(',').Select(Guid.Parse).Contains(e.UniqueId)).ToList();
                    foreach (var se in sbEvents)
                    {
                        vmGuest.MySubEvents.Add(new SubEventVM()
                        {
                            SubEventName = se.SubEventName,
                            SubEventId = se.UniqueId,
                            End = se.EndDateTime.Value,
                            Start = se.StartDateTime.Value,
                        });
                    }
                }
                eventReportVM.Guests.Add(vmGuest);


            }
            var sz = JsonConvert.SerializeObject(eventReportVM);

            //-----------------------------------------------------------------------------------
            return View(eventReportVM);
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
        //{
        //    throw new NotImplementedException();
        //}
    }
}