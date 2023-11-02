using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace s4dServer.Models
{
    public class Album
    {
        [Key]
        public int AlbumID { get; set; }

        [Required]
        public string? AlbumTitle { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [ForeignKey("Artist")]
        public int ArtistID { get; set; }

        public virtual Artist? Artist { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
