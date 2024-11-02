using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LastDash.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string? ImagePath { get; set; }

        [NotMapped] // Ignore this property for EF Core
        public IFormFile? Image { get; set; }

        [Required]
        public string Category { get; set; }

        // Navigation property for comments
        public ICollection<UserComments> Comments { get; set; } = new List<UserComments>(); // Initialize the collection
        public int CommentsCount { get; set; } // Property to hold the comment count
    }
}