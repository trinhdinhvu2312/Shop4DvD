using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace s4dServer.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public string? CategoryName { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
