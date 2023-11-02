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

        public decimal DiscountPercentage { get; internal set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();

        [IgnoreDataMember]
        public virtual ICollection<PromotionProduct> PromotionProducts { get; set; } = new List<PromotionProduct>();
       
    }
}
