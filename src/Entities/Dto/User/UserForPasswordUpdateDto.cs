using System.ComponentModel.DataAnnotations;

namespace Entities.Dto.User
{
    public class UserForPasswordUpdateDto
    {
        [Required(ErrorMessage = "Password is required")]
        [StringLength(60, MinimumLength = 8, ErrorMessage = "Password can't be less than 8 characters")]
        public string Password { get; set; }
    }
}
