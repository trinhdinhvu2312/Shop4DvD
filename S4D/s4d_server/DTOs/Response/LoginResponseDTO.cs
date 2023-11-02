namespace s4dServer.DTOs.Response
{
    public class LoginResponseDTO
    {
        public int UserID { get; set; }
        public string? Username { get; set; }
        public string? AccessToken { get; set; }
        public string? Role { get; set; }
        public string? TokenType { get; set; } = "Bearer";
        public int ErrorCode { get; set; } = 200;
        public bool Status { get; set; } = true;
    }

}
