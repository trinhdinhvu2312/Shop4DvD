using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace s4dServer.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Subtotal { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
