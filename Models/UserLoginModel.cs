using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, ErrorMessage = "Username must be less than 100 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, ErrorMessage = "Password must be less than 50 characters.")]
        public string Password { get; set; }
    }
}
