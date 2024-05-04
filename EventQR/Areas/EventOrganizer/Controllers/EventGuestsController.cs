using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventQR.EF;
using EventQR.Models;
using EventQR.Services;

namespace EventQR.Areas.EventOrganizer.Controllers
{
    [Area("EventOrganizer")]
    public class EventGuestsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEventOrganizer _eventService;
        private readonly Organizer _org;
        private readonly Event _thisEvent;
        public EventGuestsController(AppDbContext context, IEventOrganizer eventService)
        {
            _context = context;
            _eventService = eventService;
            _org = eventService.GetLoggedInEventOrg();
        }

        // GET: EventOrganizer/EventGuests
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Guests.Include(e => e.MyEvent);
            return View(await appDbContext.ToListAsync());
        }

        // GET: EventOrganizer/EventGuests/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventGuest = await _context.Guests
                .Include(e => e.MyEvent)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (eventGuest == null)
            {
                return NotFound();
            }

            return View(eventGuest);
        }

        // GET: EventOrganizer/EventGuests/Create
        public async Task<IActionResult> Create(Guid id)
        {
            EventGuest _guest = null;
            var currentEvent = _eventService.GetCurrentEvent();
            if (currentEvent != null)
            {
                currentEvent.SubEvents = await _context.SubEvents.Where(e => e.EventId == currentEvent.UniqueId).ToListAsync();
                _guest = await _context.Guests.Where(s => s.UniqueId.Equals(id)
                && s.EventId == currentEvent.UniqueId).FirstOrDefaultAsync();
                _guest ??= new EventGuest() { EventId = currentEvent.UniqueId };
                _guest.SubEvents = currentEvent.SubEvents;
                _guest.SubEvents.ForEach(e =>
                {
                    if (!string.IsNullOrWhiteSpace(_guest.AllowedSubEventsIdsCommaList))
                        if (_guest.AllowedSubEventsIdsCommaList.Contains(e.UniqueId.ToString()))
                            e.IsIncludedForThisGuest = true;
                });
            }
            ViewBag.currentEvent = currentEvent;
            return View(_guest);
        }

        // POST: EventOrganizer/EventGuests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventGuest eventGuest)
        {
            if (ModelState.IsValid)
            {

                var currentEvent = _eventService.GetCurrentEvent();
                var selectedIds = eventGuest.SubEvents.Where(e => e.IsIncludedForThisGuest).Select(e => e.UniqueId.ToString()).ToArray();
                if (currentEvent != null)
                {
                    if (eventGuest.UniqueId.Equals(Guid.Empty))
                    {
                        eventGuest.UniqueId = Guid.NewGuid();
                        eventGuest.EventId = currentEvent.UniqueId;
                        eventGuest.CreatedDate = DateTime.Now;
                        eventGuest.AllowedSubEventsIdsCommaList = string.Join(",", selectedIds);
                        _context.Add(eventGuest);
                        await _context.SaveChangesAsync();
                    }
                    else if (eventGuest.EventId == currentEvent.UniqueId)
                    {
                        var dbGuest = await _context.Guests.FindAsync(eventGuest.UniqueId);
                        if (dbGuest != null)
                        {
                            dbGuest.Name = eventGuest.Name;
                            dbGuest.MobileNo1 = eventGuest.MobileNo1;
                            dbGuest.MobileNo2 = eventGuest.MobileNo2;
                            dbGuest.Email = eventGuest.Email;
                            dbGuest.AllowedSubEventsIdsCommaList = string.Join(",", selectedIds);
                            dbGuest.LastUpdatedDate = DateTime.Now;
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(eventGuest);
        }

        // GET: EventOrganizer/EventGuests/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventGuest = await _context.Guests
                .Include(e => e.MyEvent)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (eventGuest == null)
            {
                return NotFound();
            }

            return View(eventGuest);
        }

        // POST: EventOrganizer/EventGuests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var eventGuest = await _context.Guests.FindAsync(id);
            if (eventGuest != null)
            {
                _context.Guests.Remove(eventGuest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventGuestExists(Guid id)
        {
            return _context.Guests.Any(e => e.UniqueId == id);
        }
    }
}
