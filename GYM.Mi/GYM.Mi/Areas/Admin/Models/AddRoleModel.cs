using System.ComponentModel.DataAnnotations;

namespace GYM.Mi.Areas.Admin.Models
{
    public class AddRoleModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }
    }
}
