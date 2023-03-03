namespace assignment_aspwebapp.Models.Identity
{
    public class UserAccount
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string StreetName { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public IFormFile? PicUrl { get; set; }
        public string? ImageName { get; set; } = null!;
        public string? Company { get; set; }
    }
}
