using System.ComponentModel.DataAnnotations;

namespace LastDash.Models
{
    public class Contact
    {
        public int Id { get; set; } // Primary key

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
