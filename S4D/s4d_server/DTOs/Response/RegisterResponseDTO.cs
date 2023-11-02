namespace s4dServer.DTOs.Response
{
    public class RegisterResponseDTO
    {
        public string Username { get; set; }
        // Include other properties you want to return

        public RegisterResponseDTO(string username)
        {
            this.Username = username;
        }
    }
}