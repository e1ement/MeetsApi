using System.ComponentModel.DataAnnotations;

namespace Entities.Dto.Auth
{
    public class UserForRegisterDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(60, ErrorMessage = "Username can't be longer than 60 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(60, MinimumLength = 8, ErrorMessage = "Password can't be less than 8 characters")]
        public string Password { get; set; }
    }
}
