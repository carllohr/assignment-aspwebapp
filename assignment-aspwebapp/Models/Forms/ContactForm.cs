namespace assignment_aspwebapp.Models.Forms
{
    public class ContactForm
    {
        public string? ReturnUrl { get; set; }

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Company { get; set; }
        public string Message { get; set; } = null!;
    }
}
