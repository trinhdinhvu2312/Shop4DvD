namespace s4dServer.DTOs.Response
{
    public class AlbumResponseDTO
    {
        public int AlbumID { get; set; }
        public string? AlbumTitle { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public ArtistResponseDTO? Artist { get; set; }
    }

}
