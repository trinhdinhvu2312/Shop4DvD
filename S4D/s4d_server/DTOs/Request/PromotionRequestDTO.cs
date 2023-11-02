﻿namespace s4dServer.DTOs.Request
{
    public class PromotionRequestDTO
    {
        public int PromotionId { get; set; }
        public string? PromotionName { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

}
