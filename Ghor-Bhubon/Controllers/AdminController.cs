using Ghor_Bhubon.Data;
using Ghor_Bhubon.Models;
using Microsoft.AspNetCore.Hosting;
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
            /*var totalHomesRented = await _context.Flats.CountAsync(h => h.IsRented);
            var totalTransactions = await _context.Transactions.SumAsync(t => t.Amount);
            var totalRevenue = await _context.Transactions.Where(t => t.IsAdminFee).SumAsync(t => t.Amount);

            var newUsersThisMonth = await _context.Users.CountAsync(u => u.CreatedAt.Month == currentMonth && u.CreatedAt.Year == currentYear);
            var newPostsThisMonth = await _context.Posts.CountAsync(p => p.CreatedAt.Month == currentMonth && p.CreatedAt.Year == currentYear);
            var housesRentedThisMonth = await _context.Homes.CountAsync(h => h.RentedAt.Month == currentMonth && h.RentedAt.Year == currentYear);
            var transactionsThisMonth = await _context.Transactions.Where(t => t.CreatedAt.Month == currentMonth && t.CreatedAt.Year == currentYear).SumAsync(t => t.Amount);
            var revenueThisMonth = await _context.Transactions.Where(t => t.IsAdminFee && t.CreatedAt.Month == currentMonth && t.CreatedAt.Year == currentYear).SumAsync(t => t.Amount);*/

            var model = new AdminDashboardViewModel
            {
                TotalUsers = totalUsers,
                TotalLandlords = totalLandlords,
                TotalTenants = totalTenants,
                TotalPosts = totalPosts,
                /*TotalHomesRented = totalHomesRented,
                TotalTransactions = totalTransactions,
                TotalRevenue = totalRevenue,
                NewUsersThisMonth = newUsersThisMonth,
                NewPostsThisMonth = newPostsThisMonth,
                HousesRentedThisMonth = housesRentedThisMonth,
                TransactionsThisMonth = transactionsThisMonth,
                RevenueThisMonth = revenueThisMonth*/
            };

            return View(model);
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
            return View();
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
    }
}