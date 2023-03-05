using assignment_aspwebapp.Contexts;
using assignment_aspwebapp.Models.Forms;
using assignment_aspwebapp.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace assignment_aspwebapp.Services
{
    public class AuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserService _userService;

        public AuthService(UserManager<IdentityUser> userManager, IdentityContext context, SignInManager<IdentityUser> signInManager, UserService userService)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _userService = userService;
        }

        public async Task<bool> RegisterAsync(RegisterForm form)
        {
            var identityUser = new IdentityUser 
            { 
                UserName = form.Email,
                Email = form.Email,
                PhoneNumber = form.PhoneNumber 
            };
            var identityProfile = new IdentityUserProfile
            {
                UserId = identityUser.Id,
                FirstName = form.FirstName,
                LastName = form.LastName,
                City = form.City,
                StreetName = form.StreetName,
                PostalCode = form.PostalCode,
                Company = form.Company,
            };
            if (form.PicUrl != null)
            {
                identityProfile.ImageName = await _userService.UploadProfileImageAsync(form.PicUrl);
            }

            var result = await _userManager.CreateAsync(identityUser, form.Password);
            var signInResult = await _signInManager.PasswordSignInAsync(identityUser, form.Password, false, false);
            if (result.Succeeded)
            {
                _context.UserProfiles.Add(identityProfile);
                await _context.SaveChangesAsync();
                await _userManager.AddToRoleAsync(identityUser, "User");
                return true;

            }
                
            

            return false;
        }

        public async Task<bool> LoginAsync(LoginForm form, bool keepMeLoggedIn = false)
        {
            if(form.KeepMeLoggedIn == true)
            {
                keepMeLoggedIn = true;
            }
            var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, keepMeLoggedIn, false);
            return result.Succeeded;

        }
    }
}
