using System.ComponentModel.DataAnnotations;

namespace LastDash.Models
{
    public class UserComments
    {
        public int Id { get; set; }

        // Make Content required
        [Required]
        public string Content { get; set; }

        // Make WrittenDate optional
        public DateTime? WrittenDate { get; set; } = DateTime.Now;


        // Foreign key for Post (make it optional)
        public int? PostId { get; set; } // Nullable foreign key

        // Navigation property (make it optional)
        public virtual Post? Post { get; set; } // Nullable navigation property
    }
}
