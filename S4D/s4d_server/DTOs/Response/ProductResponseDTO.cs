namespace s4dServer.DTOs.Response
{
    public class ProductResponseDTO
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public CategoryResponseDTO? Category { get; set; }
        public decimal Price { get; set; }
        public AlbumResponseDTO? Album { get; set; }
        public string? ProviderName { get; set; }
        public int Duration { get; set; }
        public string? Image { get; set; }
        public PromotionResponseDTO? Promotion { get; set; }
    }

}
