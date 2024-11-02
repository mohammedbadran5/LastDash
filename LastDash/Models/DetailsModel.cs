using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LastDash.Data; // Replace with your actual data context namespace
using LastDash.Models; // Replace with your actual model namespace
using System.Linq;

namespace LastDash.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context; // Replace with your actual DbContext

        public Post Post { get; set; } // This property holds the Post

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id) // Change to int since Id is an int
        {
            Post = _context.Posts.FirstOrDefault(p => p.Id == id);

            if (Post == null)
            {
                return NotFound(); // Handle case when post is not found
            }

            return Page();
        }
    }
}
