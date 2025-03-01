using Ghor_Bhubon.Data;
using Microsoft.AspNetCore.Http;
using Ghor_Bhubon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

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

        public IActionResult AdminDashBoard()
        {
            return View();
        }


        public IActionResult Expenses()
        {
            return View();
        }


        public async Task<IActionResult> ViewLandlords()
        {
            var item = await _context.Users.Where(f => f.Role == UserRole.Landlord).ToListAsync();
            return View(item);
        }

        public async Task<IActionResult> ViewTenants()
        {
            var item = await _context.Users.Where(f => f.Role == UserRole.Tenant).ToListAsync();
            return View(item);
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
                return RedirectToAction("AdminDashBoard"); // Redirect to a safe page
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



