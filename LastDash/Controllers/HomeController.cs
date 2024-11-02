using LastDash.Data;
using LastDash.Models;
using LastDash.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LastDash.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Fetch all posts
        private async Task<List<Post>> GetPostsAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<IActionResult> Index()
        {
            var posts = await GetPostsAsync();
            return View(posts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Advertise()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        // POST: Home/CreateContact
        [HttpPost]
        public async Task<IActionResult> CreateContact(Contact contact) // Ensure you have a Contact model
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(contact); // Assuming you have a Contacts DbSet in your context
                await _context.SaveChangesAsync();
                return RedirectToAction("Index"); // Redirect to a suitable page after saving
            }

            return View(contact); // If validation fails, return the same view with the entered data
        }

        public async Task<IActionResult> AllPosts()
        {
            var posts = await GetPostsAsync();
            return View(posts);
        }

        public async Task<IActionResult> News()
        {
            var posts = await GetPostsAsync();
            return View(posts);
        }

        public async Task<IActionResult> Sport()
        {
            var posts = await GetPostsAsync();
            return View(posts);
        }

        public async Task<IActionResult> Opinion()
        {
            var posts = await GetPostsAsync();
            return View(posts);
        }

        public async Task<IActionResult> Politics()
        {
            var posts = await GetPostsAsync();
            return View(posts);
        }

        public async Task<IActionResult> Entertainment()
        {
            var posts = await GetPostsAsync();
            return View(posts);
        }

        public IActionResult Comments()
        {
            // Fetch comments from the database
            var comments = _context.UserComments.ToList();

            // Check if comments is null or empty (for debugging)
            if (comments == null)
            {
                // Log or handle the null case
                // You can also return a view that handles no comments
            }

            return View(comments); // Pass the comments to the view
        }



        [HttpGet("Home/Details")]
        public async Task<IActionResult> Details(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var comments = await _context.UserComments
                .Where(c => c.PostId == id)
                .ToListAsync();

            var viewModel = new PostDetailsViewModel
            {
                Post = post,
                RelatedPosts = await _context.Posts.Where(p => p.Category == post.Category).ToListAsync(),
                Comments = comments
            };

            return View(viewModel); // Pass the view model to the view
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, string content)
        {
            if (ModelState.IsValid)
            {
                var comment = new UserComments
                {
                    Content = content,
                    PostId = postId,
                    WrittenDate = DateTime.Now
                };

                _context.UserComments.Add(comment);
                await _context.SaveChangesAsync();

                // Redirect back to the Details action with the postId
                return RedirectToAction("Details", "Home", new { id = postId });
            }

            // If the model state is not valid, return the same view with an error message
            return RedirectToAction("Details", "Home", new { id = postId });
        }



    }
}
