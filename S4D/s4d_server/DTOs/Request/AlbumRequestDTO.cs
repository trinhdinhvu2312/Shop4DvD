namespace s4dServer.DTOs.Request
{
    public class AlbumRequestDTO
    {
        public int AlbumId { get; set; }
        public string? AlbumTitle { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int ArtistID { get; set; }
    }

}
