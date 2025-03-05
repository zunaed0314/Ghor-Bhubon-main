using Ghor_Bhubon.Data;
using Ghor_Bhubon.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ghor_Bhubon.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
            /*var totalTransactions = await _context.Transactions.SumAsync(t => t.Amount);*/
            /*var totalRevenue = await _context.Transactions.Where(t => t.IsAdminFee).SumAsync(t => t.Amount);*/

            var newUsersThisMonth = await _context.Users.CountAsync(u => u.CreatedAt.Month == currentMonth && u.CreatedAt.Year == currentYear);
            /*var newPostsThisMonth = await _context.Posts.CountAsync(p => p.CreatedAt.Month == currentMonth && p.CreatedAt.Year == currentYear);*/
            /*var housesRentedThisMonth = await _context.Homes.CountAsync(h => h.RentedAt.Month == currentMonth && h.RentedAt.Year == currentYear);*/
            /*var transactionsThisMonth = await _context.Transactions.Where(t => t.CreatedAt.Month == currentMonth && t.CreatedAt.Year == currentYear).SumAsync(t => t.Amount);*/
            /*var revenueThisMonth = await _context.Transactions.Where(t => t.IsAdminFee && t.CreatedAt.Month == currentMonth && t.CreatedAt.Year == currentYear).SumAsync(t => t.Amount);*/

            var model = new AdminDashboardViewModel
            {
                TotalUsers = totalUsers,
                TotalLandlords = totalLandlords,
                TotalTenants = totalTenants,
                TotalPosts = totalPosts,
                TotalPending = totalPending,
                /*TotalHomesRented = totalHomesRented,*/
                /*TotalTransactions = totalTransactions,*/
                /*TotalRevenue = totalRevenue,*/
                NewUsersThisMonth = newUsersThisMonth,
                /*NewPostsThisMonth = newPostsThisMonth,*/
                /*HousesRentedThisMonth = housesRentedThisMonth,*/
                /*TransactionsThisMonth = transactionsThisMonth,*/
                /*RevenueThisMonth = revenueThisMonth*/
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
            // Fetch the property details from PropertyPending table by ID
            var pendingProperty = await _context.PropertyPending
                                                 .Where(p => p.ID == id)
                                                 .FirstOrDefaultAsync();

            if (pendingProperty == null)
            {
                return NotFound();
            }

            // Create a new Flat object using the PropertyPending data
            var newFlat = new Flat
            {
                UserID = pendingProperty.UserID,
                Rent = pendingProperty.Rent,
                Location = pendingProperty.Location,
                Description = pendingProperty.Description,
                NumberOfRooms = pendingProperty.NumberOfRooms,
                NumberOfBathrooms = pendingProperty.NumberOfBathrooms,
                Availability = "Available",  // Set availability as Available when approved
                ImagePaths = pendingProperty.ImagePaths,
                PdfPath = pendingProperty.PdfFilePath,
                Latitude= pendingProperty.Latitude,
                Longitude= pendingProperty.Longitude,
            };

            // Add the new Flat to the Flats table
            _context.Flats.Add(newFlat);
            await _context.SaveChangesAsync();

            // Remove the corresponding record from PropertyPending table
            _context.PropertyPending.Remove(pendingProperty);
            await _context.SaveChangesAsync();

            // Redirect to the PropertyPending list page or other page after success
            return RedirectToAction(nameof(PropertyPendingList)); // Replace with the actual action to show the list
        }


        public async Task<IActionResult> Decline(int id)
        {
            // Fetch the property details from PropertyPending table by ID
            var pendingProperty = await _context.PropertyPending
                                                 .Where(p => p.ID == id)
                                                 .FirstOrDefaultAsync();

            if (pendingProperty == null)
            {
                return NotFound();  // If the property is not found, return 404
            }

            // Remove the PropertyPending entry from the database
            _context.PropertyPending.Remove(pendingProperty);
            await _context.SaveChangesAsync();

            // Redirect to the list of pending properties or other page after success
            return RedirectToAction(nameof(PendingPosts));  // Replace with the appropriate action
        }

        public async Task<IActionResult> PropertyDetailsPending(int id)
        {
            // Fetch the property details from PropertyPending by ID
            var propertyPending = await _context.PropertyPending
                                                 .Where(p => p.ID == id)
                                                 .FirstOrDefaultAsync();

            if (propertyPending == null)
            {
                return NotFound(); // Return 404 if the property is not found
            }

            // Return the PropertyPending details view
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

            // Fetch flats where UserID (landlord ID) matches the given id
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