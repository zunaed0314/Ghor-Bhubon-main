using Ghor_Bhubon.Data;
using Ghor_Bhubon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Ghor_Bhubon.Controllers
{
    public class TenantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TenantController(ApplicationDbContext context) => _context = context;


        public IActionResult TenantDashboard()
        {
            var flats = _context.Flats?.ToList() ?? new List<Flat>(); // Ensure it's never null
            return View(flats);
        }



        public IActionResult ViewProperty(int id)
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