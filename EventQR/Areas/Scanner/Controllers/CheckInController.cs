using EventQR.EF;
using EventQR.Models;
using EventQR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventQR.Areas.Scanner.Controllers
{
    [Area("Scanner")]
    [Authorize(Roles = "Scanner")]
    public class CheckInController : Controller
    {
        private readonly AppDbContext _context;
        public CheckInController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> AllowGuest(Guid guestId, Guid eventId)
        {

            var guest = await _context.Guests.Where(g => g.UniqueId == guestId && g.EventId == eventId).FirstOrDefaultAsync();
            var allowedSubEvents = guest.AllowedSubEventsIdsCommaList.Split(',');
            GuestCheckIn _checkin = new GuestCheckIn()
            {
                GuestId = guestId,
                EventId = eventId,
                CheckIn = DateTime.Now
            };

            await _context.CheckIns.AddAsync(_checkin);
            await _context.SaveChangesAsync();
            //  var check= await _context.CheckIns.FindAsync(GuestId, EventId);
            return View();
        }


        public async Task<IActionResult> AllowGuest(string id)
        {
            // decript id



            Guid guestId = Guid.Empty;
            Guid eventId = Guid.Empty;

            var guest = await _context.Guests.Where(g => g.UniqueId == guestId && g.EventId == eventId).FirstOrDefaultAsync();
            var allowedSubEvents = guest.AllowedSubEventsIdsCommaList.Split(',');
            GuestCheckIn _checkin = new GuestCheckIn()
            {
                GuestId = guestId,
                EventId = eventId,
                CheckIn = DateTime.Now
            };

            await _context.CheckIns.AddAsync(_checkin);
            await _context.SaveChangesAsync();
            //  var check= await _context.CheckIns.FindAsync(GuestId, EventId);
            return View();
        }
    }
}
