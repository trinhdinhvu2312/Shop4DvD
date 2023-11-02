using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace s4dServer.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public string? ProductName { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [ForeignKey("Album")]
        public int? AlbumID { get; set; }

        public string? ProviderName { get; set; }

        public int Duration { get; set; }

        public string? Image { get; set; }

        public virtual Album? Album { get; set; }

        public virtual Category? Category { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        [IgnoreDataMember]
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        [IgnoreDataMember]
        public virtual ICollection<PromotionProduct> PromotionProducts { get; set; } = new List<PromotionProduct>();
    }
}
