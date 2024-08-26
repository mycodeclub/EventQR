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
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Net;
using IronPdf.Engines.Chrome;
using Microsoft.Extensions.Logging;
using System.Xml;

namespace EventQR.Areas.EventOrganizer.Controllers
{
    [Area("EventOrganizer")]
    [Authorize(Roles = "EventOrganizer")]
    public class HostDetailsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEventOrganizer _eventService;
        private readonly Organizer _org;
        public HostDetailsController(AppDbContext context, IEventOrganizer eventService)
        {
            _context = context;
            _eventService = eventService;
            _org = eventService.GetLoggedInEventOrg();
        }
        public async Task<IActionResult> Index()
        {
            var thisEvent = _eventService.GetCurrentEvent();
            if (thisEvent != null)
            {


                var sEvent = await _context.HostDetails.Where(ts => ts.EventId == thisEvent.UniqueId).SingleOrDefaultAsync();
                return View(sEvent);
            }
            else
            {
                return RedirectToAction("Index", "HostDetails");
            }
        }

    }
}
