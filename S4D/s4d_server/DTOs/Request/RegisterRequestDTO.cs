using System.ComponentModel.DataAnnotations;

namespace s4dServer.DTOs.Request
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Username must be between 6 and 100 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 255 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Re-enter password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string RePassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
}