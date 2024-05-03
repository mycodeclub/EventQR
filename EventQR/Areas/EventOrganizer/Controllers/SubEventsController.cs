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
    public class SubEventsController : Controller
    {
        private readonly AppDbContext _context;

        public SubEventsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: EventOrganizer/SubEvents
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.SubEvents.Include(s => s.Event);
            return View(await appDbContext.ToListAsync());
        }

        // GET: EventOrganizer/SubEvents/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subEvent = await _context.SubEvents
                .Include(s => s.Event)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (subEvent == null)
            {
                return NotFound();
            }

            return View(subEvent);
        }

        // GET: EventOrganizer/SubEvents/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "UniqueId", "UniqueId");
            return View();
        }

        // POST: EventOrganizer/SubEvents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UniqueId,EventId,SubEventName,StartDateTime,EndDateTime,CreatedDate,LastUpdatedDate")] SubEvent subEvent)
        {
            if (ModelState.IsValid)
            {
                subEvent.UniqueId = Guid.NewGuid();
                _context.Add(subEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "UniqueId", "UniqueId", subEvent.EventId);
            return View(subEvent);
        }

        // GET: EventOrganizer/SubEvents/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subEvent = await _context.SubEvents.FindAsync(id);
            if (subEvent == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "UniqueId", "UniqueId", subEvent.EventId);
            return View(subEvent);
        }

        // POST: EventOrganizer/SubEvents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UniqueId,EventId,SubEventName,StartDateTime,EndDateTime,CreatedDate,LastUpdatedDate")] SubEvent subEvent)
        {
            if (id != subEvent.UniqueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubEventExists(subEvent.UniqueId))
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
            ViewData["EventId"] = new SelectList(_context.Events, "UniqueId", "UniqueId", subEvent.EventId);
            return View(subEvent);
        }

        // GET: EventOrganizer/SubEvents/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subEvent = await _context.SubEvents
                .Include(s => s.Event)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (subEvent == null)
            {
                return NotFound();
            }

            return View(subEvent);
        }

        // POST: EventOrganizer/SubEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var subEvent = await _context.SubEvents.FindAsync(id);
            if (subEvent != null)
            {
                _context.SubEvents.Remove(subEvent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubEventExists(Guid id)
        {
            return _context.SubEvents.Any(e => e.UniqueId == id);
        }
    }
}
