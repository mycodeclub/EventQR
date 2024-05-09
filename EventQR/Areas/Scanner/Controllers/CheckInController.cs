﻿using EventQR.EF;
using EventQR.Models;
using EventQR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EventQR.Areas.Scanner.Controllers
{
    [Area("Scanner")]
 //   [Authorize(Roles = "Scanner")]
    public class CheckInController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEventOrganizer _eventService;

        public CheckInController(AppDbContext context, IEventOrganizer eventService)
        {
            _context = context;
            _eventService = eventService;
        }

        public async Task<IActionResult> AllowGuestNew(string id)
        {
            var DecryptString = EventQR.Common.Static.Variables.Decrypt(id);
            Guid guestId = Guid.Empty;
            Guid eventId = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(DecryptString))
            {
                var ids = DecryptString.Split(",");
                Guid.TryParse(ids[0], out guestId);
                Guid.TryParse(ids[1], out eventId);
            }
            var guest = await _context.Guests.Include(g => g.MyEvent).Where(g => g.UniqueId == guestId && g.EventId == eventId).FirstOrDefaultAsync();
            if (string.IsNullOrWhiteSpace(guest.AllowedSubEventsIdsCommaList))
            {
                var allowedSubEvents = guest.AllowedSubEventsIdsCommaList.Split(',').Select(Guid.Parse);
                guest.SubEvents = _context.SubEvents.Where(e => allowedSubEvents.Contains(e.UniqueId)).ToList();
            }
            GuestCheckIn _checkin = new GuestCheckIn()
            {
                UserLoginId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                GuestId = guestId,
                Guest = guest,
                EventId = eventId,
                Event = guest.MyEvent,
                CheckIn = DateTime.Now
            };
            await _context.CheckIns.AddAsync(_checkin);
            await _context.SaveChangesAsync();
            return View(_checkin);
        }
        public async Task<IActionResult> AllowGuestOld(Guid guestId, Guid eventId)
        {
            var guest = await _context.Guests.Include(g => g.MyEvent).Where(g => g.UniqueId == guestId && g.EventId == eventId).FirstOrDefaultAsync();
            if (!string.IsNullOrWhiteSpace(guest.AllowedSubEventsIdsCommaList))
            {
                var allowedSubEvents = guest.AllowedSubEventsIdsCommaList.Split(',').Select(Guid.Parse);
                guest.SubEvents = _context.SubEvents.Where(e => allowedSubEvents.Contains(e.UniqueId)).ToList();
            }
            GuestCheckIn _checkin = new GuestCheckIn()
            {
                UserLoginId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                GuestId = guestId,
                Guest = guest,
                EventId = eventId,
                Event = guest.MyEvent,
                //   CheckIn = DateTime.Now
            };
            await _context.CheckIns.AddAsync(_checkin);
            await _context.SaveChangesAsync();
            return View(_checkin);
        }


        public async Task<IActionResult> AllowGuestForSubEvent(Guid guestId, Guid eventId, Guid subEventId)
        {
            var guest = await _context.Guests.Where(g => g.UniqueId == guestId && g.EventId == eventId).FirstOrDefaultAsync();
            var _scannerLoginId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var allowedSubEvents = guest.AllowedSubEventsIdsCommaList.Split(',');

            // --- 
            GuestCheckIn _checkin = new GuestCheckIn()
            {
                UserLoginId = _scannerLoginId,
                GuestId = guestId,
                EventId = eventId,
                CheckIn = DateTime.Now
            };

            await _context.CheckIns.AddAsync(_checkin);
            await _context.SaveChangesAsync();
           // var check= await _context.CheckIns.FindAsync(GuestId, EventId);
            return View();
        }

        /*public async Task<IActionResult> AllowGuest(string id)
        {
            var DecryptString = EventQR.Common.Static.Variables.Decrypt(id);
            Guid guestId = Guid.Empty;
            Guid eventId = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(DecryptString))
            {
                var ids = DecryptString.Split(",");
                Guid.TryParse(ids[0], out guestId);
                Guid.TryParse(ids[1], out eventId);
            }
            var allowedSubEvents = guest.AllowedSubEventsIdsCommaList.Split(',');
            GuestCheckIn _checkin = new GuestCheckIn()
            {
                UserLoginId = loggedInUserId,
                GuestId = guestId,
                Guest = guest,
                EventId = eventId,
                Event = guest.MyEvent,
                CheckIn = DateTime.Now
            };


            await _context.CheckIns.AddAsync(_checkin);
            await _context.SaveChangesAsync();
            //var check= await _context.CheckIns.FindAsync(GuestId, EventId);
            return View();
        }*/
        public async Task<IActionResult> GuestList()
        {
            var thisEvent = _eventService.GetCurrentEvent();
            if (thisEvent != null)
            {
                var list = await _context.Guests.Where(g => g.EventId == thisEvent.UniqueId).ToListAsync();
                return View(list);
            }
            return View();
           // return View("guest");
        } 
        public IActionResult EventDetails()
        {
             var thisEvent = _eventService.GetCurrentEvent();
            return View(thisEvent);
        }

    }
}
