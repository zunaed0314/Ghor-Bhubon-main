using Ghor_Bhubon.Data;
using Ghor_Bhubon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace Ghor_Bhubon.Controllers
{
    public class LandlordController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LandlordController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Dashboard()
        {
            var landlordId = HttpContext.Session.GetInt32("UserID");

            if (landlordId.HasValue)
            {
                var flats = _context.Flats.Where(f => f.UserID == landlordId.Value).ToList();
                return View(flats);
            }

            return RedirectToAction("Login", "User");
        }

        public IActionResult AddProperty()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProperty(Flat flat, List<IFormFile> Images)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserID");

                if (userId.HasValue)
                {
                    flat.UserID = userId.Value;

                    List<string> imagePaths = new List<string>();

                    if (Images != null && Images.Count > 0)
                    {
                        // List to store paths of all uploaded files
                        var filePaths = new List<string>();

                        string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                        // Ensure the upload folder exists
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        // Loop through each uploaded image
                        foreach (var image in Images)
                        {
                            if (image.Length > 0)
                            {
                                // Generate a unique file name for each image
                                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(image.FileName);
                                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                                // Save the image file to the server
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    image.CopyTo(fileStream);
                                }

                                // Add the relative file path to the list
                                filePaths.Add(Path.Combine("/uploads/", uniqueFileName));
                            }
                        }

                        // Join the file paths into a comma-separated string
                        flat.ImagePaths = string.Join(",", filePaths);
                        Console.WriteLine("Final Image Paths: " + flat.ImagePaths); // Debugging
                    }


                    // Add the flat property to the database and save changes
                    _context.Flats.Add(flat);
                    _context.SaveChanges();

                    return RedirectToAction("Dashboard");
                }

                ModelState.AddModelError("", "User not found.");
            }

            return View(flat);
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

        [HttpPost]
        public IActionResult DeleteProperty(int flatId)
        {
            var flat = _context.Flats.FirstOrDefault(f => f.FlatID == flatId);

            if (flat != null)
            {
                // Optional: If you want to delete the images related to the property, you can delete the files from the server.
                if (!string.IsNullOrEmpty(flat.ImagePaths))
                {
                    var imagePaths = flat.ImagePaths.Split(',');
                    foreach (var imagePath in imagePaths)
                    {
                        var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                }

                // Remove the property from the database
                _context.Flats.Remove(flat);
                _context.SaveChanges();

                // Redirect to the Dashboard after deletion
                return RedirectToAction("Dashboard");
            }

            // If flat not found, return an error page or handle appropriately
            return NotFound();
        }

    }
}
