namespace s4dServer.DTOs.Request
{
    public class UserRequestDTO
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public byte Status { get; set; }
        public string? Address { get; set; } 
        public string? PhoneNumber { get; set; }
    }

}
