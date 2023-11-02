using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace s4dServer.Models
{
    public class Promotion
    {
        [Key]
        public int PromotionID { get; set; }

        [Required]
        public string? PromotionName { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int DiscountPercentage { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
