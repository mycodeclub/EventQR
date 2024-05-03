using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventQR.EF;
using EventQR.Models;
using Microsoft.AspNetCore.Authorization;
using EventQR.Services;

namespace EventQR.Areas.EventOrganizer.Controllers
{

    [Authorize(Roles = "EventOrganizer")]
    [Area("EventOrganizer")]
    public class EventsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEventOrganizer _eventService;

        public EventsController(AppDbContext context, IEventOrganizer eventService)
        {
            _context = context;
            _eventService = eventService;
        }

        // GET: EventOrganizer/Events
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Events;
            return View(await appDbContext.ToListAsync());
        }

        // GET: EventOrganizer/Events/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: EventOrganizer/Events/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: EventOrganizer/Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.UniqueId = Guid.NewGuid();
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventOrganizerId"] = new SelectList(_context.EventOrganizers, "UniqueId", "OrganizationName", @event.EventOrganizerId);
            return View(@event);
        }


        // GET: EventOrganizer/Events/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: EventOrganizer/Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(Guid id)
        {
            return _context.Events.Any(e => e.UniqueId == id);
        }
    }
}
