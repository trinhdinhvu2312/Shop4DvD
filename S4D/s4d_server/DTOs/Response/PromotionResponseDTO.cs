namespace s4dServer.DTOs.Response
{
    public class PromotionResponseDTO
    {
        public int PromotionID { get; set; }
        public string? PromotionName { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int DiscountPercentage { get; set; }
    }

}
