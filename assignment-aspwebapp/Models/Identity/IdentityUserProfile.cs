using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace assignment_aspwebapp.Models.Identity
{
    public class IdentityUserProfile
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string StreetName { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string PostalCode { get; set; } = null!;
        public string? ImageName { get; set; }
        public string? Company { get; set; }
        public IdentityUser User { get; set; } = null!;
    }
}
