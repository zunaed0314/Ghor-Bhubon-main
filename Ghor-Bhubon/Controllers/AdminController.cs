using Ghor_Bhubon.Data;
using Ghor_Bhubon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;


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


        public async Task<IActionResult> ManageUsers()
        {
            var user = _context.Users.ToList();
            return View(user);
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult PendingPosts()
        {
            return View();
        }
    }
}



