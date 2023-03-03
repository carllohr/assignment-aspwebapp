using assignment_aspwebapp.Models.Identity;
using assignment_aspwebapp.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace assignment_aspwebapp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserService userService, SignInManager<IdentityUser> signInManager)
        {
            _userService = userService;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(string ReturnUrl = null!)
        {
            var userAccount = await _userService.GetUserAccountAsync(User.Identity!.Name!);
            return View(userAccount);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserAccount user)
        {
            var userAccount = await _userService.GetUserAccountAsync(User.Identity!.Name!);
            userAccount.Id = user.Id;

            if(ModelState.IsValid)
            {
                var result = await _userService.UpdateUserAsync(user);
                if(result is OkResult)
                {
                    return LocalRedirect("/Account");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error");
                }
            }
            return View(userAccount);
        }

        public async Task<IActionResult> LogOut()
        {
            if (_signInManager.IsSignedIn(User))
            {
                await _signInManager.SignOutAsync();
            }
            return LocalRedirect("~/");
        }
       
    }
}
