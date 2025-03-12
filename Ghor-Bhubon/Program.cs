using Ghor_Bhubon.Controllers;
using Ghor_Bhubon.Data;
using Ghor_Bhubon.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

builder.Services.AddSignalR(); // Add SignalR for real-time communication

// Enable session management
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register PropertyService for DI (dependency injection)
builder.Services.AddScoped<PropertyService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userController = new UserController(dbContext);
    userController.SeedAdminUser();
}

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Mapping SignalR Hubs

app.MapHub<PendingPostHub>("/pendingPostHub");
app.MapHub<PropertyHub>("/propertyHub");    

app.UseSession(); 

app.UseAuthorization();

// Default route mapping
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
