using assignment_aspwebapp.Models.Forms;
using assignment_aspwebapp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace assignment_aspwebapp.Controllers
{
    public class RegisterController : Controller
    {
        private readonly AuthService _authService;
        private readonly UserManager<IdentityUser> _userManager;

        public RegisterController(AuthService authService, UserManager<IdentityUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string ReturnUrl = null!)
        {
            if (!await _userManager.Users.AnyAsync()) 
            {
                return RedirectToAction("Configure", "Admin");
            }
               
            var form = new RegisterForm { ReturnUrl = ReturnUrl ?? Url.Content("/") };
            return View(form);
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterForm form)
        {
            if (form.TermsAndService != false)
            {
                if (ModelState.IsValid)
                {
                    if (await _userManager.Users.AnyAsync(x => x.Email == form.Email))
                    {
                        ModelState.AddModelError(string.Empty, "A user with input email already exists");
                        return View(form);
                    }

                    if (await _authService.RegisterAsync(form))
                        return LocalRedirect(form.ReturnUrl!);
                    else
                        return View(form);
                }
            }
            else { ModelState.AddModelError("TermsAndService", " ! * Please accept the terms and service to sign up * ! "); }

            return View(form);
            
        }
    }
}
