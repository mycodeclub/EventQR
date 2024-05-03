using EventQR.EF;
using EventQR.Models.Acc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;

namespace EventQR.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        //var userEmail = User.Identity.Name;
        //var dbUser = _context.Users.SingleOrDefault(u => u.Email == userEmail);
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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
                    var result = await _userManager.CreateAsync(appUser, appUser.Password);
                    if (result.Succeeded)
                    {


                        var result2 = await _userManager.AddToRoleAsync(appUser, "EventOrganizer");
                        await _signInManager.SignInAsync(appUser, isPersistent: false).ConfigureAwait(false);
                        return RedirectToAction("Index", "Home", new { Areas = "EventOrganizer" });
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
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

                    if (role.Contains("SuperAdmin"))
                        return RedirectToAction("Dashboard", "Home", new { Area = "SuperAdmin" });

                    else if (role.Contains("EventOrganizer"))
                        return RedirectToAction("Dashboard", "Home", new { Area = "EventOrganizer" });

                    else if (role.Contains("Scanner"))
                        return RedirectToAction("Dashboard", "Home", new { Area = "Scanner" });

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
                    Password = "Admin@20",
                    ConfirmPassword = "Admin@20",
                    PhoneNumber = "9999999999",
                };

                var result = await _userManager.CreateAsync(appUser, appUser.Password);
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

    }
}
