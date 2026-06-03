namespace GYM.Mi.Areas.Admin.Models
{
    public class UserRoleViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
