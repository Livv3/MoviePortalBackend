using System.ComponentModel.DataAnnotations;

namespace movies_BLL.DTOs
{
    public class UserUpdateModel
    {
        [Required]
        [StringLength(256)]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string? Password { get; set; }
    }

}
