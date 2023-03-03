using assignment_aspwebapp.Models.Forms;
using assignment_aspwebapp.Services;
using Microsoft.AspNetCore.Mvc;

namespace assignment_aspwebapp.Controllers
{
    public class LoginController : Controller
    {
        private AuthService _authService;

        public LoginController(AuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Index(string ReturnUrl = null!)
        {
            var form = new LoginForm { ReturnUrl = ReturnUrl ?? Url.Content("~/") };
            return View(form);
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(LoginForm form)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.LoginAsync(form);
                if (result == true) 
                {
                    return LocalRedirect(form.ReturnUrl!);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Unable to login");
                }
            }

            return View(form);
            
        }
    }
}
