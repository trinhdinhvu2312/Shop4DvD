using System.ComponentModel.DataAnnotations;

namespace s4dServer.DTOs.Request
{
    public class LoginRequestDTO
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }

}
