using assignment_aspwebapp.Contexts;
using assignment_aspwebapp.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace assignment_aspwebapp.Services
{
    public class UserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityContext _context;
        private readonly IWebHostEnvironment _web;

        public UserService(UserManager<IdentityUser> userManager, IdentityContext context, IWebHostEnvironment web)
        {
            _userManager = userManager;
            _context = context;
            _web = web;
        }

        public async Task<string> UploadProfileImageAsync (IFormFile profileImage)
        {
            var profilePath = $"{_web.WebRootPath}/Images/profiles";
            var imageName = $"profile_{Guid.NewGuid()}{Path.GetExtension(profileImage.FileName)}";
            string filePath = $"{profilePath}/{imageName}";

            using var fs = new FileStream(filePath, FileMode.Create);
            await profileImage.CopyToAsync(fs);

            return imageName;
        }
       
        public async Task<IActionResult> UpdateUserAsync(UserAccount user)
        {
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == user.Id);
            var identityUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (identityUser != null) 
            {
                identityUser.PhoneNumber = user.PhoneNumber;

                if(userProfile != null)
                {
                    userProfile.FirstName = user.FirstName;
                    userProfile.LastName = user.LastName;
                    userProfile.StreetName = user.StreetName;
                    userProfile.City = user.City;
                    userProfile.PostalCode = user.PostalCode;
                    userProfile.Company = user.Company;

                    if(user.PicUrl!= null)
                    {
                        userProfile.ImageName = await UploadProfileImageAsync(user.PicUrl);
                    }
                    
                     _context.Entry(userProfile).State = EntityState.Modified;
                     await _context.SaveChangesAsync();

                     return new OkResult();
                    }
                }

            return new NotFoundResult();
        }
        
        public async Task<UserAccount> GetUserByIdAsync(string id)
        {
            var identityUser = await _userManager.FindByIdAsync(id);
            if (identityUser != null)
            {
                var identityProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == identityUser.Id);
                if (identityProfile != null)
                {
                    return new UserAccount
                    {
                        Id = identityUser.Id,
                        UserName = identityUser.Email!,
                        FirstName = identityProfile.FirstName,
                        LastName = identityProfile.LastName,
                        Email = identityUser.Email!,
                        PhoneNumber = identityUser.PhoneNumber,
                        StreetName = identityProfile.StreetName,
                        City = identityProfile.City,
                        PostalCode = identityProfile.PostalCode,
                        Company = identityProfile.Company,
                        ImageName = identityProfile.ImageName,
                    };
                }
            }
            return null!;

        }
        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var identityUser = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(identityUser!);
                
                return true;
            } catch { return false; }
        }
        public async Task<UserAccount> GetUserAccountAsync(string userName)
        {
            var identityUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (identityUser != null) 
            {
                var identityProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == identityUser.Id);
                if (identityProfile != null)
                {
                    return new UserAccount
                    {
                        Id = identityUser.Id,
                        UserName = identityUser.Email!,
                        FirstName = identityProfile.FirstName,
                        LastName = identityProfile.LastName,
                        Email = identityUser.Email!,
                        PhoneNumber = identityUser.PhoneNumber,
                        StreetName = identityProfile.StreetName,
                        City = identityProfile.City,
                        PostalCode = identityProfile.PostalCode,
                        Company = identityProfile.Company,
                        ImageName = identityProfile.ImageName,
                    };
                }
            }
            return null!;
        }
        
    }
}
