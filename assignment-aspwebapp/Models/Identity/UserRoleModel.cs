namespace assignment_aspwebapp.Models.Identity
{
    public class UserRoleModel
    {
        public string RoleId { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public bool IsSelected { get; set; }
    }
}
