using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace s4dServer.Models
{
    public class PromotionProduct
    {
        [Key]
        public int PromotionProductID { get; set; }

        [ForeignKey("Promotion")]
        public int PromotionID { get; set; }

        public virtual Promotion? Promotion { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        public virtual Product? Product { get; set; }
    }
}
