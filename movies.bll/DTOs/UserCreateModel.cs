using System.ComponentModel.DataAnnotations;

namespace movies_BLL.DTOs
{
    public class UserCreateModel
    {
        [Required]
        [StringLength(256)]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
