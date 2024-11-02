using LastDash.Data;
using LastDash.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using LastDash.ViewModels;

public class PostsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PostsController(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<IActionResult> Home()
    {
        var posts = await _context.Posts.ToListAsync(); // Get all posts
        var categories = new List<string> { "News", "Politics", "Opinion", "Sport", "Entertainment" };

        var model = new HomePageViewModel
        {
            Categories = categories,
            Posts = posts.GroupBy(p => p.Category).ToDictionary(g => g.Key, g => g.ToList())
        };

        return View(model); // Pass the model to the view
    }



    [HttpGet("Home/Details/{id}")]
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

        return View(viewModel); // This should pass a PostDetailsViewModel to the view
    }


    public IActionResult EditContact(int id)
    {
        var contact = _context.Contacts.Find(id); // Retrieve the contact by ID
        if (contact == null)
        {
            return NotFound();
        }
        return View(contact); // Pass contact to the edit view
    }

    [HttpPost]
    public IActionResult EditContact(Contact contact)
    {
        if (ModelState.IsValid)
        {
            _context.Contacts.Update(contact);
            _context.SaveChanges();
            return RedirectToAction("Contact"); // Return to the main list after editing
        }
        return View(contact);
    }


    [HttpPost]
    public IActionResult DeleteContact(int id)
    {
        var contact = _context.Contacts.Find(id);
        if (contact != null)
        {
            _context.Contacts.Remove(contact);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }



    // GET: Posts/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Post post)
    {
        if (!ModelState.IsValid)
        {
            return View(post); // Return the view with validation errors
        }

        // Handle the file upload
        if (post.Image != null && post.Image.Length > 0)
        {
            // Save the image and get the path
            post.ImagePath = await SaveImage(post.Image);
        }

        // Add the post to the context
        _context.Posts.Add(post);

        // Save changes to the database
        await _context.SaveChangesAsync();

        // Redirect to the Index action
        return RedirectToAction(nameof(Index));
    }

    private async Task<string> SaveImage(IFormFile imageFile)
    {
        var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
        if (!Directory.Exists(imagesDirectory))
        {
            Directory.CreateDirectory(imagesDirectory);
        }

        var filePath = Path.Combine(imagesDirectory, imageFile.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        return $"/images/{imageFile.FileName}";
    }

    // GET: Posts/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        return View(post);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Post post)
    {
        if (id != post.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Save the new image or keep the existing one
                post.ImagePath = await SaveImage(post.Image, post.ImagePath);
                _context.Update(post);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Consider logging the exception here
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(post);
    }







    // GET: Posts/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        return View(Index);
    }





    // POST: Posts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }




    // In your PostsController
[HttpGet]
public JsonResult SearchPostTitles(string query)
{
    // Assume you have a list of posts in your database
    var matchingTitles = _context.Posts
        .Where(p => p.Title.Contains(query))
        .Select(p => p.Title)
        .Take(5) // Limit suggestions to 5
        .ToList();

    return Json(matchingTitles);
}



    public async Task<IActionResult> EditComment(int id)
    {
        var comment = await _context.UserComments
            .Include(c => c.Post) // Include the Post entity
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
        {
            return NotFound();
        }

        ViewBag.Posts = await _context.Posts.ToListAsync(); // Pass the posts to the view
        return View(comment);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditComment(int id, UserComments comment)
    {
        if (id != comment.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(comment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(comment.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Consider logging the exception here
                }
            }
            return RedirectToAction(nameof(Comments));
        }
        return View(comment);
    }





    // Check if the comment exists
    private bool CommentExists(int id)
    {
        return _context.UserComments.Any(e => e.Id == id);
    }

    // GET: Comments/Delete/5
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _context.UserComments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }
        return View(Comments); // Pass the comment to the delete view
    }

    // POST: Comments/Delete/5
    [HttpPost, ActionName("DeleteComment")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed2(int id)
    {
        var comment = await _context.UserComments.FindAsync(id);
        if (comment != null)
        {
            _context.UserComments.Remove(comment);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Comments));
    }




    // Check if a post exists
    private bool PostExists(int id)
    {
        return _context.Posts.Any(e => e.Id == id);
    }




    // GET: Posts
    public async Task<IActionResult> Index()
    {
        var posts = await _context.Posts
            .Include(p => p.Comments) // Include comments to avoid lazy loading
            .ToListAsync();

        return View(posts); // Pass the list of posts to the view
    }



    public async Task<IActionResult> Comments()
    {
        var comments = await _context.UserComments
            .Include(c => c.Post) // Include the Post entity
            .ToListAsync();
        return View(comments); // Pass the list of comments with posts to the view
    }


    public async Task<IActionResult> Contact()
    {
        var contacts = await _context.Contacts.ToListAsync(); // Retrieve all contacts from the database
        return View(contacts); // Pass the list of contacts to the view
    }



    private async Task<string?> SaveImage(IFormFile? imageFile, string? existingImagePath = null)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(imagesDirectory))
            {
                Directory.CreateDirectory(imagesDirectory);
            }

            var filePath = Path.Combine(imagesDirectory, imageFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/images/{imageFile.FileName}";
        }
        return existingImagePath; // Return existing path if no new image is uploaded
    }




    public async Task<IActionResult> PostsByCategory()
    {
        var posts = await _context.Posts.ToListAsync();
        var groupedPosts = posts
            .GroupBy(p => p.Category)
            .Select(g => new
            {
                Category = g.Key,
                Posts = g.Take(3).ToList() // Take the top 3 posts from each category
            })
            .ToList();

        return View(groupedPosts);
    }


   



}
