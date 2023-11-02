using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace s4dServer.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime? Date { get; set; }

        public virtual Product? Product { get; set; }

        public virtual User? User { get; set; }
    }
}
