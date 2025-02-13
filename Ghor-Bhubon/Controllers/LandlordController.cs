// Controllers/LandlordController.cs
using Ghor_Bhubon.Data;
using Ghor_Bhubon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace Ghor_Bhubon.Controllers
{
    public class LandlordController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LandlordController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public IActionResult Dashboard()
        {
            return View(); 
        }

       
        public IActionResult AddProperty()
        {
            return View();  
        }


        [HttpPost]
        public IActionResult AddProperty(Flat flat)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserID");

                if (userId.HasValue)
                {
                    flat.UserID = userId.Value;  

                    _context.Flats.Add(flat);  
                    _context.SaveChanges(); 

                    return RedirectToAction("Dashboard");  
                }

                ModelState.AddModelError("", "User not found.");  
            }
            else
            {
                
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            return View(flat); 
        }




        public IActionResult Logout()
        {
            HttpContext.Session.Clear();  
            return RedirectToAction("Index", "Home");  
        }
    }
}
