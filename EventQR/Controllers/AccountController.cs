﻿using EventQR.EF;
using EventQR.Models;
using EventQR.Models.Acc;
using EventQR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventQR.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IEventOrganizer _eventService;

        //var userEmail = User.Identity.Name;
        //var dbUser = _context.Users.SingleOrDefault(u => u.Email == userEmail);
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context, IEventOrganizer eventOrganizerService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _eventService = eventOrganizerService;
        }


        public IActionResult Index()
        {
            ViewBag.ActiveTabId = 4;
            return View();
        }
        [HttpGet]
        public IActionResult RegisterNewOrganizer()
        {
            ViewBag.ActiveTabId = 4;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewOrganizer(AppUser appUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    appUser.UserName = appUser.Email;
                    await RegisterOrg(appUser);
                    await _signInManager.SignInAsync(appUser, isPersistent: false).ConfigureAwait(false);
                    //   return RedirectToAction("Index", "Home", new { Areas = "EventOrganizer" });
                    return RedirectToAction("Index", "Home", new { Area = "EventOrganizer" });
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return View(appUser);
        }



        [HttpGet]
        public IActionResult Login()
        {
            // ankit@bitprosoftec.com
            // Admin@20
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.LoginName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.LoginName);
                    var role = await _userManager.GetRolesAsync(user);

                    var org = _context.EventOrganizers.Where(o => o.OrganizerUserId.ToString().Equals(user.Id)).FirstOrDefaultAsync();
                    if (org != null)
                        _eventService.SetLoggedInEventOrgSession(await org);
                    if (role.Contains("SuperAdmin"))
                        return RedirectToAction("Dashboard", "Home", new { Area = "SuperAdmin" });

                    else if (role.Contains("EventOrganizer"))
                        return RedirectToAction("Dashboard", "Home", new { Area = "EventOrganizer" });

                    //optimize these query so that it hit databaseonce and fetch the details
                        //var ts = await _context.TicketScanners.Where(ts => ts.UserLoginId.ToString() == user.Id).FirstOrDefaultAsync();
                        //var thisEvent = await _context.Events.FindAsync(ts.EventId);

                            //_eventService.SetCurrentEvent(thisEvent);

                        else if (role.Contains("Scanner"))
                        {
                            var scannerWithEvent = await _context.TicketScanners
                                .Where(ts => ts.UserLoginId.ToString() == user.Id)
                                .Join(
                                    _context.Events,
                                    ts => ts.EventId,
                                    e => e.UniqueId,
                                    (ts, e) => new { TicketScanner = ts, Event = e }
                                )
                                .FirstOrDefaultAsync();

                            if (scannerWithEvent != null)
                            {
                                _eventService.SetCurrentEvent(scannerWithEvent.Event);
                                return RedirectToAction("Dashboard", "Home", new { Area = "Scanner" });
                            }
                        }
                    
                }
                else { ModelState.AddModelError("", "Invalid Email Id or Password"); }
            }
            ViewBag.ActiveTabId = 4;
            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            ViewBag.ActiveTabId = 1;
            return RedirectToAction("Index", "Home");
        }

        //[HttpGet]
        //public JsonResult GetCitiesByStateId(int Id)
        //{
        //    var states = _context.Cities.Where(c => c.StateId.Equals(Id)).OrderBy(c => c.Name);
        //    return Json(new SelectList(states, "UniqueId", "Name"));
        //}

        // ----------------------------------------------------------------------


        /// <summary>
        /// Aditional access ... 
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> CreateMasterUser()
        {
            var resultStr = string.Empty;
            try
            {
                AppUser appUser = new AppUser()
                {
                    UserName = "admin@bpst.com",
                    Email= "admin@bpst.com",
                    Password = "Admin@20",
                    ConfirmPassword = "Admin@20",
                    PhoneNumber = "9999999999",
                };

                var result = await RegisterOrg(appUser);

                if (result.Succeeded)
                {
                    var userRoles = _context.Roles.ToList();
                    foreach (var role in userRoles)
                        await _userManager.AddToRoleAsync(appUser, role.Name).ConfigureAwait(false);
                    resultStr = "Master User Created Successfully.";
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        resultStr = "Some Error: " + error.Code;
                    }
                }

            }
            catch (Exception ex)
            {
                resultStr = "Some Error: " + ex.Message;
            }
            return RedirectToAction("AutoLogin");
        }

        public async Task<IActionResult> AutoLogin()
        {
            var result = await AutoAdminLogin();
            if (result)
            {
                return RedirectToAction("Index", "Home", new { Area = "EventOrganizer" });
                return RedirectToAction("Index", "Home", new { Area = "SuperAdmin" });
                return RedirectToAction("Index", "Home", new { Area = "Scanner" });
            }
            else { return RedirectToAction("CreateMasterUser"); }
        }


        private async Task<bool> AutoAdminLogin()
        {
            var result = await _signInManager.PasswordSignInAsync("admin@bpst.com", "Admin@20", true, lockoutOnFailure: false);
            return result.Succeeded;
        }


        private async Task<IdentityResult> RegisterOrg(AppUser appUser)
        {
        //    appUser.UserName = appUser.Email;
            var result = await _userManager.CreateAsync(appUser, appUser.Password);
            if (result.Succeeded)
            {
                var organizer = new Organizer()
                {
                    Email = appUser.Email,
                    Phone1 = appUser.PhoneNumber,
                    OrganizationName = appUser.OrganizationName,
                    OrganizerUserId = Guid.Parse(appUser.Id),
                    CreatedDate = DateTime.UtcNow,
                    LogoImageName = "default.jpeg",
                    ProfileImageName = "default.jpg"
                };
                _context.EventOrganizers.Add(organizer);
                await _context.SaveChangesAsync();
                appUser.OrganizerUniqueIdFk = organizer.UniqueId;
                await _context.SaveChangesAsync();
                // Common.Static.Active.SetOrganizerForUserId(organizer, organizer.OrganizerUserId);
                var result2 = await _userManager.AddToRoleAsync(appUser, "EventOrganizer");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return result;
        }

    }
}
