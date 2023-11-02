namespace s4dServer.DTOs.Request
{
    public class ProductRequestDTO
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int CategoryID { get; set; }
        public decimal Price { get; set; }
        public int AlbumID { get; set; }
        public string? ProviderName { get; set; }
        public int Duration { get; set; }
        public string? Image { get; set; }
    }

}
