namespace s4dServer.DTOs.Response
{
    public class UserResponseDTO
    {
        public int UserID { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Address { get; set; } 
        public string? PhoneNumber { get; set; }
        public ulong Status { get; internal set; }
    }

}
