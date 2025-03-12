using Ghor_Bhubon.Data;
using Ghor_Bhubon.Models;
using Ghor_Bhubon.Hubs;  // Add this for the SignalR Hub
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;  // Add this for SignalR
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ghor_Bhubon.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PropertyService _propertyService;
        private readonly IHubContext<PendingPostHub> _hubContext;  // Inject the IHubContext

        // Modify the constructor to accept IHubContext
        public AdminController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, PropertyService propertyService, IHubContext<PendingPostHub> hubContext)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _propertyService = propertyService;
            _hubContext = hubContext;  // Assign the hub context
        }

        public async Task<IActionResult> AdminDashBoard()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var totalUsers = await _context.Users.CountAsync();
            var totalLandlords = await _context.Users.CountAsync(u => u.Role == UserRole.Landlord);
            var totalTenants = await _context.Users.CountAsync(u => u.Role == UserRole.Tenant);
            var totalPosts = await _context.Flats.CountAsync();
            var totalPending = await _context.PropertyPending.CountAsync();
            var totalHomesRented = await _context.Flats.CountAsync(h => h.Availability == "Available");

            var newUsersThisMonth = await _context.Users.CountAsync(u => u.CreatedAt.Month == currentMonth && u.CreatedAt.Year == currentYear);

            var model = new AdminDashboardViewModel
            {
                TotalUsers = totalUsers,
                TotalLandlords = totalLandlords,
                TotalTenants = totalTenants,
                TotalPosts = totalPosts,
                TotalPending = totalPending,
                NewUsersThisMonth = newUsersThisMonth,
            };

            return View(model);
        }

        public IActionResult PropertyPendingList()
        {
            var pendingProperties = _context.PropertyPending.ToList();
            return View(pendingProperties); // You can return a View that displays this list
        }

        public async Task<IActionResult> Approve(int id)
        {
            var pendingProperty = await _context.PropertyPending
                                                 .Where(p => p.ID == id)
                                                 .FirstOrDefaultAsync();

            if (pendingProperty == null)
            {
                return NotFound();
            }

            var newFlat = new Flat
            {
                UserID = pendingProperty.UserID,
                Rent = pendingProperty.Rent,
                Location = pendingProperty.Location,
                Description = pendingProperty.Description,
                NumberOfRooms = pendingProperty.NumberOfRooms,
                NumberOfBathrooms = pendingProperty.NumberOfBathrooms,
                Availability = "Available",
                ImagePaths = pendingProperty.ImagePaths,
                PdfPath = pendingProperty.PdfFilePath,
                Latitude = pendingProperty.Latitude,
                Longitude = pendingProperty.Longitude,
                City = pendingProperty.City,
                Area = pendingProperty.Area,
                AvailableFrom = pendingProperty.AvailableFrom
            };

            _context.Flats.Add(newFlat);
            await _context.SaveChangesAsync();

            _context.PropertyPending.Remove(pendingProperty);
            await _context.SaveChangesAsync();

            // Notify all clients that a property has been approved
            var totalPending = await _context.PropertyPending.CountAsync();
            await _hubContext.Clients.All.SendAsync("ReceivePendingPostUpdate", totalPending);

            return RedirectToAction(nameof(PendingPosts));
        }

        public async Task<IActionResult> Decline(int id)
        {
            var pendingProperty = await _context.PropertyPending
                                                 .Where(p => p.ID == id)
                                                 .FirstOrDefaultAsync();

            if (pendingProperty == null)
            {
                return NotFound();
            }

            _context.PropertyPending.Remove(pendingProperty);
            await _context.SaveChangesAsync();

            // Notify all clients that a property has been declined
            var totalPending = await _context.PropertyPending.CountAsync();
            await _hubContext.Clients.All.SendAsync("ReceivePendingPostUpdate", totalPending);

            return RedirectToAction(nameof(PendingPosts));
        }

        public async Task<IActionResult> PropertyDetailsPending(int id)
        {
            var propertyPending = await _context.PropertyPending
                                                 .Where(p => p.ID == id)
                                                 .FirstOrDefaultAsync();

            if (propertyPending == null)
            {
                return NotFound();
            }

            return View(propertyPending);
        }

        public IActionResult Expenses()
        {
            return View();
        }

        public async Task<IActionResult> ViewLandlords()
        {
            var landlords = await _context.Users.Where(f => f.Role == UserRole.Landlord).ToListAsync();
            return View(landlords);
        }

        public async Task<IActionResult> ViewTenants()
        {
            var tenants = await _context.Users.Where(f => f.Role == UserRole.Tenant).ToListAsync();
            return View(tenants);
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult PendingPosts()
        {
            var pendingProperties = _context.PropertyPending.ToList();
            return View(pendingProperties);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == id);

            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found or has already been deleted.";
                return RedirectToAction("AdminDashBoard");
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteNow(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(user.Role == UserRole.Landlord ? "ViewLandlords" : "ViewTenants");
            }

            TempData["ErrorMessage"] = "User not found or has already been deleted.";
            return RedirectToAction("AdminDashBoard");
        }

        public IActionResult LandlordDashboard(int id)
        {
            var landlord = _context.Users.FirstOrDefault(u => u.UserID == id);
            if (landlord == null)
            {
                return NotFound();
            }

            var flats = _context.Flats.Where(f => f.UserID == id).ToList();
            return View("LandlordProperties", flats);
        }

        public IActionResult PropertyDetails(int id)
        {
            var flat = _context.Flats.FirstOrDefault(f => f.FlatID == id);
            if (flat == null)
            {
                return NotFound();
            }

            Console.WriteLine("Property Image Paths: " + flat.ImagePaths); // Debugging

            return View(flat);
        }
    }
}
