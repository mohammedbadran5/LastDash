using LastDash.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure the database context with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    // Use a custom error page for production
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Enable HSTS for enhanced security
}

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();
app.UseStaticFiles(); // Enable serving static files

app.UseRouting(); // Enable routing capabilities

app.UseAuthorization(); // Enable authorization middleware

// Define the default route for the application
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});





app.Run(); // Start the application
