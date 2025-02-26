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
        public IActionResult AddProperty(Flat flat, string uploadedImages)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserID");

                if (userId.HasValue)
                {
                    flat.UserID = userId.Value;

                    // Store multiple file paths (sent from JavaScript via hidden input)
                    flat.ImagePaths = uploadedImages;
                    Console.WriteLine("Stored Image Paths: " + flat.ImagePaths); // Debugging

                    _context.Flats.Add(flat);
                    _context.SaveChanges();

                    return RedirectToAction("Dashboard");
                }

                ModelState.AddModelError("", "User not found.");
            }

            return View(flat);
        }

        [HttpPost]
        public IActionResult AddSingleImage(IFormFile Image)
        {
            if (Image != null && Image.Length > 0)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                // Ensure the upload folder exists
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Generate a unique file name
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Image.FileName);
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                // Save the image file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(fileStream);
                }

                // Return JSON response with the image path
                return Json(new { success = true, imagePath = "/uploads/" + uniqueFileName });
            }

            return Json(new { success = false, error = "Invalid file upload" });
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


        public IActionResult EditProperty(int id)
        {
            var flat = _context.Flats.FirstOrDefault(f => f.FlatID == id);

            if (flat == null)
            {
                return NotFound();
            }

            return View(flat); // Load EditProperty.cshtml with data
        }


        [HttpPost]
        public IActionResult SaveEditedProperty(Flat flat, List<IFormFile> NewImages, List<string> deleteImages)
        {
            var existingFlat = _context.Flats.FirstOrDefault(f => f.FlatID == flat.FlatID);

            if (existingFlat != null)
            {
                existingFlat.Location = flat.Location;
                existingFlat.Rent = flat.Rent;
                existingFlat.NumberOfRooms = flat.NumberOfRooms;
                existingFlat.NumberOfBathrooms = flat.NumberOfBathrooms;
                existingFlat.Description = flat.Description;

                // Process image deletions
                if (deleteImages != null && deleteImages.Count > 0)
                {
                    var imageList = existingFlat.ImagePaths.Split(",").ToList();
                    foreach (var img in deleteImages)
                    {
                        imageList.Remove(img);
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, img.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    existingFlat.ImagePaths = string.Join(",", imageList);
                }

                // Process new images
                if (NewImages != null && NewImages.Count > 0)
                {
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    List<string> uploadedPaths = existingFlat.ImagePaths.Split(",").ToList();
                    foreach (var image in NewImages)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(image.FileName);
                        string filePath = Path.Combine(uploadFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(fileStream);
                        }

                        uploadedPaths.Add("/uploads/" + uniqueFileName);
                    }

                    existingFlat.ImagePaths = string.Join(",", uploadedPaths);
                }

                _context.SaveChanges();
                return RedirectToAction("PropertyDetails", new { id = flat.FlatID }); // Redirect to details page
            }

            return View(flat);
        }


    }
}
