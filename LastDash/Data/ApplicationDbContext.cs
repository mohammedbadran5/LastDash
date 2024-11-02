using LastDash.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace LastDash.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Post> Posts { get; set; }

        public DbSet<UserComments> UserComments { get; set; }

        public DbSet<Contact> Contacts { get; set; } // Add DbSet for Contact model


    }
}