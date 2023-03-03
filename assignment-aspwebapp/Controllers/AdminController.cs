using assignment_aspwebapp.Contexts;
using assignment_aspwebapp.Models.Forms;
using assignment_aspwebapp.Models.Identity;
using assignment_aspwebapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace assignment_aspwebapp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AuthService _authService;
        private readonly IdentityContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserService _userService;

        public AdminController(UserManager<IdentityUser> userManager, AuthService authService, IdentityContext context, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager, UserService userService)
        {
            _userManager = userManager;
            _authService = authService;
            _context = context;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userService = userService;
        }

        public async Task<IActionResult> Index(string ReturnUrl = null!)
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Configure(string ReturnUrl = null!)
        {
            if (await _userManager.Users.AnyAsync())
            {
                return RedirectToAction("Index", "Login");
            }

            var form = new RegisterForm { ReturnUrl = ReturnUrl ?? Url.Content("~/") };
            return View(form);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Configure(RegisterForm form)
        {
            if (ModelState.IsValid)
            {
                if (!await _roleManager.Roles.AnyAsync())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Administrator"));
                    await _roleManager.CreateAsync(new IdentityRole("Product Manager"));
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }
                var identityUser = new IdentityUser
                {
                    UserName = form.Email,
                    Email = form.Email,
                    PhoneNumber = form.PhoneNumber
                };

                var result = await _userManager.CreateAsync(identityUser, form.Password);

                if (result.Succeeded)
                {
                    var identityProfile = new IdentityUserProfile
                    {
                        UserId = identityUser.Id,
                        FirstName = form.FirstName,
                        LastName = form.LastName,
                        StreetName = form.StreetName,
                        City = form.City,
                        PostalCode = form.PostalCode,
                        Company = form.Company,
                    };

                    if (form.PicUrl != null)
                    {
                        identityProfile.ImageName = await _userService.UploadProfileImageAsync(form.PicUrl);
                    }
                    _context.UserProfiles.Add(identityProfile);
                    await _context.SaveChangesAsync();
                    await _userManager.AddToRoleAsync(identityUser, "Administrator");
                    var signInResult = await _signInManager.PasswordSignInAsync(identityUser, form.Password, false, false);

                    return LocalRedirect(form.ReturnUrl!);
                }
            }


            ModelState.AddModelError(string.Empty, "Unable to create account");
            return View(form);

        }

        public async Task<IActionResult> EditUserInfo(string id, string ReturnUrl = null!) { 
            
            var user = await _userService.GetUserByIdAsync(id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserInfo(UserAccount user)
        {
            var userAccount = await _userService.GetUserAccountAsync(user.Email);
            userAccount.Id = user.Id;

            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateUserAsync(user);
                if (result is OkResult)
                {
                    return LocalRedirect("/Admin");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error");
                }
            }
            return View(userAccount);
        }

        
        public async Task<IActionResult> ManageUserRole(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);

            var model = new List<UserRoleModel>();

            if (user != null)
            {

                foreach (var role in _roleManager.Roles.ToList())
                {
                    var userRole = new UserRoleModel
                    {
                        RoleId = role.Id,
                        RoleName = role.Name!
                    };

                    if (await _userManager.IsInRoleAsync(user!, role.Name!))
                    {
                        userRole.IsSelected = true;
                    }
                    else
                    {
                        userRole.IsSelected = false;
                    }

                    model.Add(userRole);

                }
                return View(model);
            }

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserRole(List<UserRoleModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if(user != null)
            {
                var userroles = await _userManager.GetRolesAsync(user);
                var result = await _userManager.RemoveFromRolesAsync(user, userroles);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot complete action");
                    return View(model);
                }

                result = await _userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));

                if(!result.Succeeded) 
                {
                    ModelState.AddModelError("", "Cannot complete action");
                }

                return RedirectToAction("EditUserInfo", new { id = userId });
            }

            return View(model);
        }
        public async Task<IActionResult> Delete()
        {
            return View("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {

            var result = await _userService.DeleteAsync(id);
            if (result)
            {
                return RedirectToAction("Index", "Admin");
            }

            return new BadRequestResult();
        }
    }
}
