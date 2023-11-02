using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace s4dServer.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public virtual User? User { get; set; }

        public DateTime? OrderDate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
