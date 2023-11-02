using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace s4dServer.Models
{
    public class Artist
    {
        [Key]
        public int ArtistID { get; set; }

        [Required]
        public string? ArtistName { get; set; }

        public string? Description { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
    }
}
