using EventQR.EF;
using EventQR.Models;
using EventQR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventQR.Areas.EventOrganizer.Controllers
{
    [Authorize(Roles = "EventOrganizer")]
    [Area("EventOrganizer")]
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEventOrganizer _eventService;
        private readonly Organizer _org;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(AppDbContext context, IEventOrganizer eventService, IWebHostEnvironment environment)
        {
            _context = context;
            _eventService = eventService;
            _org = eventService.GetLoggedInEventOrg();
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            return View(_org);
        }
        [HttpGet]
        public IActionResult MyProfile()
        {
            ViewBag.LogoImageName = _org.LogoImageName;
            return View(_org);
        }

        // GET: Organizer/Merchants/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            ViewBag.LogoImageName = _org.LogoImageName;
            return View(_org);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventQR.Models.Organizer orgVm)
        {
            ViewBag.LogoImageName = _org.LogoImageName;
            if (ModelState.IsValid)
            {
                try
                {
                    var orgDb = await _context.EventOrganizers.FindAsync(orgVm.UniqueId);
                    if (orgDb != null)
                    {
                        if (await SaveProfilePic(orgVm))
                            orgDb.ProfileImageName = orgVm.ProfileImageName;
                        if (await SaveLogo(orgVm))
                            orgDb.LogoImageName = orgVm.LogoImageName;
                        orgDb.LastUpdatedDate = DateTime.UtcNow;
                        orgDb.Name = orgVm.Name;
                        orgDb.OrganizationName = orgVm.OrganizationName;
                        orgDb.OfficeAddress = orgVm.OfficeAddress;
                        orgDb.Phone1 = orgVm.Phone1;
                        orgDb.Phone2 = orgVm.Phone2;
                        orgDb.Website = orgVm.Website;

                         var result = await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var msg = ex.Message;
                    if (!EventExists(orgVm.UniqueId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyProfile));
            }
            //  ViewData["OrganizerUserId"] = new SelectList(_context.Users, "Id", "Id", @event.OrganizerUserId);
            return View(orgVm);
        }

        private bool EventExists(Guid id)
        {
            return (_context.EventOrganizers?.Any(e => e.UniqueId == id)).GetValueOrDefault();
        }
        private async Task<bool> SaveProfilePic(Organizer organizer)
        {
            var path = String.Empty;
            var res = false;
            try
            {
                if (organizer.ProfileImage != null && organizer.ProfileImage.Length > 0)
                {
                    path = _environment.WebRootPath + Common.Static.Variables.OrgProfilePicsPath.Replace("/", "\\");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    organizer.ProfileImageName = organizer.ProfileImage.FileName.Replace(" ", "").Replace("/", "").Replace("-", "").Replace("_", "");
                    await organizer.ProfileImage.CopyToAsync(new FileStream(path + "//" + organizer.ProfileImageName, FileMode.Create));
                    return true;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return res;
        }
        private async Task<bool> SaveLogo(Organizer organizer)
        {
            var restult = false;
            try
            {
                if (organizer.LogoImage != null && organizer.LogoImage.Length > 0)
                {
                    var path = String.Empty;
                    path = _environment.WebRootPath + Common.Static.Variables.OrgLogoPath.Replace("/", "\\");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    organizer.LogoImageName = organizer.LogoImage.FileName.Replace(" ", "").Replace("/", "").Replace("-", "").Replace("_", "");
                    await organizer.LogoImage.CopyToAsync(new FileStream(path + "//" + organizer.LogoImageName, FileMode.Create));
                    return true;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return restult;
        }
    }
}
