using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventQR.EF;
using EventQR.Models;

namespace EventQR.Areas.EventOrganizer.Controllers
{
    [Area("EventOrganizer")]
    public class EventGuestsController : Controller
    {
        private readonly AppDbContext _context;

        public EventGuestsController(AppDbContext context)
        {
            _context = context;
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
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "UniqueId", "UniqueId");
            return View();
        }

        // POST: EventOrganizer/EventGuests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UniqueId,EventId,Name,Address,MobileNo1,MobileNo2,Email,GuestCount,IsInviteAccepted,InviteAcceptedOn,IsInviteSent,InviteSentOn,QrCodeImageUri,CreatedDate,LastUpdatedDate")] EventGuest eventGuest)
        {
            if (ModelState.IsValid)
            {
                eventGuest.UniqueId = Guid.NewGuid();
                _context.Add(eventGuest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "UniqueId", "UniqueId", eventGuest.EventId);
            return View(eventGuest);
        }

        // GET: EventOrganizer/EventGuests/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventGuest = await _context.Guests.FindAsync(id);
            if (eventGuest == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "UniqueId", "UniqueId", eventGuest.EventId);
            return View(eventGuest);
        }

        // POST: EventOrganizer/EventGuests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UniqueId,EventId,Name,Address,MobileNo1,MobileNo2,Email,GuestCount,IsInviteAccepted,InviteAcceptedOn,IsInviteSent,InviteSentOn,QrCodeImageUri,CreatedDate,LastUpdatedDate")] EventGuest eventGuest)
        {
            if (id != eventGuest.UniqueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventGuest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventGuestExists(eventGuest.UniqueId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "UniqueId", "UniqueId", eventGuest.EventId);
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
