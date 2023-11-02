using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace s4dServer.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage ="Username is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage ="Username must be between 6 and 100 characters")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 255 characters")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string? Role { get; set; }

        [Range(0, ulong.MaxValue, ErrorMessage = "Invalid Status value")]
        public ulong Status { get; set; }

        [StringLength(255, ErrorMessage = "Address must not exceed 255 characters")]
        public string? Address { get; set; }

        [StringLength(20, ErrorMessage = "Phone number must not exceed 20 characters")]
        public string? PhoneNumber { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        [IgnoreDataMember]
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
